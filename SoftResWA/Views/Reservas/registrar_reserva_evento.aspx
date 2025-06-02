<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="registrar_reserva_evento.aspx.cs" Inherits="SoftResWA.Views.Reservas.registrar_reserva_evento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Registrar reservas evento
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container mt-4">
        <div class="card">
            <div class="card-header bg-warning">
                <h5 class="mb-0"><i class="fas fa-calendar-plus me-2"></i>Registrar Reserva para Evento</h5>
            </div>
            <div class="card-body">
                <div class="row g-4">
                    <!-- Información General -->
                    <div class="col-md-6">
                        <div class="border rounded p-3">
                            <h6 class="fw-bold mb-3">Información General</h6>
                            <div class="mb-3">
                                <label class="form-label">Nombre del evento</label>
                                <asp:TextBox ID="txtNombreEvento" runat="server" CssClass="form-control" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Descripción del Evento</label>
                                <asp:TextBox ID="txtDescripcionEvento" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
                            </div>
                            <div class="row g-2 mb-3">
                                <div class="col-md-6">
                                    <label class="form-label">Fecha</label>
                                    <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date" />
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Hora</label>
                                    <asp:TextBox ID="txtHora" runat="server" CssClass="form-control" TextMode="Time" />
                                </div>
                            </div>
                            <div class="row g-2 mb-3">
                                <div class="col-md-6">
                                    <label class="form-label">Cantidad de personas</label>
                                    <asp:TextBox ID="txtCantidadPersonas" runat="server" CssClass="form-control" TextMode="Number" />
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Cantidad de mesas</label>
                                    <asp:TextBox ID="txtCantidadMesas" runat="server" CssClass="form-control" TextMode="Number" />
                                </div>
                            </div>
                            <div class="mb-2">
                                <label class="form-label">Observaciones</label>
                                <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                            </div>
                        </div>
                    </div>

                    <!-- Información del Local, Mesa, y Usuario -->
                    <div class="col-md-6">
                        <!-- Local -->
                        <div class="border rounded p-3 mb-3">
                            <h6 class="fw-bold mb-3">Información del local</h6>
                            <div class="input-group mb-2">
                                <asp:TextBox ID="txtIdLocal" runat="server" CssClass="form-control" placeholder="Id del Local" />
                                <asp:Button ID="btnBuscarLocal" runat="server" Text="buscar" CssClass="btn btn-outline-secondary" />
                            </div>
                            <asp:TextBox ID="txtNombreLocal" runat="server" CssClass="form-control" placeholder="Nombre" ReadOnly="true" />
                        </div>

                        <!-- Ubicación Mesa Preferida -->
                        <div class="border rounded p-3 mb-3">
                            <h6 class="fw-bold mb-3">Información de la ubicación de mesa preferida</h6>
                            <div class="input-group mb-2">
                                <asp:TextBox ID="txtIdTipoMesa" runat="server" CssClass="form-control" placeholder="Id del tipo de mesa" />
                                <asp:Button ID="btnBuscarTipoMesa" runat="server" Text="buscar" CssClass="btn btn-outline-secondary" />
                            </div>
                            <asp:TextBox ID="txtNombreTipoMesa" runat="server" CssClass="form-control" placeholder="Nombre" ReadOnly="true" />
                        </div>

                        <!-- Usuario -->
                        <div class="border rounded p-3">
                            <h6 class="fw-bold mb-3">Información del usuario</h6>
                            <div class="row g-2">
                                <div class="col-md-6">
                                    <label class="form-label">Documento de identidad</label>
                                    <asp:TextBox ID="txtDocumentoUsuario" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-md-4">
                                    <label class="form-label">Tipo de documento</label>
                                    <asp:DropDownList ID="ddlTipoDocumento" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="Tipo de documento" Value="" />
                                        <asp:ListItem Text="DNI" Value="DNI" />
                                        <asp:ListItem Text="CE" Value="CE" />
                                        <asp:ListItem Text="Pasaporte" Value="Pasaporte" />
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 d-flex align-items-end">
                                    <asp:Button ID="btnBuscarUsuario" runat="server" Text="buscar" CssClass="btn btn-outline-secondary w-100" />
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Nombre</label>
                                    <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control" ReadOnly="true" />
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Tipo de Cliente</label>
                                    <asp:TextBox ID="txtTipoCliente" runat="server" CssClass="form-control" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Botón Guardar -->
                <div class="text-end mt-4">
                    <asp:Button ID="btnGuardarReserva" runat="server" Text="Guardar" CssClass="btn btn-danger px-4" OnClick="btnGuardar_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
