var objDanhMuc = {};
var ho_so_chi_tiet = {};
const _multitabService = new MultitabService();
_multitabService.addTab('divThongTinTKiemHoSoNguoi');
_multitabService.addTab('divThongTinChungHoSoNguoi');
_multitabService.addTab('divThongTinTaiLieuHoSoNguoi');


const PAGE_TITLE = GetPageTitle().trim();

var collapseBoSung = null;
const ESCS_MA_DOI_TAC = $("#escs_ma_doi_tac").val();
const ESCS_MA_CHI_NHANH = $("#escs_ma_chi_nhanh").val();
const ESCS_NSD = $("#escs_tai_khoan").val();
const ESCS_LOAI_KH = $("#escs_loai_kh").val();
const GRID_HO_SO_SO_DONG = 12;
const ngayHienTai = new Date().ddmmyyyy();
const ngayDauMacDinh = new Date().getNgayDauThang();

const _service = new Service();
const _viewerService = new ViewerService('ESCS_view');
const _partnerListService = new PartnerListService();
const _branchListService = new BranchListService();
const _healthCompensationService = new HealthCompensationService();
//Form
var _frmTimKiem = new FormService('frmTimKiem');
var _frmTimKiemNDBH = new FormService("frmTimKiemNDBH");
var _frmThongTinLienHe = new FormService("frmThongTinLienHe");
//var _modalXemThongTinHoSo = new ModalService("modalXemThongTinHoSo");
var _modalMoHoSoBT = new ModalService("modalMoHoSoBT");
// Navtab
var _navTabTimKiemNguoi = new NavTabService("navTabTimKiemNguoi", ["tabTimKiem", "tabThongTinLienHe"], "quy-trinh");
var _tblHoSoConNguoi = new DatatableService('tblHoSoConNguoi', true, GRID_HO_SO_SO_DONG);

var _selectPhanTrang = new SelectService("selectPhanTrang", true, 'sm').createInstance();
function selectTrang() {
    getPaging(_selectPhanTrang.element.value);
}
_selectPhanTrang.onChange(selectTrang);

_tblHoSoConNguoi
    .addCol(1, 'sott', 'STT', 50, (cell) => { cell.classList.add('text-center') })
    .addCol(2, 'ngay_ht', 'NGÀY MỞ', 100, (cell) => { cell.classList.add('text-center') })
    .addCol(3, 'trang_thai_ten', 'TRẠNG THÁI', 200, (cell) => { cell.classList.add('text-center') })
    .addCol(4, 'so_hs', 'SỐ HỒ SƠ', 200, (cell, value) => { cell.classList.add('text-center'); if (value == '&nbsp;') return; cell.setAttribute('title', value) })
    .addCol(5, 'ten_trang_thai_hs_goc', 'TRẠNG THÁI HS GỐC', 150, (cell) => { cell.classList.add('text-center') })
    .addCol(6, 'ten', 'TÊN NGUỜI ĐƯỢC BẢO HIỂM', 200, (cell) => { cell.classList.add('text-center') })
    .addCol(7, 'ngay_sinh', 'NGÀY SINH', 100, (cell) => { cell.classList.add('text-center') })
    .addCol(8, 'so_tien_yc', 'SỐ TIỀN YÊU CẦU', 100, (cell, value) => { cell.classList.add('text-end'); if (value == '&nbsp;') return; cell.innerHTML = ESUtil.formatMoney(value) })
    .addCol(10, 'gcn', 'SỐ GCN', 150, (cell) => { cell.classList.add('text-center') })
    .addCol(11, 'ten_sp', 'SẢN PHẨM', 150, (cell) => { cell.classList.add('text-center') })
    .createInstance();
_tblHoSoConNguoi.keyColumn = 'sott';

