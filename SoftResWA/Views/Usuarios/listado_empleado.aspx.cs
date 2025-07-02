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
    public partial class listado_empleado : System.Web.UI.Page
    {
        private UsuarioBO usuarioBO;
        private RolBO rolBO;
        private TipoDocumentoBO tipoDocumentoBO;
        private BindingList<usuariosDTO> listadoUsuarios;

        public UsuarioBO UsuarioBO { get => usuarioBO; set => usuarioBO = value; }
        public RolBO RolBO { get => rolBO; set => rolBO = value; }
        public TipoDocumentoBO TipoDocumentoBO { get => tipoDocumentoBO; set => tipoDocumentoBO = value; }
        public BindingList<usuariosDTO> ListadoUsuarios { get => listadoUsuarios; set => listadoUsuarios = value; }

        //CONSTRUCTOR 
        public listado_empleado()
        {
            this.usuarioBO = new UsuarioBO();
            this.rolBO = new RolBO();
            this.tipoDocumentoBO = new TipoDocumentoBO();
            usuariosParametros parametros = new usuariosParametros();
            parametros.estadoSpecified = false;
            parametros.idTipoDocumentoSpecified = false;
            parametros.esClienteSpecified = true;
            parametros.esCliente = false;
            parametros.idTipoUsuarioSpecified = false;
            parametros.nombreCompleto = null;
            parametros.numDocumento = null;
            this.listadoUsuarios = this.UsuarioBO.Listar(parametros); 
        }

        //CONFIGURACION VISUAL DE LISTADO
        protected List<object> ConfigurarListado(BindingList<usuariosDTO> lista)
        {
            var listaAdaptada = lista.Select(u => new
            {
                u.idUsuario,
                tipoEmp = u.rol?.nombre ?? "No especificado",
                u.nombreComp,
                tipoDocumento = u.tipoDocumento?.nombre ?? "No especificado",
                u.numeroDocumento,
                u.email,
                u.telefono,
                u.sueldo,
                fechaContratacion = u.fechaContratacionSpecified ? u.fechaContratacion : (DateTime?)null,
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
        protected void dgv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem;

                // Obtener estado vía reflexión
                bool estado = true; // Por defecto activo
                var prop = dataItem.GetType().GetProperty("estadoBool");
                if (prop != null)
                {
                    estado = (bool)prop.GetValue(dataItem);
                }

                int idUsuario = (int)dataItem.GetType().GetProperty("idUsuario")?.GetValue(dataItem);

                HyperLink lnkModificar = (HyperLink)e.Row.FindControl("lnkModificar");
                LinkButton btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");

                lnkModificar.Visible = estado;
                btnEliminar.Visible = estado;

                if (estado)
                {
                    btnEliminar.OnClientClick = $"return confirmarEliminacion({idUsuario}, '{hdnIdEliminar.ClientID}', '{btnEliminarUsuario.ClientID}');";
                }
            }
        }
        private void MostrarResultado(bool exito, string entidad, string modo)
        {
            string accion = (modo == "modificar") ? "modificado" :
                            (modo == "eliminar") ? "eliminado" : "registrado";

            string accionNo = (modo == "modificar") ? "modificar" :
                              (modo == "eliminar") ? "eliminar" : "registrar";

            string baseMensaje = exito ? $"El {entidad} fue {accion}" : $"No se pudo {accionNo} el {entidad}";
            string tipo = exito ? "success" : "warning";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje",
                $"Swal.fire('¡{entidad} {(exito ? accion : $"NO {accion}")}!', '{baseMensaje} correctamente.', '{tipo}');", true);
        }

        //FUNCIONES DE EMPLEADOS
        private void CargarRoles()
        {
            try
            {
                var roles = this.rolBO.Listar().Where(r => !r.esCliente).ToList(); // Solo roles no cliente para empleados
                this.CargarDropDownList(ddlRolFiltro, roles, "nombre", "idRol", "Todos");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorRoles",
                    $"Swal.fire('Error', 'No se pudieron cargar los roles: {ex.Message}', 'error');", true);
            }
        }
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

        //PAGE_LOAD
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            dgvEmpleados.RowDataBound += dgv_RowDataBound;
            if (!IsPostBack)
            {
                var listaAdaptada = this.ConfigurarListado(ListadoUsuarios);

                dgvEmpleados.DataSource = listaAdaptada;
                dgvEmpleados.DataBind();
                CargarRoles();
                CargarTiposDocumento();
                // TODO: Implementar CargarRolesModal() y CargarTiposDocumentoEmpleadoModal() si son necesarios
            }
        }
        //BOTONES
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                usuariosParametros parametros = new usuariosParametros();
                parametros.nombreCompleto = txtNombreCompFiltro.Text.Trim();
                parametros.numDocumento = ""; // Sin filtro por documento en empleados
                parametros.idTipoDocumento = !string.IsNullOrEmpty(ddlTipoDocumentoFiltro.SelectedValue) ? int.Parse(ddlTipoDocumentoFiltro.SelectedValue) : 0;
                parametros.idTipoDocumentoSpecified = !string.IsNullOrEmpty(ddlTipoDocumentoFiltro.SelectedValue);
                parametros.idTipoUsuario = !string.IsNullOrEmpty(ddlRolFiltro.SelectedValue) ? int.Parse(ddlRolFiltro.SelectedValue) : 0;
                parametros.idTipoUsuarioSpecified = !string.IsNullOrEmpty(ddlRolFiltro.SelectedValue);
                parametros.estadoSpecified = !string.IsNullOrEmpty(ddlEstadoFiltro.SelectedValue);
                parametros.esCliente = false;
                parametros.esClienteSpecified = true;
                if (parametros.estadoSpecified)
                    parametros.estado = ddlEstadoFiltro.SelectedValue == "1";

                var lista = this.usuarioBO.Listar(parametros);
                var listaAdaptada = this.ConfigurarListado(lista);
                dgvEmpleados.DataSource = listaAdaptada;
                dgvEmpleados.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorBuscar",
                    $"Swal.fire('Error', 'Error en la búsqueda: {ex.Message}', 'error');", true);
            }
        }

        protected void btnEliminarUsuario_Click(object sender, EventArgs e)
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
                        MostrarResultado(exito, "Empleado", "eliminar");
                        if (exito) btnBuscar_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errorEliminar",
                        $"Swal.fire('Error', 'Error al eliminar el Empleado: {ex.Message}', 'error');", true);
                }
            }
        }
    }
}