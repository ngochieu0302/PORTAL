function ModalDragService(modalId, title = undefined, placement = "bottom") {
    this.modalId = null;
    this.title = title;
    this.left = 0;
    this.target = null,
    this.options = {
        placement: placement
    }
    this.OnInit = function () {
        this.modalId = modalId;
        if (title !== undefined && $("#" + modalId + " .modal-drag-title") !== undefined) {
            $("#" + modalId + " .modal-drag-title").html(title);
        }
    };
    this.setTitle = function (title) {
        var id = this.modalId;
        if (title !== undefined && $("#" + id + " .modal-drag-title") !== undefined) {
            $("#" + id + " .modal-drag-title").html(title);
        }
    };
    this.show = function (target = undefined) {
        var _instance = this;
        _instance.target = target;
        $('#' + this.modalId).addClass("open");
        if (target && $(target)) {
            _instance.setPosition($(target)[0].getBoundingClientRect());
        }

    };
    this.hide = function (placement) {
        $('#' + this.modalId).removeClass("open");
    };
    this.setPlacement = function (placement) {
        var _instance = this;
        _instance.options.placement = placement;
    }
    this.setPosition = function (pos) {
        var _instance = this;
        var options = _instance.options;
        var el = $("#" + _instance.modalId);
        var position;
        var actualWidth = el.width();
        var actualHeight = el.height();
        switch (options.placement) {
            case 'bottom':
                position = { top: pos.top + pos.height, left: pos.left + pos.width / 2 - actualWidth / 2 };
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