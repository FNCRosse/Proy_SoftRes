using SoftResBusiness;
using SoftResBusiness.LocalWSClient;
using SoftResBusiness.MotivoCancelacionWSClient;
using SoftResBusiness.ReservaWSClient;
using SoftResBusiness.UsuarioWSClient;
using System;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using usuariosDTO = SoftResBusiness.UsuarioWSClient.usuariosDTO;

namespace SoftResWA.Views.Reservas
{
    public partial class reserva_gestion : System.Web.UI.Page
    {
        private readonly ReservaBO reservaBO;
        private readonly LocalBO localBO;
        private readonly MotivoCancelacionBO motivoCancelacionBO;

        // Propiedad para acceder al usuario de la sesión de forma segura
        public usuariosDTO UsuarioActual => Session["UsuarioLogueado"] as usuariosDTO;

        public reserva_gestion()
        {
            reservaBO = new ReservaBO();
            localBO = new LocalBO();
            motivoCancelacionBO = new MotivoCancelacionBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Seguridad: Si no hay sesión, o si el usuario es un cliente, redirigir.
            if (UsuarioActual == null || UsuarioActual.rol.esCliente)
            {
                Response.Redirect("~/Views/Login/Login.aspx"); // O a una página de acceso denegado
                return;
            }

            if (!IsPostBack)
            {
                CargarFiltros();
                BindGrid();
            }
        }

        #region Carga de Datos y Grid

        private void CargarFiltros()
        {
            try
            {
                // Cargar Locales activos
                ddlLocal.DataSource = localBO.Listar(new localParametros { estado = true, estadoSpecified = true });
                ddlLocal.DataTextField = "nombre";
                ddlLocal.DataValueField = "idLocal";
                ddlLocal.DataBind();
                ddlLocal.Items.Insert(0, new ListItem("Todos los locales", ""));

                // Cargar Estados de reserva
                ddlEstado.DataSource = Enum.GetValues(typeof(estadoReserva));
                ddlEstado.DataBind();
                ddlEstado.Items.Insert(0, new ListItem("Todos los estados", ""));

                // Cargar Motivos de Cancelación para el modal
                ddlMotivoCancelacion.DataSource = motivoCancelacionBO.Listar();
                ddlMotivoCancelacion.DataTextField = "descripcion";
                ddlMotivoCancelacion.DataValueField = "idMotivo";
                ddlMotivoCancelacion.DataBind();
                ddlMotivoCancelacion.Items.Insert(0, new ListItem("Selecciona un motivo...", "0"));
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error de Carga", "No se pudieron cargar los filtros.", "error");
            }
        }

        private void BindGrid()
        {
            try
            {
                var parametros = new reservaParametros();

                // Aplicar filtros de la UI
                if (!string.IsNullOrWhiteSpace(txtDniCliente.Text)) { parametros.dniCliente = txtDniCliente.Text.Trim(); }
                if (!string.IsNullOrEmpty(txtFechaDesde.Text)) { parametros.fechaInicio = DateTime.Parse(txtFechaDesde.Text); parametros.fechaInicioSpecified = true; }
                if (!string.IsNullOrEmpty(txtFechaHasta.Text)) { parametros.fechaFin = DateTime.Parse(txtFechaHasta.Text); parametros.fechaFinSpecified = true; }
                if (!string.IsNullOrEmpty(ddlLocal.SelectedValue)) { parametros.idLocal = int.Parse(ddlLocal.SelectedValue); parametros.idLocalSpecified = true; }
                if (!string.IsNullOrEmpty(ddlEstado.SelectedValue)) { parametros.estado = (estadoReserva)Enum.Parse(typeof(estadoReserva), ddlEstado.SelectedValue); parametros.estadoSpecified = true; }

                BindingList<reservaDTO> reservas = reservaBO.Listar(parametros);
                gvReservas.DataSource = reservas?.OrderByDescending(r => r.fechaHoraRegistro).ToList();
                gvReservas.DataBind();
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error", "No se pudieron cargar las reservas.", "error");
            }
        }
        #endregion

        #region Eventos de Botones y Grid

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            gvReservas.PageIndex = 0; // Volver a la primera página al buscar
            BindGrid();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtDniCliente.Text = "";
            txtFechaDesde.Text = "";
            txtFechaHasta.Text = "";
            ddlLocal.SelectedIndex = 0;
            ddlEstado.SelectedIndex = 0;
            gvReservas.PageIndex = 0;
            BindGrid();
        }

