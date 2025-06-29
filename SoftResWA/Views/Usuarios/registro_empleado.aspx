<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="registro_empleado.aspx.cs" Inherits="SoftResWA.Views.Usuarios.registro_empleado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Registro de Empleados
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container mt-4">
        <div class="card">
            <div class="card-header" style="background-color: #FFF3CD; color: #856404;">
                <h4><i class="fas fa-user-plus me-2"></i>
                    <asp:Label ID="lblTitulo" runat="server" Text="Registrar Empleado" /></h4>
            </div>
            <div class="card-body">
                <div class="row g-3">

                    <!-- Nombre completo -->
                    <div class="col-md-6">
                        <label for="txtNombreCompleto" class="form-label">Nombre Completo <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtNombreCompleto" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                            ControlToValidate="txtNombreCompleto" ErrorMessage="Nombre completo es requerido"
                            CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <!-- Rol -->
                    <div class="col-md-6">
                        <label for="ddlRol" class="form-label">Rol <span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvRol" runat="server"
                            ControlToValidate="ddlRol" ErrorMessage="Rol es requerido"
                            CssClass="text-danger" Display="Dynamic" InitialValue="" />
                    </div>

                    <!-- Número de documento -->
                    <div class="col-md-6">
                        <label for="txtNumeroDocumento" class="form-label">Número de Documento <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtNumeroDocumento" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvNumeroDoc" runat="server"
                            ControlToValidate="txtNumeroDocumento" ErrorMessage="Número de documento es requerido"
                            CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <!-- Tipo de documento -->
                    <div class="col-md-6">
                        <label for="ddlTipoDocumento" class="form-label">Tipo de Documento <span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlTipoDocumento" runat="server" CssClass="form-select">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvTipoDoc" runat="server"
                            ControlToValidate="ddlTipoDocumento" ErrorMessage="Tipo de documento es requerido"
                            CssClass="text-danger" Display="Dynamic" InitialValue="" />
                    </div>

                    <!-- Email -->
                    <div class="col-md-6">
                        <label for="txtEmail" class="form-label">Email <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                            ControlToValidate="txtEmail" ErrorMessage="Email es requerido"
                            CssClass="text-danger" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server"
                            ControlToValidate="txtEmail" ErrorMessage="Formato de email inválido"
                            ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                            CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <!-- Teléfono -->
                    <div class="col-md-6">
                        <label for="txtTelefono" class="form-label">Teléfono <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvTelefono" runat="server"
                            ControlToValidate="txtTelefono" ErrorMessage="Teléfono es requerido"
                            CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <!-- Sueldo -->
                    <div class="col-md-6">
                        <label for="txtSueldo" class="form-label">Sueldo (S/.) <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtSueldo" runat="server" CssClass="form-control" TextMode="Number" step="0.01" />
                        <asp:RequiredFieldValidator ID="rfvSueldo" runat="server"
                            ControlToValidate="txtSueldo" ErrorMessage="Sueldo es requerido"
                            CssClass="text-danger" Display="Dynamic" />
                        <asp:RangeValidator ID="rvSueldo" runat="server"
                            ControlToValidate="txtSueldo" MinimumValue="1" MaximumValue="999999"
                            Type="Double" ErrorMessage="El sueldo debe ser mayor a 0"
                            CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <!-- Fecha de contratación con calendario toggle -->
                    <div class="col-md-6">
                        <label for="txtFechaContratacion" class="form-label">Fecha de Contratación <span class="text-danger">*</span></label>
                        <div class="input-group">
                            <asp:TextBox ID="txtFechaContratacion" runat="server" CssClass="form-control" placeholder="dd-MM-yyyy" />
                            <asp:LinkButton ID="btnCalendario" runat="server" CssClass="btn btn-outline-secondary" OnClick="btnCalendario_Click" CausesValidation="false">
                                <i class="fas fa-calendar-alt"></i>
                            </asp:LinkButton>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvFechaContratacion" runat="server"
                            ControlToValidate="txtFechaContratacion" ErrorMessage="Fecha de contratación es requerida"
                            CssClass="text-danger" Display="Dynamic" />
                        <asp:Calendar ID="calFechaContratacion" runat="server" CssClass="mt-2 border rounded p-2 bg-white" Visible="false" OnSelectionChanged="calFechaContratacion_SelectionChanged" />
                    </div>
                    <!-- Contraseña -->
                    <div class="row" id="divContrasenas" runat="server" visible="true">
                        <div class="col-md-6">
                            <label for="txtContrasena" class="form-label">Contraseña <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtContrasena" runat="server" CssClass="form-control" TextMode="Password" />
                            <asp:RequiredFieldValidator ID="rfvContrasena" runat="server"
                                ControlToValidate="txtContrasena" ErrorMessage="El campo es requerido"
                                CssClass="text-danger" Display="Dynamic" />
                        </div>
                        <div class="col-md-6">
                            <label for="txtConfirmPassword" class="form-label">Confirmar Contraseña <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" />
                            <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server"
                                ControlToValidate="txtConfirmPassword" ErrorMessage="El campo es requerido"
                                CssClass="text-danger" Display="Dynamic" />
                            <asp:CompareValidator ID="cvPassword" runat="server"
                                ControlToValidate="txtConfirmPassword" ControlToCompare="txtContrasena"
                                ErrorMessage="Las contraseñas no coinciden"
                                CssClass="text-danger" Display="Dynamic" />
                        </div>
                    </div>

                    <asp:CheckBox ID="chkCambiarContrasena" runat="server" Text="¿Deseas cambiar tu contraseña?" AutoPostBack="true" OnCheckedChanged="chkCambiarContrasena_CheckedChanged" Visible="false" CssClass="mt-3" />
                    <!-- Botón que abre modal si chk está activo -->
                    <asp:Button ID="btnAbrirModalCambio" runat="server" Text="Cambiar contraseña por correo"
                        CssClass="btn btn-outline-danger mt-2"
                        OnClientClick="abrirModalCorreo(); return false;"
                        Visible="false" />
                    <div class="modal fade" id="modalCorreoCambio" tabindex="-1" aria-labelledby="modalCorreoCambioLabel" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header bg-danger text-white">
                                    <h5 class="modal-title" id="modalCorreoCambioLabel">Recuperar Contraseña</h5>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                                </div>
                                <div class="modal-body">
                                    <p>Se enviará un enlace de recuperación a la siguiente dirección de correo electrónico:</p>
                                    <div class="text-center my-3">
                                        <strong>
                                            <asp:Label ID="lblCorreoConfirmacion" runat="server" Text="[Correo del Empleado]"></asp:Label>
                                        </strong>
                                    </div>
                                    <p>¿Deseas continuar?</p>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnEnviarCorreoCambio" runat="server" CssClass="btn btn-danger" Text="Enviar" OnClick="btnEnviarCorreoCambio_Click" />
                                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Botones -->
                <div class="text-end mt-4">
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"
                        CssClass="btn btn-secondary me-2 px-4"
                        OnClick="btnCancelar_Click" CausesValidation="false" />
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar"
                        CssClass="btn btn-danger px-4"
                        OnClick="btnGuardar_Click" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function abrirModalCorreo() {
            var correoEmpleado = document.getElementById('<%= txtEmail.ClientID %>').value;
            if (correoEmpleado === '') {
                Swal.fire('Atención', 'El campo de correo no puede estar vacío.', 'warning');
                return;
            }
            document.getElementById('<%= lblCorreoConfirmacion.ClientID %>').innerText = correoEmpleado;
            var modal = new bootstrap.Modal(document.getElementById('modalCorreoCambio'));
            modal.show();
        }
    </script>
</asp:Content>

