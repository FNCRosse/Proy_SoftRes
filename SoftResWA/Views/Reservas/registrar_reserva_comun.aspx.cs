using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Reservas
{
    public partial class registar_reserva_comun : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtCantidadPersonas.Text = "1";
                txtCantidadMesas.Text = "1";
                txtCantidadPersonas.Attributes["min"] = "1";
                txtCantidadMesas.Attributes["min"] = "1";
                txtCantidadPersonas.Attributes["max"] = "5";
                txtCantidadMesas.Attributes["max"] = "5";
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            // Aquí irá la lógica para guardar el cliente
            ScriptManager.RegisterStartupScript(this, this.GetType(), "registroExitoso", "Swal.fire('¡Reserva registrada!', 'El registro se completó correctamente.', 'success');", true);

        }
    }
}