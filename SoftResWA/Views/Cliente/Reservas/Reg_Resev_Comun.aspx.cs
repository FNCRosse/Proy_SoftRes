using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftResBusiness;
using SoftResBusiness.ReservaWSClient;
using SoftResBusiness.LocalWSClient;
using SoftResBusiness.UsuarioWSClient;
using SoftResBusiness.MesaWSClient;
using SoftResBusiness.ReservaxMesaWSClient;
using SoftResBusiness.FilaEsperaWSClient;
using System.ComponentModel;
using reservaDTO = SoftResBusiness.ReservaWSClient.reservaDTO;
using tipoReserva = SoftResBusiness.ReservaWSClient.tipoReserva;
using estadoReserva = SoftResBusiness.ReservaWSClient.estadoReserva;
using localDTO = SoftResBusiness.LocalWSClient.localDTO;
using usuariosDTO = SoftResBusiness.UsuarioWSClient.usuariosDTO;
using mesaDTO = SoftResBusiness.MesaWSClient.mesaDTO;
using reservaxMesasDTO = SoftResBusiness.ReservaxMesaWSClient.reservaxMesasDTO;

namespace SoftResWA.Views.Cliente.Reservas
{
    public partial class Reg_Resev_Comun : System.Web.UI.Page
    {
        private ReservaBO reservaBO;
        private LocalBO localBO;
        private MesaBO mesaBO;
        private ReservaxMesaBO reservaxMesaBO;
        private FilaEsperaBO filaEsperaBO;

        // Propiedad para obtener el usuario logueado
        public usuariosDTO UsuarioActual
        {
            get { return Session["UsuarioLogueado"] as usuariosDTO; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Verificar si el usuario está logueado
                if (UsuarioActual == null)
                {
                    Response.Redirect("~/Views/Cliente/Home/Login_Home.aspx");
                    return;
                }

                CargarLocales();
                EstablecerFechaMinima();
                InicializarServicios();
            }
        }

        private void InicializarServicios()
        {
            reservaBO = new ReservaBO();
            localBO = new LocalBO();
            mesaBO = new MesaBO();
            reservaxMesaBO = new ReservaxMesaBO();
            filaEsperaBO = new FilaEsperaBO();
        }

        /// <summary>
        /// Carga la lista de locales disponibles desde el backend
        /// </summary>
        private void CargarLocales()
        {
            try
            {
                localBO = new LocalBO();
                var parametros = new SoftResBusiness.LocalWSClient.localParametros();
                parametros.estadoSpecified = true;
                parametros.estado = true; // Solo locales activos
                parametros.nombre = null;
                parametros.idSedeSpecified = false;

                var locales = localBO.Listar(parametros);
                
                ddlLocal.DataTextField = "nombre";
                ddlLocal.DataValueField = "idLocal";
                ddlLocal.DataSource = locales;
                ddlLocal.DataBind();
                
                // Insertar opción por defecto
                ddlLocal.Items.Insert(0, new ListItem("Seleccione un local", ""));
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error", "Error al cargar locales: " + ex.Message, "error");
            }
        }

