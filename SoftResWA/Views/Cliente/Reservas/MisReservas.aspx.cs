using SoftResBusiness;
using SoftResBusiness.ReservaWSClient;
using SoftResBusiness.LocalWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Cliente.Reservas
{
    public partial class MisReservas : System.Web.UI.Page
    {
        private ReservaBO reservaBO;
        private LocalBO localBO;
        private BindingList<reservaDTO> listadoReservas;
        private BindingList<SoftResBusiness.LocalWSClient.localDTO> listadoLocales;
        private int idUsuarioActual = 1; // TODO: Obtener del Session

        public ReservaBO ReservaBO { get => reservaBO; set => reservaBO = value; }
        public LocalBO LocalBO { get => localBO; set => localBO = value; }
        public BindingList<reservaDTO> ListadoReservas { get => listadoReservas; set => listadoReservas = value; }
        public BindingList<SoftResBusiness.LocalWSClient.localDTO> ListadoLocales { get => listadoLocales; set => listadoLocales = value; }

        public MisReservas()
        {
            this.reservaBO = new ReservaBO();
            this.localBO = new LocalBO();
        }

        private void CargarDatos()
        {
            try
            {
                // Inicializar listas vacías por defecto
                this.listadoReservas = new BindingList<reservaDTO>();
                this.listadoLocales = new BindingList<SoftResBusiness.LocalWSClient.localDTO>();

                // Cargar reservas del usuario actual
                reservaParametros parametros = new reservaParametros();
                // Note: reservaParametros no tiene idUsuario, se filtra por DNI
                parametros.dniCliente = "12345678"; // TODO: Obtener DNI del usuario actual
                parametros.estadoSpecified = false;
                parametros.tipoReservaSpecified = false;
                parametros.fechaInicioSpecified = false;
                parametros.fechaFinSpecified = false;
                parametros.idLocalSpecified = false;
                
                var reservas = this.reservaBO.Listar(parametros);
                if (reservas != null)
                {
                    this.listadoReservas = reservas;
                }

                // Cargar locales activos
                SoftResBusiness.LocalWSClient.localParametros lParametros = new SoftResBusiness.LocalWSClient.localParametros();
                lParametros.estadoSpecified = true;
                lParametros.estado = true;
                
                var locales = this.localBO.Listar(lParametros);
                if (locales != null)
                {
                    this.listadoLocales = locales;
                }
            }
            catch (Exception ex)
            {
                // Asegurar que las listas estén inicializadas aunque haya error
                this.listadoReservas = this.listadoReservas ?? new BindingList<reservaDTO>();
                this.listadoLocales = this.listadoLocales ?? new BindingList<SoftResBusiness.LocalWSClient.localDTO>();

                // Log error y mostrar mensaje genérico
                System.Diagnostics.Debug.WriteLine($"Error al cargar datos: {ex.Message}");
                CargarReservasDemo(); // Fallback a datos demo
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Cargar datos siempre para mantener la información actualizada
            CargarDatos();
            CargarFiltros();
            CargarReservas();
        }

        private void CargarFiltros()
        {
            try
            {
                // TODO: Los controles de filtro no están definidos en el designer
                // Implementar cuando se agreguen al archivo .aspx
                /*
                // Cargar dropdown de locales
                CargarDropDownListLocales(ddlLocalFiltro);
                
                // Cargar dropdown de estados
                CargarEstados(ddlEstadoFiltro);
                */
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorFiltros",
                    $"console.error('Error al cargar filtros: {ex.Message}');", true);
            }
        }

        private void CargarDropDownListLocales(DropDownList ddl)
        {
            if (ddl == null || this.listadoLocales == null)
            {
                return;
            }

            var localesFiltrados = this.listadoLocales.Select(l => new
            {
                Text = l.nombre ?? "Sin nombre",
                Value = l.idLocalSpecified ? l.idLocal.ToString() : ""
            }).ToList();

            ddl.DataSource = localesFiltrados;
            ddl.DataTextField = "Text";
            ddl.DataValueField = "Value";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("Todos los locales", ""));
        }

        private void CargarEstados(DropDownList ddl)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("Todos los estados", ""));
            ddl.Items.Add(new ListItem("Pendiente", "PENDIENTE"));
            ddl.Items.Add(new ListItem("Confirmada", "CONFIRMADA"));
            ddl.Items.Add(new ListItem("Cancelada", "CANCELADA"));
        }

        private void CargarReservas()
        {
            try
            {
                var reservasFiltradas = AplicarFiltros(this.listadoReservas);
                var reservasParaRepeater = PrepararDatosParaRepeater(reservasFiltradas);
                
                rptReservas.DataSource = reservasParaRepeater ?? new List<object>();
                rptReservas.DataBind();

                // Mostrar mensaje si no hay reservas
                if (reservasParaRepeater == null || !reservasParaRepeater.Any())
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "sinReservas",
                        "mostrarMensajeSinReservas();", true);
                }
            }
            catch (Exception ex)
            {
                // En caso de error, asegurar que el repeater tenga una fuente vacía
                rptReservas.DataSource = new List<object>();
                rptReservas.DataBind();
                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorCarga",
                    $"Swal.fire('Error', 'Error al cargar reservas: {ex.Message}', 'error');", true);
            }
        }

        private List<reservaDTO> AplicarFiltros(BindingList<reservaDTO> reservas)
        {
            if (reservas == null)
            {
                return new List<reservaDTO>();
            }

            var resultado = reservas.AsEnumerable();

            // TODO: Los controles de filtro no están definidos, aplicar filtros cuando estén disponibles
            /*
            // Filtro por fecha desde
            if (!string.IsNullOrEmpty(txtFechaDesde.Text))
            {
                DateTime fechaDesde = DateTime.Parse(txtFechaDesde.Text);
                resultado = resultado.Where(r => r.fecha_HoraSpecified && r.fecha_Hora.Date >= fechaDesde.Date);
            }

            // Filtro por fecha hasta
            if (!string.IsNullOrEmpty(txtFechaHasta.Text))
            {
                DateTime fechaHasta = DateTime.Parse(txtFechaHasta.Text);
                resultado = resultado.Where(r => r.fecha_HoraSpecified && r.fecha_Hora.Date <= fechaHasta.Date);
            }

            // Filtro por local
            if (!string.IsNullOrEmpty(ddlLocalFiltro.SelectedValue))
            {
                int idLocal = int.Parse(ddlLocalFiltro.SelectedValue);
                resultado = resultado.Where(r => r.local != null && r.local.idLocalSpecified && r.local.idLocal == idLocal);
            }

            // Filtro por estado
            if (!string.IsNullOrEmpty(ddlEstadoFiltro.SelectedValue))
            {
                estadoReserva estado = (estadoReserva)Enum.Parse(typeof(estadoReserva), ddlEstadoFiltro.SelectedValue);
                resultado = resultado.Where(r => r.estadoSpecified && r.estado == estado);
            }
            */

            return resultado.OrderByDescending(r => r.fecha_HoraSpecified ? r.fecha_Hora : DateTime.MinValue).ToList();
        }

        private List<object> PrepararDatosParaRepeater(List<reservaDTO> reservas)
        {
            if (reservas == null)
            {
                return new List<object>();
            }

            return reservas.Select(r => new
            {
                ReservaID = r.idReservaSpecified ? r.idReserva : 0,
                Fecha = r.fecha_HoraSpecified ? r.fecha_Hora.ToString("dd/MM/yyyy") : "No especificado",
                Hora = r.fecha_HoraSpecified ? r.fecha_Hora.ToString("HH:mm") : "No especificado",
                Local = r.local?.nombre ?? "No especificado",
                Estado = r.estadoSpecified ? r.estado.ToString() : "No especificado",
                EstadoClass = ObtenerClaseEstado(r.estadoSpecified ? r.estado.ToString() : ""),
                Personas = r.cantidad_personasSpecified ? r.cantidad_personas.ToString() : "No especificado",
                Mesas = r.numeroMesasSpecified ? r.numeroMesas.ToString() : "No especificado",
                Ubicacion = r.tipoMesa?.nombre ?? "No especificado",
                TipoReserva = r.tipoReservaSpecified ? r.tipoReserva.ToString() : "COMUN",
                NombreEvento = r.tipoReservaSpecified && r.tipoReserva == tipoReserva.EVENTO ? r.nombreEvento : null,
                DescripcionEvento = r.tipoReservaSpecified && r.tipoReserva == tipoReserva.EVENTO ? r.descripcionEvento : null,
                Observaciones = r.observaciones ?? "Ninguna",
                PuedeEditar = r.estadoSpecified && (r.estado == estadoReserva.PENDIENTE || r.estado == estadoReserva.CONFIRMADA),
                PuedeConfirmar = r.estadoSpecified && r.estado == estadoReserva.PENDIENTE,
                EstaCancelada = r.estadoSpecified && r.estado == estadoReserva.CANCELADA,
                MotivoCancelacion = r.motivoCancelacion?.descripcion ?? ""
            }).ToList<object>();
        }

        private string ObtenerClaseEstado(string estado)
        {
            if (estado == "PENDIENTE") return "badge bg-warning text-dark";
            else if (estado == "CONFIRMADA") return "badge bg-success";
            else if (estado == "CANCELADA") return "badge bg-danger";
            else return "badge bg-secondary";
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: Implementar validaciones cuando los controles estén disponibles
                /*
                // Validar fechas
                if (!string.IsNullOrEmpty(txtFechaDesde.Text) && !string.IsNullOrEmpty(txtFechaHasta.Text))
                {
                    DateTime fechaDesde = DateTime.Parse(txtFechaDesde.Text);
                    DateTime fechaHasta = DateTime.Parse(txtFechaHasta.Text);
                    
                    if (fechaDesde > fechaHasta)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "errorFecha",
                            "Swal.fire('Error', 'La fecha desde no puede ser mayor que la fecha hasta', 'warning');", true);
                        return;
                    }
                }
                */

                // Recargar datos y aplicar filtros
                CargarDatos();
                CargarReservas();
                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "filtroAplicado",
                    "Swal.fire('Filtros aplicados', 'Se han actualizado las reservas', 'info', { timer: 1500, showConfirmButton: false });", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorFiltro",
                    $"Swal.fire('Error', 'Error al aplicar filtros: {ex.Message}', 'error');", true);
            }
        }

        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            // TODO: Implementar cuando los controles estén disponibles
            /*
            txtFechaDesde.Text = "";
            txtFechaHasta.Text = "";
            ddlLocalFiltro.SelectedIndex = 0;
            ddlEstadoFiltro.SelectedIndex = 0;
            */
            CargarReservas();
        }

        private void CargarReservasDemo()
        {
            try
            {
                // Crear reservas demo para pruebas cuando no hay conexión
                var reservasDemo = new List<object>
                {
                    new {
                        ReservaID = 1001,
                        Fecha = DateTime.Now.AddDays(3).ToString("dd/MM/yyyy"),
                        Hora = "19:00",
                        Local = "Restaurante San Miguel",
                        Estado = "PENDIENTE",
                        EstadoClass = "badge bg-warning text-dark",
                        Personas = "4",
                        Mesas = "1",
                        Ubicacion = "Mesa junto a ventana",
                        TipoReserva = "COMUN",
                        NombreEvento = "",
                        DescripcionEvento = "",
                        Observaciones = "Solicitud especial: mesa cerca de ventana",
                        PuedeEditar = true,
                        PuedeConfirmar = true,
                        EstaCancelada = false,
                        MotivoCancelacion = ""
                    },
                    new {
                        ReservaID = 1002,
                        Fecha = DateTime.Now.AddDays(7).ToString("dd/MM/yyyy"),
                        Hora = "20:00",
                        Local = "Restaurante San Miguel",
                        Estado = "CONFIRMADA",
                        EstadoClass = "badge bg-success",
                        Personas = "6",
                        Mesas = "2",
                        Ubicacion = "Terraza",
                        TipoReserva = "EVENTO",
                        NombreEvento = "Cumpleaños de María",
                        DescripcionEvento = "Celebración de cumpleaños con decoración especial",
                        Observaciones = "Incluir torta de chocolate",
                        PuedeEditar = true,
                        PuedeConfirmar = false,
                        EstaCancelada = false,
                        MotivoCancelacion = ""
                    },
                    new {
                        ReservaID = 1003,
                        Fecha = DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy"),
                        Hora = "18:30",
                        Local = "Restaurante Callao",
                        Estado = "CANCELADA",
                        EstadoClass = "badge bg-danger",
                        Personas = "2",
                        Mesas = "1",
                        Ubicacion = "Interior",
                        TipoReserva = "COMUN",
                        NombreEvento = "",
                        DescripcionEvento = "",
                        Observaciones = "Cena romántica",
                        PuedeEditar = false,
                        PuedeConfirmar = false,
                        EstaCancelada = true,
                        MotivoCancelacion = "Solicitud del cliente"
                    },
                    new {
                        ReservaID = 1004,
                        Fecha = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"),
                        Hora = "13:00",
                        Local = "Restaurante San Miguel",
                        Estado = "PENDIENTE",
                        EstadoClass = "badge bg-warning text-dark",
                        Personas = "8",
                        Mesas = "2",
                        Ubicacion = "Salón privado",
                        TipoReserva = "EVENTO",
                        NombreEvento = "Almuerzo corporativo",
                        DescripcionEvento = "Reunión de trabajo con almuerzo",
                        Observaciones = "Menú ejecutivo para 8 personas",
                        PuedeEditar = true,
                        PuedeConfirmar = true,
                        EstaCancelada = false,
                        MotivoCancelacion = ""
                    }
                };

                rptReservas.DataSource = reservasDemo;
                rptReservas.DataBind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "datosDemo",
                    "Swal.fire('Modo Demo', 'Se están mostrando datos de ejemplo. Verifique la conexión con el servidor.', 'info', { timer: 3000, showConfirmButton: false });", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorDemo",
                    $"Swal.fire('Error', 'Error al cargar datos demo: {ex.Message}', 'error');", true);
            }
        }

        public string GetBotonPorEstado(string estado, string reservaId)
        {
            if (estado == "PENDIENTE")
            {
                return $@"
                    <button class='btn btn-warning rounded-pill px-3 me-2' onclick='confirmarReservaCliente({reservaId})'>
                        ✅ Confirmar
                    </button>
                    <button class='btn btn-outline-primary rounded-pill px-3 me-2' onclick='editarReservaCliente({reservaId})'>
                        ✏️ Editar
                    </button>
                    <button class='btn btn-outline-danger rounded-pill px-3' onclick='cancelarReservaCliente({reservaId})'>
                        ❌ Cancelar
                    </button>";
            }
            else if (estado == "CONFIRMADA")
            {
                return $@"
                    <button class='btn btn-outline-primary rounded-pill px-3 me-2' onclick='editarReservaCliente({reservaId})'>
                        ✏️ Editar
                    </button>
                    <button class='btn btn-outline-danger rounded-pill px-3' onclick='cancelarReservaCliente({reservaId})'>
                        ❌ Cancelar
                    </button>";
            }
            else if (estado == "CANCELADA")
            {
                return "<span class='text-muted'><i class='fas fa-ban me-1'></i>Reserva cancelada</span>";
            }
            else
            {
                return "";
            }
        }
    }
}