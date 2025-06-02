<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TuNamespace.Login" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8">
    <title>Login</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Content/Fonts/css/Login.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
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
            <div class="checkbox mb-3 form-check form-switch">
                <asp:CheckBox ID="chkRecordar" runat="server" CssClass="form-check-input" />
                <label class="form-check-label" for="chkRecordar">Recordar contraseña</label>
            </div>
            <asp:Button ID="BtnAcceder" runat="server" CssClass="w-100 btn btn-lg" Text="Acceder" OnClick="BtnAcceder_Click" />
            <p class="copyright mt-3">&copy; 2024</p>
        </main>
    </form>

    <!-- Scripts -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
