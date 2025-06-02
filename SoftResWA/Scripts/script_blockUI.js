var loader = function (txt) {
    $.blockUI({
        message: txt == undefined ? '<h1><i class="fa fa-spinner fa-pulse fa-3x fa-fw margin-bottom" style="color:cyan"></i></h1>' : '<h1 class="text-white">' + txt + "</h1>",
        overlayCSS: {
            backgroundColor: '#008cba',
            opacity: 0.8,
            cursor: 'wait'
        },
        css: {
            border: 0,
            padding: 0,
            //color: '#fff',
            backgroundColor: 'transparent'
        }
    });
}

var unLoader = function () {
    $.unblockUI();
}