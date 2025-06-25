
using SoftResBusiness;
using SoftResBusiness.ComentarioWSClient;
using SoftResBusiness.LocalWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using localDTO = SoftResBusiness.LocalWSClient.localDTO;

namespace SoftResWA.Views.Comentarios
{
    public partial class comentario_listado : System.Web.UI.Page
    {
        private ComentarioBO comentarioBO;
        private LocalBO localBO;
        private BindingList<localDTO> listadoLocal;
        private BindingList<comentariosDTO> listadoComentatrio;

        public ComentarioBO ComentarioBO { get => comentarioBO; set => comentarioBO = value; }
        public LocalBO LocalBO { get => localBO; set => localBO = value; }
        public BindingList<localDTO> ListadoLocal { get => listadoLocal; set => listadoLocal = value; }
        public BindingList<comentariosDTO> ListadoComentatrio { get => listadoComentatrio; set => listadoComentatrio = value; }

        public comentario_listado()
        {
            this.ComentarioBO = new ComentarioBO();
            this.LocalBO= new LocalBO();
            localParametros parametros = new localParametros();
            parametros.nombre = null;
            parametros.estado = true;
            parametros.estadoSpecified = true;
            parametros.idSedeSpecified = false;
            this.ListadoLocal = this.LocalBO.Listar(parametros);
            comentarioParametros cParametros = new comentarioParametros();
            cParametros.idLocalSpecified = false;
            cParametros.puntuacionSpecified = false;
            cParametros.idReservaSpecified = false;
            cParametros.estadoSpecified = true;
            cParametros.estado = true;
            this.ListadoComentatrio=this.ComentarioBO.Listar(cParametros);
        }
        protected List<object> ConfigurarListado(BindingList<comentariosDTO> lista)
        {
            var listaAdaptada = lista.Select(l => new
            {
                l.idComentario,
                l.mensaje,
                l.puntuacion,
                usuario_nombre = l.usuario?.nombreComp ?? "",
                usuario_num = l.usuario?.numeroDocumento?? "",
                local_nombre = l.reserva?.local?.nombre ?? "",
                l.fechaCreacion,
                l.usuarioCreacion,
                fechaModificacion = l.fechaModificacionSpecified ? l.fechaModificacion : (DateTime?)null,
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)  // 👈 Esto es clave
            {
                var listaAdaptada = this.ConfigurarListado(ListadoComentatrio);
                dgvComentarios.DataSource = listaAdaptada;
                dgvComentarios.DataBind();
                this.CargarDropDownList(ddlLocal, ListadoLocal, "nombre", "idLocal", "-- Seleccione --");
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            comentarioParametros parametros = new comentarioParametros();
            parametros.numDocCliente = string.IsNullOrWhiteSpace(txtUsuario.Text) ? null : txtUsuario.Text.Trim();
            parametros.estadoSpecified = !string.IsNullOrEmpty(ddlEstado.SelectedValue);
            parametros.estado = ddlEstado.SelectedValue == "1";
            parametros.idLocalSpecified = !string.IsNullOrEmpty(ddlLocal.SelectedValue);
            parametros.idLocal = !string.IsNullOrEmpty(ddlLocal.SelectedValue) ? int.Parse(ddlLocal.SelectedValue) : 0;
            parametros.puntuacionSpecified = !string.IsNullOrEmpty(ddlPuntuacion.SelectedValue);
            parametros.puntuacion = !string.IsNullOrEmpty(ddlPuntuacion.SelectedValue) ? int.Parse(ddlPuntuacion.SelectedValue) : 0;

            var lista = this.ComentarioBO.Listar(parametros);
            var listaAdaptada = this.ConfigurarListado(lista);
            dgvComentarios.DataSource = listaAdaptada;
            dgvComentarios.DataBind();
        }
    }
}