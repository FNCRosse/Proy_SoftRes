using SoftResBusiness.HorarioxSedeWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SoftResBusiness
{
    public class HorarioxSedeBO
    {
        private HorarioxSedeClient hxsClienteWA;

        public HorarioxSedeBO()
        {
            var endpoint = new EndpointAddress("http://localhost:8080/SoftResWSCliente/horarioxsede");
            var binding = new BasicHttpBinding();
            this.hxsClienteWA = new HorarioxSedeClient(binding, endpoint);
        }

        public int Insertar(horariosxSedesDTO horario)
        {
            return this.hxsClienteWA.insertar(horario);
        }

        public int Eliminar(horariosxSedesDTO horario)
        {
            return this.hxsClienteWA.eliminar(horario);
        }

        public BindingList<horariosxSedesDTO> Listar(int idSede)
        {
            horariosxSedesDTO[] lista = this.hxsClienteWA.listar(idSede);
            return new BindingList<horariosxSedesDTO>(lista);
        }
    }
}
