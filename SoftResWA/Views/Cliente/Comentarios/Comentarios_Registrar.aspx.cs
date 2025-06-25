using SoftResBusiness;
using SoftResBusiness.ComentarioWSClient;
using SoftResBusiness.LocalWSClient;
using SoftResBusiness.ReservaWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using reservaDTO = SoftResBusiness.ReservaWSClient.reservaDTO;

namespace SoftResWA.Views.Cliente.Comentarios
{
    public partial class Comentarios_Registrar : System.Web.UI.Page
    {
        private ComentarioBO comentarioBO;
        private ReservaBO reservaBO;

        public ComentarioBO ComentarioBO { get => comentarioBO; set => comentarioBO = value; }
        public ReservaBO ReservaBO { get => reservaBO; set => reservaBO = value; }

        public Comentarios_Registrar()
        {
            this.comentarioBO = new ComentarioBO();
            this.reservaBO = new ReservaBO();  
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
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
        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            comentariosDTO comentario = new comentariosDTO();
            comentario.mensaje = txtMensaje.Text.Trim();
            comentario.estado = true;
            comentario.estadoSpecified = true;
            comentario.puntuacionSpecified = true;
            comentario.puntuacion = int.Parse(rblPuntuacion.SelectedValue);
            comentario.fechaCreacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            comentario.fechaCreacionSpecified = true;
            comentario.fechaModificacionSpecified = false;
            comentario.usuarioCreacion = "admin";
            bool exito = this.comentarioBO.Insertar(comentario) > 0;
            MostrarResultado(exito, "Comentario", "registrar");
            Response.Redirect("~/Views/Cliente/Comentarios/Comentarios_Listado.aspx");
        }
    }
}