using SoftResBusiness;
using SoftResBusiness.UsuarioWSClient;
using SoftResWA.Util;
using System;
using System.Collections.Generic;
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

        public Login_Home()
        {
            this.usuarioBO = new UsuarioBO();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogueado"] != null)
            {
                Response.Redirect("~/Views/Cliente/Home_Cliente.aspx"); // Ajusta esta ruta a la página principal del cliente logueado
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
                    if (rol == "cliente normal" || rol == "cliente vip")
                    {
                        Session["UsuarioLogueado"] = usuarioLogueado;
                        Response.Redirect("~/Views/Cliente/Home/Home_Cliente.aspx");
                    }
                    else
                    {
                        lblLoginError.Text = "Acceso no permitido para este tipo de usuario.";
                        lblLoginError.Visible = true;
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