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
            $("#updatecycle").val("1");
            $("modelmessage").text("Are you sure approve this now?")
            $('#SubmitInfoModal').modal();
        });

        $("#btn_retrieve_participants").click(function () {
            $("#updatecycle").val("0");
            var int_error_count = 0;
            var str_error_message = "";

            $("#cyclename").removeClass("warningclass");
            $("#Stage1StartDate").removeClass("warningclass");
            $("#Stage1EndDate").removeClass("warningclass");
            $("#Stage2StartDate").removeClass("warningclass");
            $("#Stage2EndDate").removeClass("warningclass");
            $("#Stage3StartDate").removeClass("warningclass");
            $("#Stage3EndDate").removeClass("warningclass");

            if ($.trim($("#cyclename").val()).length == 0) {
                int_error_count++;
                $("#cyclename").addClass("warningclass");

                str_error_message = "Please provide the name of the cycle.";
            }

            if ($.trim($("#Stage1StartDate").val()).length == 0) {
                int_error_count++;
                $("#Stage1StartDate").addClass("warningclass");

                if (str_error_message == "") {
                    str_error_message = "Please provide the start date of Goal Setting Stage for cycle.";
                }
                else {
                    str_error_message = str_error_message + "<br/> Please provide the start date of Goal Setting Stage for cycle.";
                }
            }
            else {
                if (!validators_boo_checkDate($.trim($("#Stage1StartDate").val()), "/")) {
                    int_error_count++;
                    $("#Stage1StartDate").addClass("warningclass");

                    if (str_error_message == "") {
                        str_error_message = "Goal Setting Stage Start Date is not valid. It should be in DD/MM/YYYY format.";
                    }
                    else {
                        str_error_message = str_error_message + "<br/> Goal Setting Stage Start Date is not valid. It should be in DD/MM/YYYY format.";
                    }
                }
            }

            if ($.trim($("#Stage1EndDate").val()).length == 0) {
                int_error_count++;
                $("#Stage1EndDate").addClass("warningclass");

                if (str_error_message == "") {
                    str_error_message = "Please provide the end date of Goal Setting Stage for cycle.";
                }
                else {
                    str_error_message = str_error_message + "<br/> Please provide the end date of Goal Setting Stage for cycle.";
                }
            }
            else {
                if (!validators_boo_checkDate($.trim($("#Stage1EndDate").val()), "/")) {
                    int_error_count++;
                    $("#Stage1EndDate").addClass("warningclass");

                    if (str_error_message == "") {
                        str_error_message = "Goal Setting Stage End Date is not valid. It should be in DD/MM/YYYY format.";
                    }
                    else {
                        str_error_message = str_error_message + "<br/> Goal Setting Stage End Date is not valid. It should be in DD/MM/YYYY format.";
                    }
                }
            }

            if ($.trim($("#Stage2StartDate").val()).length == 0) {
                int_error_count++;
                $("#Stage2StartDate").addClass("warningclass");

                if (str_error_message == "") {
                    str_error_message = "Please provide the start date of Progress Review Stage for cycle.";
                }
                else {
                    str_error_message = str_error_message + "<br/> Please provide the start date of Progress Review Stage for cycle.";
                }
            }
            else {
                if (!validators_boo_checkDate($.trim($("#Stage2StartDate").val()), "/")) {
                    int_error_count++;
                    $("#Stage2StartDate").addClass("warningclass");

                    if (str_error_message == "") {
                        str_error_message = "Progress Review Stage Start Date is not valid. It should be in DD/MM/YYYY format.";
                    }
                    else {
                        str_error_message = str_error_message + "<br/> Progress Review Stage Start Date is not valid. It should be in DD/MM/YYYY format.";
                    }
                }
            }

            if ($.trim($("#Stage2EndDate").val()).length == 0) {
                int_error_count++;
                $("#Stage2EndDate").addClass("warningclass");

                if (str_error_message == "") {
                    str_error_message = "Please provide the end date of Progress Review Stage for cycle.";
                }
                else {
                    str_error_message = str_error_message + "<br/> Please provide the end date of Progress Review Stage for cycle.";
                }
            }
            else {
                if (!validators_boo_checkDate($.trim($("#Stage2EndDate").val()), "/")) {
                    int_error_count++;
                    $("#Stage2EndDate").addClass("warningclass");
                }

                if (str_error_message == "") {
                    str_error_message = "Progress Review Stage End Date is not valid. It should be in DD/MM/YYYY format.";
                }
                else {
                    str_error_message = str_error_message + "<br/> Progress Review End Date is not valid. It should be in DD/MM/YYYY format.";
                }
            }

            if ($.trim($("#Stage3StartDate").val()).length == 0) {
                int_error_count++;
                $("#Stage3StartDate").addClass("warningclass");

                if (str_error_message == "") {
                    str_error_message = "Please provide the start date of Final Year Review Stage for cycle.";
                }
                else {
                    str_error_message = str_error_message + "<br/> Please provide the start date of Final Year Review Stage for cycle.";
                }
            }
            else {
                if (!validators_boo_checkDate($.trim($("#Stage3StartDate").val()), "/")) {
                    int_error_count++;
                    $("#Stage3StartDate").addClass("warningclass");
                }

                if (str_error_message == "") {
                    str_error_message = "Final Year Review Stage Start Date is not valid. It should be in DD/MM/YYYY format.";
                }
                else {
                    str_error_message = str_error_message + "<br/> Final Year Review Stage Start Date is not valid. It should be in DD/MM/YYYY format.";
                }
            }

            if ($.trim($("#Stage3EndDate").val()).length == 0) {
                int_error_count++;
                $("#Stage3EndDate").addClass("warningclass");

                if (str_error_message == "") {
                    str_error_message = "Please provide the end date of Final Year Review Stage for cycle.";
                }
                else {
                    str_error_message = str_error_message + "<br/> Please provide the end date of Final Year Review Stage for cycle.";
                }
            }
            else {
                if (!validators_boo_checkDate($.trim($("#Stage3EndDate").val()), "/")) {
                    int_error_count++;
                    $("#Stage3EndDate").addClass("warningclass");
                }

                if (str_error_message == "") {
                    str_error_message = "Final Year Review Stage End Date is not valid. It should be in DD/MM/YYYY format.";
                }
                else {
                    str_error_message = str_error_message + "<br/> Final Year Review Stage End Date is not valid. It should be in DD/MM/YYYY format.";
                }
            }

            if (int_error_count == 0) {
                if (getDate($("#Stage1StartDate").val()).getTime() < getDate($("#forRazorValue").attr("nowdateformat")).getTime()) {
                    int_error_count++;
                    $("#Stage1StartDate").addClass("warningclass");
                    str_error_message = "Goal Setting Start Date must not be earlier than today.";
                }
                else if (getDate($("#Stage1StartDate").val()).getTime() >= getDate($("#Stage1EndDate").val()).getTime()) {
                    int_error_count++;
                    $("#Stage1StartDate").addClass("warningclass");
                    str_error_message = "Goal Setting Start Date must be earlier than the Goal Setting End Date.";
                }
                else if (getDate($("#Stage1EndDate").val()).getTime() >= getDate($("#Stage2StartDate").val()).getTime()) {
                    int_error_count++;
                    $("#Stage1EndDate").addClass("warningclass");
                    str_error_message = "Goal Setting End Date must be earlier than the Progress Review Start Date.";
                }
                else if (getDate($("#Stage2StartDate").val()).getTime() >= getDate($("#Stage2EndDate").val()).getTime()) {
                    int_error_count++;
                    $("#Stage2StartDate").addClass("warningclass");
                    str_error_message = "Progress Review End Date must be later than the Progress Review Start Date.";
                }
                else if (getDate($("#Stage2EndDate").val()).getTime() >= getDate($("#Stage3StartDate").val()).getTime()) {
                    int_error_count++;
                    $("#Stage2EndDate").addClass("warningclass");
                    str_error_message = "Progress Review End Date must be earlier than the Final Year Review Start Date.";
                }
                else if (getDate($("#Stage3StartDate").val()).getTime() >= getDate($("#Stage3EndDate").val()).getTime()) {
                    int_error_count++;
                    $("#Stage3StartDate").addClass("warningclass");
                    str_error_message = "Final Year Review End Date must be later than the Final Year Review Start Date.";
                }
            }

            if (int_error_count == 0) {
                $("div.div_error").text("");
                $("div.div_error").removeClass("warningclass");
                $("form").submit();
            }
            else {
                $("div.div_error").html(str_error_message + "<br/><br/>");
                $("div.div_error").addClass("warningclass");
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