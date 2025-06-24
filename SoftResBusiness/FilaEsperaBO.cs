using SoftResBusiness.FilaEsperaWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SoftResBusiness
{
    public class FilaEsperaBO
    {
        private FilaEsperaClient filaEsperaClienteWA;

        public FilaEsperaBO()
        {
            var endpoint = new EndpointAddress("http://localhost:8080/SoftResWSCliente/filaEspera");
            var binding = new BasicHttpBinding();
            this.filaEsperaClienteWA = new FilaEsperaClient(binding, endpoint);
        }

        public int Insertar(filaEsperaDTO filaespera)
        {
            return this.filaEsperaClienteWA.insertar(filaespera);
        }
        public filaEsperaDTO ObtenerPorID(int filaesperaID)
        {
            return this.filaEsperaClienteWA.obtenerPorId(filaesperaID);
        }
        public int Modificar(filaEsperaDTO filaespera)
        {
            return this.filaEsperaClienteWA.modificar(filaespera);
        }

        public int Eliminar(filaEsperaDTO filaespera)
        {
            return this.filaEsperaClienteWA.eliminar(filaespera);
        }

        public BindingList<filaEsperaDTO> Listar(filaEsperaParametros parametros)
        {
            filaEsperaDTO[] lista = this.filaEsperaClienteWA.listar(parametros);
            return new BindingList<filaEsperaDTO>(lista);
        }
    }
}
