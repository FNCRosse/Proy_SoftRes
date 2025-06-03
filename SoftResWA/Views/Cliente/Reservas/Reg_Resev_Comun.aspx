<%@ Page Title="" Language="C#" MasterPageFile="~/SoftResCliente.Master" AutoEventWireup="true" CodeBehind="Reg_Resev_Comun.aspx.cs" Inherits="SoftResWA.Views.Cliente.Reservas.Reg_Resev_Comun" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Registrar Reserva Común
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="reserva-form-section py-5">
        <div class="container">
            <div class="form-wrapper mx-auto">
                <h2 class="text-center text-danger fw-bold">Haz Tu Reserva</h2>
                <p class="text-center subtitulo mb-4">Asegura tu lugar para saborear nuestra fusión peruano–china</p>

                <div class="row g-4 justify-content-center">
                    <!-- Fecha -->
                    <div class="col-md-5">
                        <label class="form-label campo-label">Seleccione una fecha</label>
                        <div class="input-group input-rounded">
                            <input type="date" class="form-control custom-input" />
                            <span class="input-group-text icono-input">
                                <i class="fas fa-calendar-day"></i>
                            </span>
                        </div>
                    </div>

                    <!-- Hora -->
                    <div class="col-md-5">
                        <label class="form-label campo-label">Seleccione una hora</label>
                        <div class="input-group input-rounded">
                            <input type="time" class="form-control custom-input" />
                            <span class="input-group-text icono-input">
                                <i class="fas fa-clock"></i>
                            </span>
                        </div>
                    </div>

                    <!-- Personas -->
                    <div class="col-md-5">
                        <label class="form-label campo-label">Elige la cantidad de personas</label>
                        <select class="form-select custom-input">
                            <option selected disabled>Seleccione</option>
                            <option>1</option>
                            <option>2</option>
                            <option>3</option>
                        </select>
                    </div>

                    <!-- Local -->
                    <div class="col-md-5">
                        <label class="form-label campo-label">Seleccione un local</label>
                        <select class="form-select custom-input">
                            <option selected disabled>Seleccione</option>
                            <option>Local 1</option>
                            <option>Local 2</option>
                        </select>
                    </div>

                    <!-- Mesas -->
                    <div class="col-md-5">
                        <label class="form-label campo-label">Elige la cantidad de mesas</label>
                        <select class="form-select custom-input">
                            <option selected disabled>Seleccione</option>
                            <option>1</option>
                            <option>2</option>
                        </select>
                    </div>

                    <!-- Ubicación -->
                    <div class="col-md-5">
                        <label class="form-label campo-label">Elije tu ubicación favorita</label>
                        <select class="form-select custom-input">
                            <option selected disabled>Seleccione</option>
                            <option>Interior</option>
                            <option>Terraza</option>
                        </select>
                    </div>

                    <!-- Observaciones -->
                    <div class="col-md-10">
                        <label class="form-label campo-label">Observaciones</label>
                        <textarea class="form-control custom-input" rows="4" placeholder="Escriba sus observaciones aquí..."></textarea>
                    </div>

                    <!-- Botón -->
                    <div class="text-center mt-3">
                        <button class="btn btn-danger rounded-pill px-5 py-2 fw-bold" style="background-color: #BC1F1F; border: none;">Enviar</button>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

