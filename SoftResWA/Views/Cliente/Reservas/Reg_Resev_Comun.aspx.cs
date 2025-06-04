using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Cliente.Reservas
{
    public partial class Reg_Resev_Comun : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnUnirseEspera_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                "Swal.fire('¡Listo!', 'Has sido añadido a la lista de espera. Te notificaremos al correo registrado.', 'success');", true);
        }

    }
}