﻿require(['Config'], function () {
    require(['stage2approval.performance']);
}),
define("stage2approval.performance", ['jquery', 'bootstrap'], function ($) {
    var pdfsave = false;
    var savefunction = function () {
        $.ajax({
            url: $("#forRazorValue").attr("saveurl"),
            type: "POST",
            dataType: "Json",
            data: { KPIID: $("#Comments").attr("KPIID"), Comments: $("#Comments").val() },
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
        $("#stage1kpisave").click(function () {
            if ($(".alert-specialChar").length > 0) {
                $(this).button('reset');
                return false;
            }
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
    });
    //setInterval(autosavefunction, 300000); 
});