//create by: thanhnx.vbi
function CommonService() {
    // Đổi mật khẩu
    this.doiMatKhau = function (obj) {
        var _service = new Service();
        return _service.postData("/home/changepass", obj);
    }
    // Thoát
    this.dangXuat = function () {
        window.location.href = "/home/logout";
    }
    // Lấy ảnh chi tiết hiển thị lên Viewer
    this.layAnhChiTiet = function (obj) {
        var _service = new Service();
        return _service.postData("/contract/health/getfiles", obj);
    }
}
function validateEmailControl(el) {
    var error = "";
    if ($(el)) {
        var val = $(el).val();
        if (val != undefined && val != null && val.trim() !== "") {
            var check = ESCheck.isEmail(val);
            if (!check) {
                error = "Không đúng định dạng email";
            }
        }
    }
    return error;
}
function validateCMTControl(el) {
    var error = "";
    if ($(el)) {
        var val = $(el).val();
        if (val != undefined && val != null && val !== "") {
            if (val.length != 8 && val.length != 9 && val.length != 12) {
                error = "Không đúng định dạng CMT/CCCD";
            }
        }
    }
    return error;
}
function validatePhoneControl(el) {
    var error = "";
    if ($(el)) {
        var val = $(el).val().replace(/_/g, '');
        if (val != undefined && val != null && val !== "") {
            if (val.length != 10 && val.length != 11) {
                error = "Không đúng định dạng số điện thoại";
                return error;
            }
            if (val.substring(0, 1) != "0" && val.substring(0, 1) != "1") {
                error = "Số điện thoại phải bắt đầu từ số 0 hoặc 1";
                return error;
            }
            var arr_11 = [
                "120", "121", "122", "126", "128",//mobilephone
                "123", "124", "125", "127", "127",//vinaphone
                "186", "188", "189",//vietnammobile
                "199"//gmobile
            ];
            var arr_10 = [
                "70", "79", "77", "76", "78", "93", "89", "90",//mobilephone
                "83", "84", "85", "81", "82", "91", "94", "88", "87",//vinaphone
                "32", "33", "34", "35", "36", "37", "38", "39", "86", "96", "97", "98",//viettell
                "56", "58", "92", "52",//vietnammobile,
                "99", "59", //gmobile //"1900"
            ];
            var arr_ban = [
                "212", "213", "214", "215", "216", "217", "218", // tây bắc bộ
                "203", "204", "205", "206", "207", "208", "209", "210", "219", // đông bắc bộ
                "211", "220", "221", "222", "225", "226", "227", "228", "229", //đồng bằng sông hồng
                "240", "241", "242", "243", "244", "245", "246", "247", "248", "249", // Hà nội
                "232", "233", "234", "237", "238", "239", // bắc trung bộ
                "235", "236", "252", "255", "256", "257", "258", "259", // nam trung bộ
                "260", "261", "262", "263", "269", // các tỉnh tây nguyên
                "280", "281", "282", "283", "284", "285", "286", "287", "288", "289",// TP HCM
                "251", "254", "271", "274", "276", // đông nam bộ
                "270", "272", "273", "275", "277", "290", "291", "292", "293", "294", "296", "297", "299" // tây nam bộ
            ];
            arr_tong_dai = ["900"];
            if (val.length == 11) {
                var ba_ky_tu = val.substring(1, 4);
                if (!arr_11.includes(ba_ky_tu) && !arr_ban.includes(ba_ky_tu)) {
                    error = "Đầu số điện thoại không hợp lệ";
                    return error;
                }
            }
            if (val.length == 10) {
                var hai_ky_tu = val.substring(1, 3);
                var ba_ky_tu = val.substring(1, 4);
                if (!arr_10.includes(hai_ky_tu) && !arr_ban.includes(ba_ky_tu) && !arr_tong_dai.includes(ba_ky_tu)) {
                    error = "Đầu số điện thoại không hợp lệ";
                    return error;
                }
            }
        }
    }
    return error;
}
