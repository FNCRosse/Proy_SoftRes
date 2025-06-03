using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Cliente.Reservas
{
    public partial class MisReservas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarReservasDemo();
        }
        private void CargarReservasDemo()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ReservaID");
            dt.Columns.Add("Fecha", typeof(DateTime));
            dt.Columns.Add("Hora");
            dt.Columns.Add("Local");
            dt.Columns.Add("Estado");
            dt.Columns.Add("Personas");
            dt.Columns.Add("Mesas");
            dt.Columns.Add("Ubicacion");
            dt.Columns.Add("TipoReserva");
            dt.Columns.Add("NombreEvento");
            dt.Columns.Add("DescripcionEvento");
            dt.Columns.Add("Observaciones");

            dt.Rows.Add("1", DateTime.Now, "4:00pm", "San Miguel", "Confirmado", 4, 1, "Ventana", "Normal", "", "", "Ninguna");
            dt.Rows.Add("2", DateTime.Now, "4:00pm", "San Miguel", "Confirmado", 4, 1, "Ventana", "Evento", "Cumpleaños", "Cumpleaños de mi hijo", "Ninguna");
            dt.Rows.Add("3", DateTime.Now, "4:00pm", "San Miguel", "Pendiente", 4, 1, "Ventana", "Normal", "", "", "Ninguna");
            dt.Rows.Add("4", DateTime.Now, "4:00pm", "San Miguel", "Cancelada", 4, 1, "Ventana", "Normal", "", "", "Ninguna");

            rptReservas.DataSource = dt;
            rptReservas.DataBind();
        }

        public string GetBotonPorEstado(string estado)
        {
            if (estado == "Confirmado")
                return "<button class='btn btn-danger rounded-pill px-4'>Editar</button>";
            else if (estado == "Pendiente")
                return "<button class='btn btn-warning rounded-pill px-4 text-white'>Confirmar</button>";
            else
                return "";
        }
    }
}