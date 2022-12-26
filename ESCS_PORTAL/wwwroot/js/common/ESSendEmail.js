function ESSendEmail() {
    this.ho_so = { ma_doi_tac:"", so_id:"", ma:"", nv: "", ma_doi_tac_ql:""};
    this.modalId = "#modalSendEmailCommon";
    this.formName = "frmSendEmailCommon";
    this.frmSendEmailCommon = null;
    this.danhSachTemplate = null;
    this.data_mail = null;
    this.data_chung = { ds_file: [] };
    this.OnInit = function () {
        $('#frmSendEmailCommon_noi_dung').jqxEditor({
            height: "280px",
            width: '100%',
            theme: null,
            disabled: true,
            editable: true,
            tools: 'datetime | print | clear | backcolor | font | bold italic underline'
        });
        $("#btnFrmSendEmailZaloCommonSubmit").addClass("d-none");
        this.frmSendEmailCommon = new FormService("frmSendEmailCommon");
        var _instance = this;
        var _form = this.frmSendEmailCommon;
        this.frmSendEmailCommon.getControl("loai").addEventChange(val => {
            if (val === "") {
                return;
            }
            _instance.ho_so.ma = val;
            var _commonService = new CommonService();
            _commonService.layNoiDungEmail(_instance.ho_so).then(res => {
                if (res !== undefined && res !== null && res.state_info.status === "NotOK") {
                    return;
                }
                this.data_mail = res.data_info;
                if (res.data_info.nguoi_nhan !== null) {
                    _instance.frmSendEmailCommon.getControl("key").val(res.data_info.key);
                    _instance.frmSendEmailCommon.getControl("nguoi_nhan").val(res.data_info.nguoi_nhan.email);
                    _instance.frmSendEmailCommon.getControl("cc").val(res.data_info.nguoi_nhan.cc);
                    _instance.frmSendEmailCommon.getControl("bcc").val(res.data_info.nguoi_nhan.bcc);
                    _instance.frmSendEmailCommon.getControl("tieu_de").val(res.data_info.nguoi_nhan.tieu_de);
                }
                _instance.frmSendEmailCommon.getControl("template").val(res.data_info.template);
                $("#frmSendEmailCommon_noi_dung").jqxEditor('val', res.data_info.template);
                
                if (_instance.ho_so.loai!="") {
                    $(_instance.modalId).modal("show");
                }
            });
        });
        $("#btnFrmSendEmailCommonSubmit").click(function () {
            var _service = new Service();
            var _notifyService = new NotifyService();
            var jsonData = {};
            if (!_form.isValid()) {
                return;
            }
            jsonData.ma_doi_tac = _instance.ho_so.ma_doi_tac;
            jsonData.so_id = _instance.ho_so.so_id;
            jsonData.nv = _instance.ho_so.nv;
            jsonData.loai = _instance.frmSendEmailCommon.getControl("loai").val();
            jsonData.email_nhan = _form.getControl("nguoi_nhan").val();
            jsonData.email_cc = _form.getControl("cc").val();
            jsonData.email_bcc = _form.getControl("bcc").val();
            jsonData.tieu_de_mail = _form.getControl("tieu_de").val();
            jsonData.file_id = $("#modalSendEmailCommonDinhKemFile").attr("data-file-id");
            jsonData.file_name = $("#modalSendEmailCommonDinhKemFile").attr("data-file-name");

            _notifyService.confirm("Bạn có chắc chắn muốn gửi email này không?", "", () => {
                _service.postData("/common/sendmail_v2", jsonData).then(res => {
                    if (res.state_info.status != 'OK') {
                        _notifyService.error("Có lỗi trong quá trình gửi email");
                        return;
                    }
                    _notifyService.success("Gửi email thành công");
                });
            });
        });
        $("#btnFrmSendEmailZaloCommonSubmit").click(function () {
            var _service = new Service();
            var _notifyService = new NotifyService();
            var jsonData = {};
            if (!_form.isValid()) {
                return;
            }
            jsonData.ma_doi_tac = _instance.ho_so.ma_doi_tac;
            jsonData.so_id = _instance.ho_so.so_id;
            jsonData.nv = _instance.ho_so.nv;
            jsonData.loai = _instance.frmSendEmailCommon.getControl("loai").val();
            jsonData.email_nhan = _form.getControl("nguoi_nhan").val();
            jsonData.email_cc = _form.getControl("cc").val();
            jsonData.email_bcc = _form.getControl("bcc").val();
            jsonData.tieu_de_mail = _form.getControl("tieu_de").val();
            _notifyService.confirm("Bạn có chắc chắn muốn gửi email và zalo không?", "", () => {
                _service.postData("/common/sendEmailZalo", jsonData).then(res => {
                    if (res.state_info.status != 'OK') {
                        _notifyService.error("Có lỗi trong quá trình gửi email");
                        return;
                    }
                    _notifyService.success("Gửi email thành công");
                });
            });
        });
        $("#modalSendEmailCommonDinhKemFile").click(function () {
            var file_id = $("#modalSendEmailCommonDinhKemFile").attr("data-file-id");
            var file_name = $("#modalSendEmailCommonDinhKemFile").attr("data-file-name");
            if (!file_id) {
                file_id = "";
                file_name = "";
            }
            var arrFileId = file_id.split(",").where(n => n != "");
            var arrFileName = file_name.split(",").where(n => n != "");
            ESUtil.genHTML("modalSendEmailCommonFileAttachTableTemplate", "modalSendEmailCommonFileAttachTable", { data: [] });
            if (arrFileId.length > 0) {
                var arr = [];
                for (var i = 0; i < arrFileId.length; i++) {
                    var obj = { id: "", ten: "" };
                    if (arrFileId[i])
                        obj.id = arrFileId[i];
                    if (arrFileName[i])
                        obj.ten = arrFileName[i];
                    arr.push(obj)
                }
                ESUtil.genHTML("modalSendEmailCommonFileAttachTableTemplate", "modalSendEmailCommonFileAttachTable", { data: arr });
            }
            $("#modalSendEmailCommonFileAttach").modal('show');
        });
        $("#modalSendEmailCommonFileAttachAdd").click(function () {
            if (_uploadService) {
                _uploadService.setParam({
                    ma_doi_tac: _instance.ho_so.ma_doi_tac,
                    so_id: _instance.ho_so.so_id,
                    url_api:"/upload/attachfileemail"
                });
                _uploadService.showPupup();
            }
        });
    }
    this.show = function (ho_so = undefined) {
        if (ho_so != undefined) {
            this.ho_so = ho_so;
        }
        $("#modalSendEmailCommonDinhKemFile").html("File đính kèm thêm(0)");
        $("#modalSendEmailCommonDinhKemFile").attr("data-file-id","");
        $("#modalSendEmailCommonDinhKemFile").attr("data-file-name", "");
        var _instance = this;
        var loai = _instance.ho_so.loai || "";
        _instance.frmSendEmailCommon.resetForm();
        if (_instance.danhSachTemplate === null) {
            var _commonService = new CommonService();
            var _service = new Service();
            _service.all([
                _commonService.layDsTemplateEmail(_instance.ho_so),
                _commonService.layDsFileDinhKem(_instance.ho_so)
            ]).then(arrRes => {
                if (arrRes[0] !== undefined && arrRes[0] !== null && arrRes[0].state_info.status === "NotOK") {
                    return;
                }
                _instance.frmSendEmailCommon.getControl("loai").setDataSource(arrRes[0].data_info, "ten", "ma", "Chọn loại thông báo", _instance.ho_so.loai);
                _instance.frmSendEmailCommon.getControl("loai").trigger("select2:select");
                _instance.data_chung.ds_file = arrRes[1].data_info;
            });
        }
        else {
            _instance.frmSendEmailCommon.getControl("loai").setDataSource([], "ten", "ma", "Chọn loại thông báo", loai);
            _instance.frmSendEmailCommon.getControl("loai").trigger("select2:select");
        }
        if (loai=="") {
            $(_instance.modalId).modal("show");
        }
    }
    this.OnInit();
}
function ESSendEmail_OnDeleteFile(idFile) {
    var file_id = $("#modalSendEmailCommonDinhKemFile").attr("data-file-id");
    var file_name = $("#modalSendEmailCommonDinhKemFile").attr("data-file-name");
    if (!file_id) {
        file_id = "";
        file_name = "";
    }
    var arrFileId = file_id.split(",").where(n => n != "");
    var arrFileName = file_name.split(",").where(n => n != "");
    if (arrFileId.length > 0) {
        var arr = [];
        for (var i = 0; i < arrFileId.length; i++) {
            var obj = { id: "", ten: "" };
            if (arrFileId[i])
                obj.id = arrFileId[i];
            if (arrFileName[i])
                obj.ten = arrFileName[i];
            arr.push(obj)
        }
        arr = arr.where(n => n.id != idFile);
        file_id = "";
        file_name = "";
        for (var i = 0; i < arr.length; i++) {
            if (i == 0) {
                file_id = arr[i].id;
                file_name = arr[i].ten;
            }
            else {
                file_id += ","+arr[i].id;
                file_name += "," +arr[i].ten;
            }
        }
        var so_file = arr.length;
        $("#modalSendEmailCommonDinhKemFile").html("File đính kèm thêm(" + so_file + ")");
        $("#modalSendEmailCommonDinhKemFile").attr("data-file-id", file_id);
        $("#modalSendEmailCommonDinhKemFile").attr("data-file-name", file_name);
        ESUtil.genHTML("modalSendEmailCommonFileAttachTableTemplate", "modalSendEmailCommonFileAttachTable", { data: arr });
    }
}