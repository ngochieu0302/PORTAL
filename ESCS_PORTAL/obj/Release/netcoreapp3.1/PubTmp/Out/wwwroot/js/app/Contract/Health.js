var objDanhMuc = {};
var hop_dong_chi_tiet = {};
const _multitabService = new MultitabService();
_multitabService.addTab('divThongTinTKiemHopDongNguoi');
_multitabService.addTab('divThongTinChungHopDongNguoi');
_multitabService.addTab('divThongTinGCNHopDongNguoi');
_multitabService.addTab('divThemHoSoNguoi');

const PAGE_TITLE = GetPageTitle().trim();
const ESCS_MA_DOI_TAC = $("#escs_ma_doi_tac").val();
const ESCS_MA_DOI_TAC_QL = $("#escs_ma_doi_tac_ql").val();
const ESCS_MA_CHI_NHANH = $("#escs_ma_chi_nhanh").val();
const ESCS_NSD = $("#escs_tai_khoan").val();
const ESCS_LOAI_KH = $("#escs_loai_kh").val();
const GRID_HO_SO_SO_DONG = 12;
const GRID_GCN_SO_DONG = window.innerWidth >= 768 ? 11 : 7;
const dateNow = new Date().ddmmyyyy();
const ngayDauThang = new Date().getNgayDauThang();

var _service = new Service();
var _partnerListService = new PartnerListService();
var _branchListService = new BranchListService();
var _printedService = new PrintedService();
var _healthService = new HealthService();

const _paginationService = new PaginationService(GRID_GCN_SO_DONG);
//Form
var _frmTimKiem = new FormService('frmTimKiem');
var _frmThongTinKhachHang = new FormService('frmThongTinKhachHang');
var _frmThongTinHopDong = new FormService('frmThongTinHopDong');
var _frmThongTinLienHe = new FormService('frmThongTinLienHe');
//Select
var _selectDoiTac = new SelectService("ma_doi_tac", true).getInstance().createInstance();
var _selectChiNhanh = new SelectService("ma_chi_nhanh", true).getInstance().createInstance();
var _selectPhanTrang = new SelectService("selectPhanTrang", true, 'sm').createInstance();
var _selectPhanTrangGCN = new SelectService("selectPhanTrangGCN", true, 'sm').createInstance();
var _selectNhomNguyenNhan = new SelectService("nhom_nguyen_nhan", true).getInstance().createInstance();
var _selectHinhThuc = new SelectService("hinh_thuc", true).getInstance().createInstance();
var _selectTinhThanh = new SelectService("tinh_thanh", true).getInstance().createInstance();
var _selectBenhVien = new SelectService("benh_vien", true).getInstance().createInstance();
var _selectNganHangThuHuong = new SelectService("thu_huong_ngan_hang", true).getInstance().createInstance();
var _selectChiNhanhNganHangThuHuong = new SelectService("thu_huong_cn_ngan_hang", true).getInstance().createInstance();
function selectTrang() {
    getPaging(_selectPhanTrang.element.value);
}
_selectPhanTrang.onChange(selectTrang);

