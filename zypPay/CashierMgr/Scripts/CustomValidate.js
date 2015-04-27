// <script src="jquery.validate1.js" type="text/javascript"></script>
//    <script src="query.validate.vaidationgroup.js" type="text/javascript"></script>
//    <script src="jQuery.validate.message_cn.js" type="text/javascript"></script>
$(document).ready(function () {
    /** 
    * 身份证号码验证 
    * 
    */
    function isIdCardNo(num) {

        var factorArr = new Array(7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1);
        var parityBit = new Array("1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2");
        var varArray = new Array();
        var intValue;
        var lngProduct = 0;
        var intCheckDigit;
        var intStrLen = num.length;
        var idNumber = num;
        // initialize  
        if ((intStrLen != 15) && (intStrLen != 18)) {
            return false;
        }
        // check and set value  
        for (i = 0; i < intStrLen; i++) {
            varArray[i] = idNumber.charAt(i);
            if ((varArray[i] < '0' || varArray[i] > '9') && (i != 17)) {
                return false;
            } else if (i < 17) {
                varArray[i] = varArray[i] * factorArr[i];
            }
        }

        if (intStrLen == 18) {
            //check date  
            var date8 = idNumber.substring(6, 14);
            if (isDate8(date8) == false) {
                return false;
            }
            // calculate the sum of the products  
            for (i = 0; i < 17; i++) {
                lngProduct = lngProduct + varArray[i];
            }
            // calculate the check digit  
            intCheckDigit = parityBit[lngProduct % 11];
            // check last digit  
            if (varArray[17] != intCheckDigit) {
                return false;
            }
        }
        else {        //length is 15  
            //check date  
            var date6 = idNumber.substring(6, 12);
            if (isDate6(date6) == false) {

                return false;
            }
        }
        return true;

    }
    /** . * 判断是否为“YYYYMM”式的时期 
    * 
    */
    function isDate6(sDate) {
        if (!/^[0-9]{6}$/.test(sDate)) {
            return false;
        }
        var year, month, day;
        year = sDate.substring(0, 4);
        month = sDate.substring(4, 6);
        if (year < 1700 || year > 2500) return false
        if (month < 1 || month > 12) return false
        return true
    }
    /** 
    * 判断是否为“YYYYMMDD”式的时期 
    * 
    */
    function isDate8(sDate) {
        if (!/^[0-9]{8}$/.test(sDate)) {
            return false;
        }
        var year, month, day;
        year = sDate.substring(0, 4);
        month = sDate.substring(4, 6);
        day = sDate.substring(6, 8);
        var iaMonthDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]
        if (year < 1700 || year > 2500) return false
        if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0)) iaMonthDays[1] = 29;
        if (month < 1 || month > 12) return false
        if (day < 1 || day > iaMonthDays[month - 1]) return false
        return true
    }
    // 身份证号码验证     
    jQuery.validator.addMethod("idcardno", function (value, element) {
        if ($.trim(value).length > 0) {
            return this.optional(element) || isIdCardNo(value);
        }
        else {
            return true;
        }
    }, "请正确输入身份证号码");

    //字母数字  
    jQuery.validator.addMethod("alnum", function (value, element) {
        return this.optional(element) || /^[a-zA-Z0-9]+$/.test(value);
    }, "只能包括英文字母和数字");

    // 邮政编码验证  
    jQuery.validator.addMethod("zipcode", function (value, element) {
        if ($.trim(value).length > 0) {
            var tel = /^[0-9]{6}$/;
            return this.optional(element) || (tel.test(value));
        }
        else return true;
    }, "请正确填写邮政编码");

    // 银行代码验证  
    jQuery.validator.addMethod("cbankcode", function (value, element) {
        if ($.trim(value).length > 0) {
            var tel = /^[0-9]{12}$/;
            return this.optional(element) || (tel.test(value));
        }
        else return true;
    }, "银行代码12位数字");
    // 汉字字母数字_.·  
    jQuery.validator.addMethod("nospecial", function (value, element) {
        var tel = /^[a-zA-Z0-9_.·\u4e00-\u9fa5]+$/;
        return this.optional(element) || (tel.test(value));
    }, "请输入汉字、字母、数字、下划线、点");
    // 汉字  
    jQuery.validator.addMethod("chcharacter", function (value, element) {
        var tel = /^[\u4e00-\u9fa5]+$/;
        return this.optional(element) || (tel.test(value));
    }, "请输入汉字");

    //手机号码
    jQuery.validator.addMethod("mobile", function (value, element) {

        if ($.trim(value).length > 0) {
            var tel = /^(130|131|132|145|155|156|185|186|134|135|136|137|138|139|147|150|151|152|157|158|159|182|183|187|188|133|153|189|180)\d{8}$/;
            return tel.test(value) || this.optional(element);
        }
        else {
            return true;
        }

    }, "请输入正确的手机号码");

    //电话号码
    jQuery.validator.addMethod("telephone", function (value, element) {
        if ($.trim(value).length > 0) {
            var tel = /^\d{3,4}-?\d{7,9}$/; 
            return tel.test(value) || this.optional(element);
        }
        else {
            return true;
        }
    }, "请输入正确的电话号码");


    //电话或者手机
    jQuery.validator.addMethod("phoneormobile", function (value, element) {
        var tel = /^\d{3,4}-?\d{7,9}$/;
        var mobile = /^(130|131|132|133|134|135|136|137|138|139|147|150|153|157|158|159|180|182|186|187|188|189)\d{8}$/;
        return this.optional(element) || (tel.test(value) || mobile.test(value));
    }, "请输入正确的联系电话号码");



    jQuery.validator.addMethod("nowDate",
       function (value, element) {

           return new Date() >= new Date(Date.parse(value.replace("-", "/")));
       },
       "时间不能早于当前时间");



   });

   jQuery.validator.addMethod("positiveinteger",
    function (value, element) {
        if ($.trim(value).length > 0) {
            var aint = parseInt(value);
            return aint > 0 && (aint + "") == value;
        } else return true;
    }, "请输入正整数");
    jQuery.validator.addMethod("positiveinteger2",
    function (value, element) {
        if ($.trim(value).length > 0) {
            var aint = parseInt(value);
            return aint >= 0 && (aint + "") == value;
        } else return true;
    }, "请输入正整数");

   jQuery.validator.addMethod("decimal2", 
   function(value, element) {
   var decimal = /^-?\d+(\.\d{1,2})?$/;
   return this.optional(element) || (decimal.test(value));
}, "请输入数字，小数点后保留两位小数");

