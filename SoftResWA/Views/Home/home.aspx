<%@ Page Title="" Language="C#" MasterPageFile="~/SoftRes.Master" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="SoftResWA.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Home
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container-fluid">

        <!-- Page Heading -->
        <div class="d-sm-flex align-items-center justify-content-between mb-4">
            <div class="text-start">
                <h1 class="h4 text-danger fw-bold mb-0 me-3">
                    <i class="fas fa-chart-line me-2"></i>Dashboard
                </h1>
            </div>
            <div class="dropdown">
                <button class="btn btn-sm shadow-sm dropdown-toggle d-none d-sm-inline-block"
                    style="background-color: #FBC02D; border-color: #FBC02D; color: black;"
                    type="button" id="dropdownReporte" data-bs-toggle="dropdown" aria-expanded="false">
                    <i class="fas fa-download fa-sm text-black-50 me-2"></i>Generar Reporte
                </button>
                <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="dropdownReporte">
                    <li><a class="dropdown-item" href="/Views/Reportes/reporte_cliente.aspx">Reporte de Clientes</a></li>
                    <li><a class="dropdown-item" href="/Views/Reportes/reporte_reservas.aspx">Reporte de Reservas</a></li>
                </ul>
            </div>
        </div>

        <!-- Content Row -->
        <div class="row">
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false" Text=""></asp:Label>

            <!-- Cantidad de Reservas (Diario) Card  -->
            <div class="col-xl-3 col-md-6 mb-4">
                <div class="card border-left-primary shadow h-100 py-2">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                                    Cantidad de Reservas (Diario)
                                </div>
                                <!-- Label for Cantidad de Reservas Diarias -->
                                <asp:Label ID="lblCantidadReservasDiarias" runat="server" Text="5" CssClass="h5 mb-0 font-weight-bold text-gray-800" />
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-calendar-days fa-2x text-gray-300"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <!-- Cantidad de Reservas (Semanalmente) Card -->
            <div class="col-xl-3 col-md-6 mb-4">
                <div class="card border-left-success shadow h-100 py-2">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                    Cantidad de Reservas (Semanalmente)
                                </div>
                                <!-- Label for Cantidad de Reservas Semanales -->
                                <asp:Label ID="lblCantidadReservasSemanales" runat="server" Text="10" CssClass="h5 mb-0 font-weight-bold text-gray-800" />
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-calendar-week fa-2x text-gray-300"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Ocupación de Mesas Actual Card -->
            <div class="col-xl-3 col-md-6 mb-4">
                <div class="card border-left-info shadow h-100 py-2">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold text-info text-uppercase mb-1">
                                    Ocupación de Mesas Actual
                                </div>
                                <div class="row no-gutters align-items-center">
                                    <div class="col-auto">
                                        <div class="h5 mb-0 mr-3 font-weight-bold text-gray-800">
                                            <!-- Label for Ocupación de Mesas -->
                                            <asp:Label ID="lblPorcentajeOcupacionMesas" runat="server" Text="50%" CssClass="h5 mb-0 font-weight-bold text-gray-800" />
                                        </div>
                                    </div>
                                    <div class="col">
                                        <div class="progress progress-sm mr-2">
                                            <div class="progress-bar bg-info" role="progressbar" style="width: 50%" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-check-to-slot fa-2x text-gray-300"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <!-- Cancelaciones recientes Card -->
            <div class="col-xl-3 col-md-6 mb-4">
                <div class="card border-left-warning shadow h-100 py-2">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                    Cancelaciones recientes
                                </div>
                                <!-- Label for Cancelaciones recientes -->
                                <asp:Label ID="lblCantidadCancelacionesRecientes" runat="server" Text="3" CssClass="h5 mb-0 font-weight-bold text-gray-800" />
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-rectangle-xmark fa-2x text-gray-300"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Content Row -->

            <div class="row">

                <!-- Area Chart -->
                <div class="col-xl-8 col-lg-7">
                    <div class="card shadow mb-4">
                        <!-- Card Header - Dropdown -->
                        <div
                            class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                            <h6 class="m-0 font-weight-bold text-danger">Evolución de reservas mensuales</h6>
                        </div>
                        <!-- Card Body -->
                        <div class="card-body">
                            <div class="chart-area">
                                <canvas id="myAreaChart"></canvas>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Pie Chart -->
                <div class="col-xl-4 col-lg-5">
                    <div class="card shadow mb-4">
                        <!-- Card Header - Dropdown -->
                        <div
                            class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                            <h6 class="m-0 font-weight-bold text-danger">Porcentaje de reservas por Locales </h6>
                        </div>
                        <!-- Card Body -->
                        <div class="card-body">
                            <div class="chart-pie pt-4 pb-2">
                                <canvas id="myPieChart"></canvas>
                            </div>
                            <div class="mt-4 text-center small">
                                <span class="mr-2">
                                    <i class="fas fa-circle text-primary me-1"></i>San Miguel
                                </span>
                                <span class="mr-2">
                                    <i class="fas fa-circle text-success me-1"></i>Callao
                                </span>
                                <span class="mr-2">
                                    <i class="fas fa-circle text-info me-1"></i>Miraflores
                                </span>
                                <span class="mr-2">
                                    <i class="fas fa-circle text-danger me-1"></i>Ate
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Content Row -->
            <div class="row">

                <!-- Content Column -->
                <div class="col-lg-6 mb-4">

                    <!-- Project Card Example -->
                    <div class="card shadow mb-4">
                        <div class="card-header py-3">
                            <h6 class="m-0 font-weight-bold text-danger">Estado actual de las reservas</h6>
                        </div>
                        <div class="card-body">
                            <h4 class="small font-weight-bold">Canceladas <span
                                class="float-right">20%</span></h4>
                            <div class="progress mb-4">
                                <div class="progress-bar bg-danger" role="progressbar" style="width: 20%"
                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                </div>
                            </div>
                            <h4 class="small font-weight-bold">Pendiente <span
                                class="float-right">40%</span></h4>
                            <div class="progress mb-4">
                                <div class="progress-bar bg-warning" role="progressbar" style="width: 40%"
                                    aria-valuenow="40" aria-valuemin="0" aria-valuemax="100">
                                </div>
                            </div>
                            <h4 class="small font-weight-bold">Confirmadas <span
                                class="float-right">60%</span></h4>
                            <div class="progress mb-4">
                                <div class="progress-bar" role="progressbar" style="width: 60%"
                                    aria-valuenow="60" aria-valuemin="0" aria-valuemax="100">
                                </div>
                            </div>
                            <h4 class="small font-weight-bold">Finzalizadas <span
                                class="float-right">Complete!</span></h4>
                            <div class="progress">
                                <div class="progress-bar bg-success" role="progressbar" style="width: 100%"
                                    aria-valuenow="100" aria-valuemin="0" aria-valuemax="100">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-6 mb-4">

                    <!-- Acceso Directo -->
                    <div class="card shadow mb-4">
                        <div class="card-header py-3">
                            <h6 class="m-0 font-weight-bold text-danger">Accesos Directos</h6>
                        </div>
                        <div class="card-body">
                            <div class="row text-center">
                                <!-- Acceso directo 1 -->
                                <div class="col-md-4 mb-3">
                                    <div class="dropdown">
                                        <a class="text-decoration-none" href="#" role="button" id="dropdownReserva" data-bs-toggle="dropdown" aria-expanded="false">
                                            <div class="card border-left-danger shadow h-100 py-2">
                                                <div class="card-body text-center">
                                                    <i class="fas fa-calendar-plus fa-2x text-danger mb-2 d-block"></i>
                                                    <div class="h6 text-gray-800">Registrar Reserva</div>
                                                </div>
                                            </div>
                                        </a>
                                        <ul class="dropdown-menu dropdown-menu-start" aria-labelledby="dropdownReserva">
                                            <li><a class="dropdown-item" href="../Reservas/registrar_reserva_comun.aspx">Reserva Común</a></li>
                                            <li><a class="dropdown-item" href="../Reservas/registrar_reserva_evento.aspx">Reserva Evento</a></li>
                                        </ul>
                                    </div>
                                </div>
                                <!-- Acceso directo 2 -->
                                <div class="col-md-4 mb-3">
                                    <a href="../Reservas/reserva_gestion.aspx" class="text-decoration-none">
                                        <div class="card border-left-info shadow h-100 py-2">
                                            <div class="card-body">
                                                <div class="text-center">
                                                    <i class="fas fa-list fa-2x text-info mb-2"></i>
                                                    <div class="h6 text-gray-800">Listar Reservas</div>
                                                </div>
                                            </div>
                                        </div>
                                    </a>
                                </div>
                                <!-- Acceso directo 3 -->
                                <div class="col-md-4 mb-3">
                                    <a href="../Usuarios/registro_cliente.aspx" class="text-decoration-none">
                                        <div class="card border-left-success shadow h-100 py-2">
                                            <div class="card-body">
                                                <div class="text-center">
                                                    <i class="fas fa-user-plus fa-2x text-success mb-2"></i>
                                                    <div class="h6 text-gray-800">Registrar Cliente</div>
                                                </div>
                                            </div>
                                        </div>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
</asp:Content>
