function ESBoxDrag(id,option=undefined) {
    this.id = id
    this.option = {
        width: "500px",
        height: "350px",
        top: "50%",
        left: "50%"
    };
    this.OnInit = function () {
        var _instance = this;
        if (option) {
            _instance.option.width = option.width == undefined ? "500px" : option.width;
            _instance.option.height = option.height == undefined ? "350px" : option.height;
            _instance.option.top = option.top == undefined ? "50%" : option.top;
            _instance.option.left = option.left == undefined ? "50%" : option.left;
        }
        $("#" + _instance.id).css("position", "absolute");
        $("#" + _instance.id).css("width", _instance.option.width);
        $("#" + _instance.id).css("z-index", "9999999");
        $("#" + _instance.id).css("border", "solid 3px #1e88e5");
        $("#" + _instance.id).css("border-radius", "0.85rem");
        $("#" + _instance.id).css("top", _instance.option.top);
        $("#" + _instance.id).css("left", _instance.option.left);
        $("#" + _instance.id).css("background-color", "#fff");

        $("#" + _instance.id + " .modal-drag-header").css("cursor", "pointer");


        $("#" + _instance.id + " .modal-drag-footer").css("width", "100%");
        $("#" + _instance.id + " .modal-drag-footer").css("position", "absolute");
        $("#" + _instance.id + " .modal-drag-footer").css("bottom", "0");
        $("#" + _instance.id + " .modal-drag-footer").css("text-align", "right");
        $("#" + _instance.id + " .modal-drag-footer").css("padding", "5px");
        $("#" + _instance.id + " .modal-drag-close").click(function () {
            $("#" + _instance.id).hide();
        });
        _instance.dragElement(_instance.id);
        $("#" + _instance.id).hide();
    }
    this.show = function () {
        var _instance = this;
        $("#" + _instance.id).show();
    }
    this.dragElement = function () {
        var _instance = this;
        var elmnt = document.getElementById(_instance.id);
        var pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;
        if (document.getElementById(elmnt.id + "Header")) {
            document.getElementById(elmnt.id + "Header").onmousedown = dragMouseDown;
        }
        else {
            elmnt.onmousedown = dragMouseDown;
        }
        function dragMouseDown(e) {
            e = e || window.event;
            e.preventDefault();
            // get the mouse cursor position at startup:
            pos3 = e.clientX;
            pos4 = e.clientY;
            document.onmouseup = closeDragElement;
            // call a function whenever the cursor moves:
            document.onmousemove = elementDrag;
        }
        function elementDrag(e) {
            e = e || window.event;
            e.preventDefault();
            // calculate the new cursor position:
            pos1 = pos3 - e.clientX;
            pos2 = pos4 - e.clientY;
            pos3 = e.clientX;
            pos4 = e.clientY;
            // set the element's new position:
            elmnt.style.top = (elmnt.offsetTop - pos2) + "px";
            elmnt.style.left = (elmnt.offsetLeft - pos1) + "px";
        }
        function closeDragElement() {
            // stop moving when mouse button is released:
            document.onmouseup = null;
            document.onmousemove = null;
        }
    }
    this.OnInit();
}
