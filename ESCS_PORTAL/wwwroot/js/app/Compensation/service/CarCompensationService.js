function CarCompensationService() {
    var _service = new Service();
    //Tìm kiếm + phân trang
    this.getPaging = function (obj) {
        return _service.postData("/compensation/carcompensation/getPaging", obj);
    };
    //Liệt kê chi tiết
    this.getDetail = function (obj) {
        return _service.postData("/compensation/carcompensation/getDetail", obj);
    };

    //Lấy ảnh chi tiết
    this.getFiles = function (obj) {
        return _service.postData("/compensation/carcompensation/getFiles", obj);
    }
    //Lấy ảnh thumnail
    this.getFilesThumnail = function (obj) {
        return _service.postData("/compensation/carcompensation/getFilesThumnail", obj);
    }
}