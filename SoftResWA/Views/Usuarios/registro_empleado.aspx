<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="registro_empleado.aspx.cs" Inherits="SoftResWA.Views.Usuarios.registro_empleado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Registro de Empleados
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container mt-4">
        <div class="card">
            <div class="card-header" style="background-color: #FFF3CD; color: #856404;">
                <h4><i class="fas fa-user-plus me-2"></i><asp:Label ID="lblTitulo" runat="server" Text="Registrar Empleado" /></h4>
            </div>
            <div class="card-body">
                <asp:ScriptManager ID="ScriptManager1" runat="server" />
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

                    <!-- Contraseña -->
                    <div class="col-md-6">
                        <label for="txtContrasena" class="form-label">Contraseña <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtContrasena" runat="server" CssClass="form-control" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="rfvContrasena" runat="server" 
                            ControlToValidate="txtContrasena" ErrorMessage="Contraseña es requerida" 
                            CssClass="text-danger" Display="Dynamic" />
                    </div>

                    <!-- Validar contraseña -->
                    <div class="col-md-6">
                        <label for="txtConfirmPassword" class="form-label">Confirmar Contraseña <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" 
                            ControlToValidate="txtConfirmPassword" ErrorMessage="Confirmación de contraseña es requerida" 
                            CssClass="text-danger" Display="Dynamic" />
                        <asp:CompareValidator ID="cvPassword" runat="server" 
                            ControlToValidate="txtConfirmPassword" ControlToCompare="txtContrasena"
                            ErrorMessage="Las contraseñas no coinciden" 
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

                    <!-- Cantidad de reservas -->
                    <div class="col-md-6">
                        <label for="txtCantReservas" class="form-label">Cantidad de Reservas</label>
                        <asp:TextBox ID="txtCantReservas" runat="server" CssClass="form-control" TextMode="Number" Text="0" />
                        <asp:RangeValidator ID="rvCantReservas" runat="server" 
                            ControlToValidate="txtCantReservas" MinimumValue="0" MaximumValue="999999"
                            Type="Integer" ErrorMessage="La cantidad de reservas debe ser mayor o igual a 0" 
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

                    <!-- Estado -->
                    <div class="col-md-6">
                        <div class="form-check mt-4">
                            <asp:CheckBox ID="chkEstado" runat="server" CssClass="form-check-input" Checked="true" />
                            <label class="form-check-label" for="chkEstado">
                                Empleado Activo
                            </label>
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
</asp:Content>

