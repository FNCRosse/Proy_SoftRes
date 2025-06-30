<%@ Page Title="" Language="C#" MasterPageFile="~/SoftResCliente.Master" AutoEventWireup="true" CodeBehind="Comentarios_Listado.aspx.cs" Inherits="SoftResWA.Views.Cliente.Comentarios.Comentarios_Listado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-5">
        <div class="text-center mb-5">
            <h2 class="text-danger fw-bold">Opiniones de Nuestros Clientes</h2>
            <p class="text-muted">La experiencia de nuestros comensales es nuestra mejor carta de presentación.</p>
            <i class="fas fa-bowl-food fa-2x text-warning"></i>

        </div>

        <asp:Repeater ID="rptComentarios" runat="server">
            <ItemTemplate>
                <div class="card shadow-sm mb-4 border-0 rounded-4 p-3 bg-light animate__animated animate__fadeInUp">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <h5 class="fw-semibold text-danger">
                                <%# Eval("usuario.nombreComp") %>
                            </h5>
                            <small class="text-muted"><i class="fas fa-calendar-alt me-1"></i><%# Convert.ToDateTime(Eval("fechaCreacion")).ToString("dd/MM/yyyy") %></small>
                        </div>
                        <div class="text-end">
                            <%# GetStars(Convert.ToInt32(Eval("puntuacion"))) %>
                        </div>
                    </div>
                    <p class="mt-3 text-dark">
                        <i class="fas fa-quote-left text-danger me-2"></i>
                        <%# Eval("mensaje") %>
                    </p>
                </div>
            </ItemTemplate>
            <FooterTemplate>
                <% if (rptComentarios.Items.Count == 0)
                    { %>
                <div class="alert alert-info text-center">Aún no hay comentarios. ¡Sé el primero en compartir tu experiencia!</div>
                <% } %>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
