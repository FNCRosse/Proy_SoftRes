using SoftResBusiness;
using SoftResBusiness.UsuarioWSClient;
using SoftResWA.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Login
{
    public partial class CambiarContrasena : System.Web.UI.Page
    {
        private UsuarioBO usuarioBO;

        public UsuarioBO UsuarioBO { get => usuarioBO; set => usuarioBO = value; }

        public CambiarContrasena()
        {
            this.usuarioBO = new UsuarioBO();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            if (!IsPostBack)
            {
                string tokenCifrado = Request.QueryString["token"];
                if (string.IsNullOrEmpty(tokenCifrado))
                {
                    MostrarAlerta("URL inválido", "No se proporcionó un correo válido.", "error");
                    btnCambiar.Enabled = false;
                }
                string dataDescifrada = Protector.Descifrar(tokenCifrado);

                if (string.IsNullOrEmpty(dataDescifrada))
                {
                    MostrarAlerta("URL inválido", "El enlace de recuperación ha sido alterado.", "error");
                    btnCambiar.Enabled = false;
                    return;
                }
                // Separamos el email y la marca de tiempo
                string[] partes = dataDescifrada.Split('|');
                if (partes.Length != 2)
                {
                    MostrarAlerta("URL inválido", "El formato del enlace es incorrecto.", "error");
                    btnCambiar.Enabled = false;
                    return;
                }

                string email = partes[0];
                long ticks = long.Parse(partes[1]);
                DateTime fechaCreacion = new DateTime(ticks);

                // Validamos que el enlace no tenga más de 60 minutos de antigüedad
                if (DateTime.UtcNow > fechaCreacion.AddMinutes(60))
                {
                    MostrarAlerta("URL Expirado", "El enlace de recuperación ha caducado. Por favor, solicita uno nuevo.", "error");
                    btnCambiar.Enabled = false;
                    return;
                }

                // ¡Éxito! Guardamos el email para usarlo al hacer clic en el botón.
                ViewState["EmailValido"] = email;
                // Opcional: mostrar al usuario para quién está cambiando la contraseña.
                //lblUsuario.Text = $"Cambiando contraseña para: {email}";
            }
        }
        protected void btnCambiar_Click(object sender, EventArgs e)
        {
            string email = ViewState["EmailValido"] as string;

            if (string.IsNullOrEmpty(email))
            {
                MostrarAlerta("Error", "La sesión ha expirado. Por favor, solicita un nuevo enlace.", "error");
                return;
            }
            string nueva = txtNuevaContrasena.Text.Trim();
            string confirmar = txtConfirmarContrasena.Text.Trim();

            if (nueva != confirmar)
            {
                MostrarAlerta("Error", "Las contraseñas no coinciden 🔁", "warning");
                return;
            }

            // Buscar al usuario por email
            usuariosDTO usuario = ObtenerPorEmail(email);

            if (usuario == null)
            {
                MostrarAlerta("No encontrado", "El correo no está registrado 📭", "error");
                return;
            }

            usuario.contrasenha = nueva;
            usuario.fechaModificacion = DateTime.Now;
            usuario.fechaModificacionSpecified = true;
            usuario.usuarioModificacion = "self-reset";

            bool exito = usuarioBO.CambiarContraseña(usuario) > 0;
            if (exito)
            {
                MostrarAlerta("¡Éxito! 🎉", "Tu contraseña ha sido actualizada.", "success");
            }
            else
            {
                MostrarAlerta("Ups...", "Hubo un problema al actualizar tu contraseña.", "error");
            }
        }

        private void MostrarAlerta(string titulo, string mensaje, string tipo)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "swal",
                $"Swal.fire('{titulo}', '{mensaje}', '{tipo}');", true);
        }
        public usuariosDTO ObtenerPorEmail(string email)
        {
            usuariosParametros uParametros = new usuariosParametros();
            uParametros.estado = true;
            uParametros.estadoSpecified = true;
            var lista = this.usuarioBO.Listar(uParametros); 
            return lista.FirstOrDefault(u => u.email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }
}