using SoftResBusiness.ReservaxMesaWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SoftResBusiness
{
    public class ReservaxMesaBO
    {
        private ReservaxMesaClient rxmClienteWA;

        public ReservaxMesaBO()
        {
            var endpoint = new EndpointAddress("http://localhost:8080/SoftResWSCliente/reservaxmesa");
            var binding = new BasicHttpBinding();
            this.rxmClienteWA = new ReservaxMesaClient(binding, endpoint);
        }

        public int Insertar(reservaxMesasDTO rxm)
        {
            return this.rxmClienteWA.insertar(rxm);
        }

        public int Eliminar(reservaxMesasDTO rxm)
        {
            return this.rxmClienteWA.eliminar(rxm);
        }

        public BindingList<reservaxMesasDTO> Listar(int idReserva)
        {
            reservaxMesasDTO[] lista = this.rxmClienteWA.listar(idReserva);
            return new BindingList<reservaxMesasDTO>(lista);
        }
    }
}
