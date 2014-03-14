require.config({
    paths: {
        "css3.mediaqueries": "lib/css3-mediaqueries",
        "jquery": "lib/jquery-1.9.1",
        "bootstrap": "lib/bootstrap",
        "bootstrap.select": "lib/bootstrap-select.min",
        "bootstrap.datetimepicker": "lib/bootstrap-datetimepicker.min",
        "bootstrap.paginator": "lib/bootstrap-paginator",
        "typeahead": "lib/typeahead.min",
        "moment": "lib/moment.min",
        "validators": "lib/validators"
    },
    shim: {
        "bootstrap": {
            deps: ["jquery"]
        },
        "bootstrap.select": {
            deps: ["bootstrap"]
        },
        "bootstrap.datetimepicker": {
            deps: ["bootstrap", "moment"]
        },
        "bootstrap.paginator": {
            deps: ["bootstrap"]
        }
    }
});