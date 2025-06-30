using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SoftResBusiness.ReporteReservaWSClient;

namespace SoftResBusiness
{
    public class ReporteReservaBO
    {
        private ReporteReservaClient reporteReservaClienteSOAP;

        public ReporteReservaBO()
        {
            this.reporteReservaClienteSOAP = new ReporteReservaClient();
        }
        public byte[] reporteReserva()
        {
            return this.reporteReservaClienteSOAP.reporteReserva();
        }
        public void abrirReporte(HttpResponse response, string nombreDeReporte, byte[] reporte)
        {
            response.Clear();
            response.ContentType = "application/pdf";
            response.AddHeader("Content-Disposition", "inline;filename=" + nombreDeReporte + ".pdf");
            response.BinaryWrite(reporte);
            response.End();
        }
    }
}
