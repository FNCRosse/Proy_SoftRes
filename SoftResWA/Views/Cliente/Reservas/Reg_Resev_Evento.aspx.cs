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
    public partial class Reg_Resev_Evento : System.Web.UI.Page
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
            // Para eventos, establecemos una fecha mínima de al menos 7 días en el futuro
            DateTime fechaMinima = DateTime.Today.AddDays(7);
            txtFecha.Attributes["min"] = fechaMinima.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Maneja el evento de clic del botón registrar evento
        /// </summary>
        protected void btnRegistrarEvento_Click(object sender, EventArgs e)
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
                        tipoReserva = tipoReserva.EVENTO,
                        tipoReservaSpecified = true,
                        estado = estadoReserva.PENDIENTE,
                        estadoSpecified = true,
                        fechaCreacion = DateTime.Now,
                        fechaCreacionSpecified = true,
                        usuarioCreacion = UsuarioActual.nombreComp,
                        // Campos específicos de eventos
                        nombreEvento = txtNombreEvento.Text.Trim(),
                        descripcionEvento = txtDescripcionEvento.Text.Trim(),
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
                        MostrarModalEventoRegistrado();
                        LimpiarFormulario();
                    }
                    else
                    {
                        MostrarAlerta("Error", "No se pudo registrar el evento. Por favor, inténtelo nuevamente.", "error");
                    }
                }
                catch (Exception ex)
                {
                    MostrarAlerta("Error", "Error al registrar el evento: " + ex.Message, "error");
                }
            }
        }

        /// <summary>
        /// Valida todos los campos del formulario específicos para eventos
        /// </summary>
        private bool ValidarDatos()
        {
            // Validar nombre del evento
            if (string.IsNullOrWhiteSpace(txtNombreEvento.Text))
            {
                MostrarAlerta("Validación", "Por favor ingrese el nombre del evento.", "warning");
                return false;
            }

            if (txtNombreEvento.Text.Trim().Length < 3)
            {
                MostrarAlerta("Validación", "El nombre del evento debe tener al menos 3 caracteres.", "warning");
                return false;
            }

            // Validar descripción del evento
            if (string.IsNullOrWhiteSpace(txtDescripcionEvento.Text))
            {
                MostrarAlerta("Validación", "Por favor ingrese una descripción del evento.", "warning");
                return false;
            }

            if (txtDescripcionEvento.Text.Trim().Length < 10)
            {
                MostrarAlerta("Validación", "La descripción del evento debe tener al menos 10 caracteres.", "warning");
                return false;
            }

            // Validar fecha
            if (string.IsNullOrWhiteSpace(txtFecha.Text))
            {
                MostrarAlerta("Validación", "Por favor seleccione una fecha para su evento.", "warning");
                return false;
            }

            DateTime fechaEvento;
            if (!DateTime.TryParse(txtFecha.Text, out fechaEvento))
            {
                MostrarAlerta("Validación", "La fecha seleccionada no es válida.", "warning");
                return false;
            }

            // Para eventos, requerimos al menos 7 días de anticipación
            if (fechaEvento < DateTime.Today.AddDays(7))
            {
                MostrarAlerta("Validación", "Los eventos deben programarse con al menos 7 días de anticipación.", "warning");
                return false;
            }

            // Validar hora
            if (string.IsNullOrWhiteSpace(txtHora.Text))
            {
                MostrarAlerta("Validación", "Por favor seleccione una hora para su evento.", "warning");
                return false;
            }

            TimeSpan horaEvento;
            if (!TimeSpan.TryParse(txtHora.Text, out horaEvento))
            {
                MostrarAlerta("Validación", "La hora seleccionada no es válida.", "warning");
                return false;
            }

            // Validar que la hora esté en horario de eventos (10:00 AM - 8:00 PM para que terminen antes del cierre)
            if (horaEvento < new TimeSpan(10, 0, 0) || horaEvento > new TimeSpan(20, 0, 0))
            {
                MostrarAlerta("Validación", "Los eventos se pueden programar entre las 10:00 AM y 8:00 PM (duración mínima 3 horas).", "warning");
                return false;
            }

            // Validar cantidad de personas
            if (string.IsNullOrEmpty(ddlCantidadPersonas.SelectedValue))
            {
                MostrarAlerta("Validación", "Por favor seleccione la cantidad de personas.", "warning");
                return false;
            }

            int cantidadPersonas = int.Parse(ddlCantidadPersonas.SelectedValue);
            if (cantidadPersonas < 20)
            {
                MostrarAlerta("Validación", "Los eventos requieren un mínimo de 20 personas.", "warning");
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

            // Validar coherencia entre personas y mesas para eventos
            int personas = int.Parse(ddlCantidadPersonas.SelectedValue);
            int mesas = int.Parse(ddlCantidadMesas.SelectedValue);
            
            // Para eventos, asumimos 4-5 personas por mesa
            if (personas > mesas * 5)
            {
                MostrarAlerta("Validación", "La cantidad de mesas no es suficiente para la cantidad de personas seleccionada.", "warning");
                return false;
            }

            if (personas < mesas * 3)
            {
                MostrarAlerta("Validación", "Demasiadas mesas para la cantidad de personas. Considere reducir el número de mesas.", "warning");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Muestra el modal de evento registrado
        /// </summary>
        private void MostrarModalEventoRegistrado()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showEventModal",
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
            txtNombreEvento.Text = "";
            txtDescripcionEvento.Text = "";
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