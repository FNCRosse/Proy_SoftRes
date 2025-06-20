<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="sede_gestion.aspx.cs" Inherits="SoftResWA.Views.Sedes.SedeGestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Mantenimiento de Sedes
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="text-start mb-3">
        <h1 class="h3 text-danger fw-bold mb-0 me-3">
            <i class="fas fa-gears me-2"></i>Mantenimiento de Sedes
        </h1>
    </div>
    <!-- Filtros para  -->
    <div class="container-fluid mb-4 ps-4">
        <div class="row gx-3">
            <!-- Nombre -->
            <div class="col-auto">
                <label for="txtNombre" class="form-label">Nombre</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ej. Sede Central" />
            </div>
            <!-- Estado -->
            <div class="col-auto">
                <label for="ddlEstado" class="form-label">Estado</label>
                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select" AutoPostBack="False">
                    <asp:ListItem Text="-- Todos --" Value="" Selected="True" />
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
                    OnClick="btnNuevoSede_Click"
                    Style="background-color: #FFF3CD; color: #856404; border: 1px solid #d39e00;" />
            </div>
        </div>
    </div>

    <!-- Tabla -->
    <div class="row">
        <asp:GridView ID="dgvSede" runat="server" AllowPaging="false" AutoGenerateColumns="false"
            CssClass="table table-hover table-responsive table-striped">
            <Columns>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnModificar" runat="server" CssClass="btn btn-sm btn-primary"
                            CommandArgument='<%# Eval("idSede") %>'
                            OnCommand="btnModificar_Command">M</asp:LinkButton>
                        <%# "<button type='button' class='btn btn-sm btn-danger' onclick=\"confirmarEliminacion(" + Eval("idSede") + ", '" + hdnIdEliminar.ClientID + "', '" + btnEliminarSede.ClientID + "')\">C</button>" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Código" DataField="idSede" />
                <asp:BoundField HeaderText="Nombre" DataField="nombre" />
                <asp:BoundField HeaderText="Distrito" DataField="distrito" />
                <asp:BoundField HeaderText="Horarios" DataField="horarios" />
                <asp:BoundField HeaderText="Fecha Creación" DataField="fechaCreacion" />
                <asp:BoundField HeaderText="Usuario Creación" DataField="usuarioCreacion" />
                <asp:BoundField HeaderText="Fecha Modificación" DataField="fechaModificacion" />
                <asp:BoundField HeaderText="Usuario Modificación" DataField="usuarioModificacion" />
                <asp:BoundField HeaderText="Estado" DataField="estado" />
            </Columns>
        </asp:GridView>
        <asp:HiddenField ID="hdnIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarSede" runat="server" Style="display: none;" OnClick="btn_eliminar_Click" />
    </div>
    <!-- Modales -->
    <!-- Modal Registrar Sede -->
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:HiddenField ID="hdnIdSede" runat="server" />
    <asp:HiddenField ID="hdnModoModal" runat="server" />
    <!-- Modal Registrar Sede -->
    <div class="modal fade" id="modalRegistrarSede" tabindex="-1" aria-labelledby="modalRegistrarSedeLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl modal-dialog-centered">
            <div class="modal-content border-warning">
                <div class="modal-header bg-warning-subtle">
                    <h5 class="modal-title fw-bold" id="tituloModalSede">
                        <i class="fas fa-map-marker-alt me-2 text-danger"></i>Registrar Sede
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <!-- Información general -->
                    <div class="border p-3 mb-3">
                        <h6 class="fw-bold mb-3">Información General</h6>
                        <div class="row g-3">
                            <div class="col-md-6">
                                <label for="txtNombreSede" class="form-label">Nombre</label>
                                <asp:TextBox ID="txtNombreSede" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-md-6">
                                <label for="txtDistritoSede" class="form-label">Distrito</label>
                                <asp:TextBox ID="txtDistritoSede" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                    </div>

                    <!-- Información del horario -->
                    <div class="row">
                        <div class="col-md-6">
                            <div class="border p-3">
                                <h6 class="fw-bold mb-3">Información del Horario</h6>

                                <!-- ID + Buscar -->
                                <div class="mb-3">
                                    <label for="ddlHorarios" class="form-label">Seleccionar Horario</label>
                                    <asp:DropDownList ID="ddlHorarios" runat="server" CssClass="form-select"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlHorarios_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>

                                <!-- Campos visuales -->
                                <div class="mb-2">
                                    <label class="form-label">Día de la Semana</label>
                                    <asp:Label ID="lblDiaSemana" runat="server" CssClass="form-control border bg-light" />
                                </div>
                                <div class="mb-2 row">
                                    <div class="col">
                                        <label class="form-label">Hora Inicio</label>
                                        <asp:Label ID="lblHoraInicio" runat="server" CssClass="form-control border bg-light" />
                                    </div>
                                    <div class="col">
                                        <label class="form-label">Hora Fin</label>
                                        <asp:Label ID="lblHoraFin" runat="server" CssClass="form-control border bg-light" />
                                    </div>
                                </div>
                                <div class="mb-2">
                                    <label class="form-label">Feriado</label>
                                    <asp:Label ID="lblFeriado" runat="server" CssClass="form-control border bg-light" />
                                </div>

                                <!-- Botón Añadir -->
                                <div class="text-end">
                                    <asp:Button ID="btnAñadirHorario" runat="server" Text="Añadir"
                                        CssClass="btn btn-warning"
                                        UseSubmitBehavior="false"
                                        OnClick="btnAñadirHorario_Click" />
                                </div>
                            </div>
                        </div>

                        <!-- Detalle horarios -->
                        <div class="col-md-6">
                            <div class="border p-3">
                                <h6 class="fw-bold mb-3">Detalle de Horarios</h6>
                                <asp:GridView ID="gvDetalleHorario" runat="server" AutoGenerateColumns="false"
                                    CssClass="table table-sm table-bordered text-center">
                                    <Columns>
                                        <asp:BoundField HeaderText="ID" DataField="idHorario" />
                                        <asp:BoundField HeaderText="Día Semana" DataField="diaSemana" />
                                        <asp:BoundField HeaderText="Hora Inicio" DataField="horaInicio" />
                                        <asp:BoundField HeaderText="Hora Fin" DataField="horaFin" />
                                        <asp:BoundField HeaderText="Es Feriado?" DataField="feriadoTexto" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Footer -->
                <div class="modal-footer">
                    <asp:Button ID="btnGuardarSede" runat="server" Text="Guardar" CssClass="btn btn-danger fw-bold "
                        OnClick="btnGuardarSede_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
