using SoftResBusiness.HorarioAtencionWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SoftResBusiness
{
    public class HorarioAtencionBO
    {
        private HorarioAtencionClient horarioClienteSOAP;

        public HorarioAtencionBO()
        {
            // Usa la configuración del web.config (name="HorarioAtencionPort")
            this.horarioClienteSOAP = new HorarioAtencionClient();
        }

        public int Insertar(horarioAtencionDTO horario)
        {
            return this.horarioClienteSOAP.insertar(horario);
        }

        public horarioAtencionDTO ObtenerPorID(int horarioID)
        {
            return this.horarioClienteSOAP.obtenerPorId(horarioID);
        }

        public int Modificar(horarioAtencionDTO horario)
        {
            return this.horarioClienteSOAP.modificar(horario);
        }

        public int Eliminar(horarioAtencionDTO horario)
        {
            return this.horarioClienteSOAP.eliminar(horario);
        }

        public BindingList<horarioAtencionDTO> Listar(horarioParametros parametros)
        {
            var lista = this.horarioClienteSOAP.listar(parametros);

            if (lista == null)
                return new BindingList<horarioAtencionDTO>(); // retorna lista vacía sin error

            return new BindingList<horarioAtencionDTO>(lista);
        }
    }

}
