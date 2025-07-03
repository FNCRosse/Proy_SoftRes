using SoftResBusiness;
using SoftResBusiness.FilaEsperaWSClient; // Importante para la fila de espera
using SoftResBusiness.LocalWSClient;
using SoftResBusiness.ReservaWSClient;
using SoftResBusiness.TipoMesaWSClient;
using SoftResBusiness.UsuarioWSClient;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using usuariosDTO = SoftResBusiness.UsuarioWSClient.usuariosDTO;
namespace SoftResWA.Views.Cliente.Reservas
{
    public partial class Reg_Resev_Comun : System.Web.UI.Page
    {
        private readonly ReservaBO reservaBO;
        private readonly LocalBO localBO;
        private readonly TipoMesaBO tipoMesaBO;
        private readonly FilaEsperaBO filaEsperaBO; // BO para la lista de espera

        public usuariosDTO UsuarioActual => Session["UsuarioLogueado"] as usuariosDTO;

        public Reg_Resev_Comun()
        {
            reservaBO = new ReservaBO();
            localBO = new LocalBO();
            tipoMesaBO = new TipoMesaBO();
            filaEsperaBO = new FilaEsperaBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UsuarioActual == null)
            {
                // Si no está logueado, lo mandamos a la página de login
                Response.Redirect("~/Views/Cliente/Home/Login_Home.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarControlesIniciales();
            }
        }

        private void CargarControlesIniciales()
        {
            try
            {
                // Cargar Locales activos
                ddlLocal.DataSource = localBO.Listar(new localParametros { estado = true, estadoSpecified = true });
                ddlLocal.DataTextField = "nombre";
                ddlLocal.DataValueField = "idLocal";
                ddlLocal.DataBind();
                ddlLocal.Items.Insert(0, new ListItem("Seleccione un local...", "0"));

                // Cargar Tipos de Mesa
                ddlTipoMesa.DataSource = tipoMesaBO.Listar();
                ddlTipoMesa.DataTextField = "nombre";
                ddlTipoMesa.DataValueField = "idTipoMesa";
                ddlTipoMesa.DataBind();
                ddlTipoMesa.Items.Insert(0, new ListItem("Seleccione una preferencia...", "0"));
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error", "No se pudieron cargar los datos del formulario.", "error");
            }
        }

        protected void btnEnviarReserva_Click(object sender, EventArgs e)
        {
            Page.Validate("ReservaCliente");
            if (!Page.IsValid) return;

            try
            {
                reservaDTO nuevaReserva = ConstruirDTOBase();

                // Guardamos los datos de la reserva en la sesión por si el usuario
                // decide unirse a la lista de espera.
                Session["ReservaTemporal"] = nuevaReserva;

                int resultado = reservaBO.Insertar(nuevaReserva);

                if (resultado > 0)
                {
                    MostrarAlertaConRedireccion("¡Reserva Registrada!", "Tu solicitud de reserva ha sido enviada. Recibirás una notificación cuando sea confirmada.", "success", "MisReservas.aspx");
                }
                else if (resultado == -1)
                {
                    // No hay disponibilidad, abrimos el modal de lista de espera
                    ScriptManager.RegisterStartupScript(this, GetType(), "abrirModalEspera", "abrirModal('modalListaEspera');", true);
                }
                else
                {
                    MostrarAlerta("Error", "No se pudo registrar tu reserva.", "error");
                }
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error de Solicitud", ex.Message, "warning");
            }
        }

        protected void btnUnirseEspera_Click(object sender, EventArgs e)
        {
            try
            {
                // Recuperamos la reserva que el usuario intentó hacer
                var reservaTemporal = Session["ReservaTemporal"] as reservaDTO;
                if (reservaTemporal == null)
                {
                    MostrarAlerta("Error", "Se ha perdido la información de tu reserva. Por favor, inténtalo de nuevo.", "error");
                    return;
                }

                // Creamos un objeto para la fila de espera
                var filaEspera = new SoftResBusiness.ReservaWSClient.filaEsperaDTO
                {
                    usuario = reservaTemporal.usuario,
                    local = reservaTemporal.local,
                    tipoReserva = reservaTemporal.tipoReserva,
                    cantidadPersonas = reservaTemporal.cantidadPersonas,
                    tipoMesa = reservaTemporal.tipoMesa,
                    fechaHoraDeseada = reservaTemporal.fechaHoraRegistro,
                    observaciones = reservaTemporal.observaciones,
                    // El backend se encarga del estado y fecha de registro
                };

                // Llamamos al BO para insertar en la fila de espera
                //filaEsperaDTO resultado = filaEsperaBO.Insertar(filaEspera);

                //if (resultado != null && resultado.idFila > 0)
                //{
                //    MostrarAlertaConRedireccion("¡Estás en la Lista!", "Te hemos añadido a la lista de espera. Te notificaremos si se libera un lugar.", "success", "Home_Cliente.aspx");
                //}
                //else
                //{
                //    MostrarAlerta("Error", "No pudimos añadirte a la lista de espera.", "error");
                //}
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error Inesperado", ex.Message, "error");
            }
        }

        private reservaDTO ConstruirDTOBase()
        {
            DateTime fecha = DateTime.Parse(txtFecha.Text);
            TimeSpan hora = TimeSpan.Parse(txtHora.Text);
            DateTime fechaHoraCompleta = fecha.Add(hora);

            return new reservaDTO
            {
                fechaHoraRegistro = fechaHoraCompleta,
                fechaHoraRegistroSpecified = true,
                cantidadPersonas = int.Parse(txtCantidadPersonas.Text),
                cantidadPersonasSpecified = true,
                numeroMesas = 1, // Para reserva común, el backend decidirá cuántas mesas son necesarias
                numeroMesasSpecified = true,
                observaciones = txtObservaciones.Text.Trim(),
                tipoReserva = SoftResBusiness.ReservaWSClient.tipoReserva.COMUN,
                tipoReservaSpecified = true,
                usuario = new SoftResBusiness.ReservaWSClient.usuariosDTO { idUsuario = UsuarioActual.idUsuario, idUsuarioSpecified = true },
                local = new SoftResBusiness.ReservaWSClient.localDTO { idLocal = int.Parse(ddlLocal.SelectedValue), idLocalSpecified = true },
                tipoMesa = new SoftResBusiness.ReservaWSClient.tipoMesaDTO { idTipoMesa = int.Parse(ddlTipoMesa.SelectedValue), idTipoMesaSpecified = true },
                usuarioCreacion = UsuarioActual.nombreComp,
                fechaCreacion = DateTime.Now,
                fechaCreacionSpecified = true
            };
        }

        private void MostrarAlerta(string titulo, string mensaje, string tipo)
        {
            string script = $"Swal.fire('{titulo}', '{Server.HtmlEncode(mensaje)}', '{tipo}');";
            ScriptManager.RegisterStartupScript(this, GetType(), $"alerta_{Guid.NewGuid()}", script, true);
        }

        private void MostrarAlertaConRedireccion(string titulo, string mensaje, string tipo, string url)
        {
            string script = $@"Swal.fire({{
                                    title: '{titulo}',
                                    text: '{Server.HtmlEncode(mensaje)}',
                                    icon: '{tipo}',
                                    timer: 3000,
                                    showConfirmButton: false
                                }}).then(function() {{
                                    window.location.href = '{url}';
                                }});";
            ScriptManager.RegisterStartupScript(this, GetType(), "alertaConRedirect", script, true);
        }

    }
}