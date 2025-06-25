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
        private HorarioxSedeClient hxsClienteSOAP;

        public HorarioxSedeBO()
        {
            // Usa la configuración del web.config (name="HorarioxSedePort")
            this.hxsClienteSOAP = new HorarioxSedeClient();
        }

        public int Insertar(horariosxSedesDTO horario)
        {
            return this.hxsClienteSOAP.insertar(horario);
        }

        public int Eliminar(horariosxSedesDTO horario)
        {
            return this.hxsClienteSOAP.eliminar(horario);
        }

        public BindingList<horariosxSedesDTO> Listar(int idSede)
        {
            var lista = this.hxsClienteSOAP.listar(idSede);

            if (lista == null)
                return new BindingList<horariosxSedesDTO>(); // retorna lista vacía sin error

            return new BindingList<horariosxSedesDTO>(lista);
        }
    }
}
