using SoftResBusiness;
using SoftResBusiness.LocalWSClient;
using SoftResBusiness.MesaWSClient;
using SoftResBusiness.UsuarioWSClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mesaDTO = SoftResBusiness.MesaWSClient.mesaDTO;

namespace SoftResWA.Views.Mesas
{
    public partial class mesas_gestion : System.Web.UI.Page
    {
        private MesaBO mesaBO;
        private TipoMesaBO tipoMesaBO;
        private LocalBO localBO;
        private BindingList<mesaDTO> listadoMesas;

        public MesaBO MesaBO { get => mesaBO; set => mesaBO = value; }
        public TipoMesaBO TipoMesaBO { get => tipoMesaBO; set => tipoMesaBO = value; }
        public LocalBO LocalBO { get => localBO; set => localBO = value; }
        public BindingList<mesaDTO> ListadoMesas { get => listadoMesas; set => listadoMesas = value; }
        public usuariosDTO UsuarioActual
        {
            get
            {
                if (Session["UsuarioLogueado"] != null)
                {
                    return (usuariosDTO)Session["UsuarioLogueado"];
                }
                return null;
            }
        }

        //CONSTRUCTOR
        public mesas_gestion()
        {
            this.mesaBO = new MesaBO();
            this.tipoMesaBO = new TipoMesaBO();
            this.localBO = new LocalBO();
            mesaParametros parametros = new mesaParametros();
            parametros.estadoSpecified = false;
            parametros.idLocalSpecified = false;
            parametros.idTipoMesaSpecified = false;
            parametros.nombre = null;
            this.listadoMesas = this.MesaBO.Listar(parametros);
        }

        //CONFIGURACION VISUAL DE LISTADOS
        protected List<object> ConfigurarListado(BindingList<mesaDTO> lista)
        {
            var listaAdaptada = lista.Select(m => new
            {
                m.idMesa,
                m.numeroMesa,
                m.capacidad,
                estado = m.estado.ToString().Replace("_", " "),
                m.x,
                m.y,
                ubicacionMesa = m.tipoMesa?.nombre?? "No especificado",
                local_nombre = m.local?.nombre?? "No especificado",
                m.fechaCreacion,
                m.usuarioCreacion,
                fechaModificacion = m.fechaModificacionSpecified ? m.fechaModificacion : (DateTime?)null,
                m.usuarioModificacion,
                estadoBool = true // Asumimos que todas las mesas mostradas están activas
            }).ToList<Object>();
            return listaAdaptada;
        }

        private string ObtenerNombreTipoMesa(int tipoMesaId)
        {
            try
            {
                SoftResBusiness.TipoMesaWSClient.tipoMesaDTO tipoMesa = this.tipoMesaBO.ObtenerPorID(tipoMesaId);
                return tipoMesa?.nombre ?? "No encontrado";
            }
            catch
            {
                return "Error al cargar";
            }
        }

        private string ObtenerNombreLocal(int localId)
        {
            try
            {
                SoftResBusiness.LocalWSClient.localDTO local = this.localBO.ObtenerPorID(localId);
                return local?.nombre ?? "No encontrado";
            }
            catch
            {
                return "Error al cargar";
            }
        }

        private void MostrarModal(string modo, string titulo)
        {
            hdnModoModal.Value = modo;

            string script = "setTimeout(function() {" +
                            $"document.getElementById('tituloModal').innerHTML = '<i class=\\\"fas fa-chair me-2 text-danger\\\"></i>{titulo}';" +
                            "var modal = new bootstrap.Modal(document.getElementById('modalRegistrarMesa'));" +
                            "modal.show();" +
                            "}, 200);";

            ScriptManager.RegisterStartupScript(this, this.GetType(), $"mostrarModal_{modo}", script, true);
        }

        private void CargarDropDownList(DropDownList ddl, object dataSource, string textField, string valueField, string textoDefault)
        {
            ddl.DataSource = dataSource;
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(textoDefault, ""));
        }

        private void CargarEstadosMesa()
        {
            var estadosMesa = new List<object>
            {
                new { nombre = "DISPONIBLE", id = "DISPONIBLE" },
                new { nombre = "EN_USO", id = "EN_USO" },
                new { nombre = "RESERVADA", id = "RESERVADA" },
                new { nombre = "EN_MANTENIMIENTO", id = "EN_MANTENIMIENTO" },
                new { nombre = "DESECHADA", id = "DESECHADA" }
            };

            this.CargarDropDownList(ddlEstadoMesa, estadosMesa, "nombre", "id", "Seleccionar...");
            this.CargarDropDownList(ddlEstadoFiltro, estadosMesa, "nombre", "id", "Todos");
        }