jQuery.validator.addMethod("positivedecimal2",
   function (value, element) {
       var decimal = /^-?\d+(\.\d{1,2})?$/;
       
       return this.optional(element) || (decimal.test(value) && Number(value)>0);
   }, "请输入大于0的数字，小数点后保留两位小数");

jQuery.validator.addMethod("small", function (value, element, param) {
    if ($.trim(value).length > 0 && jQuery(param).val().length>0) {
        return Number(value) < Number(jQuery(param).val());
    } else return true;
}, "最小值必须小于最大值");

jQuery.validator.addMethod("lat", function (value, element) {
    if ($.trim(value).length > 0 ) {
        return (Number(value) <=90 && Number(value)>=0);
    } else return true;
}, "纬度必须为0-90之间的数字");

jQuery.validator.addMethod("lon", function (value, element) {
    if ($.trim(value).length > 0 ) {
        return (Number(value) >= 0 && Number(value)<=180);
    } else return true;
}, "经度必须为0-180之间的数字");

jQuery.validator.addMethod("more", function (value, element, param) {
    if ($.trim(value).length > 0 && jQuery(param).val().length > 0) {
        return Number(value) > Number(jQuery(param).val());
    } else return true;
}, "输入值必须大于{0}");

jQuery(function(){        
        jQuery.validator.methods.compareDate = function(value, element, param) {
           
            var startDate = jQuery(param).val();
            
            var date1 = new Date(Date.parse(startDate.replace("-", "/")));
            var date2 = new Date(Date.parse(value.replace("-", "/")));
            return date1 <= date2;
        };
    });

    jQuery(function () {
        jQuery.validator.methods.notequalTo = function (value, element, param) {

            return value != $(param).val();
        };
    });

    jQuery.validator.addMethod("maxmoney",
   function (value, element) {
       var decimal = /^[\w\.]{1,10}$/;

       return this.optional(element) || (decimal.test(value) && Number(value) <= 9999999.99);
   }, "请输入小于9999999.99数值");
 

   

    