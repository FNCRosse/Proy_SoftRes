using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SoftResBusiness.SedeWSClient;

namespace SoftResBusiness
{
    public class SedeBO
    {
        private SedeClient sedesClienteWA;

        public SedeBO()
        {
            var endpoint = new EndpointAddress("http://localhost:8080/SoftResWSCliente/sedes");
            var binding = new BasicHttpBinding();
            this.sedesClienteWA = new SedeClient(binding, endpoint);
        }

        public int Insertar(sedeDTO sede)
        {
            return this.sedesClienteWA.insertar(sede);
        }
        public sedeDTO ObtenerPorID(int sedeID)
        {
            return this.sedesClienteWA.obtenerPorId(sedeID);
        }
        public int Modificar(sedeDTO sede)
        {
            return this.sedesClienteWA.modificar(sede);
        }

        public int Eliminar(sedeDTO sede)
        {
            return this.sedesClienteWA.eliminar(sede);
        }

        public BindingList<sedeDTO> Listar(sedeParametros parametros)
        {
            sedeDTO[] lista = this.sedesClienteWA.listar(parametros);
            return new BindingList<sedeDTO>(lista);
        }
    }
}
