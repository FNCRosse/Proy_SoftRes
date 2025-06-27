using SoftResBusiness;
using SoftResBusiness.UsuarioWSClient;
using SoftResBusiness.RolWSClient;
using SoftResBusiness.TipoDocumentoWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SoftResWA.Views.Usuarios
{
    public partial class registro_empleado : System.Web.UI.Page
    {
        private UsuarioBO usuarioBO;
        private RolBO rolBO;
        private TipoDocumentoBO tipoDocumentoBO;
        private int idUsuario = 0;
        private bool esModificacion = false;

        public UsuarioBO UsuarioBO { get => usuarioBO; set => usuarioBO = value; }
        public RolBO RolBO { get => rolBO; set => rolBO = value; }
        public TipoDocumentoBO TipoDocumentoBO { get => tipoDocumentoBO; set => tipoDocumentoBO = value; }

        public registro_empleado()
        {
            this.usuarioBO = new UsuarioBO();
            this.rolBO = new RolBO();
            this.tipoDocumentoBO = new TipoDocumentoBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            
            if (!IsPostBack)
            {
                txtSueldo.Attributes["min"] = "1";
                CargarRoles();
                CargarTiposDocumento();
                
                // Verificar si es modificación
                if (Request.QueryString["id"] != null)
                {
                    if (int.TryParse(Request.QueryString["id"], out idUsuario))
                    {
                        esModificacion = true;
                        ViewState["IdUsuario"] = idUsuario;
                        ViewState["EsModificacion"] = true;
                        CargarDatosUsuario(idUsuario);
                        btnGuardar.Text = "Modificar";
                        //lblTitulo.Text = "Modificar Empleado";
                    }
                }
                else
                {
                    ViewState["EsModificacion"] = false;
                    //lblTitulo.Text = "Registrar Empleado";
                }
            }
            else
            {
                esModificacion = ViewState["EsModificacion"] != null && (bool)ViewState["EsModificacion"];
                if (esModificacion)
                {
                    idUsuario = ViewState["IdUsuario"] != null ? (int)ViewState["IdUsuario"] : 0;
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

        private void CargarRoles()
        {
            try
            {
                var roles = this.rolBO.Listar().Where(r => !r.esCliente).ToList(); // Solo roles no cliente para empleados
                this.CargarDropDownList(ddlRol, roles, "nombre", "idRol", "Seleccionar...");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorRoles",
                    $"Swal.fire('Error', 'No se pudieron cargar los roles: {ex.Message}', 'error');", true);
            }
        }

        private void CargarTiposDocumento()
        {
            try
            {
                var tiposDoc = this.tipoDocumentoBO.Listar();
                this.CargarDropDownList(ddlTipoDocumento, tiposDoc, "nombre", "idTipoDocumento", "Seleccionar...");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorTiposDoc",
                    $"Swal.fire('Error', 'No se pudieron cargar los tipos de documento: {ex.Message}', 'error');", true);
            }
        }

        private void CargarDatosUsuario(int id)
        {
            try
            {
                usuariosDTO usuario = this.usuarioBO.ObtenerPorID(id);
                if (usuario != null)
                {
                    txtNombreCompleto.Text = usuario.nombreComp;
                    txtNumeroDocumento.Text = usuario.numeroDocumento;
                    txtEmail.Text = usuario.email;
                    txtTelefono.Text = usuario.telefono;
                    txtSueldo.Text = usuario.sueldo.ToString("F2");
                    txtCantReservas.Text = usuario.cantidadReservacion.ToString();
                    txtContrasena.Text = usuario.contrasenha;
                    chkEstado.Checked = usuario.estado;
                    
                    if (usuario.fechaContratacionSpecified)
                    {
                        txtFechaContratacion.Text = usuario.fechaContratacion.ToString("dd-MM-yyyy");
                    }
                    
                    ddlTipoDocumento.SelectedValue = usuario.tipoDocumento.ToString();
                    ddlRol.SelectedValue = usuario.rol.ToString();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorCargar",
                    $"Swal.fire('Error', 'Error al cargar los datos del usuario: {ex.Message}', 'error');", true);
            }
        }

        private usuariosDTO ConstruirDTO(usuariosDTO usuario)
        {
            if (usuario == null)
                usuario = new usuariosDTO();

            usuario.nombreComp = txtNombreCompleto.Text.Trim();
            usuario.numeroDocumento = txtNumeroDocumento.Text.Trim();
            usuario.email = txtEmail.Text.Trim();
            usuario.telefono = txtTelefono.Text.Trim();
            usuario.contrasenha = txtContrasena.Text.Trim();
            //usuario.sueldo = decimal.Parse(txtSueldo.Text.Trim());
            usuario.sueldoSpecified = true;
            usuario.cantidadReservacion = int.Parse(txtCantReservas.Text.Trim());
            usuario.cantidadReservacionSpecified = true;
            usuario.estado = chkEstado.Checked;
            usuario.estadoSpecified = true;
            //usuario.tipoDocumento = int.Parse(ddlTipoDocumento.SelectedValue);
            //usuario.tipoDocIdSpecified = true;
            usuario.idUsuario = int.Parse(ddlRol.SelectedValue);
            usuario.idUsuarioSpecified = true;

            if (!string.IsNullOrEmpty(txtFechaContratacion.Text))
            {
                if (DateTime.TryParseExact(txtFechaContratacion.Text, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fechaContratacion))
                {
                    usuario.fechaContratacion = DateTime.SpecifyKind(fechaContratacion, DateTimeKind.Unspecified);
                    usuario.fechaContratacionSpecified = true;
                }
            }

            return usuario;
        }

        private void MostrarResultado(bool exito, string entidad, string modo)
        {
            string accion = (modo == "modificar") ? "modificado" : "registrado";
            string accionNo = (modo == "modificar") ? "modificar" : "registrar";

            string baseMensaje = exito ? $"El {entidad} fue {accion}" : $"No se pudo {accionNo} el {entidad}";
            string tipo = exito ? "success" : "warning";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje",
                $"Swal.fire('¡{entidad} {(exito ? accion : $"NO {accion}")}!', '{baseMensaje} correctamente.', '{tipo}');", true);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                bool exito = false;
                string modo = esModificacion ? "modificar" : "registrar";

                if (esModificacion)
                {
                    // Modificar usuario existente
                    usuariosDTO usuario = this.usuarioBO.ObtenerPorID(idUsuario);
                    usuario = ConstruirDTO(usuario);
                    usuario.idUsuario = idUsuario;
                    usuario.idUsuarioSpecified = true;

                    usuario.fechaModificacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                    usuario.fechaModificacionSpecified = true;
                    usuario.usuarioModificacion = "admin"; // usar Session["usuario"] si aplica

                    exito = this.usuarioBO.Modificar(usuario) > 0;
                }
                else
                {
                    // Registrar nuevo usuario
                    usuariosDTO usuario = new usuariosDTO();
                    usuario = ConstruirDTO(usuario);
                    usuario.fechaCreacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                    usuario.fechaCreacionSpecified = true;
                    usuario.fechaModificacionSpecified = false;
                    usuario.usuarioCreacion = "admin"; // usar Session["usuario"] si aplica

                    exito = this.usuarioBO.Insertar(usuario) > 0;
                }

                MostrarResultado(exito, "Empleado", modo);

                if (exito)
                {
                    // Redirigir al listado después de un registro/modificación exitoso
                    string script = "setTimeout(function() { window.location.href = 'listado_empleado.aspx'; }, 2000);";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", script, true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                    $"Swal.fire('Error', 'Error al procesar el empleado: {ex.Message}', 'error');", true);
            }
        }

        protected void btnCalendario_Click(object sender, EventArgs e)
        {
            calFechaContratacion.Visible = true;
        }

        protected void calFechaContratacion_SelectionChanged(object sender, EventArgs e)
        {
            txtFechaContratacion.Text = calFechaContratacion.SelectedDate.ToString("dd-MM-yyyy");
            calFechaContratacion.Visible = false;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("listado_empleado.aspx");
        }
    }
}