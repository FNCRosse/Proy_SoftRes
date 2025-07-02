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
using System.ComponentModel;
using reservaDTO = SoftResBusiness.ReservaWSClient.reservaDTO;
using tipoReserva = SoftResBusiness.ReservaWSClient.tipoReserva;
using estadoReserva = SoftResBusiness.ReservaWSClient.estadoReserva;
using localDTO = SoftResBusiness.LocalWSClient.localDTO;
using usuariosDTO = SoftResBusiness.UsuarioWSClient.usuariosDTO;

namespace SoftResWA.Views.Cliente.Reservas
{
    public partial class Reg_Resev_Comun : System.Web.UI.Page
    {
        private ReservaBO reservaBO;
        private LocalBO localBO;

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
            }
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
        /// Maneja el evento de clic del botón registrar reserva
        /// </summary>
        protected void btnRegistrarReserva_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                try
                {
                    reservaBO = new ReservaBO();
                    
                    // Combinar fecha y hora
                    DateTime fechaCompleta = DateTime.Parse(txtFecha.Text).Date.Add(TimeSpan.Parse(txtHora.Text));
                    
                    var nuevaReserva = new reservaDTO
                    {
                        fecha_Hora = fechaCompleta,
                        fecha_HoraSpecified = true,
                        cantidad_personas = int.Parse(ddlCantidadPersonas.SelectedValue),
                        cantidad_personasSpecified = true,
                        numeroMesas = int.Parse(ddlCantidadMesas.SelectedValue),
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
                            idLocal = int.Parse(ddlLocal.SelectedValue),
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
                        if (string.IsNullOrEmpty(nuevaReserva.observaciones))
                            nuevaReserva.observaciones = "";
                        nuevaReserva.observaciones += $" Ubicación preferida: {ddlUbicacion.SelectedValue}";
                    }

                    int resultado = reservaBO.Insertar(nuevaReserva);
                    
                    if (resultado > 0)
                    {
                        MostrarAlerta("¡Éxito!", "Tu reserva común ha sido registrada exitosamente. Te contactaremos para confirmar la disponibilidad.", "success");
                        LimpiarFormulario();
                    }
                    else
                    {
                        // Si no se pudo registrar, mostrar modal de lista de espera
                        MostrarModalListaEspera();
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
    }
}