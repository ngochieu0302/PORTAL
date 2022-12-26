function SelectCheckBoxService(idInput, options = undefined) {
    this.id = idInput;
    this.options = $.extend(true, {
        width_box: 250,
        height_box: 500,
        placeholder: "Click để chọn",
    }, options);

    this.source = [];
    this.source_base = [];
    this.setDataSource = function (arr) {
        var optionsObj = this.options;
        this.source_base = arr;
        var sourceArr = JSON.parse(JSON.stringify(arr));
        //Chuẩn hóa lại json
        //Danh sách mã cấp trên là null
        var ds_cha = sourceArr.where(n => n[optionsObj.group_name] == null);
        for (var i = 0; i < ds_cha.length; i++) {
            ds_cha[i].so_luong_pt = 0;
            var ds_con = sourceArr.where(n => n[optionsObj.group_name] == ds_cha[i][optionsObj.value_name]);
            if (ds_con != undefined && ds_con != null && ds_con.length > 0) {
                ds_cha[i].so_luong_pt = ds_con.length;
                ds_cha[i].children = ds_con;
            }
        }
        this.source = ds_cha;
    }
    this.setCheckedValue = function (arr) {
        $("#" + idInput).attr("data-val", "");
        $("#" + idInput).val("");
        if (arr === undefined || arr === null) {
            return;
        }
        var _instance = this;
        var optionsObj = _instance.options;
        var source = _instance.source;

        var ma = ""; var ten = "";
        for (var i = 0; i < arr.length; i++) {
            var o = $.grep(source, function (n) {
                return n[optionsObj.value_name] === arr[i];
            });
            if (o && o.length > 0) {
                if (i < 4) {
                    if (ten === "") {
                        ten = o[0][optionsObj.dispay_name];
                    }
                    else {
                        ten += ", " + o[0][optionsObj.dispay_name];
                    }
                }
                else {
                    ten = "Có " + (i + 1) + " sự lựa chọn";
                }
            }
            if (i === 0) {
                ma = arr[i];
            }
            else {
                ma += "," + arr[i];
            }
        }
        $("#" + idInput).attr("data-val", ma);
        $("#" + idInput).val(ten);
    };
    this.getValue = function () {
        return $("#" + idInput).data("val");
    }
    this.disabled = function (arr, isDisable = true) {
        $("input[name='custom-control-input-name']").removeAttr("disabled");
        $("#" + idInput).removeAttr("arr-disabled");
        if (arr && isDisable) {
            var arr_disabled = "";
            var i = 0;
            $("input:checkbox[name='custom-control-input-name']").each(function () {
                if (arr.indexOf($(this).val()) > -1) {
                    $(this).attr("disabled", "disabled");
                    if (i === 0) {
                        arr_disabled = $(this).val();
                    }
                    else {
                        arr_disabled += "," + $(this).val();
                    }
                    i++;
                }
            });
            $("#" + idInput).attr("arr-disabled", arr_disabled);
        }
    };
    this.OnInit = function () {
        var _instance = this;
        _instance.disabled([], false);
        var optionsObj = this.options;
        var input = $("#" + idInput);
        input.keydown(false);
        input.attr("data-val", "");
        input.attr("arr-disabled", "");
        input.attr("placeholder", optionsObj.placeholder);
        input.css("cursor", "pointer");
        input.addClass("select-checkbox-input");

        var header_box = $("<div class='select-checkbox-header'></div>");
        header_box.css("background-color", "#e9ecef");
        header_box.css("padding", "5px");
        header_box.css("width", "100%");
        header_box.css("border-top-left-radius", "4px");
        header_box.html("<b style='font-weight: bold'>" + optionsObj.title + "</b>");
        var content_box = $("<div class='select-checkbox-content'></div>");
        content_box.css("width", "100%");
        content_box.css("height", "100%");

        var box = $("<div class='select-checkbox-box scrollable' id='" + _instance.id + "-select-checkbox'></div>");
        box.css("width", optionsObj.width_box);
        box.css("height", optionsObj.height_box);
        box.css("border", "1px solid rgba(0,0,0,0.2)");
        box.css("border-radius", "5px");
        box.css("position", "absolute");
        box.css("background-color", "#fff");
        box.css("display", "none");
        box.css("z-index", optionsObj.z_index.toString());
        box.append(header_box);
        box.append(content_box);
        input.after(box);

        var ul = $("<ul class='custom-control-list'></ul>");
        input.click(function (e) {
            if (!$("#" + idInput).attr("data-val")) {
                $("#" + idInput).attr("data-val", "");
            }
            $("input:checkbox[name='custom-control-input-name']").removeAttr("disabled");
            var arr_val_input = $("#" + idInput).attr("data-val").split(',');
            ul.html("");
            content_box.html(ul);
            var source_base = _instance.source_base;
            ul.css("padding", "0 15px");
            var ds_cha_co_con = _instance.source.where(n => n.so_luong_pt > 0);
            var ds_cha_khong_co_con = _instance.source.where(n => n.so_luong_pt == 0);
            for (var i = 0; i < ds_cha_co_con.length; i++) {
                var liGroup = $("<li><label class='font-weight-bold m-0'>" + ds_cha_co_con[i][optionsObj.dispay_name] + "</label></li>");
                liGroup.css("list-style-type", "none");
                ul.append(liGroup);
                var Items = ds_cha_co_con[i].children;
                for (var index_item in Items) {
                    if (typeof Items[index_item] !== 'object' || Items[index_item] === null) {
                        continue;
                    }
                    var li_gr = $("<li></li>");
                    li_gr.css("list-style-type", "none");
                    var div_checkbox_gr = $('<div class="custom-control custom-checkbox custom-control-inline"></div>');
                    var input_checkbox_gr;
                    if (arr_val_input !== undefined && arr_val_input !== null && arr_val_input.length > 0 && arr_val_input.includes(Items[index_item][optionsObj.value_name])) {
                        input_checkbox_gr = $('<input type="checkbox" data-group="' + ds_cha_co_con[i][optionsObj.value_name] + '" id="' + Items[index_item][optionsObj.value_name] + '" name="custom-control-input-name" class="custom-control-input" value="' + Items[index_item][optionsObj.value_name] + '" checked="checked">');
                    }
                    else {
                        input_checkbox_gr = $('<input type="checkbox" data-group="' + ds_cha_co_con[i][optionsObj.value_name] + '" id="' + Items[index_item][optionsObj.value_name] + '" name="custom-control-input-name" class="custom-control-input" value="' + Items[index_item][optionsObj.value_name] + '">');
                    }
                    var label_checkbox_gr = $('<label class="custom-control-label" for="' + Items[index_item][optionsObj.value_name] + '">' + Items[index_item][optionsObj.dispay_name] + '</label>');
                    div_checkbox_gr.append(input_checkbox_gr);
                    div_checkbox_gr.append(label_checkbox_gr);
                    li_gr.append(div_checkbox_gr);
                    ul.append(li_gr);
                }
            }
            if (ds_cha_khong_co_con.length > 0) {
                var liGroup = $("<li><label class='font-weight-bold m-0'>Khác</label></li>");
                liGroup.css("list-style-type", "none");
                ul.append(liGroup);
                for (var indexItem in ds_cha_khong_co_con) {
                    if (typeof ds_cha_khong_co_con[indexItem] !== 'object' || ds_cha_khong_co_con[indexItem] === null) {
                        continue;
                    }
                    var li = $("<li></li>");
                    li.css("list-style-type", "none");

                    var div_checkbox = $('<div class="custom-control custom-checkbox custom-control-inline"></div>');
                    var input_checkbox;
                    if (arr_val_input !== undefined && arr_val_input !== null && arr_val_input.length > 0 && arr_val_input.includes(ds_cha_khong_co_con[indexItem][optionsObj.value_name])) {
                        input_checkbox = $('<input type="checkbox" data-group="GR_KHAC" id="' + ds_cha_khong_co_con[indexItem][optionsObj.value_name] + '" name="custom-control-input-name" class="custom-control-input" value="' + ds_cha_khong_co_con[indexItem][optionsObj.value_name] + '" checked="checked">');
                    }
                    else {
                        input_checkbox = $('<input type="checkbox" data-group="GR_KHAC" id="' + ds_cha_khong_co_con[indexItem][optionsObj.value_name] + '" name="custom-control-input-name" class="custom-control-input" value="' + ds_cha_khong_co_con[indexItem][optionsObj.value_name] + '">');
                    }
                    var label_checkbox = $('<label class="custom-control-label" for="' + ds_cha_khong_co_con[indexItem][optionsObj.value_name] + '">' + ds_cha_khong_co_con[indexItem][optionsObj.dispay_name] + '</label>');
                    div_checkbox.append(input_checkbox);
                    div_checkbox.append(label_checkbox);
                    li.append(div_checkbox);
                    ul.append(li);
                }
            }
            content_box.html(ul);
            $("input:checkbox[name='custom-control-input-name']:checked").each(function () {
                var val = $(this).val();
                var group = $(this).attr("data-group");
                if (group != "GR_KHAC") {
                    $("input:checkbox[name='custom-control-input-name'][data-group='" + group + "']").attr("disabled", "disabled");
                    $(this).removeAttr("disabled");
                }
            });

            $("input[name='custom-control-input-name']").click(function () {
                var arr = [];
                var text = "";
                var arr_val = "";
                var count = 0;
                if (optionsObj.single_checked) {
                    $("input:checkbox[name='custom-control-input-name']").prop("checked", false);
                    var checked = $(this).is(":checked");
                    $(this).prop("checked", !checked);
                }
                $("input:checkbox[name='custom-control-input-name']").removeAttr("disabled");
                $("input:checkbox[name='custom-control-input-name']:checked").each(function () {
                    var val = $(this).val();
                    var group = $(this).attr("data-group");
                    var item = source_base.where(n => n[optionsObj.value_name] === val).firstOrDefault();
                    if (item != null) {
                        arr.push(item);
                        count++;
                        if (count < 4) {
                            if (text === "") {
                                text = item[optionsObj.dispay_name];
                                arr_val = item[optionsObj.value_name];
                            }
                            else {
                                text += ", " + item[optionsObj.dispay_name];
                                arr_val += "," + item[optionsObj.value_name];
                            }
                        }
                        else {
                            if (arr_val === "") {
                                arr_val = item[optionsObj.value_name];
                            }
                            else {
                                arr_val += "," + item[optionsObj.value_name];
                            }
                            text = "Có " + count + " sự lựa chọn";
                        }
                    }
                    if (group !="GR_KHAC") {
                        $("input:checkbox[name='custom-control-input-name'][data-group='" + group + "']").attr("disabled", "disabled");
                        $(this).removeAttr("disabled");
                    }
                });
                $("#" + idInput).val(text);
                $("#" + idInput).attr("data-val", arr_val);
                if (optionsObj.onChecked) {
                    optionsObj.onChecked(arr);
                }
            });

            box.slideToggle("fast");
        });
        $('body').mouseup(function (e) {
            var container = box;
            if (!container.is(e.target) && container.has(e.target).length === 0) {
                container.hide();
            }
        });

    }
    this.OnInit();
}