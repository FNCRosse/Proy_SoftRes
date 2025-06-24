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
        private HorarioAtencionClient horarioClienteWA;

        public HorarioAtencionBO()
        {
            var endpoint = new EndpointAddress("http://localhost:8080/SoftResWSCliente/horarioAtencion");
            var binding = new BasicHttpBinding();
            this.horarioClienteWA = new HorarioAtencionClient(binding, endpoint);
        }

        public int Insertar(horarioAtencionDTO horario)
        {
            return this.horarioClienteWA.insertar(horario);
        }
        public horarioAtencionDTO ObtenerPorID(int horarioID)
        {
            return this.horarioClienteWA.obtenerPorId(horarioID);
        }
        public int Modificar(horarioAtencionDTO horario)
        {
            return this.horarioClienteWA.modificar(horario);
        }

        public int Eliminar(horarioAtencionDTO horario)
        {
            return this.horarioClienteWA.eliminar(horario);
        }

        public BindingList<horarioAtencionDTO> Listar(horarioParametros parametros)
        {
            horarioAtencionDTO[] lista = this.horarioClienteWA.listar(parametros);
            return new BindingList<horarioAtencionDTO>(lista);
        }
    }
}
