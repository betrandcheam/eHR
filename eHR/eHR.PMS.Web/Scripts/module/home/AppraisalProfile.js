require(['Config'], function () {
    require(['home.profile']);
}),
define("home.profile", ['jquery', 'bootstrap'], function ($) {
    $(function () {
        var obj = new Object();
        $('.Reviewer').on('keyup', function () {
            obj = $(this);
            obj.next().html("");
            if (obj.val() != "") {
                $.ajax({
                    url: $("#forRazorValue").attr("searchurl"),
                    dataType: 'json',
                    type: 'POST',
                    data: { name: obj.val() },
                    success: function (data) {
                        if (obj.next().css('display') == 'none')
                            obj.next().css('display', 'block');
                        var html = '<div class="tt-dataset-0"><span class="tt-suggestions" style="display: block;">';
                        $.each(data, function (index) {
                            html = [html, '<div usrid="', data[index].Id, '" class="tt-suggestion" style="white-space: nowrap; cursor: pointer;">', '<p style="white-space: normal;">', data[index].PreferredName + " (" + data[index].DomainId + ")", '</p>', '</div>'].join('');
                        });
                        html = [html, '<span></div>'].join('');
                        $(html).appendTo(obj.next());
                        $(".tt-suggestion").mouseover(function () {
                            $(this).addClass("tt-is-under-cursor");
                        }).mouseleave(function () {
                            $(this).removeClass("tt-is-under-cursor");
                        })
                        $(".tt-dropdown-menu").on('mousedown', '.tt-suggestion', function () {
                            obj.attr('usrid', $(this).attr("usrid"));
                            obj.val($(this).children().text());
                            obj.next().hide();
                        });
                        obj.focus(function () {
                            obj.next().show();
                        }).blur(function () {
                            obj.next().hide();
                        });
                    }
                });
            }
            else {
                obj.next().hide();
            }
        });

        $("#reviewersave").click(function () {
            var reviewers = "";
            $.each($(".Reviewer"), function (index) {
                if ($(this).val() != "")
                    reviewers = [reviewers, '|', $(this).attr("usrid")].join('');
            });
            if (reviewers != "")
                reviewers = reviewers.substring(1, reviewers.length);
            $.ajax({
                url: $("#forRazorValue").attr("saveurl"),
                dataType: 'json',
                type: 'POST',
                data: { "reviewers": reviewers, "apprid": parseInt($("#apprid").val()) },
                beforeSend: function () {
                    $('#resultcontent').hide();
                    $('#loadingcontent').show();
                    $('#InfoModal').modal();
                },
                success: function (data) {
                    if (data == "") {
                        $('#loadingcontent').hide();
                        $('#resultcontent').show();
                    }
                }
            });
        });

        // Approver1 search
        $('.Approver1').on('keyup', function () {
            obj = $(this);
            obj.next().html("");
            if (obj.val() != "") {
                $.ajax({
                    url: $("#forRazorValue").attr("searchurl"),
                    dataType: 'json',
                    type: 'POST',
                    data: { name: obj.val() },
                    success: function (data) {
                        if (obj.next().css('display') == 'none')
                            obj.next().css('display', 'block');
                        var html = '<div class="tt-dataset-0"><span class="tt-suggestions" style="display: block;">';
                        $.each(data, function (index) {
                            html = [html, '<div usrid="', data[index].Id, '" class="tt-suggestion" style="white-space: nowrap; cursor: pointer;">', '<p style="white-space: normal;">', data[index].PreferredName + " (" + data[index].DomainId + ")", '</p>', '</div>'].join('');
                        });
                        html = [html, '<span></div>'].join('');
                        $(html).appendTo(obj.next());
                        $(".tt-suggestion").mouseover(function () {
                            $(this).addClass("tt-is-under-cursor");
                        }).mouseleave(function () {
                            $(this).removeClass("tt-is-under-cursor");
                        })
                        $(".tt-dropdown-menu").on('mousedown', '.tt-suggestion', function () {
                            obj.attr('usrid', $(this).attr("usrid"));
                            obj.val($(this).children().text());
                            obj.next().hide();
                        });
                        obj.focus(function () {
                            obj.next().show();
                        }).blur(function () {
                            obj.next().hide();
                        });
                    }
                });
            }
            else {
                obj.next().hide();
            }
        });
        // End Approver1 search

        // Approver2 search
        $('.Approver2').on('keyup', function () {
            obj = $(this);
            obj.next().html("");
            if (obj.val() != "") {
                $.ajax({
                    url: $("#forRazorValue").attr("searchurl"),
                    dataType: 'json',
                    type: 'POST',
                    data: { name: obj.val() },
                    success: function (data) {
                        if (obj.next().css('display') == 'none')
                            obj.next().css('display', 'block');
                        var html = '<div class="tt-dataset-0"><span class="tt-suggestions" style="display: block;">';
                        $.each(data, function (index) {
                            html = [html, '<div usrid="', data[index].Id, '" class="tt-suggestion" style="white-space: nowrap; cursor: pointer;">', '<p style="white-space: normal;">', data[index].PreferredName + " (" + data[index].DomainId + ")", '</p>', '</div>'].join('');
                        });
                        html = [html, '<span></div>'].join('');
                        $(html).appendTo(obj.next());
                        $(".tt-suggestion").mouseover(function () {
                            $(this).addClass("tt-is-under-cursor");
                        }).mouseleave(function () {
                            $(this).removeClass("tt-is-under-cursor");
                        })
                        $(".tt-dropdown-menu").on('mousedown', '.tt-suggestion', function () {
                            obj.attr('usrid', $(this).attr("usrid"));
                            obj.val($(this).children().text());
                            obj.next().hide();
                        });
                        obj.focus(function () {
                            obj.next().show();
                        }).blur(function () {
                            obj.next().hide();
                        });
                    }
                });
            }
            else {
                obj.next().hide();
            }
        });
        // End Approver2 search

        // Approver Save
        $("#approversave").click(function () {
            var approvers = "";
            if ($.trim($(".Approver1").val()) == "" || $.trim($(".Approver2").val()) == "") {
                $("#errormodalmessage").text("Both level 1 and level 2 approvers must be provided.");
                $('#ErrorInfoModal').modal();
            }
            else if ($(".Approver1").attr("usrid") == $(".Approver2").attr("usrid")) {
                $("#errormodalmessage").text("Level 1 and Level 2 approvers cannot be the same person.");
                $('#ErrorInfoModal').modal();
            }
            else {
                approvers = $(".Approver1").attr("usrid") + "|" + $(".Approver2").attr("usrid");
                if (approvers != "") {
                    approvers = approvers.substring(1, approvers.length);
                }

                $.ajax({
                    url: $("#forRazorValue").attr("saveapprurl"),
                    dataType: 'json',
                    type: 'POST',
                    data: { "approver1": $(".Approver1").attr("usrid"), "approver2": $(".Approver2").attr("usrid"), "appraisalId": parseInt($("#apprid").val()) },
                    beforeSend: function () {
                        $('#resultcontent').hide();
                        $('#loadingcontent').show();
                        $('#modalmessage').text("Approvers saved successfully.");
                        $('#InfoModal').modal();
                    },
                    success: function (data) {
                        if (data == "") {
                            $('#loadingcontent').hide();
                            $('#resultcontent').show();
                        }
                    }
                });
            }

            /*
            $.each($(".Approver"), function (index) {
            if ($(this).val() != "") {
            alert($(this).val());
            alert($(this).attr("usrid"));
            if (approvers.indexOf($(this).attr("usrid")) != 0) {
            alert("here");
            }
            else {
            alert("there");
            }


            approvers = [approvers, '|', $(this).attr("usrid")].join('');
            }
                            
            });
            if(approvers!="")
            approvers = approvers.substring(1, approvers.length);
            */
            /*
            $.ajax({
            url: "@(Url.Content("~/Home/SaveProfile"))",
            dataType: 'json',
            type: 'POST',
            data: { "reviewers": reviewers, "apprid": parseInt($("#apprid").val()) },
            beforeSend: function () {
            $('#resultcontent').hide();
            $('#loadingcontent').show();
            $('#InfoModal').modal();
            },
            success: function (data) {
            if (data == "") {
            $('#loadingcontent').hide();
            $('#resultcontent').show();
            }
            }
            });
            */
        });
        // End Approver Save

        // SMT Member search
        $('.SMT').on('keyup', function () {
            obj = $(this);
            obj.next().html("");
            if (obj.val() != "") {
                $.ajax({
                    url: $("#forRazorValue").attr("searchurl"),
                    dataType: 'json',
                    type: 'POST',
                    data: { name: obj.val() },
                    success: function (data) {
                        if (obj.next().css('display') == 'none')
                            obj.next().css('display', 'block');
                        var html = '<div class="tt-dataset-0"><span class="tt-suggestions" style="display: block;">';
                        $.each(data, function (index) {
                            html = [html, '<div usrid="', data[index].Id, '" class="tt-suggestion" style="white-space: nowrap; cursor: pointer;">', '<p style="white-space: normal;">', data[index].PreferredName + " (" + data[index].DomainId + ")", '</p>', '</div>'].join('');
                        });
                        html = [html, '<span></div>'].join('');
                        $(html).appendTo(obj.next());
                        $(".tt-suggestion").mouseover(function () {
                            $(this).addClass("tt-is-under-cursor");
                        }).mouseleave(function () {
                            $(this).removeClass("tt-is-under-cursor");
                        })
                        $(".tt-dropdown-menu").on('mousedown', '.tt-suggestion', function () {
                            obj.attr('usrid', $(this).attr("usrid"));
                            obj.val($(this).children().text());
                            obj.next().hide();
                        });
                        obj.focus(function () {
                            obj.next().show();
                        }).blur(function () {
                            obj.next().hide();
                        });
                    }
                });
            }
            else {
                obj.next().hide();
            }
        });
        // End SMT Member search

        // SMT Member Save
        $("#smtsave").click(function () {
            var smt_member = "";
            if ($.trim($(".SMT").val()) == "") {
                $("#errormodalmessage").text("Senior Management Team Member must be provided.");
                $('#ErrorInfoModal').modal();
            }
            else {
                smt_member = $(".SMT").attr("usrid");

                $.ajax({
                    url: $("#forRazorValue").attr("savesmturl"),
                    dataType: 'json',
                    type: 'POST',
                    data: { "smt": $(".SMT").attr("usrid"), "appraisalId": parseInt($("#apprid").val()) },
                    beforeSend: function () {
                        $('#resultcontent').hide();
                        $('#loadingcontent').show();
                        $('#modalmessage').text("Senior Management Team Member saved successfully.");
                        $('#InfoModal').modal();
                    },
                    success: function (data) {
                        if (data == "") {
                            $('#loadingcontent').hide();
                            $('#resultcontent').show();
                        }
                    }
                });
            }
        });
        // End SMT Member Save

        $("#btn_appraisal_cancel").click(function () {
            $('#CancelInfoModal').modal();
        });

        $("#btn_cancel_modal_ok").click(function () {
            window.location.href($("#forRazorValue").attr("previousurl"));
        });
    });
});