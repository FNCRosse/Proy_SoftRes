using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftResBusiness;

namespace SoftResWA.Views.Reportes
{
    public partial class reporte_reservas : System.Web.UI.Page
    {
        private ReporteReservaBO reporteReservaBO;
        public reporte_reservas()
        {
            this.reporteReservaBO = new ReporteReservaBO();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            byte[] reporte = this.reporteReservaBO.reporteReserva();
            this.reporteReservaBO.abrirReporte(Response, "ReporteReservas.pdf", reporte);
        }
    }
}