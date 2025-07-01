using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftResBusiness;
using SoftResBusiness.UsuarioWSClient;
using SoftResWA.Util;
using static SoftResWA.Util.ServicioCorreo;

namespace SoftResWA.Views.Login
{
    public partial class Login : System.Web.UI.Page
    {
        UsuarioBO usuarioBO;

        public UsuarioBO UsuarioBO { get => usuarioBO; set => usuarioBO = value; }

        public Login()
        {
            this.usuarioBO = new UsuarioBO();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void BtnAcceder_Click(object sender, EventArgs e)
        {
            try
            {
                credencialesDTO credenciales = new credencialesDTO();   
                credenciales.contrasenha = txtContrasena.Text.Trim();
                credenciales.email = txtEmail.Text;
                usuariosDTO usuarioLogueado = this.usuarioBO.login(credenciales);
                if(usuarioLogueado != null && !string.IsNullOrEmpty(usuarioLogueado.rol.nombre))
                {
                    Session["UsuarioLogueado"] = usuarioLogueado;
                    string rol = usuarioLogueado.rol.nombre.ToLower();
                    if(rol == "cliente normal" || rol == "cliente vip")
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
                    lblMensajeError.Text = "Email o contraseña incorrectos.";
                    lblMensajeError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensajeError.Text = "Ocurrió un error en el servicio. Por favor, intente más tarde.";
                lblMensajeError.Visible = true;
            }
        }
        protected void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(EnviarCorreoYMostrarAlerta));
        }

        private async System.Threading.Tasks.Task EnviarCorreoYMostrarAlerta()
        {
            string correo = txtEmailRecuperacion.Text.Trim();
            string nombre = null;
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
    }
}