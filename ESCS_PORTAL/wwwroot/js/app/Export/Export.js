const objDanhMuc = {
    ngaytk: [
        {
            ma: 'NGAY_MO',
            ten: 'Tìm theo ngày mở hồ sơ'
        },
        {
            ma: 'NGAY_DONG',
            ten: 'Tìm theo ngày đóng hồ sơ'
        },
    ],
    loai_hinh_xe: [
        {
            ma: 'BB',
            ten: 'Bảo hiểm bắt buộc'
        },
        {
            ma: 'TN',
            ten: 'Bảo hiểm tự nguyện'
        },
    ],
    nguon_khai_bao: [
        {
            ma: 'CTCT',
            ten: 'Tổng đài'
        },
        {
            ma: 'MOBILE',
            ten: 'App mobile'
        },
        {
            ma: 'TTGD',
            ten: 'Trực tiếp'
        },
    ]
};
const ngayHienTai = new Date().ddmmyyyy();
const ngayDauMacDinh = new Date().getNgayDauThang();

const ESCS_MA_DOI_TAC = $("#escs_ma_doi_tac").val();
const ESCS_MA_DOI_TAC_QL = $("#escs_ma_doi_tac_ql").val();
const ESCS_MA_CHI_NHANH = $("#escs_ma_chi_nhanh").val();
const ESCS_NSD = $("#escs_tai_khoan").val();
const ESCS_LOAI_KH = $("#escs_loai_kh").val();

var _frmBaoCaoNG = new FormService('frmBaoCaoNG');
var _frmBaoCaoXE = new FormService('frmBaoCaoXE');

const _partnerListService = new PartnerListService();
const _branchListService = new BranchListService();
const _printedService = new PrintedService();
const _exportService = new ExportService();
const _service = new Service();

const _selectBieuMauNG = new SelectService("selectBieuMauNG", true).getInstance().createInstance();
const _selectBieuMauXE = new SelectService("selectBieuMauXE", true).getInstance().createInstance();

const _selectTieuChiNgayNG = new SelectService("selectTieuChiNgayNG", false).getInstance().createInstance();
const _selectTieuChiNgayXE = new SelectService("selectTieuChiNgayXE", false).getInstance().createInstance();

const _selectDoiTacNG = new SelectService("selectDoiTacNG", true).getInstance().createInstance();
const _selectDoiTacXE = new SelectService("selectDoiTacXE", true).getInstance().createInstance();
const _selectChiNhanhNG = new SelectService("selectChiNhanhNG", true).getInstance().createInstance();
const _selectChiNhanhXE = new SelectService("selectChiNhanhXE", true).getInstance().createInstance();

const _selectChonBenhVienNG = new SelectService("selectChonBenhVienNG", true).getInstance().createInstance();
const _selectLoaiHinhBaoHiemNG = new SelectService("selectLoaiHinhBaoHiemNG", true).getInstance().createInstance();
const _selectTrangThaiHoSoNG = new SelectService("selectTrangThaiHoSoNG", true).getInstance().createInstance();
const _selectNguyenNhanDieuTriNG = new SelectService("selectNguyenNhanDieuTriNG", true).getInstance().createInstance();
const _selectHinhThucDieuTriNG = new SelectService("selectHinhThucDieuTriNG", false).getInstance().createInstance();

const _selectLoaiHinhBaoHiemXE = new SelectService("selectLoaiHinhBaoHiemXE", false).getInstance().createInstance();
const _selectTrangThaiHoSoXE = new SelectService("selectTrangThaiHoSoXE", true).getInstance().createInstance();
const _selectNguyenNhanTaiNanXE = new SelectService("selectNguyenNhanTaiNanXE", true).getInstance().createInstance();
const _selectNguonKhaiBaoXE = new SelectService("selectNguonKhaiBaoXE", false).getInstance().createInstance();

