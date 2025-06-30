<%@ Page Title="Gestión de Notificaciones" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="Notificaciones_gestion.aspx.cs" Inherits="SoftResWA.Views.Home.Notificaciones_gestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Gestión de Notificaciones
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="text-start mb-3">
        <h1 class="h3 text-danger fw-bold mb-0 me-3">
            <i class="fas fa-bell me-2"></i>Gestión de Notificaciones
        </h1>
    </div>

    <!-- Filtros para búsqueda -->
    <div class="container-fluid mb-4 ps-4">
        <div class="row gx-3">
            <!-- Tipo de Notificación -->
            <div class="col-auto">
                <label for="ddlTipoNotificacionFiltro" class="form-label">Tipo de Notificación</label>
                <asp:DropDownList ID="ddlTipoNotificacionFiltro" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
            <!-- Estado de Envío -->
            <div class="col-auto">
                <label for="ddlEstadoFiltro" class="form-label">Estado de Envío</label>
                <asp:DropDownList ID="ddlEstadoFiltro" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
            <!-- Estado de Lectura -->
            <div class="col-auto">
                <label for="ddlLeidaFiltro" class="form-label">Estado de Lectura</label>
                <asp:DropDownList ID="ddlLeidaFiltro" runat="server" CssClass="form-select">
                    <asp:ListItem Value="" Text="Todos"></asp:ListItem>
                    <asp:ListItem Value="0" Text="No Leídas"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Leídas"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <!-- Botones -->
            <div class="col-auto d-flex align-items-end">
                <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-danger me-2"
                    Text="Buscar"
                    CausesValidation="false"
                    OnClick="btnBuscar_Click" />
            </div>
        </div>
    </div>

    <!-- Tabla de Resultados -->
    <div class="row">
        <asp:GridView ID="gvNotificaciones" runat="server"
            AllowPaging="True" PageSize="15"
            AutoGenerateColumns="False"
            CssClass="table table-hover table-responsive table-striped">
            <Columns>
                <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="text-center">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnMarcarLeida" runat="server" CssClass="btn btn-sm btn-outline-secondary" ToolTip="Marcar como Leída/No Leída"
                            OnCommand="btnMarcarLeida_Command"
                            CommandArgument='<%# Eval("idNotificacion") %>'>
                            <i class="fas fa-eye"></i>
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-sm btn-danger"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo" ItemStyle-CssClass="text-center">
                    <ItemTemplate>
                        <i class='<%# GetIconoNotificacion(Eval("tipoNotificacion")) %> fa-2x' title="<%# Eval("tipoNotificacion") %>"></i>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Mensaje" DataField="mensaje" />
                <asp:TemplateField HeaderText="Estado" ItemStyle-CssClass="text-center">
                    <ItemTemplate>
                        <%# (bool)Eval("Leida") 
                            ? "<span class='badge bg-secondary'>Leída</span>" 
                            : "<span class='badge bg-success'>No Leída</span>" %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div class="alert alert-info text-center mt-3">No se encontraron notificaciones que coincidan con los filtros.</div>
            </EmptyDataTemplate>
        </asp:GridView>
        <asp:HiddenField ID="hdnIdMesa" runat="server" />
        <asp:HiddenField ID="hdnIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarNotificacion" runat="server" Style="display: none;" OnClick="btnEliminar_Click" />
    </div>
</asp:Content>
