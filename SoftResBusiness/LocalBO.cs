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
        private LocalClient LocalClienteWA;

        public LocalBO()
        {
            var endpoint = new EndpointAddress("http://localhost:8080/SoftResWSCliente/locales");
            var binding = new BasicHttpBinding();
            this.LocalClienteWA = new LocalClient(binding, endpoint);
        }

        public int Insertar(localDTO local)
        {
            return this.LocalClienteWA.insertar(local);
        }
        public localDTO ObtenerPorID(int localID)
        {
            return this.LocalClienteWA.obtenerPorId(localID);
        }
        public int Modificar(localDTO local)
        {
            return this.LocalClienteWA.modificar(local);
        }

        public int Eliminar(localDTO local)
        {
            return this.LocalClienteWA.eliminar(local);
        }

        public BindingList<localDTO> Listar(localParametros parametros)
        {

            localDTO[] lista = this.LocalClienteWA.listar(parametros);

            if (lista == null)
                return new BindingList<localDTO>(); // retorna una lista vacía sin error

            return new BindingList<localDTO>(lista);
        }
    }
}
