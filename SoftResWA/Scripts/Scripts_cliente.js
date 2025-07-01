/*Boton de Reserva aqui */
document.addEventListener("DOMContentLoaded", function () {
    const dropdown = document.getElementById("dropdownReserva");
    const menu = dropdown?.nextElementSibling;

    if (dropdown && menu) {
        dropdown.addEventListener("click", function (e) {
            e.preventDefault();
            const isShown = menu.classList.contains("show");
            document.querySelectorAll(".dropdown-anim").forEach(m => m.classList.remove("show"));
            if (!isShown) menu.classList.add("show");
        });

        document.addEventListener("click", function (e) {
            if (!dropdown.contains(e.target) && !menu.contains(e.target)) {
                menu.classList.remove("show");
            }
        });
    }
});
/*Boton de notificaciones*/
document.addEventListener("DOMContentLoaded", function () {
    const btnNoti = document.getElementById("notificacionesDropdown");
    const menu = document.getElementById("notificacionesMenu");
    const badge = document.getElementById("notiBadge");
    const lista = document.getElementById("notificacionesLista");

    btnNoti.addEventListener("click", function (e) {
        e.preventDefault();
        menu.classList.toggle("show");

        // Al abrir, marcamos como leídas (opcional)
        if (menu.classList.contains("show")) {
            badge.style.display = "none";
        }
    });

    // Cierra al hacer clic fuera
    document.addEventListener("click", function (e) {
        if (!btnNoti.contains(e.target) && !menu.contains(e.target)) {
            menu.classList.remove("show");
        }
    });

    // Opcional: contar cuántas notificaciones hay y mostrarlo dinámicamente
    const cantidad = lista.querySelectorAll("li").length;
    badge.textContent = cantidad;
    badge.style.display = cantidad > 0 ? "inline-block" : "none";
});
/*Google maps para locales */
let map;
let marker;
let geocoder;

function initMap() {
    const initialDireccion = "Av. Javier Prado Este 123, San Isidro";

    map = new google.maps.Map(document.getElementById("mapa"), {
        zoom: 15,
        center: { lat: -12.097222, lng: -77.033333 }, // por si geocode falla
    });

    marker = new google.maps.Marker({
        map: map,
    });

    geocoder = new google.maps.Geocoder();

    geocodeDireccion(initialDireccion, function (location) {
        map.setCenter(location);
        marker.setPosition(location);
    });
}

function geocodeDireccion(direccion, callback) {
    geocoder.geocode({ address: direccion }, function (results, status) {
        if (status === "OK") {
            callback(results[0].geometry.location);
        } else {
            console.error("Error al geocodificar:", status);
            alert("No se pudo mostrar la dirección: " + direccion);
        }
    });
}

document.addEventListener("DOMContentLoaded", function () {
    const cards = document.querySelectorAll(".card-local");

    cards.forEach(card => {
        card.addEventListener("click", () => {
            // Cambiar estilo visual
            cards.forEach(c => c.classList.remove("card-local-activa"));
            card.classList.add("card-local-activa");

            // Obtener dirección
            const direccion = card.dataset.direccion;
            geocodeDireccion(direccion, function (location) {
                map.setCenter(location);
                marker.setPosition(location);
            });
        });
    });
});
/*Autentificacion de google */

//const firebaseConfig = {
//    apiKey: "AIzaSyAyXwIHz65o0VK2BIP0hKuVKFmnr6d64Sw",
//    authDomain: "proyect-softres-e4cf7.firebaseapp.com",
//    projectId: "proyect-softres-e4cf7",
//    storageBucket: "proyect-softres-e4cf7.firebasestorage.app",
//    messagingSenderId: "254654882272",
//    appId: "1:254654882272:web:6ed1a81b9d8797b54f0b85",
//    measurementId: "G-P8SM887XYK"
//};

//firebase.initializeApp(firebaseConfig);

//function loginConGoogle() {
//    const provider = new firebase.auth.GoogleAuthProvider();
//    firebase.auth().signInWithPopup(provider)
//        .then((result) => {
//            const user = result.user;
//            alert("Bienvenido, " + user.displayName);
//            window.location.href = "/Views/Cliente/Home/Home_Cliente.aspx";
//        })
//        .catch((error) => {
//            console.error("Error con Google:", error);
//            alert("Ocurrió un error al iniciar sesión.");
//        });
//}
/*Modal de inicio de session y registro */
function togglePassword(inputId, icon) {
    const input = document.getElementById(inputId);
    const isHidden = input.type === "password";
    input.type = isHidden ? "text" : "password";
    icon.classList.toggle("fa-eye");
    icon.classList.toggle("fa-eye-slash");
}
/*Activar modal si no hay mesas disponibles*/
function simularDisponibilidad() {
    // Aquí puedes colocar lógica real con AJAX si luego conectas a BD
    // Por ahora, siempre simula que no hay disponibilidad
    let modal = new bootstrap.Modal(document.getElementById('modalListaEspera'));
    modal.show();
}
