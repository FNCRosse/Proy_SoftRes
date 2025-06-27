using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SoftResBusiness.SedeWSClient;

namespace SoftResBusiness
{
    public class SedeBO
    {
        private SedeClient sedeClienteSOAP;

        public SedeBO()
        {
            // Usa la configuración del web.config (name="SedePort")
            this.sedeClienteSOAP = new SedeClient();
        }

        public int Insertar(sedeDTO sede)
        {
            return this.sedeClienteSOAP.insertar(sede);
        }

        public sedeDTO ObtenerPorID(int sedeID)
        {
            return this.sedeClienteSOAP.obtenerPorId(sedeID);
        }

        public int Modificar(sedeDTO sede)
        {
            return this.sedeClienteSOAP.modificar(sede);
        }

        public int Eliminar(sedeDTO sede)
        {
            return this.sedeClienteSOAP.eliminar(sede);
        }

        public BindingList<sedeDTO> Listar(sedeParametros parametros)
        {
            var lista = this.sedeClienteSOAP.listar(parametros);

            if (lista == null)
                return new BindingList<sedeDTO>(); // retorna una lista vacía sin error

            return new BindingList<sedeDTO>(lista);
        }
    }

}
