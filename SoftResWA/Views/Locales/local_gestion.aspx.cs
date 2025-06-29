using SoftResBusiness;
using SoftResBusiness.LocalWSClient;
using SoftResBusiness.SedeWSClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Locales
{
    public partial class LocalGestion : System.Web.UI.Page
    {
        private LocalBO localBO;
        private SedeBO sedeBO;
        private BindingList<localDTO> listadoLocal;
        private BindingList<SoftResBusiness.SedeWSClient.sedeDTO> listaOpSedes;

        public LocalBO LocalBO { get => localBO; set => localBO = value; }
        public BindingList<localDTO> ListadoLocal { get => listadoLocal; set => listadoLocal = value; }
        public SedeBO SedeBO { get => sedeBO; set => sedeBO = value; }
        public BindingList<SoftResBusiness.SedeWSClient.sedeDTO> ListaOpSedes { get => listaOpSedes; set => listaOpSedes = value; }

        //CONSTRUCTOR
        public LocalGestion()
        {
            this.localBO = new LocalBO();
            this.sedeBO = new SedeBO();
            localParametros parametros = new localParametros();
            parametros.estadoSpecified = false;
            parametros.idSedeSpecified = false;
            parametros.nombre = null;
            this.listadoLocal = this.localBO.Listar(parametros);
            sedeParametros parametrosSede = new sedeParametros();
            parametrosSede.estadoSpecified = true;
            parametrosSede.nombre = null;
            parametrosSede.estado = true;
            this.listaOpSedes = this.sedeBO.Listar(parametrosSede);
        }

        //CONFIGURACION VISUAL DE LISTADOS
        protected List<object> ConfigurarListado(BindingList<localDTO> lista)
        {
            var listaAdaptada = lista.Select(l => new
            {
                l.idLocal,
                l.nombre,
                l.direccion,
                SedeNombre = l.sede?.nombre ?? "",
                l.telefono,
                l.cantidadMesas,
                l.fechaCreacion,
                l.usuarioCreacion,
                fechaModificacion = l.fechaModificacionSpecified ? l.fechaModificacion : (DateTime?)null,
                l.usuarioModificacion,
                estadoBool = l.estado,
                Estado = l.estado ? "Activo" : "Inactivo"
            }).ToList<Object>();
            return listaAdaptada;
        }

        //FUNCIONES GENERALES
        private void CargarDropDownList(DropDownList ddl, object dataSource, string textField, string valueField, string textoDefault)
        {
            ddl.DataSource = dataSource;
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(textoDefault, ""));
        }
        private void MostrarModal(string modo, string titulo)
        {
            hdnModoModal.Value = modo;

            string script = "setTimeout(function() {" +
                            $"document.getElementById('tituloModal').innerHTML = '<i class=\\\"fas fa-map-marker-alt me-2 text-danger\\\"></i>{titulo}';" +
                            "var modal = new bootstrap.Modal(document.getElementById('modalRegistrarLocal'));" +
                            "modal.show();" +
                            "}, 200);";

            ScriptManager.RegisterStartupScript(this, this.GetType(), $"mostrarModal_{modo}", script, true);
        }
        private void MostrarResultado(bool exito, string entidad, string modo)
        {
            string accion = (modo == "modificar") ? "modificado" :
                            (modo == "eliminar") ? "eliminado" : "registrado";

            string accionNo = (modo == "modificar") ? "modificar" :
                              (modo == "eliminar") ? "eliminar" : "registrar";

            string baseMensaje = exito ? $"El {accion}" : $"El {accionNo} NO";
            string tipo = exito ? "success" : "warning";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje",
                $"Swal.fire('¡{entidad} {(exito ? accion : $"NO {accion}")}!', '{baseMensaje} se completó correctamente.', '{tipo}');", true);
        }
        protected void dgvLocal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Esta funcion es para que se oculte los botones de modificar y cancelar cuando la entidad esta inactiva
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem;

                // Obtener estadoBool vía reflexión
                bool estado = false;
                var prop = dataItem.GetType().GetProperty("estadoBool");
                if (prop != null)
                {
                    estado = (bool)prop.GetValue(dataItem);
                }

                int idLocal = (int)dataItem.GetType().GetProperty("idLocal")?.GetValue(dataItem);

                LinkButton btnModificar = (LinkButton)e.Row.FindControl("btnModificar");
                LinkButton btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");

                btnModificar.Visible = estado;
                btnEliminar.Visible = estado;

                if (estado)
                {
                    btnEliminar.OnClientClick = $"return confirmarEliminacion({idLocal}, '{hdnIdEliminar.ClientID}', '{btnEliminarLocal.ClientID}');";
                }
            }
        }
        
        //FUNCIONES PARA LOCAL
        private localDTO ConstruirLocalDTO(localDTO local)
        {
            if (local == null)
                local = new SoftResBusiness.LocalWSClient.localDTO();
            var sede = new SoftResBusiness.LocalWSClient.sedeDTO();
            sede.idSede = int.Parse(ddlSedeOp.SelectedValue);
            sede.idSedeSpecified = true;
            local.sede = sede;
            local.nombre = txtNombreLocal.Text;
            local.direccion = txtDireccionLocal.Text;
            local.telefono = txtTelefonoLocal.Text;
            local.estado = true;
            local.estadoSpecified = true;
            return local;
        }
       
        //PAGE_LOAD
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            dgvLocal.RowDataBound += dgvLocal_RowDataBound;
            if (!IsPostBack)
            {
                //Aqui van para mostrar el listado de lcoales
                var listaAdaptada = this.ConfigurarListado(ListadoLocal);

                dgvLocal.DataSource = listaAdaptada;
                dgvLocal.DataBind();

                //Aqui los listados de los filtros para sede
                this.CargarDropDownList(ddlSede, ListaOpSedes, "nombre", "idSede", "-- Seleccione --");
                this.CargarDropDownList(ddlSedeOp, ListaOpSedes, "nombre", "idSede", "Seleccione Sede");
            }
        }

        //BOTONES
        protected void btnGuardarLocal_Click(object sender, EventArgs e)
        {
            string modo = hdnModoModal.Value;
            bool exito = false;
            if (modo == "registrar")
            {
                localDTO local = new SoftResBusiness.LocalWSClient.localDTO();
                local = ConstruirLocalDTO(local);
                local.fechaCreacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                local.fechaCreacionSpecified = true;
                local.fechaModificacionSpecified = false;
                local.usuarioCreacion = "admin"; // usar Session["usuario"] si aplica

                exito = this.localBO.Insertar(local) > 0;
            }
            else if (modo == "modificar")
            {
                int idLocal = int.Parse(hdnIdLocal.Value);
                localDTO local = this.localBO.ObtenerPorID(idLocal);

                local = ConstruirLocalDTO(local); // actualiza campos pero mantiene ID, creación, etc.
                local.idLocal = idLocal;
                local.idLocalSpecified = true;

                local.fechaModificacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                local.fechaModificacionSpecified = true;
                local.usuarioModificacion = "admin"; // usar Session["usuario"] si aplica

                exito = this.localBO.Modificar(local) > 0;
            }
            MostrarResultado(exito, "Local", modo);
            if (exito) btnBuscar_Click(sender, e);
        }
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            // Limpia campos
            txtNombreLocal.Text = "";
            txtDireccionLocal.Text = "";
            txtTelefonoLocal.Text = "";

            // Cambia título a "Registrar"
            this.MostrarModal("registrar", "Registrar Local");
        }
        protected void btnModificar_Command(object sender, CommandEventArgs e)
        {
            int idLocal = int.Parse(e.CommandArgument.ToString());
            if (idLocal > 0)
            {
                localDTO local = this.localBO.ObtenerPorID(idLocal);
                if (local != null)
                {
                    hdnIdLocal.Value = local.idLocal.ToString();
                    txtNombreLocal.Text = local.nombre;
                    txtDireccionLocal.Text = local.direccion;
                    txtTelefonoLocal.Text = local.telefono;
                    ddlSedeOp.SelectedValue = local.sede.idSede.ToString();
                    this.MostrarModal("modificar", "Modificar Local");
                }
            }
        }
        protected void btn_eliminar_Click(object sender, EventArgs e)
        {
            int id = int.Parse(hdnIdEliminar.Value);
            if (id > 0)
            {
                localDTO local = this.localBO.ObtenerPorID(id);
                if (local != null)
                {
                    local.idLocal = id;
                    local.idLocalSpecified = true;
                    bool exito = this.localBO.Eliminar(local) > 0;
                    MostrarResultado(exito, "Local", "eliminar");
                    if (exito) btnBuscar_Click(sender, e);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            localParametros parametros = new localParametros();
            parametros.nombre = txtNombre.Text.Trim();
            parametros.estadoSpecified = !string.IsNullOrEmpty(ddlEstado.SelectedValue);
            parametros.estado = ddlEstado.SelectedValue == "1";
            parametros.idSedeSpecified = !string.IsNullOrEmpty(ddlSede.SelectedValue);
            parametros.idSede = !string.IsNullOrEmpty(ddlSede.SelectedValue) ? int.Parse(ddlSede.SelectedValue) : 0;

            var lista = this.localBO.Listar(parametros);
            var listaAdaptada = this.ConfigurarListado(lista);
            dgvLocal.DataSource = listaAdaptada;
            dgvLocal.DataBind();
        }
    }
}