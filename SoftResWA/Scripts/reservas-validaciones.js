// ===============================================
// VALIDACIONES Y FUNCIONES PARA GESTIÓN DE RESERVAS
// ===============================================

// Configuración global
const ReservasConfig = {
    fechaMinima: new Date(),
    horaMinima: "08:00",
    horaMaxima: "23:00",
    diasMaximosReserva: 60,
    mensajes: {
        fechaInvalida: "Por favor seleccione una fecha válida",
        fechaPasada: "No se pueden hacer reservas para fechas pasadas",
        fechaMuyLejana: "No se pueden hacer reservas con más de 60 días de anticipación",
        horaInvalida: "La hora debe estar entre 08:00 y 23:00",
        campoRequerido: "Este campo es obligatorio",
        dniInvalido: "El DNI debe tener exactamente 8 dígitos",
        emailInvalido: "Por favor ingrese un email válido",
        telefonoInvalido: "El teléfono debe tener entre 7 y 9 dígitos",
        personasInvalidas: "La cantidad de personas debe ser entre 1 y 20",
        mesasInvalidas: "La cantidad de mesas debe ser entre 1 y 10"
    }
};

// Funciones de validación básicas
const Validaciones = {
    // Validar fecha
    validarFecha: function(fecha, permitirHoy = false) {
        if (!fecha) return { valido: false, mensaje: ReservasConfig.mensajes.fechaInvalida };
        
        const fechaSeleccionada = new Date(fecha);
        const hoy = new Date();
        hoy.setHours(0, 0, 0, 0);
        
        const fechaMaxima = new Date();
        fechaMaxima.setDate(fechaMaxima.getDate() + ReservasConfig.diasMaximosReserva);
        
        if (!permitirHoy && fechaSeleccionada < hoy) {
            return { valido: false, mensaje: ReservasConfig.mensajes.fechaPasada };
        }
        
        if (fechaSeleccionada > fechaMaxima) {
            return { valido: false, mensaje: ReservasConfig.mensajes.fechaMuyLejana };
        }
        
        return { valido: true };
    },

    // Validar hora
    validarHora: function(hora) {
        if (!hora) return { valido: false, mensaje: ReservasConfig.mensajes.horaInvalida };
        
        const [horas, minutos] = hora.split(':').map(Number);
        const [horaMinHoras, horaMinMinutos] = ReservasConfig.horaMinima.split(':').map(Number);
        const [horaMaxHoras, horaMaxMinutos] = ReservasConfig.horaMaxima.split(':').map(Number);
        
        const horaTotal = horas * 60 + minutos;
        const horaMinTotal = horaMinHoras * 60 + horaMinMinutos;
        const horaMaxTotal = horaMaxHoras * 60 + horaMaxMinutos;
        
        if (horaTotal < horaMinTotal || horaTotal > horaMaxTotal) {
            return { valido: false, mensaje: ReservasConfig.mensajes.horaInvalida };
        }
        
        return { valido: true };
    },

    // Validar DNI
    validarDNI: function(dni) {
        if (!dni) return { valido: false, mensaje: ReservasConfig.mensajes.campoRequerido };
        
        const dniLimpio = dni.replace(/\D/g, '');
        
        if (dniLimpio.length !== 8) {
            return { valido: false, mensaje: ReservasConfig.mensajes.dniInvalido };
        }
        
        return { valido: true };
    },

    // Validar email
    validarEmail: function(email) {
        if (!email) return { valido: false, mensaje: ReservasConfig.mensajes.campoRequerido };
        
        const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        
        if (!regex.test(email)) {
            return { valido: false, mensaje: ReservasConfig.mensajes.emailInvalido };
        }
        
        return { valido: true };
    },

    // Validar teléfono
    validarTelefono: function(telefono) {
        if (!telefono) return { valido: false, mensaje: ReservasConfig.mensajes.campoRequerido };
        
        const telefonoLimpio = telefono.replace(/\D/g, '');
        
        if (telefonoLimpio.length < 7 || telefonoLimpio.length > 9) {
            return { valido: false, mensaje: ReservasConfig.mensajes.telefonoInvalido };
        }
        
        return { valido: true };
    },

    // Validar cantidad de personas
    validarPersonas: function(personas) {
        const num = parseInt(personas);
        
        if (isNaN(num) || num < 1 || num > 20) {
            return { valido: false, mensaje: ReservasConfig.mensajes.personasInvalidas };
        }
        
        return { valido: true };
    },

    // Validar cantidad de mesas
    validarMesas: function(mesas) {
        const num = parseInt(mesas);
        
        if (isNaN(num) || num < 1 || num > 10) {
            return { valido: false, mensaje: ReservasConfig.mensajes.mesasInvalidas };
        }
        
        return { valido: true };
    }
};

