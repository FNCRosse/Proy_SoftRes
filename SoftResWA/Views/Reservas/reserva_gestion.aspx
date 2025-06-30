<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="reserva_gestion.aspx.cs" Inherits="SoftResWA.Views.Reservas.reserva_gestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Gestión de Reservas
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="text-start mb-3">
        <h1 class="h3 text-danger fw-bold mb-0 me-3">
            <i class="fas fa-calendar-check me-2"></i>Gestión de Reservas
        </h1>
    </div>
    
    <!-- Filtros para búsqueda -->
    <div class="container-fluid mb-4 ps-4">
        <div class="row gx-3">
            <!-- Tipo de Reservas -->
            <div class="col-auto">
                <label for="ddlTipRes" class="form-label">Tipo de Reservas</label>
                <select id="ddlTipRes" name="ddlTipRes" class="form-select">
                    <option value="">-- Todas --</option>
                    <option value="1">Común</option>
                    <option value="0">Evento</option>
                </select>
            </div>
            <!-- Fecha desde -->
            <div class="col-md-2">
                <label for="txtFechaDesde" class="form-label">Fecha Desde</label>
                <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control" TextMode="Date" />
            </div>
            <!-- Fecha Hasta -->
            <div class="col-md-2">
                <label for="txtFechaHasta" class="form-label">Fecha Hasta</label>
                <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control" TextMode="Date" />
            </div>
            <!-- DNI Cliente -->
            <div class="col-auto">
                <label for="txtDniCliente" class="form-label">DNI Cliente</label>
                <input type="text" id="txtDniCliente" name="txtDniCliente" class="form-control" placeholder="Ej. 12345678" maxlength="8" />
            </div>
            <!-- Local -->
            <div class="col-auto">
                <label for="ddlLocal" class="form-label">Local</label>
                <asp:DropDownList ID="ddlLocal" runat="server" CssClass="form-select">
                </asp:DropDownList>
            </div>
            <!-- Botones -->
            <div class="col-auto d-flex align-items-end">
                <asp:Button ID="btnBuscar" runat="server" Text="🔍 Buscar" CssClass="btn btn-danger me-2"
                    OnClick="btnBuscar_Click" />
                <asp:Button ID="btnNuevo" runat="server" Text="➕ Nueva Reserva" CssClass="btn shadow-sm"
                    OnClientClick="window.location.href='registrar_reserva_comun.aspx'; return false;"
                    Style="background-color: #FFF3CD; color: #856404; border: 1px solid #d39e00;" />
            </div>
        </div>
    </div>

    <!-- Navegación por pestañas -->
    <ul class="nav nav-tabs mb-3" id="tabsGestion" role="tablist">
        <li class="nav-item" role="presentation">
            <button class="nav-link active" id="tab-reservas" data-bs-toggle="tab" data-bs-target="#tabpane-reservas" 
                type="button" role="tab" aria-controls="tabpane-reservas" aria-selected="true">
                <i class="fas fa-calendar-check me-2"></i>📋 Gestión de Reservas
            </button>
        </li>
        <li class="nav-item" role="presentation">
            <button class="nav-link" id="tab-asignaciones" data-bs-toggle="tab" data-bs-target="#tabpane-asignaciones" 
                type="button" role="tab" aria-controls="tabpane-asignaciones" aria-selected="false">
                <i class="fas fa-table me-2"></i>🪑 Asignación de Mesas
            </button>
        </li>
    </ul>

    <!-- Contenido de las pestañas -->
    <div class="tab-content" id="tabsGestionContent">
        
        <!-- Pestaña: Gestión de Reservas -->
        <div class="tab-pane fade show active" id="tabpane-reservas" role="tabpanel" aria-labelledby="tab-reservas">
            <div class="row">
                <div class="col-12">
            <asp:GridView ID="gvReservas" runat="server" CssClass="table table-hover table-striped" 
                AutoGenerateColumns="False" EmptyDataText="No se encontraron reservas">
                <Columns>
                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <!-- Los botones se generan dinámicamente en el code-behind -->
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="ID" DataField="idReserva" HeaderStyle-Width="60px" />
                    <asp:BoundField HeaderText="Tipo" DataField="TipoReserva" />
                    <asp:BoundField HeaderText="Fecha" DataField="Fecha" />
                    <asp:BoundField HeaderText="Hora" DataField="Hora" />
                    <asp:BoundField HeaderText="Local" DataField="Local" />
                    <asp:BoundField HeaderText="Solicitante" DataField="Solicitante" />
                    <asp:BoundField HeaderText="Estado" DataField="Estado" />
                    <asp:BoundField HeaderText="Ubicación Mesa" DataField="UbicacionMesa" />
                    <asp:BoundField HeaderText="Observaciones" DataField="observaciones" />
                    <asp:BoundField HeaderText="Motivo Cancelación" DataField="MotivoCancelacion" />
                </Columns>
                <HeaderStyle CssClass="table-dark" />
                <EmptyDataRowStyle CssClass="text-center text-muted" />
            </asp:GridView>
                </div>
            </div>
        </div>

        <!-- Pestaña: Asignación de Mesas -->
        <div class="tab-pane fade" id="tabpane-asignaciones" role="tabpanel" aria-labelledby="tab-asignaciones">
            <div class="row">
                <div class="col-12">
                    <div class="alert alert-info mb-3" role="alert">
                        <i class="fas fa-info-circle me-2"></i>
                        <strong>Asignación de Mesas:</strong> Aquí puede ver y gestionar qué mesas específicas están asignadas a cada reserva.
                    </div>
                    
                    <asp:GridView ID="gvReservaxMesas" runat="server" CssClass="table table-hover table-striped" 
                        AutoGenerateColumns="False" EmptyDataText="No se encontraron asignaciones de mesas">
                        <Columns>
                                                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Button ID="btnEliminarAsignacion" runat="server" 
                                        Text="🗑️" ToolTip="Eliminar asignación" 
                                        CssClass="btn btn-sm btn-outline-danger"
                                        CommandArgument='<%# Eval("ReservaID") + "|" + Eval("MesaID") %>'
                                        OnClientClick="return confirm('¿Está seguro que desea eliminar esta asignación de mesa?');"
                                        OnClick="btnEliminarAsignacion_Click"
                                        Visible='<%# (bool)Eval("PuedeEliminar") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:BoundField HeaderText="Reserva ID" DataField="ReservaID" HeaderStyle-Width="80px" />
                            <asp:BoundField HeaderText="Tipo" DataField="TipoReserva" HeaderStyle-Width="100px" />
                            <asp:BoundField HeaderText="Fecha" DataField="FechaReserva" HeaderStyle-Width="140px" />
                            <asp:BoundField HeaderText="Cliente" DataField="Cliente" HeaderStyle-Width="150px" />
                            <asp:BoundField HeaderText="Local" DataField="Local" HeaderStyle-Width="120px" />
                            <asp:BoundField HeaderText="Estado Reserva" DataField="EstadoReserva" HeaderStyle-Width="120px" />
                            
                            <asp:BoundField HeaderText="Mesa ID" DataField="MesaID" HeaderStyle-Width="70px" />
                            <asp:BoundField HeaderText="Núm. Mesa" DataField="NumeroMesa" HeaderStyle-Width="90px" />
                            <asp:BoundField HeaderText="Capacidad" DataField="CapacidadMesa" HeaderStyle-Width="80px" />
                            <asp:BoundField HeaderText="Tipo Mesa" DataField="TipoMesa" HeaderStyle-Width="100px" />
                            <asp:BoundField HeaderText="Estado Mesa" DataField="EstadoMesa" HeaderStyle-Width="100px" />
                            <asp:BoundField HeaderText="Ubicación" DataField="UbicacionMesa" HeaderStyle-Width="90px" />
                        </Columns>
                        <HeaderStyle CssClass="table-dark" />
                        <EmptyDataRowStyle CssClass="text-center text-muted" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

    <!-- Hidden fields para JavaScript -->
    <asp:HiddenField ID="hdnIdReservaCancelar" runat="server" />
    <asp:HiddenField ID="hdnIdMotivoCancelacion" runat="server" />
    <asp:Button ID="btnCancelarReservaHidden" runat="server" Style="display: none;" OnClick="btnCancelarReserva_Click" />

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
                            <option value="2">No disponibilidad de mesa</option>
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
            const dniInput = document.getElementById('txtDniCliente');
            
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
            
            // Validar DNI (solo números, 8 dígitos)
            if (dniInput) {
                dniInput.addEventListener('input', function() {
                    let value = this.value.replace(/\D/g, ''); // Solo números
                    if (value.length > 8) {
                        value = value.substring(0, 8);
                    }
                    this.value = value;
                });
                
                dniInput.addEventListener('blur', function() {
                    if (this.value && this.value.length !== 8) {
                        Swal.fire('Error', 'El DNI debe tener exactamente 8 dígitos', 'warning');
                        this.focus();
                    }
                });
            }
            
            // Cambiar evento del botón "Nuevo"
            const btnNuevo = document.getElementById('<%= btnNuevo.ClientID %>');
            if (btnNuevo) {
                btnNuevo.setAttribute('onclick', 'mostrarTipoReserva(); return false;');
            }
        });

        // Función para limpiar filtros
        function limpiarFiltros() {
            document.getElementById('ddlTipRes').value = '';
            document.getElementById('<%= txtFechaDesde.ClientID %>').value = '';
            document.getElementById('<%= txtFechaHasta.ClientID %>').value = '';
            document.getElementById('txtDniCliente').value = '';
            document.getElementById('<%= ddlLocal.ClientID %>').value = '';
        }

        // Función para confirmar eliminación de asignación (mejorada con SweetAlert)
        function confirmarEliminacionAsignacion(idReserva, idMesa) {
            Swal.fire({
                title: '¿Eliminar asignación?',
                text: `¿Está seguro que desea eliminar la asignación de la mesa ${idMesa} de la reserva ${idReserva}?`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#dc3545',
                cancelButtonColor: '#6c757d',
                confirmButtonText: '🗑️ Sí, eliminar',
                cancelButtonText: '❌ Cancelar',
                focusCancel: true
            }).then((result) => {
                if (result.isConfirmed) {
                    return true; // Permitir que continúe el postback
                } else {
                    return false; // Cancelar el postback
                }
            });
            return false; // Evitar el postback inicial mientras se muestra el modal
        }

        // Función para mostrar información de una reserva
        function mostrarInfoReserva(idReserva) {
            Swal.fire({
                title: 'Información de la Reserva',
                text: `Mostrando detalles de la reserva ${idReserva}`,
                icon: 'info',
                confirmButtonText: 'Cerrar'
            });
        }

        // Función para mostrar estadísticas de asignaciones
        function mostrarEstadisticas() {
            const tablaAsignaciones = document.querySelector('#tabpane-asignaciones table tbody');
            if (tablaAsignaciones) {
                const filas = tablaAsignaciones.querySelectorAll('tr');
                const totalAsignaciones = filas.length;
                
                Swal.fire({
                    title: 'Estadísticas de Asignaciones',
                    html: `
                        <div class="text-start">
                            <p><strong>Total de asignaciones:</strong> ${totalAsignaciones}</p>
                            <p><strong>Estado:</strong> Sistema funcionando correctamente</p>
                        </div>
                    `,
                    icon: 'info',
                    confirmButtonText: 'Cerrar'
                });
            }
        }
    </script>

    <style>
        .table th {
            border-top: none;
            font-weight: 600;
            font-size: 0.9rem;
        }
        
        .table td {
            vertical-align: middle;
            font-size: 0.9rem;
        }
        
        .btn-sm {
            padding: 0.25rem 0.5rem;
            font-size: 0.875rem;
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

        /* Mejorar la apariencia de la tabla de asignaciones */
        #tabpane-asignaciones .table {
            font-size: 0.85rem;
        }

        #tabpane-asignaciones .table th {
            background-color: #343a40;
            color: white;
            font-size: 0.8rem;
            padding: 0.5rem;
        }

        #tabpane-asignaciones .table td {
            padding: 0.5rem;
        }

        /* Resaltar filas importantes */
        .table-hover tbody tr:hover {
            background-color: rgba(0, 123, 255, 0.075);
        }

        /* Botones de acción mejorados */
        .btn-outline-danger:hover {
            transform: scale(1.05);
            transition: transform 0.2s;
        }

        /* Alertas personalizadas */
        .alert-info {
            border-left: 4px solid #17a2b8;
        }
    </style>
</asp:Content>
