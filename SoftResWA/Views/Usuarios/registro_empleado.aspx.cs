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
        public async Task<bool> EnviarCorreoBrevo(string correoDestino, string nombreUsuario, string urlCambio)
        {
            var apiKey = ConfigurationManager.AppSettings["BrevoApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                System.Diagnostics.Debug.WriteLine("Error: La API Key de Brevo no está configurada en Web.config.");
                return false;
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

                    System.Diagnostics.Debug.WriteLine($"Respuesta de Brevo - Status: {(int)respuesta.StatusCode}, Body: {cuerpoRespuesta}");

                    if (!respuesta.IsSuccessStatusCode)
                    {
                        // Si falla, muestra el error real de la API para facilitar el debug.
                        ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, typeof(Page), "debugError",
                            $"Swal.fire('Error desde API', 'Status: {(int)respuesta.StatusCode} <br/> Respuesta: {cuerpoRespuesta.Replace("'", "\\'")}', 'error');", true);
                    }

                    return respuesta.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    // Captura errores de red o de conexión.
                    System.Diagnostics.Debug.WriteLine("Excepción al conectar con Brevo: " + ex.Message);
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, typeof(Page), "exceptionError",
                        $"Swal.fire('Error de Conexión', 'No se pudo contactar al servidor de correos. Error: {ex.Message.Replace("'", "\\'")}', 'error');", true);
                    return false;
                }
            }
        }

        // 1. El método del evento de clic ahora es un 'void' normal.
        protected void btnEnviarCorreoCambio_Click(object sender, EventArgs e)
        {
            // 2. Le decimos a la página que ejecute una tarea asíncrona y que espere a que termine.
            RegisterAsyncTask(new PageAsyncTask(EnviarCorreoYMostrarAlerta));
        }

        // 3. Creamos un nuevo método 'async Task' que contiene toda la lógica.
        private async System.Threading.Tasks.Task EnviarCorreoYMostrarAlerta()
        {
            string correo = txtCorreoModal.Text.Trim();
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

            bool enviado = await EnviarCorreoBrevo(correo, nombre, linkCambio);

            if (enviado)
            {
                string script = $"Swal.fire('¡Correo Enviado!', 'Se ha enviado un enlace de recuperación a {correo.Replace("'", "\\'")}', 'success');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "correoExito", script, true);
            }
            else
            {
                // También es buena idea manejar el caso de error
                string script = "Swal.fire('Error', 'No se pudo enviar el correo. Por favor, inténtalo de nuevo.', 'error');";
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