<%@ Page Title="" Language="C#" MasterPageFile="~/SoftResCliente.Master" AutoEventWireup="true" CodeBehind="Comentarios_Registrar.aspx.cs" Inherits="SoftResWA.Views.Cliente.Comentarios.Comentarios_Registrar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Registrar Comentario
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container my-5 animate__animated animate__fadeIn">
        <div class="text-center mb-4">
            <h2 class="text-danger fw-bold">¡Tu opinión nos importa!</h2>
            <p class="text-muted">Cuéntanos cómo fue tu experiencia y ayúdanos a seguir mejorando.</p>
            <i class="fas fa-comments fa-3x text-warning mb-3"></i>
        </div>

        <div class="card shadow-sm border-0 rounded-4 p-4 bg-light">

            <div class="mb-3">
                <label for="txtMensaje" class="form-label fw-bold">Tu Comentario</label>
                <asp:TextBox ID="txtMensaje" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" placeholder="Cuéntanos tu experiencia..." />
            </div>

            <div class="mb-4">
                <label class="form-label fw-bold">Puntuación</label>
                <div class="d-flex gap-2">
                    <asp:RadioButtonList ID="rblPuntuacion" runat="server" RepeatDirection="Horizontal" CssClass="form-check form-check-inline">
                        <asp:ListItem Value="1">&#9733;</asp:ListItem>
                        <asp:ListItem Value="2">&#9733;&#9733;</asp:ListItem>
                        <asp:ListItem Value="3">&#9733;&#9733;&#9733;</asp:ListItem>
                        <asp:ListItem Value="4">&#9733;&#9733;&#9733;&#9733;</asp:ListItem>
                        <asp:ListItem Value="5">&#9733;&#9733;&#9733;&#9733;&#9733;</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>

            <div class="text-end">
                <asp:Button ID="btnEnviar" runat="server" Text="Enviar Comentario" CssClass="btn btn-danger px-4 fw-bold" OnClick="btnEnviar_Click" />
            </div>
        </div>
    </div>
</asp:Content>
