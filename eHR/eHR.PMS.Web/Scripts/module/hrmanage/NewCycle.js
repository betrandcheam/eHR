require(['Config'], function () {
    require(['hrmanage.newcycle']);
}),
define("hrmanage.newcycle", ['jquery', 'bootstrap', 'moment', 'bootstrap.datetimepicker', 'bootstrap.paginator', 'validators'], function ($) {
    $(function () {
        var total = $("#forRazorValue").attr("num");
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
                        data: { "page": page, "Stage1EndDate": $("#Stage1EndDate").val(), "Stage3EndDate": $("#Stage3EndDate").val() },
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
               window.location.href = "(Url.Content("~/"))HRManage/AddParticipants/" + $("#strStage1EndDate").val() + "/" + $("#strStage3EndDate").val();
            }
        });
        $("#RemoveParticipants").click(function () {
            if (intTotal == 0) {
                $("#RemoveParticipantModal").model();
                return false;
            }
            else {
               window.location.href = "(Url.Content("~/"))HRManage/RemoveParticipants/" + $("#strStage1EndDate").val() + "/" + $("#strStage3EndDate").val();
            } 
        });*/
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
        $('#datetimepicker_stage1submissiondeadline').datetimepicker({
            format: "DD/MM/YYYY",
            pickTime: false
        });
        $('#datetimepicker_stage1level1approvaldeadline').datetimepicker({
            format: "DD/MM/YYYY",
            pickTime: false
        });
        $('#datetimepicker_stage1level2approvaldeadline').datetimepicker({
            format: "DD/MM/YYYY",
            pickTime: false
        });
        $('#datetimepicker_stage2submissiondeadline').datetimepicker({
            format: "DD/MM/YYYY",
            pickTime: false
        });
        $('#datetimepicker_stage2level1approvaldeadline').datetimepicker({
            format: "DD/MM/YYYY",
            pickTime: false
        });
        $('#datetimepicker_stage2level2approvaldeadline').datetimepicker({
            format: "DD/MM/YYYY",
            pickTime: false
        });


        $("#btn_start_cycle").click(function () {
            $("#startcycle").val("1");
            $("modelmessage").text("Are you sure approve this now?")
            $('#SubmitInfoModal').modal();
        });

        $("#btn_retrieve_participants").click(function () {
            $("#startcycle").val("0");
            var int_error_count = 0;
            var str_error_message = "";

            $("#cyclename").removeClass("warningclass");
            $("#Stage1StartDate").removeClass("warningclass");
            $("#Stage1EndDate").removeClass("warningclass");
            $("#Stage2StartDate").removeClass("warningclass");
            $("#Stage2EndDate").removeClass("warningclass");
            $("#Stage3StartDate").removeClass("warningclass");
            $("#Stage3EndDate").removeClass("warningclass");

            if ($.trim($("#cyclename").val()).length == 0)
            {
                int_error_count ++;
                $("#cyclename").addClass("warningclass");

                str_error_message = "Please provide the name of the cycle.";
            }

            if ($.trim($("#Stage1StartDate").val()).length == 0)
            {
                int_error_count ++;
                $("#Stage1StartDate").addClass("warningclass");

                if (str_error_message == "")
                {
                    str_error_message = "Please provide the start date of Goal Setting Stage for cycle.";
                }
                else
                {   
                    str_error_message = str_error_message + "<br/> Please provide the start date of Goal Setting Stage for cycle.";
                }
            }
            else
            {
                if (!validators_boo_checkDate($.trim($("#Stage1StartDate").val()),"/"))
                {
                    int_error_count ++;
                    $("#Stage1StartDate").addClass("warningclass");

                    if (str_error_message == "")
                    {
                        str_error_message = "Goal Setting Stage Start Date is not valid. It should be in DD/MM/YYYY format.";
                    }
                    else
                    {   
                        str_error_message = str_error_message + "<br/> Goal Setting Stage Start Date is not valid. It should be in DD/MM/YYYY format.";
                    }
                }
            }

            if ($.trim($("#Stage1EndDate").val()).length == 0)
            {
                int_error_count ++;
                $("#Stage1EndDate").addClass("warningclass");

                if (str_error_message == "")
                {
                    str_error_message = "Please provide the end date of Goal Setting Stage for cycle.";
                }
                else
                {   
                    str_error_message = str_error_message + "<br/> Please provide the end date of Goal Setting Stage for cycle.";
                }
            }
            else
            {
                if (!validators_boo_checkDate($.trim($("#Stage1EndDate").val()),"/"))
                {
                    int_error_count ++;
                    $("#Stage1EndDate").addClass("warningclass");
                
                    if (str_error_message == "")
                    {
                        str_error_message = "Goal Setting Stage End Date is not valid. It should be in DD/MM/YYYY format.";
                    }
                    else
                    {   
                        str_error_message = str_error_message + "<br/> Goal Setting Stage End Date is not valid. It should be in DD/MM/YYYY format.";
                    }
                }
            }
 
            if ($.trim($("#Stage2StartDate").val()).length == 0)
            {
                int_error_count ++;
                $("#Stage2StartDate").addClass("warningclass");

                if (str_error_message == "")
                {
                    str_error_message = "Please provide the start date of Progress Review Stage for cycle.";
                }
                else
                {   
                    str_error_message = str_error_message + "<br/> Please provide the start date of Progress Review Stage for cycle.";
                }
            }
            else
            {
                if (!validators_boo_checkDate($.trim($("#Stage2StartDate").val()),"/"))
                {
                    int_error_count ++;
                    $("#Stage2StartDate").addClass("warningclass");

                    if (str_error_message == "")
                    {
                        str_error_message = "Progress Review Stage Start Date is not valid. It should be in DD/MM/YYYY format.";
                    }
                    else
                    {   
                        str_error_message = str_error_message + "<br/> Progress Review Stage Start Date is not valid. It should be in DD/MM/YYYY format.";
                    }
                }
            }

            if ($.trim($("#Stage2EndDate").val()).length == 0)
            {
                int_error_count ++;
                $("#Stage2EndDate").addClass("warningclass");

                if (str_error_message == "")
                {
                    str_error_message = "Please provide the end date of Progress Review Stage for cycle.";
                }
                else
                {   
                    str_error_message = str_error_message + "<br/> Please provide the end date of Progress Review Stage for cycle.";
                }
            }
            else
            {
                if (!validators_boo_checkDate($.trim($("#Stage2EndDate").val()),"/"))
                {
                    int_error_count ++;
                    $("#Stage2EndDate").addClass("warningclass");
                }

                if (str_error_message == "")
                {
                    str_error_message = "Progress Review Stage End Date is not valid. It should be in DD/MM/YYYY format.";
                }
                else
                {   
                    str_error_message = str_error_message + "<br/> Progress Review End Date is not valid. It should be in DD/MM/YYYY format.";
                }
            }

            if ($.trim($("#Stage3StartDate").val()).length == 0)
            {
                int_error_count ++;
                $("#Stage3StartDate").addClass("warningclass");

                if (str_error_message == "")
                {
                    str_error_message = "Please provide the start date of Final Year Review Stage for cycle.";
                }
                else
                {   
                    str_error_message = str_error_message + "<br/> Please provide the start date of Final Year Review Stage for cycle.";
                }
            }
            else
            {
                if (!validators_boo_checkDate($.trim($("#Stage3StartDate").val()),"/"))
                {
                    int_error_count ++;
                    $("#Stage3StartDate").addClass("warningclass");
                }

                if (str_error_message == "")
                {
                    str_error_message = "Final Year Review Stage Start Date is not valid. It should be in DD/MM/YYYY format.";
                }
                else
                {   
                    str_error_message = str_error_message + "<br/> Final Year Review Stage Start Date is not valid. It should be in DD/MM/YYYY format.";
                }
            }

            if ($.trim($("#Stage3EndDate").val()).length == 0)
            {
                int_error_count ++;
                $("#Stage3EndDate").addClass("warningclass");

                if (str_error_message == "")
                {
                    str_error_message = "Please provide the end date of Final Year Review Stage for cycle.";
                }
                else
                {   
                    str_error_message = str_error_message + "<br/> Please provide the end date of Final Year Review Stage for cycle.";
                }
            }
            else
            {
                if (!validators_boo_checkDate($.trim($("#Stage3EndDate").val()),"/"))
                {
                    int_error_count ++;
                    $("#Stage3EndDate").addClass("warningclass");
                }

                if (str_error_message == "")
                {
                    str_error_message = "Final Year Review Stage End Date is not valid. It should be in DD/MM/YYYY format.";
                }
                else
                {   
                    str_error_message = str_error_message + "<br/> Final Year Review Stage End Date is not valid. It should be in DD/MM/YYYY format.";
                }
            }

            if (int_error_count == 0)
            {
                if (getDate($("#Stage1StartDate").val()).getTime() < getDate($("#forRazorValue").attr("nowdate")).getTime())
                {
                    int_error_count ++;
                    $("#Stage1StartDate").addClass("warningclass");
                    str_error_message = "Goal Setting Start Date must not be earlier than today.";
                }
                else if(getDate($("#Stage1StartDate").val()).getTime() >= getDate($("#Stage1EndDate").val()).getTime())
                {
                    int_error_count ++;
                    $("#Stage1StartDate").addClass("warningclass");
                    str_error_message = "Goal Setting Start Date must be earlier than the Goal Setting End Date.";
                }
                else if(getDate($("#Stage1EndDate").val()).getTime() >= getDate($("#Stage2StartDate").val()).getTime())
                {
                    int_error_count ++;
                    $("#Stage1EndDate").addClass("warningclass");
                    str_error_message = "Goal Setting End Date must be earlier than the Progress Review Start Date.";
                }
                else if(getDate($("#Stage2StartDate").val()).getTime() >= getDate($("#Stage2EndDate").val()).getTime())
                {
                    int_error_count ++;
                    $("#Stage2StartDate").addClass("warningclass");
                    str_error_message = "Progress Review End Date must be later than the Progress Review Start Date.";
                }
                else if(getDate($("#Stage2EndDate").val()).getTime() >= getDate($("#Stage3StartDate").val()).getTime())
                {
                    int_error_count ++;
                    $("#Stage2EndDate").addClass("warningclass");
                    str_error_message = "Progress Review End Date must be earlier than the Final Year Review Start Date.";
                }
                else if(getDate($("#Stage3StartDate").val()).getTime() >= getDate($("#Stage3EndDate").val()).getTime())
                {
                    int_error_count ++;
                    $("#Stage3StartDate").addClass("warningclass");
                    str_error_message = "Final Year Review End Date must be later than the Final Year Review Start Date.";
                }
            }

            if(int_error_count == 0)
            {
                $("div.div_error").text("");
                $("div.div_error").removeClass("warningclass");
                $("form").submit();
            }
            else
            {
                $("div.div_error").html(str_error_message + "<br/><br/>");
                $("div.div_error").addClass("warningclass");
            }
        });

        $("#btn_cancel").click(function () {
            $('#CancelInfoModal').modal();
        });

        $("#btn_cancel_modal_ok").click(function () {
            window.location.href($("#forRazorValue").attr("rooturl"));
        });

        var message = $("#forRazorValue").attr("message");
        $(function(){
            if (message)
                $('#RedirectModal').modal();
        });

        
        var error_message = $("#forRazorValue").attr("errormessage");
        $(function(){
            if (error_message)
            {
                $('#ErrorModal').modal();     
            }
        });

        function getDate(datestring)
        {
            var dateparts = datestring.split("/");
            return new Date(dateparts[2],dateparts[1],dateparts[0]);
       }
    });
});