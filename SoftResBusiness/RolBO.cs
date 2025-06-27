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
        private RolClient rolClienteSOAP;

        public RolBO()
        {
            // Usa la configuración declarada en web.config (name="RolPort")
            this.rolClienteSOAP = new RolClient();
        }

        public int Insertar(rolDTO rol)
        {
            return this.rolClienteSOAP.insertar(rol);
        }

        public rolDTO ObtenerPorID(int rolID)
        {
            return this.rolClienteSOAP.obtenerPorId(rolID);
        }

        public int Modificar(rolDTO rol)
        {
            return this.rolClienteSOAP.modificar(rol);
        }

        public int Eliminar(rolDTO rol)
        {
            return this.rolClienteSOAP.eliminar(rol);
        }

        public BindingList<rolDTO> Listar()
        {
            var lista = this.rolClienteSOAP.listar();

            if (lista == null)
                return new BindingList<rolDTO>();   // retorna lista vacía sin error

            return new BindingList<rolDTO>(lista);
        }
    }
}
