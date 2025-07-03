<%@ Page Title="Gestión de Reservas" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="reserva_gestion.aspx.cs" Inherits="SoftResWA.Views.Reservas.reserva_gestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Gestión de Reservas
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <!-- Título de la Página -->
    <div class="d-flex justify-content-between align-items-center mb-4">
        <h1 class="h3 text-danger fw-bold mb-0">
            <i class="fas fa-calendar-check me-2"></i>Gestión de Reservas
        </h1>
        <asp:Button ID="btnNuevaReserva" runat="server" Text="🕜 Nueva Reserva"
            CssClass="btn btn-danger"
            OnClientClick="abrirModalTipoReserva(); return false;" />
    </div>

    <!-- Panel de Filtros -->
    <div class="card shadow-sm mb-4">
        <div class="card-header">
            <i class="fas fa-filter me-2"></i>Filtros de Búsqueda
        </div>
        <div class="card-body">
            <div class="row g-3 align-items-end">
                <div class="col-md-3 col-lg-2">
                    <label for="<%= txtDniCliente.ClientID %>" class="form-label">N° Documento</label>
                    <asp:TextBox ID="txtDniCliente" runat="server" CssClass="form-control" placeholder="Ej. 12345678" MaxLength="8" />
                </div>
                <div class="col-md-3 col-lg-2">
                    <label for="<%= ddlLocal.ClientID %>" class="form-label">Local</label>
                    <asp:DropDownList ID="ddlLocal" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="col-md-3 col-lg-2">
                    <label for="<%= ddlEstado.ClientID %>" class="form-label">Estado</label>
                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="col-md-3 col-lg-2">
                    <label for="<%= txtFechaDesde.ClientID %>" class="form-label">Desde</label>
                    <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
                <div class="col-md-3 col-lg-2">
                    <label for="<%= txtFechaHasta.ClientID %>" class="form-label">Hasta</label>
                    <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
                <div class="col-md-3 col-lg-2 d-flex">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary me-2" OnClick="btnBuscar_Click" />
                    <asp:LinkButton ID="btnLimpiar" runat="server" CssClass="btn btn-outline-secondary" OnClick="btnLimpiar_Click">Limpiar</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

    <!-- Tabla de Resultados con UpdatePanel -->
    <asp:UpdatePanel ID="updGrid" runat="server">
        <ContentTemplate>
            <div class="card shadow-sm">
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="gvReservas" runat="server"
                            CssClass="table table-hover" HeaderStyle-CssClass="table-dark"
                            AutoGenerateColumns="False" AllowPaging="True" PageSize="10"
                            OnPageIndexChanging="gvReservas_PageIndexChanging"
                            OnRowCommand="gvReservas_RowCommand" DataKeyNames="idReserva">
                            <Columns>
                                <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="text-nowrap">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnVerDetalle" runat="server" CssClass="btn btn-sm btn-outline-info" ToolTip="Ver Detalle"
                                            CommandName="VerDetalle" CommandArgument='<%# Eval("idReserva") %>'><i class="fas fa-eye"></i></asp:LinkButton>
                                        <asp:LinkButton ID="btnAsignarMesas" runat="server" CssClass="btn btn-sm btn-outline-success ms-1" ToolTip="Asignar Mesas"
                                            CommandName="Asignar" CommandArgument='<%# Eval("idReserva") %>'
                                            Visible='<%# Eval("estado").ToString() == "PENDIENTE" || Eval("estado").ToString() == "CONFIRMADA" %>'><i class="fas fa-chair"></i></asp:LinkButton>
                                        <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-sm btn-outline-danger ms-1" ToolTip="Cancelar Reserva"
                                            CommandName="AbrirCancelar" CommandArgument='<%# Eval("idReserva") %>'
                                            Visible='<%# Eval("estado").ToString() == "PENDIENTE" || Eval("estado").ToString() == "CONFIRMADA" %>'
                                            CausesValidation="false"><i class="fas fa-ban"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="idReserva" HeaderText="ID" SortExpression="idReserva" />
                                <asp:BoundField DataField="usuario.nombreComp" HeaderText="Cliente" SortExpression="usuario.nombreComp" />
                                <asp:BoundField DataField="usuario.numeroDocumento" HeaderText="N° Documento" SortExpression="usuario.numeroDocumento" />
                                <asp:BoundField DataField="local.nombre" HeaderText="Local" SortExpression="local.nombre" />
                                <asp:BoundField DataField="fechaHoraRegistro" HeaderText="Fecha Reserva" SortExpression="fechaHoraRegistro" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                <asp:BoundField DataField="cantidadPersonas" HeaderText="Personas" SortExpression="cantidadPersonas" />
                                <asp:TemplateField HeaderText="Estado" SortExpression="estado">
                                    <ItemTemplate>
                                        <span class='<%# GetEstadoCssClass(Eval("estado")) %>'><%# Eval("estado") %></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="alert alert-warning text-center">No se encontraron reservas con los criterios de búsqueda especificados.</div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- Modal para Cancelar Reserva -->
    <div class="modal fade" id="modalCancelarReserva" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header bg-danger text-white">
                    <h5 class="modal-title"><i class="fas fa-exclamation-triangle me-2"></i>Cancelar Reserva</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p>¿Estás seguro de que deseas cancelar esta reserva? Esta acción no se puede deshacer.</p>
                    <div class="mb-3">
                        <label class="form-label fw-semibold">Motivo de la cancelación <span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlMotivoCancelacion" runat="server" CssClass="form-select" ValidationGroup="CancelarGroup"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvMotivo" runat="server" ControlToValidate="ddlMotivoCancelacion" InitialValue="0"
                            ErrorMessage="Debes seleccionar un motivo." CssClass="text-danger" Display="Dynamic" ValidationGroup="CancelarGroup" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Volver</button>
                    <asp:Button ID="btnConfirmarCancelacion" runat="server" Text="Sí, Cancelar Reserva" CssClass="btn btn-danger"
                        OnCommand="btnConfirmarCancelacion_Command" ValidationGroup="CancelarGroup" />
                </div>
            </div>
        </div>
    </div>
    <!-- =============================================================== -->
    <!-- ==        MODAL PARA SELECCIONAR TIPO DE RESERVA             == -->
    <!-- =============================================================== -->
    <div class="modal fade" id="modalTipoReserva" tabindex="-1" aria-labelledby="modalTipoReservaLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title text-danger fw-bold" id="modalTipoReservaLabel"><i class="fas fa-calendar-plus me-2"></i>Seleccionar Tipo de Reserva</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p class="text-center text-muted mb-4">¿Qué tipo de reserva deseas crear?</p>
                    <div class="row g-4">
                        <!-- Opción Reserva Común -->
                        <div class="col-md-6">
                            <div class="card h-100 text-center card-hover-select" onclick="window.location.href='registrar_reserva_comun.aspx';">
                                <div class="card-body">
                                    <i class="fas fa-utensils fa-3x text-primary mb-3"></i>
                                    <h5 class="card-title fw-bold">Reserva Común</h5>
                                    <p class="card-text small">Para comidas regulares y ocasiones cotidianas.</p>
                                </div>
                            </div>
                        </div>
                        <!-- Opción Reserva de Evento -->
                        <div class="col-md-6">
                            <div class="card h-100 text-center card-hover-select" onclick="window.location.href='registrar_reserva_evento.aspx';">
                                <div class="card-body">
                                    <i class="fas fa-glass-cheers fa-3x text-success mb-3"></i>
                                    <h5 class="card-title fw-bold">Reserva de Evento</h5>
                                    <p class="card-text small">Para celebraciones y grupos grandes.</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- ... tu otro código .aspx ... -->

    <script type="text/javascript">
        // ... tus otras funciones de script (si las tienes) ...

        // Función para abrir el modal de selección de tipo
        function abrirModalTipoReserva() {
            var modal = new bootstrap.Modal(document.getElementById('modalTipoReserva'));
            modal.show();
        }
    </script>

    <style>
        .card-hover-select {
            cursor: pointer;
            transition: all 0.3s ease;
        }

            .card-hover-select:hover {
                transform: translateY(-5px);
                box-shadow: 0 8px 25px rgba(0,0,0,0.1);
                border-color: #BC1F1F !important;
            }
    </style>
    <!-- Script para abrir modales desde el servidor -->
    <script type="text/javascript">
        function abrirModal(modalId) {
            var modal = new bootstrap.Modal(document.getElementById(modalId));
            modal.show();
        }
    </script>
</asp:Content>
