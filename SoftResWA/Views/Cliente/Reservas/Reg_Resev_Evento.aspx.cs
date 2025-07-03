using SoftResBusiness;
using SoftResBusiness.FilaEsperaWSClient;
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
    public partial class Reg_Resev_Evento : System.Web.UI.Page
    {
        private readonly ReservaBO reservaBO;
        private readonly LocalBO localBO;
        private readonly TipoMesaBO tipoMesaBO;
        private readonly FilaEsperaBO filaEsperaBO;

        public usuariosDTO UsuarioActual => Session["UsuarioLogueado"] as usuariosDTO;

        public Reg_Resev_Evento()
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
                ddlLocal.DataSource = localBO.Listar(new localParametros { estado = true, estadoSpecified = true });
                ddlLocal.DataTextField = "nombre";
                ddlLocal.DataValueField = "idLocal";
                ddlLocal.DataBind();
                ddlLocal.Items.Insert(0, new ListItem("Seleccione un local...", "0"));

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
            Page.Validate("ReservaEventoCliente");
            if (!Page.IsValid) return;

            try
            {
                reservaDTO nuevaReserva = ConstruirDTOBase();
                Session["ReservaTemporal"] = nuevaReserva; // Guardar para posible lista de espera

                int resultado = reservaBO.Insertar(nuevaReserva);

                if (resultado > 0)
                {
                    MostrarAlertaConRedireccion("¡Solicitud Enviada!", "Tu solicitud para el evento ha sido registrada. Recibirás una notificación cuando sea confirmada.", "success", "MisReservas.aspx");
                }
                else if (resultado == -1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "abrirModalEspera", "abrirModal('modalListaEspera');", true);
                }
                else
                {
                    MostrarAlerta("Error", "No se pudo registrar la reserva de tu evento.", "error");
                }
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error de Solicitud", ex.Message, "warning");
            }
        }

        // El método btnUnirseEspera_Click sería idéntico al de la reserva común,
        // ya que la lógica de la lista de espera es la misma.
        protected void btnUnirseEspera_Click(object sender, EventArgs e)
        {
            try
            {
                var reservaTemporal = Session["ReservaTemporal"] as reservaDTO;
                if (reservaTemporal == null)
                {
                    MostrarAlerta("Error", "Se ha perdido la información de tu reserva. Por favor, inténtalo de nuevo.", "error");
                    return;
                }

                var filaEspera = new SoftResBusiness.ReservaWSClient.filaEsperaDTO
                {
                    usuario = reservaTemporal.usuario,
                    local = reservaTemporal.local,
                    tipoReserva = reservaTemporal.tipoReserva, // Ya será de tipo EVENTO
                    cantidadPersonas = reservaTemporal.cantidadPersonas,
                    tipoMesa = reservaTemporal.tipoMesa,
                    fechaHoraDeseada = reservaTemporal.fechaHoraRegistro,
                    observaciones = reservaTemporal.observaciones,
                };

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
                // === CAMPOS ESPECÍFICOS DE EVENTO ===
                tipoReserva = SoftResBusiness.ReservaWSClient.tipoReserva.EVENTO,
                tipoReservaSpecified = true,
                nombreEvento = txtNombreEvento.Text.Trim(),
                descripcionEvento = txtDescripcionEvento.Text.Trim(),
                // ===================================

                fechaHoraRegistro = fechaHoraCompleta,
                fechaHoraRegistroSpecified = true,
                cantidadPersonas = int.Parse(txtCantidadPersonas.Text),
                cantidadPersonasSpecified = true,
                numeroMesas = int.Parse(txtCantidadMesas.Text),
                numeroMesasSpecified = true,
                observaciones = txtObservaciones.Text.Trim(),
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