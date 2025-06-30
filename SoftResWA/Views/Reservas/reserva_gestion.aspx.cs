using SoftResBusiness;
using SoftResBusiness.FilaEsperaWSClient;
using SoftResBusiness.MesaWSClient;
using SoftResBusiness.MotivoCancelacionWSClient;
using SoftResBusiness.ReservaWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Reservas
{
    public partial class reserva_gestion : System.Web.UI.Page
    {

        private ReservaBO reservaBO;
        private UsuarioBO usuarioBO;
        private LocalBO localBO;
        private BindingList<reservaDTO> listadoReserva;

        public ReservaBO ReservaBO { get => reservaBO; set => reservaBO = value; }
        public UsuarioBO UsuarioBo { get => usuarioBO; set => usuarioBO = value; }
        public LocalBO LocalBO { get => localBO; set => localBO = value; }
        public BindingList<reservaDTO> ListadoReserva { get => listadoReserva; set => value = listadoReserva; }

        // CONSTRUCTOR

        public reserva_gestion()
        {
            this.reservaBO = new ReservaBO();
            this.usuarioBO = new UsuarioBO();
            this.localBO = new LocalBO();
            reservaParametros parametros = new reservaParametros();
            parametros.tipoReservaSpecified = false;
            parametros.estadoSpecified = false;
            parametros.fechaFinSpecified = false;
            parametros.fechaInicioSpecified = false;
            parametros.idLocalSpecified = false;
            this.listadoReserva = this.reservaBO.Listar(parametros);
        }

        //CONFIGURACION VISUAL DE LISTADOS
        protected List<object> ConfigurarListado(BindingList<reservaDTO> lista)
        {
            var listaAdaptada = lista.Select(l => new
            {
                TipoReserva = l.tipoReservaSpecified ? ToString(l.tipoReserva) : " ",
                Fecha = l.fecha_Hora,
                Hora = l.fecha_Hora,
                Local = l.local,
                Solicitante = l.usuario,
                Observaciones = l.observaciones,
                MotivoCancelacion = l.motivoCancelacion?.descripcion??" ",
                UbicacionMesa = ,
                FilaEspera = l.filaEspera,
                Estado = l.estado,
            }).ToList<Object>();
            return listaAdaptada;
        }

        //FUNCIONES GENERALES
        private void MostrarResultado(bool exito, string entidad, string modo)
        {
            string accion = (modo == "modificar") ? "modificada" :
                            (modo == "eliminar") ? "eliminada" : "registrada";

            string accionNo = (modo == "modificar") ? "modificar" :
                              (modo == "eliminar") ? "eliminar" : "registrar";

            string baseMensaje = exito ? $"La {entidad} fue {accion}" : $"No se pudo {accionNo} la {entidad}";
            string tipo = exito ? "success" : "warning";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje",
                $"Swal.fire('¡{entidad} {(exito ? accion : $"NO {accion}")}!', '{baseMensaje} correctamente.', '{tipo}');", true);
        }

        protected void dgv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem;

                // Obtener estadoBool vía reflexión
                bool estado = true; // Por defecto activo
                var prop = dataItem.GetType().GetProperty("estadoBool");
                if (prop != null)
                {
                    estado = (bool)prop.GetValue(dataItem);
                }

                int idReserva = (int)dataItem.GetType().GetProperty("idReserva")?.GetValue(dataItem);

                LinkButton btnModificar = (LinkButton)e.Row.FindControl("btnModificar");
                LinkButton btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");

                btnModificar.Visible = estado;
                btnEliminar.Visible = estado;

                if (estado)
                {
                    btnEliminar.OnClientClick = $"return confirmarEliminacion({idReserva}, '{hdnIdEliminar.ClientID}', '{btnEliminarReserva.ReservaID}');";
                }
            }
        }

        private void CargarDropDownList(DropDownList ddl, object dataSource, string textField, string valueField, string textoDefault)
        {
            ddl.DataSource = dataSource;
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(textoDefault, ""));
        }

        //FUNCIONES DE RESERVA
        private void CargarEstadosMesa()
        {
            var estadosReserva = Enum.GetNames(typeof(SoftResBusiness.ReservaWSClient.estadoReserva))
                .Select(d => new { nombre = d, id = d })
                .ToList();

            this.CargarDropDownList(ddlEstadoReserva, estadosReserva, "nombre", "id", "Seleccionar...");
            this.CargarDropDownList(ddlEstadoFiltro, estadosReserva, "nombre", "id", "Todos");
        }

        private void CargarUsuario()
        {
            try
            {
                SoftResBusiness.UsuarioWSClient.usuariosParametros parametros = new SoftResBusiness.UsuarioWSClient.usuariosParametros();
                parametros.esClienteSpecified = false
                parametros.estadoSpecified = true;
                parametros.estado = true;

                var usuario = this.usuarioBO.Listar(parametros);
                this.CargarDropDownList(ddlUsuario, usuario, "nombre", "idUsuario", "Seleccionar...");
                this.CargarDropDownList(ddlUsuarioFiltro, usuario, "nombre", "idUsuario", "Todos");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorUsuario",
                    $"Swal.fire('Error', 'No se pudo cargar el usuario: {ex.Message}', 'error');", true);
            }
        }

        private void CargarLocales()
        {
            try
            {
                SoftResBusiness.LocalWSClient.localParametros parametros = new SoftResBusiness.LocalWSClient.localParametros();
                parametros.estadoSpecified = true;
                parametros.estado = true;
                parametros.idSedeSpecified = false;
                parametros.nombre = null;
                var locales = this.localBO.Listar(parametros);
                this.CargarDropDownList(ddlLocal, locales, "nombre", "idLocal", "Seleccionar...");
                this.CargarDropDownList(ddlLocalFiltro, locales, "nombre", "idLocal", "Todos");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorLocales",
                    $"Swal.fire('Error', 'No se pudieron cargar los locales: {ex.Message}', 'error');", true);
            }
        }

        private reservaDTO ConstruirDTO(reservaDTO reserva)
        {
            if(reserva == null)
                reserva = new reservaDTO();


        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}