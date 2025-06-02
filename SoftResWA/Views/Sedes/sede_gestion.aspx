<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="sede_gestion.aspx.cs" Inherits="SoftResWA.Views.Sedes.WebForm1" %>

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
                <input type="text" id="txtNombre" class="form-control" placeholder="Ej. Sede Central" />
            </div>
            <!-- Distrito -->
            <div class="col-auto">
                <label for="txtDistrito" class="form-label">Distrito</label>
                <input type="text" id="txtDistrito" class="form-control" placeholder="Ej. San Miguel" />
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
                    data-bs-toggle="modal" data-bs-target="#modalRegistrarSede">
                    <i class="fas fa-plus me-2"></i>Nuevo
                </button>
            </div>
        </div>
    </div>

    <!-- Tabla -->
    <div class="row">
        <asp:GridView ID="dgvSede" runat="server" AllowPaging="false" AutoGenerateColumns="false" OnRowCommand="dgvSede_RowCommand"
            CssClass="table table-hover table-responsive table-striped">
            <Columns>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:Button runat="server" Text="M" CssClass="btn btn-sm btn-primary"
                            CommandName="Modificar"
                            CommandArgument='<%# Eval("SedeId") %>' />
                        <button type="button" class="btn btn-sm btn-danger" onclick="confirmarCancelacion('<%# Eval("IdSede") %>')">C</button>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Código" DataField="SedeId" />
                <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
                <asp:BoundField HeaderText="Distrito" DataField="Distrito" />
                <asp:BoundField HeaderText="Horario" DataField="Horario" />
                <asp:BoundField HeaderText="Fecha Creacion" DataField="FechaCrea" />
                <asp:BoundField HeaderText="Usuario Creacion" DataField="UsuarioCrea" />
                <asp:BoundField HeaderText="Fecha Modificacion" DataField="FechaMod" />
                <asp:BoundField HeaderText="Usuario Modificacion" DataField="UsuarioMod" />
                <asp:BoundField HeaderText="Estado" DataField="Estado" />
            </Columns>
        </asp:GridView>
    </div>
    <!-- Modales -->
    <!-- Modal Registrar Sede -->
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <!-- Modal Registrar Sede -->
    <div class="modal fade" id="modalRegistrarSede" tabindex="-1" aria-labelledby="modalRegistrarSedeLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl modal-dialog-centered">
            <div class="modal-content border-warning">
                <div class="modal-header bg-warning-subtle">
                    <h5 class="modal-title fw-bold" id="modalRegistrarSedeLabel">
                        <i class="fas fa-map-marker-alt me-2 text-danger"></i>Registrar Sede
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hdnIdSede" runat="server" />
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
                                    <label for="txtIdHorario" class="form-label">ID del Horario</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtIdHorario" runat="server" CssClass="form-control" />
                                        <asp:Button ID="btnBuscarHorario" runat="server" Text="Buscar"
                                            CssClass="btn btn-secondary"
                                            UseSubmitBehavior="false"
                                            OnClick="btnBuscarHorario_Click" />
                                    </div>
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
                                        <asp:BoundField HeaderText="ID" DataField="IdHorario" />
                                        <asp:BoundField HeaderText="Día Semana" DataField="DiaSemana" />
                                        <asp:BoundField HeaderText="Hora Inicio" DataField="HoraInicio" />
                                        <asp:BoundField HeaderText="Hora Fin" DataField="HoraFin" />
                                        <asp:BoundField HeaderText="Feriado" DataField="FeriadoTexto" />
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
