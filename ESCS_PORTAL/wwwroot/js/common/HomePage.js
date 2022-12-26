var objDanhMucHome = {};
var _modalCauHinhXe = new FormService("modalCauHinhXe");
var _modalMaLoi = new FormService("modalCauHinhMaLoiHoSo");
var _frmTimKiemHoSoDashboard = new FormService("frmTimKiemHoSoDashboard");
var _notifyService = new NotifyService();
var _systemConfiguration = new SystemConfiguration();
var _saveErrorCode = new SaveErrorCode();
var _homePageService = new HomePageService();
var _homePage = new HomePage();
var _serviceHomePage = new Service();
var _partnerListServiceNew = new PartnerListServiceNew();
var _modalCauHinhHeThong = new ModalService("modalCauHinhHeThong");
var _modalCauHinhError = new ModalService("modalCauHinhError");
var _modalDashBoardTimKiemHoSo = new ModalService("modalDashBoardTimKiemHoSo");
var ESCS_MA_DOI_TAC_HOME = $("#escs_ma_doi_tac").val();
var title_default = $("head title").attr("default");

function HomePage() {
    this.service = new HomePageService();
    this.notifyService = new NotifyService();
    this.buttons = {
        btnShowNotify: new ButtonService("btnShowNotify")
    }
    this.InitPage = function () {
        var _instance = this;
        _instance.getNotify();
        _instance.getContractNotify();
    }
    this.getNotify = function (page = 1, callback = undefined) {
        var _instance = this;
        _instance.service.layThongBaoNguoiDung({ ma_doi_tac: ESCS_MA_DOI_TAC_HOME, trang: page }, false).then(res => {

            $("head title").html(title_default);

            $("#app-notify-not-read").removeClass("point");
            if (res.state_info.status !== "OK") {
                _instance.notifyService.error(res.state_info.message_body);
                return;
            }
            if (res.data_info.data !== undefined && res.data_info.data !== null && res.data_info.data.length > 0) {
                res.trang = page;
                var tn_chua_doc = res.out_value.so_tb_chua_doc;
                if (tn_chua_doc > 0) {
                    $("#app-notify-not-read").addClass("point");
                    $("head title").html("(" + tn_chua_doc + ") " + title_default);
                }
                try { ESUtil.genHTML("app_notify_template", "app_notify", res); } catch (ex) { }
            }
            else {
                try { ESUtil.genHTML("app_notify_template", "app_notify", { data_info: [] }); } catch (ex) { }
            }

        });
    }
    this.getContractNotify = function (page = 1, callback = undefined) {
        var _instance = this;
        _instance.service.layThongBaoHoSoMoi({ trang: page }, false).then(res => {
            $("#app-notify-not-read-contract").removeClass("point");
            if (res.state_info.status !== "OK") {
                _instance.notifyService.error(res.state_info.message_body);
                return;
            }
            if (res.data_info.data !== undefined && res.data_info.data !== null && res.data_info.data.length > 0) {
                res.trang = page;
                var tn_chua_doc = res.out_value.so_tb_chua_doc;
                if (tn_chua_doc > 0) {
                    $("#app-notify-not-read-contract").addClass("point");
                    $("head title").html("(" + tn_chua_doc + ") " + title_default);
                }
                ESUtil.genHTML("app_notify_contract_template", "app_notify_contract", res);
            }
            else {
                ESUtil.genHTML("app_notify_contract_template", "app_notify_contract", { data_info: { data: [], tong_so_dong: 0 } });
            }

        });
    }
}
function readNotify(gid, ctiet_xem, ctiet_ma_doi_tac, ctiet_so_id, ctiet_hanh_dong) {
    var thong_bao = {
        gid: gid,
        ctiet_xem: ctiet_xem,
        ctiet_ma_doi_tac: ctiet_ma_doi_tac,
        ctiet_so_id: ctiet_so_id,
        ctiet_hanh_dong: ctiet_hanh_dong
    };
    var _notifyService = new NotifyService();
    var notify_url = _notifyService.getUrl(thong_bao);
    //Nếu có xem chi tiết thì chuyển hướng
    if (thong_bao.ctiet_xem == 1) {
        window.location.href = "/home/redirectnotify?gid=" + thong_bao.gid + "&ma_doi_tac=" + thong_bao.ctiet_ma_doi_tac + "&so_id=" + thong_bao.ctiet_so_id + "&hanh_dong=" + thong_bao.ctiet_hanh_dong + "&url_redirect=" + notify_url;
    }
    else {
        _homePageService.docNoiDungThongBao(thong_bao).then(res => {
            $("#notify-item-" + thong_bao.gid + " span .dot").addClass("active");
            $("#notify-item-" + thong_bao.gid + " div.d-inline-block p").removeClass("font-weight-bold");
            $("#notify-item-" + thong_bao.gid + " div.d-inline-block span").removeClass("font-weight-bold");
            if (res.out_value.so_tb_chua_doc > 0) {
                $("#app-notify-not-read").addClass("point");
                $("head title").html("(" + res.out_value.so_tb_chua_doc + ") ESCS - ESmart Claim Solution");
            }
            else {
                $("#app-notify-not-read").removeClass("point");
                $("head title").html("ESCS - ESmart Claim Solution");
            }
        });
    }
    //Nếu không xem chi tiết thì cập nhật trạng thái đã đọc thông báo
}
function SystemConfiguration() {
    //Lưu thông tin cấu hình xe
    this.luuThongTinCauHinhXe = function (obj) {
        return _serviceHomePage.postData("/common/savevehicleconfiguration", obj);
    };
}
function SaveErrorCode() {
    //Lưu thông tin mã lỗi hồ sơ
    this.luuThongTinMaLoiHoSo = function (obj) {
        return _serviceHomePage.postData("/common/saveerrorcode", obj);
    };
}
function PartnerListServiceNew() {
    //Lấy tất cả danh sách đối tác theo đối tác quản lý
    this.layDsDoiTac = function () {
        return _serviceHomePage.postData("/admin/partnerlist/getall", {});
    };
}
function xemTatCaNoiDungTinNhan() {
    _homePageService.docTatCaNoiDungThongBao().then(res => {
        $(".n-noi-dung-tin-nhan-dot").addClass("active");
        $("div.n-noi-dung-tin-nhan p").removeClass("font-weight-bold");
        $("div.n-noi-dung-tin-nhan span").removeClass("font-weight-bold");
        $("#app-notify-not-read").removeClass("point");
        $("head title").html("ESCS - ESmart Claim Solution");
    });
}
function xemThemNoiDungThongBao(page) {
    _homePage.service.layThongBaoNguoiDung({ ma_doi_tac: ESCS_MA_DOI_TAC_HOME, trang: page }, false).then(res => {
        $("head title").html("ESCS - ESmart Claim Solution");
        $("#app-notify-not-read").removeClass("point");
        if (res.state_info.status !== "OK") {
            _instance.notifyService.error(res.state_info.message_body);
            return;
        }
        if (res.data_info.data !== undefined && res.data_info.data !== null && res.data_info.data.length > 0) {
            res.trang = page;
            var tn_chua_doc = res.out_value.so_tb_chua_doc;
            if (tn_chua_doc > 0) {
                $("#app-notify-not-read").addClass("point");
                $("head title").html("(" + tn_chua_doc + ") ESCS - ESmart Claim Solution");
            }
            $("#notify_xem_them").remove();
            ESUtil.appendHTML("app_notify_template", "app_notify", res);
        }
    });
}
function gotoInformation() {
    window.location.assign("/admin/UserInformation");
}
function getPagingTimKiemHoSoDashboard(trang, callback = undefined) {
     var objTimKiem = _frmTimKiemHoSoDashboard.getJsonData();
    objTimKiem.trang = trang;
    objTimKiem.so_dong = 8;
    _homePageService.timKiemHoSo(objTimKiem).then(res => {
        if (res.state_info.status !== "OK") {
            _notifyService.error(res.state_info.message_body);
            return;
        }
        if (res.data_info.data !== undefined && res.data_info.data !== null && res.data_info.data.length > 0) {
            ESUtil.genHTML("danhSachHoSoDashboardTemplate", "danhSachHoSoDashboard", { data: res.data_info.data }, () => {
                _modalDashBoardTimKiemHoSo.show();
            });
        } else {
            _notifyService.error("Không tìm thấy hồ sơ bồi thường");
            return;
        }
        $("#tableDanhSachHoSoDashboard_pagination").html(ESUtil.pagingHTML("getPagingTimKiemHoSoDashboard", objTimKiem.trang, res.data_info.tong_so_dong, objTimKiem.so_dong));
        if (callback) {
            callback(res);
        }
    });
}
function ShowInvestigationDisplay(ma_doi_tac, so_id, hanh_dong) {
    var data = {
        ma_doi_tac: ma_doi_tac,
        so_id: so_id,
        hanh_dong: hanh_dong
    };
    var notify_url = "/carclaim/carinvestigation";
    window.open("/Home/TransInvestigationDisplay?ma_doi_tac=" + data.ma_doi_tac + "&so_id=" + data.so_id + "&hanh_dong=" + data.hanh_dong + "&url_redirect=" + notify_url, '_self');
}
function ShowCompensationDisplay(ma_doi_tac, so_id, hanh_dong) {
    var data = {
        ma_doi_tac: ma_doi_tac,
        so_id: so_id,
        hanh_dong: hanh_dong
    };
    var notify_url = "/carclaim/carcompensation";
    window.open("/Home/TransCompensationDisplay?ma_doi_tac=" + data.ma_doi_tac + "&so_id=" + data.so_id + "&hanh_dong=" + data.hanh_dong + "&url_redirect=" + notify_url, '_self');
}
function ShowApprovedDisplay(ma_doi_tac, so_id, nv, lhnv, loai, bt, ten, hanh_dong) {
    var notify_url = "/manager/approved";
    window.open("/Home/TransApprovedDisplay?ma_doi_tac=" + ma_doi_tac + "&so_id=" + so_id + "&nv=" + nv + "&lhnv=" + lhnv + "&loai=" + loai + "&bt=" + bt + "&ten=" + ten + "&hanh_dong=" + hanh_dong +"&url_redirect=" + notify_url,'_self');
}
$(document).ready(function () {
    _homePage.InitPage();
    _partnerListServiceNew.layDsDoiTac().then(arrRes => {
        objDanhMucHome.doi_tac = arrRes;
        _modalCauHinhXe.getControl("ma_doi_tac").setDataSource(objDanhMucHome.doi_tac.data_info, "ten", "ma", "Chọn đối tác", "");
        _modalMaLoi.getControl("ma_doi_tac").setDataSource(objDanhMucHome.doi_tac.data_info, "ten", "ma", "Chọn đối tác", "");
        _frmTimKiemHoSoDashboard.getControl("ma_doi_tac").setDataSource(objDanhMucHome.doi_tac.data_info, "ten", "ma", "Chọn đối tác", "");
        _frmTimKiemHoSoDashboard.getControl("ma_doi_tac").setValue(ESCS_MA_DOI_TAC_HOME);
        _frmTimKiemHoSoDashboard.getControl("ma_doi_tac").readOnly();
    });

    //var device = (/android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini/i.test(navigator.userAgent.toLowerCase()));
    //if (device) {
    //    window.location.assign("/Home/DetectDevice");
    //    return;
    //}

    $("#CauHinhXe").click(function () {
        _modalCauHinhHeThong.show();
    });
    $("#CauHinhError").click(function () {
        _modalCauHinhError.show();
    });
    $("#btnLuu_CHHT").click(function () {
        if (_modalCauHinhXe.isValid()) {
            var formData = _modalCauHinhXe.getJsonData();
            _systemConfiguration.luuThongTinCauHinhXe(formData).then(res => {
                if (res.state_info.status === "OK") {
                    _notifyService.success("Lưu thông tin cấu hình hệ thống thành công.");
                    _modalCauHinhHeThong.hide();
                }
                else {
                    _notifyService.error(res.state_info.message_body);
                }
            });
        }
    });
    $("#btnLuu_CodeError").click(function () {
        if (_modalMaLoi.isValid()) {
            var formData = _modalMaLoi.getJsonData();
            _saveErrorCode.luuThongTinMaLoiHoSo(formData).then(res => {
                if (res.state_info.status === "OK") {
                    _notifyService.success("Lưu thông tin mã lỗi thành công.");
                    _modalCauHinhError.hide();
                }
                else {
                    _notifyService.error(res.state_info.message_body);
                }
            });
        }
    });
    $('#btnThemMoi').bind('click', function () {
        _modalCauHinhXe.resetForm();
        _modalCauHinhXe.clearErrorMessage();
    });
    $('#btnNew').bind('click', function () {
        _modalMaLoi.resetForm();
        _modalMaLoi.clearErrorMessage();
    });
    if ($('#userHover img').attr('src') == '/') {
        $(".dropdown-user img").attr("src", "/images/default.png");
    }
    var url_logo = $("#layoutpage-logo").attr("src");
    ESUtil.checkLoadImage(url_logo, null, () => {
        $("#layoutpage-logo").attr("src", "/images/logo-light-icon.png");
    });

    $("#frmThongTinTimKiemHoSo").keydown(function (event) {
        var code = event.keyCode || event.which;
        if (code === 13) {
            event.preventDefault();
            $("#btnTimKiemHoSo_Dashboard").click();
        }
    });
    $("#timKiemNhanhHoSo").keydown(function (event) {
        var code = event.keyCode || event.which;
        if (code === 13) {
            event.preventDefault();
            $("#btnSearchHoSo_Dashboard").click();
        }
    });
    $("#btnTimKiemHoSo_Dashboard").click(function () {
        var obj = $("#frmThongTinTimKiemHoSo").getValue().trim();
        if (obj.trim() == null || obj.trim() == "" || obj.trim() == undefined) {
            _notifyService.error("Bạn chưa nhập thông tin tìm kiếm");
            return;
        }
        _frmTimKiemHoSoDashboard.getControl("tim").setValue(obj);
        getPagingTimKiemHoSoDashboard(1);
    });
    $("#btnSearchHoSo_Dashboard").click(function () {
        getPagingTimKiemHoSoDashboard(1);
    });

});