        protected void gvReservas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReservas.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvReservas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!int.TryParse(e.CommandArgument.ToString(), out int idReserva)) return;

            // Re-validar permiso de tiempo aquí es una buena práctica de seguridad
            reservaDTO reserva = reservaBO.ObtenerPorID(idReserva);
            if (reserva != null && !PuedeModificarCancelar(reserva.fechaHoraRegistro) && (e.CommandName == "Asignar" || e.CommandName == "AbrirCancelar"))
            {
                MostrarAlerta("Acción no permitida", "Esta reserva ya no puede ser gestionada por su proximidad.", "warning");
                return;
            }

            switch (e.CommandName)
            {
                case "VerDetalle":
                    // Aquí podrías guardar el ID y abrir un modal de detalle o redirigir
                    Response.Redirect($"detalle_reserva.aspx?id={idReserva}");
                    break;
                case "Asignar":
                    // Lógica para redirigir a la página de asignación de mesas
                    Response.Redirect($"asignar_mesas.aspx?id={idReserva}");
                    break;
                case "AbrirCancelar":
                    // Guardamos el ID en el CommandArgument del botón final de confirmación
                    btnConfirmarCancelacion.CommandArgument = idReserva.ToString();
                    ddlMotivoCancelacion.SelectedIndex = 0; // Limpiar selección previa
                    ScriptManager.RegisterStartupScript(this, GetType(), "abrirModalCancelar", "abrirModal('modalCancelarReserva');", true);
                    break;
            }
        }

        protected void btnConfirmarCancelacion_Command(object sender, CommandEventArgs e)
        {
            Page.Validate("CancelarGroup");
            if (!Page.IsValid)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "reabrirModalCancelar", "abrirModal('modalCancelarReserva');", true);
                return;
            }

            if (!int.TryParse(e.CommandArgument.ToString(), out int idReserva)) return;

            try
            {
                var reservaParaCancelar = this.reservaBO.ObtenerPorID(idReserva);
                reservaParaCancelar.idReserva = idReserva;
                reservaParaCancelar.idReservaSpecified = true;
                reservaParaCancelar.usuario = new SoftResBusiness.ReservaWSClient.usuariosDTO();
                reservaParaCancelar.usuario.idUsuario = UsuarioActual.idUsuario;
                reservaParaCancelar.usuario.idUsuarioSpecified = true;
                reservaParaCancelar.motivoCancelacion = new SoftResBusiness.ReservaWSClient.motivosCancelacionDTO();
                reservaParaCancelar.motivoCancelacion.idMotivo = int.Parse(ddlMotivoCancelacion.SelectedValue);
                reservaParaCancelar.motivoCancelacion.idMotivoSpecified = true;
                reservaParaCancelar.usuarioModificacion = UsuarioActual.nombreComp;

                if (reservaBO.Cancelar(reservaParaCancelar) > 0)
                {
                    MostrarAlerta("Reserva Cancelada", "La reserva ha sido cancelada exitosamente.", "success");
                    BindGrid();
                }
                else
                {
                    MostrarAlerta("Error", "No se pudo procesar la cancelación.", "error");
                }
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error Inesperado", ex.Message, "error");
            }
        }

        #endregion

        #region Funciones Auxiliares para la Vista

        public string GetEstadoCssClass(object estadoObj)
        {
            if (estadoObj == null) return "badge bg-secondary";
            var estado = (estadoReserva)Enum.Parse(typeof(estadoReserva), estadoObj.ToString());
            switch (estado)
            {
                case estadoReserva.CONFIRMADA: return "badge bg-success";
                case estadoReserva.PENDIENTE: return "badge bg-warning text-dark";
                case estadoReserva.CANCELADA: return "badge bg-danger";
                default: return "badge bg-secondary";
            }
        }

        public bool PuedeModificarCancelar(object fechaReservaObj)
        {
            if (fechaReservaObj == null || fechaReservaObj == DBNull.Value) return false;
            DateTime fechaReserva = (DateTime)fechaReservaObj;
            return (fechaReserva - DateTime.Now).TotalHours > 1;
        }

        private void MostrarAlerta(string titulo, string mensaje, string tipo)
        {
            string script = $"Swal.fire('{titulo}', '{Server.HtmlEncode(mensaje)}', '{tipo}');";
            ScriptManager.RegisterStartupScript(this, GetType(), $"alerta_{Guid.NewGuid()}", script, true);
        }

        #endregion
    }
}