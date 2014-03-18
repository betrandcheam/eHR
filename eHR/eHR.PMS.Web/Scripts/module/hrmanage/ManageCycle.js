require(['Config'], function () {
    require(['hrmanage.managecycle']);
}),
define("hrmanage.managecycle", ['jquery', 'bootstrap', 'bootstrap.select', 'moment', 'bootstrap.datetimepicker', 'bootstrap.paginator', 'validators'], function ($) {
    $(function () {
        //alert("loaded");
        $('.selectpicker').selectpicker();
        $('.selectpicker').change(function () {
            $("#updatecycle").val("0");
            $("form").submit();
        });
        $('#datetimepicker1').datetimepicker({
            format: "DD/MM/YYYY",
            pickTime: false
        });
        $('#datetimepicker2').datetimepicker({
            format: "DD/MM/YYYY",
            pickTime: false
        });
        $('#datetimepicker3').datetimepicker({
            format: "DD/MM/YYYY",
            pickTime: false
        });
        $('#datetimepicker4').datetimepicker({
            format: "DD/MM/YYYY",
            pickTime: false
        });
        $('#datetimepicker5').datetimepicker({
            format: "DD/MM/YYYY",
            pickTime: false
        });
        $('#datetimepicker6').datetimepicker({
            format: "DD/MM/YYYY",
            pickTime: false
        });
        var nowdate = $("#forRazorValue").attr("nowdate");
        var tempnow = nowdate.split('-');
        var temp = new Array();
        $.each($(".stagedate"), function () {
            temp = $(this).val().split('/');
            if (parseInt(tempnow[0], 10) > parseInt(temp[2], 10) || parseInt(tempnow[1], 10) > parseInt(temp[1], 10) || parseInt(tempnow[2], 10) >= parseInt(temp[0], 10)) {
                $(this).attr("readonly", true);
                $(this).parent().datetimepicker('hide');
                $(this).next().hide();
            }
        });
        if ($("#whetherpostback").val() != "-1") {
            $('.bootstrap-select').find('.filter-option').text($("#whetherpostback").attr("attrname"));
            $('.selectpicker').val($("#whetherpostback").val());
        }
        var total = $("#forRazorValue").attr("numberappr");
        var intTotal = parseInt(total);
        if (intTotal != 0) {
            $("#AfterRetrieve").show('slow');
            var pagenum = (intTotal % 10 != 0 ? (intTotal / 10 + 1) : (intTotal / 10));
            var options = {
                currentPage: 1,
                totalPages: pagenum,
                size: 'small',
                useBootstrapTooltip: true,
                onPageClicked: function (e, originalEvent, type, page) {
                    $.ajax({
                        url: $("#forRazorValue").attr("pageurl"),
                        type: "post",
                        dataType: "json",
                        data: { "page": page, "Stage1EndDate": $("#Stage1EndDate").val(), "Stage3EndDate": $("#Stage3EndDate").val(), "cycleId": $('#cycleid').val() },
                        beforeSend: function () {
                            $('#resultcontent').hide();
                            $('#loadingcontent').show();
                            $('#InfoModal').modal(); ;
                        },
                        success: function (data) {
                            $("#eicontent").html(data);
                            $('#InfoModal').modal('hide');
                        }
                    });
                }
            }

            $('#pages').bootstrapPaginator(options);
        }

        $('#showpage').change(function () {
            var page = $(this).val();
            $('#pages').bootstrapPaginator("show", page);
        })

        /*$("#AddParticipants").click(function () {
        if (intTotal == 0) {
        //$("#RedirectModal").model();
        //return false;
        }
        else {
        window.location.href = "HRManage/AddParticipants/" + $("#strStage1EndDate").val() + "/" + $("#strStage3EndDate").val();
        }
        });*/
        /*$("#RemoveParticipants").click(function () {
        if (intTotal == 0) {
        $("#RemoveParticipantModal").model();
        return false;
        }
        else {
        window.location.href = "HRManage/RemoveParticipants/" + $("#strStage1EndDate").val() + "/" + $("#strStage3EndDate").val();
        } 
        });*/


        $("#btn_start_cycle").click(function () {
            var int_error_count = 0;
            var str_error_message = "";
            $(".form-control").removeClass("warningclass");

            /*if ($.trim($("#cyclename").val()).length == 0) {
            $("#cyclename").addClass("warningclass");
            str_error_message = "Please provide the name of the cycle.";
            $("#errormsg").text(str_error_message);
            $(".alert").show();
            return false;
            }*/

            $.each($(".stagedate"), function () {
                if ($.trim($(this).val()).length == 0) {
                    int_error_count++;
                    $(this).addClass("warningclass");
                    str_error_message = "Please provide the " + $(this).attr("forerrormsg") + " for cycle.";
                    $("#errormsg").text(str_error_message);
                    $(".alert").show();
                    return false;
                }
                else {
                    if (!validators_boo_checkDate($.trim($(this).val()), "/")) {
                        int_error_count++;
                        $(this).addClass("warningclass");
                        str_error_message = $(this).attr("forerrormsg") + " is not valid. It should be in DD/MM/YYYY format.";
                        $("#errormsg").text(str_error_message);
                        $(".alert").show();
                        return false;
                    }
                }
            });

            if (int_error_count == 0) {
                if (getDate($("#Stage1StartDate").val()).getTime() < getDate($("#forRazorValue").attr("nowdate")).getTime()) {
                    $("#Stage1StartDate").addClass("warningclass");
                    str_error_message = "Goal Setting Start Date must not be earlier than today.";
                    $("#errormsg").text(str_error_message);
                    $(".alert").show();
                    return false;
                }
                else if (getDate($("#Stage1StartDate").val()).getTime() >= getDate($("#Stage1EndDate").val()).getTime()) {
                    $("#Stage1StartDate").addClass("warningclass");
                    str_error_message = "Goal Setting Start Date must be earlier than the Goal Setting End Date.";
                    $("#errormsg").text(str_error_message);
                    $(".alert").show();
                    return false;
                }
                else if (getDate($("#Stage1EndDate").val()).getTime() >= getDate($("#Stage2StartDate").val()).getTime()) {
                    $("#Stage1EndDate").addClass("warningclass");
                    str_error_message = "Goal Setting End Date must be earlier than the Progress Review Start Date.";
                    $("#errormsg").text(str_error_message);
                    $(".alert").show();
                    return false;
                }
                else if (getDate($("#Stage2StartDate").val()).getTime() >= getDate($("#Stage2EndDate").val()).getTime()) {
                    $("#Stage2StartDate").addClass("warningclass");
                    str_error_message = "Progress Review End Date must be later than the Progress Review Start Date.";
                    $("#errormsg").text(str_error_message);
                    $(".alert").show();
                    return false;
                }
                else if (getDate($("#Stage2EndDate").val()).getTime() >= getDate($("#Stage3StartDate").val()).getTime()) {
                    $("#Stage2EndDate").addClass("warningclass");
                    str_error_message = "Progress Review End Date must be earlier than the Final Year Review Start Date.";
                    $("#errormsg").text(str_error_message);
                    $(".alert").show();
                    return false;
                }
                else if (getDate($("#Stage3StartDate").val()).getTime() >= getDate($("#Stage3EndDate").val()).getTime()) {
                    $("#Stage3StartDate").addClass("warningclass");
                    str_error_message = "Final Year Review End Date must be later than the Final Year Review Start Date.";
                    $("#errormsg").text(str_error_message);
                    $(".alert").show();
                    return false;
                }
            }
            if (int_error_count == 0) {
                $("#updatecycle").val("1");
                $("modelmessage").text("Are you sure approve this now?")
                $('#SubmitInfoModal').modal();
            }
        });


        $("#btn_cancel").click(function () {
            $('#CancelInfoModal').modal();
        });

        $("#btn_cancel_modal_ok").click(function () {
            window.location.href($("#forRazorValue").attr("booturl"));
        });

        var message = $("#forRazorValue").attr("message");
        $(function () {
            if (message)
                $('#RedirectModal').modal();
        });


        var error_message = $("#forRazorValue").attr("errormsg");
        $(function () {
            if (error_message) {
                $('#ErrorModal').modal();
            }
        });

        function getDate(datestring) {
            var dateparts = datestring.split("/");
            return new Date(dateparts[2], dateparts[1], dateparts[0]);
        }
    });
});