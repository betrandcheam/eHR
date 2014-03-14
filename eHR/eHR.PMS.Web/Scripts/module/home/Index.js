require(['Config'], function () {
    require(['home.index']);
}),
define("home.index", ['jquery', 'bootstrap'], function ($) {
    var message = $("#forRazorValue").attr("message");
    $(function () {
        if (message)
            $('#RedirectModal').modal();
    });
});
            