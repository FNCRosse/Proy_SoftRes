<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CambiarContrasena.aspx.cs" Inherits="SoftResWA.Views.Login.CambiarContrasena" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cambiar Contraseña</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <style>
        body {
            background: linear-gradient(to bottom, #FFF3CD, #FFE5E5);
            font-family: 'Segoe UI', sans-serif;
        }

        .container-card {
            max-width: 500px;
            margin: 5% auto;
            background-color: white;
            border-radius: 20px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
            padding: 30px;
        }

        .titulo {
            color: #D32F2F;
            font-weight: bold;
            font-size: 24px;
            text-align: center;
        }

        .emoji-lock {
            font-size: 50px;
            display: block;
            text-align: center;
        }

        .btn-rojo {
            background-color: #D32F2F;
            color: white;
        }

            .btn-rojo:hover {
                background-color: #b3261e;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-card">
            <span class="emoji-lock">🔒</span>
            <h2 class="titulo">¡Cambia tu contraseña!</h2>
            <p class="text-center text-muted">Ingresa tu nueva contraseña para acceder de forma segura 😊</p>

            <div class="mb-3">
                <label for="txtNuevaContrasena" class="form-label">🔐 Nueva Contraseña</label>
                <asp:TextBox ID="txtNuevaContrasena" runat="server" CssClass="form-control" TextMode="Password" />
                <asp:RequiredFieldValidator ID="rfvNueva" runat="server"
                    ControlToValidate="txtNuevaContrasena"
                    ErrorMessage="Debes ingresar una contraseña"
                    CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="mb-3">
                <label for="txtConfirmarContrasena" class="form-label">✅ Confirmar Contraseña</label>
                <asp:TextBox ID="txtConfirmarContrasena" runat="server" CssClass="form-control" TextMode="Password" />
                <asp:RequiredFieldValidator ID="rfvConfirmar" runat="server"
                    ControlToValidate="txtConfirmarContrasena"
                    ErrorMessage="Debes confirmar tu contraseña"
                    CssClass="text-danger" Display="Dynamic" />
                <asp:CompareValidator ID="cvContrasenas" runat="server"
                    ControlToValidate="txtConfirmarContrasena"
                    ControlToCompare="txtNuevaContrasena"
                    ErrorMessage="Las contraseñas no coinciden"
                    CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="d-grid">
                <asp:Button ID="btnCambiar" runat="server" Text="Cambiar Contraseña" CssClass="btn btn-rojo" OnClick="btnCambiar_Click" />
            </div>
        </div>
    </form>
</body>
</html>

