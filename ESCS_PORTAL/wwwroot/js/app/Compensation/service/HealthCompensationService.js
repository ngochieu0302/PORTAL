function HealthCompensationService() {
    var _service = new Service();
    //Tìm kiếm + phân trang
    this.getPaging = function (obj) {
        return _service.postData("/compensation/healthcompensation/getPaging", obj);
    };
    this.LayToanBoThongTinHoSo = function (obj) {
        return _service.postData("/compensation/healthcompensation/LayToanBoThongTinHoSo", obj);
    };

    //Lấy ảnh chi tiết
    this.getFiles = function (obj) {
        return _service.postData("/compensation/healthcompensation/getFiles", obj);
    }
    //Lấy ảnh thumnail
    this.getFilesThumnail = function (obj) {
        return _service.postData("/compensation/healthcompensation/getFilesThumnail", obj);
    }
}