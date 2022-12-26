function PrintedService() {
    var _service = new Service();
    
    this.layDanhSachBieuMauBaoCao = function (obj) {
        return _service.postData("/admin/printed/layDanhSachBieuMauBaoCao", obj);
    }
    this.layDSCoSoYTe = function (obj) {
        return _service.postData("/admin/printed/layDSCoSoYTe", obj);
    }
    this.layDMChungNG = function (obj) {
        return _service.postData("/admin/printed/layDMChungNG", obj);
    }
    this.getAllProductHuman = function (obj) {
        return _service.postData("/admin/printed/getAllProductHuman", obj);
    }
    this.getControl = function (obj) {
        return _service.postData("/admin/printed/getControl", obj);
    }
    this.layDMChungXE = function (obj) {
        return _service.postData("/admin/printed/layDMChungXE", obj);
    }
    this.exportBaoCao = function (obj) {
        obj.ma_mau_in = obj.mau_bao_cao;
        return _service.postData("/admin/printed/exportBaoCao", obj);
    }
    // Lấy danh sách đơn vị hành chính
    this.layTatCaDonViHanhChinh = function (obj = {}) {
        var _service = new Service();
        return _service.postData("/admin/printed/layTatCaDonViHanhChinh", obj);
    };
    // Lấy danh sách ngân hàng
    this.layTatCaNganHang = function (obj = {}) {
        var _service = new Service();
        return _service.postData("/admin/printed/layTatCaNganHang", obj);
    };
} 