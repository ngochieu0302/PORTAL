function HomePageService() {
    var _service = new Service();
    this.base = new Service();
    //Lấy thông báo người sử dụng
    this.layThongBaoNguoiDung = function (obj,loading = true) {
        var _service = new Service(loading);
        return _service.postData("/home/getnotify", obj);
    };
    //Lấy thông báo hồ sơ mới
    this.layThongBaoHoSoMoi = function (obj, loading = true) {
        var _service = new Service(loading);
        return _service.postData("/home/getcontractnotify", obj);
    };
    this.layThongTinNguoiDung = function () {
        var _service = new Service();
        return _service.getData("/user/get");
    };
    this.docNoiDungThongBao = function (obj) {
        var _service = new Service(false,"body");
        return _service.postData("/home/readnotify", obj);
    };
    this.docTatCaNoiDungThongBao = function () {
        var _service = new Service(false, "body");
        return _service.postData("/home/readallnotify", {});
    };
    this.timKiemHoSo = function (obj, loading = true) {
        var _service = new Service(loading);
        return _service.postData("/home/timKiemHoSo", obj);
    };
}
$(document).ready(function () {
    //var _homePageService = new HomePageService();
    //_homePageService.layThongTinNguoiDung().then(res => {
    //    console.log("Người dùng",res);
    //});
});