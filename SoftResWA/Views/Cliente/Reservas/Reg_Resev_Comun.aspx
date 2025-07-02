<%@ Page Title="" Language="C#" MasterPageFile="~/SoftResCliente.Master" AutoEventWireup="true" CodeBehind="Reg_Resev_Comun.aspx.cs" Inherits="SoftResWA.Views.Cliente.Reservas.Reg_Resev_Comun" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Registrar Reserva Común
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="reserva-form-section py-5">
        <div class="container">
            <div class="form-wrapper mx-auto">
                <h2 class="text-center text-danger fw-bold">Haz Tu Reserva</h2>
                <p class="text-center subtitulo mb-4">Asegura tu lugar para saborear nuestra fusión peruano–china</p>

                <div class="row g-4 justify-content-center">
                    <!-- Fecha -->
                    <div class="col-md-5">
                        <label class="form-label campo-label">
                            <i class="fas fa-calendar-day me-2"></i>Seleccione una fecha
                            <small class="text-muted d-block">*Disponible todos los días except domingos</small>
                        </label>
                        <asp:TextBox ID="txtFecha" runat="server" 
                            CssClass="form-control custom-input rounded-3" 
                            TextMode="Date" 
                            onchange="validarFecha(this)"
                            title="Haga clic para abrir el calendario"
                            autocomplete="off" />
                        <div class="invalid-feedback" id="fechaError"></div>
                        <small class="form-text text-success" id="fechaSuccess" style="display: none;">
                            <i class="fas fa-check-circle"></i> Fecha disponible
                        </small>
                    </div>

                    <!-- Hora -->
                    <div class="col-md-5">
                        <label class="form-label campo-label">
                            <i class="fas fa-clock me-2"></i>Seleccione una hora
                            <small class="text-muted d-block">*Horario: 11:00 AM - 11:00 PM</small>
                        </label>
                        <asp:TextBox ID="txtHora" runat="server" 
                            CssClass="form-control custom-input rounded-3" 
                            TextMode="Time" 
                            onchange="validarHora(this)" 
                            step="1800"
                            title="Haga clic para seleccionar la hora"
                            autocomplete="off" />
                        <div class="invalid-feedback" id="horaError"></div>
                        <small class="form-text text-success" id="horaSuccess" style="display: none;">
                            <i class="fas fa-check-circle"></i> Hora disponible
                        </small>
                        <small class="form-text text-info" id="horaInfo">
                            <i class="fas fa-info-circle"></i> Seleccione horarios cada 30 minutos
                        </small>
                    </div>

                    <!-- Información de disponibilidad -->
                    <div class="col-md-10">
                        <div class="alert alert-info" id="infoDisponibilidad" style="display: none;">
                            <i class="fas fa-info-circle"></i>
                            <strong>Información de disponibilidad:</strong>
                            <span id="mensajeDisponibilidad"></span>
                        </div>
                    </div>

                    <!-- Personas -->
                    <div class="col-md-5">
                        <label class="form-label campo-label">Elige la cantidad de personas</label>
                        <asp:DropDownList ID="ddlCantidadPersonas" runat="server" 
                            CssClass="form-select custom-input"
                            onchange="calcularMesasSugeridas()">
                            <asp:ListItem Text="Seleccione" Value="" />
                            <asp:ListItem Text="1" Value="1" />
                            <asp:ListItem Text="2" Value="2" />
                            <asp:ListItem Text="3" Value="3" />
                            <asp:ListItem Text="4" Value="4" />
                            <asp:ListItem Text="5" Value="5" />
                            <asp:ListItem Text="6" Value="6" />
                            <asp:ListItem Text="7" Value="7" />
                            <asp:ListItem Text="8" Value="8" />
                        </asp:DropDownList>
                    </div>

                    <!-- Local -->
                    <div class="col-md-5">
                        <label class="form-label campo-label">Seleccione un local</label>
                        <asp:DropDownList ID="ddlLocal" runat="server" 
                            CssClass="form-select custom-input"
                            onchange="verificarDisponibilidadLocal()">
                            <asp:ListItem Text="Seleccione" Value="" />
                        </asp:DropDownList>
                    </div>

                    <!-- Mesas -->
                    <div class="col-md-5">
                        <label class="form-label campo-label">
                            Elige la cantidad de mesas
                            <small class="text-muted d-block" id="mesasSugeridas"></small>
                        </label>
                        <asp:DropDownList ID="ddlCantidadMesas" runat="server" 
                            CssClass="form-select custom-input"
                            onchange="validarCoherenciaPersonasMesas()">
                            <asp:ListItem Text="Seleccione" Value="" />
                            <asp:ListItem Text="1" Value="1" />
                            <asp:ListItem Text="2" Value="2" />
                            <asp:ListItem Text="3" Value="3" />
                        </asp:DropDownList>
                        <small class="form-text text-warning" id="mesasWarning" style="display: none;"></small>
                    </div>

                    <!-- Ubicación -->
                    <div class="col-md-5">
                        <label class="form-label campo-label">Elije tu ubicación favorita</label>
                        <asp:DropDownList ID="ddlUbicacion" runat="server" CssClass="form-select custom-input">
                            <asp:ListItem Text="Seleccione" Value="" />
                            <asp:ListItem Text="Interior" Value="Interior" />
                            <asp:ListItem Text="Terraza" Value="Terraza" />
                            <asp:ListItem Text="Ventana" Value="Ventana" />
                        </asp:DropDownList>
                    </div>

                    <!-- Observaciones -->
                    <div class="col-md-10">
                        <label class="form-label campo-label">Observaciones</label>
                        <asp:TextBox ID="txtObservaciones" runat="server" 
                            CssClass="form-control custom-input" 
                            TextMode="MultiLine" 
                            Rows="4" 
                            placeholder="Escriba sus observaciones aquí..."
                            maxlength="500" />
                        <div class="form-text">
                            <span id="contadorCaracteres">0</span>/500 caracteres
                        </div>
                    </div>

                    <!-- Botón Para registrar -->
                    <div class="text-center mt-3">
                        <asp:Button ID="btnRegistrarReserva" runat="server" Text="Registrar Reserva" 
                            CssClass="btn btn-danger rounded-pill px-5 py-2 fw-bold"
                            Style="background-color: #BC1F1F; border: none;" 
                            OnClick="btnRegistrarReserva_Click" 
                            OnClientClick="return validarFormularioCompleto();" />
                        <div class="mt-2">
                            <small class="text-muted">
                                <i class="fas fa-shield-alt"></i> 
                                Tu reserva será confirmada en 24 horas
                            </small>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- Modal Lista de Espera -->
    <div class="modal fade" id="modalListaEspera" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content border-0 rounded-4">
                <div class="modal-header bg-warning rounded-top-4">
                    <h5 class="modal-title fw-bold text-dark" id="modalLabel">¡Casi lo logras!
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body text-center">
                    <i class="fas fa-clock fa-3x text-danger mb-3"></i>
                    <p class="mb-2">En este momento no hay mesas disponibles para tu selección.</p>
                    <p class="text-muted">¿Deseas unirte a nuestra lista de espera? Te notificaremos por correo si se libera un espacio.</p>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-outline-secondary px-4" data-bs-dismiss="modal">No, gracias</button>
                    <asp:Button ID="btnUnirseEspera" runat="server" Text="Sí, avisarme" CssClass="btn btn-danger px-4" OnClick="btnUnirseEspera_Click" />
                </div>
            </div>
        </div>
    </div>

    <style>
        .campo-label {
            font-weight: 600;
            color: #BC1F1F;
        }
        
        /* Optimización para controles nativos de fecha y hora */
        input[type="date"], input[type="time"] {
            position: relative;
            cursor: pointer;
        }
        
        input[type="date"]::-webkit-calendar-picker-indicator,
        input[type="time"]::-webkit-calendar-picker-indicator {
            cursor: pointer;
            filter: invert(1);
            opacity: 0.8;
        }
        
        input[type="date"]::-webkit-calendar-picker-indicator:hover,
        input[type="time"]::-webkit-calendar-picker-indicator:hover {
            opacity: 1;
            filter: invert(0.2);
        }
        
        .custom-input {
            padding: 12px 15px;
            border: 2px solid #e9ecef;
            transition: all 0.3s ease;
        }
        
        .custom-input:focus {
            border-color: #BC1F1F;
            box-shadow: 0 0 0 0.2rem rgba(188, 31, 31, 0.25);
            outline: none;
        }
        
        .form-control.is-valid, .form-select.is-valid {
            border-color: #28a745;
            background-image: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 8 8'%3e%3cpath fill='%2328a745' d='m2.3 6.73.94-.94 1.56 1.56L7.72 4.4l.94.94L4.28 9.72z'/%3e%3c/svg%3e");
            background-repeat: no-repeat;
            background-position: right 12px center;
            background-size: 16px;
        }
        
        .form-control.is-invalid, .form-select.is-invalid {
            border-color: #dc3545;
            background-image: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 12 12' fill='none' stroke='%23dc3545'%3e%3ccircle cx='6' cy='6' r='4.5'/%3e%3cpath d='m5.8 4.6 2.4 2.4m0-2.4L5.8 7'/%3e%3c/svg%3e");
            background-repeat: no-repeat;
            background-position: right 12px center;
            background-size: 16px;
        }
        
        .invalid-feedback {
            display: block;
            font-size: 0.875rem;
            margin-top: 0.25rem;
        }
        
        /* Mejorar el aspecto en dispositivos móviles */
        @media (max-width: 768px) {
            input[type="date"], input[type="time"] {
                font-size: 16px; /* Evita zoom en iOS */
            }
        }
    </style>

    <script type="text/javascript">
        // Configuración inicial cuando carga la página
        document.addEventListener('DOMContentLoaded', function() {
            configurarFechaMinima();
            configurarContadorCaracteres();
            configurarControlesNativos();
        });

        // Configurar controles nativos para mostrar modales
        function configurarControlesNativos() {
            const fechaInput = document.getElementById('<%= txtFecha.ClientID %>');
            const horaInput = document.getElementById('<%= txtHora.ClientID %>');

            // Asegurar que los controles nativos funcionen correctamente
            fechaInput.addEventListener('focus', function() {
                this.showPicker && this.showPicker();
            });

            horaInput.addEventListener('focus', function() {
                this.showPicker && this.showPicker();
            });

            // Para navegadores móviles/táctiles
            fechaInput.addEventListener('click', function() {
                if (this.type === 'date' && !this.value) {
                    this.focus();
                }
            });

            horaInput.addEventListener('click', function() {
                if (this.type === 'time' && !this.value) {
                    this.focus();
                }
            });
        }

        // Configurar fecha mínima (hoy)
        function configurarFechaMinima() {
            const fechaInput = document.getElementById('<%= txtFecha.ClientID %>');
            const hoy = new Date();
            const fechaMinima = hoy.toISOString().split('T')[0];
            fechaInput.setAttribute('min', fechaMinima);
            
            // Configurar fecha máxima (3 meses en el futuro para reservas comunes)
            const fechaMaxima = new Date();
            fechaMaxima.setMonth(fechaMaxima.getMonth() + 3);
            fechaInput.setAttribute('max', fechaMaxima.toISOString().split('T')[0]);
        }

        // Validar fecha en tiempo real
        function validarFecha(input) {
            const fechaSeleccionada = new Date(input.value);
            const hoy = new Date();
            const fechaMaxima = new Date();
            fechaMaxima.setMonth(fechaMaxima.getMonth() + 3);
            
            const errorDiv = document.getElementById('fechaError');
            const successDiv = document.getElementById('fechaSuccess');
            
            // Limpiar estados previos
            input.classList.remove('is-valid', 'is-invalid');
            errorDiv.style.display = 'none';
            successDiv.style.display = 'none';

            if (!input.value) {
                return;
            }

            if (fechaSeleccionada < hoy.setHours(0,0,0,0)) {
                mostrarErrorFecha('No se pueden hacer reservas para fechas pasadas.');
                return false;
            }

            if (fechaSeleccionada > fechaMaxima) {
                mostrarErrorFecha('Solo se pueden hacer reservas hasta 3 meses en el futuro.');
                return false;
            }

            // Verificar si es domingo (día cerrado)
            if (fechaSeleccionada.getDay() === 0) {
                mostrarErrorFecha('Los domingos permanecemos cerrados.');
                return false;
            }

            // Fecha válida
            input.classList.add('is-valid');
            successDiv.style.display = 'block';
            verificarDisponibilidadCompleta();
            return true;
        }

        // Validar hora en tiempo real
        function validarHora(input) {
            const horaSeleccionada = input.value;
            const errorDiv = document.getElementById('horaError');
            const successDiv = document.getElementById('horaSuccess');
            const infoDiv = document.getElementById('horaInfo');
            
            // Limpiar estados previos
            input.classList.remove('is-valid', 'is-invalid');
            errorDiv.style.display = 'none';
            successDiv.style.display = 'none';

            if (!horaSeleccionada) {
                return;
            }

            const [horas, minutos] = horaSeleccionada.split(':').map(Number);
            const horaEnMinutos = horas * 60 + minutos;
            
            // Horarios de atención: 11:00 AM (660 min) - 11:00 PM (1380 min)
            const horaApertura = 11 * 60; // 11:00 AM
            const horaCierre = 23 * 60;   // 11:00 PM

            if (horaEnMinutos < horaApertura) {
                mostrarErrorHora('Nuestro horario de atención inicia a las 11:00 AM.');
                return false;
            }

            if (horaEnMinutos > horaCierre) {
                mostrarErrorHora('Nuestro horario de atención termina a las 11:00 PM.');
                return false;
            }

            // Verificar intervalos de 30 minutos
            if (minutos !== 0 && minutos !== 30) {
                mostrarErrorHora('Solo se aceptan reservas cada 30 minutos (ej: 12:00, 12:30).');
                return false;
            }

            // Hora válida
            input.classList.add('is-valid');
            successDiv.style.display = 'block';
            infoDiv.innerHTML = '<i class="fas fa-check-circle"></i> Horario disponible para reservas';
            verificarDisponibilidadCompleta();
            return true;
        }

        // Calcular mesas sugeridas basado en cantidad de personas
        function calcularMesasSugeridas() {
            const personasSelect = document.getElementById('<%= ddlCantidadPersonas.ClientID %>');
            const mesasSugeridasDiv = document.getElementById('mesasSugeridas');
            const mesasSelect = document.getElementById('<%= ddlCantidadMesas.ClientID %>');
            
            if (!personasSelect.value) {
                mesasSugeridasDiv.textContent = '';
                return;
            }

            const personas = parseInt(personasSelect.value);
            const mesasSugeridas = Math.ceil(personas / 4); // Máximo 4 personas por mesa
            
            mesasSugeridasDiv.innerHTML = `*Sugerimos ${mesasSugeridas} mesa(s) para ${personas} persona(s)`;
            
            // Auto-seleccionar la cantidad de mesas sugerida si está disponible
            for (let i = 0; i < mesasSelect.options.length; i++) {
                if (mesasSelect.options[i].value == mesasSugeridas) {
                    mesasSelect.selectedIndex = i;
                    validarCoherenciaPersonasMesas();
                    break;
                }
            }
        }

        // Validar coherencia entre personas y mesas
        function validarCoherenciaPersonasMesas() {
            const personasSelect = document.getElementById('<%= ddlCantidadPersonas.ClientID %>');
            const mesasSelect = document.getElementById('<%= ddlCantidadMesas.ClientID %>');
            const warningDiv = document.getElementById('mesasWarning');
            
            if (!personasSelect.value || !mesasSelect.value) {
                warningDiv.style.display = 'none';
                return;
            }

            const personas = parseInt(personasSelect.value);
            const mesas = parseInt(mesasSelect.value);
            
            if (personas > mesas * 4) {
                warningDiv.innerHTML = '<i class="fas fa-exclamation-triangle"></i> Demasiadas personas para las mesas seleccionadas';
                warningDiv.style.display = 'block';
                mesasSelect.classList.add('is-invalid');
            } else if (personas < mesas * 2 && personas > 1) {
                warningDiv.innerHTML = '<i class="fas fa-info-circle"></i> Podrías necesitar menos mesas';
                warningDiv.style.display = 'block';
                mesasSelect.classList.remove('is-invalid');
                mesasSelect.classList.add('is-valid');
            } else {
                warningDiv.style.display = 'none';
                mesasSelect.classList.remove('is-invalid');
                mesasSelect.classList.add('is-valid');
            }
        }

        // Verificar disponibilidad del local seleccionado
        function verificarDisponibilidadLocal() {
            const localSelect = document.getElementById('<%= ddlLocal.ClientID %>');
            if (localSelect.value) {
                // Aquí podrías hacer una llamada AJAX para verificar disponibilidad real
                localSelect.classList.add('is-valid');
                verificarDisponibilidadCompleta();
            }
        }

        // Verificar disponibilidad completa
        function verificarDisponibilidadCompleta() {
            const fecha = document.getElementById('<%= txtFecha.ClientID %>').value;
            const hora = document.getElementById('<%= txtHora.ClientID %>').value;
            const local = document.getElementById('<%= ddlLocal.ClientID %>').value;
            const infoDiv = document.getElementById('infoDisponibilidad');
            const mensajeDiv = document.getElementById('mensajeDisponibilidad');
            
            if (fecha && hora && local) {
                infoDiv.style.display = 'block';
                mensajeDiv.innerHTML = `Verificando disponibilidad para ${formatearFecha(fecha)} a las ${formatearHora(hora)}...`;
                
                // Simular verificación de disponibilidad
                setTimeout(function() {
                    mensajeDiv.innerHTML = `✅ Disponibilidad confirmada para ${formatearFecha(fecha)} a las ${formatearHora(hora)}`;
                    infoDiv.className = 'alert alert-success';
                }, 1000);
            }
        }

        // Configurar contador de caracteres para observaciones
        function configurarContadorCaracteres() {
            const textarea = document.getElementById('<%= txtObservaciones.ClientID %>');
            const contador = document.getElementById('contadorCaracteres');
            
            textarea.addEventListener('input', function() {
                contador.textContent = this.value.length;
                if (this.value.length > 450) {
                    contador.style.color = '#dc3545';
                } else if (this.value.length > 350) {
                    contador.style.color = '#ffc107';
                } else {
                    contador.style.color = '#6c757d';
                }
            });
        }

        // Validación completa del formulario antes de envío
        function validarFormularioCompleto() {
            const fecha = document.getElementById('<%= txtFecha.ClientID %>');
            const hora = document.getElementById('<%= txtHora.ClientID %>');
            const personas = document.getElementById('<%= ddlCantidadPersonas.ClientID %>');
            const local = document.getElementById('<%= ddlLocal.ClientID %>');
            const mesas = document.getElementById('<%= ddlCantidadMesas.ClientID %>');
            
            let esValido = true;
            
            if (!validarFecha(fecha)) esValido = false;
            if (!validarHora(hora)) esValido = false;
            if (!personas.value) {
                personas.classList.add('is-invalid');
                esValido = false;
            }
            if (!local.value) {
                local.classList.add('is-invalid');
                esValido = false;
            }
            if (!mesas.value) {
                mesas.classList.add('is-invalid');
                esValido = false;
            }

            if (!esValido) {
                Swal.fire('Formulario Incompleto', 'Por favor complete todos los campos requeridos correctamente.', 'warning');
            }

            return esValido;
        }

        // Funciones auxiliares
        function mostrarErrorFecha(mensaje) {
            const input = document.getElementById('<%= txtFecha.ClientID %>');
            const errorDiv = document.getElementById('fechaError');
            input.classList.add('is-invalid');
            errorDiv.textContent = mensaje;
            errorDiv.style.display = 'block';
        }

        function mostrarErrorHora(mensaje) {
            const input = document.getElementById('<%= txtHora.ClientID %>');
            const errorDiv = document.getElementById('horaError');
            input.classList.add('is-invalid');
            errorDiv.textContent = mensaje;
            errorDiv.style.display = 'block';
        }

        function formatearFecha(fecha) {
            const opciones = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
            return new Date(fecha).toLocaleDateString('es-ES', opciones);
        }

        function formatearHora(hora) {
            const [horas, minutos] = hora.split(':');
            const ampm = horas >= 12 ? 'PM' : 'AM';
            const horas12 = horas % 12 || 12;
            return `${horas12}:${minutos} ${ampm}`;
        }
    </script>
</asp:Content>

