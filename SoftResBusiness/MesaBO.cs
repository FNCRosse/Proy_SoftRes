using SoftResBusiness.MesaWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SoftResBusiness
{
    public class MesaBO
    {
        private MesaClient mesaClienteWA;

        public MesaBO()
        {
            var endpoint = new EndpointAddress("http://localhost:8080/SoftResWSCliente/mesas");
            var binding = new BasicHttpBinding();
            this.mesaClienteWA = new MesaClient(binding, endpoint);
        }

        public int Insertar(mesaDTO mesa)
        {
            return this.mesaClienteWA.insertar(mesa);
        }
        public mesaDTO ObtenerPorID(int mesaID)
        {
            return this.mesaClienteWA.obtenerPorId(mesaID);
        }
        public int Modificar(mesaDTO mesa)
        {
            return this.mesaClienteWA.modificar(mesa);
        }

        public int Eliminar(mesaDTO mesa)
        {
            return this.mesaClienteWA.eliminar(mesa);
        }

        public BindingList<mesaDTO> Listar(mesaParametros parametros)
        {
            mesaDTO[] lista = this.mesaClienteWA.listar(parametros);
            return new BindingList<mesaDTO>(lista);
        }

    }
}
