using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Cliente.Reservas
{
    public partial class Reg_Resev_Evento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnRegReserva_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "registroExitoso", "Swal.fire('¡Reserva registrada!', 'El registro se completó correctamente.', 'success');", true);
        }
    }
}