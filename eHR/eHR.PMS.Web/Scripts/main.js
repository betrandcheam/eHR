require(['Config'], function () {
    require(['layout']);
})
, define("layout", ['jquery', 'bootstrap'], function ($) {
    var stop = $(".navbar").offset().top;
    $(window).scroll(function () {
        /*if ($(this).scrollTop() > (stop + $(".navbar").height()) && $(".navbar-static-top").length > 0) {
        //$(".navbar").removeClass("navbar-static-top").addClass("navbar-fixed-top");
        $("#fixedtopnav").show();
        }
        else if ($(this).scrollTop() <= (stop + $(".navbar").height()) && $(".navbar-fixed-top").length > 0) {
        //$(".navbar").removeClass("navbar-fixed-top").addClass("navbar-static-top");
        $("#fixedtopnav").hide();
        }*/
        ;
    });
    $(function () {
        $('input[type="text"]').addClass("idleField");
        $('input[type="text"]').focus(function () {
            $(this).removeClass("idleField").addClass("focusField").removeClass("warningclass");
            if (this.value == this.defaultValue && this.value.indexOf("Type something here") == 0) {
                this.value = "";
            }
            if (this.value != this.defaultValue) {
                this.select();
            }
        });
        $('input[type="text"]').blur(function () {
            $(this).removeClass("focusField").addClass("idleField");
            if ($.trim(this.value) == "" && this.defaultValue.indexOf("Type something here") == 0) {
                this.value = (this.defaultValue ? this.defaultValue : '');
            }
        });
        $('textarea').focus(function () {
            $(this).removeClass("idleField").addClass("focusField").removeClass("warningclass");
            if (this.value == this.defaultValue && this.value.indexOf("Type something here") == 0) {
                this.value = "";
            }
            if (this.value != this.defaultValue) {
                this.select();
            }
        });
        $('textarea').blur(function () {
            $(this).removeClass("focusField").addClass("idleField");
            if ($.trim(this.value) == '') {
                this.value = (this.defaultValue ? this.defaultValue : '');
            }
        });
        $.each($('.panel-collapse'), function () {
            $(this).on('shown.bs.collapse', function () {
                $(this).prev().find(".forcollapseicon").removeClass("glyphicon-chevron-down").addClass("glyphicon-chevron-up");
            });

            $(this).on('hidden.bs.collapse', function () {
                $(this).prev().find(".forcollapseicon").removeClass("glyphicon-chevron-up").addClass("glyphicon-chevron-down");
            });
        });
        
        $(document).on("click", function (e) {
            var $target = $(e.target);
            isPopover = $(e.target).is('.ViewKpiComments');
            inPopover = $(e.target).closest('.popover').length > 0;

            //hide only if clicked on button or inside popover
            if (!isPopover && !inPopover) $(".ViewKpiComments").popover('destroy');
        });
    });
})