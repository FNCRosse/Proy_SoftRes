using SoftResBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.ComponentModel;
using SoftResBusiness.SedeWSClient;

namespace SoftResWA.Views.Sedes
{
    public partial class SedeGestion : System.Web.UI.Page
    {
        private SedeBO sedeBO;
        private BindingList<sedeDTO> listadoSedes;

        public SedeBO SedeBO { get => sedeBO; set => sedeBO = value; }
        public BindingList<sedeDTO> ListadoSedes { get => listadoSedes; set => listadoSedes = value; }

        public SedeGestion()
        {
            this.sedeBO = new SedeBO();
            sedeParametros parametros = new sedeParametros();
            parametros.nombre = null;
            parametros.estado = true;
            this.listadoSedes = this.sedeBO.Listar(parametros);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dgvSede.DataSource = ListadoSedes;
                dgvSede.DataBind();

                //Esto es provicional en lo que termina los demas
                var horariosOriginales = new List<HorarioFake>
                    {
                        new HorarioFake { idHorario = 1, diaSemana = "Lunes", horaInicio = "09:00", horaFin = "18:00" },
                        new HorarioFake { idHorario = 2, diaSemana = "Sábado", horaInicio = "10:00", horaFin = "14:00" }
                    };

                var horarios = horariosOriginales.Select(h => new
                {
                    idHorario = h.idHorario,
                    descripcion = $"{h.diaSemana} - {h.horaInicio} a {h.horaFin}"
                }).ToList();

                ddlHorarios.DataSource = horarios;
                ddlHorarios.DataTextField = "descripcion";
                ddlHorarios.DataValueField = "idHorario";
                ddlHorarios.DataBind();
                ddlHorarios.Items.Insert(0, new ListItem("-- Seleccione un horario --", ""));

            }
        }


        protected void btnAñadirHorario_Click(object sender, EventArgs e)
        {
            // Aquí irá la lógica para añadir el horario

        }
        protected void btnGuardarSede_Click(object sender, EventArgs e)
        {
            // Aquí irá la lógica para guardar la sede
            ScriptManager.RegisterStartupScript(this, this.GetType(), "registroExitoso", "Swal.fire('¡Sede registrada!', 'El registro se completó correctamente.', 'success');", true);

        }
        protected void ddlHorarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlHorarios.SelectedValue))
            {
                int idHorario = int.Parse(ddlHorarios.SelectedValue);

                // Simulación de búsqueda real
                var horario = new HorarioFake
                {
                    idHorario = idHorario,
                    diaSemana = "Lunes",
                    horaInicio = "09:00",
                    horaFin = "18:00",
                    esFeriado = false
                };

                lblDiaSemana.Text = horario.diaSemana;
                lblHoraInicio.Text = horario.horaInicio;
                lblHoraFin.Text = horario.horaFin;
                lblFeriado.Text = horario.esFeriado ? "Sí" : "No";
                string titulo = hdnModoModal.Value == "modificar" ? "Modificar Sede" : "Registrar Sede";
                string script = "setTimeout(function() {" +
                    $"document.getElementById('tituloModalSede').innerHTML = '<i class=\\\"fas fa-map-marker-alt me-2 text-danger\\\"></i>{titulo}';" +
                    "var modal = new bootstrap.Modal(document.getElementById('modalRegistrarSede'));" +
                    "modal.show();" +
                "}, 200);";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalHorario", script, true);
            }
        }

        protected void btnNuevoSede_Click(object sender, EventArgs e)
        {
            // Limpia campos
            txtNombreSede.Text = "";
            txtDistritoSede.Text = "";
            hdnIdSede.Value = "";

            // Cambia título a "Registrar"
            hdnModoModal.Value = "registrar";
            string script = "setTimeout(function() {" +
                "document.getElementById('tituloModalSede').innerHTML = '<i class=\\\"fas fa-map-marker-alt me-2 text-danger\\\"></i>Registrar Sede';" +
                "var modal = new bootstrap.Modal(document.getElementById('modalRegistrarSede'));" +
                "modal.show();" +
            "}, 200);";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModalNuevo", script, true);
        }

        protected void btnModificar_Command(object sender, CommandEventArgs e)
        {
            int idSede = int.Parse(e.CommandArgument.ToString());

            //var sede = sedeBO.ObtenerPorId(idSede);

            //txtNombreSede.Text = sede.Nombre;
            //txtDistritoSede.Text = sede.Distrito;
            //hdnIdSede.Value = idSede.ToString();

            hdnModoModal.Value = "modificar";
            string script = "setTimeout(function() {" +
                "document.getElementById('tituloModalSede').innerHTML = '<i class=\\\"fas fa-map-marker-alt me-2 text-danger\\\"></i>Modificar Sede';" +
                "var modal = new bootstrap.Modal(document.getElementById('modalRegistrarSede'));" +
                "modal.show();" +
            "}, 200);";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalModificar", script, true);
        }
        protected void btn_eliminar_Click(object sender, EventArgs e)
        {
            int id = int.Parse(hdnIdEliminar.Value);

            // Eliminar por tipo de entidad según la página
            // Por ejemplo: eliminar sede, local, usuario...

            ScriptManager.RegisterStartupScript(this, this.GetType(), "eliminado",
                "Swal.fire('¡Eliminado!', 'El registro fue eliminado correctamente.', 'success');", true);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //var parametros = new SedeParametros
            //{
            //    Nombre = txtNombre.Text.Trim(),
            //    Estado = string.IsNullOrEmpty(ddlEstado.SelectedValue) ? null : (int?)int.Parse(ddlEstado.SelectedValue)
            //};

            //// Llama a tu BO
            //var listaFiltrada = sedeBO.Listar(parametros); // Simulado

            //// Recarga el GridView
            //dgvSede.DataSource = listaFiltrada;
            //dgvSede.DataBind();
        }

    }
}