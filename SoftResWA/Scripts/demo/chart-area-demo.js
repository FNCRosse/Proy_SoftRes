// Nuevos valores por defecto
Chart.defaults.font.family = 'Nunito, -apple-system, system-ui, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif';
Chart.defaults.color = '#858796';

// Formato de número
function number_format(number, decimals, dec_point, thousands_sep) {
    number = (number + '').replace(',', '').replace(' ', '');
    var n = !isFinite(+number) ? 0 : +number,
        prec = Math.abs(decimals),
        sep = thousands_sep || ',',
        dec = dec_point || '.',
        s = '',
        toFixedFix = function (n, prec) {
            return (Math.round(n * Math.pow(10, prec)) / Math.pow(10, prec)).toFixed(prec);
        };

    s = (prec ? toFixedFix(n, prec) : '' + Math.round(n)).split('.');
    s[0] = s[0].replace(/\B(?=(\d{3})+(?!\d))/g, sep);
    if ((s[1] || '').length < prec) {
        s[1] = (s[1] || '') + Array(prec - s[1].length + 1).join('0');
    }
    return s.join(dec);
}

// Área Chart
const ctx = document.getElementById("myAreaChart").getContext('2d');
const myLineChart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
        datasets: [{
            label: "Reservas",
            data: [0, 10, 5, 15, 10, 20, 15, 25, 20, 30, 25, 40],
            tension: 0.3,
            backgroundColor: "rgba(78, 115, 223, 0.05)",
            borderColor: "#42A5F5",
            pointRadius: 3,
            pointBackgroundColor: "#42A5F5",
            pointBorderColor: "#42A5F5",
            pointHoverRadius: 3,
            pointHoverBackgroundColor: "#42A5F5",
            pointHoverBorderColor: "#42A5F5",
            pointHitRadius: 10,
            pointBorderWidth: 2
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false,
        layout: {
            padding: {
                left: 10,
                right: 25,
                top: 25,
                bottom: 0
            }
        },
        scales: {
            x: {
                grid: {
                    display: false,
                    drawBorder: false
                },
                ticks: {
                    maxTicksLimit: 12
                }
            },
            y: {
                grid: {
                    color: "rgb(234, 236, 244)",
                    zeroLineColor: "rgb(234, 236, 244)",
                    drawBorder: false,
                    borderDash: [2],
                    zeroLineBorderDash: [2]
                },
                ticks: {
                    maxTicksLimit: 5,
                    padding: 10,
                    callback: function (value) {
                        return number_format(value);
                    }
                }
            }
        },
        plugins: {
            legend: {
                display: false
            },
            tooltip: {
                backgroundColor: "rgb(255,255,255)",
                titleColor: '#6e707e',
                bodyColor: "#858796",
                borderColor: '#dddfeb',
                borderWidth: 1,
                padding: 15,
                displayColors: false,
                intersect: false,
                mode: 'index',
                callbacks: {
                    label: function (context) {
                        const label = context.dataset.label || '';
                        return label + ': ' + number_format(context.parsed.y);
                    }
                }
            }
        }
    }
});
