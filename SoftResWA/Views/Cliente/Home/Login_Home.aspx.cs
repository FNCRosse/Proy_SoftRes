using SoftResBusiness;
using SoftResBusiness.UsuarioWSClient;
using SoftResWA.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SoftResWA.Util.ServicioCorreo;

namespace SoftResWA.Views.Cliente.Home
{
    public partial class Login_Home : System.Web.UI.Page
    {
        private UsuarioBO usuarioBO;
        private TipoDocumentoBO tipoDocumentoBO;

        public Login_Home()
        {
            this.usuarioBO = new UsuarioBO();
            this.tipoDocumentoBO = new TipoDocumentoBO();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] != null)
            {
                Response.Redirect("~/Views/Cliente/Home_Cliente.aspx"); // Ajusta esta ruta a la página principal del cliente logueado
            }
            if (!IsPostBack)
            {
                CargarTiposDocumento();
            }
        }
        private void CargarTiposDocumento()
        {
            try
            {
                BindingList<SoftResBusiness.TipoDocumentoWSClient.tipoDocumentoDTO> tipos = this.tipoDocumentoBO.Listar();
                ddlRegTipoDoc.DataSource = tipos;
                ddlRegTipoDoc.DataTextField = "nombre";
                ddlRegTipoDoc.DataValueField = "idTipoDocumento";
                ddlRegTipoDoc.DataBind();
                ddlRegTipoDoc.Items.Insert(0, new ListItem("Seleccione tipo de documento", ""));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al cargar tipos de documento: " + ex.Message);
                ddlRegTipoDoc.Items.Insert(0, new ListItem("Error al cargar", "0"));
            }
        }
        protected void btnCrearCuenta_Click(object sender, EventArgs e)
        {
            Page.Validate("RegistroGroup");
            if (!Page.IsValid || !chkTerminos.Checked)
            {
                if (!chkTerminos.Checked)
                {
                    cvTerminos.IsValid = false;
                }
                return;
            }

            try
            {
                if (usuarioBO.ValidarDocumentoUnico(txtRegNumDoc.Text.Trim()))
                {
                    MostrarAlerta("Error de Registro", "El número de documento ya está registrado.", "error");
                    return;
                }
                if (usuarioBO.ValidarEmailUnico(txtRegEmail.Text.Trim()))
                {
                    MostrarAlerta("Error de Registro", "El email ya está registrado.", "error");
                    return;
                }

                var nuevoUsuario = new usuariosDTO();
                nuevoUsuario.nombreComp = txtRegNombre.Text.Trim();
                nuevoUsuario.email = txtRegEmail.Text.Trim();
                nuevoUsuario.numeroDocumento = txtRegNumDoc.Text.Trim();
                nuevoUsuario.telefono = txtRegNumDoc.Text.Trim();
                nuevoUsuario.contrasenha = txtRegPassword.Text.Trim();
                nuevoUsuario.rol = new SoftResBusiness.UsuarioWSClient.rolDTO();
                nuevoUsuario.rol.idRolSpecified = true;
                nuevoUsuario.rol.idRol = 3;
                nuevoUsuario.tipoDocumento = new SoftResBusiness.UsuarioWSClient.tipoDocumentoDTO();
                nuevoUsuario.tipoDocumento.idTipoDocumento = int.Parse(ddlRegTipoDoc.SelectedValue);
                nuevoUsuario.tipoDocumento.idTipoDocumentoSpecified = true;
                nuevoUsuario.estado = true;
                nuevoUsuario.estadoSpecified = true;
                nuevoUsuario.fechaCreacion=DateTime.Now;
                nuevoUsuario.fechaCreacionSpecified = true;
                nuevoUsuario.usuarioCreacion = txtRegNombre.Text.Trim();

                int resultado = usuarioBO.Insertar(nuevoUsuario);

                if (resultado > 0)
                {
                    // Mostrar mensaje de éxito y quizá loguearlo automáticamente
                    string script = @"Swal.fire({
                                    title: '¡Registro Exitoso!',
                                    text: 'Tu cuenta ha sido creada. Ahora puedes iniciar sesión.',
                                    icon: 'success'
                                }).then(function() {
                                    // Cierra el modal de registro y abre el de login
                                    var modalRegistro = bootstrap.Modal.getInstance(document.getElementById('modalRegistro'));
                                    if(modalRegistro) modalRegistro.hide();
                                    var modalLogin = new bootstrap.Modal(document.getElementById('modalLogin'));
                                    modalLogin.show();
                                });";
                    ScriptManager.RegisterStartupScript(this, GetType(), "registroExito", script, true);
                }
                else
                {
                    MostrarAlerta("Error", "No se pudo crear tu cuenta. Por favor, inténtalo de nuevo.", "error");
                }
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error Inesperado", "Ocurrió un problema durante el registro.", "error");
                System.Diagnostics.Debug.WriteLine("Error en registro de cliente: " + ex.Message);
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var credenciales = new credencialesDTO();
                credenciales.email = txtLoginEmail.Text.Trim();
                credenciales.contrasenha = txtLoginPassword.Text;

                usuariosDTO usuarioLogueado = usuarioBO.login(credenciales);

                if (usuarioLogueado != null && !string.IsNullOrEmpty(usuarioLogueado.rol.nombre))
                {
                    string rol = usuarioLogueado.rol.nombre.ToLower();
                    Session["UsuarioLogueado"] = usuarioLogueado;
                    if (rol == "cliente normal" || rol == "cliente vip")
                    {
                        Response.Redirect("~/Views/Cliente/Home/Home_Cliente.aspx");
                    }
                    else
                    {
                        Response.Redirect("~/Views/Home/home.aspx");
                    }
                }
                else
                {
                    lblLoginError.Text = "Correo o contraseña incorrectos.";
                    lblLoginError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblLoginError.Text = "Ocurrió un error en el servicio.";
                lblLoginError.Visible = true;
                System.Diagnostics.Debug.WriteLine("Error en Login Cliente: " + ex.Message);
            }
        }

        protected void btnEnviarRecuperacion_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(EnviarCorreoRecuperacionAsync));
        }

        private async System.Threading.Tasks.Task EnviarCorreoRecuperacionAsync()
        {
            string correo = txtRecuperarEmail.Text.Trim();

            if (string.IsNullOrEmpty(correo))
            {
                MostrarAlerta("Error", "Por favor, ingresa una dirección de correo.", "error");
                return;
            }
            string nombreUsuario = correo.Split('@')[0];

            string dataParaCifrar = $"{correo}|{DateTime.UtcNow.Ticks}";
            string tokenCifrado = Protector.Cifrar(dataParaCifrar); 
            string linkCambio = $"http://localhost:52960/Views/Login/CambiarContrasena.aspx?token={tokenCifrado}";

            RespuestaEnvioCorreo respuesta = await ServicioCorreo.EnviarCorreoRecuperacion(correo, nombreUsuario, linkCambio);

            if (respuesta.Exito)
            {
                MostrarAlerta("¡Correo Enviado!", "Si el correo está registrado, recibirás un enlace para restablecer tu contraseña.", "success");
            }
            else
            {
                MostrarAlerta("Error de Envío", "No se pudo enviar el correo en este momento. Inténtalo más tarde.", "error");
                System.Diagnostics.Debug.WriteLine($"Error al enviar correo de recuperación: {respuesta.MensajeError}");
            }
        }

        private void MostrarAlerta(string titulo, string mensaje, string tipo)
        {
            // Función auxiliar para no repetir código de ScriptManager
            string script = $"Swal.fire('{titulo}', '{Server.HtmlEncode(mensaje)}', '{tipo}');";
            ScriptManager.RegisterStartupScript(this, GetType(), $"alerta_{Guid.NewGuid()}", script, true);
        }
    }
}