using SoftResBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Reservas
{
    public partial class ReporteReservas : System.Web.UI.Page
    {
        private ReservaBO reservaBO;

        public ReporteReservas()
        {
            this.reservaBO = new ReservaBO();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            byte[] reporte = this.reservaBO.reporteReservas();
            this.reservaBO.abrirReporte(Response,"ReporteReserva.pdf",reporte);
        }
    }
}