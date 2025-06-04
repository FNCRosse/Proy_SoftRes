<%@ Page Title="" Language="C#" MasterPageFile="~/SoftResCliente.Master" AutoEventWireup="true" CodeBehind="Login_Home.aspx.cs" Inherits="SoftResWA.Views.Cliente.Home.Login_Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Login
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="login-menu-section d-flex align-items-center justify-content-center" style="min-height: 90vh;">
        <div class="text-center">
            <p class="texto-ingresa mb-1">INGRESA A</p>
            <h2 class="fw-bold texto-rojo">SHIFU KAY</h2>
            <p class="texto-subtitulo">y disfruta de una manera más rápida</p>
            <p class="texto-subtitulo">todo nuestro sabor</p>

            <!-- Botón Correo -->
            <button type="button" class="btn btn-correo mb-3" data-bs-toggle="modal" data-bs-target="#modalLogin">
                INICIAR SESIÓN CON CORREO
            </button>
            <br />

            <!-- Botón Google -->
            <button type="button" class="btn btn-google mb-3" onclick="loginConGoogle()">
                INICIAR SESIÓN CON GOOGLE
            </button>

            <p class="text-dark small mx-4">
                Al iniciar sesión con Google, aceptas nuestros
                <strong>términos y condiciones</strong> y
                <em>políticas de privacidad</em>.
            </p>

            <!-- Botón Registro -->
            <button type="button" class="btn btn-registro mt-3" data-bs-toggle="modal" data-bs-target="#modalRegistro">
                REGISTRARME
            </button>
        </div>
    </section>

    <!-- Modal Login -->
    <div class="modal fade" id="modalLogin" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content login-modal">
                <button type="button" class="btn-close-custom" data-bs-dismiss="modal" aria-label="Close">
                    <i class="fas fa-times"></i>
                </button>
                <div class="modal-body">
                    <h3 class="modal-title text-danger fw-bold">Iniciar Sesión</h3>
                    <p class="mb-3">Por favor, Ingrese la siguiente información</p>

                    <input type="email" class="form-control mb-3" placeholder="Correo electrónico" />

                    <div class="position-relative mb-3">
                        <input type="password" class="form-control" id="loginPass" placeholder="Contraseña" />
                        <i class="fas fa-eye toggle-password" onclick="togglePassword('loginPass', this)"></i>
                    </div>

                    <button class="btn btn-danger w-100 mb-2 fw-bold">Iniciar sesión</button>
                    <a href="#" class="text-primary small d-block text-center mt-1" data-bs-toggle="modal" data-bs-target="#modalRecuperar">¿Olvidaste tu contraseña?
                    </a>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Registro -->
    <div class="modal fade" id="modalRegistro" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content login-modal">
                <button type="button" class="btn-close-custom" data-bs-dismiss="modal" aria-label="Close">
                    <i class="fas fa-times"></i>
                </button>
                <div class="modal-body">
                    <h3 class="modal-title text-danger fw-bold">Registrarme</h3>
                    <p class="mb-3">Por favor, Ingrese la siguiente información</p>

                    <input type="text" class="form-control mb-2" placeholder="Nombre Completo" />
                    <input type="email" class="form-control mb-2" placeholder="Correo electrónico" />

                    <div class="row g-2 mb-2">
                        <div class="col">
                            <select class="form-select">
                                <option selected disabled>Seleccione tipo de documento</option>
                                <option>DNI</option>
                                <option>CE</option>
                                <option>Pasaporte</option>
                            </select>
                        </div>
                        <div class="col">
                            <input type="text" class="form-control" placeholder="Documento" />
                        </div>
                    </div>

                    <div class="row g-2 mb-2">
                        <div class="col">
                            <input type="text" class="form-control" placeholder="Teléfono" />
                        </div>
                        <div class="col position-relative">
                            <input type="password" class="form-control" id="registerPass" placeholder="Contraseña" />
                            <i class="fas fa-eye toggle-password" onclick="togglePassword('registerPass', this)"></i>
                        </div>
                    </div>

                    <div class="form-check mb-1">
                        <input class="form-check-input" type="checkbox" id="terminos1" />
                        <label class="form-check-label small fw-semibold" for="terminos1">
                            He leído y acepto los <span class="fst-italic">Términos y Condiciones y Políticas de Privacidad</span>
                        </label>
                    </div>

                    <div class="form-check mb-3">
                        <input class="form-check-input" type="checkbox" id="promos" />
                        <label class="form-check-label small fst-italic" for="promos">
                            Acepto el envío de Ofertas, Promociones y otros fines adicionales
                        </label>
                    </div>

                    <button class="btn btn-danger w-100 fw-bold">Crear</button>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal Recuperar Contraseña -->
    <div class="modal fade" id="modalRecuperar" tabindex="-1" aria-labelledby="modalRecuperarLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content login-modal border-0 rounded-4">
                <button type="button" class="btn-close-custom" data-bs-dismiss="modal" aria-label="Cerrar">
                    <i class="fas fa-times"></i>
                </button>
                <div class="modal-body text-center px-4 py-4">
                    <h4 class="text-danger fw-bold mb-3">¿Olvidaste tu contraseña?</h4>
                    <p class="text-muted mb-3">No te preocupes, ingresa tu correo electrónico y te enviaremos un enlace para restablecerla.</p>

                    <input type="email" class="form-control mb-3" placeholder="Tu correo electrónico" />

                    <button class="btn btn-warning w-100 fw-bold" style="color: #000;">
                        Enviar enlace de recuperación
                    </button>

                    <p class="small text-muted mt-3">Revisa también tu bandeja de spam o promociones.</p>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