function loadPage(action = '') {
    const currentPage = _selectPhanTrang.element.value;
    const firstPage = _selectPhanTrang.element.dataset.trangDau;
    const lastPage = _selectPhanTrang.element.dataset.trangCuoi;
    if (action == 'next') {
        if (currentPage == lastPage) return;
        getPaging(currentPage - 0 + 1);
        return;
    }
    if (currentPage == firstPage) return;
    getPaging(currentPage - 1);
}
var _tblHopDongConNguoi = new DatatableService('tblHopDongConNguoi', true, GRID_HO_SO_SO_DONG);
_tblHopDongConNguoi
    .addCol(1, 'sott', 'STT', 20, (cell) => { cell.classList.add('text-center') })
    .addCol(2, 'ngay_text', 'NGÀY NHẬP', 80, (cell) => { cell.classList.add('text-center') })
    .addCol(3, 'ngay_cap', 'NGÀY CẤP', 90, (cell) => { cell.classList.add('text-center') })
    .addCol(4, 'kieu_hd_ten', 'KIỂU HĐ', 50, (cell) => { cell.classList.add('text-center') })
    .addCol(5, 'trang_thai_ten', 'TRẠNG THÁI', 100, (cell) => { cell.classList.add('text-center') })
    .addCol(6, 'ten_chi_nhanh', 'ĐƠN VỊ CẤP ĐƠN', 150, (cell) => { cell.classList.add('text-center') })
    .addCol(7, 'so_hd', 'SỐ HỢP ĐỒNG', 200, (cell, value) => { cell.classList.add('text-center'); if (value == '&nbsp;') return; cell.setAttribute('title', value) })
    .addCol(8, 'ten', 'TÊN KHÁCH', 200, (cell) => { cell.classList.add('text-center') })
    .addCol(9, 'so_luong_dt', 'SL NGƯỜI', 50, (cell) => { cell.classList.add('text-end') })
    .addCol(10, 'tong_phi', 'TỔNG PHÍ', 100, (cell, value) => { cell.classList.add('text-end'); if (value == '&nbsp;') return; cell.innerHTML = ESUtil.formatMoney(value) })
    .createInstance();
_tblHopDongConNguoi.keyColumn = 'sott';

