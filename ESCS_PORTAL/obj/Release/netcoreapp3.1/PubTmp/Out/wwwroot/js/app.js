$(function () {
    'use strict';

    function initMenu() {
        $('.logout').click(function () {
            var _common = new CommonService();
            var _notifyService = new NotifyService();
            _notifyService.confirmDelete("Bạn có thực sự muốn đăng xuất không?", "", val => {
                _common.dangXuat();
            });
        });
        $('#changePass').click(function () {
            var _common = new CommonService();
            var frmChangePass = new FormService('frmChangePass');
            if (frmChangePass.isValid()) {
                var pas_moi = frmChangePass.getControl('pas_moi').getValue();
                var pas_nhap_lai = frmChangePass.getControl('pas_nhap_lai').getValue();
                if (pas_moi !== pas_nhap_lai) {
                    $('#re_input_error').show();
                    $('form[name=frmChangePass] input[name=pas_nhap_lai]').addClass("is-invalid");
                } else {
                    $('#re_input_error').hide();
                    $('form[name=frmChangePass] input[name=pas_nhap_lai]').removeClass("is-invalid");
                    var obj = frmChangePass.getJsonData();
                    _common.doiMatKhau(obj).then(res => {
                        if (res.state_info.status === "OK") {
                            ShowAlert('Đổi mật khẩu thành công!', '', '', 'success');
                            frmChangePass.resetForm();
                        } else {
                            ShowAlert(res.state_info.message_body, '', '', 'danger');
                        }
                    });
                }
            }
        });
        $("#toRecover").click(function () {
            ShowAlert('Vui lòng liên hệ quản trị viên để được cấp lại mật khẩu!', '', '', 'danger');
        });
    }
    function initComponents() {
        // digit
        $(document).on("keypress", "input.digit", function (e) {
            var keycode = e.which || e.keyCode;
            var arrKeycode = [8, 37, 39];
            if (!(e.shiftKey == false && ((arrKeycode.indexOf(keycode) > 0) || (keycode >= 48 && keycode <= 57)))) {
                e.preventDefault();
            }
        });
        // number
        $(document).on("keypress", "input.number", function (e) {
            var keycode = e.which || e.keyCode;
            var arrKeycode = [8, 37, 39];
            if (!(e.shiftKey == false && ((arrKeycode.indexOf(keycode) > 0) || (keycode >= 48 && keycode <= 57)))) {
                e.preventDefault();
            }
        });
        // decimal
        $(document).on("keypress", "input.decimal", function (e) {
            var keycode = e.which || e.keyCode;
            var arrKeycode = [8, 37, 39, 46];
            if (!(event.shiftKey == false && ((arrKeycode.indexOf(keycode) > 0) || (keycode >= 48 && keycode <= 57)))) {
                event.preventDefault();
            }
        });
    }

    function initComponentsChanged() {
        // digit
        $(document).on("keypress", "input.digit", function (e) {
            var keycode = e.which || e.keyCode;
            var arrKeycode = [8, 37, 39];
            if (!(e.shiftKey == false && ((arrKeycode.indexOf(keycode) > 0) || (keycode >= 48 && keycode <= 57)))) {
                e.preventDefault();
            }
        });
        // number
        $(document).on("keypress", "input.number", function (e) {
            var keycode = e.which || e.keyCode;
            var arrKeycode = [8, 37, 39];
            if (!(e.shiftKey == false && ((arrKeycode.indexOf(keycode) > 0) || (keycode >= 48 && keycode <= 57)))) {
                e.preventDefault();
            }
        });
        // upper
        $(document).on("keyup", "input.upper", function (e) {
            var value = $(this).val();
            $(this).val(value.toUpperCase());
        });
        // code
        $(document).on("keyup", "input.code", function (e) {
            var value = $(this).val();
            $(this).val(value.replace(/[^a-zA-Z0-9]/g, ''));
        });
        $(document).on("keyup", "input.number", function (e) {
            var value = ESUtil.formatMoney(parseInt($(this).val().replace(/[^0-9]+/g, '')));
            $(this).val(value);
        });
        // decimal
        $(document).on("keypress", "input.decimal", function (e) {
            var keycode = e.which || e.keyCode;
            var arrKeycode = [8, 37, 39, 46];
            if (!(event.shiftKey == false && ((arrKeycode.indexOf(keycode) > 0) || (keycode >= 48 && keycode <= 57)))) {
                event.preventDefault();
            }
        });

        $('.modal-dialog-move').draggable({
            handle: ".modal-header"
        });
        // tooltip
        if (jQuery().tooltip) {
            $(document).on("DOMNodeInserted", '[data-toggle="tooltip"]', function () {
                jQuery(this).tooltip();
            });
        }
        //popover
        if (jQuery().popover) {
            $(document).on("DOMNodeInserted", '[data-toggle="popover"]', function () {
                jQuery(this).popover();
            });
        }
        // datetimepicker
        if (jQuery().datetimepicker) {
            $(document).on("DOMNodeInserted", 'input.datepicker', function () {
                jQuery(this).datetimepicker({
                    format: "DD/MM/YYYY",
                    icons: {
                        time: "fa fa-clock-o",
                        date: "fa fa-calendar",
                        up: "fa fa-arrow-up",
                        down: "fa fa-arrow-down",
                        previous: "fa fa-chevron-left",
                        next: "fa fa-angle-right",
                        today: "fa fa-clock-o",
                        clear: "fa fa-trash-o"
                    }
                });
            });
        }
        //colorpicker
        $(document).on("DOMNodeInserted", '.colorpicker', function () {
            jQuery(this).minicolors({
                control: $(this).attr('data-control') || 'hue',
                defaultValue: $(this).attr('data-defaultValue') || '',
                format: $(this).attr('data-format') || 'hex',
                keywords: $(this).attr('data-keywords') || '',
                inline: $(this).attr('data-inline') === 'true',
                letterCase: $(this).attr('data-letterCase') || 'lowercase',
                opacity: $(this).attr('data-opacity'),
                position: $(this).attr('data-position') || 'bottom left',
                swatches: $(this).attr('data-swatches') ? $(this).attr('data-swatches').split('|') : [],
                change: function (value, opacity) {
                    if (!value) return;
                    if (opacity) value += ', ' + opacity;
                    if (typeof console === 'object') {
                        console.log(value);
                    }
                },
                theme: 'bootstrap'
            });
        });
        //dual listbox
        $(document).on("DOMNodeInserted", '.duallistbox', function () {
            jQuery(this).bootstrapDualListbox();
        });
        $(document).on("DOMNodeInserted", '.currency-inputmask', function () {
            jQuery(this).inputmask("999,999,999,999,999");
        });

        $(document).on("DOMNodeInserted", '.phone', function () {
            jQuery(this).inputmask("9999999999");
        });
        $(document).on("DOMNodeInserted", '.phone-long', function () {
            jQuery(this).inputmask("999999999999");
        });
        //Repeater
        $(document).on("DOMNodeInserted", '.repeater', function () {
            jQuery(this).repeater({
                show: function () {
                    $(this).slideDown();
                },
                hide: function (remove) {
                    if (confirm('Are you sure you want to remove this item?')) {
                        $(this).slideUp(remove);
                    }
                }
            });
        });
        //Daterangepicker
        $(document).on("DOMNodeInserted", '.daterange', function () {
            jQuery(this).daterangepicker({
                locale: {
                    format: "DD/MM/YYYY"
                },
                drops: $(this).data('drops')
            });
        });
        //Datepicker
        $(document).on("DOMNodeInserted", '.datepicker', function () {
            jQuery(this).daterangepicker({
                singleDatePicker: true,
                showDropdowns: true,
                locale: {
                    format: "DD/MM/YYYY"
                },
                drops: $(this).data('drops')
            });
        });
        //timepicker
        $(document).on("DOMNodeInserted", '.time', function () {
            jQuery(this).timepicker({
                minuteStep: 15,
                showMeridian: false
            });
        });
        //Modal draggable
        $(document).on("DOMNodeInserted", '.modal-draggable', function () {
            jQuery(this).on({
                mousedown: function (mousedownEvt) {
                    var $draggable = $(this);
                    var x = mousedownEvt.pageX - $draggable.offset().left,
                        y = mousedownEvt.pageY - $draggable.offset().top;
                    $("body").on("mousemove.draggable", function (mousemoveEvt) {
                        $draggable.closest(".modal-dialog").offset({
                            "left": mousemoveEvt.pageX - x,
                            "top": mousemoveEvt.pageY - y
                        });
                    });
                    $("body").one("mouseup", function () {
                        $("body").off("mousemove.draggable");
                    });
                    $draggable.closest(".modal").one("bs.modal.hide", function () {
                        $("body").off("mousemove.draggable");
                    });
                }
            });
        });

        $(document).on("DOMNodeInserted", '[data-toggle="inline-popup"]', function () {
            $('[data-toggle="inline-popup"]').inlinePopover();
        });
    }

    function init() {
        initMenu();
        initComponents();
        initComponentsChanged();
    }
    init();
});