using SoftResBusiness;
using SoftResBusiness.HorarioAtencionWSClient;
using SoftResBusiness.LocalWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Horarios
{
    public partial class horario_gestion : System.Web.UI.Page
    {
        private HorarioAtencionBO horarioAtencionBO;
        private BindingList<horarioAtencionDTO> listadoHorarios;

        public HorarioAtencionBO HorarioAtencionBO { get => horarioAtencionBO; set => horarioAtencionBO = value; }
        public BindingList<horarioAtencionDTO> ListadoHorarios { get => listadoHorarios; set => listadoHorarios = value; }
        public horario_gestion()
        {
            this.horarioAtencionBO = new HorarioAtencionBO();
            horarioParametros parametros = new horarioParametros();
            parametros.diaSemanaSpecified = false;
            parametros.esFeriadoSpecified = false;
            parametros.estadoSpecified = false;
            this.listadoHorarios = this.HorarioAtencionBO.Listar(parametros);
        }
        protected List<object> ConfigurarListado(BindingList<horarioAtencionDTO> lista)
        {
            var listaAdaptada = lista.Select(l => new
            {
                l.idHorario,
                l.diaSemana,
                l.horaInicioStr,
                l.horaFinStr,
                esFeriado = l.esFeriado ? "SI" : "NO",
                l.fechaCreacion,
                l.usuarioCreacion,
                fechaModificacion = l.fechaModificacionSpecified ? l.fechaModificacion : (DateTime?)null,
                l.usuarioModificacion,
                estadoBool = l.estado,
                Estado = l.estado ? "Activo" : "Inactivo"
            }).ToList<Object>();
            return listaAdaptada;
        }
        private void MostrarModal(string modo, string titulo)
        {
            hdnModoModal.Value = modo;

            string script = "setTimeout(function() {" +
                            $"document.getElementById('tituloModal').innerHTML = '<i class=\\\"fas fa-map-marker-alt me-2 text-danger\\\"></i>{titulo}';" +
                            "var modal = new bootstrap.Modal(document.getElementById('modalRegistrarHorario'));" +
                            "modal.show();" +
                            "}, 200);";

            ScriptManager.RegisterStartupScript(this, this.GetType(), $"mostrarModal_{modo}", script, true);
        }
        private void CargarDropDownList(DropDownList ddl, object dataSource, string textField, string valueField, string textoDefault)
        {
            ddl.DataSource = dataSource;
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(textoDefault, ""));
        }
        private void CargarDiasSemana()
        {
            var diasSemana = Enum.GetNames(typeof(SoftResBusiness.HorarioAtencionWSClient.diaSemana))
                .Select(d => new { nombre = d, id = d }) 
                .ToList();

            this.CargarDropDownList(ddlDiaSemana, diasSemana, "nombre", "id", "Seleccionar...");
            this.CargarDropDownList(ddlDia, diasSemana, "nombre", "id", "Seleccionar...");
        }
        private void MostrarResultado(bool exito, string entidad, string modo)
        {
            string accion = (modo == "modificar") ? "modificado" :
                            (modo == "eliminar") ? "eliminado" : "registrado";

            string accionNo = (modo == "modificar") ? "modificar" :
                              (modo == "eliminar") ? "eliminar" : "registrar";

            string baseMensaje = exito ? $"El {accion}" : $"El {accionNo} NO";
            string tipo = exito ? "success" : "warning";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje",
                $"Swal.fire('¡{entidad} {(exito ? accion : $"NO {accion}")}!', '{baseMensaje} se completó correctamente.', '{tipo}');", true);
        }
        protected void dgv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem;

                // Obtener estadoBool vía reflexión
                bool estado = false;
                var prop = dataItem.GetType().GetProperty("estadoBool");
                if (prop != null)
                {
                    estado = (bool)prop.GetValue(dataItem);
                }

                int idLocal = (int)dataItem.GetType().GetProperty("idHorario")?.GetValue(dataItem);

                LinkButton btnModificar = (LinkButton)e.Row.FindControl("btnModificar");
                LinkButton btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");

                btnModificar.Visible = estado;
                btnEliminar.Visible = estado;

                if (estado)
                {
                    btnEliminar.OnClientClick = $"return confirmarEliminacion({idLocal}, '{hdnIdEliminar.ClientID}', '{btnEliminarHorario.ClientID}');";
                }
            }
        }
        private horarioAtencionDTO ConstruirDTO(horarioAtencionDTO horario)
        {
            if (horario == null)
                horario = new horarioAtencionDTO();
            horario.diaSemana = (diaSemana)Enum.Parse(typeof(diaSemana), ddlDiaSemana.SelectedValue);
            horario.diaSemanaSpecified = true;
            horario.horaInicioStr = txtHoraInicio.Text.Trim();
            horario.horaFinStr = txtHoraFin.Text.Trim();
            horario.esFeriado = rbFeriadoSi.Checked;
            horario.esFeriadoSpecified = true;
            horario.estado = true;
            horario.estadoSpecified = true;
            return horario;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var listaAdaptada = this.ConfigurarListado(ListadoHorarios);

                dgvHorario.DataSource = listaAdaptada;
                dgvHorario.DataBind();
                CargarDiasSemana();
            }
        }

        protected void btnGuardarHorario_Click(object sender, EventArgs e)
        {
            string modo = hdnModoModal.Value;
            bool exito = false;
            if (modo == "registrar")
            {
                horarioAtencionDTO horario = new horarioAtencionDTO();
                horario = ConstruirDTO(horario);
                horario.fechaCreacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                horario.fechaCreacionSpecified = true;
                horario.fechaModificacionSpecified = false;
                horario.usuarioCreacion = "admin"; // usar Session["usuario"] si aplica

                exito = this.horarioAtencionBO.Insertar(horario) > 0;
            }
            else if (modo == "modificar")
            {
                int id = int.Parse(hdnIdHorario.Value);
                horarioAtencionDTO horario = this.horarioAtencionBO.ObtenerPorID(id);

                horario = ConstruirDTO(horario); // actualiza campos pero mantiene ID, creación, etc.
                horario.idHorario = id;
                horario.idHorarioSpecified = true;

                horario.fechaModificacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                horario.fechaModificacionSpecified = true;
                horario.usuarioModificacion = "admin"; // usar Session["usuario"] si aplica

                exito = this.horarioAtencionBO.Modificar(horario) > 0;
            }
            MostrarResultado(exito, "Horario", modo);
            if (exito) btnBuscar_Click(sender, e);
        }
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            // Limpia campos
            ddlDiaSemana.Text = "";
            txtHoraInicio.Text = "";
            txtHoraFin.Text = "";
            rbFeriadoSi.Text = "";
            rbFeriadoNo.Text = "";

            // Cambia título a "Registrar"
            this.MostrarModal("registrar", "Registrar Horario");
        }
        protected void btnModificar_Command(object sender, CommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            if (id > 0)
            {
                horarioAtencionDTO horario = this.horarioAtencionBO.ObtenerPorID(id);
                if (horario != null)
                {
                    hdnIdHorario.Value = horario.idHorario.ToString();
                    ddlDiaSemana.SelectedValue = horario.diaSemana.ToString();
                    txtHoraInicio.Text = horario.horaInicioStr;
                    txtHoraFin.Text = horario.horaFinStr;
                    rbFeriadoSi.Checked = horario.esFeriado;
                    rbFeriadoNo.Checked = !horario.esFeriado;
                    this.MostrarModal("modificar", "Modificar Horario");
                }
            }
        }
        protected void btn_eliminar_Click(object sender, EventArgs e)
        {
            int id = int.Parse(hdnIdEliminar.Value);
            if (id > 0)
            {
                horarioAtencionDTO horario = this.horarioAtencionBO.ObtenerPorID(id);
                if (horario != null)
                {
                    horario.idHorario = id;
                    horario.idHorarioSpecified = true;
                    bool exito = this.horarioAtencionBO.Eliminar(horario) > 0;
                    MostrarResultado(exito, "Horario", "eliminar");
                    if (exito) btnBuscar_Click(sender, e);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            horarioParametros parametros = new horarioParametros();
            parametros.estadoSpecified = !string.IsNullOrEmpty(ddlEstado.SelectedValue);
            parametros.estado = ddlEstado.SelectedValue == "1";
            parametros.esFeriadoSpecified = !string.IsNullOrEmpty(ddlFeriado.SelectedValue);
            if (parametros.esFeriadoSpecified)
                parametros.esFeriado = ddlFeriado.SelectedValue == "1";
            parametros.diaSemanaSpecified = !string.IsNullOrEmpty(ddlDia.SelectedValue);
            if (parametros.diaSemanaSpecified)
                parametros.diaSemana = (SoftResBusiness.HorarioAtencionWSClient.diaSemana)Enum.Parse(typeof(SoftResBusiness.HorarioAtencionWSClient.diaSemana), ddlDia.SelectedValue);

            var lista = this.horarioAtencionBO.Listar(parametros);
            var listaAdaptada = this.ConfigurarListado(lista);
            dgvHorario.DataSource = listaAdaptada;
            dgvHorario.DataBind();
        }
    }
}