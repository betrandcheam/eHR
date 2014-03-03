function validators_boo_checkDate(datestring, seperator) {
    var is_valid_date_string = true;
    var required_pattern = "^(\\d{1,2})(\\" + seperator + ")(\\d{1,2})(\\" + seperator + ")(\\d{4})$";
    var reg_exp = new RegExp(required_pattern);
    var dt_array = datestring.match(reg_exp);

    if (!validators_boo_checkDateFormat(datestring, seperator)) {
        is_valid_date_string = false;
    }
    else {
        if (!validators_boo_checkDateValue(datestring, seperator)) {
            is_valid_date_string = false;
        }
    }

    return is_valid_date_string;
}

function validators_boo_checkDateFormat(datestring, seperator) {

    var required_pattern = "^(\\d{1,2})(\\" + seperator + ")(\\d{1,2})(\\" + seperator + ")(\\d{4})$";
    var reg_exp = new RegExp(required_pattern);
    var dt_array = datestring.match(reg_exp);

    return dt_array != null;
}

function validators_boo_checkDateValue(datestring, seperator) {
    var is_valid_date_string = true;
    var required_pattern = "^(\\d{1,2})(\\" + seperator + ")(\\d{1,2})(\\" + seperator + ")(\\d{4})$";
    var reg_exp = new RegExp(required_pattern);
    var dt_array = datestring.match(reg_exp);

    var day = dt_array[1];
    var month = dt_array[3];
    var year = dt_array[5];

    if (day.length > 1) {
        if (day.substring(0, 1) == 0) {
            day = day.substring(1, 2);
        }
    }

    if (month.length > 1) {
        if (month.substring(0, 1) == 0) {
            month = month.substring(1, 2);
        }
    }

    if (year.length != 4) {
        is_valid_date_string = false;
    }

    if (month < 1 || month > 12) {
        is_valid_date_string = false;
    }
    else if (day < 1 || day > 31) {
        is_valid_date_string = false;
    }
    else if (month == 4 || month == 6 || month == 9 || month == 11) {
        if (day == 31) {
            is_valid_date_string = false;
        }
    }
    else if (month == 2) {
        if (day > 29) {
            is_valid_date_string = false;
        }
        else {
            var is_leap_year = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
            if (!is_leap_year && day == 29) {
                is_valid_date_string = false;
            }
        }
    }
    return is_valid_date_string;
}