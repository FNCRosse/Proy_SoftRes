<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/SoftResCliente.Master" AutoEventWireup="true" CodeBehind="Login_Home.aspx.cs" Inherits="SoftResWA.Views.Cliente.Home.Login_Home" %>

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

                    <asp:TextBox ID="txtLoginEmail" runat="server" CssClass="form-control mb-3" placeholder="Correo electrónico" TextMode="Email" />

                    <div class="position-relative mb-3">
                        <asp:TextBox ID="txtLoginPassword" runat="server" CssClass="form-control" placeholder="Contraseña" TextMode="Password" />
                        <i class="fas fa-eye toggle-password" onclick="togglePassword('loginPass', this)"></i>
                    </div>
                    <asp:Label ID="lblLoginError" runat="server" CssClass="text-danger small d-block text-center mb-2" Visible="false" />
                    <asp:Button ID="btnLogin" runat="server" Text="Iniciar sesión" CssClass="btn btn-danger w-100 mb-2 fw-bold" OnClick="btnLogin_Click" />
                    <a href="#" class="text-primary small d-block text-center mt-1" data-bs-toggle="modal" data-bs-target="#modalRecuperar">¿Olvidaste tu contraseña?
                    </a>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Registro -->
    <!-- ==================================== -->
    <!-- ==        MODAL REGISTRO          == -->
    <!-- ==================================== -->
    <div class="modal fade" id="modalRegistro" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content login-modal">
                <button type="button" class="btn-close-custom" data-bs-dismiss="modal" aria-label="Close">
                    <i class="fas fa-times"></i>
                </button>
                <div class="modal-body">
                    <h3 class="modal-title text-danger fw-bold">Registrarme</h3>
                    <p class="mb-3">Por favor, Ingrese la siguiente información</p>

                    <asp:TextBox ID="txtRegNombre" runat="server" CssClass="form-control mb-2" placeholder="Nombre Completo" />
                    <asp:RequiredFieldValidator ID="rfvRegNombre" runat="server" ControlToValidate="txtRegNombre"
                        ErrorMessage="El nombre es requerido." CssClass="text-danger small d-block mb-1" Display="Dynamic" ValidationGroup="RegistroGroup" />

                    <asp:TextBox ID="txtRegEmail" runat="server" CssClass="form-control mb-2" placeholder="Correo electrónico" TextMode="Email" />
                    <asp:RequiredFieldValidator ID="rfvRegEmail" runat="server" ControlToValidate="txtRegEmail"
                        ErrorMessage="El correo es requerido." CssClass="text-danger small d-block mb-1" Display="Dynamic" ValidationGroup="RegistroGroup" />
                    <asp:RegularExpressionValidator ID="revRegEmail" runat="server" ControlToValidate="txtRegEmail"
                        ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"
                        ErrorMessage="Formato de correo inválido." CssClass="text-danger small d-block mb-1" Display="Dynamic" ValidationGroup="RegistroGroup" />

                    <div class="row g-2 mb-2">
                        <div class="col">
                            <asp:DropDownList ID="ddlRegTipoDoc" runat="server" CssClass="form-select" />
                            <asp:RequiredFieldValidator ID="rfvRegTipoDoc" runat="server" ControlToValidate="ddlRegTipoDoc"
                                ErrorMessage="Seleccione un tipo de documento." CssClass="text-danger small d-block" Display="Dynamic" InitialValue="0" ValidationGroup="RegistroGroup" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtRegNumDoc" runat="server" CssClass="form-control" placeholder="Documento" />
                            <asp:RequiredFieldValidator ID="rfvRegNumDoc" runat="server" ControlToValidate="txtRegNumDoc"
                                ErrorMessage="El documento es requerido." CssClass="text-danger small d-block" Display="Dynamic" ValidationGroup="RegistroGroup" />
                        </div>
                    </div>

                    <div class="row g-2 mb-2">
                        <div class="col">
                            <asp:TextBox ID="txtRegTelefono" runat="server" CssClass="form-control" placeholder="Teléfono" />
                            <asp:RequiredFieldValidator ID="rfvRegTelefono" runat="server" ControlToValidate="txtRegTelefono"
                                ErrorMessage="El teléfono es requerido." CssClass="text-danger small d-block" Display="Dynamic" ValidationGroup="RegistroGroup" />
                        </div>
                        <div class="col position-relative">
                            <asp:TextBox ID="txtRegPassword" runat="server" CssClass="form-control" placeholder="Contraseña" TextMode="Password" />
                            <i class="fas fa-eye toggle-password" onclick="togglePassword('<%= txtRegPassword.ClientID %>', this)"></i>
                            <asp:RequiredFieldValidator ID="rfvRegPassword" runat="server" ControlToValidate="txtRegPassword"
                                ErrorMessage="La contraseña es requerida." CssClass="text-danger small d-block" Display="Dynamic" ValidationGroup="RegistroGroup" />
                        </div>
                    </div>

                    <div class="form-check mb-1">
                        <asp:CheckBox ID="chkTerminos" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label small fw-semibold" for="<%= chkTerminos.ClientID %>">
                            He leído y acepto los <span class="fst-italic">Términos y Condiciones y Políticas de Privacidad</span>
                        </label>
                        <asp:CustomValidator ID="cvTerminos" runat="server" ErrorMessage="Debes aceptar los términos y condiciones."
                            ClientValidationFunction="validarTerminos" CssClass="text-danger small d-block" Display="Dynamic" ValidationGroup="RegistroGroup" />
                    </div>

                    <div class="form-check mb-3">
                        <asp:CheckBox ID="chkPromos" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label small fst-italic" for="<%= chkPromos.ClientID %>">
                            Acepto el envío de Ofertas, Promociones y otros fines adicionales
                        </label>
                    </div>

                    <asp:Button ID="btnCrearCuenta" runat="server" Text="Crear" CssClass="btn btn-danger w-100 fw-bold"
                        OnClick="btnCrearCuenta_Click" ValidationGroup="RegistroGroup" />
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

                    <asp:TextBox ID="txtRecuperarEmail" runat="server" CssClass="form-control mb-3" placeholder="Tu correo electrónico" TextMode="Email"></asp:TextBox>

                    <asp:Button ID="btnEnviarRecuperacion" runat="server" Text="Enviar enlace de recuperación" CssClass="btn btn-warning w-100 fw-bold" Style="color: #000;" OnClick="btnEnviarRecuperacion_Click" />

                    <p class="small text-muted mt-3">Revisa también tu bandeja de spam o promociones.</p>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function togglePassword(fieldId, icon) {
            const field = document.getElementById(fieldId);
            if (field.type === "password") {
                field.type = "text";
                icon.classList.remove("fa-eye");
                icon.classList.add("fa-eye-slash");
            } else {
                field.type = "password";
                icon.classList.remove("fa-eye-slash");
                icon.classList.add("fa-eye");
            }
        }
    </script>
    <script type="text/javascript">
        function togglePassword(fieldId, icon) {
            // ... tu función existente
        }

        // NUEVA FUNCIÓN para validar el checkbox de términos
        function validarTerminos(source, args) {
            var chk = document.getElementById('<%= chkTerminos.ClientID %>');
            args.IsValid = chk.checked;
        }
    </script>
</asp:Content>