_tblHoSoConNguoi.rowClick((data) => {
    var { row: clickedRow } = data;
    if (clickedRow[_tblHoSoConNguoi.keyColumn] == '&nbsp;') return;
    _tblHoSoConNguoi.element.querySelectorAll(`tr`).forEach(tr => tr.classList.remove('active-row'));
    _tblHoSoConNguoi.element.querySelector(`tr[data-mdb-index="${data.index}"]`).classList.add('active-row');
    
    var fullRowData = _tblHoSoConNguoi.rows.find(item => item[_tblHoSoConNguoi.keyColumn] === clickedRow[_tblHoSoConNguoi.keyColumn]);
    layThongTinHoSo(fullRowData, () => {
        let { so_hs } = fullRowData;
        if (so_hs.trim() == "" || so_hs.trim() == null) {
            so_hs = 'Hồ sơ chưa lấy số';
        }
        UpdatePageTitle(`<span class="d-none d-md-block">${PAGE_TITLE}:&nbsp;</span>${so_hs}`);
        _multitabService.showTabByIndex(1);
        setCollapse(true);
        const defaultFisrtTabs = document.querySelectorAll('.defaultFisrtTab');
        if (defaultFisrtTabs.length > 0) {
            defaultFisrtTabs.forEach(tab => {
                mdb.Tab.getInstance(tab).show();
            });
        }
    });
})
function layThongTinHoSo(data, callback = undefined) {
    if (data.so_id == undefined || data.so_id == null || data.so_id == 0 || data.so_id == "") {
        ShowAlert(`Không xác định được hồ sơ bồi thường`, '', '', 'danger');
        return;
    }
    data.loai_kh = ESCS_LOAI_KH;
    data.pm = 'TINH_TOAN';
    _healthCompensationService.LayToanBoThongTinHoSo(data).then(res => {
        ho_so_chi_tiet = res;
        const output = res.data_info;
        ESUtil.genHTML("tabThongTinChungContent_template", "tabThongTinChungContent", { data: output.ho_so });
        ESUtil.genHTML("giayToBoSung_body_template", "giayToBoSung_body", { data: output.hs });
        ESUtil.genHTML("tabThongTinGCNContent_template", "tabThongTinGCNContent", { data: output.gcn[0] });
        ESUtil.genHTML("tabThongTinQuyenLoiGCNContent_template", "tabThongTinQuyenLoiGCNContent", { data: output.gcn_ql });
        ESUtil.genHTML("tabThongTinHoSoGiayToContent_template", "tabThongTinHoSoGiayToContent", { data: output.ho_so_giay_to });
        ESUtil.genHTML("tabQuaTrinhXuLyContent_template", "tabQuaTrinhXuLyContent", { data: output.qua_trinh_xly });
        ESUtil.genHTML("tabThongTinYCBH_template", "tabThongTinYCBH", { lan_kham: output.lan_kham, lan_kham_qloi: output.lan_kham_qloi });
        ESUtil.genHTML("tabThongTinLichSuTonThat_template", "tabThongTinLichSuTonThat", { data: output.lich_su_ton_that });

        collapseBoSung = new mdb.Collapse(document.getElementById('collapseBoSung'));
        setTimeout(() => {
            collapseBoSung.hide();
            document.getElementById('collapseBoSung').classList.remove('d-none');
        }, 500);
        console.log(output.ho_so);
        loadAnhThumbnailHoSo(output.ho_so);
    });
    if (callback) {
        callback();
    }
}
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
function getPaging(trang) {
    const objTimKiem = _frmTimKiem.getJsonData();
    objTimKiem.trang = trang;
    objTimKiem.so_dong = GRID_HO_SO_SO_DONG;
    objTimKiem.loai_kh = ESCS_LOAI_KH;
    _healthCompensationService.getPaging(objTimKiem).then(res => {
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
            _tblHoSoConNguoi.rows = res.data_info.data;
            _tblHoSoConNguoi.options.loading = false;
            _tblHoSoConNguoi.update();
        }
        else {
            ShowAlert(response.state_info.message_body, '', '', 'danger');
        }
    });
}
function phanLoaiTaiLieu(data) {
    const arr_phanLoai = [];
    data.forEach(item => {
        if (arr_phanLoai.length == 0) {
            const nhom_moi = {
                id: `danhmuctailieu${item.ma_file}`,
                name: item.nhom_anh,
                data: []
            };
            nhom_moi.data.push(item);
            arr_phanLoai.push(nhom_moi);
        }
        else {
            const nhom = arr_phanLoai.find(x => x.id === `danhmuctailieu${item.ma_file}`);
            if (nhom === undefined) {
                const nhom_moi = {
                    id: `danhmuctailieu${item.ma_file}`,
                    name: item.nhom_anh,
                    data: []
                };
                nhom_moi.data.push(item);
                arr_phanLoai.push(nhom_moi);
            }
            else {
                nhom.data.push(item);
            }
        }
    });
    return arr_phanLoai;
}
function xemDanhSachHoSoBSCT() {
    collapseBoSung.toggle();
}
function loadAnhThumbnailHoSo(data, callback = undefined) {
    _viewerService.deleteAllData();
    document.getElementById('thumbnailContainer').innerHTML = '';
    document.getElementById('filterContainer').innerHTML = '';
    const checkboxFilters = document.getElementById('checkbox-filters');
    let checkboxFiltersInstance = Filters.getInstance(checkboxFilters);
    if (checkboxFiltersInstance !== null) checkboxFiltersInstance.dispose();
    $('#reset-filters').replaceWith($('#reset-filters').clone());
    data.pm = 'BH';
    data.loai_kh = ESCS_LOAI_KH;
    _healthCompensationService.getFilesThumnail(data).then(res => {
        if (res.data_info.length > 0) phanLoaiTaiLieu(res.data_info).forEach(loaitailieu => {
            const thumbnailEl = document.createElement('div');
            thumbnailEl.dataset.mdbDanhmuc = loaitailieu.id;
            thumbnailEl.classList.add('row', 'g-3', 'pt-3', 'filter-item');
            thumbnailEl.innerHTML = `<div class="col-12">
                                        <span class="fs-5 text-muted text-uppercase">${loaitailieu.name}</span>
                                    </div>`;
            const el = document.createElement('div');
            el.classList.add('col-12', 'gap-3', 'd-flex','flex-wrap');
            loaitailieu.data.forEach(thumbnail => {
                const anh = createImage(thumbnail);
                anh.classList.add('ESCS_thumbnail', 'p-0', 'border', 'border-info', 'rounded-5', 'overflow-hidden');
                const ma = thumbnail.bt;
                const danhmuc = loaitailieu.id;
                anh.addEventListener('click', () => {
                    switch (getFileType(thumbnail.extension)) {
                        case 'img':
                            _viewerService.createViewElement(danhmuc);
                            if (_viewerService.checkData(danhmuc, ma)) {
                                _viewerService.reqestImage(danhmuc, ma);
                            }
                            else {
                                thumbnail.pm = 'BH';
                                thumbnail.loai_kh = ESCS_LOAI_KH;
                                _healthCompensationService.getFiles(thumbnail).then(res => {
                                    const newImg = createImage(res.data_info);
                                    newImg.dataset.view = danhmuc;
                                    newImg.dataset.ma = ma;
                                    newImg.alt = res.data_info.ten_file;
                                    _viewerService.pushImage(danhmuc, newImg);
                                    _viewerService.reqestImage(danhmuc, ma);
                                });
                            }
                            break;
                        case 'pdf':
                            thumbnail.pm = 'BH';
                            thumbnail.loai_kh = ESCS_LOAI_KH;
                            _healthCompensationService.getFiles(thumbnail).then(res => {
                                const blob = base64toBlob(res.data_info.duong_dan);
                                const blobUrl = URL.createObjectURL(blob);
                                window.open(blobUrl, '_blank');
                            });
                            break;
                        case 'xls':
                            thumbnail.pm = 'BH';
                            thumbnail.loai_kh = ESCS_LOAI_KH;
                            _healthCompensationService.getFiles(thumbnail).then(res => {
                                const mime = res.data_info.extension == '.xlsx' ? 'vnd.openxmlformats-officedocument.spreadsheetml.sheet' : 'vnd.ms-excel';
                                const blob = base64toBlob(res.data_info.duong_dan, mime);
                                const blobUrl = URL.createObjectURL(blob);
                                window.open(blobUrl);
                            });
                            break;
                    }
                })
                el.appendChild(anh);
            });
            thumbnailEl.appendChild(el);
            document.getElementById('thumbnailContainer').appendChild(thumbnailEl);

            const filterEl = document.createElement('li');
            filterEl.classList.add('list-group-item');
            filterEl.innerHTML = `<input class="form-check-input" type="checkbox" value="${loaitailieu.id}" id="${loaitailieu.id}" /><label class="form-check-label d-inline" for="${loaitailieu.id}">${loaitailieu.name}</label>`;
            document.getElementById('filterContainer').appendChild(filterEl);

            checkboxFiltersInstance = new Filters(checkboxFilters);
            document.getElementById('reset-filters').addEventListener('click', () => {
                checkboxFiltersInstance.clear();
            });
        });
        if (callback) {
            callback(res);
        }
    });
}
function hienThiTabTKiem(tab) {
    var currentTab = _navTabTimKiemNguoi.currentTab;
    if (currentTab == "tabTimKiem") {
        $("#btnTiepTheo").trigger("click");
    } else {
        _navTabTimKiemNguoi.showTab(tab);
    }
}
$(document).ready(function () {
    _frmTimKiem.getControl("ngay_d").setValue(ngayDauMacDinh);
    _frmTimKiem.getControl("ngay_c").setValue(ngayHienTai);
    _service.all([
        _partnerListService.layDsDoiTac(),
        _branchListService.layDsChiNhanh()
    ]).then(arrRes => {
        objDanhMuc.ma_doi_tac = arrRes[0].data_info;
        objDanhMuc.ma_chi_nhanh = arrRes[1].data_info;
    });

    getPaging(1);
    $("#btnTimKiem").click(function () {
        getPaging(1);
    });
    //Tìm kiếm
    $("form[name='frmTimKiem'] input[name='ngay_d'],input[name='ngay_c'],input[name='so_hs'],input[name='ten_kh']").on('keyup', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13) {
            getPaging(1);
        }
    });
    updateLabel();
});