_tblHopDongConNguoi.rowClick((data) => {
    var { row: clickedRow } = data;
    if (clickedRow[_tblHopDongConNguoi.keyColumn] == '&nbsp;') return;
    _tblHopDongConNguoi.element.querySelectorAll(`tr`).forEach(tr => tr.classList.remove('active-row'));
    _tblHopDongConNguoi.element.querySelector(`tr[data-mdb-index="${data.index}"]`).classList.add('active-row');
    
    var fullRowData = _tblHopDongConNguoi.rows.find(item => item[_tblHopDongConNguoi.keyColumn] === clickedRow[_tblHopDongConNguoi.keyColumn]);
    layDanhSachHopDong(fullRowData, () => {
        _multitabService.showTabByIndex(1);
    });
})
function getPaging(trang) {
    var objTimKiem = _frmTimKiem.getJsonData();
    objTimKiem.trang = trang;
    objTimKiem.so_dong = GRID_HO_SO_SO_DONG;
    objTimKiem.loai_kh = ESCS_LOAI_KH;
    _healthService.getPaging(objTimKiem).then(res => {
        if (res.state_info.status === "OK") {
            const pagiARR = [];
            const pageCount = Math.ceil(res.out_value.tong_so_dong / res.out_value.so_dong)
            for (let i = 1; i <= pageCount; i++) {
                pagiARR.push({ val: i, text: `Trang ${i}` });
            }
            $('#tong_so_dong').html(res.out_value.tong_so_dong);
            $('#tong_so_trang').html(`${trang}` +`/`+ `${pageCount}`);
            _selectPhanTrang.setDataSource(pagiARR, 'val', 'text');
            _selectPhanTrang.setValue(res.out_value.trang);
            _selectPhanTrang.element.dataset.trangDau = 1;
            _selectPhanTrang.element.dataset.trangCuoi = pageCount;
            _tblHopDongConNguoi.rows = res.data_info.data;
            _tblHopDongConNguoi.options.loading = false;
            _tblHopDongConNguoi.update();
        }
        else {
            ShowAlert(res.state_info.message_body, '', '', 'danger');
        }
    });
}
function layDanhSachHopDong(data, callback = undefined) {
    if (data.so_id == undefined || data.so_id == null || data.so_id == 0 || data.so_id == "") {
        ShowAlert(`Vui lòng chọn hợp đồng`, '', '', 'danger');
        return;
    }
    data.loai_kh = ESCS_LOAI_KH;
    _healthService.layDanhSachHopDong(data).then(res => {
        if (res.state_info.status === "OK") {
            hop_dong_chi_tiet.hd_sdbs = res;
            const dshd_show = res.data_info.map((hd, i) => {
                return {
                    ...hd,
                    select_text: i == res.data_info.length - 1 ? 'Hợp đồng gốc' : `Bổ sung lần ${res.data_info.length-1-i}`,
                };
            });

            ESUtil.genHTML("tblDachSachHopDongSDBS_template", "tblDachSachHopDongSDBS", { data: dshd_show.reverse() }, () => {
                if (dshd_show.length < 2) {
                    $('.prevHD').addClass('d-none');
                    $('.nextHD').addClass('d-none');
                }
                $("#tblDachSachHopDongSDBS .ESCS_HopDong")[0].click();
            });
        }
        else {
            ShowAlert(res.state_info.message_body, '', '', 'danger');
        }
    });
    if (callback) {
        callback();
    }
}
//Xem thông tin chi tiết của hợp đồng
function onChangeXemThongTinHopDong(ma_doi_tac, ma_doi_tac_ql, ma_chi_nhanh_ql, so_id_d, so_id) {
    if ($("#tblDachSachHopDongSDBS .ESCS_HopDong[data-hd='" + so_id + "']").hasClass("active")) return;
    $("#tblDachSachHopDongSDBS .ESCS_HopDong").removeClass("active");
    $("#tblDachSachHopDongSDBS .ESCS_HopDong[data-hd='" + so_id + "']").addClass("active");
    var obj = {
        ma_doi_tac: ma_doi_tac,
        ma_doi_tac_ql: ma_doi_tac_ql,
        ma_chi_nhanh_ql: ma_chi_nhanh_ql,
        so_id_d: so_id_d,
        so_id: so_id,
        loai_kh: ESCS_LOAI_KH
    }
    _healthService.getDetail(obj).then(res => {
        if (res.state_info.status === "OK") {
            UpdatePageTitle(`<span class="d-none d-md-block">${PAGE_TITLE}:&nbsp;</span>${res.data_info.hd.so_hd}`)
            hop_dong_chi_tiet.hd = res.data_info.hd;
            hop_dong_chi_tiet.gcn = res.data_info.gcn;
            if (res.data_info.hd != null) {
                ESUtil.genHTML("healthInfoContract_template", "healthInfoContract", { hd: res.data_info.hd });
            }
            $("#filterGCN").val('')
            _paginationService.deleteData();
            document.getElementById('tong_so_dongGCN').innerHTML = 0;
            document.getElementById('tong_so_trangGCN').innerHTML = 1;
            if (res.data_info.gcn != null) {
                phanTrangGCN(res.data_info.gcn)
                $("#filterGCN").keyup(ESUtil.delay(function (e) {
                    _service.triggerLoading();
                    var tim = ESUtil.xoaKhoangTrangText($(this).val().toLowerCase().trim());
                    if (tim == "") {
                        phanTrangGCN(res.data_info.gcn);
                    }
                    else {
                        phanTrangGCN(res.data_info.gcn.filter(gcn => gcn.nd_tim.toLowerCase().trim().includes(tim)));
                    }
                }, 500));
            }
        }
        else {
            ShowAlert(res.state_info.message_body, '', '', 'danger');
        }
    });
}
function checkChuyenHopDong(length, index) {
    return {
        prev: index > 0,
        next: index < (length - 1)
    };
}
function chuyenHopDong(action = '') {
    const arrLink = document.querySelectorAll("#tblDachSachHopDongSDBS .ESCS_HopDong");
    if (arrLink.length < 2) return;
    let active = -1;
    arrLink.forEach((link, index) => { if (link.classList.contains('active')) active = index; })
    if (action == 'next') {
        if (active + 1 >= arrLink.length) return;
        arrLink[active + 1].click();
        $('.prevHD').removeClass('text-muted');
        if (!checkChuyenHopDong(arrLink.length, active + 1).next) {
            $('.nextHD').addClass('text-muted');
        }
        else {
            $('.nextHD').removeClass('text-muted');
        }
        return;
    }
    if (active - 1 < 0) return;
    arrLink[active - 1].click();
    $('.nextHD').removeClass('text-muted');
    if (!checkChuyenHopDong(arrLink.length, active - 1).prev) {
        $('.prevHD').addClass('text-muted');
    }
    else {
        $('.prevHD').removeClass('text-muted');
    }
    return;
}
function phanTrangGCN(data) {
    _paginationService.deleteData();
    _paginationService.dataArr = data;
    const arrTrangGCN = [];
    for (let i = 1; i <= _paginationService.pageCount(); i++) {
        arrTrangGCN.push({ val: i, text: `Trang ${i}` });
    }
    _selectPhanTrangGCN.setDataSource(arrTrangGCN, 'val', 'text');
    _selectPhanTrangGCN.onChange(() => {
        const trang = _selectPhanTrangGCN.element.value;
        ESUtil.genHTML("dsGCN_body_template", "dsGCN_body", { gcn: _paginationService.getPage(trang), offset: (trang - 1) * _paginationService.soDong, max: GRID_GCN_SO_DONG });
    });
    document.getElementById('prevGCN').onclick = () => {
        if (_paginationService.canPrev()) {
            const page = _paginationService.currentPage - 1;
            ESUtil.genHTML("dsGCN_body_template", "dsGCN_body", { gcn: _paginationService.getPage(page), offset: (page - 1) * _paginationService.soDong, max: GRID_GCN_SO_DONG });
            _selectPhanTrangGCN.setValue(page);
        }
    }
    document.getElementById('nextGCN').onclick = () => {
        if (_paginationService.canNext()) {
            const page = _paginationService.currentPage + 1;
            ESUtil.genHTML("dsGCN_body_template", "dsGCN_body", { gcn: _paginationService.getPage(page), offset: (page - 1) * _paginationService.soDong, max: GRID_GCN_SO_DONG });
            _selectPhanTrangGCN.setValue(page);
        }
    }
    ESUtil.genHTML("dsGCN_body_template", "dsGCN_body", { gcn: _paginationService.getPage(1), offset: 0, max: GRID_GCN_SO_DONG });
    _selectPhanTrangGCN.setValue(1);

    $('#tong_so_dongGCN').html(_paginationService.dataArr.length);
    $('#tong_so_trangGCN').html(_paginationService.pageCount());
}
//Xem TT ndbh
function xemThongTinNDBH(so_id_dt) {
    $(".so_gcn").removeClass('active-row');
    $("#so_gcn_" + so_id_dt).addClass('active-row');
    const gcn = hop_dong_chi_tiet.gcn.find(item => item.so_id_dt == so_id_dt);
    gcn.loai_kh = ESCS_LOAI_KH;
    ESUtil.genHTML("chiTietNDBH_template", "chiTietNDBH", { gcn: gcn });
    _healthService.layQuyenLoiGoiBaoHiem(gcn).then(res => {
        var dsql = res.data_info;
        ESUtil.genHTML("tblQuyenLoiNDBH_body_template", "tblQuyenLoiNDBH_body", { ql: dsql });
    });
    _multitabService.showTabByIndex(2);
}
function set_ma_chi_nhanh() {
    _selectChiNhanh.setDataSource(objDanhMuc.ma_chi_nhanh.filter(item => item.ma_doi_tac === _selectDoiTac.element.value), 'ma', 'ten_tat', { ma: '', ten_tat: 'Chọn chi nhánh' });
}
function resetFormKhaiBaoHSBT() {
    $(".btn-tab-1").removeClass("d-none");
    $(".btn-tab-2").addClass("d-none");
    $(".btn-tab-3").addClass("d-none");
    // reset class active
    $("#khaiBaoHoSoBTTab1").addClass("divActived");
    $("#khaiBaoHoSoBTTab2").removeClass("divActived");
    $("#khaiBaoHoSoBTTab3").removeClass("divActived");
    // reset chức năng trong tab
    $("#khaiBaoHoSoBTTab1").find("input").prop("readonly", false);
    $("#khaiBaoHoSoBTTab2").find("input").prop("readonly", true);
    $("#khaiBaoHoSoBTTab3").find("input").prop("readonly", true);
    $("#khaiBaoHoSoBTTab1").find("select").prop("readonly", false);
    $("#khaiBaoHoSoBTTab2").find("select").prop("readonly", true);
    $("#khaiBaoHoSoBTTab3").find("select").prop("readonly", true);
}
function OnTiepTheo(tab) {
    $(".btn-tab-1").addClass("d-none");
    $(".btn-tab-2").addClass("d-none");
    $(".btn-tab-3").addClass("d-none");
    // xóa class active
    $("#khaiBaoHoSoBTTab1").removeClass("divActived");
    $("#khaiBaoHoSoBTTab2").removeClass("divActived");
    $("#khaiBaoHoSoBTTab3").removeClass("divActived");
    // disable chức năng trong tab
    $("#khaiBaoHoSoBTTab1").find("input").prop("readonly", true);
    $("#khaiBaoHoSoBTTab2").find("input").prop("readonly", true);
    $("#khaiBaoHoSoBTTab3").find("input").prop("readonly", true);
    $("#khaiBaoHoSoBTTab1").find("select").prop("readonly", true);
    $("#khaiBaoHoSoBTTab2").find("select").prop("readonly", true);
    $("#khaiBaoHoSoBTTab3").find("select").prop("readonly", true);
    if (tab == "tab_1") {
        $("#khaiBaoHoSoBTTab2").addClass("divActived");
        $("#khaiBaoHoSoBTTab2").find("input").prop("readonly", false);
        $("#khaiBaoHoSoBTTab2").find("select").prop("readonly", false);
        $(".btn-tab-2").removeClass("d-none");
    } else if (tab == "tab_2") {
        $("#khaiBaoHoSoBTTab3").addClass("divActived");
        $("#khaiBaoHoSoBTTab3").find("input").prop("readonly", false);
        $("#khaiBaoHoSoBTTab3").find("select").prop("readonly", false);
        $(".btn-tab-3").removeClass("d-none");
    } else if (tab == "tab_3") {
        $("#khaiBaoHoSoBTTab3").addClass("divActived");
        $("#khaiBaoHoSoBTTab3").find("input").prop("readonly", false);
        $("#khaiBaoHoSoBTTab3").find("select").prop("readonly", false);
        $(".btn-tab-3").removeClass("d-none");
        var obj = _frmThongTinLienHe.getJsonData();
        obj.ma_doi_tac_ql = hop_dong_chi_tiet.hd.ma_doi_tac_ql;
        obj.ma_chi_nhanh_ql = hop_dong_chi_tiet.hd.ma_chi_nhanh_ql;
        obj.so_id_hd_d = hop_dong_chi_tiet.hd.so_id_d;
        obj.so_id_hd = hop_dong_chi_tiet.hd.so_id;
        obj.so_id_dt = $("#chiTietNDBH").find("input[name=so_id_dt]").val();
        obj.nguon = "MOBILE";
        //if (!checkForm(obj)) return;
        _healthService.moHoSoBT(obj).then(res => {
            if (res.state_info.status !== 'OK') {
                ShowAlert(res.state_info.message_body, '', '', 'danger');
                return;
            }
            ShowAlert("Khai báo hồ sơ bồi thường thành công");
        });
    }
}
function OnQuayVe(tab) {
    $(".btn-tab-1").addClass("d-none");
    $(".btn-tab-2").addClass("d-none");
    $(".btn-tab-3").addClass("d-none");
    // xóa class active
    $("#khaiBaoHoSoBTTab1").removeClass("divActived");
    $("#khaiBaoHoSoBTTab2").removeClass("divActived");
    $("#khaiBaoHoSoBTTab3").removeClass("divActived");
    // disable chức năng trong tab
    $("#khaiBaoHoSoBTTab1").find("input").prop("readonly", true);
    $("#khaiBaoHoSoBTTab2").find("input").prop("readonly", true);
    $("#khaiBaoHoSoBTTab3").find("input").prop("readonly", true);
    $("#khaiBaoHoSoBTTab1").find("select").prop("readonly", true);
    $("#khaiBaoHoSoBTTab2").find("select").prop("readonly", true);
    $("#khaiBaoHoSoBTTab3").find("select").prop("readonly", true);
    if (tab == "tab_2") {
        $("#khaiBaoHoSoBTTab1").addClass("divActived");
        $("#khaiBaoHoSoBTTab1").find("input").prop("readonly", false);
        $("#khaiBaoHoSoBTTab1").find("select").prop("readonly", false);
        $(".btn-tab-1").removeClass("d-none");
    } else if (tab == "tab_3") {
        $("#khaiBaoHoSoBTTab2").addClass("divActived");
        $("#khaiBaoHoSoBTTab2").find("select").prop("readonly", false);
        $(".btn-tab-2").removeClass("d-none");
    }
}
//function checkForm(data) {
//    const arrLoi = [];
//    if (data.mau_bao_cao == '') arrLoi.push('Chưa chọn biểu mẫu');
//    if (data.ngay_tkiem == '' || data.ngayd == '' || data.ngayc == '') arrLoi.push('Sai hoặc thiếu thông tin ngày tìm kiếm');
//    if (arrLoi.length > 0) {
//        let msg = '';
//        arrLoi.forEach((loi, index) => {
//            if (index > 0) msg += '</br>';
//            msg += `${loi}`;
//        });
//        ShowAlert(msg, '', '', 'danger');
//        return false;
//    }
//    return true;
//}

