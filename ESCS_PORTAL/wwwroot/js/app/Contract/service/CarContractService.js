function CarContractService() {
    var _service = new Service();
    //Tìm kiếm + phân trang
    this.getPaging = function (obj) {
        return _service.postData("/contract/carcontract/getPaging", obj);
    };
    //Liệt kê danh hồ sơ khách hàng
    this.getDetail = function (obj) {
        return _service.postData("/contract/carcontract/getDetail", obj);
    }
}