﻿function defaultVal(type) {
    if (typeof type !== 'string') throw new TypeError('Type must be a string.');
    switch (type) {
        case 'boolean':
            return false;
        case 'function':
            return function () { };
        case 'null':
            return null;
        case 'number':
            return 0;
        case 'object':
            return {};
        case 'string':
            return "";
        case 'symbol':
            return Symbol();
        case 'undefined':
            return void 0;
    }
    try {
        var ctor = typeof this[type] === 'function' ?
            this[type] :
            eval(type);

        return new ctor;
    } catch (e) { return {}; }
}
$.fn.bindJsonToHtml = function (obj) {
    $(this).bindings('create')(obj);
};
$.fn.isSelectHour = function (textOption = undefined, defaultValue = undefined) {
    var select = $(this);
    select.html("");
    if (textOption !== undefined) {
        var option = "<option value=''>" + textOption + "</option>";
        select.append(option);
    }
    for (var i = 1; i < 24; i++) {
        var optionValue = "<option value='" + i + "'>" + i + " giờ</option>";
        select.append(optionValue);
    }
    if (defaultValue !== undefined) {
        select.val(defaultValue);
    }
    return this;
};
$.fn.isSelectMinutes = function (textOption = undefined, defaultValue = undefined) {
    var select = $(this);
    select.html("");
    if (textOption !== undefined) {
        var option = "<option value=''>" + textOption + "</option>";
        select.append(option);
    }
    for (var i = 0; i < 60; i++) {
        var val = "";
        if (i < 10) {
            val = "0" + i;
        } else {
            val = i.toString();
        }
        var optionValue = "<option value='" + i + "'>" + val + " phút</option>";
        select.append(optionValue);
    }
    if (defaultValue !== undefined) {
        select.val(defaultValue);
    }
    return this;
};
$.fn.setDataSourceDonVi = function (data, displayMember, valueMember, textOption, defaultValue) {
    var select = $(this);
    select.html("");
    if (textOption !== undefined) {
        var option = "<option value=''>" + textOption + "</option>";
        select.append(option);
    }
    $.each(data, function (key, value) {
        var option = "";
        if (value["cap_do"] === 1) {
            option = "<option value='" + value[valueMember] + "' class='text-bold'>" + value[displayMember] + "</option>";
        }
        if (value["cap_do"] === 2) {
            option = "<option value='" + value[valueMember] + "'>+ " + value[displayMember] + "</option>";
        }
        select.append(option);
    });
    if (defaultValue !== undefined) {
        select.val(defaultValue);
    }
    return this;
};
$.fn.addEventLoadChildren = function (data, name, parent, displayMember, valueMember, textOption, defaultValue, callback = undefined) {
    var select = $(this);
    var formName = '';
    try {
        formName = "form[name='" + select[0].form.name + "'] ";
    } catch {
        formName = '';
    }
    select.off("select2:select");
    select.on("select2:select", function (e) {
        var value = $(this).val();
        var selectChildren = $(formName + "select[name='" + name + "']");
        selectChildren.html("");
        if (value === null) {
            return;
        }
        if (textOption !== undefined) {
            var option = "<option value=''>" + textOption + "</option>";
            selectChildren.append(option);
        }
        var arr = $.grep(data, function (n, i) {
            return n[parent].toString() === value.toString();
        });
        $.each(arr, function (key, value) {
            var option = "<option value='" + value[valueMember] + "'>" + value[displayMember] + "</option>";
            selectChildren.append(option);
        });
        if (defaultValue !== undefined) {
            selectChildren.val(defaultValue);
        }
        if (callback !== undefined) {
            callback(value);
        }
    });

    return this;
};
$.fn.addEventUnSelected = function (callback) {
    var select = $(this);
    var formName = '';
    try {
        formName = "form[name='" + select[0].form.name + "'] ";
    } catch {
        formName = '';
    }
    select.off("select2:unselect");
    select.on("select2:unselect", function (e) {
        var value = $(this).val();
        callback(value);
    });
    return this;
};
$.fn.selectedValue = function (value) {
    var select = $(this);
    if (value !== undefined && value !== null) {
        select.val(value.toString()).trigger("select2:select");
    } else {
        select.val("").trigger("select2:select");
    }
    return this;
};
$.fn.showImage = function (url) {
    if (url !== undefined && url !== null) {
        $(this).attr("src", url);
    }
};
$.fn.setImageDefault = function (type) {
    switch (type) {
        case "NGUOI_DUNG":
            $(this).attr("src", "/Content/asset/img/avatars/anh_dai_dien_default.jpg");
            break;
        default:
    }
};
$.fn.addEventChangeImage = function () {
    var ctrl = $(this);
    var name = ctrl[0].name;
    ctrl.change(function () {
        if (ctrl[0].files && ctrl[0].files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#show_' + name).attr('src', e.target.result);
            };
            reader.readAsDataURL(ctrl[0].files[0]);
        }
    });
};
$.fn.inputMoney = function () {
    var ctrl = $(this);
    ctrl.autoNumeric('init', { aPad: false, vMin: '0', mDec: '0' });
    return ctrl;
};
Array.prototype.where = function (lamdaExpr) {
    return $.grep(this, lamdaExpr);
};
Array.prototype.select = function (lamdaExpr) {
    var arr = [];
    $.each(this, (i, item) => {
        arr.push(lamdaExpr(item));
    });
    return arr;
};
Array.prototype.sum = function (lamdaExpr) {
    var sum = 0;
    $.each(this, (i, item) => {
        sum += lamdaExpr(item) || 0;
    });
    return sum;
};
Array.prototype.max = function (lamdaExpr) {
    var max = 0;
    $.each(this, (i, item) => {
        if (lamdaExpr(item)>max) {
            max = lamdaExpr(item)
        }
    });
    return max;
};
Array.prototype.clone = function (lamdaExpr) {
    return this.slice();
};
Array.prototype.firstOrDefault = function () {
    return this.length <= 0 ? null : this[0];
};
Array.prototype.sortBy = function (p) {
    return this.slice(0).sort(function (a, b) {
        return (a[p] > b[p]) ? 1 : (a[p] < b[p]) ? -1 : 0;
    });
}
Array.prototype.removeItem = function (lamdaExpr) {
    var arr = this.filter(item => !lamdaExpr(item));
    if (arr == undefined || arr == null) {
        arr = [];
    }
    return JSON.parse(JSON.stringify(arr));
}

