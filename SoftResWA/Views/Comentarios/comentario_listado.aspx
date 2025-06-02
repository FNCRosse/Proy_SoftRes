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
                <input type="text" id="txtUsuario" class="form-control" placeholder="Ej. 12345678" />
            </div>
            <!-- Local -->
            <div class="col-auto">
                <label for="ddlLocal" class="form-label">Local</label>
                <select id="ddlLocal" class="form-select">
                    <option selected disabled>Seleccionar...</option>
                    <option value="1">Local San Miguel</option>
                    <option value="2">Local Callao</option>
                </select>
            </div>
            <!-- Puntuacion -->
            <div class="col-auto">
                <label for="ddlPuntuacion" class="form-label">Puntuacion</label>
                <select id="ddlPuntuacion" class="form-select">
                    <option selected disabled>Seleccionar...</option>
                    <option value="1">⭐⭐⭐⭐⭐</option>
                    <option value="2">⭐⭐⭐⭐</option>
                    <option value="2">⭐⭐⭐</option>
                    <option value="2">⭐⭐</option>
                    <option value="2">⭐</option>
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
            </div>
        </div>
    </div>
    <div class="row">
        <asp:GridView ID="dgvComentarios" runat="server" AllowPaging="false" AutoGenerateColumns="false" CssClass="table table-hover table-responsive table-striped">
            <Columns>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <button type="button" class="btn btn-sm btn-primary" onclick="modificarReserva('<%# Eval("IdComentario") %>')">M</button>
                        <button type="button" class="btn btn-sm btn-danger" onclick="confirmarCancelacion('<%# Eval("IdComentario) %>')">C</button>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="ID" DataField="COMENTARIO_ID" />
                <asp:BoundField HeaderText="Comentario" DataField="MENSAJE" />
                <asp:BoundField HeaderText="Puntuación" DataField="PUNTUACION" />
                <asp:BoundField HeaderText="Cliente" DataField="USUARIO_NOMBRE" />
                <asp:BoundField HeaderText="Local" DataField="LOCAL_NOMBRE" />
                <asp:BoundField HeaderText="Fecha Creación" DataField="FECHA_CREACION" />
                <asp:BoundField HeaderText="Usuario Creación" DataField="USUARIO_CREACION" />
                <asp:BoundField HeaderText="Fecha Modificación" DataField="FECHA_MODIFICACION" />
                <asp:BoundField HeaderText="Usuario Modificación" DataField="USUARIO_MODIFICACION" />
                <asp:BoundField HeaderText="Estado" DataField="ESTADO" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
