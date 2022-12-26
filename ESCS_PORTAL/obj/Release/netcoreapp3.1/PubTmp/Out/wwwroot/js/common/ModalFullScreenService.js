function ModalFullScreenService(modalId, fullScreenId = undefined, title = undefined) {
    this.modalId = null;
    this.title = title;
    this.fullScreenId = fullScreenId;
    this.options = null;
    this.OnInit = function () {
        this.modalId = modalId; 
        if (title !== undefined && $("#" + modalId+" .modal-title") !== undefined) {
            $("#" + modalId +" .modal-title").html(title);
        }
		if (fullScreenId !== undefined) {
            $("#" + modalId).data('fullscreen', fullScreenId);
        } 
    };
    this.setTitle = function (title) {
        var id = this.modalId;
        if (title !== undefined && $("#" + id +" .modal-title") !== undefined) {
            $("#" + id + " .modal-title").html(title);
        }
    };
    this.show = function () {
		$('#' + this.modalId + ' .modal-content').css('height','auto');
        $('#' + this.modalId).modalFullscreen('show');
    };
    this.showWithPosition = function (target = undefined) {
        var _instance = this;
        $('#' + this.modalId + ' .modal-content').css('height', 'auto');
        $('#' + this.modalId).modalFullscreen('show');
        if (target && $(target)) {
            _instance.setPosition($(target)[0].getBoundingClientRect());
        }
    };
    this.hide = function () {
        $('#' + this.modalId).removeClass("in");
        $('#' + this.modalId).css("display", "none");
        $('.modal-backdrop').remove();
        $('#' + this.modalId).modalFullscreen('hide');
    };
    this.dismiss = function (callback = undefined) {
        $('#' + this.modalId).on('hidden.bs.modal', callback);
    };
    this.css = function (attribute,value) {
        $('#' + this.modalId).css(attribute, value);
    };
    this.setPosition = function (pos) {
        var _instance = this;
        _instance.left = _instance.left || 0;
        var options = _instance.options;
        var el = $("#" + _instance.modalId);
        var position;
        var actualWidth = el.width();
        var actualHeight = el.height();
        switch (options.placement) {
            case 'bottom':
                position = { top: pos.top + pos.height, left: pos.left + pos.width / 2 - actualWidth / 2 - _instance.left };
                break;
            case 'bottom bottom-left':
                position = { top: pos.top, left: pos.left };
                break;
            case 'bottom bottom-right':
                position = { top: pos.top + pos.height, left: pos.left - pos.width / 2 - actualWidth };
                break;
            case 'top':
                position = { top: pos.top - actualHeight, left: pos.left + pos.width / 2 - actualWidth / 2 };
                break;
            case 'top top-left':
                position = { top: pos.top - actualHeight, left: pos.left };
                break;
            case 'top top-right':
                position = { top: pos.top - actualHeight, left: pos.left + pos.width - actualWidth };
                break;
            case 'left':
                position = { top: pos.top + pos.height / 2 - actualHeight / 2, left: pos.left - actualWidth - _instance.left };
                break;
            case 'left left-top':
                position = { top: pos.top, left: pos.left - actualWidth };
                break;
            case 'left left-bottom':
                position = { top: pos.top + pos.height - actualHeight, left: pos.left - actualWidth };
                break;
            case 'right':
                position = { top: pos.top + pos.height / 2 - actualHeight / 2, left: pos.left + pos.width };
                break;
            case 'right right-top':
                position = { top: pos.top, left: pos.left + pos.width };
                break;
            case 'right right-bottom':
                position = { top: pos.top + pos.height - actualHeight, left: pos.left + pos.width };
                break;
            default:
                position = null;
        }
        if (position === undefined || position === null) {
            return;
        }
        el.css(position);
    }
    this.OnInit();
}