var objDanhMuc = {};
var hop_dong_chi_tiet = {};
const _multitabService = new MultitabService();
_multitabService.addTab('divThongTinTKiemHopDongXe');
_multitabService.addTab('divThongTinChungHopDongXe');
_multitabService.addTab('divThongTinGCNHopDongXe');

const PAGE_TITLE = GetPageTitle().trim();
const ESCS_MA_DOI_TAC = $("#escs_ma_doi_tac").val();
const ESCS_MA_DOI_TAC_QL = $("#escs_ma_doi_tac_ql").val();
const ESCS_MA_CHI_NHANH = $("#escs_ma_chi_nhanh").val();
const ESCS_NSD = $("#escs_tai_khoan").val();
const ESCS_LOAI_KH = $("#escs_loai_kh").val();
const GRID_HO_SO_SO_DONG = 12;
const GRID_GCN_SO_DONG = window.innerWidth >= 768 ? 11 : 7;
const dateNow = new Date().ddmmyyyy();

var _service = new Service();
var _partnerListService = new PartnerListService();
var _branchListService = new BranchListService();
var _carContractService = new CarContractService();
const _paginationService = new PaginationService(GRID_GCN_SO_DONG);
//Form
var _frmTimKiem = new FormService('frmTimKiem');
var _frmThongTinKhachHang = new FormService('frmThongTinKhachHang');
var _frmThongTinHopDong = new FormService('frmThongTinHopDong');
//Select
var _selectDoiTac = new SelectService("ma_doi_tac", true).getInstance().createInstance();
var _selectChiNhanh = new SelectService("ma_chi_nhanh", true).getInstance().createInstance();
var _selectHopDong = new SelectService("chon_hd", false).getInstance().createInstance();
var _selectPhanTrang = new SelectService("selectPhanTrang", true, 'sm').createInstance();
var _selectPhanTrangGCN = new SelectService("selectPhanTrangGCN", true, 'sm').createInstance();
function selectTrang() {
    getPaging(_selectPhanTrang.element.value);
}
_selectPhanTrang.onChange(selectTrang);

function loadPage(action = '') {
    let currentPage = _selectPhanTrang.element.value;
    let firstPage = _selectPhanTrang.element.dataset.trangDau;
    let lastPage = _selectPhanTrang.element.dataset.trangCuoi;
    if (action == 'next') {
        if (currentPage == lastPage) return;
        getPaging(currentPage - 0 + 1);
        return;
    }
    if (currentPage == firstPage) return;
    getPaging(currentPage - 1);
}

var _tblHopDongXeCoGioi = new DatatableService('tblHopDongXeCoGioi', true, GRID_HO_SO_SO_DONG);
_tblHopDongXeCoGioi
    .addCol(1, 'sott', 'STT', 50, (cell) => { cell.classList.add('text-center') })
    .addCol(2, 'ngay_cap_text', 'NGÀY CẤP', 100, (cell) => { cell.classList.add('text-center') })
    .addCol(3, 'trang_thai_ten', 'TRẠNG THÁI', 100, (cell) => { cell.classList.add('text-center') })
    .addCol(4, 'ten_chi_nhanh', 'ĐƠN VỊ CẤP ĐƠN', 150, (cell) => { cell.classList.add('text-center') })
    .addCol(5, 'so_hd', 'SỐ HỢP ĐỒNG', 150, (cell, value) => { cell.classList.add('text-center'); if (value == '&nbsp;') return; cell.setAttribute('title', value) })
    .addCol(6, 'ten', 'TÊN KHÁCH', 200, (cell) => { cell.classList.add('text-center') })
    .addCol(7, 'so_luong_dt', 'SL XE', 50, (cell) => { cell.classList.add('text-end') })
    .addCol(8, 'tong_phi', 'TỔNG PHÍ', 100, (cell, value) => { cell.classList.add('text-end'); if (value == '&nbsp;') return; cell.innerHTML = ESUtil.formatMoney(value) })
    .createInstance();
_tblHopDongXeCoGioi.keyColumn = 'sott';

