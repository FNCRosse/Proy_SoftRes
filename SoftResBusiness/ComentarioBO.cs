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
        private ComentarioClient  comentClienteWA;

        public ComentarioBO()
        {
            var endpoint = new EndpointAddress("http://localhost:8080/SoftResWSCliente/comentarios");
            var binding = new BasicHttpBinding();
            this.comentClienteWA = new ComentarioClient(binding, endpoint);
        }

        public int Insertar(comentariosDTO coment)
        {
            return this.comentClienteWA.insertar(coment);
        }
        public comentariosDTO ObtenerPorID(int comentID)
        {
            return this.comentClienteWA.obtenerPorId(comentID);
        }
        public int Modificar(comentariosDTO coment)
        {
            return this.comentClienteWA.modificar(coment);
        }

        public int Eliminar(comentariosDTO coment)
        {
            return this.comentClienteWA.eliminar(coment);
        }

        public BindingList<comentariosDTO> Listar(comentarioParametros parametros)
        {
            comentariosDTO[] lista = this.comentClienteWA.listar(parametros);
            if (lista == null)
                return new BindingList<comentariosDTO>();
            return new BindingList<comentariosDTO>(lista);
        }
    }
}
