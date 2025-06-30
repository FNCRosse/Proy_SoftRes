using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SoftResBusiness.ReporteClienteWSClient;

namespace SoftResBusiness
{
    public class ReporteClienteBO
    {
        private ReporteClienteClient reporteClienteClienteSOAP;

        public ReporteClienteBO()
        {
        }
        public byte[] reporteCliente()
        {
            return this.reporteClienteClienteSOAP.reporteCliente();
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
