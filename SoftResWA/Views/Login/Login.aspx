<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SoftResWA.Views.Login.Login" %>

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
                <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="Usuario"></asp:TextBox>
                <label for="txtUsuario">Usuario</label>
            </div>

            <div class="form-floating mb-3">
                <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password" CssClass="form-control" placeholder="Contraseña"></asp:TextBox>
                <label for="txtContrasena">Contraseña</label>
            </div>

            <div class="form-check mb-3">
                <asp:CheckBox ID="chkRecordar" runat="server" CssClass="simple-checkbox" Text="Recordar contraseña" />
            </div>

            <asp:Button ID="BtnAcceder" runat="server" Text="Acceder" CssClass="w-100 btn btn-lg" OnClick="BtnAcceder_Click" />
            <div class="mt-3 text-center">
                <a href="RecuperarContraseña.aspx" class="forgot-link">¿Olvidaste tu contraseña?</a>
            </div>

            <p class="copyright mt-3">&copy; 2024</p>
        </main>
    </form>
    <!-- Scripts -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
