<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="local_gestion.aspx.cs" Inherits="SoftResWA.Views.Locales.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Mantenimiento de Locales
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
            <!-- Nombre -->
            <div class="col-auto">
                <label for="txtNombre" class="form-label">Nombre</label>
                <input type="text" id="txtNombre" class="form-control" placeholder="Ej. Local San Miguel" />
            </div>
            <!-- Sede -->
            <div class="col-auto">
                <label for="ddlSede" class="form-label">Sede</label>
                <select id="ddlSede" class="form-select">
                    <option selected disabled>Seleccionar...</option>
                    <option value="1">Sede Callao</option>
                    <option value="0">Sede Lima Metropolitana</option>
                    <option value="0">Sede Ica</option>
                </select>
            </div>
            <!-- Estado -->
            <div class="col-auto">
                <label for="ddlEstado" class="form-label">Estado</label>
                <select id="ddlEstado" class="form-select">
                    <option selected disabled>Seleccionar...</option>
                    <option value="1">Activo</option>
                    <option value="0">Inactivo</option>
                </select>
            </div>
            <!-- Botones -->
            <div class="col-auto d-flex align-items-end">
                <button type="button" class="btn btn-danger me-2">
                    <i class="fas fa-search me-1"></i>Buscar
                </button>
                <button type="button" class="btn shadow-sm"
                    style="background-color: #FFF3CD; color: #856404; border: 1px solid #d39e00;"
                    data-bs-toggle="modal" data-bs-target="#modalRegistrarLocal">
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
                        <button type="button" class="btn btn-sm btn-primary" onclick="modificarReserva('<%# Eval("IdLocal") %>')">M</button>
                        <button type="button" class="btn btn-sm btn-danger" onclick="confirmarCancelacion('<%# Eval("IdLocal") %>')">C</button>
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
    <!-- Modal Registrar Local -->
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="modal fade" id="modalRegistrarLocal" tabindex="-1" role="dialog" aria-labelledby="modalRegistrarLocalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-scrollable" role="document">
            <div class="modal-content">
                <div class="modal-header bg-warning ">
                    <h5 class="modal-title fw-bold" id="modalRegistrarLocalLabel">
                        <i class="fas fa-map-marker-alt me-2 text-danger"></i>Registrar Local
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <!-- Información General -->
                    <div class="row mb-3">
                        <div class="col-md-6 border rounded p-3">
                            <h6 class="fw-bold mb-3">Información General</h6>
                            <div class="mb-2">
                                <label for="txtNombreLocal" class="form-label">Nombre</label>
                                <asp:TextBox ID="txtNombreLocal" runat="server" CssClass="form-control" />
                            </div>
                            <div class="mb-2">
                                <label for="txtDireccionLocal" class="form-label">Dirección</label>
                                <asp:TextBox ID="txtDireccionLocal" runat="server" CssClass="form-control" />
                            </div>
                            <div class="row">
                                <div class="col">
                                    <label for="txtTelefonoLocal" class="form-label">Teléfono</label>
                                    <asp:TextBox ID="txtTelefonoLocal" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>

                        <!-- Detalles de Mesas -->
                        <div class="col-md-6 border rounded p-3">
                            <h6 class="fw-bold mb-3">Detalles de mesas</h6>
                            <asp:GridView ID="gvMesas" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="ID" />
                                    <asp:BoundField DataField="Numero" HeaderText="Número" />
                                    <asp:BoundField DataField="Capacidad" HeaderText="Capacidad" />
                                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                    <asp:BoundField DataField="Ubicacion" HeaderText="Ubicación" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                    <!-- Información de la Mesa -->
                    <div class="row">
                        <div class="col-md-6 border rounded p-3">
                            <h6 class="fw-bold mb-3">Información de la mesa</h6>

                            <div class="mb-2">
                                <label for="txtIdMesa" class="form-label">ID de la Mesa</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtIdMesa" runat="server" CssClass="form-control" />
                                    <asp:Button ID="btnBuscarMesa" runat="server" Text="Buscar" CssClass="btn btn-secondary" UseSubmitBehavior="false" OnClick="btnBuscarMesa_Click" />
                                </div>
                            </div>
                            <div class="mb-2">
                                <label for="txtNumeroMesa" class="form-label">Número</label>
                                <asp:TextBox ID="txtNumeroMesa" runat="server" CssClass="form-control" />
                            </div>
                            <div class="mb-2">
                                <label for="txtCapacidadMesa" class="form-label">Capacidad</label>
                                <asp:TextBox ID="txtCapacidadMesa" runat="server" CssClass="form-control" />
                            </div>
                            <div class="mb-2">
                                <label for="txtEstadoMesa" class="form-label">Estado</label>
                                <asp:TextBox ID="txtEstadoMesa" runat="server" CssClass="form-control" />
                            </div>
                            <div class="mb-2">
                                <label for="txtUbicacionMesa" class="form-label">Ubicación</label>
                                <asp:TextBox ID="txtUbicacionMesa" runat="server" CssClass="form-control" />
                            </div>

                            <asp:Button ID="btnAñadirMesa" runat="server" Text="Añadir" CssClass="btn btn-outline-success mt-2" UseSubmitBehavior="false" OnClick="btnAñadirMesa_Click" />
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <asp:Button ID="btnGuardarLocal" runat="server" Text="Guardar" CssClass="btn btn-warning fw-bold text-dark" OnClick="btnGuardarLocal_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

