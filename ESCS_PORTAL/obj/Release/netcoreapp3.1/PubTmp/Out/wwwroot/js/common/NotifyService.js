//create by: thanhnx.vbi
function createCancelButton(text, fnClick) {
    return $('<button class="btn btn-default">' + text + '</button>').on('click', fnClick);
}
function createAcceptButton(text, fnClick) {
    return $('<button class="btn btn-danger">' + text + '</button>').on('click', fnClick);
}
function NotifyService() {
    this.success = function (msg) {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "50",
            "hideDuration": "0",
            "timeOut": "3000",
            "extendedTimeOut": "500",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "slideDown",
            "hideMethod": "fadeOut"
        };
        toastr.success(msg);
    };
    this.error = function (msg) {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "50",
            "hideDuration": "0",
            "timeOut": "3000",
            "extendedTimeOut": "500",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "slideDown",
            "hideMethod": "fadeOut"
        };
        toastr.error(msg);
    };
    this.warning = function (msg) {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "50",
            "hideDuration": "0",
            "timeOut": "3000",
            "extendedTimeOut": "500",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "slideDown",
            "hideMethod": "fadeOut"
        };
        toastr.warning(msg);
    };
    this.confirmDelete = function (message, dataDetele = "", fnAccept = undefined) {
        var data = dataDetele;
        Swal.fire({
            title: 'Thông báo',
            text: message,
            type: 'warning',
            showCancelButton: true,
            cancelButtonText: "Đóng",
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Chấp nhận'
        }).then((result) => {
            if (result.value !== undefined && result.value === true) {
                if (fnAccept) {
                    fnAccept(data);
                }
            }
        });
    };
    this.confirm = function (message, dataDetele = "", fnAccept = undefined) {
        var data = dataDetele;
        Swal.fire({
            title: 'Thông báo',
            text: message,
            type: 'warning',
            showCancelButton: true,
            cancelButtonText: "Đóng",
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Đồng ý'
        }).then((result) => {
            if (result.value !== undefined && result.value === true) {
                if (fnAccept) {
                    fnAccept(data);
                }
            }
        });
    }
    this.confirmPhuongAn = function (message, fnAccept = undefined) {
        Swal.fire({
            title: 'Thông báo',
            text: message,
            type: 'warning',
            showCancelButton: true,
            cancelButtonText: "Đóng",
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Đồng ý'
        }).then((result) => {
            if (fnAccept) {
                fnAccept(result.value);
            }
        });
    }
    this.confirmHTML = function (message, dataDetele = "", fnAccept = undefined) {
        var data = dataDetele;
        Swal.fire({
            title: 'Thông báo',
            html: message,
            type: 'warning',
            showCancelButton: true,
            cancelButtonText: "Đóng",
            confirmButtonColor: '#0c8241',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Đồng ý'
        }).then((result) => {
            if (result.value !== undefined && result.value === true) {
                if (fnAccept) {
                    fnAccept(data);
                }
            }
        });
    }
    this.info = function (message, fnAccept = undefined) {
        Swal.fire({
            title: message,
            icon: 'success',
            type: 'success',
            allowOutsideClick: false,
            showCancelButton: false,
            confirmButtonColor: '#0c8241',
            confirmButtonText: 'Đồng ý'
        }).then((result) => {
            if (result.value !== undefined && result.value === true) {
                if (fnAccept) {
                    fnAccept();
                }
            }
        });
    }
    this.confirmAddProfile = function (message, fnAccept = undefined) {
        Swal.fire({
            title: "Thông báo",
            text: message,
            showDenyButton: true,
            showCancelButton: true,
            confirmButtonText: "Đồng ý",
            confirmButtonColor: '#3085d6',
            //denyButtonText: "Đồng ý và gửi email",
            //denyButtonColor: '#3085d6',
            //cancelButtonText: "Đóng",
            cancelButtonColor: '#d33',
            preDeny: false
        }).then((result) => {
            var type = result.isConfirmed ? "DONG_Y" : (result.isDenied ? "" : "");
            if (type != "" && fnAccept) {
                fnAccept(type);
            }
        });
    }
    this.confirmApprove = function (message, fnAccept = undefined) {
        Swal.fire({
            title: 'Thông báo',
            text: message,
            showCancelButton: true,
            cancelButtonText: "Đóng",
            cancelButtonColor: '#d33',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Phê duyệt',
        }).then((result) => {
            var type = result.isConfirmed ? "PHE_DUYET_VA_GUI_EMAIL" : "";
            if (type != "" && fnAccept) {
                fnAccept(type);
            }
        });
    }
    this.confirmEmail = function (message, fnAccept = undefined) {
        Swal.fire({
            title: 'Thông báo',
            text: message,
            showCancelButton: true,
            cancelButtonText: "Đóng",
            cancelButtonColor: '#d33',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Đồng ý',
        }).then((result) => {
            var type = result.isConfirmed ? "CHUYEN_VA_GUI_MAIL" : "";
            if (type != "" && fnAccept) {
                fnAccept(type);
            }
        });
    }
    this.confirmProcessed = function (message, fnAccept = undefined) {
        Swal.fire({
            title: 'Thông báo',
            text: message,
            showCancelButton: true,
            cancelButtonText: "Đóng",
            cancelButtonColor: '#d33',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Trình duyệt',
        }).then((result) => {
            var type = result.isConfirmed ? "TRINH_DUYET_VA_GUI_EMAIL" : "";
            if (type != "" && fnAccept) {
                fnAccept(type);
            }
        });
    }
    this.confirmOTP = function (message, fnAccept = undefined) {
        Swal.fire({
            title: 'Nhập thông tin mã OTP',
            text: message,
            input: 'text',
            showCancelButton: true,
            cancelButtonText: "Đóng",
            cancelButtonColor: '#d33',
            confirmButtonText: 'OK',
            confirmButtonColor: '#3085d6',
            showLoaderOnConfirm: true,
            allowOutsideClick: false
        }).then(function (result) {
            var data = result.value;
            if (fnAccept) {
                fnAccept(data);
            }
        })
    }
    this.trinhDuyetBoiThuong = function (message, fnAccept = undefined) {
        Swal.fire({
            title: "Thông báo",
            text: message,
            showDenyButton: true,
            showCancelButton: true,
            confirmButtonText: "Trình bồi thường",
            confirmButtonColor: '#3085d6',
            denyButtonText: "Trình bồi thường kèm bảo lãnh",
            denyButtonColor: '#3085d6',
            cancelButtonText: "Đóng",
            cancelButtonColor: '#d33',
            preDeny: false,
            width: '420px'
        }).then((result) => {
            var type = result.isConfirmed ? "BOI_THUONG" : (result.isDenied ? "BOI_THUONG_BAO_LANH" : "");
            if (type != "" && fnAccept) {
                fnAccept(type);
            }
        });
    }
    this.ketThucGiamDinh = function (message, fnDidOpen = undefined, fnAccept = undefined) {
        Swal.fire({
            title: "Thông báo",
            text: message,
            html: message +"<hr class='gd_duyet_gia_tu_dong'><div class='custom-control custom-checkbox gd_duyet_gia_tu_dong'><input type='checkbox' id='gd_duyet_gia_tu_dong' class='custom-control-input'><label class='custom-control-label' for='gd_duyet_gia_tu_dong'>Hồ sơ đủ điều kiện duyệt giá tự động theo cấu hình. Vui lòng tích chọn xác nhận duyệt giá tự động hồ sơ này.</label></div>", 
            showDenyButton: true,
            showCancelButton: true,
            confirmButtonText: "Kết thúc",
            confirmButtonColor: '#3085d6',
            denyButtonText: "Kết thúc và chuyển bồi thường",
            denyButtonColor: '#3085d6',
            cancelButtonText: "Đóng",
            cancelButtonColor: '#d33',
            preDeny: false,
            width: '420px',
            didOpen: () => {
                fnDidOpen();
            }
        }).then((result) => {
            var type = result.isConfirmed ? "KET_THUC" : (result.isDenied ? "KET_THUC_CHUYEN" : "");
            if (type != "" && fnAccept) {
                fnAccept(type);
            }
        });
    }
    this.ChuyenTinhToan = function (message, fnDidOpen = undefined, fnAccept = undefined) {
        Swal.fire({
            title: "Thông báo",
            text: message,
            html: message + "<hr class='tt_duyet_tu_dong'><div class='custom-control custom-checkbox tt_duyet_tu_dong'><input type='checkbox' id='tt_duyet_tu_dong' class='custom-control-input'><label class='custom-control-label' for='tt_duyet_tu_dong'>Hồ sơ đủ điều kiện duyệt tự động theo cấu hình. Vui lòng tích chọn xác nhận duyệt tự động hồ sơ này.</label></div>",
            showDenyButton: false,
            showCancelButton: true,
            confirmButtonText: "Chuyển tính toán",
            confirmButtonColor: '#3085d6',
            cancelButtonText: "Đóng",
            cancelButtonColor: '#d33',
            width: '420px',
            didOpen: () => {
                fnDidOpen();
            }
        }).then((result) => {
            var type = result.isConfirmed ? "CHUYEN_TINH_TOAN" : "";
            if (type != "" && fnAccept) {
                fnAccept(type);
            }
        });
    }

    this.ketThucBaoGia = function (message, fnAccept = undefined) {
        Swal.fire({
            title: "Thông báo",
            text: message,
            showDenyButton: true,
            showCancelButton: true,
            confirmButtonText: "Kết thúc",
            confirmButtonColor: '#3085d6',
            denyButtonText: "Kết thúc & chuyển phương án",
            denyButtonColor: '#3085d6',
            cancelButtonText: "Đóng",
            cancelButtonColor: '#d33',
            preDeny: false,
            width: '420px'
        }).then((result) => {
            var type = result.isConfirmed ? "KET_THUC_BG" : (result.isDenied ? "KET_THUC_BG_LAP_PA" : "");
            if (type != "" && fnAccept) {
                fnAccept(type);
            }
        });
    }
    this.ketThucBaoGiaGD = function (message, fnAccept = undefined) {
        Swal.fire({
            title: "Thông báo",
            text: message,
            showCancelButton: true,
            confirmButtonText: "Kết thúc",
            confirmButtonColor: '#3085d6',
            cancelButtonText: "Đóng",
            cancelButtonColor: '#d33',
            preDeny: false,
            width: '420px'
        }).then((result) => {
            var type = result.isConfirmed ? "KET_THUC_BG" : "";
            if (type != "" && fnAccept) {
                fnAccept(type);
            }
        });
    }

    this.trinhDuyetBoiThuongPA = function (message, fnAccept = undefined) {
        Swal.fire({
            title: 'Thông báo',
            text: message,
            showCancelButton: true,
            cancelButtonText: "Đóng",
            cancelButtonColor: '#d33',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Trình duyệt',
            //html: "<div class='custom-control custom-checkbox custom-control-inline mr-3' style='margin:unset;'><input type='checkbox' id='CHECK_PHUONG_AN' disabled='disabled' class='custom-control-input' data-val='PHUONG_AN' checked='checked'><label class='custom-control-label' style='cursor:pointer;padding-top: 3px;' for='CHECK_PHUONG_AN'>Trình phương án</label></div>"
            //    + "<div class='custom-control custom-checkbox custom-control-inline mr-3' style='margin:unset;'><input type='checkbox' id='CHECK_BOI_THUONG' class='custom-control-input' data-val='BOI_THUONG'><label class='custom-control-label' style='cursor:pointer;padding-top: 3px;' for='CHECK_BOI_THUONG'>Trình bồi thường</label></div>"
            //    + "<div class='custom-control custom-checkbox custom-control-inline mr-3' style='margin:unset;'><input type='checkbox' id='CHECK_BAO_LANH' disabled='disabled' class='custom-control-input' data-val='BAO_LANH'><label class='custom-control-label' style='cursor:pointer;padding-top: 3px;' for='CHECK_BAO_LANH'>Trình bảo lãnh</label></div>",  
            //didOpen: () => {
            //    $("#CHECK_BOI_THUONG").change(function () {
            //        $("#CHECK_BAO_LANH").attr("disabled", "disabled");
            //        $("#CHECK_BAO_LANH").prop("checked", false);
            //        var checked = $("#CHECK_BOI_THUONG").is(":checked");
            //        if (checked) {
            //            $("#CHECK_BAO_LANH").removeAttr("disabled");
            //        }
            //    });
            //},
            width: '455px'
        }).then((result) => {
            var type = "PHUONG_AN";
            if ($("#CHECK_BOI_THUONG").is(":checked")) {
                type += "_BOI_THUONG";
            }
            if ($("#CHECK_BAO_LANH").is(":checked")) {
                type += "_BAO_LANH";
            }
            if (type != "" && fnAccept) {
                fnAccept(type);
            }
        });
    }
    this.confirmDelay = function (message, timeDelaySecond, fnAccept = undefined) {
        var timerInterval = null;
        Swal.fire({
            title: "Thông báo",
            html: message,
            showConfirmButton: false,
            showCancelButton: true,
            cancelButtonText: "<i class='fas fa-times'></i> Hủy thao tác",
            cancelButtonColor: '#d33',
            timer: timeDelaySecond * 1000,
            allowOutsideClick: false,
            didOpen: (toast) => {
                timerInterval = setInterval(() => {
                    Swal.getHtmlContainer().querySelector('strong')
                        .textContent = Math.round(Swal.getTimerLeft() / 1000);
                }, 1000);
            },
            didClose: () => {
                clearInterval(timerInterval)
            },
        }).then((result) => {
            if (result.dismiss.toLowerCase() == "timer") {
                if (fnAccept) {
                    fnAccept();
                }
            }
        });
    };
    this.createButton = function (text, cssClass, cb) {
        return $('<button class="' + cssClass + '">' + text + '</button>').on('click', cb);
    }
    this.showNotify = function (msg, thong_bao) {
        var notify_url = this.getUrl(thong_bao);
        var notify_id = thong_bao.gid;
        var notify_ctiet_ma_doi_tac = thong_bao.ctiet_ma_doi_tac;
        var notify_ctiet_so_id = thong_bao.ctiet_so_id;
        var notify_ctiet_hanh_dong = thong_bao.ctiet_hanh_dong;
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "showDuration": "50",
            "hideDuration": "0",
            "timeOut": "3000",
            "extendedTimeOut": "500",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "slideDown",
            "hideMethod": "fadeOut",
            "onclick": function () {
                if (notify_url === undefined || notify_url === null || notify_url === "" ||
                    (ho_so_chi_tiet !== undefined && ho_so_chi_tiet !== null &&
                        ho_so_chi_tiet.data_info !== undefined && ho_so_chi_tiet.data_info !== null &&
                        ho_so_chi_tiet.data_info.ho_so !== undefined && ho_so_chi_tiet.data_info.ho_so !== null &&
                        ho_so_chi_tiet.data_info.ho_so.so_id !== undefined && ho_so_chi_tiet.data_info.ho_so.so_id !== null &&
                        ho_so_chi_tiet.data_info.ho_so.so_id.toString() === notify_ctiet_so_id && ho_so_chi_tiet.data_info.ho_so.ma_doi_tac === notify_ctiet_ma_doi_tac)
                ) {
                    return;
                }
                window.location.href = "/home/redirectnotify?gid=" + notify_id + "&ma_doi_tac=" + notify_ctiet_ma_doi_tac + "&so_id=" + notify_ctiet_so_id + "&hanh_dong=" + notify_ctiet_hanh_dong + "&url_redirect=" + notify_url;
            }
        };
        toastr.clear();
        var t = toastr.info(msg);
        t.attr('id', "notify-app-" + thong_bao.id);
        t.attr('notify-id', thong_bao.id);
        t.attr('notify-url', this.getUrl(thong_bao));
    };
    this.getUrl = function (thong_bao) {
        if (thong_bao.ctiet_hanh_dong === null || thong_bao.ctiet_hanh_dong === "")
            return;
        var url = "";
        switch (thong_bao.ctiet_hanh_dong) {
            case "XEM_CTIET_HO_SO_GD":
                url = "/carclaim/carinvestigation"; break;
            case "XEM_CTIET_HO_SO_BT":
                url = "/carclaim/carcompensation"; break;
            case "XEM_CTIET_HO_SO_PHE_DUYET":
                url = "/manager/approved"; break;
            case "XEM_CTIET_HO_SO_THANH_TOAN":
                url = "/manager/payment"; break;
            default:
        }
        return url;
    }
}
