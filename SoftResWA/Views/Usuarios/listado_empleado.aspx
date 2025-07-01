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
                    <asp:ListItem Text="-- Todos --" Value="" Selected="True" />
                    <asp:ListItem Text="Activo" Value="1" />
                    <asp:ListItem Text="Inactivo" Value="0" />
                </asp:DropDownList>
            </div>
            <!-- Botones -->
            <div class="col-auto d-flex align-items-end">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-danger me-2"
                    OnClick="btnBuscar_Click"></asp:Button>
                <asp:Button ID="btnNuevoEmp" runat="server" Text="Nuevo Empleado"
                    CssClass="btn shadow-sm"
                    OnClientClick="window.location.href='registro_empleado.aspx'; return false;"
                    Style="background-color: #FFF3CD; color: #856404; border: 1px solid #d39e00;" />
            </div>
        </div>
    </div>

    <!-- Tabla -->
    <div class="row">
        <asp:GridView ID="dgvEmpleados" runat="server" AllowPaging="false" AutoGenerateColumns="false"
            CssClass="table table-hover table-responsive table-striped">
            <Columns>
                <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkModificar" runat="server" CssClass="btn btn-sm btn-primary"
                            NavigateUrl='<%# "~/Views/Usuarios/registro_empleado.aspx?id=" + Eval("idUsuario") %>'
                            Text="M"
                            ToolTip="Modificar">
                        </asp:HyperLink>
                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-sm btn-danger"
                            ToolTip="Eliminar"
                            Text="C">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="ID" DataField="idUsuario" />
                <asp:BoundField DataField="tipoEmp" HeaderText="Rol" ItemStyle-Width="60px" />
                <asp:BoundField HeaderText="Nombre Completo" DataField="nombreComp" />
                <asp:BoundField HeaderText="Tipo Doc." DataField="tipoDocumento" />
                <asp:BoundField HeaderText="Número Documento" DataField="numeroDocumento" />
                <asp:BoundField HeaderText="Email" DataField="email" />
                <asp:BoundField HeaderText="Teléfono" DataField="telefono" />
                <asp:BoundField HeaderText="Sueldo" DataField="sueldo" DataFormatString="{0:C}" />
                <asp:BoundField HeaderText="Fecha Contratación" DataField="fechaContratacion" />
                <asp:BoundField HeaderText="Fecha Creación" DataField="fechaCreacion" />
                <asp:BoundField HeaderText="Usuario Creación" DataField="usuarioCreacion" />
                <asp:BoundField HeaderText="Fecha Modificación" DataField="fechaModificacion" />
                <asp:BoundField HeaderText="Usuario Modificación" DataField="usuarioModificacion" />
                <asp:BoundField HeaderText="Estado" DataField="estadoTexto" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- Controles ocultos -->
    <asp:HiddenField ID="hdnIdEliminar" runat="server" />
    <asp:Button ID="btnEliminarUsuario" runat="server" Style="display: none;" OnClick="btnEliminarUsuario_Click" />
</asp:Content>
