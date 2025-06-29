<%@ Page Title="Gestión de Comentarios" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="comentario_listado.aspx.cs" Inherits="SoftResWA.Views.Comentarios.comentario_listado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Gestión de Comentarios
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="text-start mb-3">
        <h1 class="h3 text-danger fw-bold mb-0 me-3">
            <i class="fas fa-comments me-2"></i>Listado de Comentarios
        </h1>
    </div>

    <!-- Filtros para Comentarios -->
    <div class="container-fluid mb-4 ps-4">
        <div class="row gx-3">
            <!-- Dni de cliente -->
            <div class="col-auto">
                <label for="txtUsuario" class="form-label">DNI Cliente</label>
                <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="Ej. 12345678" />
            </div>
            <!-- Local -->
            <div class="col-auto">
                <label for="ddlLocal" class="form-label">Local</label>
                <asp:DropDownList ID="ddlLocal" runat="server" CssClass="form-select"
                    AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <!-- Puntuacion -->
            <div class="col-auto">
                <label for="ddlPuntuacion" class="form-label">Puntuacion</label>
                <asp:DropDownList ID="ddlPuntuacion" runat="server" CssClass="form-select">
                    <asp:ListItem Text="-- Todos --" Value="" Selected="True" />
                    <asp:ListItem Text="⭐⭐⭐⭐⭐" Value="5" />
                    <asp:ListItem Text="⭐⭐⭐⭐" Value="4" />
                    <asp:ListItem Text="⭐⭐⭐" Value="3" />
                    <asp:ListItem Text="⭐⭐" Value="2" />
                    <asp:ListItem Text="⭐" Value="1" />
                    <asp:ListItem Text="" Value="0" />
                </asp:DropDownList>
            </div>
            <!-- Estado -->
            <div class="col-auto">
                <label for="ddlEstado" class="form-label">Estado</label>
                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                    <asp:ListItem Text="-- Todos --" Value="" Selected="True" />
                    <asp:ListItem Text="Activo" Value="1" />
                    <asp:ListItem Text="Inactivo" Value="0" />
                </asp:DropDownList>
            </div>
            <!-- Botones -->
            <div class="col-auto d-flex align-items-end">
                <div class="col-auto d-flex align-items-end">
                    <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-danger me-2"
                        Text="Buscar"
                        OnClick="btnBuscar_Click" />
                </div>
            </div>
        </div>
        <div class="row">
            <asp:GridView ID="dgvComentarios" runat="server" AllowPaging="false" AutoGenerateColumns="false" CssClass="table table-hover table-responsive table-striped">
                <Columns>
                    <asp:BoundField HeaderText="ID" DataField="idComentario" />
                    <asp:BoundField HeaderText="Comentario" DataField="mensaje" />
                    <asp:BoundField HeaderText="Puntuación" DataField="puntuacion" />
                    <asp:BoundField HeaderText="Cliente" DataField="usuario_nombre" />
                    <asp:BoundField HeaderText="Num Cliente" DataField="usuario_num" />
                    <asp:BoundField HeaderText="Local" DataField="local_nombre" />
                    <asp:BoundField HeaderText="Fecha Creación" DataField="fechaCreacion" />
                    <asp:BoundField HeaderText="Usuario Creación" DataField="usuarioCreacion" />
                    <asp:BoundField HeaderText="Fecha Modificación" DataField="fechaModificacion" />
                    <asp:BoundField HeaderText="Usuario Modificación" DataField="usuarioModificacion" />
                    <asp:BoundField HeaderText="Estado" DataField="estado" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
