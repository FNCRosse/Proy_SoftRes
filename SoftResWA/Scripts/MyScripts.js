
function mostrarTipoReserva() {
    Swal.fire({
        title: '¿Qué tipo de reserva desea crear?',
        showDenyButton: true,
        showCancelButton: false,
        confirmButtonText: 'Reserva Común',
        denyButtonText: `Reserva Evento`
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = 'registrar_reserva_comun.aspx';
        } else if (result.isDenied) {
            window.location.href = 'registrar_reserva_evento.aspx';
        }
    });
}

function confirmarCancelacionReserva(idReserva) {
    Swal.fire({
        title: '¿Desea cancelar la reservación?',
        showCancelButton: true,
        confirmButtonText: 'Sí',
        cancelButtonText: 'No',
        icon: 'warning'
    }).then((result) => {
        if (result.isConfirmed) {
            solicitarMotivoCancelacion(idReserva);
        }
    });
}

function solicitarMotivoCancelacion(idReserva) {
    Swal.fire({
        title: 'Motivo de la cancelación',
        input: 'textarea',
        inputPlaceholder: 'Escriba el motivo...',
        showCancelButton: true,
        confirmButtonText: 'Guardar',
        cancelButtonText: 'Cancelar',
        inputValidator: (value) => {
            if (!value) {
                return 'Debe ingresar un motivo.';
            }
        }
    }).then((result) => {
        if (result.isConfirmed) {
            // Llamar al servidor para cancelar
            __doPostBack('CancelarReserva', idReserva + '|' + result.value);
        }
    });
}

function modificarReserva(idReserva) {
    window.location.href = 'modificar_reserva.aspx?id=' + idReserva;
}

function confirmarEliminacion(id, hiddenFieldId, buttonId) {
    Swal.fire({
        title: '¿Estás seguro?',
        text: 'Esta acción pondra en inactivo la entidad',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#aaa',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            document.getElementById(hiddenFieldId).value = id;
            document.getElementById(buttonId).click();
        }
    });
}
