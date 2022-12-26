const pageLogin = "/home/logout";
const INVALID_AUTH = "INVALID_AUTH";

var IS_MULTIPLE_PROMISE = false;
const _loadingService = new LoadingService();
//_loadingService.init();

let stackLoading = 0;
const checkLoading = setInterval(() => {
    if (stackLoading > 0) _loadingService.init();
    else _loadingService.destroy();
}, 500);
function showLoading(isShow, aceptShow, divLoading = 'body') {
    if (isShow && aceptShow) {
        ESLoading.show(divLoading);
    }
    else {
        ESLoading.hide(divLoading);
    }
}
function Service(isLoading = true, divLoading = "body") {
    IS_MULTIPLE_PROMISE = false;
    this.isMultiplePromise = false;
    this.divLoading = divLoading;
    this.isLoading = isLoading;
    this.isArrayData = false;
    this.isPostObject = false;
    this.showErrorMsg = true;

    this.triggerLoading = () => {
        _loadingService.init();
    }

    this.request = {
        cache: false,
        datatype: 'json',
        headers: {

        },
        beforeSend: function () {
            stackLoading += 1;
            _loadingService.init();
        },
        complete: function () {
            stackLoading -= 1;
        }
    };
    this.addHeader = function (key, value) {
        this.request.headers[key] = value;
    };
    this.getData = function (url) {
        var rq = this.request;
        return new Promise((resolve, reject) => {
            rq.type = 'get';
            rq.url = url;
            rq.data = {};
            rq.success = function (response) {
                if (response !== undefined && response !== null && response.state_info !== undefined && response.state_info !== null && response.state_info.status === "NotOK") {
                    if (response.state_info.message_code !== INVALID_AUTH) {
                        ShowAlert(res.state_info.message_body, '', '', 'danger');
                        return;
                    }
                    ShowAlert("Tài khoản của bạn đã bị thay đổi mật khẩu. Trở về trang đăng nhập sau 3 giây.", '', '', 'danger');
                    setTimeout(function () { window.location.href = pageLogin; }, 3000);
                }
                resolve(response);
            };
            rq.error = function (err) {
                if (err.status === 427) {
                    ShowAlert(err.responseJSON.message, '', '', 'danger');
                }
                else if (err.status === 428) {
                    window.location.href = pageLogin;
                }
                else if (err.status === 404) {
                    ShowAlert(err.responseJSON.message, '', '', 'danger');
                }
                else {
                    ShowAlert(err.responseJSON.message, '', '', 'danger');
                }
                reject(err);
            };
            $.ajax(rq);
        });
    };
    this.postData = function (url, data) {
        if (data !== undefined && data !== null && typeof data === 'object' && data.hasOwnProperty("nsd")) {
            delete data.nsd;
        }
        var rq = this.request;
        rq.headers["RequestVerificationToken"] = $("input[name='__RequestVerificationToken']").val();
        var isArr = this.isArrayData;
        var isPostObject = this.isPostObject;
        var showErrorMsg = this.showErrorMsg;
        return new Promise((resolve, reject) => {
            rq.type = 'post';
            rq.url = url;
            if (isArr) {
                rq.contentType = 'application/json; charset=utf-8';
                rq.data = JSON.stringify(data);
            }
            else if (isPostObject)
            {
                rq.data = data;
            }
            else {
                rq.data = JSON.stringify(data);
            }
            rq.success = function (response) {
                if (response !== undefined && response !== null && response.state_info !== undefined && response.state_info !== null && response.state_info.status === "NotOK") {
                    if (response.state_info.message_code !== INVALID_AUTH && showErrorMsg) {
                        ShowAlert(response.state_info.message_body, '', '', 'danger');
                        return;
                    }
                    if (!IS_MULTIPLE_PROMISE && showErrorMsg) {
                        ShowAlert("Tài khoản của bạn đã bị thay đổi mật khẩu. Trở về trang đăng nhập sau 3 giây.", '', '', 'danger');
                        setTimeout(function () { window.location.href = pageLogin; }, 2500);
                    }
                    if (response.state_info.message_code === INVALID_AUTH) {
                        setTimeout(function () { window.location.href = pageLogin; }, 2500);
                    }
                }
                resolve(response);
            };
            rq.error = function (err) {
                if (showErrorMsg) {
                    if (err.status === 428) {
                        ShowAlert("Tài khoản của bạn đã bị thay đổi mật khẩu. Trở về trang đăng nhập sau 3 giây.", '', '', 'danger');
                        setTimeout(function () { window.location.href = pageLogin; }, 3000);
                    }
                    else {
                        try { ShowAlert(err.responseJSON.message); } catch { ShowAlert("Lỗi chưa xác định", '', '', 'danger'); }
                    }
                }
                reject(err);
            };
            $.ajax(rq);
        });
    };
    this.postDataTimeOut = function (url, data, timeOut = 0, messageTimeOut = "Thông tin truy vấn vượt quá giới hạn thời gian cho phép") {
        if (data !== undefined && data !== null && typeof data === 'object' && data.hasOwnProperty("nsd")) {
            delete data.nsd;
        }
        var rq = this.request;
        var isArr = this.isArrayData;
        var isPostObject = this.isPostObject;
        var showErrorMsg = this.showErrorMsg;
        return new Promise((resolve, reject) => {
            rq.type = 'post';
            rq.url = url;
            if (isArr) {
                rq.contentType = 'application/json; charset=utf-8';
                rq.data = JSON.stringify(data);
            }
            else if (isPostObject) {
                rq.data = data;
            }
            else {
                rq.data = JSON.stringify(data);
            }
            rq.success = function (response) {
                if (response !== undefined && response !== null && response.state_info !== undefined && response.state_info !== null && response.state_info.status === "NotOK") {
                    if (response.state_info.message_code !== INVALID_AUTH && showErrorMsg) {
                        ShowAlert(response.state_info.message_body, '', '', 'danger');
                        return;
                    }
                    if (!IS_MULTIPLE_PROMISE && showErrorMsg) {
                        ShowAlert("Tài khoản của bạn đã bị thay đổi mật khẩu. Trở về trang đăng nhập sau 3 giây.", '', '', 'danger');
                        setTimeout(function () { window.location.href = pageLogin; }, 3000);
                    }
                }
                resolve(response);
            };
            rq.error = function (err) {
                if (showErrorMsg) {
                    if (err.statusText == 'timeout') {
                        ShowAlert(messageTimeOut, '', '', 'danger');
                        return;
                    }
                    else if (err.status === 428) {
                        ShowAlert("Tài khoản của bạn đã bị thay đổi mật khẩu. Trở về trang đăng nhập sau 3 giây.", '', '', 'danger');
                        setTimeout(function () { window.location.href = pageLogin; }, 3000);
                    }
                    else {
                        ShowAlert(err.responseJSON.message, '', '', 'danger');
                    }
                }
                reject(err);
            };
            rq.timeout = timeOut;
            $.ajax(rq);
        });
    };
    this.postDataObject = function (url, data) {
        if (data !== undefined && data !== null && typeof data === 'object' && data.hasOwnProperty("nsd")) {
            delete data.nsd;
        }
        var rq = this.request;
        var isArr = this.isArrayData;
        var isPostObject = this.isPostObject;
        return new Promise((resolve, reject) => {
            rq.type = 'post';
            rq.url = url;
            if (isArr) {
                rq.contentType = 'application/json; charset=utf-8';
                rq.data = JSON.stringify(data);
            }
            else {
                rq.data = data;
            }
            rq.success = function (response) {
                resolve(response);
            };
            rq.error = function (err) {
                if (err.status === 428) {
                    window.location.href = pageLogin;
                }
                else {
                    ShowAlert(err.responseJSON.message, '', '', 'danger');
                }
                reject(err);
            };
            $.ajax(rq);
        });
    };
    this.postFormData = function (url, formData) {
        var rq = this.request;
        return new Promise((resolve, reject) => {
            rq.type = 'post';
            rq.url = url;
            rq.processData = false;
            rq.data = formData;
            rq.contentType = false,
                rq.success = function (response) {
                    if (response !== undefined && response !== null && response.state_info !== undefined && response.state_info !== null && response.state_info.status === "NotOK") {
                        if (response.state_info.message_code !== INVALID_AUTH) {
                            ShowAlert(response.state_info.message_body, '', '', 'danger');
                            return;
                        }
                        ShowAlert("Tài khoản của bạn đã bị thay đổi mật khẩu. Trở về trang đăng nhập sau 3 giây.", '', '', 'danger');
                        setTimeout(function () { window.location.href = pageLogin; }, 3000);
                    }
                    resolve(response);
                };
            rq.error = function (err) {
                if (err.status === 428) {
                    window.location.href = pageLogin;
                }
                else {
                    ShowAlert(err.responseJSON.message, '', '', 'danger');
                }
                reject(err);
            };
            $.ajax(rq);
        });
    };
    this.getFile = function (url, data) {
        var rq = this.request;
        var isArr = this.isArrayData;
        return new Promise((resolve, reject) => {
            rq.type = 'post';
            rq.url = url;
            rq.responseType = 'arraybuffer';
            if (isArr) {
                rq.contentType = 'application/json; charset=utf-8';
                rq.data = JSON.stringify(data);
            }
            else {
                rq.data = JSON.stringify(data);
            }
            rq.success = function (response) {
                if (response !== undefined && response !== null && response.state_info !== undefined && response.state_info !== null && response.state_info.status === "NotOK") {
                    if (response.state_info.message_code !== INVALID_AUTH) {
                        ShowAlert(response.state_info.message_body, '', '', 'danger');
                        return;
                    }
                    ShowAlert("Tài khoản của bạn đã bị thay đổi mật khẩu. Trở về trang đăng nhập sau 3 giây.", '', '', 'danger');
                    setTimeout(function () { window.location.href = pageLogin; }, 3000);
                }
                resolve(response);
            };
            rq.error = function (err) {
                if (err.status === 428) {
                    window.location.href = pageLogin;
                }
                else {
                    ShowAlert(err.responseJSON.message, '', '', 'danger');
                }
                reject(err);
            };
            $.ajax(rq);
        });
    };
    this.all = function (arrPromise) {
        IS_MULTIPLE_PROMISE = true;
        return new Promise((resolve, reject) => {
            Promise.all(arrPromise).then(arrRes => {
                for (var i = 0; i < arrRes.length; i++) {
                    var response = arrRes[i];
                    if (response !== undefined && response !== null && response.state_info !== undefined && response.state_info !== null && response.state_info.status === "NotOK") {
                        if (response.state_info.message_code !== INVALID_AUTH) {
                            ShowAlert(response.state_info.message_body, '', '', 'danger');
                            return;
                        }
                        ShowAlert("Tài khoản của bạn đã bị thay đổi mật khẩu. Trở về trang đăng nhập sau 3 giây.", '', '', 'danger');

                        setTimeout(function () { window.location.href = pageLogin; }, 3000);
                        IS_MULTIPLE_PROMISE = false;
                        return;
                    }
                }
                resolve(arrRes);
            });
        });
    }
}