require(['Config'], function () {
    require(['stage1approval.careerdevelopment']);
}),
define("stage1approval.careerdevelopment", ['jquery', 'bootstrap'], function ($) {
    var pdfsave = false;
    var savefunction = function () {
        $.ajax({
            url: $("#forRazorValue").attr("saveurl"),
            type: "POST",
            dataType: "Json",
            //data: { "ApprID": $("#apprid").val(), "SectionID": $("#sectionlist li.active").attr("sectionid"), "Short-termCareerGoal": $("#Short-termCareerGoal").val(), "DevelopmentPlan": $("#DevelopmentPlan").val(), "Learninganddevelopment": $("#Learninganddevelopment").val(), "Comments": encodeURIComponent($('#Comments').val()), "KPIID": $("#Comments").attr("KPIID") },
            data: { "Comments": encodeURIComponent($.trim($('#Comments').val())), "KPIID": $("#Comments").attr("KPIID") },
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
    */
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

        $("#btn_submit_modal_ok").click(function () {
            //encode comments
            $('#Comments').val(encodeURIComponent($('#Comments').val()));
            $("form").submit();
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