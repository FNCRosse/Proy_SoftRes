using SoftResBusiness.RolWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SoftResBusiness
{
    public class RolBO
    {
        private RolClient rolClienteWA;

        public RolBO()
        {
            var endpoint = new EndpointAddress("http://localhost:8080/SoftResWSCliente/rol");
            var binding = new BasicHttpBinding();
            this.rolClienteWA = new RolClient(binding, endpoint);
        }

        public int Insertar(rolDTO rol)
        {
            return this.rolClienteWA.insertar(rol);
        }
        public rolDTO ObtenerPorID(int rolID)
        {
            return this.rolClienteWA.obtenerPorId(rolID);
        }
        public int Modificar(rolDTO rol)
        {
            return this.rolClienteWA.modificar(rol);
        }

        public int Eliminar(rolDTO rol)
        {
            return this.rolClienteWA.eliminar(rol);
        }

        public BindingList<rolDTO> Listar()
        {
            rolDTO[] lista = this.rolClienteWA.listar();
            return new BindingList<rolDTO>(lista);
        }
    }
}
