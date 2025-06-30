using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftResBusiness;

namespace SoftResWA.Views.Reportes
{
    public partial class reporte_cliente : System.Web.UI.Page
    {

        private ReporteClienteBO reporteClienteBO;

        public reporte_cliente()
        {
            this.reporteClienteBO = new ReporteClienteBO();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            byte[] reporte = this.reporteClienteBO.reporteCliente();
            this.reporteClienteBO.abrirReporte(Response, "ReporteClientes.pdf", reporte);
        }
    }
}