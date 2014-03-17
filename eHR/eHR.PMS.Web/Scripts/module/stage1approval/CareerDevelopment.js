require(['Config'], function () {
    require(['stage1approval.careerdevelopment']);
}),
define("stage1approval.careerdevelopment", ['jquery', 'bootstrap'], function ($) {
    var savefunction = function () {
            $.ajax({
                url: $("#forRazorValue").attr("saveurl"),
                type: "POST",
                dataType: "Json",
                data: { "ApprID": $("#apprid").val(), "SectionID": $("#sectionlist li.active").attr("sectionid"), "Short-termCareerGoal": $("#Short-termCareerGoal").val(), "DevelopmentPlan": $("#DevelopmentPlan").val(), "Learninganddevelopment": $("#Learninganddevelopment").val(), "Comments" : $('#Comments').val(), "KPIID" : $("#Comments").attr("KPIID") },
                beforeSend: function () {
                    //$("#stage1kpisave").button('loading');
                    //$("#buttongroup").showLoading();
                    $('#resultcontent').hide();
                    $('#loadingcontent').show();
                    $('#InfoModal').modal();
                },
                success: function (data) {
                    //$("#stage1kpisave").button('reset');
                    $('#loadingcontent').hide();
                    $('#resultcontent').show();
                    //$('#InfoModal').modal();
                }
            });
        };
        var autosavefunction = function () {
            $.ajax({
                url: $("#forRazorValue").attr("saveurl"),
                type: "POST",
                dataType: "Json",
                data: { "ApprID":$("#apprid").val(),"SectionID":$("#sectionlist li.active").attr("sectionid"),"ShorttermCareerGoal": $("#Short-termCareerGoal").val(), "DevelopmentPlan": $("#DevelopmentPlan").val(), "Learninganddevelopment": $("#Learninganddevelopment").val(), "Comments" : $("#Comments").val(), "KPIID" : $("#Comments").attr("KPIID") },
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
        $(function () {
            $('body').scrollspy({ target: '#sidenav' });
            $('button[data-loading-text]').click(function () {
                $(this).button('loading');
            });
            $("#stage1kpiapproval").click(function () {
                $("#ApORRe").val("1");
                $("#modalmessage").text($("#forRazorValue").attr("approvemessage"))
                $('#SubmitInfoModal').modal();
            });
            $("#stage1kpireject").click(function () {
                $("#ApORRe").val("0");
                $("#modalmessage").text($("#forRazorValue").attr("rejectmessage"))
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