// Funciones de UI y UX
const ReservasUI = {
    // Mostrar error en campo
    mostrarError: function(campo, mensaje) {
        this.limpiarError(campo);
        
        campo.classList.add('is-invalid');
        
        // Crear mensaje de error
        const errorDiv = document.createElement('div');
        errorDiv.className = 'invalid-feedback';
        errorDiv.textContent = mensaje;
        
        campo.parentNode.appendChild(errorDiv);
    },

    // Limpiar error de campo
    limpiarError: function(campo) {
        campo.classList.remove('is-invalid');
        
        const errorMsg = campo.parentNode.querySelector('.invalid-feedback');
        if (errorMsg) {
            errorMsg.remove();
        }
    },

    // Mostrar mensaje de éxito
    mostrarExito: function(titulo, mensaje, callback = null) {
        Swal.fire({
            title: titulo,
            text: mensaje,
            icon: 'success',
            confirmButtonText: 'Ok',
            confirmButtonColor: '#dc3545'
        }).then((result) => {
            if (result.isConfirmed && callback) {
                callback();
            }
        });
    },

    // Mostrar mensaje de error
    mostrarErrorGeneral: function(titulo, mensaje) {
        Swal.fire({
            title: titulo,
            text: mensaje,
            icon: 'error',
            confirmButtonText: 'Ok',
            confirmButtonColor: '#dc3545'
        });
    },

    // Mostrar loading
    mostrarLoading: function(mensaje = 'Procesando...') {
        Swal.fire({
            title: mensaje,
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });
    },

    // Cerrar loading
    cerrarLoading: function() {
        Swal.close();
    },

    // Confirmar acción
    confirmarAccion: function(titulo, mensaje, callback) {
        Swal.fire({
            title: titulo,
            text: mensaje,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#dc3545',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Sí, confirmar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                callback();
            }
        });
    }
};

// Funciones para formularios de reserva
const FormularioReserva = {
    // Inicializar validaciones
    inicializar: function() {
        this.configurarEventos();
        this.configurarMascaras();
    },

    // Configurar eventos de validación
    configurarEventos: function() {
        // Validación de fechas
        const camposFecha = document.querySelectorAll('input[type="date"]');
        camposFecha.forEach(campo => {
            campo.addEventListener('change', (e) => {
                const validacion = Validaciones.validarFecha(e.target.value);
                if (!validacion.valido) {
                    ReservasUI.mostrarError(e.target, validacion.mensaje);
                } else {
                    ReservasUI.limpiarError(e.target);
                }
            });
        });

        // Validación de horas
        const camposHora = document.querySelectorAll('input[type="time"]');
        camposHora.forEach(campo => {
            campo.addEventListener('change', (e) => {
                const validacion = Validaciones.validarHora(e.target.value);
                if (!validacion.valido) {
                    ReservasUI.mostrarError(e.target, validacion.mensaje);
                } else {
                    ReservasUI.limpiarError(e.target);
                }
            });
        });

        // Validación de DNI
        const camposDNI = document.querySelectorAll('input[data-tipo="dni"]');
        camposDNI.forEach(campo => {
            campo.addEventListener('blur', (e) => {
                const validacion = Validaciones.validarDNI(e.target.value);
                if (!validacion.valido) {
                    ReservasUI.mostrarError(e.target, validacion.mensaje);
                } else {
                    ReservasUI.limpiarError(e.target);
                }
            });
        });

        // Validación de email
        const camposEmail = document.querySelectorAll('input[type="email"]');
        camposEmail.forEach(campo => {
            campo.addEventListener('blur', (e) => {
                const validacion = Validaciones.validarEmail(e.target.value);
                if (!validacion.valido) {
                    ReservasUI.mostrarError(e.target, validacion.mensaje);
                } else {
                    ReservasUI.limpiarError(e.target);
                }
            });
        });

        // Validación de teléfono
        const camposTelefono = document.querySelectorAll('input[data-tipo="telefono"]');
        camposTelefono.forEach(campo => {
            campo.addEventListener('blur', (e) => {
                const validacion = Validaciones.validarTelefono(e.target.value);
                if (!validacion.valido) {
                    ReservasUI.mostrarError(e.target, validacion.mensaje);
                } else {
                    ReservasUI.limpiarError(e.target);
                }
            });
        });
    },

    // Configurar máscaras de entrada
    configurarMascaras: function() {
        // Máscara para DNI
        const camposDNI = document.querySelectorAll('input[data-tipo="dni"]');
        camposDNI.forEach(campo => {
            campo.addEventListener('input', (e) => {
                let valor = e.target.value.replace(/\D/g, '');
                if (valor.length > 8) valor = valor.substring(0, 8);
                e.target.value = valor;
            });
        });

        // Máscara para teléfono
        const camposTelefono = document.querySelectorAll('input[data-tipo="telefono"]');
        camposTelefono.forEach(campo => {
            campo.addEventListener('input', (e) => {
                let valor = e.target.value.replace(/\D/g, '');
                if (valor.length > 9) valor = valor.substring(0, 9);
                e.target.value = valor;
            });
        });

        // Máscara para cantidad de personas
        const camposPersonas = document.querySelectorAll('input[data-tipo="personas"]');
        camposPersonas.forEach(campo => {
            campo.addEventListener('input', (e) => {
                let valor = e.target.value.replace(/\D/g, '');
                if (valor.length > 2) valor = valor.substring(0, 2);
                e.target.value = valor;
            });
        });
    },

    // Validar formulario completo
    validarFormulario: function(formulario) {
        let esValido = true;
        const errores = [];

        // Limpiar errores previos
        const camposConError = formulario.querySelectorAll('.is-invalid');
        camposConError.forEach(campo => ReservasUI.limpiarError(campo));

        // Validar campos requeridos
        const camposRequeridos = formulario.querySelectorAll('[required]');
        camposRequeridos.forEach(campo => {
            if (!campo.value.trim()) {
                ReservasUI.mostrarError(campo, ReservasConfig.mensajes.campoRequerido);
                esValido = false;
            }
        });

        // Validaciones específicas
        const campoFecha = formulario.querySelector('input[type="date"]');
        if (campoFecha && campoFecha.value) {
            const validacion = Validaciones.validarFecha(campoFecha.value);
            if (!validacion.valido) {
                ReservasUI.mostrarError(campoFecha, validacion.mensaje);
                esValido = false;
            }
        }

        const campoHora = formulario.querySelector('input[type="time"]');
        if (campoHora && campoHora.value) {
            const validacion = Validaciones.validarHora(campoHora.value);
            if (!validacion.valido) {
                ReservasUI.mostrarError(campoHora, validacion.mensaje);
                esValido = false;
            }
        }

        return esValido;
    },

    // Enviar formulario con validación
    enviarFormulario: function(formulario, callback) {
        if (this.validarFormulario(formulario)) {
            ReservasUI.mostrarLoading('Guardando reserva...');
            
            // Simular delay para mejor UX
            setTimeout(() => {
                callback();
            }, 1500);
        } else {
            ReservasUI.mostrarErrorGeneral('Error de validación', 'Por favor corrija los errores en el formulario');
        }
    }
};

