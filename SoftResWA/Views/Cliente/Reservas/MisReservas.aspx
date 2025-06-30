<%@ Page Title="" Language="C#" MasterPageFile="~/SoftResCliente.Master" AutoEventWireup="true" CodeBehind="MisReservas.aspx.cs" Inherits="SoftResWA.Views.Cliente.Reservas.MisReservas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Mis Reservas
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    
    <div class="container py-4">
        <h2 class="text-center fw-bold text-danger mb-4">
            <i class="fas fa-calendar-alt me-2"></i>Mis reservas
        </h2>
        
        <div class="row">
            <!-- Panel de Filtros -->
            <div class="col-md-3">
                <div class="card border-danger">
                    <div class="card-header bg-danger text-white">
                        <h5 class="mb-0">
                            <i class="fas fa-filter me-2"></i>Filtros
                        </h5>
                    </div>
                    <div class="card-body">
                        <div class="mb-3">
                            <label for="txtFechaDesde" class="form-label fw-bold text-danger">Desde</label>
                            <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control" TextMode="Date" />
                        </div>
                        
                        <div class="mb-3">
                            <label for="txtFechaHasta" class="form-label fw-bold text-danger">Hasta</label>
                            <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control" TextMode="Date" />
                        </div>
                        
                        <div class="mb-3">
                            <label for="ddlLocalFiltro" class="form-label fw-bold text-danger">Local</label>
                            <asp:DropDownList ID="ddlLocalFiltro" runat="server" CssClass="form-select">
                            </asp:DropDownList>
                        </div>
                        
                        <div class="mb-3">
                            <label for="ddlEstadoFiltro" class="form-label fw-bold text-danger">Estado</label>
                            <asp:DropDownList ID="ddlEstadoFiltro" runat="server" CssClass="form-select">
                            </asp:DropDownList>
                        </div>
                        
                        <div class="d-grid gap-2">
                            <asp:Button ID="btnFiltrar" runat="server" Text="Aplicar Filtros" 
                                CssClass="btn btn-danger" OnClick="btnFiltrar_Click" />
                            <asp:Button ID="btnLimpiarFiltros" runat="server" Text="Limpiar" 
                                CssClass="btn btn-outline-secondary" OnClick="btnLimpiarFiltros_Click" />
                        </div>
                    </div>
                </div>
                
                <!-- Panel de acciones rápidas -->
                <div class="card border-warning mt-3">
                    <div class="card-header bg-warning text-dark">
                        <h6 class="mb-0">
                            <i class="fas fa-plus-circle me-2"></i>Acciones Rápidas
                        </h6>
                    </div>
                    <div class="card-body">
                        <div class="d-grid gap-2">
                            <button type="button" class="btn btn-outline-primary btn-sm" onclick="window.location.href='Reg_Resev_Comun.aspx'">
                                <i class="fas fa-utensils me-1"></i>Nueva Reserva Común
                            </button>
                            <button type="button" class="btn btn-outline-success btn-sm" onclick="window.location.href='Reg_Resev_Evento.aspx'">
                                <i class="fas fa-glass-cheers me-1"></i>Nueva Reserva Evento
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Lista de reservas -->
            <div class="col-md-9">
                <div id="contenedorReservas" class="d-flex flex-column gap-4">
                    <asp:Repeater ID="rptReservas" runat="server">
                        <ItemTemplate>
                            <div class="card border-start border-4 <%# Eval("EstaCancelada").ToString() == "True" ? "border-danger" : "border-success" %>">
                                <div class="card-body">
                                    <!-- Header de la reserva -->
                                    <div class="d-flex justify-content-between align-items-center mb-3">
                                        <div class="d-flex align-items-center">
                                            <h5 class="card-title mb-0 me-3">
                                                <i class="fas fa-calendar-check text-danger me-2"></i>
                                                Reserva #<%# Eval("ReservaID") %>
                                            </h5>
                                            <span class="<%# Eval("EstadoClass") %>">
                                                <%# Eval("Estado") %>
                                            </span>
                                        </div>
                                        <div class="text-muted">
                                            <i class="fas fa-clock me-1"></i>
                                            <%# Eval("Fecha") %> - <%# Eval("Hora") %>
                                        </div>
                                    </div>
                                    
                                    <!-- Información básica -->
                                    <div class="row mb-3">
                                        <div class="col-md-6">
                                            <div class="info-item mb-2">
                                                <span class="fw-bold text-danger">
                                                    <i class="fas fa-map-marker-alt me-1"></i>Local:
                                                </span>
                                                <%# Eval("Local") %>
                                            </div>
                                            <div class="info-item mb-2">
                                                <span class="fw-bold text-danger">
                                                    <i class="fas fa-users me-1"></i>Personas:
                                                </span>
                                                <%# Eval("Personas") %>
                                            </div>
                                            <div class="info-item mb-2">
                                                <span class="fw-bold text-danger">
                                                    <i class="fas fa-table me-1"></i>Mesas:
                                                </span>
                                                <%# Eval("Mesas") %>
                                            </div>
                                            <div class="info-item">
                                                <span class="fw-bold text-danger">
                                                    <i class="fas fa-chair me-1"></i>Ubicación preferida:
                                                </span>
                                                <%# Eval("Ubicacion") %>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <!-- Información específica de evento -->
                                            <asp:Panel runat="server" Visible='<%# Eval("TipoReserva").ToString() == "EVENTO" && !string.IsNullOrEmpty(Eval("NombreEvento").ToString()) %>'>
                                                <div class="info-item mb-2">
                                                    <span class="fw-bold text-danger">
                                                        <i class="fas fa-glass-cheers me-1"></i>Evento:
                                                    </span>
                                                    <%# Eval("NombreEvento") %>
                                                </div>
                                                <div class="info-item mb-2">
                                                    <span class="fw-bold text-danger">Descripción:</span>
                                                    <div class="bg-light rounded p-2 mt-1">
                                                        <%# Eval("DescripcionEvento") %>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            
                                            <!-- Observaciones -->
                                            <div class="info-item">
                                                <span class="fw-bold text-danger">
                                                    <i class="fas fa-comment-alt me-1"></i>Observaciones:
                                                </span>
                                                <div class="bg-light rounded p-2 mt-1">
                                                    <%# Eval("Observaciones") %>
                                                </div>
                                            </div>
                                            
                                            <!-- Motivo de cancelación (si aplica) -->
                                            <asp:Panel runat="server" Visible='<%# Eval("EstaCancelada").ToString() == "True" && !string.IsNullOrEmpty(Eval("MotivoCancelacion").ToString()) %>'>
                                                <div class="info-item mt-2">
                                                    <span class="fw-bold text-danger">
                                                        <i class="fas fa-exclamation-triangle me-1"></i>Motivo de cancelación:
                                                    </span>
                                                    <div class="bg-danger-subtle text-danger rounded p-2 mt-1">
                                                        <%# Eval("MotivoCancelacion") %>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                    
                                    <!-- Botones de acción -->
                                    <div class="text-end border-top pt-3">
                                        <%# GetBotonPorEstado(Eval("Estado").ToString(), Eval("ReservaID").ToString()) %>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    
                    <!-- Mensaje cuando no hay reservas -->
                    <div id="mensajeSinReservas" class="text-center py-5" style="display: none;">
                        <div class="text-muted">
                            <i class="fas fa-calendar-times fa-3x mb-3"></i>
                            <h4>No tienes reservas</h4>
                            <p>¡Haz tu primera reserva ahora!</p>
                            <div class="mt-3">
                                <button type="button" class="btn btn-danger me-2" onclick="window.location.href='Reg_Resev_Comun.aspx'">
                                    <i class="fas fa-plus me-1"></i>Nueva Reserva
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal de confirmación -->
    <div class="modal fade" id="modalConfirmarReserva" tabindex="-1" aria-labelledby="modalConfirmarReservaLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content border-warning">
                <div class="modal-header bg-warning-subtle">
                    <h5 class="modal-title fw-bold text-warning" id="modalConfirmarReservaLabel">
                        <i class="fas fa-check-circle me-2"></i>Confirmar Reserva
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <div class="alert alert-info" role="alert">
                        <i class="fas fa-info-circle me-2"></i>
                        ¿Está seguro que desea confirmar esta reserva?
                    </div>
                    <p>Al confirmar la reserva, se asegurarán las mesas seleccionadas y se enviará una confirmación por email.</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                        <i class="fas fa-times me-1"></i>Cancelar
                    </button>
                    <button type="button" class="btn btn-warning" onclick="ejecutarConfirmacion()">
                        <i class="fas fa-check me-1"></i>Confirmar Reserva
                    </button>
                </div>
            </div>
        </div>
    </div>

    <script>
        let reservaAConfirmar = null;

        // Función para confirmar reserva del cliente
        function confirmarReservaCliente(idReserva) {
            Swal.fire({
                title: '¿Confirmar reserva?',
                text: 'Su reserva será confirmada y se asegurarán las mesas',
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#ffc107',
                cancelButtonColor: '#6c757d',
                confirmButtonText: 'Sí, confirmar',
                cancelButtonText: 'Cancelar'
            }).then((result) => {
                if (result.isConfirmed) {
                    // Simular confirmación exitosa
                    Swal.fire({
                        title: '¡Reserva confirmada!',
                        text: 'Su reserva ha sido confirmada exitosamente',
                        icon: 'success',
                        confirmButtonText: 'Ok',
                        confirmButtonColor: '#dc3545'
                    }).then(() => {
                        window.location.reload();
                    });
                }
            });
        }

        // Función para editar reserva del cliente
        function editarReservaCliente(idReserva) {
            Swal.fire({
                title: '¿Editar reserva?',
                text: 'Será redirigido a la página de edición',
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#0d6efd',
                cancelButtonColor: '#6c757d',
                confirmButtonText: 'Sí, editar',
                cancelButtonText: 'Cancelar'
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = `Reg_Resev_Comun.aspx?id=${idReserva}&edit=true`;
                }
            });
        }

        // Función para cancelar reserva del cliente
        function cancelarReservaCliente(idReserva) {
            Swal.fire({
                title: '¿Cancelar reserva?',
                text: 'Esta acción no se puede deshacer',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#dc3545',
                cancelButtonColor: '#6c757d',
                confirmButtonText: 'Sí, cancelar',
                cancelButtonText: 'No'
            }).then((result) => {
                if (result.isConfirmed) {
                    // Simular cancelación exitosa
                    Swal.fire({
                        title: '¡Reserva cancelada!',
                        text: 'Su reserva ha sido cancelada correctamente',
                        icon: 'success',
                        confirmButtonText: 'Ok',
                        confirmButtonColor: '#dc3545'
                    }).then(() => {
                        window.location.reload();
                    });
                }
            });
        }

        // Función para confirmar reserva (funcionalidad antigua)
        function confirmarReserva(idReserva) {
            confirmarReservaCliente(idReserva);
        }

        // Función para editar reserva (funcionalidad antigua)
        function editarReserva(idReserva) {
            editarReservaCliente(idReserva);
        }

        // Función para mostrar mensaje cuando no hay reservas
        function mostrarMensajeSinReservas() {
            document.getElementById('mensajeSinReservas').style.display = 'block';
        }

        // Validación de fechas
        document.addEventListener('DOMContentLoaded', function() {
            const fechaDesde = document.getElementById('<%= txtFechaDesde.ClientID %>');
            const fechaHasta = document.getElementById('<%= txtFechaHasta.ClientID %>');
            
            if (fechaDesde && fechaHasta) {
                fechaDesde.addEventListener('change', function() {
                    if (fechaHasta.value && fechaDesde.value > fechaHasta.value) {
                        Swal.fire('Error', 'La fecha desde no puede ser mayor que la fecha hasta', 'warning');
                        fechaDesde.value = '';
                    }
                });
                
                fechaHasta.addEventListener('change', function() {
                    if (fechaDesde.value && fechaHasta.value < fechaDesde.value) {
                        Swal.fire('Error', 'La fecha hasta no puede ser menor que la fecha desde', 'warning');
                        fechaHasta.value = '';
                    }
                });
            }
        });

        // Función para actualizar automáticamente cuando cambian los filtros
        function aplicarFiltroAutomatico() {
            document.getElementById('<%= btnFiltrar.ClientID %>').click();
        }
    </script>

    <style>
        .card-reserva {
            border: 1px solid #dee2e6;
            border-radius: 0.5rem;
            background: white;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
            transition: transform 0.2s, box-shadow 0.2s;
        }
        
        .card-reserva:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 8px rgba(0,0,0,0.15);
        }
        
        .info-item {
            font-size: 0.9rem;
            line-height: 1.4;
        }
        
        .badge {
            font-size: 0.75rem;
            padding: 0.5rem 0.75rem;
        }
        
        .border-start.border-4 {
            border-left-width: 4px !important;
        }
        
        .btn-sm {
            padding: 0.25rem 0.75rem;
            font-size: 0.875rem;
        }
        
        .filtro-label {
            font-weight: 600;
            color: #dc3545;
            font-size: 0.9rem;
        }
        
        .bg-danger-subtle {
            background-color: rgba(220, 53, 69, 0.1);
        }
        
        .bg-warning-subtle {
            background-color: rgba(255, 193, 7, 0.1);
        }
    </style>
</asp:Content>
