using SoftResBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftResBusiness.UsuarioWSClient;
using SoftResBusiness.NotificacionWSClient;
using System.ComponentModel;
using usuariosDTO = SoftResBusiness.UsuarioWSClient.usuariosDTO;

namespace SoftResWA
{
    public partial class SoftResCliente : System.Web.UI.MasterPage
    {
        NotificacionBO notificacionBO;
        usuariosDTO usuarioSession;

        public NotificacionBO NotificacionBO { get => notificacionBO; set => notificacionBO = value; }
        public usuariosDTO UsuarioSession { get => usuarioSession; set => usuarioSession = value; }

        public SoftResCliente()
        {
            this.notificacionBO = new NotificacionBO();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Por defecto, asumimos que es un empleado y ocultamos lo de administrador
                menuReservas.Visible = false;
                notificaciones.Visible = false;
                login.Visible = true;

                // Si el rol es administrador, mostramos todo
                if (Session["UsuarioLogueado"] != null )
                {
                    UsuarioSession = (usuariosDTO)Session["UsuarioLogueado"];
                    string rol = UsuarioSession.rol.nombre.ToLower();
                    menuReservas.Visible = true;
                    notificaciones.Visible = true;
                    login.Visible = false;
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