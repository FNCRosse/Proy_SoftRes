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
        private TipoDocumentoClient tDocClienteWA;

        public TipoDocumentoBO()
        {
            var endpoint = new EndpointAddress("http://localhost:8080/SoftResWSCliente/tipoDocumento");
            var binding = new BasicHttpBinding();
            this.tDocClienteWA = new TipoDocumentoClient(binding, endpoint);
        }

        public int Insertar(tipoDocumentoDTO tdoc)
        {
            return this.tDocClienteWA.insertar(tdoc);
        }
        public tipoDocumentoDTO ObtenerPorID(int tdocID)
        {
            return this.tDocClienteWA.obtenerPorId(tdocID);
        }
        public int Modificar(tipoDocumentoDTO tdoc)
        {
            return this.tDocClienteWA.modificar(tdoc);
        }

        public int Eliminar(tipoDocumentoDTO tdoc)
        {
            return this.tDocClienteWA.eliminar(tdoc);
        }

        public BindingList<tipoDocumentoDTO> Listar()
        {
            tipoDocumentoDTO[] lista = this.tDocClienteWA.listar();
            return new BindingList<tipoDocumentoDTO>(lista);
        }
    }
}
