<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="reserva_gestion.aspx.cs" Inherits="SoftResWA.Views.Reservas.reserva_gestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Listar Reservas
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="text-start mb-3">
        <h1 class="h3 text-danger fw-bold mb-0 me-3">
            <i class="fas fa-gears me-2"></i>Mantenimiento de Locales
        </h1>
    </div>
    <!-- Filtros para  -->
    <div class="container-fluid mb-4 ps-4">
        <div class="row gx-3">
            <!-- Tipo de Reservas -->
            <div class="col-auto">
                <label for="ddlTipRes" class="form-label">Tipo de Reservas</label>
                <select id="ddlTipRes" class="form-select">
                    <option selected disabled>Seleccionar...</option>
                    <option value="1">Común</option>
                    <option value="0">Evento</option>
                </select>
            </div>
            <!-- Fecha desde -->
            <div class="col-md-2">
                <label for="txtFechaDesde" class="form-label">Fecha Desde</label>
                <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control" TextMode="Date" placeholder="Fecha desde" />
            </div>
            <!-- Fecha Hasta -->
            <div class="col-md-2">
                <label for="txtFechaHasta" class="form-label">Fecha Hasta</label>
                <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control" TextMode="Date" placeholder="Fecha hasta" />
            </div>
            <!-- Usuario -->
            <div class="col-auto">
                <label for="txtDniCliente" class="form-label">DNI Cliente</label>
                <input type="text" id="txtDniCliente" class="form-control" />
            </div>
            <!-- Local -->
            <div class="col-auto">
                <label for="ddlLocal" class="form-label">Local</label>
                <select id="ddlLocal" class="form-select">
                    <option selected disabled>Seleccionar...</option>
                    <option value="1">Local San Miguel</option>
                    <option value="0">Local Callao</option>
                </select>
            </div>
            <!-- Botones -->
            <div class="col-auto d-flex align-items-end">
                <button type="button" class="btn btn-danger me-2">
                    <i class="fas fa-search me-1"></i>Buscar
                </button>
                <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" class="btn shadow-sm"
                    style="background-color: #FFF3CD; color: #856404; border: 1px solid #d39e00;" OnClientClick="mostrarTipoReserva(); return false;" />
            </div>
        </div>
    </div>

    <!-- Tabla -->
    <div class="row">
        <asp:GridView ID="gvReservas" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <button type="button" class="btn btn-sm btn-primary" onclick="modificarReserva('<%# Eval("IdReserva") %>')">M</button>
                        <button type="button" class="btn btn-sm btn-danger" onclick="confirmarCancelacion('<%# Eval("IdReserva") %>')">C</button>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Tipo de Reserva" DataField="TipoReserva" />
                <asp:BoundField HeaderText="Fecha" DataField="Fecha" />
                <asp:BoundField HeaderText="Hora" DataField="Hora" />
                <asp:BoundField HeaderText="Local" DataField="Local" />
                <asp:BoundField HeaderText="Solicitante" DataField="Solicitante" />
                <asp:BoundField HeaderText="Observaciones" DataField="Observaciones" />
                <asp:BoundField HeaderText="Motivo de Cancelación" DataField="MotivoCancelacion" />
                <asp:BoundField HeaderText="Ubicación de mesa preferida" DataField="UbicacionMesa" />
                <asp:BoundField HeaderText="Fila de espera" DataField="FilaEspera" />
                <asp:BoundField HeaderText="Estado" DataField="Estado" />
            </Columns>
        </asp:GridView>
    </div>
    <script src="../../Scripts/MyScripts.js"></script>
</asp:Content>
