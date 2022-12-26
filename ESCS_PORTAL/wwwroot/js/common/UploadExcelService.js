function UploadExcelService() {
    this.formData = null;
    this._commonService = new CommonService();
    this._frmImportExcel = new FormService("frmImportExcel");
    this.OnInit = function () {
    };

    this.getDataExcel = function(el) {
        var formData = this._frmImportExcel.getFormFileData();
        this._commonService.docFileExcel(formData).then(res => {
            var header = res[0];
            for (var key in header) {
                if (key != undefined) {
                    $('#table_import_excel thead tr').append(
                        `<th>${header[key]}</th>`
                    );
                }
            }

            var data = res.filter(n => n != res[0]);

            objData = data;
            for (var i = 0; i < data.length; i++) {
                var obj_row = data[i];
                var html_tr = $("<tr></tr>");
                for (var col_key in obj_row) {
                    if (col_key != undefined) {
                        var col_val = obj_row[col_key];
                        var html_td = $("<td class='text-center'></td>");
                        html_td.html(col_val);
                        html_tr.append(html_td);
                    }
                    $("#table_import_excel tbody#PreviewImportExcel").append(html_tr);
                }
            }
            this._frmImportExcel.getControl('file').val('');
        });
    }

    this.refresh = function() {
        $('#table_import_excel thead tr').html("");
        $('#table_import_excel tbody').html("");
        objData = null;
    }

    this.OnInit();
}