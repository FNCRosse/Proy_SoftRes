<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="registrar_reserva_comun.aspx.cs" Inherits="SoftResWA.Views.Reservas.registar_reserva_comun" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Registrar Reserva Común
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container mt-4">
        <div class="card">
            <div class="card-header bg-warning">
                <h5 class="mb-0"><i class="fas fa-calendar-plus me-2"></i>Registrar Reserva Común</h5>
            </div>
            <div class="card-body">
                <div class="row g-3">
                    <!-- Información General -->
                    <div class="col-md-6">
                        <div class="border rounded p-3">
                            <h6 class="fw-bold mb-3">Información General</h6>
                            <div class="row g-2">
                                <div class="col-md-6">
                                    <label class="form-label">Fecha</label>
                                    <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date" />
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Hora</label>
                                    <asp:TextBox ID="txtHora" runat="server" CssClass="form-control" TextMode="Time" />
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Cantidad de personas</label>
                                    <asp:TextBox ID="txtCantidadPersonas" runat="server" CssClass="form-control" TextMode="Number" />
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Cantidad de mesas</label>
                                    <asp:TextBox ID="txtCantidadMesas" runat="server" CssClass="form-control" TextMode="Number" />
                                </div>
                                <div class="col-12">
                                    <label class="form-label">Observaciones</label>
                                    <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Información del Local + Estado -->
                    <div class="col-md-6">
                        <div class="border rounded p-3 mb-3">
                            <h6 class="fw-bold mb-3">Información del Local</h6>
                            <div class="mb-3">
                                <asp:DropDownList ID="ddlLocales" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Seleccionar Local" Value="" />
                                    <asp:ListItem Text="San Miguel" Value="1" />
                                    <asp:ListItem Text="Callao" Value="2" />
                                </asp:DropDownList>
                            </div>
                            <h6 class="fw-bold mb-3">Ubicación de Mesa Preferida</h6>
                            <div class="mb-3">
                                <asp:DropDownList ID="ddlUbicacionMesa" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Seleccionar Ubicación" Value="" />
                                    <asp:ListItem Text="Ventana" Value="Ventana" />
                                    <asp:ListItem Text="Centro" Value="Centro" />
                                    <asp:ListItem Text="Esquina" Value="Esquina" />
                                </asp:DropDownList>
                            </div>
                            <h6 class="fw-bold mb-3">Estado de Fila de espera</h6>
                            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Seleccionar Estado" Value="" />
                                <asp:ListItem Text="Pendiente" Value="Pendiente" />
                                <asp:ListItem Text="Notificado" Value="Confirmado" />
                                <asp:ListItem Text="Cancelado" Value="Cancelado" />
                                <asp:ListItem Text="Vacio" Value="Cancelado" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <!-- Información del Usuario -->
                <div class="border rounded p-3 mt-4">
                    <h6 class="fw-bold mb-3">Información del Usuario</h6>
                    <div class="row g-2">
                        <div class="col-md-4">
                            <label class="form-label">Documento de identidad</label>
                            <asp:TextBox ID="txtDocumentoUsuario" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-md-4">
                            <label class="form-label">Tipo de documento</label>
                            <asp:DropDownList ID="ddlTipoDocumento" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Seleccionar..." Value="" />
                                <asp:ListItem Text="DNI" Value="DNI" />
                                <asp:ListItem Text="Carnet de Extranjería" Value="Carnet" />
                                <asp:ListItem Text="Pasaporte" Value="Pasaporte" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4 d-flex align-items-end">
                            <asp:Button ID="btnBuscarUsuario" runat="server" Text="Buscar" CssClass="btn btn-outline-secondary w-100" OnClick="btnBuscarUsuario_Click" />
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

                <!-- Botón Guardar -->
                <div class="text-end mt-4">
                    <asp:Button ID="btnGuardarReserva" runat="server" Text="Guardar" CssClass="btn btn-danger px-4" OnClick="btnGuardar_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>