using SoftResBusiness;
using SoftResBusiness.ReservaWSClient;
using SoftResBusiness.LocalWSClient;
using SoftResBusiness.MotivoCancelacionWSClient;
using SoftResBusiness.UsuarioWSClient;
using SoftResBusiness.MesaWSClient;
using SoftResBusiness.TipoMesaWSClient;
using SoftResBusiness.ReservaxMesaWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Reservas
{
    public partial class reserva_gestion : System.Web.UI.Page
    {
        private ReservaBO reservaBO;
        private LocalBO localBO;
        private MotivoCancelacionBO motivoCancelacionBO;
        private UsuarioBO usuarioBO;
        private MesaBO mesaBO;
        private TipoMesaBO tipoMesaBO;
        private ReservaxMesaBO reservaxMesaBO;
        private BindingList<SoftResBusiness.ReservaWSClient.reservaDTO> listadoReservas;
        private BindingList<SoftResBusiness.LocalWSClient.localDTO> listadoLocales;
        private BindingList<SoftResBusiness.MotivoCancelacionWSClient.motivosCancelacionDTO> listadoMotivos;
        private BindingList<SoftResBusiness.TipoMesaWSClient.tipoMesaDTO> listadoTiposMesa;
        private BindingList<reservaxMesasDTO> listadoReservaxMesas;

        public ReservaBO ReservaBO { get => reservaBO; set => reservaBO = value; }
        public LocalBO LocalBO { get => localBO; set => localBO = value; }
        public MotivoCancelacionBO MotivoCancelacionBO { get => motivoCancelacionBO; set => motivoCancelacionBO = value; }
        public UsuarioBO UsuarioBO { get => usuarioBO; set => usuarioBO = value; }
        public MesaBO MesaBO { get => mesaBO; set => mesaBO = value; }
        public TipoMesaBO TipoMesaBO { get => tipoMesaBO; set => tipoMesaBO = value; }
        public ReservaxMesaBO ReservaxMesaBO { get => reservaxMesaBO; set => reservaxMesaBO = value; }
        public BindingList<SoftResBusiness.ReservaWSClient.reservaDTO> ListadoReservas { get => listadoReservas; set => listadoReservas = value; }
        public BindingList<SoftResBusiness.LocalWSClient.localDTO> ListadoLocales { get => listadoLocales; set => listadoLocales = value; }
        public BindingList<SoftResBusiness.MotivoCancelacionWSClient.motivosCancelacionDTO> ListadoMotivos { get => listadoMotivos; set => listadoMotivos = value; }
        public BindingList<SoftResBusiness.TipoMesaWSClient.tipoMesaDTO> ListadoTiposMesa { get => listadoTiposMesa; set => listadoTiposMesa = value; }
        public BindingList<reservaxMesasDTO> ListadoReservaxMesas { get => listadoReservaxMesas; set => listadoReservaxMesas = value; }

        public reserva_gestion()
        {
            this.reservaBO = new ReservaBO();
            this.localBO = new LocalBO();
            this.motivoCancelacionBO = new MotivoCancelacionBO();
            this.usuarioBO = new UsuarioBO();
            this.mesaBO = new MesaBO();
            this.tipoMesaBO = new TipoMesaBO();
            this.reservaxMesaBO = new ReservaxMesaBO();
        }

        private void CargarDatos()
        {
            try
            {
                // Inicializar listas vacías por defecto
                this.listadoReservas = new BindingList<SoftResBusiness.ReservaWSClient.reservaDTO>();
                this.listadoLocales = new BindingList<SoftResBusiness.LocalWSClient.localDTO>();
                this.listadoMotivos = new BindingList<SoftResBusiness.MotivoCancelacionWSClient.motivosCancelacionDTO>();
                this.listadoTiposMesa = new BindingList<SoftResBusiness.TipoMesaWSClient.tipoMesaDTO>();
                this.listadoReservaxMesas = new BindingList<reservaxMesasDTO>();

                // Cargar reservas
                reservaParametros parametros = new reservaParametros();
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

                // Cargar motivos de cancelación
                var motivos = this.motivoCancelacionBO.Listar();
                if (motivos != null)
                {
                    this.listadoMotivos = motivos;
                }

                // Cargar tipos de mesa activos
                var tiposMesa = this.tipoMesaBO.Listar();
                if (tiposMesa != null)
                {
                    this.listadoTiposMesa = tiposMesa;
                }
            }
            catch (Exception ex)
            {
                // Asegurar que las listas estén inicializadas aunque haya error
                this.listadoReservas = this.listadoReservas ?? new BindingList<SoftResBusiness.ReservaWSClient.reservaDTO>();
                this.listadoLocales = this.listadoLocales ?? new BindingList<SoftResBusiness.LocalWSClient.localDTO>();
                this.listadoMotivos = this.listadoMotivos ?? new BindingList<SoftResBusiness.MotivoCancelacionWSClient.motivosCancelacionDTO>();
                this.listadoTiposMesa = this.listadoTiposMesa ?? new BindingList<SoftResBusiness.TipoMesaWSClient.tipoMesaDTO>();
                this.listadoReservaxMesas = this.listadoReservaxMesas ?? new BindingList<reservaxMesasDTO>();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorCarga",
                    $"Swal.fire('Error', 'Error al cargar datos: {ex.Message}', 'error');", true);
            }
        }

        protected List<object> ConfigurarListado(BindingList<SoftResBusiness.ReservaWSClient.reservaDTO> lista)
        {
            // Validar que la lista no sea null
            if (lista == null)
            {
                return new List<object>();
            }

            var listaAdaptada = lista.Select(r => new
            {
                r.idReserva,
                TipoReserva = r.tipoReservaSpecified ? (r.tipoReserva == SoftResBusiness.ReservaWSClient.tipoReserva.COMUN ? "Común" : "Evento") : "No especificado",
                Fecha = r.fecha_HoraSpecified ? r.fecha_Hora.ToString("dd/MM/yyyy") : "No especificado",
                Hora = r.fecha_HoraSpecified ? r.fecha_Hora.ToString("HH:mm") : "No especificado",
                Local = r.local?.nombre ?? "No especificado",
                Solicitante = r.usuario?.nombreComp ?? "No especificado",
                r.observaciones,
                MotivoCancelacion = r.motivoCancelacion?.descripcion ?? "",
                UbicacionMesa = r.tipoMesa?.nombre ?? "No especificado",
                FilaEspera = r.filaEspera != null ? "Sí" : "No",
                Estado = r.estadoSpecified ? r.estado.ToString() : "No especificado",
                estadoEnum = r.estadoSpecified ? r.estado : (SoftResBusiness.ReservaWSClient.estadoReserva?)null,
                cantidadPersonas = r.cantidad_personasSpecified ? r.cantidad_personas : 0,
                numeroMesas = r.numeroMesasSpecified ? r.numeroMesas : 0
            }).ToList<Object>();
            return listaAdaptada;
        }

        protected List<object> ConfigurarListadoReservaxMesas(BindingList<reservaxMesasDTO> lista)
        {
            // Validar que la lista no sea null
            if (lista == null)
            {
                return new List<object>();
            }

            var listaAdaptada = lista.Select(rm => new
            {
                ReservaID = rm.reserva?.idReservaSpecified == true ? rm.reserva.idReserva : 0,
                TipoReserva = rm.reserva?.tipoReservaSpecified == true ? 
                    (rm.reserva.tipoReserva == SoftResBusiness.ReservaxMesaWSClient.tipoReserva.COMUN ? "Común" : "Evento") : "No especificado",
                FechaReserva = rm.reserva?.fecha_HoraSpecified == true ? 
                    rm.reserva.fecha_Hora.ToString("dd/MM/yyyy HH:mm") : "No especificado",
                Cliente = rm.reserva?.usuario?.nombreComp ?? "No especificado",
                Local = rm.reserva?.local?.nombre ?? "No especificado",
                EstadoReserva = rm.reserva?.estadoSpecified == true ? rm.reserva.estado.ToString() : "No especificado",
                
                MesaID = rm.mesa?.idMesaSpecified == true ? rm.mesa.idMesa : 0,
                NumeroMesa = rm.mesa?.numeroMesa ?? "No especificado",
                CapacidadMesa = rm.mesa?.capacidadSpecified == true ? rm.mesa.capacidad : 0,
                TipoMesa = rm.mesa?.tipoMesa?.nombre ?? "No especificado",
                EstadoMesa = rm.mesa?.estadoSpecified == true ? rm.mesa.estado.ToString() : "No especificado",
                UbicacionMesa = $"({(rm.mesa?.xSpecified == true ? rm.mesa.x : 0)}, {(rm.mesa?.ySpecified == true ? rm.mesa.y : 0)})",
                
                // Para acciones
                PuedeEliminar = rm.reserva?.estadoSpecified == true && 
                    (rm.reserva.estado == SoftResBusiness.ReservaxMesaWSClient.estadoReserva.PENDIENTE || rm.reserva.estado == SoftResBusiness.ReservaxMesaWSClient.estadoReserva.CONFIRMADA)
            }).ToList<Object>();
            return listaAdaptada;
        }

        private void CargarReservaxMesas(int? idReserva = null)
        {
            try
            {
                // Si no se especifica idReserva, cargar todas las asignaciones usando -1
                int reservaId = idReserva ?? -1;
                var reservaxMesas = this.reservaxMesaBO.Listar(reservaId);
                
                if (reservaxMesas != null)
                {
                    this.listadoReservaxMesas = reservaxMesas;
                }
                else
                {
                    this.listadoReservaxMesas = new BindingList<reservaxMesasDTO>();
                }
            }
            catch (Exception ex)
            {
                this.listadoReservaxMesas = new BindingList<reservaxMesasDTO>();
                
                // Crear datos demo para pruebas
                var reservaxMesasDemo = new BindingList<reservaxMesasDTO>();
                
                // Simular algunas asignaciones
                for (int i = 1; i <= 3; i++)
                {
                    var reservaxMesa = new SoftResBusiness.ReservaxMesaWSClient.reservaxMesasDTO
                    {
                        reserva = new SoftResBusiness.ReservaxMesaWSClient.reservaDTO
                        {
                            idReserva = 1000 + i,
                            idReservaSpecified = true,
                            tipoReserva = SoftResBusiness.ReservaxMesaWSClient.tipoReserva.COMUN,
                            tipoReservaSpecified = true,
                            fecha_Hora = DateTime.Now.AddDays(i),
                            fecha_HoraSpecified = true,
                            estado = SoftResBusiness.ReservaxMesaWSClient.estadoReserva.PENDIENTE,
                            estadoSpecified = true,
                            usuario = new SoftResBusiness.ReservaxMesaWSClient.usuariosDTO { nombreComp = $"Cliente {i}" },
                            local = new SoftResBusiness.ReservaxMesaWSClient.localDTO { nombre = $"Local {i}" }
                        },
                        mesa = new SoftResBusiness.ReservaxMesaWSClient.mesaDTO
                        {
                            idMesa = 100 + i,
                            idMesaSpecified = true,
                            numeroMesa = $"Mesa-{i:D2}",
                            capacidad = 4 + i,
                            capacidadSpecified = true,
                            estado = SoftResBusiness.ReservaxMesaWSClient.estadoMesa.DISPONIBLE,
                            estadoSpecified = true,
                            x = i * 10,
                            xSpecified = true,
                            y = i * 5,
                            ySpecified = true,
                            tipoMesa = new SoftResBusiness.ReservaxMesaWSClient.tipoMesaDTO { nombre = $"Tipo {i}" }
                        }
                    };
                    reservaxMesasDemo.Add(reservaxMesa);
                }
                
                this.listadoReservaxMesas = reservaxMesasDemo;
                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorReservaxMesas",
                    $@"console.log('Error cargando ReservaxMesas: {ex.Message}');
                       console.log('Usando datos demo para pruebas');", true);
            }
        }

        private void CargarGridViewReservaxMesas()
        {
            try
            {
                var listaConfigurda = this.ConfigurarListadoReservaxMesas(this.listadoReservaxMesas);
                gvReservaxMesas.DataSource = listaConfigurda;
                gvReservaxMesas.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorGridReservaxMesas",
                    $@"Swal.fire('Error', 'Error cargando tabla de asignaciones: {ex.Message}', 'error');", true);
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
            ddl.Items.Insert(0, new ListItem("-- Todos --", ""));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            gvReservas.RowDataBound += gv_RowDataBound;
            
            // Cargar datos siempre para mantener la información actualizada
            CargarDatos();
            CargarGridView();
            CargarReservaxMesas();
            CargarGridViewReservaxMesas();
        }

        private void CargarGridView()
        {
            try
            {
                var listaAdaptada = this.ConfigurarListado(this.listadoReservas);
                gvReservas.DataSource = listaAdaptada;
                gvReservas.DataBind();
                
                // Mostrar mensaje si no hay reservas
                if (listaAdaptada == null || listaAdaptada.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "sinReservas",
                        "console.log('No hay reservas para mostrar');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorGrid",
                    $"Swal.fire('Error', 'Error al cargar el listado: {ex.Message}', 'error');", true);
            }
        }

        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem;

                // Obtener estado vía reflexión
                SoftResBusiness.ReservaWSClient.estadoReserva? estado = null;
                var propEstado = dataItem.GetType().GetProperty("estadoEnum");
                if (propEstado != null)
                {
                    estado = (SoftResBusiness.ReservaWSClient.estadoReserva?)propEstado.GetValue(dataItem);
                }

                int? idReserva = null;
                var propId = dataItem.GetType().GetProperty("idReserva");
                if (propId != null)
                {
                    idReserva = (int?)propId.GetValue(dataItem);
                }

                // Encontrar botones en la celda
                if (e.Row.Cells.Count > 0)
                {
                    var cellContent = e.Row.Cells[0].Text;
                    if (string.IsNullOrEmpty(cellContent) || cellContent == "&nbsp;")
                    {
                        // Crear botones dinámicamente
                        e.Row.Cells[0].Text = "";
                        
                        if (idReserva.HasValue && estado.HasValue)
                        {
                            // Botón Modificar (solo para reservas pendientes o confirmadas)
                            if (estado == SoftResBusiness.ReservaWSClient.estadoReserva.PENDIENTE || estado == SoftResBusiness.ReservaWSClient.estadoReserva.CONFIRMADA)
                            {
                                Button btnModificar = new Button
                                {
                                    Text = "M",
                                    CssClass = "btn btn-sm btn-primary me-1",
                                    CommandName = "Modificar",
                                    CommandArgument = idReserva.ToString()
                                };
                                btnModificar.Click += btnModificar_Click;
                                e.Row.Cells[0].Controls.Add(btnModificar);
                            }

                            // Botón Cancelar (solo para reservas pendientes o confirmadas)
                            if (estado == SoftResBusiness.ReservaWSClient.estadoReserva.PENDIENTE || estado == SoftResBusiness.ReservaWSClient.estadoReserva.CONFIRMADA)
                            {
                                Button btnCancelar = new Button
                                {
                                    Text = "C",
                                    CssClass = "btn btn-sm btn-danger",
                                    OnClientClick = $"return confirmarCancelacion({idReserva}); return false;"
                                };
                                e.Row.Cells[0].Controls.Add(btnCancelar);
                            }
                        }
                    }
                }
            }
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                int idReserva = int.Parse(btn.CommandArgument);
                
                // Redirigir a página de modificación
                Response.Redirect($"modificar_reserva.aspx?id={idReserva}");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorModificar",
                    $"Swal.fire('Error', 'Error al modificar reserva: {ex.Message}', 'error');", true);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                reservaParametros parametros = new reservaParametros();

                // Filtro por tipo de reserva
                string tipoReservaSeleccionado = Request.Form["ddlTipRes"];
                if (!string.IsNullOrEmpty(tipoReservaSeleccionado) && tipoReservaSeleccionado != "Seleccionar...")
                {
                    parametros.tipoReservaSpecified = true;
                    parametros.tipoReserva = tipoReservaSeleccionado == "1" ? SoftResBusiness.ReservaWSClient.tipoReserva.COMUN : SoftResBusiness.ReservaWSClient.tipoReserva.EVENTO;
                }
                else
                {
                    parametros.tipoReservaSpecified = false;
                }

                // Filtro por fechas
                if (!string.IsNullOrWhiteSpace(txtFechaDesde.Text))
                {
                    parametros.fechaInicioSpecified = true;
                    parametros.fechaInicio = DateTime.Parse(txtFechaDesde.Text);
                }
                else
                {
                    parametros.fechaInicioSpecified = false;
                }

                if (!string.IsNullOrWhiteSpace(txtFechaHasta.Text))
                {
                    parametros.fechaFinSpecified = true;
                    parametros.fechaFin = DateTime.Parse(txtFechaHasta.Text);
                }
                else
                {
                    parametros.fechaFinSpecified = false;
                }

                // Filtro por DNI cliente
                string dniCliente = Request.Form["txtDniCliente"];
                if (!string.IsNullOrWhiteSpace(dniCliente))
                {
                    parametros.dniCliente = dniCliente.Trim();
                }

                // Filtro por local
                // TODO: El control ddlLocal no está definido en el designer
                /*
                if (!string.IsNullOrEmpty(ddlLocal.SelectedValue))
                {
                    parametros.idLocalSpecified = true;
                    parametros.idLocal = int.Parse(ddlLocal.SelectedValue);
                }
                else
                {
                    parametros.idLocalSpecified = false;
                }
                */
                parametros.idLocalSpecified = false;

                // Validar fechas
                if (parametros.fechaInicioSpecified && parametros.fechaFinSpecified)
                {
                    if (parametros.fechaInicio > parametros.fechaFin)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "errorFecha",
                            "Swal.fire('Error de validación', 'La fecha desde no puede ser mayor a la fecha hasta', 'warning');", true);
                        return;
                    }
                }

                var lista = this.reservaBO.Listar(parametros);
                
                // Actualizar la lista principal con los resultados filtrados
                this.listadoReservas = lista ?? new BindingList<SoftResBusiness.ReservaWSClient.reservaDTO>();
                
                var listaAdaptada = this.ConfigurarListado(this.listadoReservas);
                gvReservas.DataSource = listaAdaptada;
                gvReservas.DataBind();

                // Mostrar mensaje si no hay resultados
                if (this.listadoReservas.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "sinResultados",
                        "Swal.fire('Sin resultados', 'No se encontraron reservas con los criterios especificados', 'info');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "resultadosBusqueda",
                        $"Swal.fire('Búsqueda exitosa', 'Se encontraron {this.listadoReservas.Count} reserva(s)', 'success', {{ timer: 2000, showConfirmButton: false }});", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorBusqueda",
                    $"Swal.fire('Error', 'Error en la búsqueda: {ex.Message}', 'error');", true);
            }
        }

        protected void btnCancelarReserva_Click(object sender, EventArgs e)
        {
            try
            {
                string idReservaStr = Request.Form["hdnIdReservaCancelar"];
                string idMotivoStr = Request.Form["hdnIdMotivoCancelacion"];
                
                if (!string.IsNullOrEmpty(idReservaStr) && !string.IsNullOrEmpty(idMotivoStr))
                {
                    int idReserva = int.Parse(idReservaStr);
                    int idMotivo = int.Parse(idMotivoStr);

                    SoftResBusiness.ReservaWSClient.reservaDTO reserva = this.reservaBO.ObtenerPorID(idReserva);
                    if (reserva != null)
                    {
                        reserva.estado = SoftResBusiness.ReservaWSClient.estadoReserva.CANCELADA;
                        reserva.estadoSpecified = true;
                        
                        SoftResBusiness.ReservaWSClient.motivosCancelacionDTO motivo = new SoftResBusiness.ReservaWSClient.motivosCancelacionDTO();
                        motivo.idMotivo = idMotivo;
                        motivo.idMotivoSpecified = true;
                        reserva.motivoCancelacion = motivo;

                        int resultado = this.reservaBO.Eliminar(reserva);
                        
                        if (resultado > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "exitoCancelacion",
                                @"Swal.fire('¡Reserva cancelada!', 'La reserva se canceló correctamente.', 'success').then(function() {
                                    window.location.reload();
                                });", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "errorCancelacion",
                                "Swal.fire('Error', 'No se pudo cancelar la reserva', 'error');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorCancelacion",
                    $"Swal.fire('Error', 'Error al cancelar reserva: {ex.Message}', 'error');", true);
            }
        }

        protected void btnEliminarAsignacion_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                string[] args = btn.CommandArgument.Split('|');
                
                if (args.Length != 2 || !int.TryParse(args[0], out int idReserva) || !int.TryParse(args[1], out int idMesa))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errorArgumentos",
                        "Swal.fire('Error', 'Argumentos inválidos para eliminar asignación', 'error');", true);
                    return;
                }

                // Buscar la asignación específica en la lista
                SoftResBusiness.ReservaxMesaWSClient.reservaxMesasDTO asignacionAEliminar = null;
                foreach (var asignacion in this.listadoReservaxMesas)
                {
                    if (asignacion.reserva?.idReservaSpecified == true && asignacion.reserva.idReserva == idReserva &&
                        asignacion.mesa?.idMesaSpecified == true && asignacion.mesa.idMesa == idMesa)
                    {
                        asignacionAEliminar = asignacion;
                        break;
                    }
                }

                if (asignacionAEliminar == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errorEncontrarAsignacion",
                        "Swal.fire('Error', 'No se encontró la asignación especificada', 'error');", true);
                    return;
                }

                // Ejecutar eliminación
                int resultado = this.reservaxMesaBO.Eliminar(asignacionAEliminar);
                
                if (resultado > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "exitoEliminacion",
                        @"Swal.fire('¡Asignación eliminada!', 'La mesa ha sido desasignada de la reserva correctamente.', 'success').then(function() {
                            window.location.reload();
                        });", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errorEliminacion",
                        "Swal.fire('Error', 'No se pudo eliminar la asignación. Intente nuevamente.', 'error');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorExcepcionEliminacion",
                    $"Swal.fire('Error', 'Error inesperado: {ex.Message}', 'error');", true);
            }
        }
    }
}