function layDanhSachHopDong(data, callback = undefined) {
    if (data.so_id_hdong == undefined || data.so_id_hdong == null || data.so_id_hdong == 0 || data.so_id_hdong == "") {
        ShowAlert(`Vui lòng chọn hợp đồng`, '', '', 'danger');
        return;
    }
    data.loai_kh = ESCS_LOAI_KH;
    _carContractService.getDetail(data).then(res => {
        if (res.state_info.status === "OK") {
            hop_dong_chi_tiet.hd_sdbs = res;
            const dshd_show = res.data_info.hd_sdbs.map((hd, i) => {
                return {
                    ...hd,
                    select_text: i == res.data_info.hd_sdbs.length - 1 ? 'Hợp đồng gốc' : `Bổ sung lần ${res.data_info.hd_sdbs.length - 1 - i}`,
                };
            });
            ESUtil.genHTML("tblDachSachHopDongSDBS_template", "tblDachSachHopDongSDBS", { data: dshd_show.reverse(), hd: res.data_info.hd }, () => {
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

function onChangeXemThongTinHopDong(ma_doi_tac, ma_chi_nhanh, so_id, ma_duy_nhat) {
    if ($("#tblDachSachHopDongSDBS .ESCS_HopDong[data-hd='" + so_id + "']").hasClass("active")) return;
    $("#tblDachSachHopDongSDBS .ESCS_HopDong").removeClass("active");
    $("#tblDachSachHopDongSDBS .ESCS_HopDong[data-hd='" + so_id + "']").addClass("active");
    var obj = {
        ma_doi_tac: ma_doi_tac,
        ma_chi_nhanh: ma_chi_nhanh,
        so_id_hdong: so_id,
        loai_kh: ESCS_LOAI_KH,
    }
    _carContractService.getDetail(obj).then(res => {
        if (res.state_info.status === "OK") {
            UpdatePageTitle(`<span class="d-none d-md-block">${PAGE_TITLE}:&nbsp;</span>${res.data_info.hd.so_hd}`)
            hop_dong_chi_tiet.hd = res.data_info.hd;
            hop_dong_chi_tiet.gcn = res.data_info.gcn;
            hop_dong_chi_tiet.dk = res.data_info.dk;
            if (res.data_info.hd != null) {
                ESUtil.genHTML("carInfoContract_template", "carInfoContract", { hd: res.data_info.hd });
            }
            $("#filterGCN").val('')
            _paginationService.deleteData();
            $('#tong_so_dongGCN').html(0);
            $('#tong_so_trangGCN').html(1);
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
    })
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
function xemThongTinGCN(so_id_dt) {
    $(".so_gcn").removeClass('active-row');
    $("#so_gcn_" + so_id_dt).addClass('active-row');
    const gcn = hop_dong_chi_tiet.gcn.find(item => item.so_id_dt == so_id_dt);
    const dk = hop_dong_chi_tiet.dk.filter(item => item.so_id_dt == so_id_dt);
    ESUtil.genHTML("chiTietGCN_template", "chiTietGCN", { data: gcn });
    ESUtil.genHTML("tblDK_body_template", "tblDK_body", { data: dk });
    
    _multitabService.showTabByIndex(2);
}
_tblHopDongXeCoGioi.rowClick((data) => {
    var { row: clickedRow } = data;
    if (clickedRow[_tblHopDongXeCoGioi.keyColumn] == '&nbsp;') return;
    _tblHopDongXeCoGioi.element.querySelectorAll(`tr`).forEach(tr => tr.classList.remove('active-row'));
    _tblHopDongXeCoGioi.element.querySelector(`tr[data-mdb-index="${data.index}"]`).classList.add('active-row');
    var fullRowData = _tblHopDongXeCoGioi.rows.find(item => item[_tblHopDongXeCoGioi.keyColumn] === clickedRow[_tblHopDongXeCoGioi.keyColumn]);
    layDanhSachHopDong(fullRowData, () => {
        _multitabService.showTabByIndex(1);
    });
})
function getPaging(trang) {
    var objTimKiem = _frmTimKiem.getJsonData();
    objTimKiem.trang = trang;
    objTimKiem.so_dong = GRID_HO_SO_SO_DONG;
    objTimKiem.loai_kh = ESCS_LOAI_KH;
    _carContractService.getPaging(objTimKiem).then(res => {
        if (res.state_info.status === "OK") {
            var pagiARR = [];
            var pageCount = Math.ceil(res.out_value.tong_so_dong / res.out_value.so_dong)
            for (let i = 1; i <= pageCount; i++) {
                pagiARR.push({ val: i, text: `Trang ${i}` });
            }
            $('#tong_so_dong').html(res.out_value.tong_so_dong);
            $('#tong_so_trang').html(`${trang}` + `/` + `${pageCount}`);
            _selectPhanTrang.setDataSource(pagiARR, 'val', 'text');
            _selectPhanTrang.setValue(res.out_value.trang);
            _selectPhanTrang.element.dataset.trangDau = 1;
            _selectPhanTrang.element.dataset.trangCuoi = pageCount;
            _tblHopDongXeCoGioi.rows = res.data_info.data;
            _tblHopDongXeCoGioi.options.loading = false;
            _tblHopDongXeCoGioi.update();
        }
        else {
            ShowAlert(res.state_info.message_body, '', '', 'danger');
        }
    });
}
function set_ma_chi_nhanh() {
    _selectChiNhanh.setDataSource(objDanhMuc.ma_chi_nhanh.filter(item => item.ma_doi_tac === _selectDoiTac.element.value), 'ma', 'ten_tat', { ma: '', ten_tat: 'Chọn chi nhánh' });
}
$(document).ready(function () {
    _service.all([
        _partnerListService.layDsDoiTac(),
        _branchListService.layDsChiNhanh()
    ]).then(arrRes => {
        objDanhMuc.ma_doi_tac = arrRes[0].data_info;
        objDanhMuc.ma_chi_nhanh = arrRes[1].data_info;

        _selectDoiTac.setDataSource(objDanhMuc.ma_doi_tac, 'ma', 'ten_tat', { ma: '', ten_tat: 'Chọn đối tác' }).onChange(set_ma_chi_nhanh);
        _frmTimKiem.getControl("ma_doi_tac").setValue(ESCS_MA_DOI_TAC_QL);
        set_ma_chi_nhanh();
    });

    getPaging(1);
    $("#btnTimKiem").click(function () {
        getPaging(1);
    });
    updateLabel();
    //Tìm kiếm
    $("form[name='frmTimKiem'] input[name='bien_xe'],input[name='nd'],input[name='ten_kh']").on('keyup', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13) {
            getPaging(1);
        }
    });
    _selectChiNhanh.onChange(() => {
        getPaging(1);
    })
});