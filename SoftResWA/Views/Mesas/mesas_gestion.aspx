<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="mesas_gestion.aspx.cs" Inherits="SoftResWA.Views.Mesas.mesas_gestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Mantenimiento de Mesas
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="text-start mb-3">
        <h1 class="h3 text-danger fw-bold mb-0 me-3">
            <i class="fas fa-gears me-2"></i>Mantenimiento de Mesas
        </h1>
    </div>
    <!-- Filtros para  -->
    <div class="container-fluid mb-4 ps-4">
        <div class="row gx-3">
            <!-- Numero de mesa -->
            <div class="col-auto">
                <label for="txtNombre" class="form-label">Número de mesa</label>
                <input type="text" id="txtNombre" class="form-control" placeholder="Ej. Mesa 12" />
            </div>
            <!-- Ubicacion -->
            <div class="col-auto">
                <label for="ddlUbi" class="form-label">Ubicacion</label>
                <select id="ddlUbi" class="form-select">
                    <option selected disabled>Seleccionar...</option>
                    <option value="1">1er Piso</option>
                    <option value="0">2do Piso</option>
                    <option value="0">Balcon</option>
                </select>
            </div>
            <!-- Local -->
            <div class="col-auto">
                <label for="ddlLocal" class="form-label">Local</label>
                <select id="ddlSddlLocalede" class="form-select">
                    <option selected disabled>Seleccionar...</option>
                    <option value="1">Local San Miguel</option>
                    <option value="0">Local Callao</option>
                </select>
            </div>
            <!-- Estado -->
            <div class="col-auto">
                <label for="ddlEstado" class="form-label">Estado</label>
                <select id="ddlEstado" class="form-select">
                    <option selected disabled>Seleccionar...</option>
                    <option value="1">Disponible</option>
                    <option value="0">Reservada</option>
                    <option value="0">En Uso</option>
                    <option value="0">En Mantenimiento</option>
                    <option value="0">Desechada</option>
                </select>
            </div>
            <!-- Botones -->
            <div class="col-auto d-flex align-items-end">
                <button type="button" class="btn btn-danger me-2">
                    <i class="fas fa-search me-1"></i>Buscar
                </button>
                <button type="button" class="btn shadow-sm"
                    style="background-color: #FFF3CD; color: #856404; border: 1px solid #d39e00;"
                    data-bs-toggle="modal" data-bs-target="#modalRegistrarMesa">
                    <i class="fas fa-plus me-2"></i>Nuevo
                </button>
            </div>
        </div>
    </div>

    <!-- Tabla -->
    <div class="row">
        <asp:GridView ID="dgvLocal" runat="server" AllowPaging="false" AutoGenerateColumns="false"
            CssClass="table table-hover table-responsive table-striped">
            <Columns>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <button type="button" class="btn btn-sm btn-primary" onclick="modificarReserva('<%# Eval("IdMesa") %>')">M</button>
                        <button type="button" class="btn btn-sm btn-danger" onclick="confirmarCancelacion('<%# Eval("IdMesa") %>')">C</button>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Código" DataField="SedeId" />
                <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
                <asp:BoundField HeaderText="Dirección" DataField="Direccion" />
                <asp:BoundField HeaderText="Sede" DataField="Sede" />
                <asp:BoundField HeaderText="Teléfono" DataField="Telefono" />
                <asp:BoundField HeaderText="Cantidad Mesas" DataField="CantMesas" />
                <asp:BoundField HeaderText="Fecha Creacion" DataField="FechaCrea" />
                <asp:BoundField HeaderText="Usuario Creacion" DataField="UsuarioCrea" />
                <asp:BoundField HeaderText="Fecha Modificacion" DataField="FechaMod" />
                <asp:BoundField HeaderText="Usuario Modificacion" DataField="UsuarioMod" />
                <asp:BoundField HeaderText="Estado" DataField="Estado" />
            </Columns>
        </asp:GridView>
    </div>
    <!-- Modales -->
    <!-- Modal Registrar Mesa -->
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="modal fade" id="modalRegistrarMesa" tabindex="-1" role="dialog" aria-labelledby="modalRegistrarMesaLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-scrollable" role="document">
            <div class="modal-content">
                <div class="modal-header bg-warning">
                    <h5 class="modal-title fw-bold" id="modalRegistrarMesaLabel">
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
                                <label for="txtNumeroMesa" class="form-label">Número de mesa</label>
                                <asp:TextBox ID="txtNumeroMesa" runat="server" CssClass="form-control" />
                            </div>

                            <div class="row mb-2">
                                <div class="col-md-6">
                                    <label for="txtPosX" class="form-label">Posición X</label>
                                    <asp:TextBox ID="txtPosX" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-md-6">
                                    <label for="txtPosY" class="form-label">Posición Y</label>
                                    <asp:TextBox ID="txtPosY" runat="server" CssClass="form-control" />
                                </div>
                            </div>

                            <div class="mb-2">
                                <label for="txtCapacidad" class="form-label">Capacidad</label>
                                <asp:TextBox ID="txtCapacidad" runat="server" CssClass="form-control" />
                            </div>

                            <div class="mb-2">
                                <label for="ddlEstado" class="form-label">Estado</label>
                                <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Seleccionar..." Value="" />
                                    <asp:ListItem Text="Disponible" Value="Disponible" />
                                    <asp:ListItem Text="Reservada" Value="Reservada" />
                                    <asp:ListItem Text="En Uso" Value="En Uso" />
                                    <asp:ListItem Text="En Mantenimiento" Value="En Mantenimiento" />
                                    <asp:ListItem Text="Desechada" Value="Desechada" />
                                </asp:DropDownList>
                            </div>
                        </div>

                        <!-- Ubicación de mesa + Local -->
                        <div class="col-md-6">
                            <!-- Ubicación de la mesa -->
                            <div class="border rounded p-3 mb-3">
                                <h6 class="fw-bold mb-3">Información de la ubicación de mesa</h6>
                                <div class="mb-2">
                                    <label for="txtIdTipoMesa" class="form-label">Id del tipo de mesa</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtIdTipoMesa" runat="server" CssClass="form-control" />
                                        <asp:Button ID="btnBuscarTipoMesa" runat="server" Text="buscar" CssClass="btn btn-outline-secondary" UseSubmitBehavior="false" />
                                    </div>
                                </div>
                                <div class="mb-2">
                                    <label for="txtNombreTipoMesa" class="form-label">Nombre</label>
                                    <asp:TextBox ID="txtNombreTipoMesa" runat="server" CssClass="form-control" ReadOnly="true" />
                                </div>
                            </div>

                            <!-- Información del Local -->
                            <div class="border rounded p-3">
                                <h6 class="fw-bold mb-3">Información del local</h6>
                                <div class="mb-2">
                                    <label for="txtIdLocal" class="form-label">Id del local</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtIdLocal" runat="server" CssClass="form-control" />
                                        <asp:Button ID="btnBuscarLocal" runat="server" Text="buscar" CssClass="btn btn-outline-secondary" UseSubmitBehavior="false" />
                                    </div>
                                </div>
                                <div class="mb-2">
                                    <label for="txtNombreLocal" class="form-label">Nombre</label>
                                    <asp:TextBox ID="txtNombreLocal" runat="server" CssClass="form-control" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <asp:Button ID="btnGuardarMesa" runat="server" Text="Guardar" CssClass="btn btn-warning fw-bold text-dark" OnClick="btnGuardarMesa_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
