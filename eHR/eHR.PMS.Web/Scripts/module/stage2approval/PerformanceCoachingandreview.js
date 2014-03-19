require(['Config'], function () {
    require(['stage2approval.performance']);
}),
define("stage2approval.performance", ['jquery', 'bootstrap'], function ($) {
    var savefunction = function () {
            $.ajax({
                url: $("#forRazorValue").attr("saveurl"),
                type: "POST",
                dataType: "Json",
                data: { KPIID: $("#Comments").attr("KPIID"), Comments: $("#Comments").val() },
                beforeSend: function () {
                    //$("#stage1kpisave").button('loading');
                    //$("#buttongroup").showLoading();
                    $('#resultcontent').hide();
                    $('#loadingcontent').show();
                    $('#InfoModal').modal();
                },
                success: function (data) {
                    $("#stage1kpisave").button('reset');
                    $('#loadingcontent').hide();
                    $('#resultcontent').show();
                    //$('#InfoModal').modal();
                }
            });
        };
        /*
        var autosavefunction = function () {
            $.ajax({
                url: $("#forRazorValue").attr("saveurl"),
                type: "POST",
                dataType: "Json",
                data: { KPIID: $("#Comments").attr("KPIID"), Comments: $("#Comments").val() },
                beforeSend: function () {
                    $("#autosaveloading").show('slow');
                    $("#stage1kpisave").attr("disabled", true);
                },
                success: function (data) {
                    $("#autosaveloading").hide('slow');
                    $("#stage1kpisave").attr("disabled", false);
                }
            });
        };
        */
        $(function () {
            $('body').scrollspy({ target: '#sidenav' });
            $('button[data-loading-text]').click(function () {
                $(this).button('loading');
            });
            $("#btn_next_section").click(function () {
                $("form").submit();
            });
            $("#stage1kpiapproval").click(function () {
                $("#ApORRe").val("1");
                $("modalmessage").text("Are you sure approve this now?")
                $('#SubmitInfoModal').modal();
            });
            $("#stage1kpireject").click(function () {
                $("#ApORRe").val("0");
                $("modalmessage").text("Are you sure reject this now?")
                $('#SubmitInfoModal').modal();
            });
            $("#stage1kpisave").click(savefunction);

            $("#btn_appraisal_cancel").click(function () {
                $('#CancelInfoModal').modal();
            });

            $("#btn_cancel_modal_ok").click(function () {
                window.location.href($("#forRazorValue").attr("rooturl"));
            });


        });
        //setInterval(autosavefunction, 300000); 
});