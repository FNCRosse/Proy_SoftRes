<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="listado_empleado.aspx.cs" Inherits="SoftResWA.Views.Usuarios.listado_empleado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Mantenimineto de empleados
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="text-start mb-3">
        <h1 class="h3 text-danger fw-bold mb-0 me-3">
            <i class="fas fa-gears me-2"></i>Mantenimiento de Empleados
        </h1>
    </div>
    <!-- Filtros para  -->
    <div class="container-fluid mb-4 ps-4">
        <div class="row gx-3">
            <!-- Nombre -->
            <div class="col-auto">
                <label for="txtNombreComp" class="form-label">Nombre Completo</label>
                <input type="text" id="txtNombreComp" class="form-control" placeholder="Ej. Manuel Perez" />
            </div>
            <!-- Tipo Documento -->
            <div class="col-auto">
                <label for="ddlSede" class="form-label">Tipo Documento</label>
                <select id="ddlSede" class="form-select">
                    <option selected disabled>Seleccionar...</option>
                    <option value="1">DNI</option>
                    <option value="0">Pasaporte</option>
                    <option value="0">Carnet de extranjeria</option>
                </select>
            </div>
            <!-- Rol -->
            <div class="col-auto">
                <label for="ddlRol" class="form-label">Rol </label>
                <select id="ddlRol" class="form-select">
                    <option selected disabled>Seleccionar...</option>
                    <option value="1">Vendedor</option>
                    <option value="0">Administrador</option>
                </select>
            </div>
            <!-- Estado -->
            <div class="col-auto">
                <label for="ddlEstado" class="form-label">Estado</label>
                <select id="ddlEstado" class="form-select">
                    <option selected disabled>Seleccionar...</option>
                    <option value="1">Activo</option>
                    <option value="0">Inactivo</option>
                </select>
            </div>
            <!-- Botones -->
            <div class="col-auto d-flex align-items-end">
                <button type="button" class="btn btn-danger me-2">
                    <i class="fas fa-search me-1"></i>Buscar
                </button>
                <asp:HyperLink ID="lnkNuevoEmpleado" runat="server"
                    NavigateUrl="~/Views/Usuarios/registro_empleado.aspx"
                    CssClass="btn"
                    Style="background-color: #FFF3CD; color: #856404; border: 1px solid #d39e00;">
                    <i class="fas fa-plus me-1"></i>Nuevo
                </asp:HyperLink>
            </div>
        </div>
    </div>

    <!-- Tabla -->
    <div class="row">
        <asp:GridView ID="dgvLocal" runat="server" AllowPaging="false" AutoGenerateColumns="false"
            CssClass="table table-hover table-responsive table-striped">
            <Columns>
                <asp:BoundField HeaderText="Código" DataField="SedeId" />
                <asp:BoundField HeaderText="Nombre Completo" DataField="NombreComp" />
                <asp:BoundField HeaderText="Tipo de documento" DataField="TipDoc" />
                <asp:BoundField HeaderText="Número de documento" DataField="NumDoc" />
                <asp:BoundField HeaderText="Rol" DataField="Rol" />
                <asp:BoundField HeaderText="Email" DataField="Email" />
                <asp:BoundField HeaderText="Teléfono" DataField="Telefono" />
                <asp:BoundField HeaderText="Sueldo" DataField="Telefono" />
                <asp:BoundField HeaderText="Fecha de Contratación" DataField="FecContratacion" />
                <asp:BoundField HeaderText="Fecha Creacion" DataField="FechaCrea" />
                <asp:BoundField HeaderText="Usuario Creacion" DataField="UsuarioCrea" />
                <asp:BoundField HeaderText="Fecha Modificacion" DataField="FechaMod" />
                <asp:BoundField HeaderText="Usuario Modificacion" DataField="UsuarioMod" />
                <asp:BoundField HeaderText="Estado" DataField="Estado" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
