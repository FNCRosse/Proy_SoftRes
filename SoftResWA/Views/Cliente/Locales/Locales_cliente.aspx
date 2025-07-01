<%@ Page Title="" Language="C#" MasterPageFile="~/SoftResCliente.Master" AutoEventWireup="true" CodeBehind="Locales_cliente.aspx.cs" Inherits="SoftResWA.Views.Cliente.Locales.Locales_cliente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Locales
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        /* Estilos para los botones de las sedes */
        .btn-sede {
            background-color: #f8f9fa; /* Un gris muy claro */
            border: 1px solid #dee2e6; /* Borde sutil */
            color: #495057; /* Texto oscuro */
            margin: 0 5px;
            padding: 10px 20px;
            font-weight: 500;
            transition: all 0.3s ease;
            border-radius: 50px; /* Bordes completamente redondeados (píldora) */
            text-decoration: none; /* Quita el subrayado de LinkButton */
        }

            .btn-sede:hover {
                background-color: #e2e6ea;
                border-color: #dae0e5;
                transform: translateY(-2px); /* Pequeño efecto de elevación */
            }

            /* Estilo para el botón de la sede activa */
            .btn-sede.active {
                background-color: #BC1F1F; /* Color rojo principal de tu tema */
                color: #ffffff; /* Texto blanco */
                border-color: #BC1F1F;
                box-shadow: 0 4px 12px rgba(188, 31, 31, 0.4); /* Sombra roja sutil */
                transform: translateY(-2px);
            }

        /* Estilos para las tarjetas de locales */
        .card-local {
            border: 1px solid transparent;
            border-radius: 1rem; /* Bordes más redondeados */
            cursor: pointer;
            transition: all 0.3s ease-in-out;
            height: 100%; /* Asegura que todas las tarjetas en una fila tengan la misma altura */
        }

            .card-local:hover {
                transform: translateY(-5px);
                box-shadow: 0 8px 25px rgba(0,0,0,0.1);
                border-color: rgba(188, 31, 31, 0.5);
            }

            /* Estilo para la tarjeta del local activo */
            .card-local.card-local-activa {
                border: 2px solid #BC1F1F;
                transform: translateY(-5px);
                box-shadow: 0 8px 25px rgba(0,0,0,0.15);
            }

        /* Contenedor del mapa */
        .mapa-google {
            height: 450px;
            width: 100%;
            border-radius: 1rem;
            box-shadow: 0 4px 15px rgba(0,0,0,0.1);
        }
    </style>
    <section class="locales-section py-5">
        <div class="container">
            <h2 class="text-center text-danger fw-bold">Encuentra tu Shifu Kay</h2>
            <p class="text-center subtitulo mb-5">Explora nuestros espacios de fusión peruano–china cerca de ti</p>

            <!-- =============================================================== -->
            <!-- ==         CONTENEDOR DINÁMICO PARA BOTONES DE SEDES         == -->
            <!-- =============================================================== -->
            <div id="sedes-nav" class="text-center mb-5">
                <asp:Repeater ID="rptSedes" runat="server" OnItemCommand="rptSedes_ItemCommand">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnSede" runat="server"
                            CssClass='<%# (Container.ItemIndex == 0) ? "btn btn-sede active" : "btn btn-sede" %>'
                            Text='<%# Eval("nombre") %>'
                            CommandName="CargarLocales"
                            CommandArgument='<%# Eval("idSede") %>' />
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <!-- =============================================================== -->
            <!-- ==      CONTENEDOR DINÁMICO PARA TARJETAS DE LOCALES       == -->
            <!-- =============================================================== -->
            <asp:UpdatePanel ID="updLocales" runat="server">
                <ContentTemplate>
                    <div class="row g-4 justify-content-center" id="localesContainer">
                        <asp:Repeater ID="rptLocales" runat="server">
                            <ItemTemplate>
                                <div class="col-md-4">
                                    <div class="card-local shadow" data-direccion='<%# Eval("direccion") %>'>
                                        <div class="card-body text-center">
                                            <div class="icon-wrapper">
                                                <i class="fas fa-map-marker-alt fa-2x my-4 d-block" style="color: #FFC107;"></i>
                                            </div>
                                            <h5 class="fw-bold text-danger"><%# Eval("nombre") %></h5>
                                            <p class="text-muted small"><%# Eval("direccion") %></p>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>

                        <!-- Mensaje si no hay locales para la sede seleccionada -->
                        <asp:Panel ID="pnlNoLocales" runat="server" Visible="false" CssClass="text-center">
                            <div class="alert alert-info">No se encontraron locales para esta sede.</div>
                        </asp:Panel>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="rptSedes" EventName="ItemCommand" />
                </Triggers>
            </asp:UpdatePanel>

            <!-- Mapa (sigue igual) -->
            <div class="mt-5">
                <div id="mapa" class="mapa-google"></div>
            </div>
        </div>
    </section>

    <!-- Script para reiniciar los event listeners después de un UpdatePanel -->
    <script type="text/javascript">
        // Esta función se asegura de que el script del mapa se ejecute cada vez que el UpdatePanel se actualice
        function pageLoad(sender, args) {
            if (args.get_isPartialLoad()) {
                // Si es una carga parcial (postback de UpdatePanel), reiniciamos los listeners
                inicializarTarjetasLocales();
            }
        }

        // Función para añadir los event listeners a las tarjetas
        function inicializarTarjetasLocales() {
            const cards = document.querySelectorAll(".card-local");
            if (cards.length > 0) {
                // Seleccionar la primera tarjeta como activa por defecto
                cards.forEach(c => c.classList.remove("card-local-activa"));
                cards[0].classList.add("card-local-activa");

                // Actualizar el mapa con la dirección de la primera tarjeta
                const primeraDireccion = cards[0].dataset.direccion;
                geocodeDireccion(primeraDireccion, function (location) {
                    map.setCenter(location);
                    marker.setPosition(location);
                });

                // Añadir los listeners de clic a todas las tarjetas
                cards.forEach(card => {
                    card.addEventListener("click", () => {
                        cards.forEach(c => c.classList.remove("card-local-activa"));
                        card.classList.add("card-local-activa");
                        const direccion = card.dataset.direccion;
                        geocodeDireccion(direccion, function (location) {
                            map.setCenter(location);
                            marker.setPosition(location);
                        });
                    });
                });
            }
        }

        // Ejecutar al cargar la página por primera vez
        document.addEventListener("DOMContentLoaded", function () {
            inicializarTarjetasLocales();
        });
    </script>
</asp:Content>

