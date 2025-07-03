using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftResBusiness.DashBoardWSClient;

namespace SoftResWA
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Crear una instancia del cliente del servicio SOAP
                DashBoardClient client = new DashBoardClient();

                // Llamada al servicio SOAP para obtener la cantidad de reservas diarias
                int cantidadReservasDiarias = client.obtenerCantidadReservasDiarias();
                lblCantidadReservasDiarias.Text = cantidadReservasDiarias.ToString();

                // Llamada al servicio SOAP para obtener la cantidad de reservas semanales
                int cantidadReservasSemanales = client.obtenerCantidadReservasSemanales();
                lblCantidadReservasSemanales.Text = cantidadReservasSemanales.ToString();

                // Llamada al servicio SOAP para obtener el porcentaje de ocupación de mesas
                double porcentajeOcupacionMesas = client.obtenerPorcentajeOcupacionMesas();
                lblPorcentajeOcupacionMesas.Text = porcentajeOcupacionMesas.ToString("0.00") + "%";

                // Llamada al servicio SOAP para obtener la cantidad de cancelaciones recientes
                int cantidadCancelacionesRecientes = client.obtenerCantidadCancelacionesRecientes();
                lblCantidadCancelacionesRecientes.Text = cantidadCancelacionesRecientes.ToString();
            }
            catch (Exception ex)
            {
                // Manejo de errores
                lblError.Text = "Error al obtener los datos del Dashboard: " + ex.Message;
            }
        }
    }
}