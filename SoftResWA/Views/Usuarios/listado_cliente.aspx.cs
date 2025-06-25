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

        public listado_cliente()
        {
            this.usuarioBO = new UsuarioBO();
            this.rolBO = new RolBO();
            this.tipoDocumentoBO = new TipoDocumentoBO();

            // Cargar solo clientes (usuarios con rol de cliente)
            usuariosParametros parametros = new usuariosParametros();
            parametros.estadoSpecified = false;
            parametros.idTipoDocumentoSpecified = false;

            // Obtener ID del rol cliente
            try
            {
                var roles = this.rolBO.Listar();
                var rolCliente = roles.FirstOrDefault(r => r.esCliente);
                if (rolCliente != null)
                {
                    parametros.idTipoUsuarioSpecified = true;
                    parametros.idTipoUsuario = rolCliente.idRol;
                }
                else
                {
                    parametros.idTipoUsuarioSpecified = false;
                }
            }
            catch
            {
                parametros.idTipoUsuarioSpecified = false;
            }

            this.listadoClientes = this.UsuarioBO.Listar(parametros);
        }

        protected List<object> ConfigurarListado(BindingList<usuariosDTO> lista)
        {
            var listaAdaptada = lista.Select(u => new
            {
                u.idUsuario,
                u.nombreComp,
                u.idUsuarioSpecified,
                u.email,
                u.telefono,
                u.cantidadReservacion,
                tipoDocumento = Convert.ToInt32(u.numeroDocumento) > 0 ? ObtenerNombreTipoDocumento(Convert.ToInt32(u.numeroDocumento)) : "No especificado",
                rol = (Convert.ToInt32(u.rol)) > 0 ? ObtenerNombreRol(Convert.ToInt32(u.rol)) : "No especificado",
                u.fechaCreacion,
                u.usuarioCreacion,
                fechaModificacion = u.fechaModificacionSpecified ? u.fechaModificacion : (DateTime?)null,
                u.usuarioModificacion,
                u.estado,
                estadoTexto = u.estado ? "Activo" : "Inactivo",
                estadoBool = u.estado
            }).ToList<Object>();
            return listaAdaptada;
        }

        private string ObtenerNombreTipoDocumento(int tipoDocId)
        {
            try
            {
                SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO tipoDoc = this.tipoDocumentoBO.ObtenerPorID(tipoDocId);
                return tipoDoc?.nombre ?? "No encontrado";
            }
            catch
            {
                return "Error al cargar";
            }
        }

        private string ObtenerNombreRol(int rolId)
        {
            try
            {
                // Correct the namespace to use the appropriate rolDTO type
                SoftResBusiness.RolWSClient.rolDTO rol = this.rolBO.ObtenerPorID(rolId);
                return rol?.nombre ?? "No encontrado";
            }
            catch
            {
                return "Error al cargar";
            }
        }

        private void CargarDropDownList(DropDownList ddl, object dataSource, string textField, string valueField, string textoDefault)
        {
            ddl.DataSource = dataSource;
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(textoDefault, ""));
        }

        private void CargarEstadosUsuario()
        {
            var estadosUsuario = new List<object>
                {
                    new { nombre = "Activo", id = "1" },
                    new { nombre = "Inactivo", id = "0" }
                };

            this.CargarDropDownList(ddlEstadoFiltro, estadosUsuario, "nombre", "id", "Todos");
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
                bool estado = true; // Por defecto activo
                var prop = dataItem.GetType().GetProperty("estadoBool");
                if (prop != null)
                {
                    estado = (bool)prop.GetValue(dataItem);
                }

                int idUsuario = (int)dataItem.GetType().GetProperty("idUsuario")?.GetValue(dataItem);

                LinkButton btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");

                btnEliminar.Visible = estado;

                if (estado)
                {
                    btnEliminar.OnClientClick = $"return confirmarEliminacion({idUsuario}, '{hdnIdEliminar.ClientID}', '{btnEliminarCliente.ClientID}');";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            dgvClientes.RowDataBound += dgv_RowDataBound;
            if (!IsPostBack)
            {
                var listaAdaptada = this.ConfigurarListado(ListadoClientes);

                dgvClientes.DataSource = listaAdaptada;
                dgvClientes.DataBind();
                CargarEstadosUsuario();
                CargarTiposDocumento();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                usuariosParametros parametros = new usuariosParametros();

                parametros.nombreCompleto = txtNombreCompFiltro.Text.Trim();
                parametros.idTipoDocumento = Convert.ToInt32(txtNumeroDocFiltro.Text.Trim());

                parametros.estadoSpecified = !string.IsNullOrEmpty(ddlEstadoFiltro.SelectedValue);
                if (parametros.estadoSpecified)
                    parametros.estado = ddlEstadoFiltro.SelectedValue == "1";

                parametros.idTipoDocumentoSpecified = !string.IsNullOrEmpty(ddlTipoDocumentoFiltro.SelectedValue);
                if (parametros.idTipoDocumentoSpecified)
                    parametros.idTipoDocumento = int.Parse(ddlTipoDocumentoFiltro.SelectedValue);

                // Filtrar solo clientes
                try
                {
                    var roles = this.rolBO.Listar();
                    var rolCliente = roles.FirstOrDefault(r => r.esCliente);
                    if (rolCliente != null)
                    {
                        parametros.idTipoUsuarioSpecified = true;
                        parametros.idTipoUsuario = rolCliente.idRol;
                    }
                }
                catch
                {
                    parametros.idTipoUsuarioSpecified = false;
                }

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