        private void CargarTiposMesa()
        {
            try
            {
                var tiposMesa = this.tipoMesaBO.Listar();
                this.CargarDropDownList(ddlTipoMesa, tiposMesa, "nombre", "idTipoMesa", "Seleccionar...");
                this.CargarDropDownList(ddlTipoMesaFiltro, tiposMesa, "nombre", "idTipoMesa", "Todos");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorTiposMesa",
                    $"Swal.fire('Error', 'No se pudieron cargar los tipos de mesa: {ex.Message}', 'error');", true);
            }
        }

        private void CargarLocales()
        {
            try
            {
                SoftResBusiness.LocalWSClient.localParametros parametros = new SoftResBusiness.LocalWSClient.localParametros();
                parametros.estadoSpecified = true;
                parametros.estado = true;
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

                int idMesa = (int)dataItem.GetType().GetProperty("idMesa")?.GetValue(dataItem);

                LinkButton btnModificar = (LinkButton)e.Row.FindControl("btnModificar");
                LinkButton btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");

                btnModificar.Visible = estado;
                btnEliminar.Visible = estado;

                if (estado)
                {
                    btnEliminar.OnClientClick = $"return confirmarEliminacion({idMesa}, '{hdnIdEliminar.ClientID}', '{btnEliminarMesa.ClientID}');";
                }
            }
        }
        private void MostrarModal(string modo, string titulo)
        {
            hdnModoModal.Value = modo;

            string script = "setTimeout(function() {" +
                            $"document.getElementById('tituloModal').innerHTML = '<i class=\\\"fas fa-chair me-2 text-danger\\\"></i>{titulo}';" +
                            "var modal = new bootstrap.Modal(document.getElementById('modalRegistrarMesa'));" +
                            "modal.show();" +
                            "}, 200);";

            ScriptManager.RegisterStartupScript(this, this.GetType(), $"mostrarModal_{modo}", script, true);
        }
        private void CargarDropDownList(DropDownList ddl, object dataSource, string textField, string valueField, string textoDefault)
        {
            ddl.DataSource = dataSource;
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem(textoDefault, ""));
        }

        //FUNCIONES DE MESA
        private void CargarEstadosMesa()
        {
            var estadosMesa = Enum.GetNames(typeof(SoftResBusiness.MesaWSClient.estadoMesa))
                .Select(d => new { nombre = d, id = d })
                .ToList();

            this.CargarDropDownList(ddlEstadoMesa, estadosMesa, "nombre", "id", "Seleccionar...");
            this.CargarDropDownList(ddlEstadoFiltro, estadosMesa, "nombre", "id", "Todos");
        }
        private void CargarTiposMesa()
        {
            try
            {
                
                var tiposMesa = this.tipoMesaBO.Listar();
                this.CargarDropDownList(ddlTipoMesa, tiposMesa, "nombre", "idTipoMesa", "Seleccionar...");
                this.CargarDropDownList(ddlTipoMesaFiltro, tiposMesa, "nombre", "idTipoMesa", "Todos");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorTiposMesa",
                    $"Swal.fire('Error', 'No se pudieron cargar los tipos de mesa: {ex.Message}', 'error');", true);
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
        private mesaDTO ConstruirDTO(mesaDTO mesa)
        {
            if (mesa == null)
                mesa = new mesaDTO();

            // Validar campos requeridos
            if (string.IsNullOrWhiteSpace(txtNumeroMesa.Text))
                throw new ArgumentException("El número de mesa es requerido");

            if (string.IsNullOrWhiteSpace(txtCapacidad.Text) || !int.TryParse(txtCapacidad.Text, out int capacidad) || capacidad < 1)
                throw new ArgumentException("La capacidad debe ser un número mayor a 0");

            if (string.IsNullOrWhiteSpace(txtPosX.Text) || !int.TryParse(txtPosX.Text, out int posX))
                throw new ArgumentException("La posición X debe ser un número válido");

            if (string.IsNullOrWhiteSpace(txtPosY.Text) || !int.TryParse(txtPosY.Text, out int posY))
                throw new ArgumentException("La posición Y debe ser un número válido");

            if (string.IsNullOrWhiteSpace(ddlEstadoMesa.SelectedValue))
                throw new ArgumentException("El estado es requerido");

            if (string.IsNullOrWhiteSpace(ddlTipoMesa.SelectedValue))
                throw new ArgumentException("El tipo de mesa es requerido");

            if (string.IsNullOrWhiteSpace(ddlLocal.SelectedValue))
                throw new ArgumentException("El local es requerido");

            mesa.numeroMesa = txtNumeroMesa.Text.Trim();
            mesa.capacidad = capacidad;
            mesa.capacidadSpecified = true;
            mesa.estado = (estadoMesa)Enum.Parse(typeof(estadoMesa), ddlEstadoMesa.SelectedValue);
            mesa.estadoSpecified = true;
            mesa.x = posX;
            mesa.xSpecified = true;
            mesa.y = posY;
            mesa.ySpecified = true;
            
            // Configurar tipo de mesa
            mesa.tipoMesa = new tipoMesaDTO();
            mesa.tipoMesa.idTipoMesa = int.Parse(ddlTipoMesa.SelectedValue);
            mesa.tipoMesa.idTipoMesaSpecified = true;
            
            // Configurar local
            mesa.local = new SoftResBusiness.MesaWSClient.localDTO();
            mesa.local.idLocal = int.Parse(ddlLocal.SelectedValue);
            mesa.local.idLocalSpecified = true;

            return mesa;
        }

        private bool ValidarMesaUnica(string numeroMesa, int? idMesaExcluir = null)
        {
            try
            {
                mesaParametros parametros = new mesaParametros();
                parametros.nombre = numeroMesa;
                var mesasExistentes = this.mesaBO.Listar(parametros);
                
                if (idMesaExcluir.HasValue)
                {
                    // Excluir la mesa actual en caso de modificación
                    mesasExistentes = new BindingList<mesaDTO>(
                        mesasExistentes.Where(m => m.idMesa != idMesaExcluir.Value).ToList()
                    );
                }
                
                return mesasExistentes.Count == 0;
            }
            catch
            {
                return true; // Si hay error, permitir continuar
            }
        }

        //PAGE_LOAD
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            dgvMesas.RowDataBound += dgv_RowDataBound;
            
            if (!IsPostBack)
            {
                var listaAdaptada = this.ConfigurarListado(ListadoMesas);
                dgvMesas.DataSource = listaAdaptada;
                dgvMesas.DataBind();
                CargarEstadosMesa();
                CargarTiposMesa();
                CargarLocales();
            }
        }

        //BOTONES
        protected void btnGuardarMesa_Click(object sender, EventArgs e)
        {
            string modo = hdnModoModal.Value;
            bool exito = false;
            try
            {
                if (modo == "registrar")
                {
                    // Validar que el número de mesa sea único
                    if (!ValidarMesaUnica(txtNumeroMesa.Text.Trim()))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "errorDuplicado",
                            "Swal.fire('Error', 'Ya existe una mesa con este número', 'error');", true);
                        return;
                    }

                    mesaDTO mesa = new mesaDTO();
                    mesa = ConstruirDTO(mesa);
                    mesa.fechaCreacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                    mesa.fechaCreacionSpecified = true;
                    mesa.fechaModificacionSpecified = false;
                    mesa.usuarioCreacion = "admin"; // usar Session["usuario"] si aplica

                    exito = this.mesaBO.Insertar(mesa) > 0;
                }
                else if (modo == "modificar")
                {
                    int id = int.Parse(hdnIdMesa.Value);
                    
                    // Validar que el número de mesa sea único (excluyendo la mesa actual)
                    if (!ValidarMesaUnica(txtNumeroMesa.Text.Trim(), id))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "errorDuplicado",
                            "Swal.fire('Error', 'Ya existe otra mesa con este número', 'error');", true);
                        return;
                    }

                    mesaDTO mesa = this.mesaBO.ObtenerPorID(id);
                    if (mesa == null)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "errorNoEncontrada",
                            "Swal.fire('Error', 'No se encontró la mesa a modificar', 'error');", true);
                        return;
                    }

                    mesa = ConstruirDTO(mesa);
                    mesa.idMesa = id;
                    mesa.idMesaSpecified = true;
                    mesa.fechaModificacion = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                    mesa.fechaModificacionSpecified = true;
                    mesa.usuarioModificacion = "admin"; // usar Session["usuario"] si aplica

                    exito = this.mesaBO.Modificar(mesa) > 0;
                }
                
                MostrarResultado(exito, "Mesa", modo);
                if (exito) 
                {
                    btnBuscar_Click(sender, e);
                    // Cerrar modal con JavaScript
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal",
                        "setTimeout(function() { bootstrap.Modal.getInstance(document.getElementById('modalRegistrarMesa')).hide(); }, 1500);", true);
                }
            }
            catch (ArgumentException ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorValidacion",
                    $"Swal.fire('Error de Validación', '{ex.Message}', 'warning');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                    $"Swal.fire('Error', 'Error al procesar la mesa: {ex.Message}', 'error');", true);
            }
        }
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            // Limpiar campos
            txtNumeroMesa.Text = "";
            txtCapacidad.Text = "";
            txtPosX.Text = "";
            txtPosY.Text = "";
            ddlEstadoMesa.SelectedIndex = 0;
            ddlTipoMesa.SelectedIndex = 0;
            ddlLocal.SelectedIndex = 0;
            hdnIdMesa.Value = "";

            // Cambiar título a "Registrar"
            this.MostrarModal("registrar", "Registrar Mesa");
        }
        protected void btnModificar_Command(object sender, CommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            if (id > 0)
            {
                try
                {
                    mesaDTO mesa = this.mesaBO.ObtenerPorID(id);
                    if (mesa != null)
                    {
                        hdnIdMesa.Value = mesa.idMesa.ToString();
                        txtNumeroMesa.Text = mesa.numeroMesa;
                        txtCapacidad.Text = mesa.capacidad.ToString();
                        txtPosX.Text = mesa.x.ToString();
                        txtPosY.Text = mesa.y.ToString();
                        ddlEstadoMesa.SelectedValue = mesa.estado.ToString();
                        
                        if (mesa.tipoMesa != null)
                            ddlTipoMesa.SelectedValue = mesa.tipoMesa.idTipoMesa.ToString();
                        
                        if (mesa.local != null)
                            ddlLocal.SelectedValue = mesa.local.idLocal.ToString();
                        
                        this.MostrarModal("modificar", "Modificar Mesa");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "errorNoEncontrada",
                            "Swal.fire('Error', 'No se encontró la mesa', 'error');", true);
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errorModificar",
                        $"Swal.fire('Error', 'Error al cargar la mesa: {ex.Message}', 'error');", true);
                }
            }
        }
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int id = int.Parse(hdnIdEliminar.Value);
            if (id > 0)
            {
                try
                {
                    mesaDTO mesa = this.mesaBO.ObtenerPorID(id);
                    if (mesa != null)
                    {
                        mesa.idMesa = id;
                        mesa.idMesaSpecified = true;
                        bool exito = this.mesaBO.Eliminar(mesa) > 0;
                        MostrarResultado(exito, "Mesa", "eliminar");
                        if (exito) btnBuscar_Click(sender, e);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "errorNoEncontrada",
                            "Swal.fire('Error', 'No se encontró la mesa a eliminar', 'error');", true);
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errorEliminar",
                        $"Swal.fire('Error', 'Error al eliminar la mesa: {ex.Message}', 'error');", true);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                mesaParametros parametros = new mesaParametros();
                
                // Filtro por número de mesa
                if (!string.IsNullOrWhiteSpace(txtNumeroMesaFiltro.Text))
                    parametros.nombre = txtNumeroMesaFiltro.Text.Trim();
                
                // Filtro por estado
                parametros.estadoSpecified = !string.IsNullOrEmpty(ddlEstadoFiltro.SelectedValue);
                if (parametros.estadoSpecified)
                    parametros.estado = (estadoMesa)Enum.Parse(typeof(estadoMesa), ddlEstadoFiltro.SelectedValue);

                // Filtro por tipo de mesa
                parametros.idTipoMesaSpecified = !string.IsNullOrEmpty(ddlTipoMesaFiltro.SelectedValue);
                if (parametros.idTipoMesaSpecified)
                    parametros.idTipoMesa = int.Parse(ddlTipoMesaFiltro.SelectedValue);

                // Filtro por local
                parametros.idLocalSpecified = !string.IsNullOrEmpty(ddlLocalFiltro.SelectedValue);
                if (parametros.idLocalSpecified)
                    parametros.idLocal = int.Parse(ddlLocalFiltro.SelectedValue);

                var lista = this.mesaBO.Listar(parametros);
                var listaAdaptada = this.ConfigurarListado(lista);
                dgvMesas.DataSource = listaAdaptada;
                dgvMesas.DataBind();

                // Mostrar mensaje si no hay resultados
                if (lista.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "sinResultados",
                        "Swal.fire('Sin resultados', 'No se encontraron mesas con los criterios de búsqueda especificados', 'info');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorBuscar",
                    $"Swal.fire('Error', 'Error en la búsqueda: {ex.Message}', 'error');", true);
            }
        }
    }
}