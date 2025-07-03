<%@ Page Title="" Language="C#" MasterPageFile="~/SoftResCliente.Master" AutoEventWireup="true" CodeBehind="Reg_Resev_Evento.aspx.cs" Inherits="SoftResWA.Views.Cliente.Reservas.Reg_Resev_Evento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Registrar Reseva Evento
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="reserva-form-section py-5">
        <div class="container">
            <div class="form-wrapper mx-auto">
                <h2 class="text-center text-danger fw-bold">Reserva para tu Evento Especial</h2>
                <p class="text-center subtitulo mb-4">Celebra tus momentos importantes con el mejor sabor.</p>

                <asp:UpdatePanel ID="updFormulario" runat="server">
                    <ContentTemplate>
                        <div class="row g-4 justify-content-center">

                            <!-- Detalles del Evento -->
                            <div class="col-md-10">
                                <label class="form-label campo-label">Nombre del evento<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtNombreEvento" runat="server" CssClass="form-control custom-input" placeholder="Ej: Aniversario, Cumpleaños de Sofía..." />
                                <asp:RequiredFieldValidator ID="rfvNombreEvento" runat="server" ControlToValidate="txtNombreEvento"
                                    ErrorMessage="El nombre del evento es requerido." CssClass="text-danger small" Display="Dynamic" ValidationGroup="ReservaEventoCliente" />
                            </div>
                            <div class="col-md-10">
                                <label class="form-label campo-label">Descripción del evento (Opcional)</label>
                                <asp:TextBox ID="txtDescripcionEvento" runat="server" CssClass="form-control custom-input" TextMode="MultiLine" Rows="2"
                                    placeholder="Detalles sobre la celebración..."></asp:TextBox>
                            </div>

                            <!-- Fecha y Hora -->
                            <div class="col-md-5">
                                <label class="form-label campo-label">Seleccione una fecha<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control custom-input" TextMode="Date" />
                                <asp:RequiredFieldValidator ID="rfvFecha" runat="server" ControlToValidate="txtFecha"
                                    ErrorMessage="La fecha es requerida." CssClass="text-danger small" Display="Dynamic" ValidationGroup="ReservaEventoCliente" />
                            </div>
                            <div class="col-md-5">
                                <label class="form-label campo-label">Seleccione una hora<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtHora" runat="server" CssClass="form-control custom-input" TextMode="Time" />
                                <asp:RequiredFieldValidator ID="rfvHora" runat="server" ControlToValidate="txtHora"
                                    ErrorMessage="La hora es requerida." CssClass="text-danger small" Display="Dynamic" ValidationGroup="ReservaEventoCliente" />
                            </div>

                            <!-- Personas y Mesas -->
                            <div class="col-md-5">
                                <label class="form-label campo-label">Cantidad de personas<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtCantidadPersonas" runat="server" CssClass="form-control custom-input" TextMode="Number" min="1" />
                                <asp:RequiredFieldValidator ID="rfvCantidadPersonas" runat="server" ControlToValidate="txtCantidadPersonas"
                                    ErrorMessage="Indique la cantidad de personas." CssClass="text-danger small" Display="Dynamic" ValidationGroup="ReservaEventoCliente" />
                            </div>
                            <div class="col-md-5">
                                <label class="form-label campo-label">Cantidad de mesas<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtCantidadMesas" runat="server" CssClass="form-control custom-input" TextMode="Number" min="1" />
                                <asp:RequiredFieldValidator ID="rfvCantidadMesas" runat="server" ControlToValidate="txtCantidadMesas"
                                    ErrorMessage="Indique la cantidad de mesas." CssClass="text-danger small" Display="Dynamic" ValidationGroup="ReservaEventoCliente" />
                            </div>

                            <!-- Local y Tipo de Mesa -->
                            <div class="col-md-5">
                                <label class="form-label campo-label">Seleccione un local<span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlLocal" runat="server" CssClass="form-select custom-input" />
                                <asp:RequiredFieldValidator ID="rfvLocal" runat="server" ControlToValidate="ddlLocal"
                                    ErrorMessage="Debe seleccionar un local." CssClass="text-danger small" Display="Dynamic" InitialValue="0" ValidationGroup="ReservaEventoCliente" />
                            </div>
                            <div class="col-md-5">
                                <label class="form-label campo-label">Preferencia de mesa<span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlTipoMesa" runat="server" CssClass="form-select custom-input" />
                                <asp:RequiredFieldValidator ID="rfvTipoMesa" runat="server" ControlToValidate="ddlTipoMesa"
                                    ErrorMessage="Debe seleccionar un tipo de mesa." CssClass="text-danger small" Display="Dynamic" InitialValue="0" ValidationGroup="ReservaEventoCliente" />
                            </div>

                            <!-- Observaciones -->
                            <div class="col-md-10">
                                <label class="form-label campo-label">Observaciones (Opcional)</label>
                                <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control custom-input" TextMode="MultiLine" Rows="3"
                                    placeholder="Escriba requerimientos adicionales aquí..."></asp:TextBox>
                            </div>

                            <!-- Botón para Registrar -->
                            <div class="col-12 text-center mt-3">
                                <asp:Button ID="btnEnviarReserva" runat="server" Text="Enviar Solicitud de Evento"
                                    CssClass="btn btn-danger rounded-pill px-5 py-2 fw-bold"
                                    Style="background-color: #BC1F1F; border: none;"
                                    OnClick="btnEnviarReserva_Click" ValidationGroup="ReservaEventoCliente" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </section>

    <!-- Modal Lista de Espera (reutilizado) -->
    <div class="modal fade" id="modalListaEspera" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content border-0 rounded-4">
                <div class="modal-header bg-warning rounded-top-4">
                    <h5 class="modal-title fw-bold text-dark" id="modalLabel">¡Mesas Llenas!</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body text-center">
                    <i class="fas fa-clock fa-3x text-danger mb-3"></i>
                    <p class="mb-2">No hay mesas disponibles para tu selección.</p>
                    <p class="text-muted">¿Deseas unirte a nuestra lista de espera? Te notificaremos si se libera un espacio.</p>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-outline-secondary px-4" data-bs-dismiss="modal">No, gracias</button>
                    <!-- Este botón disparará la acción de unirse a la lista de espera -->
                    <asp:Button ID="btnUnirseEspera" runat="server" Text="Sí, unirme a la lista" CssClass="btn btn-danger px-4" OnClick="btnUnirseEspera_Click" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
    function abrirModal(modalId) {
        var modal = new bootstrap.Modal(document.getElementById(modalId));
        modal.show();
    }
    </script>
</asp:Content>
