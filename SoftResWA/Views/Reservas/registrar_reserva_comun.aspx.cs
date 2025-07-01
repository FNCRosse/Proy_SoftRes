using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftResBusiness;
using SoftResBusiness.ReservaWSClient;

namespace SoftResWA.Views.Reservas
{
    public partial class registar_reserva_comun : System.Web.UI.Page
    {
        private ReservaBO reservaBO;
        private LocalBO localBO;
        private UsuarioBO usuarioBO;
        private TipoDocumentoBO tipoDocumentoBO;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtCantidadPersonas.Text = "1";
                txtCantidadMesas.Text = "1";
                txtCantidadPersonas.Attributes["min"] = "1";
                txtCantidadMesas.Attributes["min"] = "1";
                txtCantidadPersonas.Attributes["max"] = "5";
                txtCantidadMesas.Attributes["max"] = "5";
                
                CargarLocales();
                CargarTiposDocumento();
            }
        }

        private void CargarLocales()
        {
            try
            {
                localBO = new LocalBO();
                var parametros = new SoftResBusiness.LocalWSClient.localParametros();
                var locales = localBO.Listar(parametros);
                ddlLocales.DataTextField = "nombre";
                ddlLocales.DataValueField = "idLocal";
                ddlLocales.DataSource = locales;
                ddlLocales.DataBind();
                ddlLocales.Items.Insert(0, new ListItem("Seleccionar Local", ""));
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorCarga", 
                    $"Swal.fire('Error', 'Error al cargar locales: {ex.Message}', 'error');", true);
            }
        }

        private void CargarTiposDocumento()
        {
            try
            {
                tipoDocumentoBO = new TipoDocumentoBO();
                var tiposDoc = tipoDocumentoBO.Listar();
                ddlTipoDocumento.DataTextField = "descripcion";
                ddlTipoDocumento.DataValueField = "idTipoDocumento";
                ddlTipoDocumento.DataSource = tiposDoc;
                ddlTipoDocumento.DataBind();
                ddlTipoDocumento.Items.Insert(0, new ListItem("Seleccionar Tipo", ""));
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorCarga", 
                    $"Swal.fire('Error', 'Error al cargar tipos de documento: {ex.Message}', 'error');", true);
            }
        }

        protected void btnBuscarUsuario_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDocumentoUsuario.Text) || ddlTipoDocumento.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "validacion",
                    "Swal.fire('Validación', 'Ingrese documento y seleccione tipo de documento', 'warning');", true);
                return;
            }

            try
            {
                usuarioBO = new UsuarioBO();
                var parametros = new SoftResBusiness.UsuarioWSClient.usuariosParametros();
                var usuarios = usuarioBO.Listar(parametros);
                var usuario = usuarios?.FirstOrDefault(u => u.documento == txtDocumentoUsuario.Text.Trim());
                
                if (usuario != null)
                {
                    txtNombreUsuario.Text = $"{usuario.nombre} {usuario.apellidoPaterno} {usuario.apellidoMaterno}";
                    txtTipoCliente.Text = usuario.rol?.descripcion ?? "Cliente";
                }
                else
                {
                    txtNombreUsuario.Text = "";
                    txtTipoCliente.Text = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "noEncontrado",
                        "Swal.fire('No encontrado', 'Usuario no encontrado con ese documento', 'info');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorBusqueda",
                    $"Swal.fire('Error', 'Error al buscar usuario: {ex.Message}', 'error');", true);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                try
                {
                    reservaBO = new ReservaBO();
                    
                    var nuevaReserva = new reservaDTO
                    {
                        fechaReserva = DateTime.Parse(txtFecha.Text),
                        horaReserva = TimeSpan.Parse(txtHora.Text),
                        cantPersonas = int.Parse(txtCantidadPersonas.Text),
                        observaciones = txtObservaciones.Text,
                        tipoReserva = tipoReserva.COMUN,
                        estadoReserva = estadoReserva.PENDIENTE,
                        activo = true,
                        fechaCreacion = DateTime.Now,
                        ubicaMesaPreferida = ddlUbicacionMesa.SelectedValue,
                        local = new SoftResBusiness.ReservaWSClient.localDTO 
                        { 
                            idLocal = int.Parse(ddlLocales.SelectedValue) 
                        },
                        solicitante = new SoftResBusiness.ReservaWSClient.usuarioDTO 
                        { 
                            documento = txtDocumentoUsuario.Text.Trim()
                        }
                    };

                    int resultado = reservaBO.insertar(nuevaReserva);
                    
                    if (resultado > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "exito",
                            "Swal.fire({title: '¡Éxito!', text: 'Reserva común registrada correctamente', icon: 'success'}).then(() => { limpiarFormulario(); });", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                            "Swal.fire('Error', 'No se pudo registrar la reserva', 'error');", true);
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errorGuardar",
                        $"Swal.fire('Error', 'Error al guardar: {ex.Message}', 'error');", true);
                }
            }
        }

        private bool ValidarDatos()
        {
            if (string.IsNullOrWhiteSpace(txtFecha.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "validacion",
                    "Swal.fire('Validación', 'Seleccione una fecha', 'warning');", true);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtHora.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "validacion",
                    "Swal.fire('Validación', 'Seleccione una hora', 'warning');", true);
                return false;
            }

            if (ddlLocales.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "validacion",
                    "Swal.fire('Validación', 'Seleccione un local', 'warning');", true);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDocumentoUsuario.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "validacion",
                    "Swal.fire('Validación', 'Ingrese el documento del usuario', 'warning');", true);
                return false;
            }

            if (ddlTipoDocumento.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "validacion",
                    "Swal.fire('Validación', 'Seleccione el tipo de documento', 'warning');", true);
                return false;
            }

            DateTime fechaReserva = DateTime.Parse(txtFecha.Text);
            if (fechaReserva < DateTime.Today)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "validacion",
                    "Swal.fire('Validación', 'La fecha de reserva no puede ser anterior a hoy', 'warning');", true);
                return false;
            }

            return true;
        }

        private void LimpiarFormulario()
        {
            txtFecha.Text = "";
            txtHora.Text = "";
            txtCantidadPersonas.Text = "1";
            txtCantidadMesas.Text = "1";
            txtObservaciones.Text = "";
            ddlLocales.SelectedIndex = 0;
            ddlUbicacionMesa.SelectedIndex = 0;
            ddlEstado.SelectedIndex = 0;
            txtDocumentoUsuario.Text = "";
            ddlTipoDocumento.SelectedIndex = 0;
            txtNombreUsuario.Text = "";
            txtTipoCliente.Text = "";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "limpiar",
                "function limpiarFormulario() { /* Formulario limpiado */ }", true);
        }
    }
}