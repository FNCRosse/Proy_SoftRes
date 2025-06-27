using SoftResBusiness.LocalWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SoftResBusiness
{
    public class LocalBO
    {
        private LocalClient localClienteSOAP;

        public LocalBO()
        {
            // Usa la configuración del web.config (name="LocalPort")
            this.localClienteSOAP = new LocalClient();
        }

        public int Insertar(localDTO local)
        {
            return this.localClienteSOAP.insertar(local);
        }

        public localDTO ObtenerPorID(int localID)
        {
            return this.localClienteSOAP.obtenerPorId(localID);
        }

        public int Modificar(localDTO local)
        {
            return this.localClienteSOAP.modificar(local);
        }

        public int Eliminar(localDTO local)
        {
            return this.localClienteSOAP.eliminar(local);
        }

        public BindingList<localDTO> Listar(localParametros parametros)
        {

            localDTO[] lista = this.localClienteSOAP.listar(parametros);

            if (lista == null)
                return new BindingList<localDTO>(); // retorna una lista vacía sin error

            return new BindingList<localDTO>(lista);
        }
    }
}
