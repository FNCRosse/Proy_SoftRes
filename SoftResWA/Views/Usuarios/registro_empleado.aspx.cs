using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Usuarios
{
    public partial class registro_empleado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                txtSueldo.Attributes["min"] = "1";
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            // Aquí irá la lógica para guardar el empleado
            ScriptManager.RegisterStartupScript(this, this.GetType(), "registroExitoso","Swal.fire('¡Empleado registrado!', 'El registro se completó correctamente.', 'success');", true);

        }
        protected void btnCalendario_Click(object sender, EventArgs e)
        {
            calFechaContratacion.Visible = true;
        }

        protected void calFechaContratacion_SelectionChanged(object sender, EventArgs e)
        {
            txtFechaContratacion.Text = calFechaContratacion.SelectedDate.ToString("dd-MM-yyyy");
            calFechaContratacion.Visible = false;
        }


    }
}