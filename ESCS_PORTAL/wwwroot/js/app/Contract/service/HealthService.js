function HealthService() {
    var _service = new Service();
    //Tìm kiếm + phân trang
    this.getPaging = function (obj) {
        return _service.postData("/contract/health/getPaging", obj);
    };
    //Liệt kê danh hồ sơ khách hàng
    this.getDetail = function (obj) {
        return _service.postData("/contract/health/getDetail", obj);
    }
    //Lấy ảnh chi tiết
    this.getFiles = function (obj) {
        return _service.postData("/contract/health/getFiles", obj);
    }
    //Lấy ảnh thumnail
    this.getFilesThumnail = function (obj) {
        return _service.postData("/contract/health/getFilesThumnail", obj);
    }
    //Lấy quyền lợi gói bảo hiểm GCN
    this.layQuyenLoiGoiBaoHiem = function (obj) {
        return _service.postData("/contract/health/layQuyenLoiGoiBaoHiem", obj);
    }
    //Lấy danh sách đồng tái của hồ sơ
    this.layDanhSachDongTai = function (obj) {
        return _service.postData("/contract/health/layDanhSachDongTai", obj);
    }
    //Lấy chi tiết đồng tái của hồ sơ
    this.layChiTietDongTai = function (obj) {
        return _service.postData("/contract/health/layChiTietDongTai", obj);
    }
    //Lấy danh sách nhà bảo hiểm
    this.layDanhSachNhaBaoHiem = function (obj) {
        return _service.postData("/contract/health/layDanhSachNhaBaoHiem", obj);
    }
    //Lấy danh sách hợp đồng
    this.layDanhSachHopDong = function (obj) {
        return _service.postData("/contract/health/layDanhSachHopDong", obj);
    }
    // Xem thông tin chi tiết GCN
    this.layThongTinGCN = function (obj) {
        return _service.postData("/contract/health/layThongTinGCN", obj);
    }
    // Mở hồ sơ bồi thường
    this.moHoSoBT = function (obj) {
        return _service.postData("/contract/health/moHoSoBT", obj);
    }
}