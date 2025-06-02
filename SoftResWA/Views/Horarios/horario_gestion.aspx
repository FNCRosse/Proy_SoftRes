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
            <!-- Dia de la semana -->
            <div class="col-auto">
                <label for="ddlDia" class="form-label">Dia de la Semana</label>
                <select id="ddlDia" class="form-select">
                    <option selected disabled>Seleccionar...</option>
                    <option value="1">Lunes</option>
                    <option value="0">Martes</option>
                    <option value="0">Miercoles</option>
                    <option value="0">Jueves</option>
                    <option value="0">Viernes</option>
                    <option value="0">Sabado</option>
                    <option value="0">Domingo</option>
                </select>
            </div>
            <!-- Es Feriano -->
            <div class="col-auto">
                <label for="ddlSede" class="form-label">Es Feriado</label>
                <select id="ddlSede" class="form-select">
                    <option selected disabled>Seleccionar...</option>
                    <option value="1">Si</option>
                    <option value="0">No</option>
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
                    data-bs-toggle="modal" data-bs-target="#modalRegistrarHorario">
                    <i class="fas fa-plus me-2"></i>Nuevo
                </button>
            </div>
        </div>
    </div>

    <!-- Tabla -->
    <div class="row">
        <asp:GridView ID="dgvHorario" runat="server" AllowPaging="false" AutoGenerateColumns="false"
            CssClass="table table-hover table-responsive table-striped">
            <Columns>
                <asp:BoundField HeaderText="Código" DataField="SedeId" />
                <asp:BoundField HeaderText="Dia de la semana" DataField="DiaSem" />
                <asp:BoundField HeaderText="Hora Inicio" DataField="HInicio" />
                <asp:BoundField HeaderText="Hora Fin" DataField="HFin" />
                <asp:BoundField HeaderText="Es Feriado" DataField="EsFeriado" />
                <asp:BoundField HeaderText="Fecha Creacion" DataField="FechaCrea" />
                <asp:BoundField HeaderText="Usuario Creacion" DataField="UsuarioCrea" />
                <asp:BoundField HeaderText="Fecha Modificacion" DataField="FechaMod" />
                <asp:BoundField HeaderText="Usuario Modificacion" DataField="UsuarioMod" />
                <asp:BoundField HeaderText="Estado" DataField="Estado" />
            </Columns>
        </asp:GridView>
    </div>
    <!-- Modales -->
    <!-- Modal para Registrar Horario -->
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="modal fade" id="modalRegistrarHorario" tabindex="-1" role="dialog" aria-labelledby="modalRegistrarHorarioLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
            <div class="modal-content">
                <!-- Título -->
                <div class="modal-header bg-warning text-black">
                    <h5 class="modal-title fw-bold" id="modalRegistrarHorarioLabel">
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
                                <asp:DropDownList ID="ddlDiaSemana" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Seleccionar..." Value="" />
                                    <asp:ListItem Text="Lunes" Value="Lunes" />
                                    <asp:ListItem Text="Martes" Value="Martes" />
                                    <asp:ListItem Text="Miércoles" Value="Miércoles" />
                                    <asp:ListItem Text="Jueves" Value="Jueves" />
                                    <asp:ListItem Text="Viernes" Value="Viernes" />
                                    <asp:ListItem Text="Sábado" Value="Sábado" />
                                    <asp:ListItem Text="Domingo" Value="Domingo" />
                                </asp:DropDownList>
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
