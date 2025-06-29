<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="mesas_gestion.aspx.cs" Inherits="SoftResWA.Views.Mesas.mesas_gestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Mantenimiento de Mesas
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="text-start mb-3">
        <h1 class="h3 text-danger fw-bold mb-0 me-3">
            <i class="fas fa-chair me-2"></i>Mantenimiento de Mesas
        </h1>
    </div>

    <!-- Filtros para búsqueda -->
    <div class="container-fluid mb-4 ps-4">
        <div class="row gx-3">
            <!-- Numero de mesa -->
            <div class="col-auto">
                <label for="txtNumeroMesaFiltro" class="form-label">Número de mesa</label>
                <asp:TextBox ID="txtNumeroMesaFiltro" runat="server" CssClass="form-control" placeholder="Ej. M-101" />
            </div>
            <!-- Ubicacion de Mesa -->
            <div class="col-auto">
                <label for="ddlTipoMesaFiltro" class="form-label">Tipo de Mesa</label>
                <asp:DropDownList ID="ddlTipoMesaFiltro" runat="server" CssClass="form-select">
                </asp:DropDownList>
            </div>
            <!-- Local -->
            <div class="col-auto">
                <label for="ddlLocalFiltro" class="form-label">Local</label>
                <asp:DropDownList ID="ddlLocalFiltro" runat="server" CssClass="form-select">
                </asp:DropDownList>
            </div>
            <!-- Estado -->
            <div class="col-auto">
                <label for="ddlEstadoFiltro" class="form-label">Estado</label>
                <asp:DropDownList ID="ddlEstadoFiltro" runat="server" CssClass="form-select">
                </asp:DropDownList>
            </div>
            <!-- Botones -->
            <div class="col-auto d-flex align-items-end">
                <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-danger me-2"
                    Text="Buscar"
                    CausesValidation="false"
                    OnClick="btnBuscar_Click" />
                <asp:Button ID="btnNuevo" runat="server" CssClass="btn shadow-sm"
                    Text="Nuevo"
                    CausesValidation="false"
                    OnClick="btnNuevo_Click"
                    Style="background-color: #FFF3CD; color: #856404; border: 1px solid #d39e00;" />
            </div>
        </div>
    </div>

    <!-- Tabla -->
    <div class="row">
        <asp:GridView ID="dgvMesas" runat="server" AllowPaging="false" AutoGenerateColumns="false"
            CssClass="table table-hover table-responsive table-striped">
            <Columns>
                <asp:TemplateField HeaderText="Opciones">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnModificar" runat="server" CssClass="btn btn-sm btn-primary"
                            Text="M"
                            CommandArgument='<%# Eval("idMesa") %>'
                            OnCommand="btnModificar_Command" />

                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-sm btn-danger"
                            Text="C" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="ID" DataField="idMesa" />
                <asp:BoundField HeaderText="Número Mesa" DataField="numeroMesa" />
                <asp:BoundField HeaderText="Capacidad" DataField="capacidad" />
                <asp:BoundField HeaderText="Estado" DataField="estado" />
                <asp:BoundField HeaderText="Pos X" DataField="x" />
                <asp:BoundField HeaderText="Pos Y" DataField="y" />
                <asp:BoundField HeaderText="Tipo Mesa" DataField="ubicacionMesa" />
                <asp:BoundField HeaderText="Local" DataField="local_nombre" />
                <asp:BoundField HeaderText="Fecha Creación" DataField="fechaCreacion" />
                <asp:BoundField HeaderText="Usuario Creación" DataField="usuarioCreacion" />
                <asp:BoundField HeaderText="Fecha Modificación" DataField="fechaModificacion" />
                <asp:BoundField HeaderText="Usuario Modificación" DataField="usuarioModificacion" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- Controles ocultos -->
    <asp:HiddenField ID="hdnModoModal" runat="server" />
    <asp:HiddenField ID="hdnIdMesa" runat="server" />
    <asp:HiddenField ID="hdnIdEliminar" runat="server" />
    <asp:Button ID="btnEliminarMesa" runat="server" Style="display: none;" OnClick="btnEliminar_Click" />

    <!-- Modal Registrar/Modificar Mesa -->
    <div class="modal fade" id="modalRegistrarMesa" tabindex="-1" role="dialog" aria-labelledby="modalRegistrarMesaLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-scrollable" role="document">
            <div class="modal-content">
                <div class="modal-header bg-warning">
                    <h5 class="modal-title fw-bold" id="tituloModal">
                        <i class="fas fa-chair me-2 text-danger"></i>Registrar Mesa
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>

                <div class="modal-body">
                    <div class="row mb-3">
                        <!-- Información General -->
                        <div class="col-md-6 border rounded p-3">
                            <h6 class="fw-bold mb-3">Información General</h6>

                            <div class="mb-2">
                                <label for="txtNumeroMesa" class="form-label">Número de mesa <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtNumeroMesa" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="rfvNumeroMesa" runat="server"
                                    ControlToValidate="txtNumeroMesa" ErrorMessage="Número de mesa es requerido"
                                    CssClass="text-danger" Display="Dynamic" />
                            </div>

                            <div class="row mb-2">
                                <div class="col-md-6">
                                    <label for="txtPosX" class="form-label">Posición X <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtPosX" runat="server" CssClass="form-control" TextMode="Number" />
                                    <asp:RequiredFieldValidator ID="rfvPosX" runat="server"
                                        ControlToValidate="txtPosX" ErrorMessage="Posición X es requerida"
                                        CssClass="text-danger" Display="Dynamic" />
                                    <asp:RangeValidator ID="rvPosX" runat="server"
                                        ControlToValidate="txtPosX" MinimumValue="0" MaximumValue="999999"
                                        Type="Double" ErrorMessage="El X no puede ser negativo"
                                        CssClass="text-danger" Display="Dynamic" />
                                </div>
                                <div class="col-md-6">
                                    <label for="txtPosY" class="form-label">Posición Y <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtPosY" runat="server" CssClass="form-control" TextMode="Number" />
                                    <asp:RequiredFieldValidator ID="rfvPosY" runat="server"
                                        ControlToValidate="txtPosY" ErrorMessage="Posición Y es requerida"
                                        CssClass="text-danger" Display="Dynamic" />
                                    <asp:RangeValidator ID="rvPosY" runat="server"
                                        ControlToValidate="txtPosY" MinimumValue="0" MaximumValue="999999"
                                        Type="Double" ErrorMessage="El Y no puede ser negativo"
                                        CssClass="text-danger" Display="Dynamic" />
                                </div>
                            </div>

                            <div class="mb-2">
                                <label for="txtCapacidad" class="form-label">Capacidad <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtCapacidad" runat="server" CssClass="form-control" TextMode="Number" />
                                <asp:RequiredFieldValidator ID="rfvCapacidad" runat="server"
                                    ControlToValidate="txtCapacidad" ErrorMessage="Capacidad es requerida"
                                    CssClass="text-danger" Display="Dynamic" />
                            </div>

                            <div class="mb-2">
                                <label for="ddlEstadoMesa" class="form-label">Estado <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlEstadoMesa" runat="server" CssClass="form-select">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvEstado" runat="server"
                                    ControlToValidate="ddlEstadoMesa" ErrorMessage="Estado es requerido"
                                    CssClass="text-danger" Display="Dynamic" InitialValue="" />
                            </div>
                        </div>

                        <!-- Tipo de Mesa y Local -->
                        <div class="col-md-6">
                            <!-- Tipo de Mesa -->
                            <div class="border rounded p-3 mb-3">
                                <h6 class="fw-bold mb-3">Tipo de Mesa</h6>
                                <div class="mb-2">
                                    <label for="ddlTipoMesa" class="form-label">Tipo de Mesa <span class="text-danger">*</span></label>
                                    <asp:DropDownList ID="ddlTipoMesa" runat="server" CssClass="form-select">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvTipoMesa" runat="server"
                                        ControlToValidate="ddlTipoMesa" ErrorMessage="Tipo de mesa es requerido"
                                        CssClass="text-danger" Display="Dynamic" InitialValue="" />
                                </div>
                            </div>

                            <!-- Local -->
                            <div class="border rounded p-3">
                                <h6 class="fw-bold mb-3">Local</h6>
                                <div class="mb-2">
                                    <label for="ddlLocal" class="form-label">Local <span class="text-danger">*</span></label>
                                    <asp:DropDownList ID="ddlLocal" runat="server" CssClass="form-select">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvLocal" runat="server"
                                        ControlToValidate="ddlLocal" ErrorMessage="Local es requerido"
                                        CssClass="text-danger" Display="Dynamic" InitialValue="" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnGuardarMesa" runat="server" Text="Guardar"
                        CssClass="btn btn-warning fw-bold text-dark" OnClick="btnGuardarMesa_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
