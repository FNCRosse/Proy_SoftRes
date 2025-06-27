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
        private MotivoCancelacionClient motivoCancelacionClienteSOAP;

        public MotivoCancelacionBO()
        {
            // Usa la configuración del web.config (name="MotivoCancelacionPort")
            this.motivoCancelacionClienteSOAP = new MotivoCancelacionClient();
        }

        public int Insertar(motivosCancelacionDTO mCancelacion)
        {
            return this.motivoCancelacionClienteSOAP.insertar(mCancelacion);
        }

        public motivosCancelacionDTO ObtenerPorID(int mCancelacionID)
        {
            return this.motivoCancelacionClienteSOAP.obtenerPorId(mCancelacionID);
        }

        public int Modificar(motivosCancelacionDTO mCancelacion)
        {
            return this.motivoCancelacionClienteSOAP.modificar(mCancelacion);
        }

        public int Eliminar(motivosCancelacionDTO mCancelacion)
        {
            return this.motivoCancelacionClienteSOAP.eliminar(mCancelacion);
        }

        public BindingList<motivosCancelacionDTO> Listar()
        {
            var lista = this.motivoCancelacionClienteSOAP.listar();

            if (lista == null)
                return new BindingList<motivosCancelacionDTO>(); // lista vacía sin error

            return new BindingList<motivosCancelacionDTO>(lista);
        }
    }
}
