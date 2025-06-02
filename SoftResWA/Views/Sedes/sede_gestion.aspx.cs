using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Sedes
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void dgvSede_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "Modificar")
            //{
            //    string idSede = e.CommandArgument.ToString();

            //    // Obtener datos de la sede desde la BD
            //    var sede = ObtenerSedePorId(idSede); // tu método de BD

            //    if (sede != null)
            //    {
            //        //txtNombreSede.Text = sede.Nombre;
            //        //txtDistritoSede.Text = sede.Distrito;
            //        //txtIdHorario.Text = sede.IdHorario.ToString();
            //        //// Y así con los demás...

            //        hdnIdSede.Value = idSede;

            //        // Cambiar título del modal (opcional)
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "tituloModal", "document.getElementById('modalRegistrarSedeLabel').innerText = 'Modificar Sede';", true);

            //        // Mostrar el modal
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModal", "$('#modalRegistrarSede').modal('show');", true);
            //    }
            //}
        }

        protected void btnAñadirHorario_Click(object sender, EventArgs e)
        {
            // Aquí irá la lógica para añadir el horario

        }
        protected void btnGuardarSede_Click(object sender, EventArgs e)
        {
            // Aquí irá la lógica para guardar la sede
            ScriptManager.RegisterStartupScript(this, this.GetType(), "registroExitoso", "Swal.fire('¡Sede registrada!', 'El registro se completó correctamente.', 'success');", true);

        }
        protected void btnBuscarHorario_Click(object sender, EventArgs e)
        {
            // Aquí irá la lógica para buscar el horario
        }
    }
}