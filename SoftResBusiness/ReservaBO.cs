using SoftResBusiness.ReservaWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SoftResBusiness
{
    public class ReservaBO
    {
        private ReservaClient reservaClienteWA;

        public ReservaBO()
        {
            var endpoint = new EndpointAddress("http://localhost:8080/SoftResWSCliente/reservas");
            var binding = new BasicHttpBinding();
            this.reservaClienteWA = new ReservaClient(binding, endpoint);
        }

        public int Insertar(reservaDTO reserva)
        {
            return this.reservaClienteWA.insertar(reserva);
        }
        public reservaDTO ObtenerPorID(int reservaID)
        {
            return this.reservaClienteWA.obtenerPorId(reservaID);
        }
        public int Modificar(reservaDTO reserva)
        {
            return this.reservaClienteWA.modificar(reserva);
        }

        public int Eliminar(reservaDTO reserva)
        {
            return this.reservaClienteWA.eliminar(reserva);
        }

        public BindingList<reservaDTO> Listar(reservaParametros parametros)
        {
            reservaDTO[] lista = this.reservaClienteWA.listar(parametros);
            return new BindingList<reservaDTO>(lista);
        }
    }
}
