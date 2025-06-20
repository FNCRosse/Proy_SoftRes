using SoftResBusiness.UsuarioWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SoftResBusiness
{
    public class UsuarioBO
    {
        private UsuarioClient usuarioClienteWA;

        public UsuarioBO()
        {
            var endpoint = new EndpointAddress("http://localhost:8080/SoftResWSCliente/usuarios");
            var binding = new BasicHttpBinding();
            this.usuarioClienteWA = new UsuarioClient(binding, endpoint);
        }

        public int Insertar(usuariosDTO usuario)
        {
            return this.usuarioClienteWA.insertar(usuario);
        }
        public usuariosDTO ObtenerPorID(int usuarioID)
        {
            return this.usuarioClienteWA.obtenerPorId(usuarioID);
        }
        public int Modificar(usuariosDTO usuario)
        {
            return this.usuarioClienteWA.modificar(usuario);
        }

        public int Eliminar(usuariosDTO usuario)
        {
            return this.usuarioClienteWA.eliminar(usuario);
        }

        public BindingList<usuariosDTO> Listar(usuariosParametros parametros)
        {
            usuariosDTO[] lista = this.usuarioClienteWA.listar(parametros);
            return new BindingList<usuariosDTO>(lista);
        }
    }
}
