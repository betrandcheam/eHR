require(['Config'], function () {
    require(['home.index']);
}),
define("home.index", ['jquery', 'bootstrap'], function ($) {
    var message = $("#forRazorValue").attr("message");
    $(function () {
        if (message)
            $('#RedirectModal').modal();
    });
    var refreshfunction = function () {
        window.location.href = "";
    }
    setInterval(refreshfunction, parseInt($("#forRazorValue").attr("refreshinterval")));
});
            