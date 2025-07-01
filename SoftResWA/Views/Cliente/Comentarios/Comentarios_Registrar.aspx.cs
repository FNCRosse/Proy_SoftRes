using SoftResBusiness;
using SoftResBusiness.ComentarioWSClient;
using SoftResBusiness.LocalWSClient;
using SoftResBusiness.ReservaWSClient;
using SoftResBusiness.UsuarioWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using reservaDTO = SoftResBusiness.ReservaWSClient.reservaDTO;
using usuariosDTO = SoftResBusiness.UsuarioWSClient.usuariosDTO;

namespace SoftResWA.Views.Cliente.Comentarios
{
    public partial class Comentarios_Registrar : System.Web.UI.Page
    {
        private ComentarioBO comentarioBO;
        private ReservaBO reservaBO;

        public ComentarioBO ComentarioBO { get => comentarioBO; set => comentarioBO = value; }
        public ReservaBO ReservaBO { get => reservaBO; set => reservaBO = value; }
        public usuariosDTO UsuarioActual
        {
            get { return Session["UsuarioLogueado"] as usuariosDTO; }
        }

        public Comentarios_Registrar()
        {
            this.comentarioBO = new ComentarioBO();
            this.reservaBO = new ReservaBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UsuarioActual == null)
            {
                Response.Redirect("~/Views/Cliente/Home/Login_Home.aspx");
                return;
            }
            if (!IsPostBack)
            {
                CargarPuntuacion();
                CargarReservasComentables();
            }
        }
        private void CargarPuntuacion()
        {
            // Creamos una lista de objetos para el Repeater.
            // El orden es inverso (de 5 a 1) para que funcione con el CSS 'direction: rtl'.
            var estrellas = new[]
            {
                new { Value = 5, Title = "5 estrellas" },
                new { Value = 4, Title = "4 estrellas" },
                new { Value = 3, Title = "3 estrellas" },
                new { Value = 2, Title = "2 estrellas" },
                new { Value = 1, Title = "1 estrella" }
            };

            rptPuntuacion.DataSource = estrellas;
            rptPuntuacion.DataBind();
        }

        protected void cvPuntuacion_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // La lógica de validación del servidor es muy simple:
            // Comprueba si el HiddenField tiene un valor.
            args.IsValid = !string.IsNullOrEmpty(hdnPuntuacionSeleccionada.Value);
        }

        private void CargarReservasComentables()
        {
            try
            {
                // Busca las reservas del usuario que están 'CONFIRMADAS' y son recientes
                reservaParametros parametros = new reservaParametros();
                parametros.dniCliente = UsuarioActual.numeroDocumento;
                parametros.estadoSpecified = true;
                parametros.estado = SoftResBusiness.ReservaWSClient.estadoReserva.CONFIRMADA;
                parametros.fechaInicioSpecified = true;
                parametros.fechaInicio = DateTime.Now.AddMonths(-3);

                BindingList<reservaDTO> reservas = reservaBO.Listar(parametros);

                if (reservas != null && reservas.Any())
                {
                    ddlReservas.DataSource = reservas.Select(r => new
                    {
                        Id = r.idReserva,
                        Texto = $"Reserva del {r.fecha_Hora:dd/MM/yyyy} en {r.local?.nombre ?? "Local"}"
                    }).ToList();
                    ddlReservas.DataTextField = "Texto";
                    ddlReservas.DataValueField = "Id";
                    ddlReservas.DataBind();
                }

                ddlReservas.Items.Insert(0, new ListItem("Selecciona una reserva para comentar...", "0"));
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error", "No se pudieron cargar tus reservas.", "error");
                System.Diagnostics.Debug.WriteLine($"Error al cargar reservas: {ex.Message}");
            }
        }
        private void MostrarAlerta(string titulo, string mensaje, string tipo)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), $"alerta_{tipo}",
                $"Swal.fire('{titulo}', '{Server.HtmlEncode(mensaje)}', '{tipo}');", true);
        }
        private void MostrarResultado(bool exito, string entidad, string modo)
        {
            string accion = (modo == "modificar") ? "modificado" :
                            (modo == "eliminar") ? "eliminado" : "registrado";

            string accionNo = (modo == "modificar") ? "modificar" :
                              (modo == "eliminar") ? "eliminar" : "registrar";

            string baseMensaje = exito ? $"El {accion}" : $"El {accionNo} NO";
            string tipo = exito ? "success" : "warning";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje",
                $"Swal.fire('¡{entidad} {(exito ? accion : $"NO {accion}")}!', '{baseMensaje} se completó correctamente.', '{tipo}');", true);
        }
        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            // Validar de nuevo en el servidor por si se omite la validación del cliente
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                var comentario = new comentariosDTO();
                // Asignar los datos del formulario
                comentario.mensaje = txtMensaje.Text.Trim();
                comentario.puntuacion = int.Parse(hdnPuntuacionSeleccionada.Value);
                comentario.puntuacionSpecified = true;
                comentario.puntuacionSpecified = true;
                // Asignar el usuario y la reserva
                comentario.usuario = new SoftResBusiness.ComentarioWSClient.usuariosDTO();
                comentario.usuario.idUsuario = UsuarioActual.idUsuario;
                comentario.usuario.idUsuarioSpecified = true;
                comentario.reserva = new SoftResBusiness.ComentarioWSClient.reservaDTO();
                comentario.reserva.idReserva = int.Parse(ddlReservas.SelectedValue);
                comentario.reserva.idReservaSpecified = true;
                // Datos de auditoría
                comentario.estado = true;
                comentario.estadoSpecified = true;
                comentario.fechaCreacion = DateTime.Now;
                comentario.fechaCreacionSpecified = true;
                comentario.usuarioCreacion = UsuarioActual.nombreComp;
                int resultado = comentarioBO.Insertar(comentario);
                if (resultado > 0)
                {
                    string script = @"Swal.fire({
                                        title: '¡Gracias por tu opinión!',
                                        text: 'Tu comentario ha sido enviado correctamente.',
                                        icon: 'success',
                                        timer: 3000,
                                        showConfirmButton: false
                                    }).then(function() {
                                        window.location.href = 'Comentarios_Listado.aspx';
                                    });";
                    ScriptManager.RegisterStartupScript(this, GetType(), "comentarioExito", script, true);
                }
                else
                {
                    MostrarAlerta("Error", "No se pudo enviar tu comentario. Por favor, inténtalo de nuevo.", "error");
                }
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error Inesperado", $"Ocurrió un error: {ex.Message}", "error");
            }
        }
    }
}