require(['Config'], function () {
    require(['stage1.performance']);
}),
define("stage1.performance", ['jquery', 'bootstrap'], function ($) {
    var message = $("#forRazorValue").attr("message");
    var pdfsave = false;
    var savefunction = function () {
        $.ajax({
            url: $("#forRazorValue").attr("saveurl"),
            type: "POST",
            dataType: "Json",
            data: { "ApprID": $("#forRazorValue").attr("apprid"), "SectionID": $("#sectionlist li.active a").attr("sectionid"), "StrengthsArea": encodeURIComponent($.trim($("#StrengthsArea").val())), "ImprovementsArea": encodeURIComponent($.trim($("#ImprovementsArea").val())) },
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

    var saveProgressFunction = function () {
        $.ajax({
            url: $("#forRazorValue").attr("saveprogressurl"),
            type: "POST",
            dataType: "Json",
            data: { "ApprID": $("#forRazorValue").attr("apprid"), "Progress": encodeURIComponent($.trim($("#Progress").val())) },
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
                    $("#stage1progresssave").button('reset');
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
            $("#StrengthsArea").val(encodeURIComponent($.trim($("#StrengthsArea").val())));
            $("#ImprovementsArea").val(encodeURIComponent($.trim($("#ImprovementsArea").val())));
            $("form").submit();
        });
        $("#stage1progresssubmit").click(function () {
            $("#Progress").val(encodeURIComponent($.trim($("#Progress").val())));
            $("form").submit();
        });
        $("#stage1kpisave").click(function () {
            if ($(".alert-specialChar").length > 0) {
                $(this).button('reset');
                return false;
            }
            savefunction();
        });
        $("#stage1progresssave").click(function () {
            pdfsave = false;
            saveProgressFunction();
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