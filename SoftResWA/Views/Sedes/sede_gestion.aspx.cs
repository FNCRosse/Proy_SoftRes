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
using SoftResBusiness.HorarioAtencionWSClient;
using SoftResBusiness.HorarioxSedeWSClient;
using sedeDTO = SoftResBusiness.SedeWSClient.sedeDTO;
using horarioAtencionDTO = SoftResBusiness.HorarioAtencionWSClient.horarioAtencionDTO;

namespace SoftResWA.Views.Sedes
{
    public partial class SedeGestion : System.Web.UI.Page
    {
        private SedeBO sedeBO;
        private HorarioAtencionBO horarioAtencionBO;
        private HorarioxSedeBO horarioxSedeBO;
        private BindingList<horarioAtencionDTO> listadoHorarios;
        private BindingList<horarioAtencionDTO> listarHorarioxSede;
        private BindingList<sedeDTO> listadoSedes;

        public SedeBO SedeBO { get => sedeBO; set => sedeBO = value; }
        public HorarioAtencionBO HorarioAtencionBO { get => horarioAtencionBO; set => horarioAtencionBO = value; }
        public HorarioxSedeBO HorarioxSedeBO { get => horarioxSedeBO; set => horarioxSedeBO = value; }
        public BindingList<horarioAtencionDTO> ListadoHorarios { get => listadoHorarios; set => listadoHorarios = value; }
        public BindingList<horarioAtencionDTO> ListarHorarioxSede { get => listarHorarioxSede; set => listarHorarioxSede = value; }
        public BindingList<sedeDTO> ListadoSedes { get => listadoSedes; set => listadoSedes = value; }

