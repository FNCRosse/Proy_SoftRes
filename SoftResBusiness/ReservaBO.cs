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
        private ReservaClient reservaClienteSOAP;

        public ReservaBO()
        {
            // Usa la configuración definida en web.config (name="ReservaPort")
            this.reservaClienteSOAP = new ReservaClient();
        }

        public int Insertar(reservaDTO reserva)
        {
            return this.reservaClienteSOAP.insertar(reserva);
        }

        public reservaDTO ObtenerPorID(int reservaID)
        {
            return this.reservaClienteSOAP.obtenerPorId(reservaID);
        }

        public int Modificar(reservaDTO reserva)
        {
            return this.reservaClienteSOAP.modificar(reserva);
        }

        public int Eliminar(reservaDTO reserva)
        {
            return this.reservaClienteSOAP.eliminar(reserva);
        }

        public BindingList<reservaDTO> Listar(reservaParametros parametros)
        {
            var lista = this.reservaClienteSOAP.listar(parametros);

            if (lista == null)
                return new BindingList<reservaDTO>();   // devuelve lista vacía si el servicio retorna null

            return new BindingList<reservaDTO>(lista);
        }
    }

}
