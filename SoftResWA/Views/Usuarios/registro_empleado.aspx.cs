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
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using SoftResWA.Util;
using static SoftResWA.Util.ServicioCorreo;

namespace SoftResWA.Views.Usuarios
{
    public partial class registro_empleado : System.Web.UI.Page
    {
        private UsuarioBO usuarioBO;
        private RolBO rolBO;
        private TipoDocumentoBO tipoDocumentoBO;
        private int idUsuario = 0;
        private bool esModificacion = false;

        public UsuarioBO UsuarioBO { get => usuarioBO; set => usuarioBO = value; }
        public RolBO RolBO { get => rolBO; set => rolBO = value; }
        public TipoDocumentoBO TipoDocumentoBO { get => tipoDocumentoBO; set => tipoDocumentoBO = value; }

        public registro_empleado()
        {
            this.usuarioBO = new UsuarioBO();
            this.rolBO = new RolBO();
            this.tipoDocumentoBO = new TipoDocumentoBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            if (!IsPostBack)
            {
                txtSueldo.Attributes["min"] = "1";
                CargarRoles();
                CargarTiposDocumento();

                // Verificar si es modificación
                if (Request.QueryString["id"] != null)
                {
                    if (int.TryParse(Request.QueryString["id"], out idUsuario))
                    {
                        esModificacion = true;
                        ViewState["IdUsuario"] = idUsuario;
                        ViewState["EsModificacion"] = true;
                        CargarDatosUsuario(idUsuario);
                        btnGuardar.Text = "Modificar";
                        lblTitulo.Text = "Modificar Empleado";
                        divContrasenas.Visible = false;
                        chkCambiarContrasena.Visible = true;
                        btnAbrirModalCambio.Visible = false;
                    }
                }
                else
                {
                    ViewState["EsModificacion"] = false;
                    lblTitulo.Text = "Registrar Empleado";
                    divContrasenas.Visible = true;
                    chkCambiarContrasena.Visible = false;
                    btnAbrirModalCambio.Visible = false;
                }
            }
            else
            {
                esModificacion = ViewState["EsModificacion"] != null && (bool)ViewState["EsModificacion"];
                if (esModificacion)
                {
                    idUsuario = ViewState["IdUsuario"] != null ? (int)ViewState["IdUsuario"] : 0;
                }
            }
        }
        protected void chkCambiarContrasena_CheckedChanged(object sender, EventArgs e)
        {
            btnAbrirModalCambio.Visible = chkCambiarContrasena.Checked;
        }
        
