<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="reporte_reservas.aspx.cs" Inherits="SoftResWA.Views.Reportes.reporte_reservas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Reporte de Reservas
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="text-start mb-3">
        <h1 class="h3 text-danger fw-bold mb-0 me-3">
            <i class="fas fa-file me-2"></i>Reporte de Reservas
        </h1>
    </div>
    <!-- Filtros para Reservas -->
    <div class="container-fluid mb-4 ps-4">
        <div class="row gx-3">
            <!-- Fecha Inicio -->
            <div class="col-auto">
                <label class="form-label">Fecha Inicio</label>
                <asp:TextBox ID="txtFechaIni" runat="server" CssClass="form-control" TextMode="Date" />
            </div>
            <!-- Fecha Final -->
            <div class="col-auto">
                <label class="form-label">Fecha Final</label>
                <asp:TextBox ID="txtFechaFin" runat="server" CssClass="form-control" TextMode="Date" />
            </div>
            <!-- Sede -->
            <div class="col-auto">
                <label for="ddlSede" class="form-label">Local</label>
                <select id="ddlSede" class="form-select">
                    <option selected disabled>Seleccionar...</option>
                    <option value="1">Sede Lima Centro</option>
                    <option value="2">Sede Callao</option>
                </select>
            </div>
            <!-- Rol -->
            <div class="col-auto">
                <label for="ddlRol" class="form-label">Local</label>
                <select id="ddlRol" class="form-select">
                    <option selected disabled>Seleccionar...</option>
                    <option value="1">Cliente VIP</option>
                    <option value="2">Callao Normal</option>
                </select>
            </div>
            <!-- Botones -->
            <div class="col-auto d-flex align-items-end">
                <button type="button" class="btn btn-danger me-2">
                    <i class="fas fa-search me-1"></i>Buscar
                </button>
                <button type="button" class="btn btn-danger me-2">
                    <i class="fas fa-download me-1"></i>Descargar PDF
                </button>
            </div>
        </div>
    </div>
</asp:Content>
