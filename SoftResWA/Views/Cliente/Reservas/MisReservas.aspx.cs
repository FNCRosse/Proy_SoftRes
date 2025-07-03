using SoftResBusiness;
using SoftResBusiness.LocalWSClient;
using SoftResBusiness.ReservaWSClient;
using SoftResBusiness.UsuarioWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using usuariosDTO = SoftResBusiness.UsuarioWSClient.usuariosDTO;

namespace SoftResWA.Views.Cliente.Reservas
{
    public partial class MisReservas : System.Web.UI.Page
    {
        private ReservaBO reservaBO;
        private LocalBO localBO;
        private MotivoCancelacionBO motivoCancelacionBO;
        public usuariosDTO UsuarioActual => Session["UsuarioLogueado"] as usuariosDTO;
        public MisReservas()
        {
            reservaBO = new ReservaBO();
            localBO = new LocalBO();
            motivoCancelacionBO = new MotivoCancelacionBO();
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
                CargarFiltros();
                BindGrid();
            }
        }
        private void CargarFiltros()
        {
            try
            {
                // Cargar Locales
                localParametros lParametros = new localParametros();
                lParametros.estadoSpecified = true;
                lParametros.estado = true;
                ddlLocal.DataSource = localBO.Listar(lParametros);
                ddlLocal.DataTextField = "nombre";
                ddlLocal.DataValueField = "idLocal";
                ddlLocal.DataBind();
                ddlLocal.Items.Insert(0, new ListItem("Todos", ""));

                // Cargar Estados
                ddlEstado.DataSource = Enum.GetValues(typeof(estadoReserva));
                ddlEstado.DataBind();
                ddlEstado.Items.Insert(0, new ListItem("Todos", ""));

                // Cargar Motivos de Cancelación para el modal
                ddlMotivoCancelacion.DataSource = motivoCancelacionBO.Listar();
                ddlMotivoCancelacion.DataTextField = "descripcion";
                ddlMotivoCancelacion.DataValueField = "idMotivo";
                ddlMotivoCancelacion.DataBind();
                ddlMotivoCancelacion.Items.Insert(0, new ListItem("Selecciona un motivo...", "0"));
            }
            catch (Exception ex) { MostrarAlerta("Error", "No se pudieron cargar los filtros.", "error"); }
        }
        private void BindGrid()
        {
            try
            {
                var parametros = new reservaParametros();
                parametros.dniCliente = UsuarioActual.numeroDocumento;

                if (!string.IsNullOrEmpty(txtFechaDesde.Text)) { parametros.fechaInicio = DateTime.Parse(txtFechaDesde.Text); parametros.fechaInicioSpecified = true; }
                if (!string.IsNullOrEmpty(txtFechaHasta.Text)) { parametros.fechaFin = DateTime.Parse(txtFechaHasta.Text); parametros.fechaFinSpecified = true; }
                if (!string.IsNullOrEmpty(ddlLocal.SelectedValue)) { parametros.idLocal = int.Parse(ddlLocal.SelectedValue); parametros.idLocalSpecified = true; }
                if (!string.IsNullOrEmpty(ddlEstado.SelectedValue)) { parametros.estado = (estadoReserva)Enum.Parse(typeof(estadoReserva), ddlEstado.SelectedValue); parametros.estadoSpecified = true; }

                BindingList<reservaDTO> reservas = reservaBO.Listar(parametros);
                rptReservas.DataSource = reservas.OrderByDescending(r => r.fechaHoraRegistro).ToList();
                rptReservas.DataBind();
            }
            catch (Exception ex) { MostrarAlerta("Error", "No se pudieron cargar tus reservas.", "error"); }
        }

