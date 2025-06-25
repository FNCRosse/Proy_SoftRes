using SoftResBusiness.TipoDocumentoWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SoftResBusiness
{
    public class TipoDocumentoBO
    {
        private TipoDocumentoClient tDocClienteSOAP;

        public TipoDocumentoBO()
        {
            // Usa la configuración declarada en web.config (name="TipoDocumentoPort")
            this.tDocClienteSOAP = new TipoDocumentoClient();
        }

        public int Insertar(tipoDocumentoDTO tdoc)
        {
            return this.tDocClienteSOAP.insertar(tdoc);
        }

        public tipoDocumentoDTO ObtenerPorID(int tdocID)
        {
            return this.tDocClienteSOAP.obtenerPorId(tdocID);
        }

        public int Modificar(tipoDocumentoDTO tdoc)
        {
            return this.tDocClienteSOAP.modificar(tdoc);
        }

        public int Eliminar(tipoDocumentoDTO tdoc)
        {
            return this.tDocClienteSOAP.eliminar(tdoc);
        }

        public BindingList<tipoDocumentoDTO> Listar()
        {
            var lista = this.tDocClienteSOAP.listar();

            if (lista == null)
                return new BindingList<tipoDocumentoDTO>();   // lista vacía sin error

            return new BindingList<tipoDocumentoDTO>(lista);
        }
    }
}
