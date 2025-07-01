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
        public usuariosDTO UsuarioActual
        {
            get { return Session["UsuarioLogueado"] as usuariosDTO; }
        }

        public SoftResCliente()
        {
            this.notificacionBO = new NotificacionBO();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // Lógica de visibilidad basada en la sesión
            if (UsuarioActual != null)
            {
                // --- El usuario SÍ está logueado ---
                pnlLogin.Visible = false; // Oculta el botón "Iniciar Sesión"
                pnlUsuario.Visible = true; // Muestra el menú de usuario

                if (!IsPostBack)
                {
                    // Asignar el nombre del usuario y cargar sus notificaciones
                    lblNombreUsuarioCliente.Text = UsuarioActual.nombreComp;
                    CargarNotificaciones();
                }
            }
            else
            {
                // --- El usuario NO está logueado ---
                pnlLogin.Visible = true;
                pnlUsuario.Visible = false;
                notificaciones.Visible = false;
                menuReservas.Visible = false;
            }

            // Lógica de protección de páginas para evitar bucles de redirección
            ProtegerPaginas();
        }
        private void ProtegerPaginas()
        {
            string currentPagePath = Request.Path.ToLower();
            var paginasPrivadas = new List<string>
            {
                "/views/cliente/reservas/misreservas.aspx",
                "/views/cliente/perfil/miperfil.aspx"
                // Añade aquí cualquier otra página que requiera estrictamente login
            };

            // Si el usuario no está logueado y está intentando acceder a una página privada
            if (UsuarioActual == null && paginasPrivadas.Contains(currentPagePath))
            {
                // Lo redirigimos al login
                Response.Redirect("~/Views/Cliente/Home/Login_Home.aspx");
            }
        }
        protected void lnkCerrarSesion_Click(object sender, EventArgs e)
        {
            // Destruye la sesión del usuario
            Session.Abandon();

            // Opcional: Borra la cookie de "Recordarme" si la usas
            if (Request.Cookies["Recuerdame"] != null)
            {
                HttpCookie cookie = new HttpCookie("Recuerdame") { Expires = DateTime.Now.AddDays(-1) };
                Response.Cookies.Add(cookie);
            }

            // Redirige a la página principal del cliente, que es pública
            Response.Redirect("~/Views/Cliente/Home/Home_Cliente.aspx");
        }
        private void CargarNotificaciones()
        {
            // El resto de tu lógica es correcta, solo asegúrate de que use la propiedad UsuarioActual
            try
            {
                notificacionParametros parametros = new notificacionParametros
                {
                    idUsuario = UsuarioActual.idUsuario, // Usando la propiedad segura
                    idUsuarioSpecified = true,
                    esLeida = false,
                    esLeidaSpecified = true
                    // He eliminado el filtro por estado 'ENVIADO' para que sea más simple,
                    // puedes volver a añadirlo si es un requisito.
                };

                BindingList<notificacionDTO> notificaciones = this.notificacionBO.Listar(parametros);

                if (notificaciones != null && notificaciones.Any())
                {
                    pnlNoNotificaciones.Visible = false;
                    lblContadorNotificaciones.Visible = true;
                    lblContadorNotificaciones.Text = notificaciones.Count.ToString();

                    var notificacionesAMostrar = notificaciones.Take(5).ToList();
                    ViewState["NotificacionesMostradasIDs"] = notificacionesAMostrar.Select(n => n.idNotificacion).ToList();

                    rptNotificaciones.DataSource = notificacionesAMostrar;
                    rptNotificaciones.DataBind();
                }
                else
                {
                    lblContadorNotificaciones.Visible = false;
                    pnlNoNotificaciones.Visible = true;
                    rptNotificaciones.DataSource = null;
                    rptNotificaciones.DataBind();
                    ViewState["NotificacionesMostradasIDs"] = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al cargar notificaciones: " + ex.Message);
                lblContadorNotificaciones.Visible = false;
            }
        }
    }
}