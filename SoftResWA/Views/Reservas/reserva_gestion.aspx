<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="reserva_gestion.aspx.cs" Inherits="SoftResWA.Views.Reservas.reserva_gestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Gestión de Reservas
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <div class="container-fluid py-4">
        <h2 class="text-center fw-bold text-danger mb-4">Gestión de Reservas</h2>

        <!-- Pestañas de navegación -->
        <ul class="nav nav-tabs mb-4" id="gestionTabs" role="tablist">
            <li class="nav-item" role="presentation">
                <button class="nav-link active" id="reservas-tab" data-bs-toggle="tab" data-bs-target="#reservas" type="button" role="tab">
                    <i class="fas fa-calendar-check me-2"></i>Reservas
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link" id="espera-tab" data-bs-toggle="tab" data-bs-target="#espera" type="button" role="tab">
                    <i class="fas fa-clock me-2"></i>Lista de Espera
                    <asp:Label ID="lblEsperaCount" runat="server" CssClass="badge bg-danger ms-2">0</asp:Label>
                </button>
            </li>
        </ul>

        <div class="tab-content" id="gestionTabsContent">
            <!-- Panel de Reservas -->
            <div class="tab-pane fade show active" id="reservas" role="tabpanel">
                <!-- Filtros de Reservas -->
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-3">
                                <label for="ddlLocal" class="form-label">Local</label>
                                <asp:DropDownList ID="ddlLocal" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Todos" Value="" />
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label for="txtFechaDesde" class="form-label">Desde</label>
                                <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control" TextMode="Date" />
                            </div>
                            <div class="col-md-3">
                                <label for="txtFechaHasta" class="form-label">Hasta</label>
                                <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control" TextMode="Date" />
                            </div>
                            <div class="col-md-3">
                                <label for="ddlEstado" class="form-label">Estado</label>
                                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Todos" Value="" />
                                    <asp:ListItem Text="Pendiente" Value="PENDIENTE" />
                                    <asp:ListItem Text="Confirmada" Value="CONFIRMADA" />
                                    <asp:ListItem Text="Cancelada" Value="CANCELADA" />
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="text-end mt-3">
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-danger" OnClick="btnBuscar_Click" />
                        </div>
                    </div>
                </div>

                <!-- GridView de Reservas -->
                <asp:GridView ID="gvReservas" runat="server" CssClass="table table-striped table-hover"
                    AutoGenerateColumns="False" DataKeyNames="idReserva" OnRowDataBound="gv_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-Width="120px">
                            <ItemTemplate>
                                <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btn btn-sm btn-primary" 
                                    CommandName="Editar" CommandArgument='<%# Eval("idReserva") %>' />
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-sm btn-danger" 
                                    CommandName="Cancelar" CommandArgument='<%# Eval("idReserva") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="ID" DataField="idReserva" HeaderStyle-Width="60px" />
                        <asp:BoundField HeaderText="Fecha" DataField="fecha_Hora" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField HeaderText="Hora" DataField="fecha_Hora" DataFormatString="{0:HH:mm}" />
                        <asp:BoundField HeaderText="Local" DataField="local.nombre" />
                        <asp:BoundField HeaderText="Personas" DataField="cantidad_personas" />
                        <asp:BoundField HeaderText="Solicitante" DataField="usuario.nombreComp" />
                        <asp:BoundField HeaderText="Estado" DataField="estado" />
                        <asp:BoundField HeaderText="Observaciones" DataField="observaciones" />
                    </Columns>
                    <HeaderStyle CssClass="table-dark" />
                    <EmptyDataTemplate>
                        <div class="text-center p-4">
                            <i class="fas fa-calendar-times fa-3x text-muted mb-3"></i>
                            <p class="lead">No hay reservas que mostrar</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>

            <!-- Panel de Lista de Espera -->
            <div class="tab-pane fade" id="espera" role="tabpanel">
                <div class="card mb-4">
                    <div class="card-body">
                        <h5 class="card-title">Lista de Espera</h5>
                        <asp:UpdatePanel ID="upListaEspera" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvListaEspera" runat="server" CssClass="table table-striped table-hover"
                                    AutoGenerateColumns="False" DataKeyNames="idFila" OnRowCommand="gvListaEspera_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="fecha_Hora" HeaderText="Fecha y Hora" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                        <asp:BoundField DataField="local.nombre" HeaderText="Local" />
                                        <asp:BoundField DataField="usuario.nombreComp" HeaderText="Cliente" />
                                        <asp:BoundField DataField="cantidad_personas" HeaderText="Personas" />
                                        <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha Solicitud" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                        <asp:TemplateField HeaderText="Acciones">
                                            <ItemTemplate>
                                                <asp:Button ID="btnCrearReserva" runat="server" Text="Crear Reserva" 
                                                    CssClass="btn btn-sm btn-primary"
                                                    CommandName="CrearReserva" 
                                                    CommandArgument='<%# Eval("idFila") %>' />
                                                <asp:Button ID="btnEliminarEspera" runat="server" Text="Eliminar" 
                                                    CssClass="btn btn-sm btn-danger"
                                                    CommandName="EliminarEspera" 
                                                    CommandArgument='<%# Eval("idFila") %>'
                                                    OnClientClick="return confirm('¿Está seguro de eliminar este registro de la lista de espera?');" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div class="text-center p-4">
                                            <i class="fas fa-clipboard-list fa-3x text-muted mb-3"></i>
                                            <p class="lead">No hay clientes en lista de espera</p>
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal para cancelar reserva -->
    <div class="modal fade" id="modalCancelarReserva" tabindex="-1" aria-labelledby="modalCancelarReservaLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content border-danger">
                <div class="modal-header bg-danger-subtle">
                    <h5 class="modal-title fw-bold text-danger" id="modalCancelarReservaLabel">
                        <i class="fas fa-exclamation-triangle me-2"></i>Cancelar Reserva
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <div class="alert alert-warning" role="alert">
                        <i class="fas fa-info-circle me-2"></i>
                        <strong>¿Está seguro que desea cancelar esta reserva?</strong>
                        <br>Esta acción no se puede deshacer.
                    </div>
                    
                    <div class="mb-3">
                        <label for="ddlMotivoCancelacion" class="form-label">Motivo de cancelación <span class="text-danger">*</span></label>
                        <select id="ddlMotivoCancelacion" class="form-select" required>
                            <option value="">Seleccione un motivo...</option>
                            <option value="1">Solicitud del cliente</option>
                            <option value="2">No disponibilidad</option>
                            <option value="3">Problema operativo</option>
                            <option value="4">Otros</option>
                        </select>
                        <div class="invalid-feedback">
                            Por favor seleccione un motivo de cancelación.
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                        <i class="fas fa-times me-1"></i>Cancelar
                    </button>
                    <button type="button" class="btn btn-danger" onclick="ejecutarCancelacion()">
                        <i class="fas fa-ban me-1"></i>Confirmar Cancelación
                    </button>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal para seleccionar tipo de reserva -->
    <div class="modal fade" id="modalTipoReserva" tabindex="-1" aria-labelledby="modalTipoReservaLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content border-warning">
                <div class="modal-header bg-warning-subtle">
                    <h5 class="modal-title fw-bold text-warning" id="modalTipoReservaLabel">
                        <i class="fas fa-calendar-plus me-2"></i>Seleccionar Tipo de Reserva
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body text-center">
                    <p class="mb-4">¿Qué tipo de reserva desea crear?</p>
                    <div class="row g-3">
                        <div class="col-md-6">
                            <div class="card h-100 border-primary">
                                <div class="card-body text-center">
                                    <i class="fas fa-utensils fa-3x text-primary mb-3"></i>
                                    <h5 class="card-title">Reserva Común</h5>
                                    <p class="card-text">Para comidas regulares y ocasiones cotidianas</p>
                                    <button type="button" class="btn btn-primary" onclick="window.location.href='registrar_reserva_comun.aspx'">
                                        Seleccionar
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="card h-100 border-success">
                                <div class="card-body text-center">
                                    <i class="fas fa-glass-cheers fa-3x text-success mb-3"></i>
                                    <h5 class="card-title">Reserva de Evento</h5>
                                    <p class="card-text">Para celebraciones y eventos especiales</p>
                                    <button type="button" class="btn btn-success" onclick="window.location.href='registrar_reserva_evento.aspx'">
                                        Seleccionar
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Hidden fields para JavaScript -->
    <asp:HiddenField ID="hdnIdReservaCancelar" runat="server" />
    <asp:HiddenField ID="hdnIdMotivoCancelacion" runat="server" />
    <asp:Button ID="btnCancelarReservaHidden" runat="server" Style="display: none;" OnClick="btnCancelarReserva_Click" />

    <script>
        // Variable global para almacenar el ID de la reserva a cancelar
        let reservaACancelar = null;

        // Función para confirmar cancelación
        function confirmarCancelacion(idReserva) {
            reservaACancelar = idReserva;
            const modal = new bootstrap.Modal(document.getElementById('modalCancelarReserva'));
            modal.show();
            return false;
        }

        // Función para ejecutar la cancelación
        function ejecutarCancelacion() {
            const motivoSelect = document.getElementById('ddlMotivoCancelacion');
            const motivo = motivoSelect.value;
            
            if (!motivo) {
                motivoSelect.classList.add('is-invalid');
                Swal.fire('Error', 'Debe seleccionar un motivo de cancelación', 'warning');
                return;
            }
            
            motivoSelect.classList.remove('is-invalid');
            
            // Establecer valores en campos ocultos
            document.getElementById('<%= hdnIdReservaCancelar.ClientID %>').value = reservaACancelar;
            document.getElementById('<%= hdnIdMotivoCancelacion.ClientID %>').value = motivo;
            
            // Cerrar modal
            const modal = bootstrap.Modal.getInstance(document.getElementById('modalCancelarReserva'));
            modal.hide();
            
            // Ejecutar postback
            document.getElementById('<%= btnCancelarReservaHidden.ClientID %>').click();
        }

        // Función para mostrar modal de tipo de reserva
        function mostrarTipoReserva() {
            const modal = new bootstrap.Modal(document.getElementById('modalTipoReserva'));
            modal.show();
        }

        // Validación de campos de fecha
        document.addEventListener('DOMContentLoaded', function() {
            const fechaDesde = document.getElementById('<%= txtFechaDesde.ClientID %>');
            const fechaHasta = document.getElementById('<%= txtFechaHasta.ClientID %>');
            
            // Validar que fecha hasta no sea menor que fecha desde
            if (fechaHasta) {
                fechaHasta.addEventListener('change', function() {
                    if (fechaDesde.value && fechaHasta.value) {
                        if (new Date(fechaHasta.value) < new Date(fechaDesde.value)) {
                            Swal.fire('Error', 'La fecha hasta no puede ser menor que la fecha desde', 'warning');
                            fechaHasta.value = '';
                        }
                    }
                });
            }
            
            // Validar que fecha desde no sea mayor que fecha hasta
            if (fechaDesde) {
                fechaDesde.addEventListener('change', function() {
                    if (fechaDesde.value && fechaHasta.value) {
                        if (new Date(fechaDesde.value) > new Date(fechaHasta.value)) {
                            Swal.fire('Error', 'La fecha desde no puede ser mayor que la fecha hasta', 'warning');
                            fechaDesde.value = '';
                        }
                    }
                });
            }
            
            // Cambiar evento del botón "Nuevo"
            const btnNuevo = document.getElementById('<%= btnNuevo.ClientID %>');
            if (btnNuevo) {
                btnNuevo.setAttribute('onclick', 'mostrarTipoReserva(); return false;');
            }
        });
    </script>

    <style>
        .text-success { color: #28a745 !important; }
        .text-warning { color: #ffc107 !important; }
        .text-danger { color: #dc3545 !important; }
        .table td { vertical-align: middle; }
        .btn-sm { padding: 0.25rem 0.5rem; font-size: 0.875rem; }
        .btn-sm + .btn-sm { margin-left: 0.5rem; }
        .table th {
            border-top: none;
            font-weight: 600;
            font-size: 0.9rem;
        }
        
        .table td {
            vertical-align: middle;
            font-size: 0.9rem;
        }
        
        .card:hover {
            transform: translateY(-2px);
            transition: transform 0.2s;
        }
        
        .is-invalid {
            border-color: #dc3545;
        }
        
        .invalid-feedback {
            display: block;
        }

        /* Estilos para las pestañas */
        .nav-tabs .nav-link {
            border: 1px solid transparent;
            border-top-left-radius: 0.375rem;
            border-top-right-radius: 0.375rem;
            transition: all 0.3s ease;
        }

        .nav-tabs .nav-link:hover {
            border-color: #e9ecef #e9ecef #dee2e6;
            isolation: isolate;
            background-color: #f8f9fa;
        }

        .nav-tabs .nav-link.active {
            color: #495057;
            background-color: #fff;
            border-color: #dee2e6 #dee2e6 #fff;
            font-weight: 600;
        }

        /* Alertas personalizadas */
        .alert-info {
            border-left: 4px solid #17a2b8;
        }
    </style>
</asp:Content>
