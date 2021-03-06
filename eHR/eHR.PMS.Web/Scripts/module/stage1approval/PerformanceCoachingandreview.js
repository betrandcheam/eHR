﻿require(['Config'], function () {
    require(['stage1approval.performance']);
}),
define("stage1approval.performance", ['jquery', 'bootstrap'], function ($) {
    var message = $("#forRazorValue").attr("message");
    var pdfsave = false;
    var savefunction = function () {
        $.ajax({
            url: $("#forRazorValue").attr("saveurl"),
            type: "POST",
            dataType: "Json",
            data: { "KPIID": $("#forRazorValue").attr("performancecoachingitemid"), "Comments": encodeURIComponent($("#Comments").val()) },
            //data: { "ApprID": $("#forRazorValue").attr("apprid"), "SectionID": $("#sectionlist li.active a").attr("sectionid"), "StrengthsArea": $("#StrengthsArea").val(), "ImprovementsArea": $("#ImprovementsArea").val() },
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
    data: { "ApprID": $("#forRazorValue").attr("apprid"), "SectionID": $("#sectionlist li.active a").attr("sectionid"), "StrengthsArea": $("#StrengthsArea").val(), "ImprovementsArea": $("#ImprovementsArea").val() },
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
            if ($(".alert-specialChar").length > 0)
                return false;
            pdfsave = false;
            $("form").submit();
        });

        $("#btn_next_section").click(function () {
            $("#Comments").val(encodeURIComponent($.trim($("#Comments").val())));
            $("form").submit();
        });

        $("#stage1kpisave").click(function () {
            if ($(".alert-specialChar").length > 0) {
                $(this).button('reset');
                return false;
            }
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
    });
    //setInterval(autosavefunction, 300000);
});