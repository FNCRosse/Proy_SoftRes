using SoftResBusiness;
using SoftResBusiness.ComentarioWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Cliente.Comentarios
{
    public partial class Comentarios_Listado : System.Web.UI.Page
    {
        private ComentarioBO comentarioBO;
        private BindingList<comentariosDTO> listaComentarios;

        public ComentarioBO ComentarioBO { get => comentarioBO; set => comentarioBO = value; }
        public BindingList<comentariosDTO> ListaComentarios { get => listaComentarios; set => listaComentarios = value; }

        public Comentarios_Listado()
        {
            this.comentarioBO = new ComentarioBO();
            comentarioParametros cParametros = new comentarioParametros();
            cParametros.idLocalSpecified = false;
            cParametros.puntuacionSpecified = false;
            cParametros.idReservaSpecified = false;
            cParametros.estadoSpecified = true;
            cParametros.estado = true;
            this.listaComentarios = this.comentarioBO.Listar(cParametros);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarComentariosDemo();
        }
        public string GetStars(int puntuacion)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                sb.Append(i < puntuacion
                    ? "<i class='fas fa-star text-warning'></i>"
                    : "<i class='far fa-star text-warning'></i>");
            }
            return sb.ToString();
        }
        private void CargarComentariosDemo()
        {
            rptComentarios.DataSource = ListaComentarios;
            rptComentarios.DataBind();
        }
        protected void btnAgregarComentario_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Cliente/Comentarios/Comentarios_Registrar.aspx");
        }

    }
}