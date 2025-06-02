using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Locales
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnAñadirMesa_Click(object sender, EventArgs e)
        {
            // Aquí irá la lógica para añadir  la mesa

        }
        protected void btnGuardarLocal_Click(object sender, EventArgs e)
        {
            // Aquí irá la lógica para guardar el local
            ScriptManager.RegisterStartupScript(this, this.GetType(), "registroExitoso", "Swal.fire('¡Local registrado!', 'El registro se completó correctamente.', 'success');", true);

        }
        protected void btnBuscarMesa_Click(object sender, EventArgs e)
        {
            // Aquí irá la lógica para buscar la mesa
        }
    }
}