$(document).ready(function () {
    _service.all([
        _partnerListService.layDsDoiTac(),
        _branchListService.layDsChiNhanh(),
        _printedService.layDMChungNG({ ma_doi_tac: ESCS_MA_DOI_TAC }),
        _printedService.layTatCaDonViHanhChinh(),
        _printedService.layDSCoSoYTe(),
        _printedService.layTatCaNganHang(),
    ]).then(arrRes => {
        objDanhMuc.ma_doi_tac = arrRes[0].data_info;
        objDanhMuc.ma_chi_nhanh = arrRes[1].data_info;
        objDanhMuc.nguyen_nhan = arrRes[2].data_info.where(n => n.nhom == "NGUYEN_NHAN");
        objDanhMuc.hinh_thuc = arrRes[2].data_info.where(n => n.nhom == "HINH_THUC");
        objDanhMuc.don_vi_hanh_chinh = arrRes[3].data_info.where(n => n.ma_quan.trim() === "" && n.ma_phuong.trim() === "");
        objDanhMuc.benh_vien = arrRes[4].data_info.benh_vien;
        objDanhMuc.ngan_hang = arrRes[5].data_info.ngan_hang;
        objDanhMuc.cn_ngan_hang = arrRes[5].data_info.cn_ngan_hang;

        _selectDoiTac.setDataSource(objDanhMuc.ma_doi_tac, 'ma', 'ten_tat', { ma: '', ten_tat: 'Chọn đối tác' }).onChange(set_ma_chi_nhanh);
        _selectNhomNguyenNhan.setDataSource(objDanhMuc.nguyen_nhan, 'ma', 'ten', { ma: '', ten: 'Chọn nguyên nhân' });
        _selectHinhThuc.setDataSource(objDanhMuc.hinh_thuc, 'ma', 'ten', { ma: '', ten: 'Chọn hình thức điều trị' });
        _selectTinhThanh.setDataSource(objDanhMuc.don_vi_hanh_chinh, 'ma_tinh', 'ten_tinh', { ma_tinh: '', ten_tinh: 'Chọn tỉnh thành' });
        _selectBenhVien.setDataSource([], 'ma', 'ten', { ma: '', ten: 'Chọn bệnh viện' });
        _selectNganHangThuHuong.setDataSource(objDanhMuc.ngan_hang, 'ma', 'ten', { ma: '', ten: 'Chọn ngân hàng' });
        _selectChiNhanhNganHangThuHuong.setDataSource([], 'ma', 'ten', { ma: '', ten: 'Chọn chi nhánh ngân hàng' });
        _frmTimKiem.getControl("ma_doi_tac").setValue(ESCS_MA_DOI_TAC_QL);
        set_ma_chi_nhanh();
    });
    _selectTinhThanh.onChange(() => {
        _selectBenhVien.setDataSource(objDanhMuc.benh_vien.filter(item => item.ma_doi_tac === hop_dong_chi_tiet.hd.ma_doi_tac && item.tinh_thanh == _selectTinhThanh.element.value), 'ma', 'ten', { ma: '', ten: 'Chọn bệnh viện' });
    });
    _selectNganHangThuHuong.onChange(() => {
        _selectChiNhanhNganHangThuHuong.setDataSource(objDanhMuc.cn_ngan_hang.filter(item => item.ma_ct == _selectNganHangThuHuong.element.value), 'ma', 'ten', { ma: '', ten: 'Chọn bệnh viện' });
    });
    _selectNhomNguyenNhan.onChange(() => {
        $(".tai_nan").addClass("d-none");
        var val = _selectNhomNguyenNhan.element.value;
        if (val == "NN001") {
            $(".tai_nan").removeClass("d-none");
        }
    });
    $("#btnTimKiem").click(function () {
        getPaging(1);
    });
    //Tìm kiếm
    $("form[name='frmTimKiem'] input[name='nd'],input[name='ten_ndbh']").on('keyup', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13 || e.keyCode === 9) {
            getPaging(1);
        }
    });
    _selectChiNhanh.onChange(() => {
        getPaging(1);
    })
    $("#btnMoHoSoBT").click(function () {
        _frmThongTinLienHe.resetForm();
        $("#chkThamGiaLienHe").prop("checked", false);
        $("#chkThamGiaThongBao").prop("checked", false);
        resetFormKhaiBaoHSBT();
    });
    $("#chkThamGiaLienHe").change(function () {
        _frmThongTinLienHe.getControl("nguoi_lh").readOnly(false);
        _frmThongTinLienHe.getControl("dthoai_nguoi_lh").readOnly(false);
        _frmThongTinLienHe.getControl("email_nguoi_lh").readOnly(false);
        if (_frmThongTinLienHe.getControlById("chkThamGiaLienHe").is(":checked")) {
            var customer = hop_dong_chi_tiet.gcn.where(n => n.so_id_dt == $("#chiTietNDBH").find("input[name=so_id_dt]").val()).firstOrDefault();
            _frmThongTinLienHe.getControl("nguoi_lh").val(customer.ten);
            _frmThongTinLienHe.getControl("dthoai_nguoi_lh").val(customer.d_thoai);
            _frmThongTinLienHe.getControl("email_nguoi_lh").val(customer.email);
            _frmThongTinLienHe.getControl("nguoi_lhla").val("BAN_THAN");
            
            _frmThongTinLienHe.getControl("nguoi_lh").readOnly();
            _frmThongTinLienHe.getControl("dthoai_nguoi_lh").readOnly();
            _frmThongTinLienHe.getControl("email_nguoi_lh").readOnly();
        }
    });
    $("#chkThamGiaThongBao").change(function () {
        _frmThongTinLienHe.getControl("nguoi_tb").readOnly(false);
        _frmThongTinLienHe.getControl("dthoai_nguoi_tb").readOnly(false);
        _frmThongTinLienHe.getControl("email_nguoi_tb").readOnly(false);
        if (_frmThongTinLienHe.getControlById("chkThamGiaThongBao").is(":checked")) {
            var customer = hop_dong_chi_tiet.gcn.where(n => n.so_id_dt == $("#chiTietNDBH").find("input[name=so_id_dt]").val()).firstOrDefault();
            _frmThongTinLienHe.getControl("nguoi_tb").val(customer.ten);
            _frmThongTinLienHe.getControl("dthoai_nguoi_tb").val(customer.d_thoai);
            _frmThongTinLienHe.getControl("email_nguoi_lh").val(customer.email);

            _frmThongTinLienHe.getControl("nguoi_tb").readOnly();
            _frmThongTinLienHe.getControl("dthoai_nguoi_tb").readOnly();
            _frmThongTinLienHe.getControl("email_nguoi_tb").readOnly();
        }
    });
    updateLabel();
    getPaging(1);
});