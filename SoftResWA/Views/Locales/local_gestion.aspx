<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="local_gestion.aspx.cs" Inherits="SoftResWA.Views.Locales.LocalGestion" %>

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
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ej. Local San Miguel" />
            </div>
            <!-- Sede -->
            <div class="col-auto">
                <label for="ddlSede" class="form-label">Sede</label>
                <asp:DropDownList ID="ddlSede" runat="server" CssClass="form-select"
                    AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <!-- Estado -->
            <div class="col-auto">
                <label for="ddlEstado" class="form-label">Estado</label>
                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                    <asp:ListItem Text="Activo" Value="1" />
                    <asp:ListItem Text="Inactivo" Value="0" />
                </asp:DropDownList>
            </div>
            <!-- Botones -->
            <div class="col-auto d-flex align-items-end">
                <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-danger me-2"
                    Text="Buscar"
                    OnClick="btnBuscar_Click" />
                <asp:Button ID="btnNuevoSede" runat="server" CssClass="btn shadow-sm"
                    Text="Nuevo"
                    OnClick="btnNuevo_Click"
                    Style="background-color: #FFF3CD; color: #856404; border: 1px solid #d39e00;" />
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
                        <asp:LinkButton ID="btnModificar" runat="server" CssClass="btn btn-sm btn-primary"
                            CommandArgument='<%# Eval("idLocal") %>'
                            OnCommand="btnModificar_Command">M</asp:LinkButton>
                        <%# "<button type='button' class='btn btn-sm btn-danger' onclick=\"confirmarEliminacion(" + Eval("idLocal") + ", '" + hdnIdEliminar.ClientID + "', '" + btnEliminarLocal.ClientID + "')\">C</button>" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Código" DataField="idLocal" />
                <asp:BoundField HeaderText="Nombre" DataField="nombre" />
                <asp:BoundField HeaderText="Dirección" DataField="direccion" />
                <asp:BoundField HeaderText="Sede" DataField="sedeNombre" />
                <asp:BoundField HeaderText="Teléfono" DataField="telefono" />
                <asp:BoundField HeaderText="Cantidad Mesas" DataField="cantidadMesas" />
                <asp:BoundField HeaderText="Fecha Creacion" DataField="fechaCreacion" />
                <asp:BoundField HeaderText="Usuario Creacion" DataField="usuarioCreacion" />
                <asp:BoundField HeaderText="Fecha Modificacion" DataField="fechaModificacion" />
                <asp:BoundField HeaderText="Usuario Modificacion" DataField="usuarioModificacion" />
                <asp:BoundField HeaderText="Estado" DataField="estado" />
            </Columns>
        </asp:GridView>
        <asp:HiddenField ID="hdnIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarLocal" runat="server" Style="display: none;" OnClick="btn_eliminar_Click" />
    </div>
    <!-- Modales -->
    <!-- Modal Registrar Local -->
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:HiddenField ID="hdnIdLocal" runat="server" />
    <asp:HiddenField ID="hdnModoModal" runat="server" />
    <div class="modal fade" id="modalRegistrarLocal" tabindex="-1" aria-labelledby="modalRegistrarLocalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-scrollable" role="document">
            <div class="modal-content">
                <div class="modal-header bg-warning ">
                    <h5 class="modal-title fw-bold" id="tituloModal">
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
                    </div>
                </div>

                <div class="modal-footer">
                    <asp:Button ID="btnGuardarLocal" runat="server" Text="Guardar" CssClass="btn btn-warning fw-bold text-dark" OnClick="btnGuardarLocal_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

