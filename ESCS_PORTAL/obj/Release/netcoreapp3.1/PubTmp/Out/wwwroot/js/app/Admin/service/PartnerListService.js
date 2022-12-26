function PartnerListService() {
    var _service = new Service();
    //Lấy tất cả danh sách đối tác theo đối tác quản lý (có cache)
    this.layDsDoiTac = function () {
        return _service.postData("/admin/partnerlist/getall", {});
    };
} 