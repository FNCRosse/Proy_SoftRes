<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SoftResWA.Views.Login.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Login</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Content/Fonts/css/Login.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="bg"></div>
        <main class="form-signin">
            <h1 class="h3">Login</h1>
            <div class="form-floating mb-3">
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Email"></asp:TextBox>
                <label for="txtEmail">Email</label>
            </div>

            <div class="form-floating mb-3">
                <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password" CssClass="form-control" placeholder="Contraseña"></asp:TextBox>
                <label for="txtContrasena">Contraseña</label>
            </div>

            <asp:Button ID="BtnAcceder" runat="server" Text="Acceder" CssClass="w-100 btn btn-lg" OnClick="BtnAcceder_Click" />
            <div class="mt-3 text-center">
                <asp:Label ID="lblMensajeError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            </div>
            <div class="mt-3 text-center">
                <a href="#" class="forgot-link" data-bs-toggle="modal" data-bs-target="#forgotPasswordModal">¿Olvidaste tu contraseña?</a>
            </div>

            <p class="copyright mt-3">&copy; 2025</p>
        </main>
        <div class="modal fade" id="forgotPasswordModal" tabindex="-1" aria-labelledby="forgotPasswordModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="forgotPasswordModalLabel">Recuperar Contraseña</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <p>Por favor, ingresa tu dirección de correo electrónico. Te enviaremos un enlace para restablecer tu contraseña.</p>
                        <div class="mb-3">
                            <label for="txtEmailRecuperacion" class="form-label">Correo Electrónico:</label>
                            <asp:TextBox ID="txtEmailRecuperacion" runat="server" CssClass="form-control" placeholder="tu.correo@example.com" TextMode="Email"></asp:TextBox>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button ID="btnEnviarCorreo" runat="server" Text="Enviar" CssClass="btn btn-primary" OnClick="btnEnviarCorreo_Click" />
                    </div>
                </div>
            </div>
        </div>
    </form>
    <!-- Scripts -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
