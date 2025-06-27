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
                        OnClientClick="window.location.href='registro_cliente.aspx'; return false;" />
                </div>
            </div>
            <div class="card-body">
                <asp:ScriptManager ID="ScriptManager1" runat="server" />
                
                <!-- Filtros de búsqueda -->
                <div class="row g-3 mb-4">
                    <div class="col-md-3">
                        <label for="txtNombreCompFiltro" class="form-label">Nombre Completo</label>
                        <asp:TextBox ID="txtNombreCompFiltro" runat="server" CssClass="form-control" placeholder="Buscar por nombre..." />
                    </div>
                    <div class="col-md-3">
                        <label for="txtNumeroDocFiltro" class="form-label">Número de Documento</label>
                        <asp:TextBox ID="txtNumeroDocFiltro" runat="server" CssClass="form-control" placeholder="Buscar por documento..." />
                    </div>
                    <div class="col-md-3">
                        <label for="ddlTipoDocumentoFiltro" class="form-label">Tipo de Documento</label>
                        <asp:DropDownList ID="ddlTipoDocumentoFiltro" runat="server" CssClass="form-select">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
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
                            <asp:BoundField DataField="tipoDocumento" HeaderText="Tipo Doc." ItemStyle-Width="100px" />
                            <asp:BoundField DataField="numeroDoc" HeaderText="Nº Documento" ItemStyle-Width="120px" />
                            <asp:BoundField DataField="email" HeaderText="Email" ItemStyle-Width="200px" />
                            <asp:BoundField DataField="telefono" HeaderText="Teléfono" ItemStyle-Width="120px" />
                            <asp:BoundField DataField="cantReservas" HeaderText="Reservas" ItemStyle-Width="80px" />
                            <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha Creación" ItemStyle-Width="120px" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="estadoTexto" HeaderText="Estado" ItemStyle-Width="80px" />
                            
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkModificar" runat="server" 
                                        CssClass="btn btn-outline-warning btn-sm me-1" 
                                        NavigateUrl='<%# "registro_cliente.aspx?id=" + Eval("idUsuario") %>'
                                        ToolTip="Modificar">
                                        <i class="fas fa-edit"></i>
                                    </asp:HyperLink>
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
