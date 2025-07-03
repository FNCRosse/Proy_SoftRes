using SoftResBusiness;
using SoftResBusiness.LocalWSClient;
using SoftResBusiness.ReservaWSClient;
using SoftResBusiness.TipoMesaWSClient;
using SoftResBusiness.UsuarioWSClient;
using System;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using usuariosDTO = SoftResBusiness.UsuarioWSClient.usuariosDTO;

namespace SoftResWA.Views.Reservas
{
    public partial class registrar_reserva_evento : System.Web.UI.Page
    {
        private readonly LocalBO localBO;
        private readonly TipoMesaBO tipoMesaBO;
        private readonly UsuarioBO usuarioBO;
        private readonly ReservaBO reservaBO;

        public usuariosDTO UsuarioActual => Session["UsuarioLogueado"] as usuariosDTO;

        public registrar_reserva_evento()
        {
            localBO = new LocalBO();
            tipoMesaBO = new TipoMesaBO();
            usuarioBO = new UsuarioBO();
            reservaBO = new ReservaBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UsuarioActual == null || UsuarioActual.rol.esCliente)
            {
                Response.Redirect("~/Views/Login/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarControlesIniciales();
            }
        }

        #region Carga de Controles y Datos

        private void CargarControlesIniciales()
        {
            try
            {
                // La lógica es idéntica a la de la reserva común

                ddlLocal.DataSource = localBO.Listar(new localParametros { estado = true, estadoSpecified = true });
                ddlLocal.DataTextField = "nombre";
                ddlLocal.DataValueField = "idLocal";
                ddlLocal.DataBind();
                ddlLocal.Items.Insert(0, new ListItem("Seleccione un local...", "0"));

                ddlTipoMesa.DataSource = tipoMesaBO.Listar();
                ddlTipoMesa.DataTextField = "nombre";
                ddlTipoMesa.DataValueField = "idTipoMesa";
                ddlTipoMesa.DataBind();
                ddlTipoMesa.Items.Insert(0, new ListItem("Seleccione un tipo...", "0"));
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error de Carga", "No se pudieron cargar los datos iniciales.", "error");
            }
        }
        #endregion

        #region Eventos de Botones

        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                string dni = txtDniCliente.Text.Trim();
                if (string.IsNullOrWhiteSpace(dni) || dni.Length != 8)
                {
                    MostrarAlerta("DNI Inválido", "Por favor, ingrese un DNI de 8 dígitos.", "warning");
                    return;
                }

                var parametros = new SoftResBusiness.UsuarioWSClient.usuariosParametros();
                parametros.numDocumento = dni;
                parametros.esClienteSpecified = true;
                parametros.esCliente = true;
                parametros.estadoSpecified = true;
                parametros.estado = true;
                BindingList<usuariosDTO> clientes = usuarioBO.Listar(parametros);

