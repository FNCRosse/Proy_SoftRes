<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="listado_empleado.aspx.cs" Inherits="SoftResWA.Views.Usuarios.listado_empleado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Mantenimiento de Empleados
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="text-start mb-3">
        <h1 class="h3 text-danger fw-bold mb-0 me-3">
            <i class="fas fa-users me-2"></i>Mantenimiento de Empleados
        </h1>
    </div>
    
    <!-- Filtros para búsqueda -->
    <div class="container-fluid mb-4 ps-4">
        <div class="row gx-3">
            <!-- Nombre -->
            <div class="col-auto">
                <label for="txtNombreCompFiltro" class="form-label">Nombre Completo</label>
                <asp:TextBox ID="txtNombreCompFiltro" runat="server" CssClass="form-control" placeholder="Ej. Manuel Perez" />
            </div>
            <!-- Tipo Documento -->
            <div class="col-auto">
                <label for="ddlTipoDocumentoFiltro" class="form-label">Tipo Documento</label>
                <asp:DropDownList ID="ddlTipoDocumentoFiltro" runat="server" CssClass="form-select">
                </asp:DropDownList>
            </div>
            <!-- Rol -->
            <div class="col-auto">
                <label for="ddlRolFiltro" class="form-label">Rol </label>
                <asp:DropDownList ID="ddlRolFiltro" runat="server" CssClass="form-select">
                </asp:DropDownList>
            </div>
            <!-- Estado -->
            <div class="col-auto">
                <label for="ddlEstadoFiltro" class="form-label">Estado</label>
                <asp:DropDownList ID="ddlEstadoFiltro" runat="server" CssClass="form-select">
                </asp:DropDownList>
            </div>
            <!-- Botones -->
            <div class="col-auto d-flex align-items-end">
                <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-danger me-2" 
                    OnClick="btnBuscar_Click">
                    <i class="fas fa-search me-1"></i>Buscar
                </asp:LinkButton>
                <asp:Button ID="btnNuevoEmpleado" runat="server" Text="Nuevo Empleado"
                    CssClass="btn"
                    Style="background-color: #FFF3CD; color: #856404; border: 1px solid #d39e00;"
                    OnClick="btnNuevoEmpleado_Click" />
            </div>
        </div>
    </div>

    <!-- Tabla -->
    <div class="row">
        <asp:GridView ID="dgvEmpleados" runat="server" AllowPaging="false" AutoGenerateColumns="false"
            CssClass="table table-hover table-responsive table-striped">
            <Columns>
                <asp:BoundField HeaderText="ID" DataField="idUsuario" />
                <asp:BoundField HeaderText="Nombre Completo" DataField="nombreComp" />
                <asp:BoundField HeaderText="Tipo Documento" DataField="tipoDocumentoNombre" />
                <asp:BoundField HeaderText="Número Documento" DataField="numeroDocumento" />
                <asp:BoundField HeaderText="Rol" DataField="rolNombre" />
                <asp:BoundField HeaderText="Email" DataField="email" />
                <asp:BoundField HeaderText="Teléfono" DataField="telefono" />
                <asp:BoundField HeaderText="Sueldo" DataField="sueldo" DataFormatString="{0:C}" />
                <asp:BoundField HeaderText="Reservas" DataField="cantidadReservacion" />
                <asp:BoundField HeaderText="Fecha Contratación" DataField="fechaContratacion" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField HeaderText="Fecha Creación" DataField="fechaCreacion" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField HeaderText="Usuario Creación" DataField="usuarioCreacion" />
                <asp:BoundField HeaderText="Fecha Modificación" DataField="fechaModificacion" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField HeaderText="Usuario Modificación" DataField="usuarioModificacion" />
                <asp:BoundField HeaderText="Estado" DataField="estadoTexto" />
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkModificar" runat="server" CssClass="btn btn-warning btn-sm me-1"
                            CommandArgument='<%# Eval("idUsuario") %>'
                            OnCommand="btnModificarEmpleado_Command">
                            <i class="fas fa-edit"></i>
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-danger btn-sm">
                            <i class="fas fa-trash"></i>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <!-- Controles ocultos -->
    <asp:HiddenField ID="hdnModoModal" runat="server" />
    <asp:HiddenField ID="hdnIdEliminar" runat="server" />
    <asp:HiddenField ID="hdnIdEmpleado" runat="server" />
    <asp:Button ID="btnEliminarUsuario" runat="server" style="display:none;" OnClick="btnEliminarUsuario_Click" />
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <!-- Modal para Registrar/Modificar Empleado -->
    <div class="modal fade" id="modalRegistrarEmpleado" tabindex="-1" aria-labelledby="tituloModalEmpleado" aria-hidden="true">
        <div class="modal-dialog modal-xl modal-dialog-centered">
            <div class="modal-content border-warning">
                <div class="modal-header bg-warning-subtle">
                    <h5 class="modal-title fw-bold" id="tituloModalEmpleado">
                        <i class="fas fa-user-plus me-2 text-danger"></i>Registrar Empleado
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <div class="row g-3">
                        <!-- Nombre completo -->
                        <div class="col-md-6">
                            <label for="txtNombreCompletoEmpleado" class="form-label">Nombre Completo <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtNombreCompletoEmpleado" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="rfvNombreEmpleado" runat="server" 
                                ControlToValidate="txtNombreCompletoEmpleado" ErrorMessage="Nombre completo es requerido" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="EmpleadoValidation" />
                        </div>

                        <!-- Rol -->
                        <div class="col-md-6">
                            <label for="ddlRolModal" class="form-label">Rol <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlRolModal" runat="server" CssClass="form-select">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvRolModal" runat="server" 
                                ControlToValidate="ddlRolModal" ErrorMessage="Rol es requerido" 
                                CssClass="text-danger" Display="Dynamic" InitialValue="" ValidationGroup="EmpleadoValidation" />
                        </div>

                        <!-- Número de documento -->
                        <div class="col-md-6">
                            <label for="txtNumeroDocumentoEmpleado" class="form-label">Número de Documento <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtNumeroDocumentoEmpleado" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="rfvNumeroDocEmpleado" runat="server" 
                                ControlToValidate="txtNumeroDocumentoEmpleado" ErrorMessage="Número de documento es requerido" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="EmpleadoValidation" />
                        </div>

                        <!-- Tipo de documento -->
                        <div class="col-md-6">
                            <label for="ddlTipoDocumentoEmpleadoModal" class="form-label">Tipo de Documento <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlTipoDocumentoEmpleadoModal" runat="server" CssClass="form-select">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvTipoDocEmpleadoModal" runat="server" 
                                ControlToValidate="ddlTipoDocumentoEmpleadoModal" ErrorMessage="Tipo de documento es requerido" 
                                CssClass="text-danger" Display="Dynamic" InitialValue="" ValidationGroup="EmpleadoValidation" />
                        </div>

                        <!-- Email -->
                        <div class="col-md-6">
                            <label for="txtEmailEmpleadoModal" class="form-label">Email <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtEmailEmpleadoModal" runat="server" CssClass="form-control" TextMode="Email" />
                            <asp:RequiredFieldValidator ID="rfvEmailEmpleadoModal" runat="server" 
                                ControlToValidate="txtEmailEmpleadoModal" ErrorMessage="Email es requerido" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="EmpleadoValidation" />
                            <asp:RegularExpressionValidator ID="revEmailEmpleadoModal" runat="server" 
                                ControlToValidate="txtEmailEmpleadoModal" ErrorMessage="Formato de email inválido"
                                ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="EmpleadoValidation" />
                        </div>

                        <!-- Teléfono -->
                        <div class="col-md-6">
                            <label for="txtTelefonoEmpleadoModal" class="form-label">Teléfono <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtTelefonoEmpleadoModal" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="rfvTelefonoEmpleadoModal" runat="server" 
                                ControlToValidate="txtTelefonoEmpleadoModal" ErrorMessage="Teléfono es requerido" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="EmpleadoValidation" />
                        </div>

                        <!-- Contraseña -->
                        <div class="col-md-6">
                            <label for="txtContrasenaEmpleadoModal" class="form-label">Contraseña <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtContrasenaEmpleadoModal" runat="server" CssClass="form-control" TextMode="Password" />
                            <asp:RequiredFieldValidator ID="rfvContrasenaEmpleadoModal" runat="server" 
                                ControlToValidate="txtContrasenaEmpleadoModal" ErrorMessage="Contraseña es requerida" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="EmpleadoValidation" />
                        </div>

                        <!-- Confirmar contraseña -->
                        <div class="col-md-6">
                            <label for="txtConfirmPasswordEmpleadoModal" class="form-label">Confirmar Contraseña <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtConfirmPasswordEmpleadoModal" runat="server" CssClass="form-control" TextMode="Password" />
                            <asp:RequiredFieldValidator ID="rfvConfirmPasswordEmpleadoModal" runat="server" 
                                ControlToValidate="txtConfirmPasswordEmpleadoModal" ErrorMessage="Confirmación de contraseña es requerida" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="EmpleadoValidation" />
                            <asp:CompareValidator ID="cvPasswordEmpleadoModal" runat="server" 
                                ControlToValidate="txtConfirmPasswordEmpleadoModal" ControlToCompare="txtContrasenaEmpleadoModal"
                                ErrorMessage="Las contraseñas no coinciden" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="EmpleadoValidation" />
                        </div>

                        <!-- Sueldo -->
                        <div class="col-md-6">
                            <label for="txtSueldoModal" class="form-label">Sueldo (S/.) <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtSueldoModal" runat="server" CssClass="form-control" TextMode="Number" step="0.01" />
                            <asp:RequiredFieldValidator ID="rfvSueldoModal" runat="server" 
                                ControlToValidate="txtSueldoModal" ErrorMessage="Sueldo es requerido" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="EmpleadoValidation" />
                            <asp:RangeValidator ID="rvSueldoModal" runat="server" 
                                ControlToValidate="txtSueldoModal" MinimumValue="1" MaximumValue="999999"
                                Type="Double" ErrorMessage="El sueldo debe ser mayor a 0" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="EmpleadoValidation" />
                        </div>

                        <!-- Cantidad de reservas -->
                        <div class="col-md-6">
                            <label for="txtCantReservasEmpleadoModal" class="form-label">Cantidad de Reservas</label>
                            <asp:TextBox ID="txtCantReservasEmpleadoModal" runat="server" CssClass="form-control" TextMode="Number" Text="0" />
                            <asp:RangeValidator ID="rvCantReservasEmpleadoModal" runat="server" 
                                ControlToValidate="txtCantReservasEmpleadoModal" MinimumValue="0" MaximumValue="999999"
                                Type="Integer" ErrorMessage="La cantidad de reservas debe ser mayor o igual a 0" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="EmpleadoValidation" />
                        </div>

                        <!-- Fecha de contratación -->
                        <div class="col-md-6">
                            <label for="txtFechaContratacionModal" class="form-label">Fecha de Contratación <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtFechaContratacionModal" runat="server" CssClass="form-control" TextMode="Date" />
                            <asp:RequiredFieldValidator ID="rfvFechaContratacionModal" runat="server" 
                                ControlToValidate="txtFechaContratacionModal" ErrorMessage="Fecha de contratación es requerida" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="EmpleadoValidation" />
                        </div>

                        <!-- Estado -->
                        <div class="col-md-6">
                            <div class="form-check mt-4">
                                <asp:CheckBox ID="chkEstadoEmpleadoModal" runat="server" CssClass="form-check-input" Checked="true" />
                                <label class="form-check-label" for="chkEstadoEmpleadoModal">
                                    Empleado Activo
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnGuardarEmpleado" runat="server" Text="Guardar" CssClass="btn btn-danger fw-bold" 
                        OnClick="btnGuardarEmpleado_Click" ValidationGroup="EmpleadoValidation" />
                </div>
            </div>
        </div>
    </div>

    <!-- Scripts -->
    <script>
        function confirmarEliminacion(id, hdnId, btnEliminar) {
            Swal.fire({
                title: '¿Está seguro?',
                text: "¡No podrá revertir esta acción!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: '#3085d6',
                confirmButtonText: 'Sí, eliminar',
                cancelButtonText: 'Cancelar'
            }).then((result) => {
                if (result.isConfirmed) {
                    document.getElementById(hdnId).value = id;
                    document.getElementById(btnEliminar).click();
                }
            });
            return false;
        }
    </script>
</asp:Content>
