var _gridWidget = {
    dropDownEditor: null,
    create: function (data, tableId, configColumn = undefined, width, height, comboFunc, filter, funcGoPage) {
        var dataNew = null;
        try {
            dataNew = data.data_info.data;
        } catch (ex) {
            dataNew = data;
        };

        if (filter == undefined || filter == null || filter == '') {
            filter = false;
        }

        //var ComboSource = _gridWidget.getComboSource(dataList);

        var source =
        {
            datatype: "json",
            localdata: dataNew
        };
        var dataAdapter = new $.jqx.dataAdapter(source);

        $("#div-" + tableId).html('<div id="' + tableId + '" style="box-sizing:inherit"></div>');
        $("#" + tableId).jqxGrid(
            {
                theme: 'metro',
                width: width,//$('#' + tableId).parent().css('width'),
                height: height,
                editable: true,
                showfilterrow: filter,
                filterable: filter,
                columnsresize: false,
                autoheight: height == undefined || height == '' ? true : false,
                scrollbarsize: 6,
                source: dataAdapter,
                columnsresize: true,
                columns: _gridWidget.generalColum(configColumn, comboFunc),
                columngroups: [{
                    text: 'Name',
                    name: 'Name',
                    align: 'center'
                }]
            });

        if (funcGoPage != undefined && funcGoPage != null) {
            if ($('#' + tableId + 'PageNavigator').length == 0) {
                $('#' + tableId).after('<div id="' + tableId + 'PageNavigator"></div>');
            }
            $('#' + tableId + 'PageNavigator').html(_gridWidget.gridPaging(funcGoPage, data.out_value.trang, data.out_value.tong_so_dong, data.out_value.so_dong, ''));
        }
    },

    createTree: function (data, tableId, configColumn = undefined, width, height, dataList, keyID, keyParent) {
        var dataNew = null;
        try {
            dataNew = data.data_info.data;
        } catch (ex) {
            dataNew = data;
        };

        var ComboSource = _gridWidget.getComboSource(dataList);

        var source =
        {
            datatype: "json",
            localdata: dataNew,
            hierarchy:
            {
                keyDataField: { name: keyID },
                parentDataField: { name: keyParent }
            },
            id: keyID
        };
        var dataAdapter = new $.jqx.dataAdapter(source);

        $("#div-" + tableId).html('<div id="' + tableId + '" style="box-sizing:inherit"></div>');
        $("#" + tableId).jqxTreeGrid(
            {
                theme: 'metro',
                width: width,//$('#' + tableId).parent().css('width'),
                height: height,
                editable: true,
                scrollBarSize: 6,
                checkboxes: true,
                autoRowHeight: false,
                editSettings: {
                    saveOnPageChange: true, saveOnBlur: true, saveOnSelectionChange: true, cancelOnEsc: true,
                    saveOnEnter: true, editSingleCell: true, editOnDoubleClick: true, editOnF2: true
                },
                source: dataAdapter,
                columns: _gridWidget.generalColum(configColumn, ComboSource),
                ready: function () {
                    $("#" + tableId).jqxTreeGrid('expandAll');
                }
            });
    },

    //getComboSource: function (data) {
    //    if (data == undefined || data == null) return null;

    //    var arr_soucr = [];
    //    for (var i = 0; i < data.length; i++) {
    //        var temp =
    //        {
    //            datatype: "json",
    //            localdata: data[i]
    //        };
    //        arr_soucr.push(new $.jqx.dataAdapter(temp));
    //    }
    //    return arr_soucr;
    //},

    generalColum: function (configColumn, comboFunc) {
        var string = "[";
        var countComboBox = 0;
        for (var i = 0; i < configColumn.length; i++) {
            var editable = false;
            var disable = false;
            var cellsalign = 'left';
            var type = configColumn[i][3];
            if (type.indexOf('edit') != -1) {
                editable = true;
                type = type.replace('_edit', '');
            }

            if (type.indexOf('disable') != -1) {
                disable = true;
                type = type.replace('_disable', '');
            }

            if (type.indexOf('center') != -1) {
                cellsalign = 'center';
                type = type.replace('_center', '');
            }

            if (type.indexOf('right') != -1) {
                cellsalign = 'right';
                type = type.replace('_right', '');
            }

            string = string + "{ text: '" + configColumn[i][1] + "'" +
                ", editable : " + editable + ", cellsalign: '" + cellsalign + "', cellclassname: _gridWidget.cellclass(" + disable + "), datafield: '" + configColumn[i][0] + "'" +
                (type == 'int' ? ",  cellsalign: 'right',  cellsFormat: 'n', createeditor: function (row, column, editor) {numberFormatGrid(editor); }, cellvaluechanging: function (row, column, columntype, oldvalue, newvalue) { return _gridWidget.convertNumber(newvalue,'int'); }" : "") +
                (type == 'float' ? ",  cellsalign: 'right',  cellsFormat: 'f', createeditor: function (row, column, editor) {numberFormatGrid(editor); }, cellvaluechanging: function (row, column, columntype, oldvalue, newvalue) { return _gridWidget.convertNumber(newvalue,'float'); }" : "") +
                (type == 'checkbox' ? ", columntype: 'checkbox'" : "") +
                (type == 'hidden' ? ", hidden: true" : "") +
                (type == 'combobox' ? ", displayfield: '" + configColumn[i][0] + "1', columntype: 'dropdownlist', createeditor: function (row, column, editor) { _gridWidget.dropDownFunc(editor, comboFunc, '" + configColumn[i][0] + "'); }" : "") +
                ", width: '" + configColumn[i][2] + "' },";

            if (type.indexOf('combobox') != -1) {
                countComboBox++;
            }
        }
        string = string + "]";
        return eval(string);
    },

    convertNumber(number, type) {
        if (number == '') number = 0;
        if (type == 'int')
            var result = parseInt(number.toString().replace(/,/g, ''));
        else if (type == 'float')
            var result = parseFloat(number.toString().replace(/,/g, ''));
        return result;
    },

    dropDownEditor: null,

    dropDownFunc: function (editor, comboFunc, field) {
        editor.jqxDropDownList({
            autoDropDownHeight: true, //source: ComboSource,
            displayMember: 'ten',
            valueMember: 'ma',
            scrollBarSize: 6,
            //dropDownWidth: 250,
            autoOpen: true
        });
        editor.on('open', function (event) {
            //event.preventDefault();
            //if (_gridWidget.dropDownEditor != null && _gridWidget.dropDownEditor == editor) return;
            //_gridWidget.dropDownEditor = editor;
            var dataList = eval(comboFunc + "('" + field + "');");
            var temp =
            {
                datatype: "json",
                localdata: dataList
            };

            var ComboSource = new $.jqx.dataAdapter(temp);
            var source = editor.jqxDropDownList('source');
            if (JSON.stringify(source.loadedData) != JSON.stringify(ComboSource._source.localdata)) {
                editor.jqxDropDownList({ source: ComboSource });
                editor.jqxDropDownList('open');
            }
        });

        editor.on('close', function (event) {

        });
    },

    generalData: function (configColumn) {
        var string = "[";
        for (var i = 0; i < configColumn.length; i++) {
            string = string + "{ name: '" + configColumn[i][0] + "', type: '" + configColumn[i][3] + "' },";
        }
        string = string + "]";
        return eval(string);
    },

    cellclass: function (disable) {
        if (disable)
            return 'grid-disable';
        else
            return '';
    },

    gridPaging: function (funcGoPageName, currentPage, totalRow, rowOnPage, color) {
        var numberOfPage;
        var prePage;
        var nextPage;
        var strHTML = "";
        var strStyle = 'onmouseover = "this.style.cursor=\'pointer\'; this.style.textDecoration = \'none\'\ " onmouseout = "this.style.cursor = \'default\'"';
        var strStyle2 = " style=\" cursor: pointer; color: " + color + "; text-decoration: none\" " +
            " onmouseover=\"this.style.color=\'#0000ff\'; this.style.textDecoration = \'underline\'\" " +
            " onmouseout=\"this.style.color=\'" + color + "\'; this.style.textDecoration = \'none\'\"";

        if (typeof totalRow == typeof undefined || totalRow == 0 || totalRow == null)
            totalRow = 0;

        if (totalRow % rowOnPage != 0)
            numberOfPage = parseInt(totalRow / rowOnPage) + 1;
        else
            numberOfPage = parseInt(totalRow / rowOnPage);

        prePage = parseInt(currentPage) - 1;
        nextPage = parseInt(currentPage) + 1;

        if (numberOfPage == 0 || numberOfPage == 1)
            strHTML += "<span style=\"padding: 2px; font-size:11px;\"><b> " + totalRow + "</b> bản ghi";
        else
            strHTML += "<span style=\"padding: 2px; font-size:11px;\"><b>" + totalRow +
                "</b> bản ghi | <b>" + numberOfPage + "</b> trang &nbsp;&nbsp;";

        if (totalRow != 0 && numberOfPage != 1) {
            if (currentPage > 1) {
                strHTML += "<a style=\"font-weight:bold\" onclick=\"" + funcGoPageName + "(" + 1 + ")\" " + strStyle + ">" + "|< </a>";
                strHTML += "<a style=\"font-weight:bold\" onclick=\"" + funcGoPageName + "(" + prePage + ")\" " + strStyle + ">" + "<< </a>";
            }

            strHTML = strHTML + "<span style=\"padding: 2px; font-size:11px;\">Trang </span><select id=\"" + funcGoPageName + "_page\"" +
                "onchange=\"" + funcGoPageName + "(this.value)\" style=\"width: 45px; font-size:8pt\">\n";
            for (var i = 1; i <= numberOfPage; ++i) {
                if (i == currentPage)
                    strHTML = strHTML + "<option value=\"" + i.toString() + "\" selected=\"selected\">" + i + "</option>\n";
                else
                    strHTML = strHTML + "<option value=\"" + i.toString() + "\">" + i.toString() + "</option>\n";
            }
            strHTML = strHTML + "</select>\n";

            if (currentPage < numberOfPage) {
                strHTML += "<a style=\"font-weight:bold\" onclick=\"" + funcGoPageName + "(" + nextPage + ")\" " + strStyle + "> >> </a>";
                strHTML += "<a style=\"font-weight:bold\" onclick=\"" + funcGoPageName + "(" + numberOfPage + ")\" " + strStyle + "> >| </a>";
            }
        }

        strHTML = strHTML + "</span>";

        return strHTML;
    },

    initNullRow: function (id, row) {
        var textData = new Array();
        var cols = $("#" + id).jqxGrid("columns");
        for (var i = 0; i < cols.length; i++) {
            textData[i] = cols[i].datafield;
        }

        var new_row = [];
        for (var i = 0; i < row; i++) {

            var object = {};
            for (var j = 0; j < textData.length; j++) {
                eval('object.' + textData[j] + '=""');
            }
            new_row.push(object);
        }

        $("#" + id).jqxGrid('addrow', null, new_row);
    }
}

