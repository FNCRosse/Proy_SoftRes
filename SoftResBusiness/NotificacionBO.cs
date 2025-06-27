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
        private NotificacionClient notificacionClienteSOAP;

        public NotificacionBO()
        {
            // Usa la configuración definida en el web.config (name="NotificacionPort")
            this.notificacionClienteSOAP = new NotificacionClient();
        }

        public int Insertar(notificacionDTO notificacion)
        {
            return this.notificacionClienteSOAP.insertar(notificacion);
        }

        public notificacionDTO ObtenerPorID(int notificacionID)
        {
            return this.notificacionClienteSOAP.obtenerPorId(notificacionID);
        }

        public int Modificar(notificacionDTO notificacion)
        {
            return this.notificacionClienteSOAP.modificar(notificacion);
        }

        public int Eliminar(notificacionDTO notificacion)
        {
            return this.notificacionClienteSOAP.eliminar(notificacion);
        }

        public BindingList<notificacionDTO> Listar(notificacionParametros parametros)
        {
            var lista = this.notificacionClienteSOAP.listar(parametros);

            if (lista == null)
                return new BindingList<notificacionDTO>();  // lista vacía sin error

            return new BindingList<notificacionDTO>(lista);
        }
    }

}
