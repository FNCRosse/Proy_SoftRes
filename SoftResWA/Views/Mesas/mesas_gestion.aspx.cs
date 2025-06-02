using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Mesas
{
    public partial class mesas_gestion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnGuardarMesa_Click(object sender, EventArgs e)
        {
            // Aquí irá la lógica para guardar el local
            ScriptManager.RegisterStartupScript(this, this.GetType(), "registroExitoso", "Swal.fire('¡Mesa registrada!', 'El registro se completó correctamente.', 'success');", true);

        }
    }
}