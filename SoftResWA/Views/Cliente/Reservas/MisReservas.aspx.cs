using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using usuariosDTO = SoftResBusiness.UsuarioWSClient.usuariosDTO;

namespace SoftResWA.Views.Cliente.Reservas
{
    public partial class MisReservas : System.Web.UI.Page
    {
        private ReservaBO reservaBO;
        private LocalBO localBO;
        private BindingList<reservaDTO> listadoReservas;
        private BindingList<SoftResBusiness.LocalWSClient.localDTO> listadoLocales;
        private int idUsuarioActual = 1; // TODO: Obtener del Session

        public ReservaBO ReservaBO { get => reservaBO; set => reservaBO = value; }
        public LocalBO LocalBO { get => localBO; set => localBO = value; }
        public BindingList<reservaDTO> ListadoReservas { get => listadoReservas; set => listadoReservas = value; }
        public BindingList<SoftResBusiness.LocalWSClient.localDTO> ListadoLocales { get => listadoLocales; set => listadoLocales = value; }

        public MisReservas()
        {
            this.reservaBO = new ReservaBO();
            this.localBO = new LocalBO();
        }

        private void CargarDatos()
        {
            try
            {
                // Inicializar listas vacías por defecto
                this.listadoReservas = new BindingList<reservaDTO>();
                this.listadoLocales = new BindingList<SoftResBusiness.LocalWSClient.localDTO>();

                // Cargar reservas del usuario actual
                reservaParametros parametros = new reservaParametros();
                // Note: reservaParametros no tiene idUsuario, se filtra por DNI
                parametros.dniCliente = "12345678"; // TODO: Obtener DNI del usuario actual
                parametros.estadoSpecified = false;
                parametros.tipoReservaSpecified = false;
                parametros.fechaInicioSpecified = false;
                parametros.fechaFinSpecified = false;
                parametros.idLocalSpecified = false;
                
                var reservas = this.reservaBO.Listar(parametros);
                if (reservas != null)
                {
                    this.listadoReservas = reservas;
                }

                // Cargar locales activos
                SoftResBusiness.LocalWSClient.localParametros lParametros = new SoftResBusiness.LocalWSClient.localParametros();
                lParametros.estadoSpecified = true;
                lParametros.estado = true;
                
                var locales = this.localBO.Listar(lParametros);
                if (locales != null)
                {
                    this.listadoLocales = locales;
                }
            }
            catch (Exception ex)
            {
                // Asegurar que las listas estén inicializadas aunque haya error
                this.listadoReservas = this.listadoReservas ?? new BindingList<reservaDTO>();
                this.listadoLocales = this.listadoLocales ?? new BindingList<SoftResBusiness.LocalWSClient.localDTO>();

                // Log error y mostrar mensaje genérico
                System.Diagnostics.Debug.WriteLine($"Error al cargar datos: {ex.Message}");
                CargarReservasDemo(); // Fallback a datos demo
            }
        }

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