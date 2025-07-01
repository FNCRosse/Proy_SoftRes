<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="horario_gestion.aspx.cs" Inherits="SoftResWA.Views.Horarios.horario_gestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Mantenimiento de Horarios
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="text-start mb-3">
        <h1 class="h3 text-danger fw-bold mb-0 me-3">
            <i class="fas fa-gears me-2"></i>Mantenimiento de Horarios
        </h1>
    </div>
    <!-- Filtros para  -->
    <div class="container-fluid mb-4 ps-4">
        <div class="row gx-3">
            <div class="col-auto">
                <label for="ddlDia" class="form-label">Dia de la Semana</label>
                <asp:DropDownList ID="ddlDia" runat="server" CssClass="form-select" />
            </div>
            <!-- Es Feriano -->
            <div class="col-auto">
                <label for="ddlFeriado" class="form-label">Es Feriado</label>
                <asp:DropDownList ID="ddlFeriado" runat="server" CssClass="form-select" AutoPostBack="False">
                    <asp:ListItem Text="-- Todos --" Value="" Selected="True" />
                    <asp:ListItem Text="Si" Value="1" />
                    <asp:ListItem Text="No" Value="0" />
                </asp:DropDownList>
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
                <div class="col-auto d-flex align-items-end">
                    <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-danger me-2"
                        Text="Buscar"
                        OnClick="btnBuscar_Click" />
                    <asp:Button ID="btnNuevoHorario" runat="server" CssClass="btn shadow-sm"
                        Text="Nuevo"
                        OnClick="btnNuevo_Click"
                        Style="background-color: #FFF3CD; color: #856404; border: 1px solid #d39e00;" />
                </div>
            </div>
        </div>
    </div>

    <!-- Tabla -->
    <div class="row">
        <asp:GridView ID="dgvHorario" runat="server" AllowPaging="false" AutoGenerateColumns="false"
            CssClass="table table-hover table-responsive table-striped">
            <Columns>
                <asp:TemplateField HeaderText="Opciones">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnModificar" runat="server" CssClass="btn btn-sm btn-primary"
                            Text="M"
                            CommandArgument='<%# Eval("idHorario") %>'
                            OnCommand="btnModificar_Command" />

                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-sm btn-danger"
                            Text="C" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Código" DataField="idHorario" />
                <asp:BoundField HeaderText="Dia de la semana" DataField="diaSemana" />
                <asp:BoundField HeaderText="Hora Inicio" DataField="horaInicioStr" />
                <asp:BoundField HeaderText="Hora Fin" DataField="horaFinStr" />
                <asp:BoundField HeaderText="Es Feriado" DataField="esFeriado" />
                <asp:BoundField HeaderText="Fecha Creacion" DataField="fechaCreacion" />
                <asp:BoundField HeaderText="Usuario Creacion" DataField="usuarioCreacion" />
                <asp:BoundField HeaderText="Fecha Modificacion" DataField="fechaModificacion" />
                <asp:BoundField HeaderText="Usuario Modificacion" DataField="usuarioModificacion" />
                <asp:BoundField HeaderText="Estado" DataField="estado" />
            </Columns>
        </asp:GridView>
        <asp:HiddenField ID="hdnIdEliminar" runat="server" />
        <asp:Button ID="btnEliminarHorario" runat="server" Style="display: none;" OnClick="btn_eliminar_Click" />
    </div>
    <!-- Modales -->
    <!-- Modal para Registrar Horario -->
    <asp:HiddenField ID="hdnIdHorario" runat="server" />
    <asp:HiddenField ID="hdnModoModal" runat="server" />
    <div class="modal fade" id="modalRegistrarHorario" tabindex="-1" role="dialog" aria-labelledby="modalRegistrarHorarioLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
            <div class="modal-content">
                <!-- Título -->
                <div class="modal-header bg-warning text-black">
                    <h5 class="modal-title fw-bold" id="tituloModal">
                        <i class="fas fa-clock me-2"></i>Registrar Horario
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>

                <!-- Cuerpo -->
                <div class="modal-body">
                    <div class="border rounded p-3">
                        <h6 class="fw-bold mb-3">Información General</h6>
                        <div class="row align-items-end">
                            <!-- Día de la Semana -->
                            <div class="col-md-3 mb-3">
                                <label for="ddlDiaSemana" class="form-label">Día de la Semana</label>
                                <div class="col-md-3 mb-3">
                                    <label for="ddlDiaSemana" class="form-label">Día de la Semana</label>
                                    <asp:DropDownList ID="ddlDiaSemana" runat="server" CssClass="form-select" />
                                </div>
                            </div>

                            <!-- Hora Inicio -->
                            <div class="col-md-2 mb-3">
                                <label for="txtHoraInicio" class="form-label">Hora Inicio</label>
                                <asp:TextBox ID="txtHoraInicio" runat="server" CssClass="form-control" placeholder="HH:mm" />
                            </div>

                            <!-- Hora Fin -->
                            <div class="col-md-2 mb-3">
                                <label for="txtHoraFin" class="form-label">Hora Fin</label>
                                <asp:TextBox ID="txtHoraFin" runat="server" CssClass="form-control" placeholder="HH:mm" />
                            </div>

                            <!-- ¿Es Feriado? -->
                            <div class="col-md-3 mb-3">
                                <label class="form-label d-block">¿Es Feriado?</label>
                                <div class="form-check form-check-inline">
                                    <asp:RadioButton ID="rbFeriadoSi" runat="server" GroupName="Feriado" CssClass="form-check-input" />
                                    <label class="form-check-label" for="rbFeriadoSi">Sí</label>
                                </div>
                                <div class="form-check form-check-inline">
                                    <asp:RadioButton ID="rbFeriadoNo" runat="server" GroupName="Feriado" CssClass="form-check-input" />
                                    <label class="form-check-label" for="rbFeriadoNo">No</label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Footer -->
                <div class="modal-footer">
                    <asp:Button ID="btnGuardarHorario" runat="server" Text="Guardar" CssClass="btn btn-danger" OnClick="btnGuardarHorario_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
