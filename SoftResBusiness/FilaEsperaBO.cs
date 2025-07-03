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
        private FilaEsperaClient filaEsperaClienteSOAP;

        public FilaEsperaBO()
        {
            // Usa la configuración del web.config (name="FilaEsperaPort")
            this.filaEsperaClienteSOAP = new FilaEsperaClient();
        }

        public filaEsperaDTO Insertar(filaEsperaDTO filaespera)
        {
            return this.filaEsperaClienteSOAP.insertar(filaespera);
        }

        public filaEsperaDTO ObtenerPorID(int filaesperaID,int usuarioID)
        {
            return this.filaEsperaClienteSOAP.obtenerPorId(filaesperaID,usuarioID);
        }

        public int Modificar(filaEsperaDTO filaespera)
        {
            return this.filaEsperaClienteSOAP.modificar(filaespera);
        }

        public int Eliminar(filaEsperaDTO filaespera)
        {
            return this.filaEsperaClienteSOAP.eliminar(filaespera);
        }

        public BindingList<filaEsperaDTO> Listar(filaEsperaParametros parametros)
        {
            var lista = this.filaEsperaClienteSOAP.listar(parametros);

            if (lista == null)
                return new BindingList<filaEsperaDTO>(); // retorna lista vacía sin error

            return new BindingList<filaEsperaDTO>(lista);
        }
    }
}