function numberFormatGrid(editor) {
    editor.css({ 'text-align': 'right', 'padding': '0px' });
    editor.val(ConvertCurrency(editor.val()));

    var ctrlDown = false;
    var ctrlKey = 17, vKey = 86, cKey = 67;

    editor.unbind('keydown');
    editor.bind('keydown', function (event) {
        if (event.keyCode == ctrlKey) ctrlDown = true;

        if ($(this).val().split('.').length > 1 && (event.keyCode == 110 || event.keyCode == 190)) {
            event.preventDefault();
        }

        if ($(this).val().split('.').length > 1 && $(this).val().split('.')[1].length == 8 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 8) {
            event.preventDefault();
        }

        if (ctrlDown && (event.keyCode == vKey || event.keyCode == cKey) || event.keyCode == ctrlKey && (event.keyCode == vKey || event.keyCode == cKey) || event.keyCode > 47 && event.keyCode < 58 || event.keyCode > 95 && event.keyCode < 106 || event.keyCode == 110 || event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 46 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 189 || event.keyCode == 109) {
            //alert(event.keyCode);
        } else {
            event.preventDefault();
        }
    });

    editor.unbind('keyup');
    editor.bind('keyup', function (event) {
        if (event.keyCode == ctrlKey) ctrlDown = false;

        if (event.keyCode > 95 && event.keyCode < 106 || event.keyCode > 47 && event.keyCode < 58 || event.keyCode == 110 || event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 46) {
            if ($(this).val().split('.').length == 1 && $(this).val() != '0') {
                $(this).val(ConvertCurrency($(this).val()));
            }
        }
    });
}

