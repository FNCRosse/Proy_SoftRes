using SoftResBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Locales
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        //private LocalBO localBO;
        //private BindingList<LocalDTO> listadoLocal;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Aqui van para mostrar el listado de lcoales
                //No olvidar poner 
                //List<LocalDTO> sedes = new List<LocalDTO>
                //dgvLocal.DataSource = locales;
                //dgvLocal.DataBind();

                ////Aqui los listados de los filtros
                ////Para sede
                //ddlSede.DataSource = sedes;
                //ddlSede.DataTextField = "descripcion";
                //ddlSede.DataValueField = "idHorario";
                //ddlSede.DataBind();
                //ddlSede.Items.Insert(0, new ListItem("-- Seleccione --", "")); //Esto no es null, en setearlo en los parametros tenemos que ponerlo null

            }
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