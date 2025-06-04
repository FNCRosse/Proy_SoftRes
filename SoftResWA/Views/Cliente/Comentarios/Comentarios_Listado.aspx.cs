using System;
using System.Collections.Generic;
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
            DataTable dt = new DataTable();
            dt.Columns.Add("USUARIO_CREACION");
            dt.Columns.Add("FECHA_CREACION");
            dt.Columns.Add("MENSAJE");
            dt.Columns.Add("PUNTUACION", typeof(int));

            dt.Rows.Add("CarmenLopez", DateTime.Now.AddDays(-1), "¡La comida fue espectacular! Volveré pronto.", 5);
            dt.Rows.Add("Carlos94", DateTime.Now.AddDays(-2), "El ambiente fue acogedor, pero el servicio algo lento.", 3);
            dt.Rows.Add("SofiaChung", DateTime.Now.AddDays(-5), "Los fideos estaban perfectos. Excelente atención.", 4);
            dt.Rows.Add("JorgeTen", DateTime.Now.AddDays(-7), "Esperaba más variedad en el menú.", 2);

            rptComentarios.DataSource = dt;
            rptComentarios.DataBind();
        }
        protected void btnAgregarComentario_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Cliente/Comentarios/Comentarios_Registrar.aspx");
        }

    }
}