function ConvertCurrency(value) {
    var value = value.replace(/,/g, '');
    return value.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
}

var _indexTimkiem;

function findItemInGridCellMulti(values, gridName, arrcolumnCheck) {
    clearTimeout(my_setInterval);
    my_setInterval = setTimeout(function (values, gridName, arrcolumnCheck, event) {
        count_setInterval = 1;

        index_timkiem = _indexTimkiem;
        var arrcolumnCheck = arrcolumnCheck.split(',');
        var temp_timkiem = removeUnicode($.trim(values)).toUpperCase();
        if (gridName != '' && values != '') {
            var numrowscount = $('#' + gridName).jqxGrid('getdatainformation').rowscount;
            var x = event.keyCode;
            for (j = 0; j < arrcolumnCheck.length; j++) {
                var columnCheck = $.trim(arrcolumnCheck[j]);
                if (x == 13) {
                    for (var i = index_timkiem + 1; i < numrowscount; i++) {
                        if (findItemInGrid_temp(columnCheck, gridName, i, temp_timkiem))
                            return;
                    }
                    if (_indexTimkiem == index_timkiem) {
                        _indexTimkiem = 0;
                        for (var i = 0; i < numrowscount; i++) {
                            if (findItemInGrid_temp(columnCheck, gridName, i, temp_timkiem))
                                return;
                        }
                    }
                } else {
                    for (var i = 0; i < numrowscount; i++) {
                        if (findItemInGrid_temp(columnCheck, gridName, i, temp_timkiem))
                            return;
                    }
                }
            }
        }
    }, 400, values, gridName, arrcolumnCheck, event);
}

