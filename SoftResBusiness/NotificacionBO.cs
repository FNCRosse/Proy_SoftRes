using SoftResBusiness.NotificacionWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SoftResBusiness
{
    public class NotificacionBO
    {
        private NotificacionClient notificacionClienteWA;

        public NotificacionBO()
        {
            var endpoint = new EndpointAddress("http://localhost:8080/SoftResWSCliente/notificaciones");
            var binding = new BasicHttpBinding();
            this.notificacionClienteWA = new NotificacionClient(binding, endpoint);
        }

        public int Insertar(notificacionDTO notificacion)
        {
            return this.notificacionClienteWA.insertar(notificacion);
        }
        public notificacionDTO ObtenerPorID(int notificacionID)
        {
            return this.notificacionClienteWA.obtenerPorId(notificacionID);
        }
        public int Modificar(notificacionDTO notificacion)
        {
            return this.notificacionClienteWA.modificar(notificacion);
        }

        public int Eliminar(notificacionDTO notificacion)
        {
            return this.notificacionClienteWA.eliminar(notificacion);
        }

        public BindingList<notificacionDTO> Listar(notificacionParametros parametros)
        {
            notificacionDTO[] lista = this.notificacionClienteWA.listar(parametros);
            return new BindingList<notificacionDTO>(lista);
        }
    }
}