        /// <summary>
        /// Establece la fecha mínima como la fecha actual
        /// </summary>
        private void EstablecerFechaMinima()
        {
            txtFecha.Attributes["min"] = DateTime.Today.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Verifica la disponibilidad de mesas para la fecha y hora seleccionadas
        /// </summary>
        private bool VerificarDisponibilidad(DateTime fechaHora, int cantidadPersonas, int idLocal, out List<mesaDTO> mesasDisponibles)
        {
            try
            {
                // Obtener todas las mesas del local
                var parametrosMesa = new SoftResBusiness.MesaWSClient.mesaParametros
                {
                    idLocalSpecified = true,
                    idLocal = idLocal,
                    estadoSpecified = true,
                    //estado = true // Solo mesas activas
                };

                var todasLasMesas = mesaBO.Listar(parametrosMesa);
                
                // Obtener reservas existentes para esa fecha/hora
                var parametrosReserva = new reservaParametros
                {
                    fechaInicioSpecified = true,
                    fechaInicio = fechaHora.Date,
                    fechaFinSpecified = true,
                    fechaFin = fechaHora.Date.AddDays(1),
                    idLocalSpecified = true,
                    idLocal = idLocal,
                    estadoSpecified = true,
                    estado = estadoReserva.CONFIRMADA
                };

                var reservasExistentes = reservaBO.Listar(parametrosReserva);
                
                // Obtener mesas ya reservadas para esa hora
                var mesasReservadas = new HashSet<int>();
                foreach (var reserva in reservasExistentes)
                {
                    if (ReservasSeSuperponen(reserva.fecha_Hora, fechaHora))
                    {
                        var reservaxMesas = reservaxMesaBO.Listar(reserva.idReserva);
                        foreach (var rm in reservaxMesas)
                        {
                            if (rm.mesa != null && rm.mesa.idMesaSpecified)
                            {
                                mesasReservadas.Add(rm.mesa.idMesa);
                            }
                        }
                    }
                }

                // Filtrar mesas disponibles que cumplan con la capacidad
                mesasDisponibles = todasLasMesas
                    .Where(m => !mesasReservadas.Contains(m.idMesa) && 
                               m.capacidadSpecified && 
                               m.capacidad >= cantidadPersonas)
                    .ToList();

                return mesasDisponibles.Count > 0;
            }
            catch (Exception)
            {
                mesasDisponibles = new List<mesaDTO>();
                return false;
            }
        }

        private bool ReservasSeSuperponen(DateTime reservaExistente, DateTime nuevaReserva)
        {
            // Consideramos que una reserva dura 2 horas
            var inicioExistente = reservaExistente;
            var finExistente = reservaExistente.AddHours(2);
            var inicioNueva = nuevaReserva;
            var finNueva = nuevaReserva.AddHours(2);

            return (inicioNueva >= inicioExistente && inicioNueva < finExistente) ||
                   (finNueva > inicioExistente && finNueva <= finExistente) ||
                   (inicioNueva <= inicioExistente && finNueva >= finExistente);
        }

        /// <summary>
        /// Maneja el evento de clic del botón registrar reserva
        /// </summary>
        protected void btnRegistrarReserva_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                try
                {
                    // Combinar fecha y hora
                    DateTime fechaCompleta = DateTime.Parse(txtFecha.Text).Date.Add(TimeSpan.Parse(txtHora.Text));
                    int cantidadPersonas = int.Parse(ddlCantidadPersonas.SelectedValue);
                    int idLocal = int.Parse(ddlLocal.SelectedValue);

                    List<mesaDTO> mesasDisponibles;
                    bool hayDisponibilidad = VerificarDisponibilidad(fechaCompleta, cantidadPersonas, idLocal, out mesasDisponibles);

                    if (!hayDisponibilidad)
                    {
                        // No hay mesas disponibles, ofrecer lista de espera
                        MostrarModalListaEspera();
                        return;
                    }

                    var nuevaReserva = new reservaDTO
                    {
                        fecha_Hora = fechaCompleta,
                        fecha_HoraSpecified = true,
                        cantidad_personas = cantidadPersonas,
                        cantidad_personasSpecified = true,
                        numeroMesas = mesasDisponibles.Count,
                        numeroMesasSpecified = true,
                        observaciones = txtObservaciones.Text.Trim(),
                        tipoReserva = tipoReserva.COMUN,
                        tipoReservaSpecified = true,
                        estado = estadoReserva.PENDIENTE,
                        estadoSpecified = true,
                        fechaCreacion = DateTime.Now,
                        fechaCreacionSpecified = true,
                        usuarioCreacion = UsuarioActual.nombreComp,
                        local = new SoftResBusiness.ReservaWSClient.localDTO 
                        { 
                            idLocal = idLocal,
                            idLocalSpecified = true
                        },
                        usuario = new SoftResBusiness.ReservaWSClient.usuariosDTO 
                        { 
                            idUsuario = UsuarioActual.idUsuario,
                            idUsuarioSpecified = true,
                            numeroDocumento = UsuarioActual.numeroDocumento,
                            nombreComp = UsuarioActual.nombreComp,
                            email = UsuarioActual.email
                        }
                    };

                    // Agregar información de ubicación preferida si se seleccionó
                    if (!string.IsNullOrEmpty(ddlUbicacion.SelectedValue))
                    {
                        nuevaReserva.observaciones = string.IsNullOrEmpty(nuevaReserva.observaciones) 
                            ? $"Ubicación preferida: {ddlUbicacion.SelectedValue}"
                            : nuevaReserva.observaciones + $" | Ubicación preferida: {ddlUbicacion.SelectedValue}";
                    }

                    int idReserva = reservaBO.Insertar(nuevaReserva);
                    
                    if (idReserva > 0)
                    {
                        // Asignar mesas a la reserva
                        foreach (var mesa in mesasDisponibles)
                        {
                            var reservaxMesa = new reservaxMesasDTO
                            {
                                reserva = new SoftResBusiness.ReservaxMesaWSClient.reservaDTO
                                {
                                    idReserva = idReserva,
                                    idReservaSpecified = true
                                },
                                mesa = new SoftResBusiness.ReservaxMesaWSClient.mesaDTO
                                {
                                    idMesa = mesa.idMesa,
                                    idMesaSpecified = true
                                }
                            };
                            
                            reservaxMesaBO.Insertar(reservaxMesa);
                        }

                        MostrarAlerta("¡Éxito!", "Tu reserva común ha sido registrada exitosamente. Te contactaremos para confirmar la disponibilidad.", "success");
                        LimpiarFormulario();
                    }
                    else
                    {
                        MostrarAlerta("Error", "No se pudo registrar la reserva. Por favor, intenta nuevamente.", "error");
                    }
                }
                catch (Exception ex)
                {
                    MostrarAlerta("Error", "Error al registrar la reserva: " + ex.Message, "error");
                }
            }
        }

