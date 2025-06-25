using SoftResBusiness.TipoMesaWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SoftResBusiness
{
    public class TipoMesaBO
    {
        private TipoMesaClient tMesaClienteSOAP;

        public TipoMesaBO()
        {
            // Usa la configuración declarada en web.config (name="TipoMesaPort")
            this.tMesaClienteSOAP = new TipoMesaClient();
        }

        public int Insertar(tipoMesaDTO tmesa)
        {
            return this.tMesaClienteSOAP.insertar(tmesa);
        }

        public tipoMesaDTO ObtenerPorID(int tmesaID)
        {
            return this.tMesaClienteSOAP.obtenerPorId(tmesaID);
        }

        public int Modificar(tipoMesaDTO tmesa)
        {
            return this.tMesaClienteSOAP.modificar(tmesa);
        }

        public int Eliminar(tipoMesaDTO tmesa)
        {
            return this.tMesaClienteSOAP.eliminar(tmesa);
        }

        public BindingList<tipoMesaDTO> Listar()
        {
            var lista = this.tMesaClienteSOAP.listar();

            if (lista == null)
                return new BindingList<tipoMesaDTO>();   // lista vacía si el servicio devuelve null

            return new BindingList<tipoMesaDTO>(lista);
        }
    }
}
