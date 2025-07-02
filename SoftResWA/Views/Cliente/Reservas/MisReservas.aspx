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
                        <label class="form-label filtro-label">Estado</label>
                        <div class="position-relative">
                            <select class="form-select form-control-custom">
                                <option>Estados</option>
                            </select>
                            <i class="fas fa-chevron-down dropdown-icon"></i>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Lista de reservas -->
            <div class="col-md-9">
                <div class="d-flex flex-column gap-4">
                    <asp:Repeater ID="rptReservas" runat="server">
                        <ItemTemplate>
                            <div class="card-reserva p-4">
                                <div class="d-flex justify-content-between align-items-center">
                                    <div>
                                        <span class="fw-bold text-danger">Fecha:</span> <%# Eval("Fecha") %>
                                        <span class="fw-bold text-danger ms-4">Hora:</span> <%# Eval("Hora") %>
                                        <span class="fw-bold text-danger ms-4">Local:</span> <%# Eval("Local") %>
                                        <span class="fw-bold text-danger ms-4">Estado:</span> <%# Eval("Estado") %>
                                    </div>
                                    <i class="fas fa-exclamation-triangle text-warning"></i>
                                </div>
                                <div class="row mt-3">
                                    <div class="col-md-6">
                                        <p class="fw-bold text-danger mb-1">Cantidad Personas:</p>
                                        <p><%# Eval("Personas") %></p>
                                        <p class="fw-bold text-danger mb-1">Cantidad de Mesas:</p>
                                        <p><%# Eval("Mesas") %></p>
                                        <p class="fw-bold text-danger mb-1">Ubicación de preferencia:</p>
                                        <p><%# Eval("Ubicacion") %></p>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:Panel runat="server" Visible='<%# Eval("NombreEvento") != DBNull.Value %>'>
                                            <p class="fw-bold text-danger mb-1">Nombre del evento:</p>
                                            <p><%# Eval("NombreEvento") %></p>
                                            <p class="fw-bold text-danger mb-1">Descripción del evento</p>
                                            <div class="bg-light rounded p-2"><%# Eval("DescripcionEvento") %></div>
                                        </asp:Panel>
                                        <p class="fw-bold text-danger mt-2 mb-1">Observaciones</p>
                                        <div class="bg-light rounded p-2"><%# Eval("Observaciones") %></div>

                                        <div class="text-end mt-3">
                                            <asp:Button ID="btnEditar" runat="server" CssClass="btn btn-danger px-4" Text="Editar" Visible='<%# Eval("Estado").ToString() == "Confirmado" %>' />
                                            <asp:Button ID="btnConfirmar" runat="server" CssClass="btn btn-warning px-4" Text="Confirmar" Visible='<%# Eval("Estado").ToString() == "Pendiente" %>' />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
