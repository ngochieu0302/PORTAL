const sidenav = document.getElementById("sidenavMainNav");
const sidenavInstance = mdb.Sidenav.getInstance(sidenav);
let innerWidth = null;
const setMode = (e) => {
    if (sidenav == null) return;

    // Check necessary for Android devices
    if (window.innerWidth === innerWidth) {
        return;
    }

    innerWidth = window.innerWidth;

    //xl
    if (window.innerWidth >= 1200) {
        sidenavInstance.changeMode("side");
        sidenavInstance.show();
        return;
    }
    //lg
    if (window.innerWidth >= 992) {
        sidenavInstance.changeMode("side");
        sidenavInstance.hide();
        return;
    }
    //md
    sidenavInstance.changeMode("over");
    sidenavInstance.hide();
};
setMode();
window.addEventListener("resize", setMode);

function updateLabel() {
    document.querySelectorAll('.form-outline').forEach((formOutline) => {
        new mdb.Input(formOutline).init();
    });
}


function formatStrDate(str) {
    const arr = str.split('/');
    if (arr.length != 3) return str;
    if (arr[0].length < 2) arr[0] = '0' + arr[0];
    if (isNaN(arr[0]) || arr[0] < 1 || arr[0] > 31) arr[0] = '01';
    if (arr[1].length < 2) arr[1] = '0' + arr[1];
    if (isNaN(arr[1]) || arr[1] < 1 || arr[1] > 12) arr[1] = '01';
    return arr.join('/');
}
document.querySelectorAll('.datepicker').forEach((element) => {
    const input = element.querySelector('input');
    input.addEventListener('focusout', () => {
        input.value = formatStrDate(input.value);
    })
    const instance = mdb.Datepicker.getOrCreateInstance(element);
    instance.update({
        monthsFull: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
        monthsShort: ['T1', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7', 'T8', 'T9', 'T10', 'T11', 'T12'],
        weekdaysFull: ['Thứ hai', 'Thứ ba', 'Thứ tư', 'Thứ năm', 'Thứ sáu', 'Thứ bảy', 'Chủ nhật'],
        weekdaysNarrow: ['CN', 'H', 'B', 'T', 'N', 'S', 'B'],
        disableFuture: true,
    });
});
const updateWrap = () => {
    document.querySelectorAll('div.d-flex.flex-nowrap.text-nowrap').forEach(el => {
        el.querySelectorAll('span.fw-bold').forEach(item => {
            item.classList.add('text-wrap');
        })
    })
}
function UpdatePageTitle(title) {
    document.getElementById('layout-page-title').innerHTML = title;
}
function GetPageTitle() {
    return document.getElementById('layout-page-title').innerHTML;
}
$(document).ready(() => {
    updateLabel();
})