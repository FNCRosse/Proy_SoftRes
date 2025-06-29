<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="listado_cliente.aspx.cs" Inherits="SoftResWA.Views.Usuarios.listado_cliente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Gestión de Clientes
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="text-start mb-3">
        <h1 class="h3 text-danger fw-bold mb-0 me-3">
            <i class="fas fa-users me-2"></i>Gestión de Clientes
        </h1>
    </div>

    <!-- Filtros de búsqueda -->
    <div class="container-fluid mb-4 ps-4">
        <div class="row gx-3">
            <div class="col-auto">
                <label for="txtNombreCompFiltro" class="form-label">Nombre Completo</label>
                <asp:TextBox ID="txtNombreCompFiltro" runat="server" CssClass="form-control" placeholder="Buscar por nombre..." />
            </div>
            <div class="col-auto">
                <label for="txtNumeroDocFiltro" class="form-label">Número de Documento</label>
                <asp:TextBox ID="txtNumeroDocFiltro" runat="server" CssClass="form-control" placeholder="Buscar por documento..." />
            </div>
            <div class="col-auto">
                <label for="ddlTipoDocumentoFiltro" class="form-label">Tipo de Documento</label>
                <asp:DropDownList ID="ddlTipoDocumentoFiltro" runat="server" CssClass="form-select">
                </asp:DropDownList>
            </div>
            <div class="col-auto">
                <label for="ddlTipoClienteFiltro" class="form-label">Tipo de Cliente</label>
                <asp:DropDownList ID="ddlTipoClienteFiltro" runat="server" CssClass="form-select">
                </asp:DropDownList>
            </div>
            <div class="col-auto">
                <label for="ddlEstadoFiltro" class="form-label">Estado</label>
                <asp:DropDownList ID="ddlEstadoFiltro" runat="server" CssClass="form-select">
                    <asp:ListItem Text="-- Todos --" Value="" Selected="True" />
                    <asp:ListItem Text="Activo" Value="1" />
                    <asp:ListItem Text="Inactivo" Value="0" />
                </asp:DropDownList>
            </div>
            <div class="col-auto d-flex align-items-end">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar"
                    CssClass="btn btn-danger me-2"
                    OnClick="btnBuscar_Click" />
                <asp:Button ID="btnNuevoCliente" runat="server" Text="Nuevo Cliente"
                    CssClass="btn shadow-sm"
                    OnClientClick="window.location.href='registro_cliente.aspx'; return false;"
                    Style="background-color: #FFF3CD; color: #856404; border: 1px solid #d39e00;" />
            </div>
        </div>
    </div>

    <!-- Tabla -->
    <div class="row">
        <asp:GridView ID="dgvClientes" runat="server" CssClass="table table-hover table-striped table-responsive"
            GridLines="None" AutoGenerateColumns="False" AllowPaging="False">
            <HeaderStyle CssClass="table-dark" />
            <Columns>
                <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkModificar" runat="server"
                            CssClass="btn btn-sm btn-primary"
                            NavigateUrl='<%# "registro_cliente.aspx?id=" + Eval("idUsuario") %>'
                            ToolTip="Modificar"
                            Text="M">
                        </asp:HyperLink>
                        <asp:LinkButton ID="btnEliminar" runat="server"
                            CssClass="btn btn-sm btn-danger"
                            ToolTip="Eliminar"
                            Text="C">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="idUsuario" HeaderText="ID" ItemStyle-Width="60px" />
                <asp:BoundField DataField="tipoCliente" HeaderText="Tipo Cliente" ItemStyle-Width="60px" />
                <asp:BoundField DataField="nombreComp" HeaderText="Nombre Completo" ItemStyle-Width="200px" />
                <asp:BoundField DataField="tipoDocumento" HeaderText="Tipo Doc." ItemStyle-Width="100px" />
                <asp:BoundField DataField="numeroDocumento" HeaderText="Nº Documento" ItemStyle-Width="120px" />
                <asp:BoundField DataField="email" HeaderText="Email" ItemStyle-Width="200px" />
                <asp:BoundField DataField="telefono" HeaderText="Teléfono" ItemStyle-Width="120px" />
                <asp:BoundField DataField="cantidadReservacion" HeaderText="Reservas" ItemStyle-Width="80px" />
                <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha Creación" ItemStyle-Width="120px" />
                <asp:BoundField DataField="usuarioCreacion" HeaderText="Usuario Creación" ItemStyle-Width="120px" />
                <asp:BoundField DataField="fechaModificacion" HeaderText="Fecha Mod" ItemStyle-Width="120px" />
                <asp:BoundField DataField="usuarioModificacion" HeaderText="Usuario Mod" ItemStyle-Width="120px" />
                <asp:BoundField DataField="estadoTexto" HeaderText="Estado" ItemStyle-Width="80px" />
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

    <!-- Hidden fields para eliminar -->
    <asp:HiddenField ID="hdnIdEliminar" runat="server" />
    <asp:Button ID="btnEliminarCliente" runat="server" Style="display: none;" OnClick="btnEliminarCliente_Click" />
</asp:Content>

