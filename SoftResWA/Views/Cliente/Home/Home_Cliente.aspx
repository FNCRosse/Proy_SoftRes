<%@ Page Title="" Language="C#" MasterPageFile="~/SoftResCliente.Master" AutoEventWireup="true" CodeBehind="Home_Cliente.aspx.cs" Inherits="SoftResWA.Views.Cliente.Home.Home_Cliente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Home
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--Call to action-->
    <div class="hero-section text-white d-flex align-items-center justify-content-center text-center" >
        <div class="hero-content text-center animate-fade-in">
            <h1 class="display-4 fw-bold">Descubre la auténtica<br />
                maestría culinaria china</h1>
            <p class="lead mt-3">
                En Shifu Kay, cada plato es una obra de arte creada por chefs expertos,<br />
                ofreciendo una experiencia gastronómica que te transportará a China.
            </p>
            <div class="d-flex justify-content-center flex-wrap gap-3 mt-4">
                <a href="/Views/Cliente/Reservas/Reg_Resev_Comun.aspx" class="btn btn-danger rounded-pill px-4 py-2 fw-bold">Reserva Aquí <i class="fas fa-arrow-right ms-2"></i>
                </a>
                <a href="/Views/Cliente/Reservas/Reg_Resev_Evento.aspx" class="btn btn-dark rounded-pill px-4 py-2">Reservas para Eventos
                </a>
            </div>
        </div>
    </div>
    <!-- Quienes somos -->
    <section class="quienes-somos-section py-5">
    <div class="container">
        <h2 class="text-danger fw-bold mb-5">¿Quiénes Somos?</h2>
        <div class="row align-items-center">
            <div class="col-md-6 mb-4 mb-md-0">
                <img src="/Image/aditya-romansa-m6p4lqWxfy0-unsplash.jpg" alt="Chef Shifu" class="img-fluid rounded-4 shadow w-75" />
            </div>
            <div class="col-md-6 ps-md-1">
                <p class="fs-5 text-justify">
                    En Shifu Kay, fusionamos la rica tradición culinaria china con los sabores vibrantes del Perú, creando un chifa moderno que celebra la autenticidad y la innovación. 
                    Cada plato es una obra maestra elaborada con ingredientes frescos y técnicas expertas, diseñada para transportarte a una experiencia gastronómica única.
                    Somos un equipo apasionado que combina herencia y creatividad para ofrecerte lo mejor de dos mundos. ¡Ven y descubre nuestra historia en cada bocado!
                </p>
            </div>
        </div>
    </div>

</section>
    <!-- Carrucel de fotos -->
    <section class="carousel-section py-5">
    <div class="container">
        <h2 class="text-center text-danger fw-bold mb-2">Platos que Cautivan</h2>
        <p class="text-center text-secondary mb-4">Descubre la fusión peruano-china en cada bocado</p>

        <div id="carouselPlatos" class="carousel slide" data-bs-ride="carousel">
            <div class="carousel-inner rounded-4 shadow-lg overflow-hidden">
                <div class="carousel-item active">
                    <img src="/Image/israel-albornoz--SsC5Fpp-9o-unsplash.jpg" class="d-block w-100" alt="Plato 1">
                </div>
                <div class="carousel-item">
                    <img src="/Image/mae-mu-H5Hj8QV2Tx4-unsplash.jpg" class="d-block w-100" alt="Plato 2">
                </div>
                <div class="carousel-item">
                    <img src="/Image/wengang-zhai-8TV_eAmrMXQ-unsplash.jpg" class="d-block w-100" alt="Plato 3">
                </div>
                <div class="carousel-item">
                    <img src="/Image/debbie-tea-LO7rNP0LRro-unsplash.jpg" class="d-block w-100" alt="Plato 4">
                </div>
            </div>

            <!-- Controles -->
            <button class="carousel-control-prev" type="button" data-bs-target="#carouselPlatos" data-bs-slide="prev">
                <span class="carousel-control-prev-icon bg-dark rounded-circle p-2" aria-hidden="true"></span>
                <span class="visually-hidden">Anterior</span>
            </button>
            <button class="carousel-control-next" type="button" data-bs-target="#carouselPlatos" data-bs-slide="next">
                <span class="carousel-control-next-icon bg-dark rounded-circle p-2" aria-hidden="true"></span>
                <span class="visually-hidden">Siguiente</span>
            </button>
        </div>
    </div>
</section>

</asp:Content>