        protected void btnEnviarCorreoCambio_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(EnviarCorreoYMostrarAlerta));
        }

        private async System.Threading.Tasks.Task EnviarCorreoYMostrarAlerta()
        {
            string correo = txtEmail.Text.Trim();
            if (string.IsNullOrEmpty(correo))
            {
                string script = "Swal.fire('Error', 'El campo de correo electrónico del empleado debe estar lleno para enviar un enlace de recuperación.', 'error');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "correoVacioError", script, true);
                return; // Detener si no hay correo
            }
            string nombre = txtNombreCompleto.Text.Trim();
            if (string.IsNullOrEmpty(nombre))
            {
                nombre = correo.Split('@')[0];
            }
            string dataParaCifrar = $"{correo}|{DateTime.UtcNow.Ticks}";
            string tokenCifrado = Protector.Cifrar(dataParaCifrar);
            //Esto para el desplegado
            //var request = HttpContext.Current.Request;
            //string baseUrl = $"{request.Url.Scheme}://{request.Url.Authority}";
            //string rutaRelativa = ResolveUrl("~/Views/Login/CambiarContrasena.aspx");
            //string linkCambio = $"{baseUrl}{rutaRelativa}?token={token}";
            string linkCambio = $"http://localhost:52960/Views/Login/CambiarContrasena.aspx?token={tokenCifrado}"; // Para probar en el local
            RespuestaEnvioCorreo enviado = await ServicioCorreo.EnviarCorreoRecuperacion(correo, nombre, linkCambio);

            if (enviado.Exito)
            {
                string script = $"Swal.fire('¡Correo Enviado!', 'Se ha enviado un enlace de recuperación a {correo.Replace("'", "\\'")}', 'success');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "correoExito", script, true);
            }
            else
            {
                // También es buena idea manejar el caso de error
                string script = "Swal.fire('Error', 'No se pudo enviar el correo. Por favor, inténtalo de nuevo.', 'error');";
                System.Diagnostics.Debug.WriteLine($"Error al enviar correo de recuperación: {enviado.MensajeError}");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "correoError", script, true);
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

        private void CargarRoles()
        {
            try
            {
                var roles = this.rolBO.Listar().Where(r => !r.esCliente).ToList(); // Solo roles no cliente para empleados
                this.CargarDropDownList(ddlRol, roles, "nombre", "idRol", "Seleccionar...");
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
                this.CargarDropDownList(ddlTipoDocumento, tiposDoc, "nombre", "idTipoDocumento", "Seleccionar...");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorTiposDoc",
                    $"Swal.fire('Error', 'No se pudieron cargar los tipos de documento: {ex.Message}', 'error');", true);
            }
        }
        private void CargarDatosUsuario(int id)
        {
            try
            {
                usuariosDTO usuario = this.usuarioBO.ObtenerPorID(id);
                if (usuario != null)
                {
                    txtNombreCompleto.Text = usuario.nombreComp;
                    txtNumeroDocumento.Text = usuario.numeroDocumento;
                    txtEmail.Text = usuario.email;
                    txtTelefono.Text = usuario.telefono;
                    txtSueldo.Text = usuario.sueldo.ToString("F2");
                    txtContrasena.Text = usuario.contrasenha;
                    if (usuario.fechaContratacionSpecified)
                    {
                        txtFechaContratacion.Text = usuario.fechaContratacion.ToString("dd-MM-yyyy");
                    }

                    ddlTipoDocumento.SelectedValue = usuario.tipoDocumento.idTipoDocumento.ToString();
                    ddlRol.SelectedValue = usuario.rol.idRol.ToString();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorCargar",
                    $"Swal.fire('Error', 'Error al cargar los datos del usuario: {ex.Message}', 'error');", true);
            }
        }

        private usuariosDTO ConstruirDTO(usuariosDTO usuario)
        {
            if (usuario == null)
                usuario = new usuariosDTO();

            usuario.nombreComp = txtNombreCompleto.Text.Trim();
            usuario.numeroDocumento = txtNumeroDocumento.Text.Trim();
            usuario.email = txtEmail.Text.Trim();
            usuario.telefono = txtTelefono.Text.Trim();
            usuario.sueldo = double.Parse(txtSueldo.Text.Trim());
            usuario.sueldoSpecified = true;
            usuario.cantidadReservacion = 0;
            usuario.cantidadReservacionSpecified = true;
            usuario.estado = true;
            usuario.estadoSpecified = true;
            usuario.tipoDocumento = new SoftResBusiness.UsuarioWSClient.tipoDocumentoDTO();
            usuario.tipoDocumento.idTipoDocumento = int.Parse(ddlTipoDocumento.SelectedValue);
            usuario.tipoDocumento.idTipoDocumentoSpecified = true;
            usuario.rol = new SoftResBusiness.UsuarioWSClient.rolDTO();
            usuario.rol.idRol = int.Parse(ddlRol.SelectedValue);
            usuario.rol.idRolSpecified = true;
            if (!string.IsNullOrEmpty(txtFechaContratacion.Text))
            {
                if (DateTime.TryParseExact(txtFechaContratacion.Text, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fechaContratacion))
                {
                    usuario.fechaContratacion = DateTime.SpecifyKind(fechaContratacion, DateTimeKind.Unspecified);
                    usuario.fechaContratacionSpecified = true;
                }
            }

            return usuario;
        }

        private void MostrarResultado(bool exito, string entidad, string modo)
        {
            string accion = (modo == "modificar") ? "modificado" : "registrado";
            string accionNo = (modo == "modificar") ? "modificar" : "registrar";

            string baseMensaje = exito ? $"El {entidad} fue {accion}" : $"No se pudo {accionNo} el {entidad}";
            string tipo = exito ? "success" : "warning";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje",
                $"Swal.fire('¡{entidad} {(exito ? accion : $"NO {accion}")}!', '{baseMensaje} correctamente.', '{tipo}');", true);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                bool exito = false;
                string modo = esModificacion ? "modificar" : "registrar";

                if (esModificacion)
                {
                    // Modificar usuario existente
                    usuariosDTO usuario = this.usuarioBO.ObtenerPorID(idUsuario);
                    usuario = ConstruirDTO(usuario);
                    usuario.idUsuario = idUsuario;
                    usuario.idUsuarioSpecified = true;
                    usuario.fechaModificacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                    usuario.fechaModificacionSpecified = true;
                    usuario.usuarioModificacion = "admin"; // usar Session["usuario"] si aplica

                    exito = this.usuarioBO.Modificar(usuario) > 0;
                }
                else
                {
                    // Registrar nuevo usuario
                    string numeroDoc = txtNumeroDocumento.Text.Trim();
                    if (this.usuarioBO.ValidarDocumentoUnico(numeroDoc))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "docExistente",
                            "Swal.fire('Duplicado', 'Ya existe un cliente registrado con este número de documento.', 'warning');", true);
                        return;
                    }
                    usuariosDTO usuario = new usuariosDTO();
                    usuario = ConstruirDTO(usuario);
                    usuario.fechaCreacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                    usuario.fechaCreacionSpecified = true;
                    usuario.fechaModificacionSpecified = false;
                    usuario.contrasenha = txtContrasena.Text.Trim();
                    usuario.usuarioCreacion = "admin"; // usar Session["usuario"] si aplica

                    exito = this.usuarioBO.Insertar(usuario) > 0;
                }

                MostrarResultado(exito, "Empleado", modo);

                if (exito)
                {
                    // Redirigir al listado después de un registro/modificación exitoso
                    string script = "setTimeout(function() { window.location.href = 'listado_empleado.aspx'; }, 2000);";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", script, true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                    $"Swal.fire('Error', 'Error al procesar el empleado: {ex.Message}', 'error');", true);
            }
        }

        protected void btnCalendario_Click(object sender, EventArgs e)
        {
            calFechaContratacion.Visible = true;
        }

        protected void calFechaContratacion_SelectionChanged(object sender, EventArgs e)
        {
            txtFechaContratacion.Text = calFechaContratacion.SelectedDate.ToString("dd-MM-yyyy");
            calFechaContratacion.Visible = false;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("listado_empleado.aspx");
        }
    }
}