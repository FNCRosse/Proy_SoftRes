using SoftResBusiness;
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
                    CargarListaEspera();
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar datos iniciales", ex);
            }
        }

        private void CargarLocales()
        {
            try
            {
                // Cargar locales
                var localParams = new SoftResBusiness.LocalWSClient.localParametros { estado = true };
                var locales = this.localBO.Listar(localParams);
                this.listadoLocales = new BindingList<SoftResBusiness.LocalWSClient.localDTO>(
                    locales != null ? locales.ToList() : new List<SoftResBusiness.LocalWSClient.localDTO>()
                );

                // Configurar DropDownList
                ddlLocal.Items.Clear();
                ddlLocal.Items.Add(new ListItem("Todos", ""));
                foreach (var local in this.listadoLocales.Where(l => l.idLocalSpecified))
                {
                    ddlLocal.Items.Add(new ListItem(local.nombre, local.idLocal.ToString()));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar locales: " + ex.Message);
            }
        }

        private void CargarListaEspera()
        {
            try
            {
                // Cargar lista de espera
                var filaEsperaParams = new SoftResBusiness.FilaEsperaWSClient.filaEsperaParametros
                {
                    estado = SoftResBusiness.FilaEsperaWSClient.estadoFilaEspera.PENDIENTE
                };

                // Aplicar filtro por local si está seleccionado
                if (!string.IsNullOrEmpty(ddlLocal.SelectedValue))
                {
                    filaEsperaParams.idUsuario = int.Parse(ddlLocal.SelectedValue);
                }

                // Usar el método Listar ya que es el único disponible
                var filaEspera = this.filaEsperaBO.Listar(filaEsperaParams);

                this.listadoFilaEspera = new BindingList<SoftResBusiness.FilaEsperaWSClient.filaEsperaDTO>(
                    filaEspera != null ? filaEspera.ToList() : new List<SoftResBusiness.FilaEsperaWSClient.filaEsperaDTO>()
                );

                // Actualizar contador y grid
                if (lblEsperaCount != null)
                {
                    lblEsperaCount.Text = this.listadoFilaEspera.Count.ToString();
                }

                if (gvListaEspera != null)
                {
                    gvListaEspera.DataSource = this.listadoFilaEspera;
                    gvListaEspera.DataBind();
                }

                // Actualizar UpdatePanel
                if (upListaEspera != null)
                {
                    upListaEspera.Update();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar lista de espera: " + ex.Message);
            }
        }

        private void CargarDatos()
        {
            try
            {
                // Cargar reservas
                var reservaParametros = new SoftResBusiness.ReservaWSClient.reservaParametros();

                // Agregar filtros si están seleccionados
                if (!string.IsNullOrEmpty(ddlLocal.SelectedValue))
                {
                    reservaParametros.idLocal = int.Parse(ddlLocal.SelectedValue);
                    reservaParametros.idLocalSpecified = true;
                }

                if (!string.IsNullOrEmpty(txtFechaDesde.Text))
                {
                    reservaParametros.fechaInicio = DateTime.Parse(txtFechaDesde.Text);
                    reservaParametros.fechaInicioSpecified = true;
                }

                if (!string.IsNullOrEmpty(txtFechaHasta.Text))
                {
                    reservaParametros.fechaFin = DateTime.Parse(txtFechaHasta.Text);
                    reservaParametros.fechaFinSpecified = true;
                }

                if (!string.IsNullOrEmpty(ddlEstado.SelectedValue))
                {
                    reservaParametros.estado = (SoftResBusiness.ReservaWSClient.estadoReserva)
                        Enum.Parse(typeof(SoftResBusiness.ReservaWSClient.estadoReserva), ddlEstado.SelectedValue);
                    reservaParametros.estadoSpecified = true;
                }

                // Obtener reservas del servicio
                var reservas = this.reservaBO.Listar(reservaParametros);
                if (reservas != null)
                {
                    this.listadoReservas = new BindingList<SoftResBusiness.ReservaWSClient.reservaDTO>(reservas.ToList());
                }
                else
                {
                    this.listadoReservas = new BindingList<SoftResBusiness.ReservaWSClient.reservaDTO>();
                }

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
                MostrarError("Error al buscar reservas", ex);
            }
        }

        protected void ddlLocal_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Recargar ambas listas con el nuevo filtro de local
                CargarDatos();
                CargarListaEspera();
                
                // Actualizar ambos UpdatePanels
                if (upReservas != null)
                {
                    upReservas.Update();
                }
                if (upListaEspera != null)
                {
                    upListaEspera.Update();
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al filtrar por local", ex);
            }
        }

        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Obtener el objeto de reserva
                var reserva = e.Row.DataItem as SoftResBusiness.ReservaWSClient.reservaDTO;
                if (reserva != null)
                {
                    // Aplicar estilos según el estado
                    string estado = reserva.estado.ToString();
                    switch (estado)
                    {
                        case "CONFIRMADA":
                            e.Row.CssClass = "table-success";
                            break;
                        case "PENDIENTE":
                            e.Row.CssClass = "table-warning";
                            break;
                        case "CANCELADA":
                            e.Row.CssClass = "table-danger";
                            break;
                    }

                    // Manejar visibilidad de botones
                    Button btnEditar = e.Row.FindControl("btnEditar") as Button;
                    Button btnCancelar = e.Row.FindControl("btnCancelar") as Button;

                    if (btnEditar != null && btnCancelar != null)
                    {
                        btnEditar.Visible = estado != "CANCELADA";
                        btnCancelar.Visible = estado != "CANCELADA";
                    }
                }
            }
        }

        protected void gvReservas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int idReserva = Convert.ToInt32(e.CommandArgument);

                switch (e.CommandName)
                {
                    case "EditarReserva":
                        Response.Redirect($"~/Views/Reservas/registrar_reserva_comun.aspx?id={idReserva}");
                        break;

                    case "CancelarReserva":
                        hdnIdReservaCancelar.Value = idReserva.ToString();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal",
                            "$('#modalCancelarReserva').modal('show');", true);
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al procesar comando", ex);
            }
        }

        protected void gvListaEspera_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int idFila = Convert.ToInt32(e.CommandArgument);
                var filaEspera = this.listadoFilaEspera.FirstOrDefault(f => f.idFila == idFila);

                if (filaEspera == null)
                {
                    throw new Exception("No se encontró la entrada en la lista de espera");
                }

                switch (e.CommandName)
                {
                    case "CrearReserva":
                        // Crear nueva reserva desde la fila de espera
                        var nuevaReserva = new SoftResBusiness.ReservaWSClient.reservaDTO
                        {
                            usuario = new SoftResBusiness.ReservaWSClient.usuariosDTO
                            {
                                idUsuario = filaEspera.usuario.idUsuario,
                                idUsuarioSpecified = true,
                                nombreComp = filaEspera.usuario.nombreComp,
                                email = filaEspera.usuario.email,
                                telefono = filaEspera.usuario.telefono
                            },
                            estado = SoftResBusiness.ReservaWSClient.estadoReserva.PENDIENTE,
                            estadoSpecified = true
                        };

                        int idReserva = this.reservaBO.Insertar(nuevaReserva);
                        if (idReserva > 0)
                        {
                            // Eliminar de la lista de espera
                            this.filaEsperaBO.Eliminar(filaEspera);
                            MostrarExito("Reserva creada exitosamente");
                            
                            // Recargar datos
                            CargarDatos();
                            CargarListaEspera();
                        }
                        break;

                    case "EliminarEspera":
                        if (this.filaEsperaBO.Eliminar(filaEspera) > 0)
                        {
                            MostrarExito("Entrada eliminada de la lista de espera");
                            CargarListaEspera();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al procesar la acción de la lista de espera", ex);
            }
        }

        protected void btnCancelarReserva_Click(object sender, EventArgs e)
        {
            try
            {
                int idReserva = Convert.ToInt32(hdnIdReservaCancelar.Value);
                int idMotivo = Convert.ToInt32(hdnIdMotivoCancelacion.Value);

                var reserva = this.listadoReservas.FirstOrDefault(r => r.idReserva == idReserva);
                if (reserva != null)
                {
                    reserva.estado = SoftResBusiness.ReservaWSClient.estadoReserva.CANCELADA;
                    reserva.estadoSpecified = true;
                    reserva.motivoCancelacion = new SoftResBusiness.ReservaWSClient.motivosCancelacionDTO 
                    { 
                        idMotivo = idMotivo,
                        idMotivoSpecified = true
                    };
                    reserva.fechaModificacion = DateTime.Now;
                    reserva.fechaModificacionSpecified = true;
                    reserva.usuarioModificacion = Session["UsuarioLogueado"]?.ToString();

                    int resultado = this.reservaBO.Modificar(reserva);
                    if (resultado > 0)
                    {
                        CargarDatos();
                        MostrarExito("La reserva ha sido cancelada exitosamente");
                    }
                    else
                    {
                        throw new Exception("No se pudo cancelar la reserva");
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al cancelar reserva", ex);
            }
        }

        private void MostrarError(string titulo, Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                $"Swal.fire('{titulo}', '{ex.Message}', 'error');", true);
        }

        private void MostrarExito(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "exito",
                $"Swal.fire('¡Éxito!', '{mensaje}', 'success');", true);
        }
    }
}