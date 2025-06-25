using SoftResBusiness.ComentarioWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SoftResBusiness
{
    public class ComentarioBO
    {
        private ComentarioClient comentarioClienteSOAP;

        public ComentarioBO()
        {
            // Usa la configuración del web.config (name="ComentarioPort")
            this.comentarioClienteSOAP = new ComentarioClient();
        }

        public int Insertar(comentariosDTO coment)
        {
            return this.comentarioClienteSOAP.insertar(coment);
        }

        public comentariosDTO ObtenerPorID(int comentID)
        {
            return this.comentarioClienteSOAP.obtenerPorId(comentID);
        }

        public int Modificar(comentariosDTO coment)
        {
            return this.comentarioClienteSOAP.modificar(coment);
        }

        public int Eliminar(comentariosDTO coment)
        {
            return this.comentarioClienteSOAP.eliminar(coment);
        }

        public BindingList<comentariosDTO> Listar(comentarioParametros parametros)
        {
            var lista = this.comentarioClienteSOAP.listar(parametros);

            if (lista == null)
                return new BindingList<comentariosDTO>(); // retorna lista vacía sin error

            return new BindingList<comentariosDTO>(lista);
        }
    }

}
