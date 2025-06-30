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

        public listado_empleado()
        {
            this.usuarioBO = new UsuarioBO();
            this.rolBO = new RolBO();
            this.tipoDocumentoBO = new TipoDocumentoBO();
            usuariosParametros parametros = new usuariosParametros();
            parametros.estadoSpecified = false;
            parametros.idTipoUsuarioSpecified = false;
            parametros.idTipoDocumentoSpecified = false;
            this.listadoUsuarios = this.UsuarioBO.Listar(parametros);
        }

        protected List<object> ConfigurarListado(BindingList<usuariosDTO> lista)
        {
            var listaAdaptada = lista.Select(u => new
            {
                u.idUsuario,
                u.nombreComp,
                u.numeroDocumento,
                u.email,
                u.telefono,
                u.sueldo,
                u.cantidadReservacion,
                tipoDocumentoNombre = u.tipoDocumento != null ? u.tipoDocumento.nombre : "No especificado",
                rolNombre = u.rol != null ? u.rol.nombre : "No especificado",
                fechaContratacion = u.fechaContratacionSpecified ? u.fechaContratacion : (DateTime?)null,
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

                LinkButton lnkModificar = (LinkButton)e.Row.FindControl("lnkModificar");
                LinkButton btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");

                lnkModificar.Visible = estado;
                btnEliminar.Visible = estado;

                if (estado)
                {
                    btnEliminar.OnClientClick = $"return confirmarEliminacion({idUsuario}, '{hdnIdEliminar.ClientID}', '{btnEliminarUsuario.ClientID}');";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            dgvEmpleados.RowDataBound += dgv_RowDataBound;
            if (!IsPostBack)
            {
                var listaAdaptada = this.ConfigurarListado(ListadoUsuarios);

                dgvEmpleados.DataSource = listaAdaptada;
                dgvEmpleados.DataBind();
                CargarEstadosUsuario();
                CargarRoles();
                CargarTiposDocumento();
                CargarRolesModal();
                CargarTiposDocumentoEmpleadoModal();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                usuariosParametros parametros = new usuariosParametros();
                
                parametros.nombreCompleto = txtNombreCompFiltro.Text.Trim();
                
                parametros.estadoSpecified = !string.IsNullOrEmpty(ddlEstadoFiltro.SelectedValue);
                if (parametros.estadoSpecified)
                    parametros.estado = ddlEstadoFiltro.SelectedValue == "1";

                parametros.idTipoUsuarioSpecified = !string.IsNullOrEmpty(ddlRolFiltro.SelectedValue);
                if (parametros.idTipoUsuarioSpecified)
                    parametros.idTipoUsuario = int.Parse(ddlRolFiltro.SelectedValue);

                parametros.idTipoDocumentoSpecified = !string.IsNullOrEmpty(ddlTipoDocumentoFiltro.SelectedValue);
                if (parametros.idTipoDocumentoSpecified)
                    parametros.idTipoDocumento = int.Parse(ddlTipoDocumentoFiltro.SelectedValue);

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
                        MostrarResultado(exito, "Usuario", "eliminar");
                        if (exito) btnBuscar_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errorEliminar",
                        $"Swal.fire('Error', 'Error al eliminar el usuario: {ex.Message}', 'error');", true);
                }
            }
        }

        private void CargarRolesModal()
        {
            try
            {
                var roles = this.rolBO.Listar().Where(r => !r.esCliente).ToList(); // Solo roles no cliente para empleados
                this.CargarDropDownList(ddlRolModal, roles, "nombre", "idRol", "-- Seleccione --");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorRolesModal",
                    $"Swal.fire('Error', 'No se pudieron cargar los roles: {ex.Message}', 'error');", true);
            }
        }

        private void CargarTiposDocumentoEmpleadoModal()
        {
            try
            {
                var tiposDoc = this.tipoDocumentoBO.Listar();
                this.CargarDropDownList(ddlTipoDocumentoEmpleadoModal, tiposDoc, "nombre", "idTipoDocumento", "-- Seleccione --");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorTiposDocEmpleadoModal",
                    $"Swal.fire('Error', 'No se pudieron cargar los tipos de documento: {ex.Message}', 'error');", true);
            }
        }

        private void MostrarModalEmpleado(string modo, string titulo)
        {
            hdnModoModal.Value = modo;

            string script = "setTimeout(function() {" +
                            $"document.getElementById('tituloModalEmpleado').innerHTML = '<i class=\\\"fas fa-user me-2 text-danger\\\"></i>{titulo}';" +
                            "var modal = new bootstrap.Modal(document.getElementById('modalRegistrarEmpleado'));" +
                            "modal.show();" +
                            "}, 200);";

            ScriptManager.RegisterStartupScript(this, this.GetType(), $"mostrarModalEmpleado_{modo}", script, true);
        }

        private usuariosDTO ConstruirEmpleadoDTO(usuariosDTO empleado)
        {
            if (empleado == null)
                empleado = new usuariosDTO();

            empleado.nombreComp = txtNombreCompletoEmpleado.Text.Trim();
            empleado.numeroDocumento = txtNumeroDocumentoEmpleado.Text.Trim();
            empleado.email = txtEmailEmpleadoModal.Text.Trim();
            empleado.telefono = txtTelefonoEmpleadoModal.Text.Trim();
            empleado.contrasenha = txtContrasenaEmpleadoModal.Text.Trim();
            empleado.sueldo = double.Parse(txtSueldoModal.Text);
            empleado.sueldoSpecified = true;
            empleado.cantidadReservacion = int.Parse(txtCantReservasEmpleadoModal.Text);

            // Fecha de contratación
            if (DateTime.TryParse(txtFechaContratacionModal.Text, out DateTime fechaContratacion))
            {
                empleado.fechaContratacion = DateTime.SpecifyKind(fechaContratacion, DateTimeKind.Unspecified);
                empleado.fechaContratacionSpecified = true;
            }

            // Rol
            if (!string.IsNullOrEmpty(ddlRolModal.SelectedValue))
            {
                int idRol = int.Parse(ddlRolModal.SelectedValue);
                var rol = this.rolBO.ObtenerPorID(idRol);
                if (rol != null)
                {
                    empleado.rol = new SoftResBusiness.UsuarioWSClient.rolDTO
                    {
                        idRol = rol.idRol,
                        nombre = rol.nombre,
                        esCliente = rol.esCliente
                    };
                }
            }

            // Tipo de documento
            if (!string.IsNullOrEmpty(ddlTipoDocumentoEmpleadoModal.SelectedValue))
            {
                int idTipoDoc = int.Parse(ddlTipoDocumentoEmpleadoModal.SelectedValue);
                var tipoDoc = this.tipoDocumentoBO.ObtenerPorID(idTipoDoc);
                if (tipoDoc != null)
                {
                    empleado.tipoDocumento = new SoftResBusiness.UsuarioWSClient.tipoDocumentoDTO
                    {
                        idTipoDocumento = tipoDoc.idTipoDocumento,
                        nombre = tipoDoc.nombre
                    };
                }
            }

            empleado.estado = chkEstadoEmpleadoModal.Checked;
            empleado.estadoSpecified = true;

            return empleado;
        }

        private void LimpiarCamposModalEmpleado()
        {
            txtNombreCompletoEmpleado.Text = "";
            txtNumeroDocumentoEmpleado.Text = "";
            txtEmailEmpleadoModal.Text = "";
            txtTelefonoEmpleadoModal.Text = "";
            txtContrasenaEmpleadoModal.Text = "";
            txtConfirmPasswordEmpleadoModal.Text = "";
            txtSueldoModal.Text = "";
            txtCantReservasEmpleadoModal.Text = "0";
            txtFechaContratacionModal.Text = "";
            ddlRolModal.SelectedIndex = 0;
            ddlTipoDocumentoEmpleadoModal.SelectedIndex = 0;
            chkEstadoEmpleadoModal.Checked = true;
            hdnIdEmpleado.Value = "";
        }

        protected void btnNuevoEmpleado_Click(object sender, EventArgs e)
        {
            LimpiarCamposModalEmpleado();
            CargarRolesModal();
            CargarTiposDocumentoEmpleadoModal();
            MostrarModalEmpleado("registrar", "Registrar Empleado");
        }

        protected void btnModificarEmpleado_Command(object sender, CommandEventArgs e)
        {
            int idEmpleado = int.Parse(e.CommandArgument.ToString());
            if (idEmpleado > 0)
            {
                usuariosDTO empleado = this.usuarioBO.ObtenerPorID(idEmpleado);
                if (empleado != null)
                {
                    hdnIdEmpleado.Value = idEmpleado.ToString();
                    
                    txtNombreCompletoEmpleado.Text = empleado.nombreComp;
                    txtNumeroDocumentoEmpleado.Text = empleado.numeroDocumento;
                    txtEmailEmpleadoModal.Text = empleado.email;
                    txtTelefonoEmpleadoModal.Text = empleado.telefono;
                    txtSueldoModal.Text = empleado.sueldo.ToString();
                    txtCantReservasEmpleadoModal.Text = empleado.cantidadReservacion.ToString();
                    chkEstadoEmpleadoModal.Checked = empleado.estado;

                    if (empleado.fechaContratacionSpecified)
                    {
                        txtFechaContratacionModal.Text = empleado.fechaContratacion.ToString("yyyy-MM-dd");
                    }

                    if (empleado.rol != null)
                    {
                        ddlRolModal.SelectedValue = empleado.rol.idRol.ToString();
                    }

                    if (empleado.tipoDocumento != null)
                    {
                        ddlTipoDocumentoEmpleadoModal.SelectedValue = empleado.tipoDocumento.idTipoDocumento.ToString();
                    }

                    // Limpiar campos de contraseña en modo modificar
                    txtContrasenaEmpleadoModal.Text = "";
                    txtConfirmPasswordEmpleadoModal.Text = "";
                    
                    CargarRolesModal();
                    CargarTiposDocumentoEmpleadoModal();
                    MostrarModalEmpleado("modificar", "Modificar Empleado");
                }
            }
        }

        protected void btnGuardarEmpleado_Click(object sender, EventArgs e)
        {
            string modo = hdnModoModal.Value;
            bool exito = false;

            if (modo == "registrar")
            {
                usuariosDTO empleado = new usuariosDTO();
                empleado = ConstruirEmpleadoDTO(empleado);
                empleado.fechaCreacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                empleado.fechaCreacionSpecified = true;
                empleado.fechaModificacionSpecified = false;
                empleado.usuarioCreacion = "admin"; // usar Session["usuario"] si aplica

                int idEmpleado = this.usuarioBO.Insertar(empleado);
                if (idEmpleado > 0) exito = true;
            }
            else if (modo == "modificar")
            {
                if (string.IsNullOrEmpty(hdnIdEmpleado.Value) || !int.TryParse(hdnIdEmpleado.Value, out int id))
                {
                    MostrarResultado(false, "Empleado", modo);
                    return;
                }

                usuariosDTO empleado = this.usuarioBO.ObtenerPorID(id);
                empleado = ConstruirEmpleadoDTO(empleado);
                empleado.idUsuario = id;
                empleado.idUsuarioSpecified = true;

                empleado.fechaModificacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                empleado.fechaModificacionSpecified = true;
                empleado.usuarioModificacion = "admin"; // usar Session["usuario"] si aplica

                exito = this.usuarioBO.Modificar(empleado) > 0;
            }

            MostrarResultado(exito, "Empleado", modo);
            if (exito)
            {
                btnBuscar_Click(sender, e);
                LimpiarCamposModalEmpleado();
            }
        }
    }
}