<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="registrar_reserva_evento.aspx.cs" Inherits="SoftResWA.Views.Reservas.registrar_reserva_evento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Registrar reservas evento
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container mt-4">
        <div class="card shadow-sm">
            <div class="card-header bg-success text-white">
                <h4 class="mb-0"><i class="fas fa-glass-cheers me-2"></i>Registrar Reserva para Evento</h4>
            </div>
            <div class="card-body p-4">
                <asp:UpdatePanel ID="updFormularioReserva" runat="server">
                    <ContentTemplate>
                        <!-- PASO 1: Detalles del Evento -->
                        <h5 class="fw-bold text-secondary mb-3">Paso 1: Detalles del Evento</h5>
                        <div class="row g-3 mb-4 p-3 border bg-light rounded-3">
                            <div class="col-12">
                                <label class="form-label">Nombre del Evento<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtNombreEvento" runat="server" CssClass="form-control" placeholder="Ej: Cumpleaños de Ana, Aniversario, etc." />
                                <asp:RequiredFieldValidator ID="rfvNombreEvento" runat="server" ControlToValidate="txtNombreEvento"
                                    ErrorMessage="El nombre del evento es requerido." CssClass="text-danger small" Display="Dynamic" />
                            </div>
                            <div class="col-12">
                                <label class="form-label">Descripción del Evento (Opcional)</label>
                                <asp:TextBox ID="txtDescripcionEvento" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"
                                    placeholder="Detalles adicionales sobre el evento, como el motivo de la celebración." />
                            </div>
                        </div>

                        <!-- PASO 2: ¿Cuándo y para cuántos? -->
                        <h5 class="fw-bold text-secondary mb-3">Paso 2: Detalles de la Reserva</h5>
                        <div class="row g-3 mb-4 p-3 border bg-light rounded-3">
                            <div class="col-md-4">
                                <label class="form-label">Local<span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlLocal" runat="server" CssClass="form-select" />
                                <asp:RequiredFieldValidator ID="rfvLocal" runat="server" ControlToValidate="ddlLocal"
                                    ErrorMessage="Debe seleccionar un local." CssClass="text-danger small" Display="Dynamic" InitialValue="0" />
                            </div>
                            <div class="col-md-4">
                                <label class="form-label">Fecha<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date" />
                                <asp:RequiredFieldValidator ID="rfvFecha" runat="server" ControlToValidate="txtFecha"
                                    ErrorMessage="La fecha es requerida." CssClass="text-danger small" Display="Dynamic" />
                            </div>
                            <div class="col-md-4">
                                <label class="form-label">Hora<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtHora" runat="server" CssClass="form-control" TextMode="Time" />
                                <asp:RequiredFieldValidator ID="rfvHora" runat="server" ControlToValidate="txtHora"
                                    ErrorMessage="La hora es requerida." CssClass="text-danger small" Display="Dynamic" />
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Cantidad de Personas<span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtCantidadPersonas" runat="server" CssClass="form-control" TextMode="Number" min="1" />
                                <asp:RequiredFieldValidator ID="rfvCantidadPersonas" runat="server" ControlToValidate="txtCantidadPersonas"
                                    ErrorMessage="La cantidad es requerida." CssClass="text-danger small" Display="Dynamic" />
                                <asp:RangeValidator ID="rvCantidadPersonas" runat="server" ControlToValidate="txtCantidadPersonas"
                                    MinimumValue="1" MaximumValue="100" Type="Integer" ErrorMessage="Debe ser entre 1 y 100."
                                    CssClass="text-danger small" Display="Dynamic" />
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Tipo de Mesa Preferido<span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlTipoMesa" runat="server" CssClass="form-select" />
                                <asp:RequiredFieldValidator ID="rfvTipoMesa" runat="server" ControlToValidate="ddlTipoMesa"
                                    ErrorMessage="Debe seleccionar un tipo de mesa." CssClass="text-danger small" Display="Dynamic" InitialValue="0" />
                            </div>
                             <div class="col-12">
                                <label class="form-label">Observaciones (Opcional)</label>
                                <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" 
                                    placeholder="Ej: Necesitamos espacio para una torta, alguna alergia, etc." />
                            </div>
                        </div>
                        
                        <!-- PASO 3: ¿Para quién es la reserva? -->
                        <h5 class="fw-bold text-secondary mb-3">Paso 3: Datos del Cliente</h5>
                        <div class="row g-3 mb-4 p-3 border bg-light rounded-3">
                            <div class="col-md-5">
                                <label class="form-label">DNI del Cliente<span class="text-danger">*</span></label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtDniCliente" runat="server" CssClass="form-control" placeholder="Buscar por DNI..." MaxLength="8" />
                                    <asp:Button ID="btnBuscarCliente" runat="server" Text="Buscar" CssClass="btn btn-outline-secondary" OnClick="btnBuscarCliente_Click" CausesValidation="false" />
                                </div>
                                <asp:RequiredFieldValidator ID="rfvDniCliente" runat="server" ControlToValidate="txtDniCliente"
                                    ErrorMessage="El DNI es requerido." CssClass="text-danger small" Display="Dynamic" />
                            </div>
                            <div class="col-md-7">
                                <label class="form-label">Nombre del Cliente</label>
                                <asp:TextBox ID="txtNombreCliente" runat="server" CssClass="form-control" ReadOnly="true" placeholder="El nombre aparecerá aquí..." />
                                <asp:HiddenField ID="hdnIdCliente" runat="server" />
                            </div>
                        </div>

                        <!-- PASO 4: Verificación de Disponibilidad -->
                        <div class="text-center mb-4">
                            <asp:Label ID="lblDisponibilidad" runat="server" CssClass="fw-bold" Visible="false"></asp:Label>
                            <br />
                            <asp:Button ID="btnVerificarDisponibilidad" runat="server" Text="Verificar Disponibilidad" CssClass="btn btn-warning text-dark mt-2" OnClick="btnVerificarDisponibilidad_Click" />
                        </div>

                        <div class="text-end mt-4">
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary me-2" OnClick="btnCancelar_Click" CausesValidation="false" />
                            <asp:Button ID="btnGuardarReserva" runat="server" Text="Guardar Reserva de Evento" CssClass="btn btn-success" OnClick="btnGuardarReserva_Click" Enabled="false" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>

