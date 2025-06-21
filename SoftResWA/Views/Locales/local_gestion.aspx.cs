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
                l.fechaModificacion,
                l.usuarioModificacion,
                Estado = l.estado ? "Activo" : "Inactivo"
            }).ToList<Object>();
            return listaAdaptada;
        }
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
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
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
        protected void btnGuardarLocal_Click(object sender, EventArgs e)
        {
            var local = new SoftResBusiness.LocalWSClient.localDTO();
            var sede = new SoftResBusiness.LocalWSClient.sedeDTO();
            sede.idSedeSpecified = true;
            sede.idSede = int.Parse(ddlSedeOp.SelectedValue);
            local.sede = sede;
            local.nombre = txtNombreLocal.Text;
            local.direccion = txtDireccionLocal.Text;
            local.telefono = txtTelefonoLocal.Text;
            local.fechaCreacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            local.fechaCreacionSpecified = true;
            local.estadoSpecified = true;
            local.estado = true;
            if (this.localBO.Insertar(local) > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "registroExitoso", "Swal.fire('¡Local registrado!', 'El registro se completó correctamente.', 'success');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "registroExitoso", "Swal.fire('¡Local NO registrado!', 'El registro NO se completó correctamente.', 'success');", true);
            }

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

            localDTO local = this.localBO.ObtenerPorID(idLocal);
            if(local != null)
            {
                txtNombreLocal.Text = local.nombre;
                txtDireccionLocal.Text = local.direccion;
                txtTelefonoLocal.Text = local.telefono;
                ddlSedeOp.SelectedValue = local.sede.idSede.ToString();
                this.MostrarModal("modificar", "Modificar Local");
            }
        }
        protected void btn_eliminar_Click(object sender, EventArgs e)
        {
            int id = int.Parse(hdnIdEliminar.Value);
            localDTO local = this.localBO.ObtenerPorID(id);

            if(local != null)
            {
                this.localBO.Eliminar(local);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "eliminado",
                "Swal.fire('¡Eliminado!', 'El registro fue eliminado correctamente.', 'success');", true);
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            localParametros parametros = new localParametros();
            parametros.nombre = txtNombre.Text.Trim();
            if (string.IsNullOrEmpty(ddlEstado.SelectedValue))
            {
                parametros.estadoSpecified = false;
            }
            else
            {
                parametros.estadoSpecified = true;
                parametros.estado = ddlEstado.SelectedValue == "1"; 
            }
            if (string.IsNullOrEmpty(ddlSede.SelectedValue))
            {
                parametros.idSedeSpecified = false;
            }
            else
            {
                parametros.idSedeSpecified = true;
                parametros.idSede = int.Parse(ddlSede.SelectedValue);
            }

            // Llama a tu BO
            var lista = this.localBO.Listar(parametros);
            var listaAdaptada = this.ConfigurarListado(lista);

            // Recarga el GridView
            dgvLocal.DataSource = listaAdaptada;
            dgvLocal.DataBind();
        }
    }
}