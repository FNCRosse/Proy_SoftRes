using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace SoftResWA.Util
{
    public static class ServicioCorreo
    {
        // Clase auxiliar para encapsular la respuesta del envío.
        public class RespuestaEnvioCorreo
        {
            public bool Exito { get; set; }
            public string MensajeError { get; set; }
        }

        public static async Task<RespuestaEnvioCorreo> EnviarCorreoRecuperacion(string correoDestino, string nombreUsuario, string urlCambio)
        {
            var apiKey = ConfigurationManager.AppSettings["BrevoApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                // Devolvemos un objeto de respuesta con el error.
                return new RespuestaEnvioCorreo
                {
                    Exito = false,
                    MensajeError = "La API Key de Brevo no está configurada en Web.config."
                };
            }

            using (var cliente = new HttpClient())
            {
                cliente.DefaultRequestHeaders.Add("api-key", apiKey);
                cliente.DefaultRequestHeaders.Add("Accept", "application/json");

                var contenido = new
                {
                    sender = new { name = "🍜 Restaurante Shifui Kay", email = "wosclb@gmail.com" },
                    to = new[] { new { email = correoDestino, name = nombreUsuario } },
                    subject = "🔐 ¡Recupera tu acceso a Shifui Kay!",
                    htmlContent = $@"
                        <div style='font-family: Arial, sans-serif; background-color: #fff3cd; padding: 20px; border-radius: 10px; color: #250505;'>
                            <h2 style='color: #bc1f1f;'>Hola {nombreUsuario} 👋</h2>
                            <p>Recibimos una solicitud para cambiar tu contraseña. Si fuiste tú, haz clic en el siguiente botón:</p>
                            <p style='text-align: center; margin: 20px 0;'>
                                <a href='{urlCambio}' style='background-color: #bc1f1f; color: #fff; padding: 12px 20px; text-decoration: none; border-radius: 8px;'>🔐 Cambiar Contraseña</a>
                            </p>
                            <p>Si no realizaste esta solicitud, puedes ignorar este correo. 💌</p>
                            <p style='margin-top: 30px;'>Gracias,<br><strong>Equipo de Shifui Kay</strong> 🍲</p>
                        </div>"
                };

                var json = JsonConvert.SerializeObject(contenido);
                var contenidoHttp = new StringContent(json, Encoding.UTF8, "application/json");

                try
                {
                    var respuesta = await cliente.PostAsync("https://api.brevo.com/v3/smtp/email", contenidoHttp);
                    string cuerpoRespuesta = await respuesta.Content.ReadAsStringAsync();

                    if (respuesta.IsSuccessStatusCode)
                    {
                        return new RespuestaEnvioCorreo { Exito = true };
                    }
                    else
                    {
                        // Devolvemos el mensaje de error para que la página que llama decida qué hacer.
                        return new RespuestaEnvioCorreo
                        {
                            Exito = false,
                            MensajeError = $"Error desde API de Brevo. Status: {(int)respuesta.StatusCode}, Body: {cuerpoRespuesta}"
                        };
                    }
                }
                catch (Exception ex)
                {
                    // Captura errores de red o de conexión.
                    return new RespuestaEnvioCorreo
                    {
                        Exito = false,
                        MensajeError = $"Excepción al conectar con Brevo: {ex.Message}"
                    };
                }
            }
        }

    }
}