// Funciones para gestión de estados de reserva
const EstadosReserva = {
    obtenerClaseEstado: function(estado) {
        const clases = {
            'PENDIENTE': 'badge bg-warning text-dark',
            'CONFIRMADA': 'badge bg-success',
            'CANCELADA': 'badge bg-danger'
        };
        
        return clases[estado] || 'badge bg-secondary';
    },

    obtenerIconoEstado: function(estado) {
        const iconos = {
            'PENDIENTE': 'fas fa-clock',
            'CONFIRMADA': 'fas fa-check-circle',
            'CANCELADA': 'fas fa-times-circle'
        };
        
        return iconos[estado] || 'fas fa-question-circle';
    },

    obtenerTextoEstado: function(estado) {
        const textos = {
            'PENDIENTE': 'Pendiente de confirmación',
            'CONFIRMADA': 'Reserva confirmada',
            'CANCELADA': 'Reserva cancelada'
        };
        
        return textos[estado] || 'Estado desconocido';
    }
};

// Funciones de utilidad
const Utils = {
    // Formatear fecha para mostrar
    formatearFecha: function(fecha) {
        if (!fecha) return '';
        const date = new Date(fecha);
        return date.toLocaleDateString('es-PE', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric'
        });
    },

    // Formatear hora para mostrar
    formatearHora: function(hora) {
        if (!hora) return '';
        return hora.substring(0, 5); // HH:mm
    },

    // Calcular días hasta la reserva
    diasHastaReserva: function(fechaReserva) {
        const hoy = new Date();
        const fecha = new Date(fechaReserva);
        const diferencia = Math.ceil((fecha - hoy) / (1000 * 60 * 60 * 24));
        return diferencia;
    },

    // Verificar si es día de la semana
    esDiaSemana: function(fecha) {
        const date = new Date(fecha);
        const dia = date.getDay();
        return dia >= 1 && dia <= 5; // Lunes a Viernes
    },

    // Obtener nombre del día
    obtenerNombreDia: function(fecha) {
        const date = new Date(fecha);
        const dias = ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'];
        return dias[date.getDay()];
    }
};

// Inicialización automática cuando se carga el DOM
document.addEventListener('DOMContentLoaded', function() {
    // Inicializar formularios de reserva
    FormularioReserva.inicializar();
    
    // Configurar tooltips de Bootstrap si existen
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
    
    console.log('Sistema de validaciones de reservas inicializado correctamente');
});

// Exportar para uso global
window.ReservasConfig = ReservasConfig;
window.Validaciones = Validaciones;
window.ReservasUI = ReservasUI;
window.FormularioReserva = FormularioReserva;
window.EstadosReserva = EstadosReserva;
window.Utils = Utils; 