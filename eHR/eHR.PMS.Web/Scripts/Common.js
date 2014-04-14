define(function () {
    var specialCharArray = [":", ";", "/", "?", "@", "&", "=", "+", "$", "#"];
    var specialCharArrayString = ":;/?@&=+$#";
    var ErrorMessgae = "You are not allowed to enter more than 2 special characters (" + specialCharArrayString + ") in a continuous sequence.";
    if (!Array.prototype.indexOf) {
        Array.prototype.indexOf = function (elt /*, from*/) {
            var len = this.length >>> 0;

            var from = Number(arguments[1]) || 0;
            from = (from < 0)
         ? Math.ceil(from)
         : Math.floor(from);
            if (from < 0)
                from += len;

            for (; from < len; from++) {
                if (from in this &&
          this[from] === elt)
                    return from;
            }
            return -1;
        };
    };
    return {
        GetspecialCharErrorMessage: function () {
            return ErrorMessgae;
        },
        IsSpecialChar: function (str) {
            for (var k = 0; k < specialCharArray.length; k++) {
                var index = str.indexOf(specialCharArray[k])
                if (index > -1) {
                    var nextchar = str.substr(index + 1, 1);
                    var nextcharEx = str.substr(index + 2, 1);
                    if (specialCharArray.indexOf(nextchar) > -1 && specialCharArray.indexOf(nextcharEx) > -1)
                        return true;
                }
            }
            return false;
        }
    }
})