//Add sự kiện khi thay đổi chọn select2
$.fn.addEventChange = function (callback) {
    var select = $(this);
    select.off("select2:select");
    select.on("select2:select", function (e) {
        var value = $(this).val();
        callback(value);
    });
    return this;
};
//Set datasource cho select2
$.fn.setDataSource = function (data, displayMember, valueMember, textOption = undefined, defaultValue = "", keyCacheHtml = undefined) {
    var select = $(this);
    var multiple = $(this).attr("multiple");
    select.html("");
    if (keyCacheHtml !== undefined) {
        var storageHtml = ESStorage.getItemLocalStorage(keyCacheHtml);
        if (storageHtml !== undefined && storageHtml !== null && storageHtml !== "") {
            select.html(storageHtml);
            if (defaultValue !== undefined) {
                select.val(defaultValue);
            }
            return this;
        }
    }
    var html = "";
    if (textOption !== undefined && multiple == undefined) {
        html += "<option value=''>" + textOption + "</option>";
        //select.append(option);
    }
    $.each(data, function (key, value) {
        html += "<option value='" + value[valueMember] + "'>" + value[displayMember] + "</option>";
        //select.append(option);
    });
    select.html(html);
    if (keyCacheHtml !== undefined) {
        ESStorage.setItemLocalStorage(keyCacheHtml, html);
    }
    if (defaultValue !== undefined) {
        select.val(defaultValue);
    }
    return this;
};
//Set datasource cho select2
$.fn.checkBindData = function () {
    var select = $(this);
    if (select.html()) {
        return select.html().trim() != "";
    }
    return false;
};
//Lấy giá trị từ 1 control
$.fn.getValue = function () {
    var element = $(this);
    switch (element.prop("type")) {
        case "radio":
            var val_radio = null;
            element.each(function () {
                if ($(this).is(':checked')) {
                    val_radio = $(this).val();
                }
            });
            return val_radio;
        case "checkbox":
            var val_checkbox = null;
            if (element !== undefined && element.length === 1) {
                element.each(function () {
                    if ($(this).is(':checked')) {
                        val_checkbox = true;
                    }
                });
            } else if (element !== undefined && element.length > 1) {
                val_checkbox = [];
                element.each(function () {
                    if ($(this).is(':checked')) {
                        val_checkbox.push($(this).val());
                    }
                });
            }
            return val_checkbox;
        default:
            if (element.hasClass("number")) {
                return element.val().replace(/[^0-9]+/g, '');
            }
            return element.val();
    }
};
//Set giá trị cho 1 control
$.fn.setValue = function (value) {
    var ctrl = $(this);
    var common = new CommonService();
    switch (ctrl.prop("type")) {
        case "radio":
            ctrl.each(function () {
                if ($(this).val() === value.toString()) {
                    $(this).attr('checked', true);
                    //$(this).prop('checked', true);
                }
            });
            break;
        case "file":
            break;
        case "checkbox":
            if (!Array.isArray(value)) {
                ctrl.each(function () {
                    if (value.toString() === "true")
                        $(this).prop('checked', true);
                });
            } else {
                ctrl.each(function () {
                    var el = $(this);
                    value.find(function (val) {
                        if (el.val() === val.toString())
                            el.prop('checked', true);
                    });
                });
            }
            break;
        case "textarea":
            ctrl.html(value);
            break;
        case "select-one":
            ctrl.val(value == null?"": value.toString()).trigger('change');
            break;
        case "date":
            if (value !== null) {
                ctrl.val(convertDateServerToClient(value.toString()));
            }
            break;
        default:
            if (ctrl.hasClass("number")) {
                if (typeof value === 'string') {
                    value = value.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                } else {
                    value = value.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                }
            }
            ctrl.val(value);
    }

};
//Set control chỉ đọc
$.fn.readOnly = function (action = true) {
    var ctrl = $(this);
    if (action) {
        ctrl.attr("readonly", "readonly");
    } else {
        ctrl.removeAttr("readonly");
    }
};
//Set min date cho datetimepicker
$.fn.setMinDate = function (strMinDate) {
    $(this).data('daterangepicker').setMinDate(strMinDate);
}
//Set max date cho datetimepicker
$.fn.setMaxDate = function (strMaxDate) {
    $(this).data('daterangepicker').setMaxDate(strMaxDate);
}
//set event change date
$.fn.addEventChangeDate = function (callback) {
    $(this).on('apply.daterangepicker', function (ev, picker) {
        callback($(this).val());
    });
}
$.fn.addTooltip = function (noidung) {
    Tipped.create(this, noidung, {
        skin: 'red',
        position: 'topleft',
    });
};
Date.prototype.HHmm = function () {
    var HH = this.getHours();
    var mm = this.getMinutes();
    return [
        (HH > 9 ? '' : '0') + HH,
        (mm > 9 ? '' : '0') + mm
    ].join(':');
};
Date.prototype.HHmmAddMinutes = function (minutes) {
    var date = new Date(this.getTime() + minutes * 60000)
    var HH = date.getHours();
    var mm = date.getMinutes();
    return [
        (HH > 9 ? '' : '0') + HH,
        (mm > 9 ? '' : '0') + mm
    ].join(':');
};
Date.prototype.yyyymmdd = function () {
    var mm = this.getMonth() + 1;
    var dd = this.getDate();
    return [this.getFullYear(),
    (mm > 9 ? '' : '0') + mm,
    (dd > 9 ? '' : '0') + dd
    ].join('-');
};
Date.prototype.ddmmyyyy = function (year = 0) {
    var mm = this.getMonth() + 1;
    var dd = this.getDate();
    return [(dd > 9 ? '' : '0') + dd,
    (mm > 9 ? '' : '0') + mm,
    this.getFullYear() + year
    ].join('/');
};
Date.prototype.getNgayDauThang = function (year = 0) {
    var mm = this.getMonth() + 1;
    return ["01",
        (mm > 9 ? '' : '0') + mm,
        this.getFullYear() + year
    ].join('/');
};
Date.prototype.getNgayDauNam = function (year = 0) {
    return ["01",
        "01",
        this.getFullYear() + year
    ].join('/');
};
Date.prototype.toDateInputValue = (function () {
    var local = new Date(this);
    local.setMinutes(this.getMinutes() - this.getTimezoneOffset());
    return local.toJSON().slice(0, 10);
});
String.prototype.dateToNumber = function () {
    var arr = this.split('/');
    return parseInt(arr[2] + arr[1] + arr[0]);
};
String.prototype.timeToNumber = function () {
    var arr = this.split(':');
    return parseInt(arr[0] + arr[1]);
};
String.prototype.like = function (search) {
    if (typeof search !== 'string' || this === null) { return false; }
    // Remove special chars
    search = search.replace(new RegExp("([\\.\\\\\\+\\*\\?\\[\\^\\]\\$\\(\\)\\{\\}\\=\\!\\<\\>\\|\\:\\-])", "g"), "\\$1");
    // Replace % and _ with equivalent regex
    search = search.replace(/%/g, '.*').replace(/_/g, '.');
    // Check matches
    return RegExp('^' + search + '$', 'gi').test(this);
}
String.prototype.indexLike = function (search) {
    if (typeof search !== 'string' || this === null) { return false; }
    search = search.split(' ')[0].trim();
    search = search.replace(new RegExp("([\\.\\\\\\+\\*\\?\\[\\^\\]\\$\\(\\)\\{\\}\\=\\!\\<\\>\\|\\:\\-])", "g"), "\\$1");
    // Replace % and _ with equivalent regex
    search = search.replace(/%/g, '.*').replace(/_/g, '.');
/*    console.log("search", search);*/
    var match = new RegExp(search).exec(this);
    if (match) {
        return match.index;
    }
    return null;
}
Number.prototype.numberToDate = function () {
    var str = this.toString();
    var year = str.substring(-1, 4);
    var month = str.substring(4, 6);
    var day = str.substring(6, 8);
    return day + "/" + month + "/" + year;
};