// Estilos por defecto compatibles con Bootstrap 5
Chart.defaults.font.family = 'Nunito, -apple-system, system-ui, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif';
Chart.defaults.color = '#858796';

// Pie Chart Example para Chart.js v3 o superior
const pieChartCtx = document.getElementById("myPieChart").getContext('2d');
const myPieChart = new Chart(pieChartCtx, {
    type: 'doughnut',
    data: {
        labels: ["San Miguel", "Callao", "Miraflores", "Ate"],
        datasets: [{
            data: [55, 30, 15, 10],
            backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc', '#e74a3b'],
            hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf', '#be2617'],
            borderColor: "rgba(234, 236, 244, 1)",
            borderWidth: 1,
        }]
    },
    options: {
        maintainAspectRatio: false,
        cutout: '80%',
        plugins: {
            tooltip: {
                backgroundColor: "rgb(255,255,255)",
                bodyColor: "#858796",
                borderColor: '#dddfeb',
                borderWidth: 1,
                padding: 15,
                displayColors: false,
                caretPadding: 10
            },
            legend: {
                display: false
            }
        }
    }
});

