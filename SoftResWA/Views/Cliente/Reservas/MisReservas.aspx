<%@ Page Title="" Language="C#" MasterPageFile="~/SoftResCliente.Master" AutoEventWireup="true" CodeBehind="MisReservas.aspx.cs" Inherits="SoftResWA.Views.Cliente.Reservas.MisReservas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Mis Reservas
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-4">
        <h2 class="text-center fw-bold text-danger mb-4">Mis Reservas</h2>
        <div class="row">
            <!-- Columna de Filtros -->
            <div class="col-lg-3">
                <div class="border-end pe-3" id="filtros-sidebar">
                    <h5 class="text-danger fw-bold">Filtros</h5>
                    <div class="mb-3">
                        <label for="txtFechaDesde" class="form-label filtro-label">Desde</label>
                        <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control form-control-custom" TextMode="Date" />
                    </div>
                    <div class="mb-3">
                        <label for="txtFechaHasta" class="form-label filtro-label">Hasta</label>
                        <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control form-control-custom" TextMode="Date" />
                    </div>
                    <div class="mb-3">
                        <label for="ddlLocal" class="form-label filtro-label">Local</label>
                        <asp:DropDownList ID="ddlLocal" runat="server" CssClass="form-select form-control-custom"></asp:DropDownList>
                    </div>
                    <div class="mb-3">
                        <label for="ddlEstado" class="form-label filtro-label">Estado</label>
                        <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select form-control-custom"></asp:DropDownList>
                    </div>
                    <div class="d-grid gap-2">
                        <asp:Button ID="btnBuscar" runat="server" Text="Aplicar Filtros"
                            CssClass="btn btn-danger" OnClick="btnBuscar_Click"
                            ValidationGroup="FiltrosGroup" />
                        <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn btn-outline-secondary" OnClick="btnLimpiar_Click">Limpiar</asp:LinkButton>
                    </div>
                </div>
            </div>

            <!-- Columna de Lista de Reservas -->
            <div class="col-lg-9">
                <asp:UpdatePanel ID="updReservas" runat="server">
                    <ContentTemplate>
                        <div class="d-flex flex-column gap-4">
                            <asp:Repeater ID="rptReservas" runat="server" OnItemCommand="rptReservas_ItemCommand">
                                <ItemTemplate>
                                    <div class="card-reserva p-4">
                                        <div class="d-flex justify-content-between align-items-center mb-3">
                                            <div>
                                                <span class="fw-bold text-danger">Fecha:</span> <%# Eval("fecha_Hora", "{0:dd 'de' MMMM, yyyy}") %>
                                                <span class="fw-bold text-danger ms-4">Hora:</span> <%# Eval("fecha_Hora", "{0:hh:mm tt}") %>
                                                <span class="fw-bold text-danger ms-4">Local:</span> <%# Eval("local.nombre") %>
                                                <span class="fw-bold text-danger ms-4">Estado:</span> <span class='<%# GetEstadoCssClass(Eval("estado")) %>'><%# Eval("estado") %></span>
                                            </div>
                                            <!-- Icono de advertencia si la reserva está próxima y no se puede modificar -->
                                            <asp:Panel ID="pnlAdvertencia" runat="server" Visible='<%# !PuedeModificarCancelar(Eval("fecha_Hora")) %>'>
                                                <i class="fas fa-exclamation-triangle text-warning" title="No se puede modificar/cancelar con menos de 1 hora de anticipación."></i>
                                            </asp:Panel>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <p class="fw-bold text-danger mb-1">Cantidad de Personas:</p>
                                                <p><%# Eval("cantidad_personas") %></p>
                                                <p class="fw-bold text-danger mb-1">Cantidad de Mesas:</p>
                                                <p><%# Eval("numeroMesas") %></p>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Panel ID="pnlEvento" runat="server" Visible='<%# Eval("tipoReserva").ToString() == "EVENTO" %>'>
                                                    <p class="fw-bold text-danger mb-1">Nombre del evento:</p>
                                                    <p><%# Eval("nombreEvento") %></p>
                                                </asp:Panel>
                                                <p class="fw-bold text-danger mt-2 mb-1">Observaciones</p>
                                                <div class="bg-light rounded p-2"><%# Eval("observaciones") %></div>
                                            </div>
                                        </div>
                                        <!-- Botones de Acción -->
                                        <div class="text-end mt-3">
                                            <!-- Botón EDITAR: Visible solo si está CONFIRMADA y se puede modificar -->
                                            <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-danger px-4"
                                                Text="Editar" CommandName="Editar" CommandArgument='<%# Eval("idReserva") %>'
                                                Visible='<%# Eval("estado").ToString() == "CONFIRMADA" && PuedeModificarCancelar(Eval("fecha_Hora")) %>' CausesValidation="false" />

                                            <!-- Botón CONFIRMAR: Visible solo si está PENDIENTE y se puede modificar -->
                                            <asp:LinkButton ID="btnConfirmar" runat="server" CssClass="btn btn-warning px-4 text-dark"
                                                Text="Confirmar" CommandName="Confirmar" CommandArgument='<%# Eval("idReserva") %>'
                                                Visible='<%# Eval("estado").ToString() == "PENDIENTE" && PuedeModificarCancelar(Eval("fecha_Hora")) %>' CausesValidation="false" />

                                            <!-- Botón CANCELAR: Visible solo si está PENDIENTE y se puede modificar -->
                                            <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-outline-danger px-4"
                                                Text="Cancelar" CommandName="AbrirCancelar" CommandArgument='<%# Eval("idReserva") %>'
                                                Visible='<%# Eval("estado").ToString() == "PENDIENTE" && PuedeModificarCancelar(Eval("fecha_Hora")) %>' CausesValidation="false" />
                                        </div>
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <% if (rptReservas.Items.Count == 0)
                                        { %>
                                    <div class="alert alert-info text-center">No tienes reservas que coincidan con los filtros seleccionados.</div>
                                    <% } %>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!-- ==================================== -->
    <!-- ==      MODAL PARA EDITAR         == -->
    <!-- ==================================== -->
    <!-- ==================================== -->
    <!-- ==      MODAL PARA EDITAR         == -->
    <!-- ==================================== -->
    <div class="modal fade" id="modalEditarReserva" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title text-danger fw-bold">Modificar Reserva</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="updModalEditar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnReservaIdEditar" runat="server" />
                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label fw-semibold">Cantidad de Personas</label>
                                    <asp:TextBox ID="txtPersonasModal" runat="server" CssClass="form-control" TextMode="Number" min="1"></asp:TextBox>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label fw-semibold">Cantidad de Mesas</label>
                                    <asp:TextBox ID="txtMesasModal" runat="server" CssClass="form-control" TextMode="Number" min="1"></asp:TextBox>
                                </div>
                            </div>

                            <!-- =============================================== -->
                            <!-- ==     SECCIÓN DINÁMICA PARA EVENTOS         == -->
                            <!-- =============================================== -->
                            <asp:Panel ID="pnlCamposEvento" runat="server" Visible="false">
                                <div class="mb-3">
                                    <label class="form-label fw-semibold">Nombre del Evento</label>
                                    <asp:TextBox ID="txtNombreEventoModal" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label fw-semibold">Descripción del Evento</label>
                                    <asp:TextBox ID="txtDescripcionEventoModal" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                </div>
                            </asp:Panel>
                            <div class="mb-3">
                                <label class="form-label fw-semibold">Observaciones Adicionales</label>
                                <asp:TextBox ID="txtObservacionesModal" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnCancelarEnModal" runat="server" Text="Cancelar Reserva" CssClass="btn btn-outline-danger me-auto" OnCommand="btnAccionReserva_Command" CommandName="AbrirCancelarDesdeModal" CausesValidation="false" />
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                    <asp:Button ID="btnGuardarCambios" runat="server" Text="Guardar Cambios" CssClass="btn btn-danger" OnCommand="btnAccionReserva_Command" CommandName="Guardar" />
                </div>
            </div>
        </div>
    </div>

    <!-- ==================================== -->
    <!-- ==     MODAL PARA CANCELAR        == -->
    <!-- ==================================== -->
    <div class="modal fade" id="modalCancelarReserva" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title text-danger fw-bold">Cancelar Reserva</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hdnReservaIdCancelar" runat="server" />
                    <p>¿Estás seguro de que deseas cancelar esta reserva? Esta acción no se puede deshacer.</p>
                    <div class="mb-3">
                        <label class="form-label">Motivo de la cancelación <span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlMotivoCancelacion" runat="server"
                            CssClass="form-select" ValidationGroup="CancelarGroup">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvMotivo" runat="server"
                            ControlToValidate="ddlMotivoCancelacion" InitialValue="0"
                            ErrorMessage="Debes seleccionar un motivo."
                            CssClass="text-danger" Display="Dynamic"
                            ValidationGroup="CancelarGroup" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">No, volver</button>
                    <asp:Button ID="btnConfirmarCancelacion" runat="server"
                        Text="Sí, Cancelar Reserva" CssClass="btn btn-danger"
                        OnCommand="btnAccionReserva_Command" CommandName="ConfirmarCancelar"
                        ValidationGroup="CancelarGroup" />
                </div>
            </div>
        </div>
    </div>

    <!-- Script para abrir modales desde el servidor -->
    <script type="text/javascript">
        function abrirModal(modalId) {
            var modal = new bootstrap.Modal(document.getElementById(modalId));
            modal.show();
        }
    </script>
</asp:Content>