        public SedeGestion()
        {
            this.SedeBO = new SedeBO();
            this.horarioAtencionBO = new HorarioAtencionBO();
            this.horarioxSedeBO = new HorarioxSedeBO();

            sedeParametros parametros = new sedeParametros();
            parametros.nombre = null;
            parametros.estadoSpecified = false;
            this.ListadoSedes = this.SedeBO.Listar(parametros);

            horarioParametros hParametros = new horarioParametros();
            hParametros.diaSemanaSpecified = false;
            hParametros.esFeriadoSpecified = false;
            hParametros.estadoSpecified = true;
            hParametros.estado = true;
            this.ListadoHorarios = this.horarioAtencionBO.Listar(hParametros);
        }
        protected List<object> ConfigurarListado(BindingList<sedeDTO> lista)
        {
            var listaAdaptada = lista.Select(l => new
            {
                l.idSede,
                l.nombre,
                l.distrito,
                l.horarios,
                l.fechaCreacion,
                l.usuarioCreacion,
                fechaModificacion = l.fechaModificacionSpecified ? l.fechaModificacion : (DateTime?)null,
                l.usuarioModificacion,
                estadoBool = l.estado,
                Estado = l.estado ? "Activo" : "Inactivo"
            }).ToList<Object>();
            return listaAdaptada;
        }
        protected List<object> ConfigurarListado(BindingList<horarioAtencionDTO> lista)
        {
            var listaAdaptada = lista.Select(l => new
            {
                l.idHorario,
                descripcion = $"{l.diaSemana} - {l.horaInicioStr} a {l.horaFinStr}",
            }).ToList<Object>();
            return listaAdaptada;
        }
        private void CargarDropDownList(DropDownList ddl, object dataSource, string textField, string valueField, string textoDefault)
        {
            ddl.DataSource = dataSource;
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(textoDefault, ""));
        }
        private void MostrarModal(string modo, string titulo)
        {
            hdnModoModal.Value = modo;

            string script = "setTimeout(function() {" +
                            $"document.getElementById('tituloModal').innerHTML = '<i class=\\\"fas fa-map-marker-alt me-2 text-danger\\\"></i>{titulo}';" +
                            "var modal = new bootstrap.Modal(document.getElementById('modalRegistrarSede'));" +
                            "modal.show();" +
                            "}, 200);";

            ScriptManager.RegisterStartupScript(this, this.GetType(), $"mostrarModal_{modo}", script, true);
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

                int idSede = (int)dataItem.GetType().GetProperty("idSede")?.GetValue(dataItem);

                LinkButton btnModificar = (LinkButton)e.Row.FindControl("btnModificar");
                LinkButton btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");

                btnModificar.Visible = estado;
                btnEliminar.Visible = estado;

                if (estado)
                {
                    btnEliminar.OnClientClick = $"return confirmarEliminacion({idSede}, '{hdnIdEliminar.ClientID}', '{btnEliminarSede.ClientID}');";
                }
            }
        }
        private sedeDTO ConstruirSedeDTO(sedeDTO sede)
        {
            if (sede == null)
                sede = new sedeDTO();
            sede.nombre = txtNombreSede.Text;
            sede.distrito = txtDistritoSede.Text;
            sede.estado = true;
            sede.estadoSpecified = true;
            return sede;
        }

        private void CargarHorariosExistentes(int idSede)
        {
            // Obtener las relaciones horario-sede
            var relacionesHorarioSede = this.horarioxSedeBO.Listar(idSede);
            
            // Lista para almacenar los horarios completos
            List<horarioAtencionDTO> horariosCompletos = new List<horarioAtencionDTO>();
            
            // Para cada relación, obtener el horario completo
            foreach (var relacion in relacionesHorarioSede)
            {
                horarioAtencionDTO horarioCompleto = this.horarioAtencionBO.ObtenerPorID(relacion.idHorario);
                if (horarioCompleto != null)
                {
                    horariosCompletos.Add(horarioCompleto);
                }
            }
            
            // Actualizar la lista de horarios seleccionados
            HorariosSeleccionados = horariosCompletos;
            
            // Actualizar la vista del GridView
            ActualizarVistaHorarios();
        }

        private void ActualizarVistaHorarios()
        {
            var listaVisual = new List<object>();
            foreach (var h in HorariosSeleccionados)
            {
                listaVisual.Add(new
                {
                    idHorario = h.idHorario,
                    diaSemana = h.diaSemana != null ? h.diaSemana.ToString() : "",
                    horaInicio = h.horaInicioStr,
                    horaFin = h.horaFinStr,
                    feriadoTexto = h.esFeriado != null && h.esFeriado ? "Sí" : "No"
                });
            }

            gvDetalleHorario.DataSource = listaVisual;
            gvDetalleHorario.DataBind();
        }

        public List<horarioAtencionDTO> HorariosSeleccionados
        {
            get
            {
                // Si no existe, inicializa la lista
                if (ViewState["HorariosSeleccionados"] == null)
                {
                    ViewState["HorariosSeleccionados"] = new List<horarioAtencionDTO>();
                }
                return (List<horarioAtencionDTO>)ViewState["HorariosSeleccionados"];
            }
            set
            {
                ViewState["HorariosSeleccionados"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            dgvSede.RowDataBound += dgv_RowDataBound;
            if (!IsPostBack)
            {
                var listaAdaptada = this.ConfigurarListado(ListadoSedes);
                dgvSede.DataSource = listaAdaptada;
                dgvSede.DataBind();
                var listaHorarioAdaptada = this.ConfigurarListado(ListadoHorarios);
                this.CargarDropDownList(ddlHorarios, listaHorarioAdaptada, "descripcion", "idHorario", "-- Seleccione un horario --");
            }
        }
        protected void btnAñadirHorario_Click(object sender, EventArgs e)
        {
            // Verifica que se haya seleccionado un horario válido
            if (!string.IsNullOrEmpty(ddlHorarios.SelectedValue))
            {
                int idHorario = int.Parse(ddlHorarios.SelectedValue);

                if (idHorario > 0)
                {
                    horarioAtencionDTO horarioObtenido = this.horarioAtencionBO.ObtenerPorID(idHorario);
                    if (horarioObtenido != null)
                    {
                        List<horarioAtencionDTO> lista = HorariosSeleccionados;
                        bool yaExiste = false;
                        foreach (var h in lista)
                        {
                            if (h.idHorario == horarioObtenido.idHorario)
                            {
                                yaExiste = true;
                                break;
                            }
                        }
                        if (!yaExiste)
                        {
                            lista.Add(horarioObtenido);
                            HorariosSeleccionados = lista;
                        }
                        
                        // Actualizar la vista del GridView
                        ActualizarVistaHorarios();
                        string titulo = hdnModoModal.Value == "modificar" ? "Modificar Sede" : "Registrar Sede";
                        this.MostrarModal(hdnModoModal.Value, titulo);
                    }
                }
            }
        }
        protected void btnEliminarHorario_Command(object sender, CommandEventArgs e)
        {
            int idHorario = int.Parse(e.CommandArgument.ToString());
            List<horarioAtencionDTO> lista = HorariosSeleccionados;
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].idHorario == idHorario)
                {
                    lista.RemoveAt(i);
                    break;
                }
            }

            HorariosSeleccionados = lista;
            
            // Actualizar la vista del GridView
            ActualizarVistaHorarios();
            string titulo = hdnModoModal.Value == "modificar" ? "Modificar Sede" : "Registrar Sede";
            this.MostrarModal(hdnModoModal.Value, titulo);
        }

        protected void btnGuardarSede_Click(object sender, EventArgs e)
        {
            string modo = hdnModoModal.Value;
            bool exito = false;
            if (modo == "registrar")
            {
                sedeDTO sede = new sedeDTO();
                sede = ConstruirSedeDTO(sede);
                sede.fechaCreacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                sede.fechaCreacionSpecified = true;
                sede.fechaModificacionSpecified = false;
                sede.usuarioCreacion = "admin"; // usar Session["usuario"] si aplica

                int idSede = this.sedeBO.Insertar(sede);
                if (idSede > 0) exito = true;
                if (exito)
                {
                    sedeDTO sedeInsertada = this.sedeBO.ObtenerPorID(idSede);
                    if (sedeInsertada != null)
                    {
                        foreach (var horario in HorariosSeleccionados)
                        {
                            horariosxSedesDTO relacion = new horariosxSedesDTO();
                            relacion.idHorario = horario.idHorario;
                            relacion.idHorarioSpecified = true;
                            relacion.idSedeSpecified = true;
                            relacion.idSede= sedeInsertada.idSede;
                            this.horarioxSedeBO.Insertar(relacion);
                        }
                    }
                }
            }
            else if (modo == "modificar")
            {
                // Validar que hdnIdSede.Value no esté vacío y sea un número válido
                if (string.IsNullOrEmpty(hdnIdSede.Value) || !int.TryParse(hdnIdSede.Value, out int id))
                {
                    MostrarResultado(false, "Sede", modo);
                    return;
                }
                
                sedeDTO sede = this.sedeBO.ObtenerPorID(id);

                sede = ConstruirSedeDTO(sede); // actualiza campos pero mantiene ID, creación, etc.
                sede.idSede = id;
                sede.idSedeSpecified = true;

                sede.fechaModificacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                sede.fechaModificacionSpecified = true;
                sede.usuarioModificacion = "admin"; // usar Session["usuario"] si aplica

                exito = this.sedeBO.Modificar(sede) > 0;
                
                // Si la modificación de la sede fue exitosa, actualizar los horarios
                if (exito)
                {
                    // Eliminar las relaciones horario-sede existentes
                    var relacionesExistentes = this.horarioxSedeBO.Listar(id);
                    foreach (var relacion in relacionesExistentes)
                    {
                        this.horarioxSedeBO.Eliminar(relacion);
                    }
                    
                    // Insertar las nuevas relaciones horario-sede
                    foreach (var horario in HorariosSeleccionados)
                    {
                        horariosxSedesDTO nuevaRelacion = new horariosxSedesDTO();
                        nuevaRelacion.idHorario = horario.idHorario;
                        nuevaRelacion.idHorarioSpecified = true;
                        nuevaRelacion.idSedeSpecified = true;
                        nuevaRelacion.idSede = id;
                        this.horarioxSedeBO.Insertar(nuevaRelacion);
                    }
                }
            }
            MostrarResultado(exito, "Sede", modo);
            if (exito)
            {
                btnBuscar_Click(sender, e);
                HorariosSeleccionados = new List<horarioAtencionDTO>(); // limpiar la lista
                                                                        // Limpiar campos visuales del modal
                txtNombreSede.Text = "";
                txtDistritoSede.Text = "";
                ddlHorarios.SelectedIndex = 0;
                lblDiaSemana.Text = "";
                lblHoraInicio.Text = "";
                lblHoraFin.Text = "";
                lblFeriado.Text = "";

                // Limpiar tabla de detalle de horarios
                gvDetalleHorario.DataSource = null;
                gvDetalleHorario.DataBind();
            }
        }
        protected void ddlHorarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlHorarios.SelectedValue))
            {
                int idHorario = int.Parse(ddlHorarios.SelectedValue);

                if (idHorario > 0)
                {
                    horarioAtencionDTO horarioObtenido = this.horarioAtencionBO.ObtenerPorID(idHorario);
                    lblDiaSemana.Text = horarioObtenido.diaSemana.ToString();
                    lblHoraInicio.Text = horarioObtenido.horaInicioStr;
                    lblHoraFin.Text = horarioObtenido.horaFinStr;
                    lblFeriado.Text = horarioObtenido.esFeriado ? "Sí" : "No";
                    string titulo = hdnModoModal.Value == "modificar" ? "Modificar Sede" : "Registrar Sede";
                    this.MostrarModal(hdnModoModal.Value, titulo);
                }
            }
        }

        protected void btnNuevoSede_Click(object sender, EventArgs e)
        {
            // Limpia campos
            txtNombreSede.Text = "";
            txtDistritoSede.Text = "";
            hdnIdSede.Value = "";
            
            // Limpiar lista de horarios seleccionados
            HorariosSeleccionados = new List<horarioAtencionDTO>();
            
            // Limpiar tabla de detalle de horarios
            gvDetalleHorario.DataSource = null;
            gvDetalleHorario.DataBind();

            // Cambia título a "Registrar"
            this.MostrarModal("registrar", "Registrar Sede");
        }

        protected void btnModificar_Command(object sender, CommandEventArgs e)
        {
            int idSede = int.Parse(e.CommandArgument.ToString());
            if (idSede > 0)
            {
                sedeDTO sede = this.SedeBO.ObtenerPorID(idSede);
                if (sede != null)
                {
                    // Establecer el ID de la sede en el campo oculto
                    hdnIdSede.Value = idSede.ToString();
                    
                    txtNombreSede.Text = sede.nombre;
                    txtDistritoSede.Text = sede.distrito;
                    
                    // Cargar los horarios actuales de la sede
                    CargarHorariosExistentes(idSede);
                    
                    this.MostrarModal("modificar", "Modificar Sede");
                }
            }
        }
        protected void btn_eliminar_Click(object sender, EventArgs e)
        {
            int idSede = int.Parse(hdnIdEliminar.Value);
            if (idSede > 0)
            {
                sedeDTO sede = this.SedeBO.ObtenerPorID(idSede);
                if (sede != null)
                {
                    sede.idSede = idSede;
                    sede.idSedeSpecified = true;
                    bool exito = this.SedeBO.Eliminar(sede) > 0;
                    MostrarResultado(exito, "Sede", "eliminar");
                    if (exito) btnBuscar_Click(sender, e);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            sedeParametros parametros = new sedeParametros();
            parametros.nombre = txtNombre.Text.Trim();
            parametros.estadoSpecified = !string.IsNullOrEmpty(ddlEstado.SelectedValue);
            parametros.estado = ddlEstado.SelectedValue == "1";

            var lista = this.SedeBO.Listar(parametros);
            var listaAdaptada = this.ConfigurarListado(lista);
            dgvSede.DataSource = listaAdaptada;
            dgvSede.DataBind();
        }

    }
}