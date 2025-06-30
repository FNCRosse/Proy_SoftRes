using SoftResBusiness.RolWSClient;
using SoftResBusiness;
using SoftResBusiness.NotificacionWSClient;
using SoftResBusiness.UsuarioWSClient;
using System.Linq;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using usuariosDTO = SoftResBusiness.UsuarioWSClient.usuariosDTO;

namespace SoftResWA
{
    public partial class SoftRes : System.Web.UI.MasterPage
    {
        NotificacionBO notificacionBO;
        usuariosDTO usuarioSession;

        public NotificacionBO NotificacionBO { get => notificacionBO; set => notificacionBO = value; }
        public usuariosDTO UsuarioSession { get => usuarioSession; set => usuarioSession = value; }

        public SoftRes()
        {
            this.notificacionBO = new NotificacionBO();
            this.usuarioSession = null;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // Verificar si el usuario está autenticado
            if (Session["UsuarioLogueado"] == null)
            {
                // Si no hay sesión, redirigir a la página de login
                Response.Redirect("~/Views/Login/Login.aspx");
                return;
            }
            //Si hay sesión, obtener el rol y controlar la visibilidad del menú
            if (!IsPostBack) 
            {
                UsuarioSession = (usuariosDTO)Session["UsuarioLogueado"];
                lblNombreUsuario.Text = UsuarioSession.nombreComp;
                string rol = UsuarioSession.rol.nombre.ToLower();

                // Por defecto, asumimos que es un empleado y ocultamos lo de administrador
                menuSedes.Visible = false;
                menuLocales.Visible = false;
                menuHorarios.Visible = false;
                menuUsuarios.Visible = false;
                menuMesas.Visible = false;

                // Si el rol es administrador, mostramos todo
                if (rol == "administrador" || rol == "gerente")
                {
                    menuSedes.Visible = true;
                    menuLocales.Visible = true;
                    menuHorarios.Visible = true;
                    menuUsuarios.Visible = true;
                    menuMesas.Visible = true;
                }
                CargarNotificaciones();
            }
        }
        protected void lnkCerrarSesion_Click(object sender, EventArgs e)
        {
            // 1. Destruir la sesión del usuario.
            Session.Abandon();
            if (Request.Cookies["Recuerdame"] != null)
            {
                HttpCookie cookie = new HttpCookie("Recuerdame");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            Response.Redirect("~/Views/Login/Login.aspx");
        }
        private void CargarNotificaciones()
        {
            try
            {
                // 2. Crear los parámetros de búsqueda.
                //    Queremos las notificaciones del usuario actual y que no estén leídas.
                notificacionParametros parametros = new notificacionParametros();
                parametros.idUsuario = UsuarioSession.idUsuario;
                parametros.idUsuarioSpecified = true;
                parametros.estado = estadoNotificacion.ENVIADO; // Asumiendo que tu DTO tiene un campo para filtrar por leídas
                parametros.estadoSpecified = true;
                parametros.esLeida = false;
                parametros.esLeidaSpecified = true;
                BindingList<notificacionDTO> notificaciones = this.notificacionBO.Listar(parametros);

                // 4. Procesar los resultados
                if (notificaciones != null && notificaciones.Count > 0)
                {
                    pnlNoNotificaciones.Visible = false;
                    lblContadorNotificaciones.Visible = true;
                    lblContadorNotificaciones.Text = notificaciones.Count.ToString();

                    // Enlazar los datos al Repeater
                    // Solo las 5 más recientes para el desplegable, por ejemplo
                    var notificacionesAMostrar = notificaciones.Take(5).ToList();
                    rptNotificaciones.DataSource = notificacionesAMostrar;
                    rptNotificaciones.DataBind();
                }
                else
                {
                    // No hay notificaciones no leídas
                    lblContadorNotificaciones.Visible = false;
                    pnlNoNotificaciones.Visible = true;
                    rptNotificaciones.DataSource = null; // Limpiar datos previos
                    rptNotificaciones.DataBind();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al cargar notificaciones: " + ex.Message);
                lblContadorNotificaciones.Visible = false;
                pnlNoNotificaciones.Visible = true;
            }
        }
    }
}