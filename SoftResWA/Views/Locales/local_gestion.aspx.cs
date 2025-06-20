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
            parametros.estadoSpecified = true;
            parametros.idSedeSpecified = false;
            parametros.nombre = null;
            parametros.estado = true;
            parametros.idSede = null;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Aqui van para mostrar el listado de lcoales
                var listaAdaptada = this.ConfigurarListado(ListadoLocal);

                dgvLocal.DataSource = listaAdaptada;
                dgvLocal.DataBind();

                //Aqui los listados de los filtros para sede
                ddlSede.DataSource = ListaOpSedes;
                ddlSede.DataTextField = "nombre";
                ddlSede.DataValueField = "idSede";
                ddlSede.DataBind();
                ddlSede.Items.Insert(0, new ListItem("-- Seleccione --", "")); //Esto no es null, en setearlo en los parametros tenemos que ponerlo null

            }
        }
        protected void btnGuardarLocal_Click(object sender, EventArgs e)
        {
            // Aquí irá la lógica para guardar el local
            ScriptManager.RegisterStartupScript(this, this.GetType(), "registroExitoso", "Swal.fire('¡Local registrado!', 'El registro se completó correctamente.', 'success');", true);

        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            // Limpia campos
            txtNombreLocal.Text = "";
            txtDireccionLocal.Text = "";
            txtTelefonoLocal.Text = "";

            // Cambia título a "Registrar"
            hdnModoModal.Value = "registrar";
            string script = "setTimeout(function() {" +
                "document.getElementById('tituloModal').innerHTML = '<i class=\\\"fas fa-map-marker-alt me-2 text-danger\\\"></i>Registrar Local';" +
                "var modal = new bootstrap.Modal(document.getElementById('modalRegistrarLocal'));" +
                "modal.show();" +
            "}, 200);";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModalNuevo", script, true);
        }
        protected void btnModificar_Command(object sender, CommandEventArgs e)
        {
            int idLocal = int.Parse(e.CommandArgument.ToString());

            //var sede = sedeBO.ObtenerPorId(idSede);

            //txtNombreSede.Text = sede.Nombre;
            //txtDistritoSede.Text = sede.Distrito;
            //hdnIdSede.Value = idSede.ToString();

            hdnModoModal.Value = "modificar";
            string script = "setTimeout(function() {" +
                "document.getElementById('tituloModal').innerHTML = '<i class=\\\"fas fa-map-marker-alt me-2 text-danger\\\"></i>Modificar Local';" +
                "var modal = new bootstrap.Modal(document.getElementById('modalRegistrarLocal'));" +
                "modal.show();" +
            "}, 200);";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirModalModificar", script, true);
        }
        protected void btn_eliminar_Click(object sender, EventArgs e)
        {
            int id = int.Parse(hdnIdEliminar.Value);

            // Eliminar por tipo de entidad según la página
            // Por ejemplo: eliminar sede, local, usuario...

            ScriptManager.RegisterStartupScript(this, this.GetType(), "eliminado",
                "Swal.fire('¡Eliminado!', 'El registro fue eliminado correctamente.', 'success');", true);
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
                parametros.estado = ddlEstado.SelectedValue == "1"; // true si "Activo", false si "Inactivo"
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