                if (clientes != null && clientes.Any())
                {
                    var cliente = clientes.First();
                    txtNombreCliente.Text = cliente.nombreComp;
                    hdnIdCliente.Value = cliente.idUsuario.ToString();
                }
                else
                {
                    MostrarAlerta("Cliente no Encontrado", "No se encontró ningún cliente con ese DNI.", "info");
                    txtNombreCliente.Text = "";
                    hdnIdCliente.Value = "";
                }
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error", "Ocurrió un problema al buscar el cliente.", "error");
            }
        }

        protected void btnVerificarDisponibilidad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlLocal.SelectedValue) || ddlLocal.SelectedValue == "0" ||
                string.IsNullOrEmpty(txtFecha.Text) || string.IsNullOrEmpty(txtHora.Text) ||
                string.IsNullOrEmpty(ddlTipoMesa.SelectedValue) || ddlTipoMesa.SelectedValue == "0" ||
                string.IsNullOrEmpty(txtCantidadPersonas.Text))
            {
                MostrarAlerta("Datos Faltantes", "Por favor, complete Local, Fecha, Hora, Cantidad de Personas y Tipo de Mesa para verificar.", "warning");
                return;
            }

            try
            {
                reservaDTO reservaParaVerificar = ConstruirDTOBase();

                // Lógica de simulación (reemplazar con la llamada real al backend)
                bool disponible = true;

                if (disponible)
                {
                    lblDisponibilidad.Text = "¡Hay disponibilidad para este evento!";
                    lblDisponibilidad.CssClass = "fw-bold text-success";
                    btnGuardarReserva.Enabled = true;
                }
                else
                {
                    lblDisponibilidad.Text = "No hay disponibilidad. Intente otra fecha, hora o local.";
                    lblDisponibilidad.CssClass = "fw-bold text-danger";
                    btnGuardarReserva.Enabled = false;
                }
                lblDisponibilidad.Visible = true;
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error", "No se pudo verificar la disponibilidad.", "error");
            }
        }

        protected void btnGuardarReserva_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            if (string.IsNullOrEmpty(hdnIdCliente.Value))
            {
                MostrarAlerta("Cliente no Válido", "Debe buscar y seleccionar un cliente válido para la reserva.", "warning");
                return;
            }

            try
            {
                reservaDTO nuevaReserva = ConstruirDTOBase();

                // === CAMBIOS CLAVE PARA EVENTO ===
                nuevaReserva.tipoReserva = tipoReserva.EVENTO; // Se establece el tipo
                nuevaReserva.tipoReservaSpecified = true;
                nuevaReserva.nombreEvento = txtNombreEvento.Text.Trim();
                nuevaReserva.descripcionEvento = txtDescripcionEvento.Text.Trim();
                // ===================================

                nuevaReserva.estado = estadoReserva.PENDIENTE;
                nuevaReserva.estadoSpecified = true;
                nuevaReserva.usuarioCreacion = UsuarioActual.email;
                nuevaReserva.fechaCreacion = DateTime.Now;
                nuevaReserva.fechaCreacionSpecified = true;

                int idReserva = reservaBO.Insertar(nuevaReserva);

                if (idReserva > 0)
                {
                    string script = @"Swal.fire({
                                        title: '¡Reserva de Evento Registrada!',
                                        text: 'La reserva ha sido creada exitosamente con estado PENDIENTE.',
                                        icon: 'success'
                                    }).then(function() {
                                        window.location.href = 'reserva_gestion.aspx';
                                    });";
                    ScriptManager.RegisterStartupScript(this, GetType(), "reservaExito", script, true);
                }
                else if (idReserva == -1)
                {
                    MostrarAlerta("Lista de Espera", "No hay disponibilidad, la solicitud se ha añadido a la lista de espera.", "info");
                }
                else
                {
                    MostrarAlerta("Error", "No se pudo registrar la reserva del evento.", "error");
                }
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error Inesperado", ex.Message, "error");
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("reserva_gestion.aspx");
        }

        #endregion

        #region Métodos Auxiliares

        private reservaDTO ConstruirDTOBase()
        {
            DateTime fecha = DateTime.Parse(txtFecha.Text);
            TimeSpan hora = TimeSpan.Parse(txtHora.Text);
            DateTime fechaHoraCompleta = fecha.Add(hora);

            var reserva = new reservaDTO
            {
                fechaHoraRegistro = fechaHoraCompleta,
                fechaHoraRegistroSpecified = true,
                cantidadPersonas = int.Parse(txtCantidadPersonas.Text),
                cantidadPersonasSpecified = true,
                numeroMesas = 1, // El backend podría recalcular esto basado en la cantidad de personas
                numeroMesasSpecified = true,
                observaciones = txtObservaciones.Text.Trim(),
                local = new SoftResBusiness.ReservaWSClient.localDTO
                {
                    idLocal = int.Parse(ddlLocal.SelectedValue),
                    idLocalSpecified = true
                },
                tipoMesa = new SoftResBusiness.ReservaWSClient.tipoMesaDTO
                {
                    idTipoMesa = int.Parse(ddlTipoMesa.SelectedValue),
                    idTipoMesaSpecified = true
                }
            };

            if (!string.IsNullOrEmpty(hdnIdCliente.Value))
            {
                reserva.usuario = new SoftResBusiness.ReservaWSClient.usuariosDTO();
                reserva.usuario.idUsuario = int.Parse(hdnIdCliente.Value);
                reserva.usuario.idUsuarioSpecified = true;
            }

            return reserva;
        }

        private void MostrarAlerta(string titulo, string mensaje, string tipo)
        {
            string script = $"Swal.fire('{titulo}', '{Server.HtmlEncode(mensaje)}', '{tipo}');";
            ScriptManager.RegisterStartupScript(this, GetType(), $"alerta_{Guid.NewGuid()}", script, true);
        }

        #endregion
    }
}