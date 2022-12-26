function ShowAlert(alertBody, alertTitle = '', alertIcon = '', alertColor = 'primary', autohide = true, ondblclick = () => { }) {
    var alertTitle = (alertTitle == null || alertTitle == '') ? 'Thông báo !' : alertTitle;
    iconstr = (alertIcon != null && alertIcon != '') ? `<i class="fas ${alertIcon} fa-lg me-2"></i>` : '';
    const newAlert = document.createElement('div');
    const x = alertColor === 'dark' ? 'btn-close-white' : '';
    newAlert.innerHTML =
        `<div class="toast-header">
            ${iconstr}<strong class="me-auto overflow-hidden fs-6">${alertTitle}</strong>
            <button type="button" class="btn-close ${x}" data-mdb-dismiss="toast" aria-label="Close"></button>
        </div>
        <div class="toast-body">${`<span class="user-select-all fs-5">${alertBody}</span>`}&nbsp;</div>`;
    newAlert.classList.add('toast', 'mt-2');
    newAlert.classList.add('fade');
    newAlert.ondblclick = ondblclick;
    document.body.appendChild(newAlert);

    const instance = new mdb.Toast(newAlert, {
        stacking: true,
        hidden: true,
        width: '400px',
        position: 'top-right',
        autohide: autohide,
        delay: 2500,
        offset: 0,
        color: alertColor,
    });
    instance.show();
}