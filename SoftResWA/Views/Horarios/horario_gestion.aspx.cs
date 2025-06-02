using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Horarios
{
    public partial class horario_gestion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnGuardarHorario_Click(object sender, EventArgs e)
        {
            // Aquí irá la lógica para guardar el horario
            ScriptManager.RegisterStartupScript(this, this.GetType(), "registroExitoso", "Swal.fire('¡Horario registrado!', 'El registro se completó correctamente.', 'success');", true);
        }
    }
}