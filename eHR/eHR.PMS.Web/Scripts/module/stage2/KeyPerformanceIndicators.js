require(['Config'], function () {
    require(['stage1.kpi']);
}),
define("stage1.kpi", ['jquery', 'bootstrap', 'bootstrap.select'], function ($) {
    var message = $("#forRazorValue").attr("message");
    var pdfsave = false;
    var savefunction = function () {
        var KPIArray = new Array();
        $.each($(".KPIforDatabase"), function () {
            KPIArray.push($(this).val());
        });
        $.ajax({
            url: $("#forRazorValue").attr("saveurl"),
            type: "POST",
            dataType: "Json",
            data: { "KPIForDatabase": JSON.stringify(KPIArray), "DeleteKPI": $("#deleteKPIid").val().substring(1) },
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
                var newkpiids = data.kpiid.split('-');
                var num = 0;
                $.each($(".KPIforDatabase"), function () {
                    if ($(this).val().indexOf("NewKPI") > -1)
                        $(this).val($(this).val().replace("NewKPI", newkpiids[num++]));
                    //$(this).val(newkpiids[num++] + $(this).val().substring(6));
                });
                num = 0;
                $.each($(".KPIID"), function () {
                    if ($(this).val().indexOf("NewKPI") > -1)
                        $(this).val($(this).val().replace("NewKPI", newkpiids[num++]));
                });
                $("#deleteKPIid").val("");
                if (!pdfsave) {
                    $("#stage1kpisave").button('reset');
                    $('#loadingcontent').hide();
                    $('#resultcontent').show();
                    $('#InfoModal').modal();
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
                               window.location.href = $("#forRazorValue").attr("openPDFurl")+data;
                            });
                        }
                    });
                }
            }
        });
    };

    var autosavefunction = function () {
        var KPIArray = new Array();
        $.each($(".KPIforDatabase"), function () {
            KPIArray.push($(this).val());
        });
        $.ajax({
            url: $("#forRazorValue").attr("saveurl"),
            type: "POST",
            dataType: "Json",
            data: { "KPIForDatabase": JSON.stringify(KPIArray), "DeleteKPI": $("#deleteKPIid").val().substring(1) },
            beforeSend: function () {
                $("#autosaveloading").show('slow');
                $("#stage1kpisave").attr("disabled", true);
            },
            success: function (data) {
                var newkpiids = data.kpiid.split('-');
                var num = 0;
                $.each($(".KPIforDatabase"), function () {
                    if ($(this).val().indexOf("NewKPI") > -1)
                        $(this).val($(this).val().replace("NewKPI", newkpiids[num++]));
                    //$(this).val(newkpiids[num++] + $(this).val().substring(6));
                });
                num = 0;
                $.each($(".KPIID"), function () {
                    if ($(this).val().indexOf("NewKPI") > -1)
                        $(this).val($(this).val().replace("NewKPI", newkpiids[num++]));
                });
                $("#deleteKPIid").val("");
                $("#autosaveloading").hide('slow');
                $("#stage1kpisave").attr("disabled", false);
            }
        });
    };

    $(function () {
        $('body').scrollspy({ target: '#sidenav' });
        $('.selectpicker').selectpicker();
        $('button[data-loading-text]').click(function () {
            $(this).button('loading');
        });
        $.each($(".KPIID"), function () {
            $(this).parent().parent().parent().parent().parent().show();
        });
        $(".Next").click(function () {
            $("#stage1kpisubmit").trigger('click');
        });
        var editbuttonobject = new Object();
        var AdddivforEdit = new Object();
        var UpdatedivforEdit = new Object();
        var KPItextforEdit = new Object();
        var PTtextforEdit = new Object();
        var PrioritytextHidSelectforEdit = new Object();
        var PrioritytextforEdit = new Object();
        $(".panel-body").on('click', '.EditKPI', function () {
            $(".EditKPI").removeClass("disabled");
            $(".EditKPI").next().removeClass("disabled");
            editbuttonobject = $(this);
            var div = $(this).parent().parent().parent().parent().parent().parent();
            KPItextforEdit = div.find(".KPItext");
            PTtextforEdit = div.find(".PTtext");
            PrioritytextHidSelectforEdit = div.parent().find(".bootstrap-select");
            PrioritytextforEdit = div.find(".selectpicker");
            AdddivforEdit = div.find(".Adddiv");
            UpdatedivforEdit = div.find(".Updatediv");
            KPItextforEdit.val($(this).parent().prev().prev().prev().prev().prev().text());
            PTtextforEdit.val($(this).parent().prev().prev().text());
            PrioritytextHidSelectforEdit.find('.filter-option').text($(this).parent().prev().prev().prev().prev().text());
            PrioritytextforEdit.val($(this).parent().prev().prev().prev().prev().text());
            $(this).addClass("disabled");
            $(this).next().addClass("disabled");
            AdddivforEdit.hide("slow");
            UpdatedivforEdit.show("slow");
            KPItextforEdit.trigger("focus");
        });
        $(".panel-body").on("click", ".RemoveKPI", function () {
            var obj = $(this).parent().parent().find(".KPIID");
            if (obj.val() != "NewKPI") {
                var oldvalue = $("#deleteKPIid").val();
                $("#deleteKPIid").val(oldvalue + "-" + obj.val());
            }
            $(this).parent().parent().remove();
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
                        return (content != "" ? content : "There are no Comments");
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
                            return (content != "" ? content : "There are no Comments");
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
        $(".panel-body").on('click', '.UpdateKPIItem', function () {

            if ($.trim(KPItextforEdit.val()) == "" || KPItextforEdit.val().indexOf("Type something here") == 0) {
                KPItextforEdit.addClass("warningclass");
                return false;
            }

            if ($.trim(PTtextforEdit.val()) == "" || PTtextforEdit.val().indexOf("Type something here") == 0) {
                PTtextforEdit.addClass("warningclass");
                return false;
            }
            $(this).parent().parent().find(".alert-specialChar").remove();
            var flag = true;
            $.each($(this).parent().parent().find("textarea"), function () {
                if (common.IsSpecialChar($(this).val())) {
                    flag = false;
                    $(this).addClass("warningclass");
                    var html = '<div class="alert alert-danger alert-specialChar alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' + common.GetspecialCharErrorMessage() + '</div>';
                    $(html).insertAfter($(this));
                    return false;
                }
            });
            if (!flag)
                return false;
            editbuttonobject.parent().prev().prev().prev().prev().prev().text(KPItextforEdit.val());
            editbuttonobject.parent().prev().prev().text(PTtextforEdit.val());
            editbuttonobject.parent().prev().prev().prev().prev().text(PrioritytextforEdit.find("option:selected").text());
            editbuttonobject.parent().prev().prev().prev().text(PrioritytextforEdit.val());
            var oldhidValue = editbuttonobject.parent().prev().find('.KPIforDatabase').val();
            var tempArray = oldhidValue.split("^&*");
            editbuttonobject.parent().prev().find('.KPIforDatabase').val([tempArray[0], '^&*', tempArray[1], '^&*', tempArray[2], '^&*', tempArray[3], '^&*', KPItextforEdit.val(), '^&*', PTtextforEdit.val(), '^&*', PrioritytextforEdit.val(), '^&*ONERECORDENDED'].join(''));
            editbuttonobject.removeClass("disabled");
            editbuttonobject.next().removeClass("disabled");
            UpdatedivforEdit.hide("slow");
            AdddivforEdit.show("slow");

            //resetting input box values
            KPItextforEdit.val("");
            PTtextforEdit.val("");

            KPItextforEdit.removeClass("warningclass");
            PTtextforEdit.removeClass("warningclass");
        });

        $(".panel-body").on('click', '.CancelKPIItem', function () {
            editbuttonobject.removeClass("disabled");
            editbuttonobject.next().removeClass("disabled");
            UpdatedivforEdit.hide("slow");
            AdddivforEdit.show("slow");

            //resetting input box values
            KPItextforEdit.val("");
            PTtextforEdit.val("");

            KPItextforEdit.removeClass("warningclass");
            PTtextforEdit.removeClass("warningclass");
        });
        $(".AddKPIItem").click(function () {
            //$(this).parent().parent().parent().parent().find(".panel-heading").removeClass(".warningclass");
            var KPItext = $(this).parent().parent().find(".KPItext");
            var PTtext = $(this).parent().parent().find(".PTtext");

            if ($.trim(KPItext.val()) == "" || KPItext.val().indexOf("Type something here") == 0) {
                KPItext.addClass("warningclass");
                return false;
            }

            if ($.trim(PTtext.val()) == "" || PTtext.val().indexOf("Type something here") == 0) {
                PTtext.addClass("warningclass");
                return false;
            }
            $(this).parent().parent().find(".alert-specialChar").remove();
            var flag = true;
            $.each($(this).parent().parent().find("textarea"), function () {
                if (common.IsSpecialChar($(this).val())) {
                    flag = false;
                    $(this).addClass("warningclass");
                    var html = '<div class="alert alert-danger alert-specialChar alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' + common.GetspecialCharErrorMessage() + '</div>';
                    $(html).insertAfter($(this));
                    return false;
                }
            });
            if (!flag)
                return false;
            var Prioritytext = $(this).parent().parent().find(".selectpicker");
            var PrioritytextHidSelect = $(this).parent().parent().parent().find(".bootstrap-select");
            var Adddiv = $(this).parent();
            var Updatediv = $(this).parent().next();
            var KPItbody = $(this).parent().parent().find(".KPItbody");
            var KPINumforThisSection = $(this).parent().parent().find(".KPItbody tr").length;
            var tablediv = $(this).parent().parent().find(".tablediv");
            var blockid = tablediv.attr("blockid");
            var apprid = $("#forRazorValue").attr("apprid");
            var sectionid = $("#sectionlist li.active a").attr("sectionid");
            var html = ['<tr><td><input type="hidden" class="KPIID" value="NewKPI" /></td><td>', KPItext.val(), '</td><td>', Prioritytext.find("option:selected").text(), '</td><td style="display:none;">', Prioritytext.val(), '</td><td>', PTtext.val(), '</td><td style="display:none;"><input type="text" class="KPIforDatabase" name="KPIforDatabase', KPINumforThisSection, 'Block', blockid, '" value="NewKPI^&*', apprid, '^&*', sectionid, '^&*', blockid, '^&*', KPItext.val(), '^&*', PTtext.val(), '^&*', Prioritytext.val(), '^&*ONERECORDENDED"/></td><td align="right"><a href="javascript:void(0)" class="EditKPI btn btn-info btn-xs"><i class="glyphicon glyphicon-wrench"></i> Edit</a> <a href="javascript:void(0)" class="RemoveKPI btn btn-danger btn-xs"><i class="glyphicon glyphicon-trash"></i> Remove</a><a href="javascript:void(0)" class="ViewKpiComments btn btn-warning btn-xs disabled"><i class="glyphicon glyphicon-pencil"></i> View Comments</a> </td></tr>'].join('');

            $(html).appendTo(KPItbody);
            if (tablediv.is(":hidden")) {
                tablediv.show("slow");
            }

            //resetting input box values
            KPItext.val("");
            PTtext.val("");
        });

        $("#stage1kpisubmit").click(function () {
            $(".alert").remove();
            $(".panel-heading").css("background-color", "rgb(233,233,233)")
            var flag = true;
            var errorplace = new Object();
            $.each($(".KPItbody"), function () {
                if ($(this).find(".KPIID").length == 0) {
                    flag = false;
                    errorplace = $(this).parent().parent().parent().parent().parent().parent();
                    return false;
                }
            });
            if (flag) {
                //$('#SubmitInfoModal').modal();
                $("form").submit();
            }
            else {
                errorplace.find(".panel-heading").css("background-color", "red");
                var errorid = errorplace.attr("id");
                var selector = "[id='" + errorid + "']";
                var html = '<div class="alert alert-danger alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>There must be at least 1 KPI item for each theme.</div>';
                $(html).insertBefore($(selector).find(".tablediv"));
                $("html,body").animate({ scrollTop: $(selector).offset().top + "px" });

            }
        });

        $("#stage1kpisave").click(function () {
            $(".alert").remove();
            pdfsave = false;
            savefunction();
        });

        $("#btn_appraisal_cancel").click(function () {
            $('#CancelInfoModal').modal();
        });

        $("#btn_cancel_modal_ok").click(function () {
            window.location.href($("#forRazorValue").attr("rooturl"));
        });
        $("#ExportPDF").click(function () {
            pdfsave = true;
            savefunction();
        });
    });

    setInterval(autosavefunction, 600000);
});