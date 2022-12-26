function BranchListService() {
    var _service = new Service();
    //Lấy tất cả danh sách chi nhánh quản lý
    this.layDsChiNhanh = function () {
        return _service.postData("/admin/branchlist/getall", { pm: "BT" });
    };
} 