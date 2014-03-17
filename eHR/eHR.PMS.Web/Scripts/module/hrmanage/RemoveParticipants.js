require(['Config'], function () {
    require(['hrmanage.removeparticipants']);
}),
define("hrmanage.removeparticipants", ['jquery', 'bootstrap', 'bootstrap.paginator'], function ($) {
    $(function () {
        var total = $("#forRazorValue").attr("participantscount");
        var intTotal = parseInt(total);
        if (intTotal != 0) {
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
                        data: { "page": page, "Stage1EndDate": $("#forRazorValue").attr("stage1enddate"), "Stage3EndDate": $("#forRazorValue").attr("stage3enddate"), "EmployeeName": $("#hidEmployeeName").val(), "DomainID": $("#hidDomainID").val(), "DepartmentName": $("#hidDepartmentName").val(), "cycleId": $("#forRazorValue").attr("cycleid") },
                        beforeSend: function () {
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
        if($(".isadd").length>0){
            $("#SelectAll").show();
        }
        else
            $("#SelectAll").hide();
        $("#SelectAll").click(function(){
            if($(this).prop("checked")){
                $(".isadd").prop("checked",true);
                var html="";
                $.each($(".isadd"),function(){
                    html=[html,"<li class='participant' eid='" , $(this).attr("eid") , "'>" , $(this).attr("objectvalue") , "</li>"].join("");
                });
                $(html).appendTo($("#totallist"));
            }
            else{
                $(".isadd").prop("checked",false);
                $("#totallist li").remove();
            }

        });
        $('#showpage').change(function () {
            var page = $(this).val();
            $('#pages').bootstrapPaginator("show", page);
        })
        $("#eicontent").on("click", ".isadd", function () {
            if ($(this).prop("checked")) {
                var html = "<li class='participant' eid='" + $(this).attr("eid") + "'>" + $(this).attr("objectvalue") + "</li>";
                $(html).appendTo($("#totallist"));
            }
            else {
                var selector = "#totallist li[eid='" + $(this).attr("eid") + "']";
                $(selector).remove();
            }
        });

        $("#btn_add_modal_ok").click(function(){
            $("#EmployeeName").val($("#hidEmployeeName").val());
            $("#DomainID").val($("#hidDomainID").val());
            $("#DepartmentName").val($("#hidDepartmentName").val());
            $("form").submit();
        });

        $("#RemoveParticipants").click(function () {
            if ($("#totallist li").length == 0) {
                $("#info").text("Please select at least one record for removal.");
                $('#RedictModal').modal();
                return false;
            }
            var EIForCache = "";
            $.each($(".participant"), function () {
                EIForCache = [EIForCache, $(this).text()].join();
            });
            $.ajax({
                url: $("#forRazorValue").attr("confirmaddurl"),
                type: "post",
                dataType: "json",
                data: { "cycleDateRangeStart": $("#forRazorValue").attr("stage1enddate"), "cycleDateRangeEnd": $("#forRazorValue").attr("stage3enddate"), "cycleId": $("#forRazorValue").attr("cycleid"), "EIForCache": EIForCache.substr(1) },
                beforeSend: function () {
                    $('#InfoModal').modal();
                },
                success: function (data) {
                    $("#addinfo").text(data);
                    $('#InfoModal').modal('hide');
                    $('#ConfirmAddModal').modal();
                }
            });
        });
    });
});