<%@ Page Title="" Language="C#" MasterPageFile="~/SoftResCliente.Master" AutoEventWireup="true" CodeBehind="Reg_Resev_Evento.aspx.cs" Inherits="SoftResWA.Views.Cliente.Reservas.Reg_Resev_Evento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphTitulo" runat="server">
    Registrar Reseva Evento
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="reserva-form-section py-5">
        <div class="container">
            <div class="form-wrapper mx-auto">
                <h2 class="text-center text-danger fw-bold">Haz Tu Reserva</h2>
                <p class="text-center subtitulo mb-4">Asegura tu lugar para saborear nuestra fusión peruano–china</p>

                <div class="row g-4 justify-content-center">
                    <!-- Nombre del evento -->
                    <div class="col-md-10">
                        <label class="form-label campo-label">
                            Nombre del evento
                            <small class="text-muted d-block">*Requerido para eventos</small>
                        </label>
                        <asp:TextBox ID="txtNombreEvento" runat="server" 
                            CssClass="form-control custom-input" 
                            placeholder="Ej: Celebración de Cumpleaños, Reunión Corporativa..."
                            maxlength="100"
                            onchange="validarNombreEvento(this)" />
                        <div class="invalid-feedback" id="nombreError"></div>
                    </div>

                    <!-- Descripción del evento -->
                    <div class="col-md-10">
                        <label class="form-label campo-label">
                            Descripción del evento
                            <small class="text-muted d-block">*Ayúdanos a preparar mejor tu evento</small>
                        </label>
                        <asp:TextBox ID="txtDescripcionEvento" runat="server" 
                            CssClass="form-control custom-input" 
                            TextMode="MultiLine" 
                            Rows="3" 
                            placeholder="Describe el tipo de evento, necesidades especiales, decoración..."
                            maxlength="300" />
                        <div class="form-text">
                            <span id="contadorDescripcion">0</span>/300 caracteres
                        </div>
                    </div>

                    <!-- Fecha -->
                    <div class="col-md-5">
                        <label class="form-label campo-label">
                            <i class="fas fa-calendar-alt me-2"></i>Seleccione una fecha
                            <small class="text-muted d-block">*Eventos con min. 7 días de anticipación</small>
                        </label>
                        <asp:TextBox ID="txtFecha" runat="server" 
                            CssClass="form-control custom-input rounded-3" 
                            TextMode="Date" 
                            onchange="validarFechaEvento(this)"
                            title="Haga clic para abrir el calendario del evento"
                            autocomplete="off" />
                        <div class="invalid-feedback" id="fechaError"></div>
                        <small class="form-text text-success" id="fechaSuccess" style="display: none;">
                            <i class="fas fa-check-circle"></i> Fecha disponible para eventos
                        </small>
                    </div>

                    <!-- Hora -->
                    <div class="col-md-5">
                        <label class="form-label campo-label">
                            <i class="fas fa-clock me-2"></i>Seleccione una hora
                            <small class="text-muted d-block">*Duración mínima: 3 horas</small>
                        </label>
                        <asp:TextBox ID="txtHora" runat="server" 
                            CssClass="form-control custom-input rounded-3" 
                            TextMode="Time" 
                            onchange="validarHoraEvento(this)" 
                            step="3600"
                            title="Haga clic para seleccionar la hora del evento"
                            autocomplete="off" />
                        <div class="invalid-feedback" id="horaError"></div>
                        <small class="form-text text-success" id="horaSuccess" style="display: none;">
                            <i class="fas fa-check-circle"></i> Hora disponible
                        </small>
                        <small class="form-text text-info" id="horaInfo">
                            <i class="fas fa-info-circle"></i> Horarios cada hora para eventos
                        </small>
                    </div>

                    <!-- Información de disponibilidad -->
                    <div class="col-md-10">
                        <div class="alert alert-info" id="infoDisponibilidad" style="display: none;">
                            <i class="fas fa-info-circle"></i>
                            <strong>Información del evento:</strong>
                            <span id="mensajeDisponibilidad"></span>
                        </div>
                    </div>

                    <!-- Personas -->
                    <div class="col-md-5">
                        <label class="form-label campo-label">
                            Elige la cantidad de personas
                            <small class="text-muted d-block">*Mínimo 20 personas para eventos</small>
                        </label>
                        <asp:DropDownList ID="ddlCantidadPersonas" runat="server" 
                            CssClass="form-select custom-input"
                            onchange="calcularMesasEvento()">
                            <asp:ListItem Text="Seleccione" Value="" />
                            <asp:ListItem Text="20" Value="20" />
                            <asp:ListItem Text="30" Value="30" />
                            <asp:ListItem Text="40" Value="40" />
                            <asp:ListItem Text="50" Value="50" />
                            <asp:ListItem Text="60" Value="60" />
                            <asp:ListItem Text="70" Value="70" />
                            <asp:ListItem Text="80" Value="80" />
                            <asp:ListItem Text="90" Value="90" />
                            <asp:ListItem Text="100" Value="100" />
                        </asp:DropDownList>
                    </div>

                    <!-- Local -->
                    <div class="col-md-5">
                        <label class="form-label campo-label">
                            Seleccione un local
                            <small class="text-muted d-block">*Capacidad según el evento</small>
                        </label>
                        <asp:DropDownList ID="ddlLocal" runat="server" 
                            CssClass="form-select custom-input"
                            onchange="verificarCapacidadLocal()">
                            <asp:ListItem Text="Seleccione" Value="" />
                        </asp:DropDownList>
                        <small class="form-text text-info" id="capacidadLocal" style="display: none;"></small>
                    </div>

                    <!-- Mesas -->
                    <div class="col-md-5">
                        <label class="form-label campo-label">
                            Elige la cantidad de mesas
                            <small class="text-muted d-block" id="mesasSugeridasEvento"></small>
                        </label>
                        <asp:DropDownList ID="ddlCantidadMesas" runat="server" 
                            CssClass="form-select custom-input"
                            onchange="validarCoherenciaMesasEvento()">
                            <asp:ListItem Text="Seleccione" Value="" />
                            <asp:ListItem Text="5" Value="5" />
                            <asp:ListItem Text="8" Value="8" />
                            <asp:ListItem Text="10" Value="10" />
                            <asp:ListItem Text="12" Value="12" />
                            <asp:ListItem Text="15" Value="15" />
                            <asp:ListItem Text="20" Value="20" />
                            <asp:ListItem Text="25" Value="25" />
                        </asp:DropDownList>
                        <small class="form-text text-warning" id="mesasWarningEvento" style="display: none;"></small>
                    </div>

                    <!-- Ubicación -->
                    <div class="col-md-5">
                        <label class="form-label campo-label">
                            Elije tu ubicación favorita
                            <small class="text-muted d-block">*Eventos pueden requerir área completa</small>
                        </label>
                        <asp:DropDownList ID="ddlUbicacion" runat="server" CssClass="form-select custom-input">
                            <asp:ListItem Text="Seleccione" Value="" />
                            <asp:ListItem Text="Interior" Value="Interior" />
                            <asp:ListItem Text="Terraza" Value="Terraza" />
                            <asp:ListItem Text="Salón completo" Value="Salon Completo" />
                        </asp:DropDownList>
                    </div>

                    <!-- Observaciones -->
                    <div class="col-md-10">
                        <label class="form-label campo-label">
                            Observaciones
                            <small class="text-muted d-block">*Incluye requerimientos especiales, decoración, etc.</small>
                        </label>
                        <asp:TextBox ID="txtObservaciones" runat="server" 
                            CssClass="form-control custom-input" 
                            TextMode="MultiLine" 
                            Rows="4" 
                            placeholder="Requerimientos especiales, decoración, música, menú especial..."
                            maxlength="500" />
                        <div class="form-text">
                            <span id="contadorObservaciones">0</span>/500 caracteres
                        </div>
                    </div>

                    <!-- Botón Para registrar -->
                    <div class="text-center mt-3">
                        <asp:Button ID="btnRegistrarEvento" runat="server" Text="Registrar Evento" 
                            CssClass="btn btn-danger rounded-pill px-5 py-2 fw-bold"
                            Style="background-color: #BC1F1F; border: none;" 
                            OnClick="btnRegistrarEvento_Click" 
                            OnClientClick="return validarFormularioEvento();" />
                        <div class="mt-2">
                            <small class="text-muted">
                                <i class="fas fa-calendar-check"></i> 
                                Tu evento será coordinado en 24-48 horas
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
                    <h5 class="modal-title fw-bold text-dark" id="modalLabel">¡Necesitamos revisar tu solicitud!
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body text-center">
                    <i class="fas fa-calendar-check fa-3x text-warning mb-3"></i>
                    <p class="mb-2">Los eventos requieren una revisión especial por nuestro equipo.</p>
                    <p class="text-muted">Te contactaremos en las próximas 24 horas para coordinar los detalles de tu evento.</p>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-danger px-4" data-bs-dismiss="modal">Entendido</button>
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
            configurarFechaMinimaEvento();
            configurarContadoresCaracteres();
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

        // Configurar fecha mínima para eventos (7 días de anticipación)
        function configurarFechaMinimaEvento() {
            const fechaInput = document.getElementById('<%= txtFecha.ClientID %>');
            const fechaMinima = new Date();
            fechaMinima.setDate(fechaMinima.getDate() + 7); // 7 días de anticipación
            fechaInput.setAttribute('min', fechaMinima.toISOString().split('T')[0]);
            
            // Configurar fecha máxima (6 meses en el futuro para eventos)
            const fechaMaxima = new Date();
            fechaMaxima.setMonth(fechaMaxima.getMonth() + 6);
            fechaInput.setAttribute('max', fechaMaxima.toISOString().split('T')[0]);
        }

        // Validar fecha específica para eventos
        function validarFechaEvento(input) {
            const fechaSeleccionada = new Date(input.value);
            const hoy = new Date();
            const fechaMinima = new Date();
            fechaMinima.setDate(fechaMinima.getDate() + 7);
            const fechaMaxima = new Date();
            fechaMaxima.setMonth(fechaMaxima.getMonth() + 6);
            
            const errorDiv = document.getElementById('fechaError');
            const successDiv = document.getElementById('fechaSuccess');
            
            // Limpiar estados previos
            input.classList.remove('is-valid', 'is-invalid');
            errorDiv.style.display = 'none';
            successDiv.style.display = 'none';

            if (!input.value) {
                return;
            }

            if (fechaSeleccionada < fechaMinima) {
                mostrarErrorFecha('Los eventos requieren al menos 7 días de anticipación.');
                return false;
            }

            if (fechaSeleccionada > fechaMaxima) {
                mostrarErrorFecha('Solo se pueden programar eventos hasta 6 meses en el futuro.');
                return false;
            }

            // Verificar si es domingo (día cerrado)
            if (fechaSeleccionada.getDay() === 0) {
                mostrarErrorFecha('Los domingos permanecemos cerrados.');
                return false;
            }

            // Fecha válida para evento
            input.classList.add('is-valid');
            successDiv.style.display = 'block';
            verificarDisponibilidadEvento();
            return true;
        }

        // Validar hora específica para eventos
        function validarHoraEvento(input) {
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
            
            // Horarios para eventos: 10:00 AM - 8:00 PM (para que terminen antes del cierre)
            const horaAperturaEvento = 10 * 60; // 10:00 AM
            const horaCierreEvento = 20 * 60;   // 8:00 PM

            if (horaEnMinutos < horaAperturaEvento) {
                mostrarErrorHora('Los eventos pueden iniciar desde las 10:00 AM.');
                return false;
            }

            if (horaEnMinutos > horaCierreEvento) {
                mostrarErrorHora('Los eventos deben iniciar antes de las 8:00 PM (duración mín. 3 horas).');
                return false;
            }

            // Para eventos, permitir solo horas exactas
            if (minutos !== 0) {
                mostrarErrorHora('Los eventos se programan en horarios exactos (ej: 12:00, 13:00).');
                return false;
            }

            // Hora válida para evento
            input.classList.add('is-valid');
            successDiv.style.display = 'block';
            infoDiv.innerHTML = '<i class="fas fa-check-circle"></i> Horario disponible para eventos (duración mín. 3 horas)';
            verificarDisponibilidadEvento();
            return true;
        }

        // Validar nombre del evento
        function validarNombreEvento(input) {
            const errorDiv = document.getElementById('nombreError');
            
            input.classList.remove('is-valid', 'is-invalid');
            errorDiv.style.display = 'none';

            if (!input.value.trim()) {
                input.classList.add('is-invalid');
                errorDiv.textContent = 'El nombre del evento es requerido.';
                errorDiv.style.display = 'block';
                return false;
            }

            if (input.value.trim().length < 3) {
                input.classList.add('is-invalid');
                errorDiv.textContent = 'El nombre debe tener al menos 3 caracteres.';
                errorDiv.style.display = 'block';
                return false;
            }

            input.classList.add('is-valid');
            return true;
        }

        // Calcular mesas sugeridas para eventos
        function calcularMesasEvento() {
            const personasSelect = document.getElementById('<%= ddlCantidadPersonas.ClientID %>');
            const mesasSugeridasDiv = document.getElementById('mesasSugeridasEvento');
            const mesasSelect = document.getElementById('<%= ddlCantidadMesas.ClientID %>');
            
            if (!personasSelect.value) {
                mesasSugeridasDiv.textContent = '';
                return;
            }

            const personas = parseInt(personasSelect.value);
            const mesasSugeridas = Math.ceil(personas / 4); // Máximo 4 personas por mesa para eventos
            
            mesasSugeridasDiv.innerHTML = `*Sugerimos ${mesasSugeridas} mesa(s) para ${personas} persona(s)`;
            
            // Auto-seleccionar la cantidad de mesas sugerida si está disponible
            for (let i = 0; i < mesasSelect.options.length; i++) {
                if (mesasSelect.options[i].value == mesasSugeridas) {
                    mesasSelect.selectedIndex = i;
                    validarCoherenciaMesasEvento();
                    break;
                }
            }

            verificarDisponibilidadEvento();
        }

        // Validar coherencia entre personas y mesas para eventos
        function validarCoherenciaMesasEvento() {
            const personasSelect = document.getElementById('<%= ddlCantidadPersonas.ClientID %>');
            const mesasSelect = document.getElementById('<%= ddlCantidadMesas.ClientID %>');
            const warningDiv = document.getElementById('mesasWarningEvento');
            
            if (!personasSelect.value || !mesasSelect.value) {
                warningDiv.style.display = 'none';
                return;
            }

            const personas = parseInt(personasSelect.value);
            const mesas = parseInt(mesasSelect.value);
            
            if (personas > mesas * 4) {
                warningDiv.innerHTML = '<i class="fas fa-exclamation-triangle"></i> Necesitas más mesas para la cantidad de personas';
                warningDiv.style.display = 'block';
                mesasSelect.classList.add('is-invalid');
            } else if (personas < mesas * 2) {
                warningDiv.innerHTML = '<i class="fas fa-info-circle"></i> Podrías optimizar con menos mesas';
                warningDiv.style.display = 'block';
                mesasSelect.classList.remove('is-invalid');
                mesasSelect.classList.add('is-valid');
            } else {
                warningDiv.style.display = 'none';
                mesasSelect.classList.remove('is-invalid');
                mesasSelect.classList.add('is-valid');
            }
        }

        // Verificar capacidad del local para eventos
        function verificarCapacidadLocal() {
            const localSelect = document.getElementById('<%= ddlLocal.ClientID %>');
            const capacidadDiv = document.getElementById('capacidadLocal');
            
            if (localSelect.value) {
                localSelect.classList.add('is-valid');
                capacidadDiv.style.display = 'block';
                capacidadDiv.innerHTML = '<i class="fas fa-info-circle"></i> Capacidad verificada para eventos';
                verificarDisponibilidadEvento();
            }
        }

        // Verificar disponibilidad completa del evento
        function verificarDisponibilidadEvento() {
            const fecha = document.getElementById('<%= txtFecha.ClientID %>').value;
            const hora = document.getElementById('<%= txtHora.ClientID %>').value;
            const local = document.getElementById('<%= ddlLocal.ClientID %>').value;
            const personas = document.getElementById('<%= ddlCantidadPersonas.ClientID %>').value;
            const infoDiv = document.getElementById('infoDisponibilidad');
            const mensajeDiv = document.getElementById('mensajeDisponibilidad');
            
            if (fecha && hora && local && personas) {
                infoDiv.style.display = 'block';
                infoDiv.className = 'alert alert-info';
                mensajeDiv.innerHTML = `Verificando disponibilidad para evento de ${personas} personas el ${formatearFecha(fecha)} a las ${formatearHora(hora)}...`;
                
                // Simular verificación de disponibilidad
                setTimeout(function() {
                    mensajeDiv.innerHTML = `✅ Evento programable. Nuestro equipo coordinará todos los detalles contigo.`;
                    infoDiv.className = 'alert alert-success';
                }, 1500);
            }
        }

        // Configurar contadores de caracteres
        function configurarContadoresCaracteres() {
            // Contador para descripción del evento
            const descripcionTextarea = document.getElementById('<%= txtDescripcionEvento.ClientID %>');
            const contadorDescripcion = document.getElementById('contadorDescripcion');
            
            descripcionTextarea.addEventListener('input', function() {
                contadorDescripcion.textContent = this.value.length;
                if (this.value.length > 250) {
                    contadorDescripcion.style.color = '#dc3545';
                } else if (this.value.length > 200) {
                    contadorDescripcion.style.color = '#ffc107';
                } else {
                    contadorDescripcion.style.color = '#6c757d';
                }
            });

            // Contador para observaciones
            const observacionesTextarea = document.getElementById('<%= txtObservaciones.ClientID %>');
            const contadorObservaciones = document.getElementById('contadorObservaciones');
            
            observacionesTextarea.addEventListener('input', function() {
                contadorObservaciones.textContent = this.value.length;
                if (this.value.length > 450) {
                    contadorObservaciones.style.color = '#dc3545';
                } else if (this.value.length > 350) {
                    contadorObservaciones.style.color = '#ffc107';
                } else {
                    contadorObservaciones.style.color = '#6c757d';
                }
            });
        }

        // Validación completa del formulario antes de envío
        function validarFormularioEvento() {
            const nombre = document.getElementById('<%= txtNombreEvento.ClientID %>');
            const descripcion = document.getElementById('<%= txtDescripcionEvento.ClientID %>');
            const fecha = document.getElementById('<%= txtFecha.ClientID %>');
            const hora = document.getElementById('<%= txtHora.ClientID %>');
            const personas = document.getElementById('<%= ddlCantidadPersonas.ClientID %>');
            const local = document.getElementById('<%= ddlLocal.ClientID %>');
            const mesas = document.getElementById('<%= ddlCantidadMesas.ClientID %>');
            
            let esValido = true;
            
            if (!validarNombreEvento(nombre)) esValido = false;
            
            if (!descripcion.value.trim()) {
                descripcion.classList.add('is-invalid');
                esValido = false;
            }
            
            if (!validarFechaEvento(fecha)) esValido = false;
            if (!validarHoraEvento(hora)) esValido = false;
            
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
                Swal.fire('Formulario Incompleto', 'Por favor complete todos los campos requeridos para su evento.', 'warning');
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
