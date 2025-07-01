<%@ Page Title="" Language="C#" MasterPageFile="~/SoftResCliente.Master" AutoEventWireup="true" CodeBehind="Comentarios_Registrar.aspx.cs" Inherits="SoftResWA.Views.Cliente.Comentarios.Comentarios_Registrar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Registrar Comentario
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Estilos CSS para el selector de estrellas interactivo -->
    <style>
        .rating {
            display: inline-block;
            direction: rtl; /* Pone las estrellas de derecha a izquierda */
            text-align: center;
        }

            .rating > input {
                display: none; /* Oculta los radio buttons reales */
            }

            .rating > label {
                color: #ddd; /* Color de estrella vacía */
                font-size: 2.5rem;
                cursor: pointer;
                transition: color 0.2s;
            }

            /* Cambia el color de las estrellas al pasar el ratón por encima */
            .rating > input:not(:checked) ~ label:hover,
            .rating > input:not(:checked) ~ label:hover ~ label {
                color: #f7d106;
            }

            /* Cambia el color de la estrella seleccionada y las anteriores */
            .rating > input:checked ~ label {
                color: #f7d106;
            }
    </style>

    <div class="container my-5 animate__animated animate__fadeIn">
        <div class="text-center mb-4">
            <i class="fas fa-comments fa-3x text-warning mb-3"></i>
            <h2 class="text-danger fw-bold">¡Tu opinión nos importa!</h2>
            <p class="text-muted">Cuéntanos cómo fue tu experiencia y ayúdanos a seguir mejorando.</p>
        </div>

        <div class="card shadow-sm border-0 rounded-4 p-lg-5 p-4 bg-light">
            <asp:UpdatePanel ID="updFormulario" runat="server">
                <ContentTemplate>
                    <!-- Selector de Reserva -->
                    <div class="mb-4">
                        <label for="ddlReservas" class="form-label fw-bold">¿Sobre qué reserva es tu comentario? <span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlReservas" runat="server" CssClass="form-select"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvReservas" runat="server" ControlToValidate="ddlReservas"
                            ErrorMessage="Debes seleccionar una reserva." CssClass="text-danger" Display="Dynamic" InitialValue="0" />
                    </div>

                    <!-- Puntuación (Estrellas Interactivas) -->
                    <div class="mb-4 text-center">
                        <label class="form-label fw-bold d-block">Califica tu experiencia <span class="text-danger">*</span></label>

                        <!-- Contenedor para las estrellas con la dirección invertida -->
                        <div class="rating">
                            <!-- Usaremos un Repeater para generar las estrellas y los inputs -->
                            <asp:Repeater ID="rptPuntuacion" runat="server">
                                <ItemTemplate>
                                    <input type="radio" id="star<%# Eval("Value") %>" name="puntuacion" value="<%# Eval("Value") %>"
                                        onclick="seleccionarPuntuacion(this.value);" />
                                    <label for="star<%# Eval("Value") %>" title="<%# Eval("Title") %>">★</label>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>

                        <!-- HiddenField para guardar el valor seleccionado -->
                        <asp:HiddenField ID="hdnPuntuacionSeleccionada" runat="server" />

                        <!-- Usamos un CustomValidator para validar el HiddenField -->
                        <asp:CustomValidator ID="cvPuntuacion" runat="server"
                            ErrorMessage="Debes seleccionar una puntuación."
                            CssClass="text-danger d-block mt-2"
                            Display="Dynamic"
                            ClientValidationFunction="validarPuntuacion"
                            OnServerValidate="cvPuntuacion_ServerValidate" />
                    </div>

                    <!-- En Comentarios_Registrar.aspx, dentro del ContentPlaceHolder -->
                    <script type="text/javascript">
                        function seleccionarPuntuacion(valor) {
                            // Actualiza el HiddenField
                            document.getElementById('<%= hdnPuntuacionSeleccionada.ClientID %>').value = valor;
                            // Dispara la validación manualmente para que el mensaje de error desaparezca si se selecciona algo
                            if (typeof (Page_ClientValidate) == 'function') {
                                Page_ClientValidate();
                            }
                        }

                        // === NUEVA FUNCIÓN PARA EL CUSTOMVALIDATOR ===
                        function validarPuntuacion(source, args) {
                            // 'source' es el validador, 'args' contiene el valor y el resultado.
                            var hdnPuntuacion = document.getElementById('<%= hdnPuntuacionSeleccionada.ClientID %>');

                            // La validación es simple: el valor no debe estar vacío.
                            args.IsValid = (hdnPuntuacion.value !== '');
                        }
                    </script>

                    <!-- Mensaje del Comentario -->
                    <div class="mb-4">
                        <label for="txtMensaje" class="form-label fw-bold">Tu Comentario <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtMensaje" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" placeholder="Ej: ¡La comida estuvo increíble y el servicio fue muy rápido!"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvMensaje" runat="server" ControlToValidate="txtMensaje"
                            ErrorMessage="Tu comentario no puede estar vacío." CssClass="text-danger" Display="Dynamic" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="text-end">
                <asp:Button ID="btnEnviar" runat="server" Text="Enviar Comentario" CssClass="btn btn-danger px-4 py-2 fw-bold" OnClick="btnEnviar_Click" />
            </div>
        </div>
    </div>
</asp:Content>
