﻿require(['Config'], function () {
    require(['stage1.careerdevelopment']);
}),
define("stage1.careerdevelopment", ['jquery', 'bootstrap'], function ($) {
    var message = $("#forRazorValue").attr("message");
    var pdfsave = false;
    var savefunction = function () {
        $.ajax({
            url: $("#forRazorValue").attr("saveurl"),
            type: "POST",
            dataType: "Json",
            data: { "ApprID": $("#forRazorValue").attr("apprid"), "SectionID": $("#sectionlist li.active a").attr("sectionid"), "ShorttermCareerGoal": encodeURIComponent($.trim($("#ShorttermCareerGoal").val())), "DevelopmentPlan": encodeURIComponent($.trim($("#DevelopmentPlan").val())), "Learninganddevelopment": encodeURIComponent($.trim($("#Learninganddevelopment").val())) },
            beforeSend: function () {
                //$("#stage1kpisave").button('loading');
                //$("#buttongroup").showLoading();
                if (!pdfsave) {
                    $('#resultcontent').hide();
                    $('#loadingcontent').show();
                    $('#InfoModal').modal();
                }
                else {
                    $('#part1').css("visibility", "visible");
                    $('#spanclass1').css("visibility", "hidden");
                    $('#part2').css("visibility", "hidden");
                    $('#spanclass2').css("visibility", "hidden");
                    $('#modal-footer').hide();
                    $('#PDFModal').modal();
                }
            },
            success: function (data) {
                if (!pdfsave) {
                    $("#stage1kpisave").button('reset');
                    $('#loadingcontent').hide();
                    $('#resultcontent').show();
                }
                else {
                    $('#part1').css("visibility", "hidden");
                    $('#spanclass1').css("visibility", "visible");
                    $.ajax({
                        url: $("#forRazorValue").attr("exportPDFurl"),
                        type: "POST",
                        dataType: "Json",
                        beforeSend: function () {
                            $('#part2').css("visibility", "visible");
                        },
                        success: function (data) {
                            $("#ExportPDF").button('reset');
                            $('#part2').css("visibility", "hidden");
                            $('#spanclass2').css("visibility", "visible");
                            $("#modal-footer").show();
                            $('#PDFOpen').click(function () {
                                window.location.href = $("#forRazorValue").attr("openPDFurl") + data;
                            });
                        }
                    });
                }
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
    */
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
            pdfsave = false;
            savefunction();
        });
        $("#btn_appraisal_cancel").click(function () {
            $('#CancelInfoModal').modal();
        });

        $("#btn_cancel_modal_ok").click(function () {
            window.location.href($("#forRazorValue").attr("rooturl"));
        });

        $(".ViewKpiComments").click(function () {
            $(this).popover({
                title: 'Comments',
                placement: 'right',
                html: 'true',
                content: function () {
                    var content = $(this).next().find(".Comments").html();
                    return (content != "" ? content : "There are no Comments");
                }
            });
            $(this).popover('show');
        });
        $("#ExportPDF").click(function () {
            pdfsave = true;



            savefunction();
        });

        $("#btn_submit_modal_ok").click(function () {
            $("#ShorttermCareerGoal").val(encodeURIComponent($.trim($("#ShorttermCareerGoal").val())));
            $("#DevelopmentPlan").val(encodeURIComponent($.trim($("#DevelopmentPlan").val())));
            $("#Learninganddevelopment").val(encodeURIComponent($.trim($("#Learninganddevelopment").val())));
            $("form").submit();
        });
    });
    //setInterval(autosavefunction, 300000); 
});