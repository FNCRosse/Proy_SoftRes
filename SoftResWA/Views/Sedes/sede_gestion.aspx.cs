﻿using SoftResBusiness;
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
using SoftResBusiness.UsuarioWSClient;
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
        public usuariosDTO UsuarioActual
        {
            get
            {
                if (Session["UsuarioLogueado"] != null)
                {
                    return (usuariosDTO)Session["UsuarioLogueado"];
                }
                return null;
            }
        }

        //CONSTRUCTOR
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

        //CONFIGURACION VISUAL DE LISTADOS
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
        protected List<object> ConfigurarListadoHorariosDetalle(List<horarioAtencionDTO> lista)
        {
            return lista.Select(h => new
            {
                idHorario = h.idHorario,
                diaSemana = h.diaSemana.ToString(),
                horaInicio = h.horaInicioStr,
                horaFin = h.horaFinStr,
                feriadoTexto = h.esFeriado ? "Si" : "No"
            }).ToList<object>();
        }

        //FUNCIONES GENERALES
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

        //FUNCIONES PARA SEDES
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
        protected void LimpiarCamposModal()
        {
            txtNombreSede.Text = "";
            txtDistritoSede.Text = "";
            hdnIdSede.Value = "";
            ddlHorarios.SelectedIndex = 0;
            lblDiaSemana.Text = "";
            lblHoraInicio.Text = "";
            lblHoraFin.Text = "";
            lblFeriado.Text = "";

            // Limpiar lista de horarios seleccionados
            HorariosSeleccionados = new List<horarioAtencionDTO>();

            // Limpiar la tabla visual
            gvDetalleHorario.DataSource = null;
            gvDetalleHorario.DataBind();
        }
        protected void EliminarRelacionesHorariosxSede(int idSede)
        {
            BindingList<horariosxSedesDTO> relaciones = this.horarioxSedeBO.Listar(idSede);
            foreach (var horario in relaciones)
            {
                horariosxSedesDTO relacion = new horariosxSedesDTO();
                relacion.idSede = idSede;
                relacion.idSedeSpecified = true;
                relacion.idHorario = horario.idHorario;
                relacion.idHorarioSpecified = true;
                this.horarioxSedeBO.Eliminar(relacion);
            }
        }
        protected void InsertarRelacionesHorariosxSede(int idSede)
        {
            foreach (var horario in HorariosSeleccionados)
            {
                horariosxSedesDTO relacion = new horariosxSedesDTO();
                relacion.idHorario = horario.idHorario;
                relacion.idHorarioSpecified = true;
                relacion.idSedeSpecified = true;
                relacion.idSede = idSede;
                this.horarioxSedeBO.Insertar(relacion);
            }
        }

        //PAGE_LOAD
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
        
        //BOTONES
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
                        gvDetalleHorario.DataSource = ConfigurarListadoHorariosDetalle(HorariosSeleccionados);
                        gvDetalleHorario.DataBind();
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
            gvDetalleHorario.DataSource = ConfigurarListadoHorariosDetalle(HorariosSeleccionados);
            gvDetalleHorario.DataBind();
            string titulo = hdnModoModal.Value == "modificar" ? "Modificar Sede" : "Registrar Sede";
            this.MostrarModal(hdnModoModal.Value, titulo);
        }
        protected void btnGuardarSede_Click(object sender, EventArgs e)
        {
            if (UsuarioActual == null)
            {
                MostrarResultado(false, "Horario", "guardar");
                return;
            }
            string modo = hdnModoModal.Value;
            bool exito = false;
            if (modo == "registrar")
            {
                sedeDTO sede = new sedeDTO();
                sede = ConstruirSedeDTO(sede);
                sede.fechaCreacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                sede.fechaCreacionSpecified = true;
                sede.fechaModificacionSpecified = false;
                sede.usuarioCreacion = UsuarioActual.nombreComp; // usar Session["usuario"] si aplica

                int idSede = this.sedeBO.Insertar(sede);
                if (idSede > 0)
                {
                    exito = true;
                    InsertarRelacionesHorariosxSede(idSede);
                }
            }
            else if (modo == "modificar")
            {
                int id = int.Parse(hdnIdSede.Value);
                sedeDTO sede = this.sedeBO.ObtenerPorID(id);

                sede = ConstruirSedeDTO(sede); // actualiza campos pero mantiene ID, creación, etc.
                sede.idSede = id;
                sede.idSedeSpecified = true;

                sede.fechaModificacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                sede.fechaModificacionSpecified = true;
                sede.usuarioModificacion = UsuarioActual.nombreComp; // usar Session["usuario"] si aplica

                exito = this.sedeBO.Modificar(sede) > 0;
                if (exito)
                {
                    // 1. Eliminar relaciones previas
                    EliminarRelacionesHorariosxSede(id);
                    // 2. Insertar nuevas relaciones
                    InsertarRelacionesHorariosxSede(id);
                }
            }
            MostrarResultado(exito, "Sede", modo);
            if (exito)
            {
                btnBuscar_Click(sender, e);
                LimpiarCamposModal();
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
                    lblFeriado.Text = horarioObtenido.esFeriado ? "Si" : "No";
                    string titulo = hdnModoModal.Value == "modificar" ? "Modificar Sede" : "Registrar Sede";
                    this.MostrarModal(hdnModoModal.Value, titulo);
                }
            }
        }
        protected void btnNuevoSede_Click(object sender, EventArgs e)
        {
            // Limpia campos
            LimpiarCamposModal();
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
                    txtNombreSede.Text = sede.nombre;
                    txtDistritoSede.Text = sede.distrito;
                    hdnIdSede.Value = sede.idSede.ToString();

                    // Obtener horarios asociados a la sede
                    BindingList<horariosxSedesDTO> relaciones = this.horarioxSedeBO.Listar(idSede);
                    List<horarioAtencionDTO> horarios = new List<horarioAtencionDTO>();
                    foreach (var rel in relaciones)
                    {
                        var horario = this.horarioAtencionBO.ObtenerPorID(rel.idHorario);
                        if (horario != null)
                            horarios.Add(horario);
                    }
                    HorariosSeleccionados = horarios;
                    gvDetalleHorario.DataSource = ConfigurarListadoHorariosDetalle(HorariosSeleccionados);
                    gvDetalleHorario.DataBind();
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
                    if (exito)
                    {
                        EliminarRelacionesHorariosxSede(idSede);
                        btnBuscar_Click(sender, e);
                    }
                    MostrarResultado(exito, "Sede", "eliminar");
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