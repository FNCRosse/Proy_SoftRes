<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="reserva_gestion.aspx.cs" Inherits="SoftResWA.Views.Reservas.reserva_gestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Gestión de Reservas
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container-fluid py-4">
        <h2 class="text-center fw-bold text-danger mb-4">Gestión de Reservas</h2>

        <!-- Pestañas de navegación -->
        <ul class="nav nav-tabs mb-4" id="gestionTabs" role="tablist">
            <li class="nav-item" role="presentation">
                <button class="nav-link active" id="reservas-tab" data-bs-toggle="tab" data-bs-target="#reservas" type="button" role="tab" aria-controls="reservas" aria-selected="true">
                    Reservas
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link" id="espera-tab" data-bs-toggle="tab" data-bs-target="#espera" type="button" role="tab" aria-controls="espera" aria-selected="false">
                    Lista de Espera <span class="badge bg-primary ms-2"><asp:Label ID="lblEsperaCount" runat="server" Text="0"></asp:Label></span>
                </button>
            </li>
        </ul>

        <div class="tab-content" id="gestionTabsContent">
            <!-- Pestaña de Reservas -->
            <div class="tab-pane fade show active" id="reservas" role="tabpanel" aria-labelledby="reservas-tab">
                <!-- Filtros de búsqueda -->
                <div class="row mb-4">
                    <div class="col-md-3">
                        <div class="form-group">
                            <label for="txtFechaDesde" class="form-label">Fecha desde:</label>
                            <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <label for="txtFechaHasta" class="form-label">Fecha hasta:</label>
                            <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <label for="ddlLocal" class="form-label">Local:</label>
                            <asp:DropDownList ID="ddlLocal" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlLocal_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <label for="ddlEstado" class="form-label">Estado:</label>
                            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Todos" Value=""></asp:ListItem>
                                <asp:ListItem Text="Pendiente" Value="PENDIENTE"></asp:ListItem>
                                <asp:ListItem Text="Confirmada" Value="CONFIRMADA"></asp:ListItem>
                                <asp:ListItem Text="Cancelada" Value="CANCELADA"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="row mb-4">
                    <div class="col-12 text-end">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                    </div>
                </div>

                <!-- GridView de Reservas -->
                <asp:UpdatePanel ID="upReservas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvReservas" runat="server" CssClass="table table-striped table-hover"
                            AutoGenerateColumns="False" DataKeyNames="idReserva" OnRowCommand="gvReservas_RowCommand"
                            OnRowDataBound="gv_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="idReserva" HeaderText="ID" />
                                <asp:BoundField DataField="fecha_Hora" HeaderText="Fecha y Hora" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                <asp:BoundField DataField="cantidad_personas" HeaderText="Personas" />
                                <asp:BoundField DataField="local.nombre" HeaderText="Local" />
                                <asp:BoundField DataField="usuario.nombreComp" HeaderText="Cliente" />
                                <asp:BoundField DataField="usuario.telefono" HeaderText="Teléfono" />
                                <asp:BoundField DataField="estado" HeaderText="Estado" />
                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btn btn-sm btn-warning"
                                            CommandName="EditarReserva" CommandArgument='<%# Eval("idReserva") %>' />
                                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-sm btn-danger"
                                            CommandName="CancelarReserva" CommandArgument='<%# Eval("idReserva") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <!-- Pestaña de Lista de Espera -->
            <div class="tab-pane fade" id="espera" role="tabpanel" aria-labelledby="espera-tab">
                <asp:UpdatePanel ID="upListaEspera" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvListaEspera" runat="server" CssClass="table table-striped table-hover"
                            AutoGenerateColumns="False" DataKeyNames="idFila" OnRowCommand="gvListaEspera_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="idFila" HeaderText="ID" />
                                <asp:BoundField DataField="usuario.nombreComp" HeaderText="Cliente" />
                                <asp:BoundField DataField="usuario.telefono" HeaderText="Teléfono" />
                                <asp:BoundField DataField="local.nombre" HeaderText="Local" />
                                <asp:BoundField DataField="cantidadPersonas" HeaderText="Personas" />
                                <asp:BoundField DataField="fechaHoraDeseada" HeaderText="Fecha y Hora Deseada" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                <asp:BoundField DataField="fechaRegistro" HeaderText="Fecha Registro" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                <asp:BoundField DataField="estado" HeaderText="Estado" />
                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <asp:Button ID="btnCrearReserva" runat="server" Text="Crear Reserva" CssClass="btn btn-sm btn-success"
                                            CommandName="CrearReserva" CommandArgument='<%# Eval("idFila") %>' />
                                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-sm btn-danger"
                                            CommandName="EliminarEspera" CommandArgument='<%# Eval("idFila") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!-- Modal de Cancelación de Reserva -->
    <div class="modal fade" id="modalCancelarReserva" tabindex="-1" aria-labelledby="modalCancelarReservaLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalCancelarReservaLabel">Cancelar Reserva</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p>¿Está seguro que desea cancelar esta reserva?</p>
                    <div class="form-group">
                        <label for="ddlMotivoCancelacion" class="form-label">Motivo de cancelación:</label>
                        <asp:DropDownList ID="ddlMotivoCancelacion" runat="server" CssClass="form-select">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                    <asp:Button ID="btnCancelarReservaHidden" runat="server" Text="Confirmar" CssClass="btn btn-danger"
                        OnClick="btnCancelarReserva_Click" />
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnIdReservaCancelar" runat="server" />
    <asp:HiddenField ID="hdnIdMotivoCancelacion" runat="server" />

    <script type="text/javascript">
        // Variable global para almacenar el ID de la reserva a cancelar
        var reservaACancelar = null;

        // Función para confirmar cancelación
        function confirmarCancelacion(idReserva) {
            reservaACancelar = idReserva;
            var modal = new bootstrap.Modal(document.getElementById('modalCancelarReserva'));
            modal.show();
            return false;
        }

        // Función para ejecutar la cancelación
        function ejecutarCancelacion() {
            var motivoSelect = document.getElementById('ddlMotivoCancelacion');
            var motivo = motivoSelect.value;
            
            if (!motivo) {
                motivoSelect.classList.add('is-invalid');
                alert('Debe seleccionar un motivo de cancelación');
                return;
            }
            
            motivoSelect.classList.remove('is-invalid');
            
            // Establecer valores en campos ocultos
            document.getElementById('<%= hdnIdReservaCancelar.ClientID %>').value = reservaACancelar;
            document.getElementById('<%= hdnIdMotivoCancelacion.ClientID %>').value = motivo;
            
            // Cerrar modal
            var modal = bootstrap.Modal.getInstance(document.getElementById('modalCancelarReserva'));
            modal.hide();
            
            // Ejecutar postback
            document.getElementById('<%= btnCancelarReservaHidden.ClientID %>').click();
        }

        // Validación de campos de fecha
        document.addEventListener('DOMContentLoaded', function() {
            var fechaDesde = document.getElementById('<%= txtFechaDesde.ClientID %>');
            var fechaHasta = document.getElementById('<%= txtFechaHasta.ClientID %>');
            
            // Validar que fecha hasta no sea menor que fecha desde
            if (fechaHasta) {
                fechaHasta.addEventListener('change', function() {
                    if (fechaDesde.value && fechaHasta.value) {
                        if (new Date(fechaHasta.value) < new Date(fechaDesde.value)) {
                            alert('La fecha hasta no puede ser menor que la fecha desde');
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
                            alert('La fecha desde no puede ser mayor que la fecha hasta');
                            fechaDesde.value = '';
                        }
                    }
                });
            }
        });
    </script>

    <style type="text/css">
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
