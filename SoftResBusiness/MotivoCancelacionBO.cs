using SoftResBusiness.MotivoCancelacionWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SoftResBusiness
{
    public class MotivoCancelacionBO
    {
        private MotivoCancelacionClient mCancelacionClienteWA;

        public MotivoCancelacionBO()
        {
            var endpoint = new EndpointAddress("http://localhost:8080/SoftResWSCliente/motivoCancelacion");
            var binding = new BasicHttpBinding();
            this.mCancelacionClienteWA = new MotivoCancelacionClient(binding, endpoint);
        }

        public int Insertar(motivosCancelacionDTO mCancelacion)
        {
            return this.mCancelacionClienteWA.insertar(mCancelacion);
        }
        public motivosCancelacionDTO ObtenerPorID(int mCancelacionID)
        {
            return this.mCancelacionClienteWA.obtenerPorId(mCancelacionID);
        }
        public int Modificar(motivosCancelacionDTO mCancelacion)
        {
            return this.mCancelacionClienteWA.modificar(mCancelacion);
        }

        public int Eliminar(motivosCancelacionDTO mCancelacion)
        {
            return this.mCancelacionClienteWA.eliminar(mCancelacion);
        }

        public BindingList<motivosCancelacionDTO> Listar()
        {
            motivosCancelacionDTO[] lista = this.mCancelacionClienteWA.listar();
            return new BindingList<motivosCancelacionDTO>(lista);
        }
    }
}
