require(['Config'], function () {
    require(['stage1approval.corevalues']);
}),
define("stage1approval.corevalues", ['jquery', 'bootstrap', 'bootstrap.select'], function ($) {
    var message = $("#forRazorValue").attr("message");
        var savefunction = function () {
            var KPIArray = new Array();
            $.each($(".KPIID"), function () {
                KPIArray.push(({ KpiId: $(this).val(), Comments: $(this).parent().parent().find(".CommentContent").val() }));
            });
            $.ajax({
                url: $("#forRazorValue").attr("saveurl"),
                type: "POST",
                dataType: "Json",
                data: { "KPIForDatabase": JSON.stringify(KPIArray) },
                beforeSend: function () {
                    //$("#stage1kpisave").button('loading');
                    //$("#buttongroup").showLoading();
                    $('#resultcontent').hide();
                    $('#loadingcontent').show();
                    $('#InfoModal').modal();
                },
                success: function (data) {
                    var newkpiids = data.kpiid.split('-');
                    var num = 0;
                    $.each($(".CommentID"), function () {
                        if ($(this).val().indexOf("NewComment") > -1)
                            $(this).val($(this).val().replace("NewComment", newkpiids[num++]));
                        if ($(this).prev().val() == "")
                            $(this).val("NewComment");
                        //$(this).val(newkpiids[num++] + $(this).val().substring(6));
                    });
                    $("#stage1kpisave").button('reset');
                    $('#loadingcontent').hide();
                    $('#resultcontent').show();
                    //$('#InfoModal').modal();
                }
            });
        };
        /*
        var autosavefunction = function () {
            var KPIArray = new Array();
            $.each($(".KPIID"), function () {
                KPIArray.push(({ KpiId: $(this).val(), Comments: $(this).parent().parent().find(".CommentContent").val() }));
            });
            $.ajax({
                url: $("#forRazorValue").attr("saveurl"),
                type: "POST",
                dataType: "Json",
                data: { "KPIForDatabase": JSON.stringify(KPIArray) },
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
            $('.selectpicker').selectpicker();
            $('button[data-loading-text]').click(function () {
                $(this).button('loading');
            });
            $.each($(".KPIID"), function () {
                $(this).parent().parent().parent().parent().parent().show();
            });
            $("#btn_next_section").click(function () {
                $("form").submit();
            });
            $("#stage1kpiapproval").click(function () {
                $("#ApORRe").val("1");
                $("modelmessage").text("Are you sure approve this now?")
                $('#SubmitInfoModal').modal();
            });
            $("#stage1kpireject").click(function () {
                $("#ApORRe").val("0");
                $("modelmessage").text("Are you sure reject this now?")
                $('#SubmitInfoModal').modal();
            });
            $("#stage1kpisave").click(savefunction);

            $("#btn_appraisal_cancel").click(function () {
                $('#CancelInfoModal').modal();
            });

            $("#btn_cancel_modal_ok").click(function () {
                window.location.href($("#forRazorValue").attr("rooturl"));
            });

            var oddClick = true;
            var oldComments = 0;
            var newComments = 0;
            $(".panel-body").on('click', '.ViewKpiComments', function () {
                if ($(".popover").length == 0) oddClick = true;
                if (oddClick) {
                    oddClick = !oddClick;
                    oldComments = newComments = $(this).attr("commentsid");
                    $(this).popover({
                        title: 'Comments',
                        placement: 'top',
                        html: 'true',
                        content: function () {
                            var content = $(this).parent().parent().find(".Comments").html();
                            return (content != "" ? content : "There is no Comments");
                        }
                    });
                    $(this).popover('show');
                }
                else {
                    newComments = $(this).attr("commentsid");
                    if (oldComments != newComments) {
                        oldComments = $(this).attr("commentsid");
                        $(".ViewKpiComments").popover('destroy');
                        $(this).popover({
                            title: 'Comments',
                            placement: 'top',
                            html: 'true',
                            content: function () {
                                var content = $(this).parent().parent().find(".Comments").html();
                                return (content != "" ? content : "There is no Comments");
                            }
                        });
                        $(this).popover('show');
                    }
                    else {
                        oddClick = !oddClick;
                        $(this).popover('destroy');
                    }
                }

            });
        });
        //setInterval(autosavefunction, 300000); 
});