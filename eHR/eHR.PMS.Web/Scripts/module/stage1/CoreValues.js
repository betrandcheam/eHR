﻿require(['Config'], function () {
    require(['stage1.corevalues']);
}),
define("stage1.corevalues", ['jquery', 'Common','bootstrap', 'bootstrap.select'], function ($,common) {
    var message = $("#forRazorValue").attr("message");
    var pdfsave = false;
    var savefunction = function () {
        var KPIArray = new Array();
        $.each($(".KPIforDatabase"), function () {
            //KPIArray.push($(this).val());
            KPIArray.push(encodeURIComponent($.trim($(this).val())));
        });
        $.ajax({
            url: $("#forRazorValue").attr("saveurl"),
            type: "POST",
            dataType: "Json",
            data: { "KPIForDatabase": JSON.stringify(KPIArray), "DeleteKPI": $("#deleteKPIid").val().substring(1) },
            beforeSend: function () {
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
        var KPIArray = new Array();
        $.each($(".Progress"), function () {
            //KPIArray.push($(this).val());
            KPIArray.push({ KpiId: $(this).attr("name"), Progress: encodeURIComponent($.trim($(this).val())) });
        });
        $.ajax({
            url: $("#forRazorValue").attr("saveprogressurl"),
            type: "POST",
            dataType: "Json",
            data: { "KPIForDatabase": JSON.stringify(KPIArray) },
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
                                window.location.href = $("#forRazorValue").attr("openPDFurl") + data;
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
            //KPIArray.push($(this).val());
            KPIArray.push(encodeURIComponent($.trim($(this).val())));
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
        //if (message)
        //    $('#RedirectModal').modal();
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
        //var KPItextHidSelectforEdit = new Object();
        //var KPItextSelectEdit = new Object();
        //added

        var PTtextforEdit = new Object();
        $(".panel-body").on('click', '.EditKPI', function () {
            $(".EditKPI").removeClass("disabled");
            $(".EditKPI").next().removeClass("disabled");
            editbuttonobject = $(this);
            var div = $(this).parent().parent().parent().parent().parent().parent();
            Adddiv = div.find(".Adddiv");
            //added
            AdddivforEdit = div.find(".Adddiv");
            UpdatedivforEdit = div.find(".Updatediv");

            Updatediv = div.find(".Updatediv");
            //KPItextHidSelectforEdit = div.parent().find(".bootstrap-select");
            //KPItextSelectEdit = div.find(".selectpicker");
            PTtextforEdit = div.find(".corevaluetext");
            //KPItextHidSelectforEdit.find('.filter-option').text($(this).parent().prev().prev().prev().text());
            //KPItextSelectEdit.val($(this).parent().prev().prev().prev().prev().text());
            //PTtextforEdit.val($(this).parent().prev().prev().text());
            PTtextforEdit.val(ReplaceEncode(br2nl(replaceAll("<BR checkedByCssHelper=\"true\">", "\n", $(this).parent().prev().prev().html()))));
            $(this).addClass("disabled");
            $(this).next().addClass("disabled");
            AdddivforEdit.hide("slow");
            UpdatedivforEdit.show("slow");
            //KPItext.trigger("focus");
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
            //aaaa

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
            //editbuttonobject.parent().prev().prev().prev().text(KPItextSelectForEdit.find("option:selected").text());
            //editbuttonobject.parent().prev().prev().prev().prev().text(KPItextSelect.val());
            //editbuttonobject.parent().prev().prev().text(PTtextforEdit.val());
            editbuttonobject.parent().prev().prev().html(nl2br($.trim(PTtextforEdit.val())));
            var oldhidValue = editbuttonobject.parent().prev().find('.KPIforDatabase').val();
            var tempArray = oldhidValue.split("^&*");
            editbuttonobject.parent().prev().find('.KPIforDatabase').val([tempArray[0], '^&*', tempArray[1], '^&*', tempArray[2], '^&*', tempArray[3], '^&*', replaceAll("\"", "'", $.trim(PTtextforEdit.val())), '^&*ONERECORDENDED'].join(''));
            editbuttonobject.removeClass("disabled");
            editbuttonobject.next().removeClass("disabled");
            Updatediv.hide("slow");
            Adddiv.show("slow");

            //resetting input box values
            //KPItextforEdit.val("");
            PTtextforEdit.val("");

            //KPItextforEdit.removeClass("warningclass");
            PTtextforEdit.removeClass("warningclass");
        });
        $(".panel-body").on('click', '.CancelKPIItem', function () {
            editbuttonobject.removeClass("disabled");
            editbuttonobject.next().removeClass("disabled");
            Updatediv.hide("slow");
            Adddiv.show("slow");

            //resetting input box values
            //KPItextforEdit.val("");
            PTtextforEdit.val("");

            //KPItextforEdit.removeClass("warningclass");
            PTtextforEdit.removeClass("warningclass");
        });
        $(".AddKPIItem").click(function () {
            //var KPItextSelect = $(this).parent().parent().parent().find(".selectpicker");
            //var KPItextHidSelect = $(this).parent().parent().parent().find(".bootstrap-select");
            var PTtext = $(this).parent().prev().find(".corevaluetext");

            if ($.trim(PTtext.val()) == "") {
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
            var Adddiv = $(this).parent();
            var Updatediv = $(this).parent().next();
            var KPINumforThisSection = $(this).parent().parent().find(".KPItbody tr").length;
            var KPItbody = $(this).parent().parent().find(".KPItbody");
            var tablediv = $(this).parent().parent().find(".tablediv");
            var blockid = tablediv.attr("blockid");
            var apprid = $("#forRazorValue").attr("apprid");
            var sectionid = $("#sectionlist li.active a").attr("sectionid");
            //var html = ['<tr><td><input type="hidden" class="KPIID" value="NewKPI" /></td><td>', PTtext.val(), '</td><td><input type="hidden" class="KPIforDatabase" name="KPIforDatabase', KPINumforThisSection, 'Block', blockid, '"value="NewKPI^&*', apprid, '^&*', sectionid, '^&*', blockid, "^&*", PTtext.val(), '^&*ONERECORDENDED"/><td align="right"><a href="javascript:void(0)" class="EditKPI btn btn-info btn-xs"><i class="glyphicon glyphicon-wrench"></i> Edit</a> <a href="javascript:void(0)" class="RemoveKPI btn btn-danger btn-xs"><i class="glyphicon glyphicon-trash"></i> Remove</a> <a href="javascript:void(0)" class="ViewKpiComments btn btn-warning btn-xs disabled"><i class="glyphicon glyphicon-pencil"></i> View Comments</a></td></tr>'].join('');
            var html = ['<tr><td><input type="hidden" class="KPIID" value="NewKPI" /></td><td>', nl2br($.trim(PTtext.val())), '</td><td><input type="hidden" class="KPIforDatabase" name="KPIforDatabase', KPINumforThisSection, 'Block', blockid, '"value="NewKPI^&*', apprid, '^&*', sectionid, '^&*', blockid, "^&*", replaceAll("\"", "'", $.trim(PTtext.val())), '^&*ONERECORDENDED"/><td align="right"><a href="javascript:void(0)" class="EditKPI btn btn-info btn-xs"><i class="glyphicon glyphicon-wrench"></i> Edit</a> <a href="javascript:void(0)" class="RemoveKPI btn btn-danger btn-xs"><i class="glyphicon glyphicon-trash"></i> Remove</a> <a href="javascript:void(0)" class="ViewKpiComments btn btn-warning btn-xs disabled"><i class="glyphicon glyphicon-pencil"></i> View Comments</a></td></tr>'].join('');
            $(html).appendTo(KPItbody);


            if (tablediv.is(":hidden")) {
                tablediv.show("slow");
            }
            //resetting input box values
            PTtext.val("");
        });
        $("#stage1kpisubmit").click(function () {
            pdfsave = false;

            $.each($(".KPIforDatabase"), function () {
                $(this).val(encodeURIComponent($.trim($(this).val())));
            })

            $("form").submit();
        });
        $("#stage1progresssubmit").click(function () {

            $.each($(".Progress"), function () {
                $(this).val(encodeURIComponent($.trim($(this).val())));
            });

            $("form").submit();
        });
        $("#stage1progresssave").click(function () {
            pdfsave = false;
            saveProgressFunction();
        });
        $("#stage1kpisave").click(savefunction);
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

    //setInterval(autosavefunction, 600000);

    if ($("#forRazorValue").attr("viewmode") != "view") {
        setInterval(autosavefunction, parseInt($("#forRazorValue").attr("autosaveinterval")));
    }

    function nl2br(str, is_xhtml) {
        //var breakTag = (is_xhtml || typeof is_xhtml === 'undefined') ? '<br />' : '<br>';
        // return (str + '').replace(/([^>\r\n]?)(\r\n|\n\r|\r|\n)/g, '$1' + breakTag + '$2');
        return str.replace(/\n/g, '<br />');

    }

    function replaceAll(find, replace, str) {
        return str.replace(new RegExp(find, 'g'), replace);
    }

    function br2nl(str) {
        return str.replace(/<br\s*\/?>/mg, "\n");
    }

    function ReplaceEncode(str) {
        var div = document.createElement('div');
        div.innerHTML = "<pre>" + str + "</pre>";
        var decoded = div.children[0].firstChild.nodeValue; 
        return decoded;
    }


});