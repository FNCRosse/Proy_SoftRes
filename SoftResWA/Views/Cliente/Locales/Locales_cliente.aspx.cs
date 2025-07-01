using SoftResBusiness;
using SoftResBusiness.LocalWSClient;
using SoftResBusiness.SedeWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sedeDTO = SoftResBusiness.SedeWSClient.sedeDTO;

namespace SoftResWA.Views.Cliente.Locales
{
    public partial class Locales_cliente : System.Web.UI.Page
    {
        private SedeBO sedeBO;
        private LocalBO localBO;
        public Locales_cliente()
        {
            this.sedeBO = new SedeBO();
            this.localBO = new LocalBO();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarSedes();
            }
        }
        private void CargarSedes()
        {
            try
            {
                sedeParametros parametros = new sedeParametros();
                parametros.estado = true;
                parametros.estadoSpecified = true;
                BindingList<sedeDTO> sedes = sedeBO.Listar(parametros);

                if (sedes != null && sedes.Any())
                {
                    rptSedes.DataSource = sedes;
                    rptSedes.DataBind();

                    // Por defecto, cargar los locales de la primera sede de la lista
                    int primeraSedeId = sedes.First().idSede;
                    CargarLocalesPorSede(primeraSedeId);
                }
            }
            catch (Exception ex)
            {
                // Manejar error de carga de sedes
                System.Diagnostics.Debug.WriteLine($"Error al cargar sedes: {ex.Message}");
            }
        }

        private void CargarLocalesPorSede(int idSede)
        {
            try
            {
                localParametros parametros = new localParametros();
                parametros.estado = true;
                parametros.estadoSpecified = true;
                parametros.idSede = idSede;
                parametros.idSedeSpecified = true;

                BindingList<localDTO> locales = localBO.Listar(parametros);

                rptLocales.DataSource = locales;
                rptLocales.DataBind();

                // Mostrar u ocultar el mensaje de "no hay locales"
                pnlNoLocales.Visible = (locales == null || !locales.Any());
            }
            catch (Exception ex)
            {
                // Manejar error de carga de locales
                System.Diagnostics.Debug.WriteLine($"Error al cargar locales por sede: {ex.Message}");
                pnlNoLocales.Visible = true;
            }
        }

        protected void rptSedes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // Este evento se dispara cuando se hace clic en un botón de sede
            if (e.CommandName == "CargarLocales")
            {
                // Cambiar el estilo del botón activo
                foreach (RepeaterItem item in rptSedes.Items)
                {
                    LinkButton btn = (LinkButton)item.FindControl("btnSede");
                    btn.CssClass = "btn btn-sede";
                }
                LinkButton botonClickeado = (LinkButton)e.CommandSource;
                botonClickeado.CssClass = "btn btn-sede active";

                // Cargar los locales correspondientes
                int idSede = int.Parse(e.CommandArgument.ToString());
                CargarLocalesPorSede(idSede);
            }
        }
    }
}