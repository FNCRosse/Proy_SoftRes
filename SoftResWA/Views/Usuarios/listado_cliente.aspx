<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="listado_cliente.aspx.cs" Inherits="SoftResWA.Views.Usuarios.listado_cliente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Gestión de Clientes
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container-fluid mt-4">
        <div class="card">
            <div class="card-header" style="background-color: #FFF3CD; color: #856404;">
                <div class="d-flex justify-content-between align-items-center">
                    <h4><i class="fas fa-users me-2 text-danger"></i>Listado de Clientes</h4>
                    <asp:Button ID="btnNuevoCliente" runat="server" Text="Nuevo Cliente"
                        CssClass="btn btn-danger px-4" 
                        OnClick="btnNuevoCliente_Click" />
                </div>
            </div>
            <div class="card-body">
                <asp:ScriptManager ID="ScriptManager1" runat="server" />
                
                <!-- Filtros de búsqueda -->
                <div class="row g-3 mb-4">
                    <div class="col-md-4">
                        <label for="txtNombreCompFiltro" class="form-label">Nombre Completo</label>
                        <asp:TextBox ID="txtNombreCompFiltro" runat="server" CssClass="form-control" placeholder="Buscar por nombre..." />
                    </div>
                    <div class="col-md-4">
                        <label for="ddlTipoDocumentoFiltro" class="form-label">Tipo de Documento</label>
                        <asp:DropDownList ID="ddlTipoDocumentoFiltro" runat="server" CssClass="form-select">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4">
                        <label for="ddlEstadoFiltro" class="form-label">Estado</label>
                        <asp:DropDownList ID="ddlEstadoFiltro" runat="server" CssClass="form-select">
                        </asp:DropDownList>
                    </div>
                    <div class="col-12">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar"
                            CssClass="btn btn-outline-danger me-2"
                            OnClick="btnBuscar_Click" />
                    </div>
                </div>

                <!-- GridView de clientes -->
                <div class="table-responsive">
                    <asp:GridView ID="dgvClientes" runat="server" CssClass="table table-hover table-striped" 
                        GridLines="None" AutoGenerateColumns="False" AllowPaging="False">
                        <HeaderStyle CssClass="table-dark" />
                        <Columns>
                            <asp:BoundField DataField="idUsuario" HeaderText="ID" ItemStyle-Width="60px" />
                            <asp:BoundField DataField="nombreComp" HeaderText="Nombre Completo" ItemStyle-Width="200px" />
                            <asp:BoundField DataField="tipoDocumentoNombre" HeaderText="Tipo Doc." ItemStyle-Width="100px" />
                            <asp:BoundField DataField="numeroDocumento" HeaderText="Nº Documento" ItemStyle-Width="120px" />
                            <asp:BoundField DataField="email" HeaderText="Email" ItemStyle-Width="200px" />
                            <asp:BoundField DataField="telefono" HeaderText="Teléfono" ItemStyle-Width="120px" />
                            <asp:BoundField DataField="cantidadReservacion" HeaderText="Reservas" ItemStyle-Width="80px" />
                            <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha Creación" ItemStyle-Width="120px" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="estadoTexto" HeaderText="Estado" ItemStyle-Width="80px" />
                            
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkModificar" runat="server" 
                                        CssClass="btn btn-outline-warning btn-sm me-1" 
                                        CommandArgument='<%# Eval("idUsuario") %>'
                                        OnCommand="btnModificarCliente_Command"
                                        ToolTip="Modificar">
                                        <i class="fas fa-edit"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnEliminar" runat="server" 
                                        CssClass="btn btn-outline-danger btn-sm" 
                                        ToolTip="Eliminar">
                                        <i class="fas fa-trash-alt"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="text-center p-4">
                                <i class="fas fa-users fa-3x text-muted mb-3"></i>
                                <h5 class="text-muted">No hay clientes registrados</h5>
                                <p class="text-muted">Comience agregando un nuevo cliente.</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

    <!-- Hidden fields para eliminar -->
    <asp:HiddenField ID="hdnIdEliminar" runat="server" />
    <asp:Button ID="btnEliminarCliente" runat="server" style="display:none;" OnClick="btnEliminarCliente_Click" />

    <!-- Hidden fields para modal -->
    <asp:HiddenField ID="hdnModoModal" runat="server" />
    <asp:HiddenField ID="hdnIdCliente" runat="server" />

    <!-- Modal para Registrar/Modificar Cliente -->
    <div class="modal fade" id="modalRegistrarCliente" tabindex="-1" aria-labelledby="tituloModalCliente" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-centered">
            <div class="modal-content border-warning">
                <div class="modal-header bg-warning-subtle">
                    <h5 class="modal-title fw-bold" id="tituloModalCliente">
                        <i class="fas fa-user-plus me-2 text-danger"></i>Registrar Cliente
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <div class="row g-3">
                        <!-- Nombre completo -->
                        <div class="col-md-6">
                            <label for="txtNombreCompleto" class="form-label">Nombre Completo <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtNombreCompleto" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                                ControlToValidate="txtNombreCompleto" ErrorMessage="Nombre completo es requerido" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="ClienteValidation" />
                        </div>

                        <!-- Tipo de documento -->
                        <div class="col-md-6">
                            <label for="ddlTipoDocumentoModal" class="form-label">Tipo de Documento <span class="text-danger">*</span></label>
                            <asp:DropDownList ID="ddlTipoDocumentoModal" runat="server" CssClass="form-select">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvTipoDocModal" runat="server" 
                                ControlToValidate="ddlTipoDocumentoModal" ErrorMessage="Tipo de documento es requerido" 
                                CssClass="text-danger" Display="Dynamic" InitialValue="" ValidationGroup="ClienteValidation" />
                        </div>

                        <!-- Número de documento -->
                        <div class="col-md-6">
                            <label for="txtNumeroDocumento" class="form-label">Número de Documento <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtNumeroDocumento" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="rfvNumeroDoc" runat="server" 
                                ControlToValidate="txtNumeroDocumento" ErrorMessage="Número de documento es requerido" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="ClienteValidation" />
                        </div>

                        <!-- Email -->
                        <div class="col-md-6">
                            <label for="txtEmailModal" class="form-label">Email <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtEmailModal" runat="server" CssClass="form-control" TextMode="Email" />
                            <asp:RequiredFieldValidator ID="rfvEmailModal" runat="server" 
                                ControlToValidate="txtEmailModal" ErrorMessage="Email es requerido" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="ClienteValidation" />
                            <asp:RegularExpressionValidator ID="revEmailModal" runat="server" 
                                ControlToValidate="txtEmailModal" ErrorMessage="Formato de email inválido"
                                ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="ClienteValidation" />
                        </div>

                        <!-- Teléfono -->
                        <div class="col-md-6">
                            <label for="txtTelefonoModal" class="form-label">Teléfono <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtTelefonoModal" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="rfvTelefonoModal" runat="server" 
                                ControlToValidate="txtTelefonoModal" ErrorMessage="Teléfono es requerido" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="ClienteValidation" />
                        </div>

                        <!-- Contraseña -->
                        <div class="col-md-6">
                            <label for="txtContrasenaModal" class="form-label">Contraseña <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtContrasenaModal" runat="server" CssClass="form-control" TextMode="Password" />
                            <asp:RequiredFieldValidator ID="rfvContrasenaModal" runat="server" 
                                ControlToValidate="txtContrasenaModal" ErrorMessage="Contraseña es requerida" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="ClienteValidation" />
                        </div>

                        <!-- Confirmar contraseña -->
                        <div class="col-md-6">
                            <label for="txtConfirmPasswordModal" class="form-label">Confirmar Contraseña <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtConfirmPasswordModal" runat="server" CssClass="form-control" TextMode="Password" />
                            <asp:RequiredFieldValidator ID="rfvConfirmPasswordModal" runat="server" 
                                ControlToValidate="txtConfirmPasswordModal" ErrorMessage="Confirmación de contraseña es requerida" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="ClienteValidation" />
                            <asp:CompareValidator ID="cvPasswordModal" runat="server" 
                                ControlToValidate="txtConfirmPasswordModal" ControlToCompare="txtContrasenaModal"
                                ErrorMessage="Las contraseñas no coinciden" 
                                CssClass="text-danger" Display="Dynamic" ValidationGroup="ClienteValidation" />
                        </div>

                        <!-- Cantidad de reservas -->
                        <div class="col-md-6">
                            <label for="txtCantReservasModal" class="form-label">Cantidad de Reservas</label>
                            <asp:TextBox ID="txtCantReservasModal" runat="server" CssClass="form-control" TextMode="Number" Text="0" ReadOnly="true" />
                            <small class="form-text text-muted">Este campo se actualiza automáticamente</small>
                        </div>

                        <!-- Estado -->
                        <div class="col-md-6">
                            <div class="form-check mt-4">
                                <asp:CheckBox ID="chkEstadoModal" runat="server" CssClass="form-check-input" Checked="true" />
                                <label class="form-check-label" for="chkEstadoModal">
                                    Cliente Activo
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnGuardarCliente" runat="server" Text="Guardar" CssClass="btn btn-danger fw-bold" 
                        OnClick="btnGuardarCliente_Click" ValidationGroup="ClienteValidation" />
                </div>
            </div>
        </div>
    </div>

    <!-- JavaScript para confirmación de eliminación -->
    <script type="text/javascript">
        function confirmarEliminacion(id, hdnId, btnEliminar) {
            Swal.fire({
                title: '¿Está seguro?',
                text: "Esta acción eliminará el cliente permanentemente",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: '#6c757d',
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