function checkForm(data) {
    const arrLoi = [];
    if (data.mau_bao_cao == '') arrLoi.push('Chưa chọn biểu mẫu');
    if (data.ngay_tkiem == '' || data.ngayd == '' || data.ngayc == '') arrLoi.push('Sai hoặc thiếu thông tin ngày tìm kiếm');
    if (arrLoi.length > 0) {
        let msg = '';
        arrLoi.forEach((loi, index) => {
            if (index > 0) msg += '</br>';
            msg += `${loi}`;
        });
        ShowAlert(msg, '', '', 'danger');
        return false;
    }
    return true;
}
function xuatBCNG() {
    const data = _frmBaoCaoNG.getJsonData();
    data.ma_chi_nhanh = _selectChiNhanhNG.instance.value;
    console.log(data);
    if (!checkForm(data)) return;
    _printedService.exportBaoCao(data).then(res => {
        ESUtil.convertBase64ToFile(res,
            data.ma_mau_in +
            "_" +
            new Date().getFullYear() +
            new Date().getMonth() +
            new Date().getDay() +
            new Date().getHours() +
            new Date().getMinutes() +
            new Date().getSeconds() +
            new Date().getMilliseconds() +
            ".xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    })
}
function xuatBCXE() {
    const data = _frmBaoCaoXE.getJsonData();
    data.ma_chi_nhanh = _selectChiNhanhXe.instance.value;
    console.log(data);
    if (!checkForm(data)) return;
    _printedService.exportBaoCao(data).then(res => {
        ESUtil.convertBase64ToFile(res,
            data.ma_mau_in +
            "_" +
            new Date().getFullYear() +
            new Date().getMonth() +
            new Date().getDay() +
            new Date().getHours() +
            new Date().getMinutes() +
            new Date().getSeconds() +
            new Date().getMilliseconds() +
            ".xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    })
}

function setMaChiNhanhNG() {
    _selectChiNhanhNG.setDataSource(objDanhMuc.ma_chi_nhanh.filter(item => item.ma_doi_tac === _selectDoiTacNG.element.value), 'ma', 'ten_tat'/*, { ma: '', ten_tat: 'Chọn chi nhánh' }*/);
}
function setMaChiNhanhXE() {
    _selectChiNhanhXE.setDataSource(objDanhMuc.ma_chi_nhanh.filter(item => item.ma_doi_tac === _selectDoiTacXE.element.value), 'ma', 'ten_tat'/*, { ma: '', ten_tat: 'Chọn chi nhánh' }*/);
}
$(document).ready(function () {
    _frmBaoCaoNG.getControl('ngayd').setValue(ngayDauMacDinh);
    _frmBaoCaoXE.getControl('ngayd').setValue(ngayDauMacDinh);
    _frmBaoCaoNG.getControl('ngayc').setValue(ngayHienTai);
    _frmBaoCaoXE.getControl('ngayc').setValue(ngayHienTai);
    _service.all([
        _partnerListService.layDsDoiTac(),
        _branchListService.layDsChiNhanh(),
        _printedService.layDanhSachBieuMauBaoCao({ nv: "NG", pm: "BC_NG_PORTAL", loai_kh: ESCS_LOAI_KH }),
        _printedService.layDanhSachBieuMauBaoCao({ nv: "XE", pm: "BC_XE_PORTAL", loai_kh: ESCS_LOAI_KH }),
        _printedService.layDSCoSoYTe(),
        _printedService.layDMChungNG({ ma_doi_tac: ESCS_MA_DOI_TAC_QL }),
        _printedService.getAllProductHuman({ ma_doi_tac: ESCS_MA_DOI_TAC_QL }),
        _printedService.getControl(),
        _printedService.layDMChungXE(),
    ]).then(arrRes => {
        objDanhMuc.ma_doi_tac = arrRes[0].data_info;
        objDanhMuc.ma_chi_nhanh = arrRes[1].data_info;
        objDanhMuc.bieu_mau_ng = arrRes[2].data_info;
        objDanhMuc.bieu_mau_xe = arrRes[3].data_info;
        objDanhMuc.dscsyt = arrRes[4].data_info;
        objDanhMuc.dmcNG = arrRes[5].data_info;
        const dmcNguyenNhan = arrRes[5].data_info.filter(dm => dm.nhom === 'NGUYEN_NHAN');
        const dmcHinhThuc = arrRes[5].data_info.filter(dm => dm.nhom === 'HINH_THUC');
        objDanhMuc.spNG = arrRes[6].data_info;
        objDanhMuc.trang_thai = arrRes[7].data_info;
        const trangThaiNG = arrRes[7].data_info.filter(tt => tt.nv === 'NG');
        const trangThaiXE = arrRes[7].data_info.filter(tt => tt.nv === 'XE');
        objDanhMuc.dmcXE = arrRes[8].data_info;
        const dmcNhomNguyenNhan = arrRes[8].data_info.filter(dm => dm.nhom === 'NHOM_NGUYEN_NHAN');

        _selectTieuChiNgayNG.setDataSource(objDanhMuc.ngaytk, 'ma', 'ten', { ma: '', ten: 'Chọn tiêu chí ngày tìm kiếm' });
        _selectTieuChiNgayXE.setDataSource(objDanhMuc.ngaytk, 'ma', 'ten', { ma: '', ten: 'Chọn tiêu chí ngày tìm kiếm' });
        _selectLoaiHinhBaoHiemXE.setDataSource(objDanhMuc.loai_hinh_xe, 'ma', 'ten', { ma: '', ten: 'Chọn loại hình' });
        _selectNguonKhaiBaoXE.setDataSource(objDanhMuc.nguon_khai_bao, 'ma', 'ten', { ma: '', ten: 'Chọn nguồn khai báo' });

        _selectBieuMauNG.setDataSource(arrRes[2].data_info, 'ma', 'ten', { ma: '', ten: 'Chọn biểu mẫu' });
        _selectBieuMauXE.setDataSource(arrRes[3].data_info, 'ma', 'ten', { ma: '', ten: 'Chọn biểu mẫu' });
        _selectChonBenhVienNG.setDataSource(arrRes[4].data_info.benh_vien, 'ma', 'ten', { ma: '', ten: 'Chọn bệnh viện' });
        _selectLoaiHinhBaoHiemNG.setDataSource(arrRes[6].data_info, 'ma', 'ten', { ma: '', ten: 'Chọn sản phẩm' });
        _selectTrangThaiHoSoNG.setDataSource(trangThaiNG, 'ma_trang_thai', 'ten', { ma_trang_thai: '', ten: 'Chọn trạng thái' });
        _selectTrangThaiHoSoXE.setDataSource(trangThaiXE, 'ma_trang_thai', 'ten', { ma_trang_thai: '', ten: 'Chọn trạng thái' });
        _selectNguyenNhanDieuTriNG.setDataSource(dmcNguyenNhan, 'ma', 'ten', { ma: '', ten: 'Chọn nguyên nhân' });
        _selectHinhThucDieuTriNG.setDataSource(dmcHinhThuc, 'ma', 'ten', { ma: '', ten: 'Chọn hình thức điều trị' });
        _selectNguyenNhanTaiNanXE.setDataSource(dmcNhomNguyenNhan, 'ma', 'ten', { ma: '', ten: 'Chọn nguyên nhân' });

        _selectDoiTacNG.setDataSource(arrRes[0].data_info, 'ma', 'ten_tat', { ma: '', ten_tat: 'Chọn đối tác' }).onChange(setMaChiNhanhNG);
        _selectDoiTacXE.setDataSource(arrRes[0].data_info, 'ma', 'ten_tat', { ma: '', ten_tat: 'Chọn đối tác' }).onChange(setMaChiNhanhXE);
        _selectDoiTacNG.setValue(ESCS_MA_DOI_TAC_QL);
        _selectDoiTacXE.setValue(ESCS_MA_DOI_TAC_QL);
        setMaChiNhanhNG();
        setMaChiNhanhXE();
    });

    updateLabel();
});

