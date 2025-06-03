<%@ Page Title="" Language="C#" MasterPageFile="~/SoftResCliente.Master" AutoEventWireup="true" CodeBehind="Locales_cliente.aspx.cs" Inherits="SoftResWA.Views.Cliente.Locales.Locales_cliente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Locales
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="locales-section py-5">
        <div class="container">
            <h2 class="text-center text-danger fw-bold">Encuentra tu Shifu Kay</h2>
            <p class="text-center subtitulo mb-5">Explora nuestros espacios de fusión peruano–china cerca de ti</p>

            <!-- Cartas de locales -->
            <div class="row g-4 justify-content-center" id="localesContainer">
                <!-- Carta 1 -->
                <div class="col-md-4">
                    <div class="card-local shadow card-local-activa" data-direccion="Av. Javier Prado Este 123, San Isidro">
                        <div class="card-body text-center">
                            <div class="icon-wrapper">
                                <i class="fas fa-map-marker-alt fa-2x my-4 d-block" style="color: #FFC107;"></i>
                            </div>
                            <h5 class="fw-bold text-danger">Shifu Kay San Isidro</h5>
                            <p class="text-muted small">Av. Javier Prado Este 123, San Isidro</p>
                        </div>
                    </div>
                </div>

                <!-- Carta 2 -->
                <div class="col-md-4">
                    <div class="card-local shadow" data-direccion="Calle Shell 456, Miraflores">
                        <div class="card-body text-center">
                            <i class="fas fa-map-marker-alt fa-2x my-4 d-block" style="color: #FFC107;"></i>
                            <h5 class="fw-bold text-danger">Shifu Kay Miraflores</h5>
                            <p class="text-muted small">Calle Shell 456, Miraflores</p>
                        </div>
                    </div>
                </div>

                <!-- Carta 3 -->
                <div class="col-md-4">
                    <div class="card-local shadow" data-direccion="Jirón de la Unión 789, Lima Centro">
                        <div class="card-body text-center">
                            <i class="fas fa-map-marker-alt fa-2x my-4 d-block" style="color: #FFC107;"></i>
                            <h5 class="fw-bold text-danger">Shifu Kay Centro</h5>
                            <p class="text-muted small">Jirón de la Unión 789, Lima Centro</p>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Mapa -->
            <div class="mt-5">
                <div id="mapa" class="mapa-google"></div>
            </div>
        </div>
    </section>
</asp:Content>

