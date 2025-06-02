<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="registro_empleado.aspx.cs" Inherits="SoftResWA.Views.Usuarios.registro_empleado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Registrar Empleado
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container mt-4">
        <div class="card">
            <div class="card-header" style="background-color: #FFF3CD; color: #856404;">
                <h4><i class="fas fa-user-plus me-2"></i>Registrar Empleado</h4>
            </div>
            <div class="card-body">
                <div class="row g-3">

                    <!-- Nombre completo -->
                    <div class="col-md-6">
                        <label for="txtNombreCompleto" class="form-label">Nombre Completo</label>
                        <asp:TextBox ID="txtNombreCompleto" runat="server" CssClass="form-control" />
                    </div>

                    <!-- Rol -->
                    <div class="col-md-6">
                        <label for="ddlRol" class="form-label">Rol</label>
                        <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Seleccionar..." Value="" />
                            <asp:ListItem Text="Empleado" Value="Empleado" />
                            <asp:ListItem Text="Administrador" Value="Administrador" />
                        </asp:DropDownList>
                    </div>

                    <!-- Número de documento -->
                    <div class="col-md-6">
                        <label for="txtDocumento" class="form-label">Número de Documento</label>
                        <asp:TextBox ID="txtDocumento" runat="server" CssClass="form-control" />
                    </div>

                    <!-- Tipo de documento -->
                    <div class="col-md-6">
                        <label for="ddlTipoDocumento" class="form-label">Tipo de Documento</label>
                        <asp:DropDownList ID="ddlTipoDocumento" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Seleccionar..." Value="" />
                            <asp:ListItem Text="DNI" Value="DNI" />
                            <asp:ListItem Text="Carnet de Extranjería" Value="Carnet" />
                            <asp:ListItem Text="Pasaporte" Value="Pasaporte" />
                        </asp:DropDownList>
                    </div>

                    <!-- Email -->
                    <div class="col-md-6">
                        <label for="txtEmail" class="form-label">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                    </div>

                    <!-- Teléfono -->
                    <div class="col-md-6">
                        <label for="txtTelefono" class="form-label">Teléfono</label>
                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
                    </div>

                    <!-- Contraseña -->
                    <div class="col-md-6">
                        <label for="txtPassword" class="form-label">Contraseña</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                    </div>

                    <!-- Validar contraseña -->
                    <div class="col-md-6">
                        <label for="txtConfirmPassword" class="form-label">Validar Contraseña</label>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" />
                    </div>

                    <!-- Sueldo -->
                    <div class="col-md-6">
                        <label for="txtSueldo" class="form-label">Sueldo (S/.)</label>
                        <asp:TextBox ID="txtSueldo" runat="server" CssClass="form-control" TextMode="Number" />
                    </div>

                    <!-- Fecha de contratación con calendario toggle -->
                    <div class="col-md-6">
                        <label for="txtFechaContratacion" class="form-label">Fecha de Contratación</label>
                        <div class="input-group">
                            <asp:TextBox ID="txtFechaContratacion" runat="server" CssClass="form-control" />
                            <asp:LinkButton ID="btnCalendario" runat="server" CssClass="btn btn-outline-secondary" OnClick="btnCalendario_Click">
                                <i class="fas fa-calendar-alt"></i>
                            </asp:LinkButton>
                        </div>
                        <asp:Calendar ID="calFechaContratacion" runat="server" CssClass="mt-2 border rounded p-2 bg-white" Visible="false" OnSelectionChanged="calFechaContratacion_SelectionChanged" />
                    </div>

                </div>

                <!-- Botón Guardar -->
                <div class="text-end mt-4">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar"
                        CssClass="btn btn-danger px-4"
                        OnClick="btnGuardar_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

