<%@ Page Title="" Language="C#" MasterPageFile="~/SoftResCliente.Master" AutoEventWireup="true" CodeBehind="Comentarios_Listado.aspx.cs" Inherits="SoftResWA.Views.Cliente.Comentarios.Comentarios_Listado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Comentarios
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Estilos para la animación del botón -->
    <style>
        .btn-comentar-flotante {
            transition: all 0.3s ease-in-out;
            box-shadow: 0 4px 15px rgba(220, 53, 69, 0.4);
        }
        .btn-comentar-flotante:hover {
            transform: translateY(-3px);
            box-shadow: 0 6px 20px rgba(220, 53, 69, 0.5);
        }
    </style>

    <div class="container py-5">
        <!-- ==              CABECERA CON EL BOTÓN NUEVO                  == -->
        <div class="d-flex justify-content-between align-items-center mb-5">
            <div class="text-start">
                <h2 class="text-danger fw-bold mb-0">Opiniones de Nuestros Clientes</h2>
                <p class="text-muted mb-0">La experiencia que nos hace mejorar.</p>
            </div>
            <div>
                <asp:Button ID="btnAgregarComentario" runat="server" 
                    Text="✍️ Escribir una opinión" 
                    CssClass="btn btn-danger btn-lg rounded-pill px-4 py-2 fw-bold btn-comentar-flotante" 
                    OnClick="btnAgregarComentario_Click" />
            </div>
        </div>

        <!-- Repeater para mostrar los comentarios (tu código existente) -->
        <asp:Repeater ID="rptComentarios" runat="server">
            <ItemTemplate>
                <div class="card shadow-sm mb-4 border-0 rounded-4 p-3 bg-light animate__animated animate__fadeInUp">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <h5 class="fw-semibold text-danger">
                                <%# Eval("usuario.nombreComp") %>
                            </h5>
                            <small class="text-muted"><i class="fas fa-calendar-alt me-1"></i><%# Eval("fechaCreacion", "{0:dd 'de' MMMM, yyyy}") %></small>
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
                <% if (rptComentarios.Items.Count == 0) { %>
                    <div class="alert alert-info text-center">
                        <h4 class="alert-heading">¡Aún no hay opiniones!</h4>
                        <p>Tu experiencia es muy importante para nosotros y para otros comensales.</p>
                        <hr>
                        <p class="mb-0">¡Anímate y sé el primero en compartir tu comentario haciendo clic en el botón de arriba!</p>
                    </div>
                <% } %>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</asp:Content>