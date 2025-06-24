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
        private TipoMesaClient tMesaClienteWA;

        public TipoMesaBO()
        {
            var endpoint = new EndpointAddress("http://localhost:8080/SoftResWSCliente/tipoMesa");
            var binding = new BasicHttpBinding();
            this.tMesaClienteWA = new TipoMesaClient(binding, endpoint);
        }

        public int Insertar(tipoMesaDTO sede)
        {
            return this.tMesaClienteWA.insertar(sede);
        }
        public tipoMesaDTO ObtenerPorID(int sedeID)
        {
            return this.tMesaClienteWA.obtenerPorId(sedeID);
        }
        public int Modificar(tipoMesaDTO sede)
        {
            return this.tMesaClienteWA.modificar(sede);
        }

        public int Eliminar(tipoMesaDTO sede)
        {
            return this.tMesaClienteWA.eliminar(sede);
        }

        public BindingList<tipoMesaDTO> Listar()
        {
            tipoMesaDTO[] lista = this.tMesaClienteWA.listar();
            return new BindingList<tipoMesaDTO>(lista);
        }
    }
}
