function ModalService(modalId, title = undefined) {
    this.modalId = null;
    this.title = title;
    this.target = null;
    this.OnInit = function () {
        this.modalId = modalId; 
        if (title !== undefined && $("#" + modalId+" .modal-title") !== undefined) {
            $("#" + modalId +" .modal-title").html(title);
        }     
        
    };
    this.setTitle = function (title) {
        var id = this.modalId;
        if (title !== undefined && $("#" + id +" .modal-title") !== undefined) {
            $("#" + id + " .modal-title").html(title);
        }
    };
    this.show = function (target = undefined) {
        this.target = target;
        $('#' + this.modalId).modal('show');
    };
    this.hide = function () {
        $('#' + this.modalId).removeClass("in");
        $('#' + this.modalId).css("display", "none");
        $('.modal-backdrop').remove();
        $('#' + this.modalId).modal('hide');
    };
    this.dismiss = function (callback = undefined) {
        $('#' + this.modalId).on('hidden.bs.modal', callback);
    };
    this.css = function (attribute,value) {
        $('#' + this.modalId).css(attribute, value);
    };

    this.OnInit();
}