function findItemInGrid_temp(columnCheck, gridName, i, temp_timkiem) {
    datarow = $('#' + gridName).jqxGrid('getrowdata', i);
    selectionmode = $('#' + gridName).jqxGrid('selectionmode');
    var rowsheight = $('#' + gridName).jqxGrid('rowsheight');
    var values_check = eval('datarow.' + columnCheck);
    var data_temp_ten = removeUnicode($.trim(values_check)).toUpperCase();

    if (data_temp_ten != '' && data_temp_ten.indexOf(temp_timkiem) != -1) {

        if (selectionmode == 'singlecell') {
            $('#' + gridName).jqxGrid('selectcell', datarow.uid, columnCheck);
        } else if (selectionmode == 'singlerow') {
            $('#' + gridName).jqxGrid('selectrow', datarow.uid);
        }

        // tim kiem ko settimeout
        //------------------
        $('#' + gridName).jqxGrid('scrolloffset', datarow.uid * rowsheight - 44, 0);
        //------------------

        //tim kiem co settimeout
        //------------------
        //var position = $('#' + gridName).jqxGrid('scrollposition');
        //var top = position.top / ((datarow.uid * 22 - 44) / 100);
        //if (position.top < datarow.uid * 22 - 44) {
        //    count_setInterval = parseInt(top);
        //    my_setInterval = setInterval(function () {
        //        $('#' + gridName).jqxGrid('scrolloffset', datarow.uid * 2.2 / 10 * count_setInterval - 44, 0);

        //        count_setInterval++;

        //        if (count_setInterval == 100) {
        //            clearInterval(my_setInterval);
        //        }
        //    }, 5);
        //} else {
        //    $('#' + gridName).jqxGrid('scrolloffset', datarow.uid * 22 - 44, 0);
        //}
        //------------------

        _indexTimkiem = i;
        return true;
    } else {
        return false;
    }
}

function isValidDate(s) {
    var bits = s.split('/');
    var d = new Date(bits[2], bits[1] - 1, bits[0]);
    return d && (d.getMonth() + 1) == bits[1];
}