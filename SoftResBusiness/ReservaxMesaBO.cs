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
        private ReservaxMesaClient rxmClienteSOAP;

        public ReservaxMesaBO()
        {
            // Usa la configuración definida en el web.config (name="ReservaxMesaPort")
            this.rxmClienteSOAP = new ReservaxMesaClient();
        }

        //public int Insertar(reservaxMesasDTO rxm)
        //{
        //    return this.rxmClienteSOAP.insertar(rxm);
        //}

        //public int Eliminar(reservaxMesasDTO rxm)
        //{
        //    return this.rxmClienteSOAP.eliminar(rxm);
        //}

        //public BindingList<reservaxMesasDTO> Listar(int idReserva)
        //{
        //    var lista = this.rxmClienteSOAP.listar(idReserva);

        //    if (lista == null)
        //        return new BindingList<reservaxMesasDTO>(); // lista vacía sin error

        //    return new BindingList<reservaxMesasDTO>(lista);
        //}
    }

}
