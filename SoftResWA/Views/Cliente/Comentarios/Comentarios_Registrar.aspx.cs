using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Cliente.Comentarios
{
    public partial class Comentarios_Registrar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Cliente/Comentarios/Comentarios_Registrar.aspx");
        }
    }
}