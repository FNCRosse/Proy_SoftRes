<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecuperarContraseña.aspx.cs" Inherits="SoftResWA.Views.Login.RecuperarContraseña" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Recuperar Contraseña</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Content/Fonts/css/Login.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="bg"></div>
        <main class="form-signin">
            <h1 class="h3">Recuperar Contraseña</h1>
            <div class="form-floating mb-3">
                <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" placeholder="Correo electrónico"></asp:TextBox>
                <label for="txtCorreo">Correo electrónico</label>
            </div>

            <asp:Button ID="btnRecuperar" runat="server" Text="Enviar enlace" CssClass="btn" OnClick="btnRecuperar_Click" />

            <div class="mt-3 text-center">
                <a href="Login.aspx" class="forgot-link">Volver al inicio de sesión</a>
            </div>

            <p class="copyright mt-3">&copy; 2025</p>
        </main>
    </form>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>