        /// <summary>
        /// Valida todos los campos del formulario
        /// </summary>
        private bool ValidarDatos()
        {
            // Validar fecha
            if (string.IsNullOrWhiteSpace(txtFecha.Text))
            {
                MostrarAlerta("Validación", "Por favor seleccione una fecha para su reserva.", "warning");
                return false;
            }

            DateTime fechaReserva;
            if (!DateTime.TryParse(txtFecha.Text, out fechaReserva))
            {
                MostrarAlerta("Validación", "La fecha seleccionada no es válida.", "warning");
                return false;
            }

            if (fechaReserva < DateTime.Today)
            {
                MostrarAlerta("Validación", "La fecha de reserva no puede ser anterior a hoy.", "warning");
                return false;
            }

            // Validar hora
            if (string.IsNullOrWhiteSpace(txtHora.Text))
            {
                MostrarAlerta("Validación", "Por favor seleccione una hora para su reserva.", "warning");
                return false;
            }

            TimeSpan horaReserva;
            if (!TimeSpan.TryParse(txtHora.Text, out horaReserva))
            {
                MostrarAlerta("Validación", "La hora seleccionada no es válida.", "warning");
                return false;
            }

            // Validar que la hora esté en horario de atención (ejemplo: 11:00 - 23:00)
            if (horaReserva < new TimeSpan(11, 0, 0) || horaReserva > new TimeSpan(23, 0, 0))
            {
                MostrarAlerta("Validación", "El horario de atención es de 11:00 AM a 11:00 PM.", "warning");
                return false;
            }

            // Validar cantidad de personas
            if (string.IsNullOrEmpty(ddlCantidadPersonas.SelectedValue))
            {
                MostrarAlerta("Validación", "Por favor seleccione la cantidad de personas.", "warning");
                return false;
            }

            // Validar local
            if (string.IsNullOrEmpty(ddlLocal.SelectedValue))
            {
                MostrarAlerta("Validación", "Por favor seleccione un local.", "warning");
                return false;
            }

            // Validar cantidad de mesas
            if (string.IsNullOrEmpty(ddlCantidadMesas.SelectedValue))
            {
                MostrarAlerta("Validación", "Por favor seleccione la cantidad de mesas.", "warning");
                return false;
            }

            // Validar coherencia entre personas y mesas
            int personas = int.Parse(ddlCantidadPersonas.SelectedValue);
            int mesas = int.Parse(ddlCantidadMesas.SelectedValue);
            
            if (personas > mesas * 4)
            {
                MostrarAlerta("Validación", "La cantidad de mesas no es suficiente para la cantidad de personas seleccionada.", "warning");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Maneja el evento de unirse a la lista de espera
        /// </summary>
        protected void btnUnirseEspera_Click(object sender, EventArgs e)
        {
            // TODO: Implementar lógica para agregar a lista de espera
            // Por ahora solo mostramos mensaje de confirmación
            MostrarAlerta("¡Listo!", "Has sido añadido a la lista de espera. Te notificaremos al correo registrado.", "success");
        }

        /// <summary>
        /// Muestra el modal de lista de espera
        /// </summary>
        private void MostrarModalListaEspera()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showWaitingList",
                @"setTimeout(function() {
                    var modal = new bootstrap.Modal(document.getElementById('modalListaEspera'));
                    modal.show();
                }, 500);", true);
        }

        /// <summary>
        /// Muestra una alerta usando SweetAlert2
        /// </summary>
        private void MostrarAlerta(string titulo, string mensaje, string tipo)
        {
            string script = $"Swal.fire('{titulo}', '{mensaje}', '{tipo}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", script, true);
        }

        /// <summary>
        /// Limpia todos los campos del formulario
        /// </summary>
        private void LimpiarFormulario()
        {
            txtFecha.Text = "";
            txtHora.Text = "";
            ddlCantidadPersonas.SelectedIndex = 0;
            ddlLocal.SelectedIndex = 0;
            ddlCantidadMesas.SelectedIndex = 0;
            ddlUbicacion.SelectedIndex = 0;
            txtObservaciones.Text = "";
        }

        protected void ddlLocal_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarDisponibilidadActual();
        }

        protected void txtFecha_TextChanged(object sender, EventArgs e)
        {
            VerificarDisponibilidadActual();
        }

        protected void txtHora_TextChanged(object sender, EventArgs e)
        {
            VerificarDisponibilidadActual();
        }

        protected void ddlCantidadPersonas_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarDisponibilidadActual();
        }

        private void VerificarDisponibilidadActual()
        {
            // Verificar que todos los campos necesarios estén completos
            if (string.IsNullOrEmpty(ddlLocal.SelectedValue) ||
                string.IsNullOrEmpty(txtFecha.Text) ||
                string.IsNullOrEmpty(txtHora.Text) ||
                string.IsNullOrEmpty(ddlCantidadPersonas.SelectedValue))
            {
                lblDisponibilidad.Text = "Complete todos los campos para verificar la disponibilidad.";
                pnlMesasDisponibles.Visible = false;
                pnlSinDisponibilidad.Visible = false;
                return;
            }

            try
            {
                DateTime fechaHora = DateTime.Parse(txtFecha.Text).Date.Add(TimeSpan.Parse(txtHora.Text));
                int cantidadPersonas = int.Parse(ddlCantidadPersonas.SelectedValue);
                int idLocal = int.Parse(ddlLocal.SelectedValue);

                List<mesaDTO> mesasDisponibles;
                bool hayDisponibilidad = VerificarDisponibilidad(fechaHora, cantidadPersonas, idLocal, out mesasDisponibles);

                if (hayDisponibilidad)
                {
                    lblDisponibilidad.Text = "¡Hay disponibilidad para tu reserva!";
                    lblMesasDisponibles.Text = $"Se encontraron {mesasDisponibles.Count} mesas disponibles que cumplen con tus requisitos.";
                    pnlMesasDisponibles.Visible = true;
                    pnlSinDisponibilidad.Visible = false;
                }
                else
                {
                    lblDisponibilidad.Text = "Lo sentimos, no hay disponibilidad para los criterios seleccionados.";
                    pnlMesasDisponibles.Visible = false;
                    pnlSinDisponibilidad.Visible = true;
                }

                upDisponibilidad.Update();
            }
            catch (Exception ex)
            {
                lblDisponibilidad.Text = "Error al verificar disponibilidad: " + ex.Message;
                pnlMesasDisponibles.Visible = false;
                pnlSinDisponibilidad.Visible = false;
                upDisponibilidad.Update();
            }
        }
    }
}