        #region Lógica de UI y Eventos
        protected void btnBuscar_Click(object sender, EventArgs e) => BindGrid();
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFechaDesde.Text = "";
            txtFechaHasta.Text = "";
            ddlLocal.SelectedIndex = 0;
            ddlEstado.SelectedIndex = 0;
            BindGrid();
        }
        protected void rptReservas_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            int idReserva;
            if (!int.TryParse(e.CommandArgument.ToString(), out idReserva)) return;

            // Obtenemos una referencia al botón que fue clickeado
            Control botonClickeado = e.CommandSource as Control;
            reservaDTO reserva = reservaBO.ObtenerPorID(idReserva);
            if (reserva == null) return;
            if (!PuedeModificarCancelar(reserva.fechaHoraRegistro))
            {
                MostrarAlerta("Acción no permitida", "No puedes modificar o cancelar una reserva con menos de una hora de anticipación.", "warning");
                return; // Detenemos la ejecución
            }
            switch (e.CommandName)
            {
                case "Editar":
                    // Obtenemos los datos frescos de la reserva para poblar el modal
                    reservaDTO reservaEditar = reservaBO.ObtenerPorID(idReserva);
                    if (reservaEditar == null) return;

                    hdnReservaIdEditar.Value = idReserva.ToString();
                    btnCancelarEnModal.CommandArgument = idReserva.ToString();
                    txtPersonasModal.Text = reservaEditar.cantidadPersonas.ToString();
                    txtMesasModal.Text = reservaEditar.numeroMesas.ToString();
                    txtObservacionesModal.Text = reservaEditar.observaciones;

                    if (reservaEditar.tipoReserva == tipoReserva.EVENTO)
                    {
                        // Si es un evento, muestra el panel y llena los campos
                        pnlCamposEvento.Visible = true;
                        txtNombreEventoModal.Text = reservaEditar.nombreEvento;
                        txtDescripcionEventoModal.Text = reservaEditar.descripcionEvento;
                    }
                    else
                    {
                        // Si no es un evento, asegúrate de que el panel esté oculto
                        pnlCamposEvento.Visible = false;
                        txtNombreEventoModal.Text = ""; // Limpiar por si acaso
                        txtDescripcionEventoModal.Text = "";
                    }

                    // Forzamos la actualización del panel del modal
                    updModalEditar.Update();

                    // Registramos el script para abrir el modal
                    ScriptManager.RegisterStartupScript(botonClickeado, botonClickeado.GetType(), "abrirModalEditar", "abrirModal('modalEditarReserva');", true);
                    break;

                case "Confirmar":
                    int resultado = reservaBO.ConfirmarReserva(idReserva, UsuarioActual.idUsuario);

                    if (resultado > 0)
                    {
                        MostrarAlerta("¡Éxito!", "Tu reserva ha sido confirmada.", "success");
                    }
                    else
                    {
                        MostrarAlerta("Error", "No se pudo confirmar la reserva. Por favor, inténtalo de nuevo.", "error");
                    }
                    BindGrid(); // Refrescar la vista
                    break;

                case "AbrirCancelar":
                    hdnReservaIdCancelar.Value = idReserva.ToString();
                    btnConfirmarCancelacion.CommandArgument = idReserva.ToString();
                    // Registramos el script para abrir el modal
                    ScriptManager.RegisterStartupScript(botonClickeado, botonClickeado.GetType(), "abrirModalCancelar", "abrirModal('modalCancelarReserva');", true);
                    break;
            }
        }

        protected void btnAccionReserva_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Guardar":
                    GuardarCambiosReserva();
                    break;
                case "ConfirmarCancelar":
                    if (!string.IsNullOrEmpty(e.CommandArgument?.ToString()))
                    {
                        int idReserva = int.Parse(e.CommandArgument.ToString());
                        CancelarReserva(idReserva); // Llama al método que contiene la lógica
                    }
                    break;
                case "AbrirCancelarDesdeModal":
                    // Toma el ID del CommandArgument del botón
                    hdnReservaIdCancelar.Value = e.CommandArgument.ToString();
                    btnConfirmarCancelacion.CommandArgument = e.CommandArgument.ToString();
                    ScriptManager.RegisterStartupScript(this, GetType(), "abrirModalCancelar", "abrirModal('modalCancelarReserva');", true);
                    break;
            }
        }

        private void GuardarCambiosReserva()
        {
            int idReserva = int.Parse(hdnReservaIdEditar.Value);
            reservaDTO reserva = reservaBO.ObtenerPorID(idReserva);
            if (!PuedeModificarCancelar(reserva.fechaHoraRegistro))
            {
                MostrarAlerta("Acción no permitida", "El tiempo para modificar esta reserva ha expirado.", "error");
                return;
            }
            reserva.idReserva = idReserva;
            reserva.idReservaSpecified = true;
            reserva.cantidadPersonas = int.Parse(txtPersonasModal.Text);
            reserva.numeroMesas = int.Parse(txtMesasModal.Text);
            reserva.observaciones = txtObservacionesModal.Text;
            if (reserva.tipoReserva == tipoReserva.EVENTO)
            {
                reserva.nombreEvento = txtNombreEventoModal.Text.Trim();
                reserva.descripcionEvento = txtDescripcionEventoModal.Text.Trim();
            }
            reserva.usuarioModificacion = UsuarioActual.nombreComp;
            reserva.fechaModificacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            reserva.fechaModificacionSpecified = true;
            reserva.filaEspera = null;

            if (reservaBO.Modificar(reserva) > 0)
            {
                MostrarAlerta("¡Éxito!", "Tu reserva ha sido modificada correctamente.", "success");
                // Aquí podrías añadir la lógica para enviar el correo de notificación.
            }
            else
            {
                MostrarAlerta("Error", "No se pudo modificar la reserva.", "error");
            }
            BindGrid();
        }

        // Ahora el método recibe el ID como parámetro
        private void CancelarReserva(int idReserva)
        {
            Page.Validate("CancelarGroup"); 
            if (!Page.IsValid) return;
            try
            {
                reservaDTO reservaOriginal = reservaBO.ObtenerPorID(idReserva);
                if (reservaOriginal == null)
                {
                    MostrarAlerta("Error", "La reserva no fue encontrada.", "error");
                    return;
                }
                if (!PuedeModificarCancelar(reservaOriginal.fechaHoraRegistro))
                {
                    MostrarAlerta("Acción no permitida", "El tiempo para cancelar esta reserva ha expirado.", "error");
                    return;
                }
                reservaOriginal.idReserva = idReserva;
                reservaOriginal.idReservaSpecified= true;
                reservaOriginal.usuario = new SoftResBusiness.ReservaWSClient.usuariosDTO();
                reservaOriginal.usuario.idUsuario = UsuarioActual.idUsuario;
                reservaOriginal.usuario.idUsuarioSpecified = true;
                reservaOriginal.motivoCancelacion = new motivosCancelacionDTO();
                reservaOriginal.motivoCancelacion.idMotivo = int.Parse(ddlMotivoCancelacion.SelectedValue);
                reservaOriginal.motivoCancelacion.idMotivoSpecified = true;
                reservaOriginal.usuarioModificacion = UsuarioActual.nombreComp;

                int resultado = reservaBO.Cancelar(reservaOriginal);

                if (resultado > 0)
                {
                    MostrarAlerta("Reserva Cancelada", "Tu reserva ha sido cancelada exitosamente.", "info");
                    // La lógica de liberar mesas y enviar correo ahora está en el backend.
                }
                else
                {
                    MostrarAlerta("Error", "No se pudo cancelar la reserva.", "error");
                }
                BindGrid(); // Refrescar la tabla
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error Inesperado", $"Ocurrió un error: {ex.Message}", "error");
            }
        }

        #endregion

        #region Funciones Auxiliares para la Vista

        public bool PuedeModificarCancelar(object fechaReservaObj)
        {
            // Si el objeto de fecha es nulo, consideramos que no se puede modificar.
            if (fechaReservaObj == null || fechaReservaObj == DBNull.Value)
            {
                return false;
            }

            DateTime fechaReserva = (DateTime)fechaReservaObj;
            // Si la fecha ya pasó, no se puede modificar.
            if (fechaReserva < DateTime.Now)
            {
                return false;
            }

            // Si faltan más de 1 hora para la reserva, se puede modificar.
            return (fechaReserva - DateTime.Now).TotalHours > 1;
        }

        public string GetEstadoCssClass(object estadoObj)
        {
            estadoReserva estado = (estadoReserva)estadoObj;
            switch (estado)
            {
                case estadoReserva.CONFIRMADA: return "badge bg-success";
                case estadoReserva.PENDIENTE: return "badge bg-warning text-dark";
                case estadoReserva.CANCELADA: return "badge bg-danger";
                default: return "badge bg-secondary";
            }
        }



        private void MostrarAlerta(string titulo, string mensaje, string tipo)
        {
            string script = $"Swal.fire('{titulo}', '{Server.HtmlEncode(mensaje)}', '{tipo}');";
            ScriptManager.RegisterStartupScript(this, GetType(), $"alerta_{Guid.NewGuid()}", script, true);
        }

        #endregion
    }
}