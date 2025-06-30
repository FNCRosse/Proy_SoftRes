using SoftResBusiness;
using SoftResBusiness.NotificacionWSClient;
using SoftResBusiness.UsuarioWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using usuariosDTO = SoftResBusiness.UsuarioWSClient.usuariosDTO;

// Asegúrate de que el namespace coincida con la ubicación de tu archivo
namespace SoftResWA.Views.Home
{
    public partial class Notificaciones_gestion : System.Web.UI.Page
    {
        private NotificacionBO notificacionBO;
        private BindingList<notificacionDTO> listadoNotificacion;

        public BindingList<notificacionDTO> ListadoNotificacion { get => listadoNotificacion; set => listadoNotificacion = value; }
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
        public Notificaciones_gestion()
        {
            this.notificacionBO = new NotificacionBO();
        }

        //CONFIGURACION VISUAL DE LISTADOS
        protected List<object> ConfigurarListado(BindingList<notificacionDTO> lista)
        {
            var listaAdaptada = lista.Select(m => new
            {
                m.idNotificacion,
                m.tipoNotificacion,
                m.estado,
                m.leida,    
                m.mensaje,
                estadoBool = true
            }).ToList<Object>();
            return listaAdaptada;
        }
        //FUNCIONES GENERALES
        private void MostrarResultado(bool exito, string entidad, string modo)
        {
            string accion = (modo == "modificar") ? "modificada" :
                            (modo == "eliminar") ? "eliminada" : "registrada";

            string accionNo = (modo == "modificar") ? "modificar" :
                              (modo == "eliminar") ? "eliminar" : "registrar";

            string baseMensaje = exito ? $"La {entidad} fue {accion}" : $"No se pudo {accionNo} la {entidad}";
            string tipo = exito ? "success" : "warning";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje",
                $"Swal.fire('¡{entidad} {(exito ? accion : $"NO {accion}")}!', '{baseMensaje} correctamente.', '{tipo}');", true);
        }
        protected void dgv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem;

                // Obtener estadoBool vía reflexión
                bool estado = true; // Por defecto activo
                var prop = dataItem.GetType().GetProperty("estadoBool");
                if (prop != null)
                {
                    estado = (bool)prop.GetValue(dataItem);
                }

                int id = (int)dataItem.GetType().GetProperty("idNotificacion")?.GetValue(dataItem);

                LinkButton btnMarcarLeido= (LinkButton)e.Row.FindControl("btnMarcarLeida");
                LinkButton btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");

                btnMarcarLeido.Visible = estado;
                btnEliminar.Visible = estado;

