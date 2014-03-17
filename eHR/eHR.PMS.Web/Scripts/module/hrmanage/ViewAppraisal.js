require(['Config'], function () {
    require(['hrmanage.viewappraisal']);
}),
define("hrmanage.viewappraisal", ['jquery', 'bootstrap', 'bootstrap.select', 'moment', 'bootstrap.datetimepicker', 'bootstrap.paginator', 'validators'], function ($) {
    $(function () {
        $('.selectpicker').selectpicker(); 
        var total = $("#forRazorValue").attr("apprcount");
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

        $("#btn_search").click(function (){
            if ($("#cycleid").val() <= 0)
            {
                $("div.div_error").html("Please select the cycle required.");
                $("div.div_error").addClass("warningclass");
                return false;
            }
            else
            {
                $("div.div_error").text("");
                $("div.div_error").removeClass("warningclass");
                return true;
            }
            
        })

       $("#btn_cancel").click(function () {
           window.location.href($("#forRazorValue").attr("cancelurl"));
        });
    });
});