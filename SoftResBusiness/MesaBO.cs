using SoftResBusiness.MesaWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SoftResBusiness
{
    public class MesaBO
    {
        private MesaClient mesaClienteSOAP;

        public MesaBO()
        {
            // Usa la configuración definida en web.config (name="MesaPort")
            this.mesaClienteSOAP = new MesaClient();
        }

        public int Insertar(mesaDTO mesa)
        {
            return this.mesaClienteSOAP.insertar(mesa);
        }

        public mesaDTO ObtenerPorID(int mesaID)
        {
            return this.mesaClienteSOAP.obtenerPorId(mesaID);
        }

        public int Modificar(mesaDTO mesa)
        {
            return this.mesaClienteSOAP.modificar(mesa);
        }

        public int Eliminar(mesaDTO mesa)
        {
            return this.mesaClienteSOAP.eliminar(mesa);
        }

        public BindingList<mesaDTO> Listar(mesaParametros parametros)
        {
            var lista = this.mesaClienteSOAP.listar(parametros);

            if (lista == null)
                return new BindingList<mesaDTO>();   // lista vacía si el servicio devuelve null

            return new BindingList<mesaDTO>(lista);
        }
    }
}