                if (estado)
                {
                    btnEliminar.OnClientClick = $"return confirmarEliminacion({id}, '{hdnIdEliminar.ClientID}', '{btnEliminarNotificacion.ClientID}');";
                }
            }
        }
        protected void cargarDatos()
        {
            notificacionParametros parametros = new notificacionParametros();
            parametros.idUsuario = UsuarioActual.idUsuario;
            parametros.idUsuarioSpecified = true;
            parametros.tipoNotificacionSpecified = false;
            parametros.esLeidaSpecified = false;
            parametros.estadoSpecified = false;
            this.listadoNotificacion = this.notificacionBO.Listar(parametros);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            gvNotificaciones.RowDataBound += dgv_RowDataBound;
            if (!IsPostBack)
            {
                if (UsuarioActual == null)
                {
                    Response.Redirect("~/Views/Login/Login.aspx"); // Seguridad
                    return;
                }
                cargarDatos();
                var listaAdaptada = this.ConfigurarListado(ListadoNotificacion);
                gvNotificaciones.DataSource = listaAdaptada;
                gvNotificaciones.DataBind();
                CargarFiltros();

            }
        }

        // --- MÉTODOS DE CARGA Y CONFIGURACIÓN ---
        private void CargarDropDownList(DropDownList ddl, object dataSource, string textField, string valueField, string textoDefault)
        {
            ddl.DataSource = dataSource;
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(textoDefault, ""));
        }
        private void CargarFiltros()
        {
            try
            {
                // Cargar TipoNotificacion
                var tipoNotificacion = Enum.GetNames(typeof(SoftResBusiness.NotificacionWSClient.tipoNotificacion))
                .Select(d => new { nombre = d, id = d })
                .ToList();
                this.CargarDropDownList(ddlTipoNotificacionFiltro, tipoNotificacion, "nombre", "id", "Todos");

                // Cargar EstadoNotificacion
                var estados = Enum.GetNames(typeof(SoftResBusiness.NotificacionWSClient.estadoNotificacion))
                .Select(d => new { nombre = d, id = d })
                .ToList();
                this.CargarDropDownList(ddlEstadoFiltro, estados, "nombre", "id", "Todos");
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error", $"No se pudieron cargar los filtros: {ex.Message}", "error");
            }
        }

        // --- MÉTODOS AUXILIARES ---

        protected string GetIconoNotificacion(object tipo)
        {
            if (tipo == null) return "fas fa-question-circle";

            // Asegurarse de que el objeto es del tipo correcto antes de castear
            if (Enum.IsDefined(typeof(tipoNotificacion), tipo))
            {
                tipoNotificacion tipoEnum = (tipoNotificacion)tipo;
                switch (tipoEnum)
                {
                    case tipoNotificacion.CONFIRMACION:
                        return "fas fa-check-circle text-success";
                    case tipoNotificacion.CANCELACION:
                        return "fas fa-times-circle text-danger";
                    case tipoNotificacion.RECORDATORIO:
                        return "fas fa-clock text-info";
                    case tipoNotificacion.MODIFICACION:
                        return "fas fa-edit text-warning";
                    default:
                        return "fas fa-info-circle text-secondary";
                }
            }
            return "fas fa-question-circle";
        }

        private void MostrarAlerta(string titulo, string mensaje, string tipo)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), $"alerta_{tipo}",
                $"Swal.fire('{titulo}', '{Server.HtmlEncode(mensaje)}', '{tipo}');", true);
        }

        // --- EVENTOS DE CONTROLES ---

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                notificacionParametros parametros = new notificacionParametros();
                parametros.idUsuario = UsuarioActual.idUsuario;
                parametros.idUsuarioSpecified = true;

                parametros.tipoNotificacionSpecified = !string.IsNullOrEmpty(ddlTipoNotificacionFiltro.SelectedValue);
                if (parametros.tipoNotificacionSpecified)
                    parametros.tipoNotificacion = (tipoNotificacion)Enum.Parse(typeof(tipoNotificacion), ddlEstadoFiltro.SelectedValue);

                parametros.esLeidaSpecified = !string.IsNullOrEmpty(ddlLeidaFiltro.SelectedValue);
                parametros.esLeida = ddlLeidaFiltro.SelectedValue == "1";

                if (parametros.estadoSpecified)
                    parametros.estado = (estadoNotificacion)Enum.Parse(typeof(estadoNotificacion), ddlEstadoFiltro.SelectedValue);
                
                this.listadoNotificacion = this.notificacionBO.Listar(parametros);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorBuscar",
                    $"Swal.fire('Error', 'Error en la búsqueda: {ex.Message}', 'error');", true);
            }
        }
        protected void btnMarcarLeida_Command(object sender, CommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            bool exito = false;
            if (id > 0)
            {
                try
                {
                    notificacionDTO notiMod = this.notificacionBO.ObtenerPorID(id,UsuarioActual.idUsuario);
                    if (notiMod != null)
                    {
                        notiMod.leida = !notiMod.leida;
                        notiMod.leidaSpecified = true;
                        exito = this.notificacionBO.Modificar(notiMod)>0;
                        MostrarResultado(exito, "Notificacion", "modificar");
                        if (exito) btnBuscar_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errorModificar",
                        $"Swal.fire('Error', 'Error al cargar la mesa: {ex.Message}', 'error');", true);
                }
            }
        }
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int id = int.Parse(hdnIdEliminar.Value);
            if (id > 0)
            {
                try
                {
                    notificacionDTO noti = this.notificacionBO.ObtenerPorID(id,UsuarioActual.idUsuario);
                    if (noti != null)
                    {
                        noti.idNotificacion = id;
                        noti.idNotificacionSpecified = true;
                        bool exito = this.notificacionBO.Eliminar(noti) > 0;
                        MostrarResultado(exito, "Notificacion", "eliminar");
                        if (exito) btnBuscar_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errorEliminar",
                        $"Swal.fire('Error', 'Error al eliminar la mesa: {ex.Message}', 'error');", true);
                }
            }
        }
    }
}