<%@ Page Title="" Language="C#" MasterPageFile="~/SoftResCliente.Master" AutoEventWireup="true" CodeBehind="Reg_Resev_Comun.aspx.cs" Inherits="SoftResWA.Views.Cliente.Reservas.Reg_Resev_Comun" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Reserva Común
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-4">
        <h2 class="text-center fw-bold text-danger mb-4">Reserva Común</h2>
        
        <div class="row">
            <div class="col-md-8 mx-auto">
                <div class="card shadow-sm">
                    <div class="card-body p-4">
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label for="ddlLocal" class="form-label">Local</label>
                                <asp:DropDownList ID="ddlLocal" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlLocal_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvLocal" runat="server" 
                                    ControlToValidate="ddlLocal"
                                    InitialValue=""
                                    ErrorMessage="Por favor seleccione un local"
                                    CssClass="text-danger"
                                    Display="Dynamic" />
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label for="txtFecha" class="form-label">Fecha</label>
                                <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date" AutoPostBack="true" OnTextChanged="txtFecha_TextChanged" />
                                <asp:RequiredFieldValidator ID="rfvFecha" runat="server"
                                    ControlToValidate="txtFecha"
                                    ErrorMessage="Por favor seleccione una fecha"
                                    CssClass="text-danger"
                                    Display="Dynamic" />
                            </div>
                            <div class="col-md-6">
                                <label for="txtHora" class="form-label">Hora</label>
                                <asp:TextBox ID="txtHora" runat="server" CssClass="form-control" TextMode="Time" AutoPostBack="true" OnTextChanged="txtHora_TextChanged" />
                                <asp:RequiredFieldValidator ID="rfvHora" runat="server"
                                    ControlToValidate="txtHora"
                                    ErrorMessage="Por favor seleccione una hora"
                                    CssClass="text-danger"
                                    Display="Dynamic" />
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label for="ddlCantidadPersonas" class="form-label">Cantidad de Personas</label>
                                <asp:DropDownList ID="ddlCantidadPersonas" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCantidadPersonas_SelectedIndexChanged">
                                    <asp:ListItem Text="Seleccione" Value="" />
                                    <asp:ListItem Text="1" Value="1" />
                                    <asp:ListItem Text="2" Value="2" />
                                    <asp:ListItem Text="3" Value="3" />
                                    <asp:ListItem Text="4" Value="4" />
                                    <asp:ListItem Text="5" Value="5" />
                                    <asp:ListItem Text="6" Value="6" />
                                    <asp:ListItem Text="7" Value="7" />
                                    <asp:ListItem Text="8" Value="8" />
                                    <asp:ListItem Text="9" Value="9" />
                                    <asp:ListItem Text="10" Value="10" />
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvCantidadPersonas" runat="server"
                                    ControlToValidate="ddlCantidadPersonas"
                                    InitialValue=""
                                    ErrorMessage="Por favor seleccione la cantidad de personas"
                                    CssClass="text-danger"
                                    Display="Dynamic" />
                            </div>
                        </div>

                        <!-- Panel de Disponibilidad -->
                        <div class="row mb-4">
                            <div class="col-12">
                                <div class="card bg-light">
                                    <div class="card-body">
                                        <h5 class="card-title mb-3">Disponibilidad</h5>
                                        <asp:UpdatePanel ID="upDisponibilidad" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlDisponibilidad" runat="server" CssClass="mb-3">
                                                    <div class="d-flex align-items-center mb-2">
                                                        <i class="fas fa-info-circle text-primary me-2"></i>
                                                        <asp:Label ID="lblDisponibilidad" runat="server" CssClass="mb-0" />
                                                    </div>
                                                    <asp:Panel ID="pnlMesasDisponibles" runat="server" Visible="false">
                                                        <div class="alert alert-success mb-0">
                                                            <i class="fas fa-check-circle me-2"></i>
                                                            <asp:Label ID="lblMesasDisponibles" runat="server" />
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlSinDisponibilidad" runat="server" Visible="false">
                                                        <div class="alert alert-warning mb-0">
                                                            <i class="fas fa-exclamation-triangle me-2"></i>
                                                            No hay mesas disponibles para la fecha y hora seleccionadas.
                                                            <asp:LinkButton ID="lnkUnirseEspera" runat="server" CssClass="alert-link" OnClick="btnUnirseEspera_Click">
                                                                Unirse a la lista de espera
                                                            </asp:LinkButton>
                                                        </div>
                                                    </asp:Panel>
                                                </asp:Panel>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlLocal" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="txtFecha" EventName="TextChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="txtHora" EventName="TextChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlCantidadPersonas" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label for="ddlUbicacion" class="form-label">Ubicación de Preferencia</label>
                                <asp:DropDownList ID="ddlUbicacion" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Seleccione" Value="" />
                                    <asp:ListItem Text="Interior" Value="Interior" />
                                    <asp:ListItem Text="Exterior" Value="Exterior" />
                                    <asp:ListItem Text="Cerca a la ventana" Value="Ventana" />
                                    <asp:ListItem Text="Cerca a la entrada" Value="Entrada" />
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="mb-3">
                            <label for="txtObservaciones" class="form-label">Observaciones</label>
                            <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                        </div>

                        <div class="text-end">
                            <asp:Button ID="btnRegistrarReserva" runat="server" Text="Registrar Reserva" 
                                CssClass="btn btn-danger px-4" OnClick="btnRegistrarReserva_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Lista de Espera -->
    <div class="modal fade" id="modalListaEspera" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Lista de Espera</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <p>Lo sentimos, no hay mesas disponibles para la fecha y hora seleccionadas.</p>
                    <p>¿Deseas unirte a la lista de espera? Te notificaremos si se libera una mesa.</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnUnirseEspera" runat="server" Text="Unirme a la Lista" 
                        CssClass="btn btn-danger" OnClick="btnUnirseEspera_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

