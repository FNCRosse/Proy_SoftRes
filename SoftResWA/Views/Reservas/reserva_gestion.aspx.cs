using SoftResBusiness;
using SoftResBusiness.ReservaWSClient;
using SoftResBusiness.LocalWSClient;
using SoftResBusiness.MotivoCancelacionWSClient;
using SoftResBusiness.UsuarioWSClient;
using SoftResBusiness.MesaWSClient;
using SoftResBusiness.TipoMesaWSClient;
using SoftResBusiness.FilaEsperaWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Reservas
{
    public partial class reserva_gestion : System.Web.UI.Page
    {
        private ReservaBO reservaBO;
        private LocalBO localBO;
        private MotivoCancelacionBO motivoBO;
        private UsuarioBO usuarioBO;
        private MesaBO mesaBO;
        private TipoMesaBO tipoMesaBO;
        private FilaEsperaBO filaEsperaBO;
        private BindingList<SoftResBusiness.ReservaWSClient.reservaDTO> listadoReservas;
        private BindingList<SoftResBusiness.LocalWSClient.localDTO> listadoLocales;
        private BindingList<SoftResBusiness.MotivoCancelacionWSClient.motivosCancelacionDTO> listadoMotivos;
        private BindingList<SoftResBusiness.TipoMesaWSClient.tipoMesaDTO> listadoTiposMesa;
        private BindingList<SoftResBusiness.FilaEsperaWSClient.filaEsperaDTO> listadoFilaEspera;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Inicializar objetos de negocio
                this.reservaBO = new ReservaBO();
                this.localBO = new LocalBO();
                this.motivoBO = new MotivoCancelacionBO();
                this.usuarioBO = new UsuarioBO();
                this.mesaBO = new MesaBO();
                this.tipoMesaBO = new TipoMesaBO();
                this.filaEsperaBO = new FilaEsperaBO();

                if (!IsPostBack)
                {
                    // Cargar datos iniciales
                    CargarLocales();
                    CargarDatos();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorCarga",
                    $"Swal.fire('Error', 'Error al cargar datos iniciales: {ex.Message}', 'error');", true);
            }
        }

        private void CargarLocales()
        {
            try
            {
                // Cargar locales
                var localParams = new SoftResBusiness.LocalWSClient.localParametros { estado = true };
                this.listadoLocales = new BindingList<SoftResBusiness.LocalWSClient.localDTO>(
                    this.localBO.Listar(localParams).ToList()
                );

                // Configurar DropDownList
                ddlLocal.Items.Clear();
                ddlLocal.Items.Add(new ListItem("Todos", ""));
                foreach (var local in this.listadoLocales)
                {
                    if (local.idLocalSpecified)
                    {
                        ddlLocal.Items.Add(new ListItem(local.nombre, local.idLocal.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar locales: " + ex.Message);
            }
        }

        private void CargarDatos()
        {
            try
            {
                // Cargar reservas
                var reservaParametros = new SoftResBusiness.ReservaWSClient.reservaParametros();

                // Agregar filtros si están seleccionados
                if (ddlLocal.SelectedValue != "")
                {
                    reservaParametros.idLocal = int.Parse(ddlLocal.SelectedValue);
                    reservaParametros.idLocalSpecified = true;
                }

                if (txtFechaDesde.Text != "")
                {
                    reservaParametros.fechaInicio = DateTime.Parse(txtFechaDesde.Text);
                    reservaParametros.fechaInicioSpecified = true;
                }

                if (txtFechaHasta.Text != "")
                {
                    reservaParametros.fechaFin = DateTime.Parse(txtFechaHasta.Text);
                    reservaParametros.fechaFinSpecified = true;
                }

                if (ddlEstado.SelectedValue != "")
                {
                    reservaParametros.estado = (SoftResBusiness.ReservaWSClient.estadoReserva)
                        Enum.Parse(typeof(SoftResBusiness.ReservaWSClient.estadoReserva), ddlEstado.SelectedValue);
                    reservaParametros.estadoSpecified = true;
                }

                this.listadoReservas = new BindingList<SoftResBusiness.ReservaWSClient.reservaDTO>(
                    this.reservaBO.Listar(reservaParametros).ToList()
                );

                // Actualizar controles
                ActualizarGridViews();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar datos: " + ex.Message);
            }
        }

        private void ActualizarGridViews()
        {
            if (gvReservas != null)
            {
                gvReservas.DataSource = this.listadoReservas;
                gvReservas.DataBind();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                CargarDatos();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorBusqueda",
                    $"Swal.fire('Error', 'Error al buscar reservas: {ex.Message}', 'error');", true);
            }
        }

        protected void btnCrearReserva_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["IdFilaEspera"] == null)
                {
                    throw new Exception("No se encontró la información de la lista de espera.");
                }

                int idFilaEspera = (int)ViewState["IdFilaEspera"];
                var filaEspera = listadoFilaEspera.FirstOrDefault(f => f.idFila == idFilaEspera);
                if (filaEspera == null)
                {
                    throw new Exception("No se encontró el registro en la lista de espera.");
                }

                // Crear nueva reserva
                var nuevaReserva = new SoftResBusiness.ReservaWSClient.reservaDTO
                {
                    tipoReserva = SoftResBusiness.ReservaWSClient.tipoReserva.COMUN,
                    tipoReservaSpecified = true,
                    estado = SoftResBusiness.ReservaWSClient.estadoReserva.PENDIENTE,
                    estadoSpecified = true,
                    fechaCreacion = DateTime.Now,
                    fechaCreacionSpecified = true,
                    usuarioCreacion = Session["UsuarioLogueado"] as string,
                    fecha_Hora = DateTime.Now,
                    fecha_HoraSpecified = true,
                    cantidad_personas = 1,
                    cantidad_personasSpecified = true,
                    local = new SoftResBusiness.ReservaWSClient.localDTO
                    {
                        idLocal = int.Parse(ddlLocal.SelectedValue),
                        idLocalSpecified = true
                    },
                    usuario = new SoftResBusiness.ReservaWSClient.usuariosDTO
                    {
                        idUsuario = filaEspera.usuario.idUsuario,
                        idUsuarioSpecified = true,
                        nombreComp = filaEspera.usuario.nombreComp,
                        email = filaEspera.usuario.email,
                        telefono = filaEspera.usuario.telefono
                    }
                };

                int idReserva = reservaBO.Insertar(nuevaReserva);
                if (idReserva > 0)
                {
                    // Eliminar de la lista de espera
                    filaEsperaBO.Eliminar(new SoftResBusiness.FilaEsperaWSClient.filaEsperaDTO { idFila = idFilaEspera, idFilaSpecified = true });

                    // Actualizar datos y mostrar mensaje de éxito
                    CargarDatos();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "exitoReserva",
                        "Swal.fire('Éxito', 'Reserva creada correctamente', 'success');", true);
                }
                else
                {
                    throw new Exception("No se pudo crear la reserva.");
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorReserva",
                    $"Swal.fire('Error', 'Error al crear la reserva: {ex.Message}', 'error');", true);
            }
        }

        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var reserva = (SoftResBusiness.ReservaWSClient.reservaDTO)e.Row.DataItem;
                if (reserva != null)
                {
                    // Formatear campos
                    if (reserva.tipoReservaSpecified)
                    {
                        e.Row.Cells[2].Text = reserva.tipoReserva == SoftResBusiness.ReservaWSClient.tipoReserva.COMUN ? "Común" : "Evento";
                    }

                    if (reserva.fecha_HoraSpecified)
                    {
                        e.Row.Cells[3].Text = reserva.fecha_Hora.ToString("dd/MM/yyyy");
                        e.Row.Cells[4].Text = reserva.fecha_Hora.ToString("HH:mm");
                    }

                    if (reserva.local != null)
                    {
                        e.Row.Cells[5].Text = reserva.local.nombre ?? "No especificado";
                    }

                    if (reserva.usuario != null)
                    {
                        e.Row.Cells[6].Text = reserva.usuario.nombreComp ?? "No especificado";
                    }

                    if (reserva.estadoSpecified)
                    {
                        e.Row.Cells[7].Text = reserva.estado.ToString();
                        switch (reserva.estado)
                        {
                            case SoftResBusiness.ReservaWSClient.estadoReserva.CONFIRMADA:
                                e.Row.Cells[7].CssClass = "text-success";
                                break;
                            case SoftResBusiness.ReservaWSClient.estadoReserva.PENDIENTE:
                                e.Row.Cells[7].CssClass = "text-warning";
                                break;
                            case SoftResBusiness.ReservaWSClient.estadoReserva.CANCELADA:
                                e.Row.Cells[7].CssClass = "text-danger";
                                break;
                        }
                    }

                    // Configurar botones
                    Button btnEditar = (Button)e.Row.FindControl("btnEditar");
                    Button btnCancelar = (Button)e.Row.FindControl("btnCancelar");

                    if (btnEditar != null && btnCancelar != null)
                    {
                        // Solo mostrar botones según el estado
                        if (reserva.estadoSpecified && reserva.estado == SoftResBusiness.ReservaWSClient.estadoReserva.CONFIRMADA)
                        {
                            btnEditar.Visible = true;
                            btnCancelar.Visible = true;
                        }
                        else
                        {
                            btnEditar.Visible = false;
                            btnCancelar.Visible = false;
                        }
                    }
                }
            }
        }

        protected void gvListaEspera_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "CrearReserva")
                {
                    int idFilaEspera = Convert.ToInt32(e.CommandArgument);
                    ViewState["IdFilaEspera"] = idFilaEspera;
                    btnCrearReserva_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorComando",
                    $"Swal.fire('Error', 'Error al procesar el comando: {ex.Message}', 'error');", true);
            }
        }
    }
}