function ModalXemQRCode() {
    this.modalId = "modalXemQRCode";
    this.data = null;
    this.frmXemQRCode = new FormService("frmXemQRCode");
    this.xemFile = function (isShow = true) {
        var _notifyService = new NotifyService();
        var _instance = this;
        if (_instance.data == undefined || _instance.data == null ||
            _instance.data.so_id == undefined || _instance.data.so_id == null || _instance.data.so_id == "" || _instance.data.so_id == "0" ||
            _instance.data.nv == undefined || _instance.data.nv == null || _instance.data.nv == "" ||
            _instance.data.loai == undefined || _instance.data.loai == null || _instance.data.loai == "") {
            _notifyService.error("Thiếu thông tin để xem file QRCode");
            return;
        }
        _instance.frmXemQRCode.getControl("loai").setValue(_instance.data.loai);
        var _service = new Service();
        _service.postData("/Common/GetQRCode", _instance.data).then(res => {
            if (res.state_info.status !== "OK") {
                _notifyService.error(res.state_info.message_body);
                return;
            }
            $("#modalXemQRCodeImage").addClass("d-none");
            if (res.data_info.file_base64 != null && res.data_info.file_base64 != "") {
                $("#modalXemQRCodeImage").removeClass("d-none");
                var src = "data:image/jpeg;base64, " + res.data_info.file_base64;
                $("#modalXemQRCodeImage").attr("src", src);
            }
            if (isShow) {
                $('#' + _instance.modalId).modal('show');
            }
        });
    }
    this.hide = function () {
        $('#' + this.modalId).removeClass("in");
        $('#' + this.modalId).css("display", "none");
        $('.modal-backdrop').remove();
        $('#' + this.modalId).modal('hide');
    };
    this.OnInit = function () {
        var _instance = this;
        _instance.frmXemQRCode.getControl("loai").addEventChange(val => {
            _instance.data.loai = val;
            _instance.xemFile(false);
        });
    }
    this.OnInit();
}
