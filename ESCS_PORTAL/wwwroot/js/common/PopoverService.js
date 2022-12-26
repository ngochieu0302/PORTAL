function PopoverService(id, title = undefined) {
    this.id = null;
    this.options = {};
    this.target = undefined;
    this.OnInit = function () {
        this.id = id;
        var _instance = $('#' + this.id);
        $('#' + this.id).popover({
            trigger: 'manual',
            placement: 'top',
            html: true,
            title: function () {
                return 'User Info: ' + $(this).data('title') + ' <a href="#" class="close" data-dismiss="alert">×</a>'
            },
            content: function () {
                return '<div class="media"><a href="#" class="pull-left"><img src=".." class="media-object" alt="Sample Image"></a><div class="media-body"><h4 class="media-heading">' + $(this).data('name') + '</h4><p>Excellent Bootstrap popover! I really love it.</p></div></div>'
            }
        });
        $(".close[data-dismiss='popover-x']").off("click").on('click', function () {
            _instance.hide();
        });
    };
    this.show = function (target = undefined, callback = undefined) {
        if (callback != undefined && !callback()) {
            return;
        }
        if (target != undefined) {
            this.target = target;
        }
        $('#' + this.id).show();
    };
    this.showWithPosition = function (target = undefined) {
        var _instance = this;
        _instance.target = target;
        if (target && $(target)) {
            _instance.setPosition($(target)[0].getBoundingClientRect());
            $('#' + _instance.id).show();
        }
    };
    this.setPosition = function (pos) {
        var _instance = this;
        var options = _instance.options;
        var el = $("#" + _instance.id);
        var position;
        var actualWidth = el.width();
        var actualHeight = el.height();
        switch (options.placement) {
            case 'custom':
                position = { top: options.position.top, left: options.position.left };
                break;
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
                position = { top: pos.top + pos.height / 2 - actualHeight / 2, left: pos.left - actualWidth };
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
        el.removeClass('bottom top left right bottom-left top-left bottom-right top-right ' +
            'left-bottom left-top right-bottom right-top').css(position);
    }
    this.hide = function () {
        $('#' + this.id).hide();
    };
    this.OnInit();
}