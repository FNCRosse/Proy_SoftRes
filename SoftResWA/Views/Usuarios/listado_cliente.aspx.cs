using SoftResBusiness;
using SoftResBusiness.UsuarioWSClient;
using SoftResBusiness.RolWSClient;
using SoftResBusiness.TipoDocumentoWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Usuarios
{
    public partial class listado_cliente : System.Web.UI.Page
    {
        private UsuarioBO usuarioBO;
        private RolBO rolBO;
        private TipoDocumentoBO tipoDocumentoBO;
        private BindingList<usuariosDTO> listadoClientes;

        public UsuarioBO UsuarioBO { get => usuarioBO; set => usuarioBO = value; }
        public RolBO RolBO { get => rolBO; set => rolBO = value; }
        public TipoDocumentoBO TipoDocumentoBO { get => tipoDocumentoBO; set => tipoDocumentoBO = value; }
        public BindingList<usuariosDTO> ListadoClientes { get => listadoClientes; set => listadoClientes = value; }

        //CONSTRUCTOR
        public listado_cliente()
        {
            this.usuarioBO = new UsuarioBO();
            this.rolBO = new RolBO();
            this.tipoDocumentoBO = new TipoDocumentoBO();

            // Cargar solo clientes (usuarios con rol de cliente)
            usuariosParametros parametros = new usuariosParametros();
            parametros.estadoSpecified = false;
            parametros.idTipoDocumentoSpecified = false;
            parametros.esClienteSpecified = true;
            parametros.esCliente = true;
            parametros.idTipoUsuarioSpecified = false;
            parametros.nombreCompleto = null;
            parametros.numDocumento = null;

            this.listadoClientes = this.UsuarioBO.Listar(parametros);
        }

        //CONFIGURACION VISUAL DE LISTADO
        protected List<object> ConfigurarListado(BindingList<usuariosDTO> lista)
        {
            var listaAdaptada = lista.Select(u => new
            {
                u.idUsuario,
                tipoCliente = u.rol?.nombre?? "No especificado",
                u.nombreComp,
                tipoDocumento = u.tipoDocumento?.nombre ?? "No especificado",
                u.numeroDocumento,
                u.email,
                u.telefono,
                u.cantidadReservacion,
                u.fechaCreacion,
                u.usuarioCreacion,
                fechaModificacion = u.fechaModificacionSpecified ? u.fechaModificacion : (DateTime?)null,
                u.usuarioModificacion,
                estadoTexto = u.estado ? "Activo" : "Inactivo",
                estadoBool = u.estado
            }).ToList<Object>();
            return listaAdaptada;
        }

        //FUNCIONES GENERALES
        private void CargarDropDownList(DropDownList ddl, object dataSource, string textField, string valueField, string textoDefault)
        {
            ddl.DataSource = dataSource;
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(textoDefault, ""));
        }
        private void MostrarResultado(bool exito, string entidad, string modo)
        {
            string accion = (modo == "eliminar") ? "eliminado" : "procesado";
            string accionNo = (modo == "eliminar") ? "eliminar" : "procesar";

            string baseMensaje = exito ? $"El {entidad} fue {accion}" : $"No se pudo {accionNo} el {entidad}";
            string tipo = exito ? "success" : "warning";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje",
                $"Swal.fire('¡{entidad} {(exito ? accion : $"NO {accion}")}!', '{baseMensaje} correctamente.', '{tipo}');", true);
        }
        protected void dgv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem;

                // Obtener estado vía reflexión
                bool estado = false;
                var prop = dataItem.GetType().GetProperty("estadoBool");
                if (prop != null)
                {
                    estado = (bool)prop.GetValue(dataItem);
                }

                int idUsuario = (int)dataItem.GetType().GetProperty("idUsuario")?.GetValue(dataItem);

                LinkButton btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");
                HyperLink lnkModificar = (HyperLink)e.Row.FindControl("lnkModificar");

                btnEliminar.Visible = estado;
                lnkModificar.Visible = estado;
                if (estado)
                {
                    btnEliminar.OnClientClick = $"return confirmarEliminacion({idUsuario}, '{hdnIdEliminar.ClientID}', '{btnEliminarCliente.ClientID}');";
                }
            }
        }

        //FUNIONES DE USUARIO
        private void CargarTiposDocumento()
        {
            try
            {
                var tiposDoc = this.tipoDocumentoBO.Listar();
                this.CargarDropDownList(ddlTipoDocumentoFiltro, tiposDoc, "nombre", "idTipoDocumento", "Todos");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorTiposDoc",
                    $"Swal.fire('Error', 'No se pudieron cargar los tipos de documento: {ex.Message}', 'error');", true);
            }
        }
        private void CargarTiposCliente()
        {
            try
            {
                var roles = this.rolBO.Listar();
                var rolesCliente = roles.Where(r => r.esCliente == true).ToList();

                this.CargarDropDownList(ddlTipoClienteFiltro, rolesCliente, "nombre", "idRol", "Todos");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorTiposDoc",
                    $"Swal.fire('Error', 'No se pudieron cargar los tipos de documento: {ex.Message}', 'error');", true);
            }
        }

        //PAGE_LOAD
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            dgvClientes.RowDataBound += dgv_RowDataBound;
            if (!IsPostBack)
            {
                var listaAdaptada = this.ConfigurarListado(ListadoClientes);
                dgvClientes.DataSource = listaAdaptada;
                dgvClientes.DataBind();
                CargarTiposDocumento();
                CargarTiposCliente();
            }
        }

        //BOTONES   
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                usuariosParametros parametros = new usuariosParametros();

                parametros.nombreCompleto = txtNombreCompFiltro.Text.Trim();
                parametros.numDocumento = txtNumeroDocFiltro.Text.Trim();
                parametros.idTipoDocumento = !string.IsNullOrEmpty(ddlTipoDocumentoFiltro.SelectedValue) ? int.Parse(ddlTipoDocumentoFiltro.SelectedValue) : 0;
                parametros.idTipoDocumentoSpecified = !string.IsNullOrEmpty(ddlTipoDocumentoFiltro.SelectedValue);
                parametros.idTipoUsuario = !string.IsNullOrEmpty(ddlTipoClienteFiltro.SelectedValue) ? int.Parse(ddlTipoClienteFiltro.SelectedValue) : 0;
                parametros.idTipoUsuarioSpecified = !string.IsNullOrEmpty(ddlTipoClienteFiltro.SelectedValue);
                parametros.estadoSpecified = !string.IsNullOrEmpty(ddlEstadoFiltro.SelectedValue);
                parametros.esCliente = true;
                parametros.esClienteSpecified = true;
                if (parametros.estadoSpecified)
                    parametros.estado = ddlEstadoFiltro.SelectedValue == "1";

                var lista = this.usuarioBO.Listar(parametros);
                var listaAdaptada = this.ConfigurarListado(lista);
                dgvClientes.DataSource = listaAdaptada;
                dgvClientes.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorBuscar",
                    $"Swal.fire('Error', 'Error en la búsqueda: {ex.Message}', 'error');", true);
            }
        }

        protected void btnEliminarCliente_Click(object sender, EventArgs e)
        {
            int id = int.Parse(hdnIdEliminar.Value);
            if (id > 0)
            {
                try
                {
                    usuariosDTO usuario = this.usuarioBO.ObtenerPorID(id);
                    if (usuario != null)
                    {
                        usuario.idUsuario = id;
                        usuario.idUsuarioSpecified = true;
                        bool exito = this.usuarioBO.Eliminar(usuario) > 0;
                        MostrarResultado(exito, "Cliente", "eliminar");
                        if (exito) btnBuscar_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errorEliminar",
                        $"Swal.fire('Error', 'Error al eliminar el cliente: {ex.Message}', 'error');", true);
                }
            }
        }
    }
}