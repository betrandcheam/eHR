﻿require(['Config'], function () {
    require(['stage1.careerdevelopment']);
}),
define("stage1.careerdevelopment", ['jquery', 'bootstrap'], function ($) {
    var message = $("#forRazorValue").attr("message");
        var savefunction = function () {
            $.ajax({
                url: $("#forRazorValue").attr("saveurl"),
                type: "POST",
                dataType: "Json",
                data: { "ApprID": $("#forRazorValue").attr("apprid"), "SectionID": $("#sectionlist li.active a").attr("sectionid"), "ShorttermCareerGoal": $("#ShorttermCareerGoal").val(), "DevelopmentPlan": $("#DevelopmentPlan").val(), "Learninganddevelopment": $("#Learninganddevelopment").val() },
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
        var autosavefunction = function () {
            $.ajax({
                url: $("#forRazorValue").attr("saveurl"),
                type: "POST",
                dataType: "Json",
                data: { "ApprID": $("#forRazorValue").attr("apprid"), "SectionID": $("#sectionlist li.active a").attr("sectionid"), "ShorttermCareerGoal": $("#ShorttermCareerGoal").val(), "DevelopmentPlan": $("#DevelopmentPlan").val(), "Learninganddevelopment": $("#Learninganddevelopment").val() },
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
            if (message)
                $('#RedirectModal').modal();
            $('body').scrollspy({ target: '#sidenav' });
            $('button[data-loading-text]').click(function () {
                $(this).button('loading');
            });
            $(".Next").click(function () {
                $("#stage1kpisubmit").trigger('click');
            });
            $("#stage1kpisubmit").click(function () {
                $(".form-control").removeClass("warningclass");
                var flag = true;
                var errorplace = new Object();
                $.each($(".form-control"), function () {
                    if ($.trim($(this).val()) == "") {
                        flag = false;
                        errorplace = $(this);
                        $(this).addClass("warningclass");
                        return false;
                    }
                });
                if (flag)
                    $('#SubmitInfoModal').modal();
                else {
                    $("html,body").animate({ scrollTop: errorplace.offset().top + "px" });
                }
            });
            $("#stage1kpisave").click(function () {
                $(".form-control").removeClass("warningclass");
                savefunction();
            });
            $("#btn_appraisal_cancel").click(function () {
                $('#CancelInfoModal').modal();
            });

            $("#btn_cancel_modal_ok").click(function () {
                window.location.href($("#forRazorValue").attr("rooturl"));
            });
        });
        //setInterval(autosavefunction, 300000); 
});