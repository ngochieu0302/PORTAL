function ModalDocumentService(title = "In ấn (Chọn 1 dòng để in)") {
    this.tabActive = "";
    this.source = [];
    //this._instance = this;
    this.onClickIem;
    this.setDataSource = function (arrData) {
        this.source = arrData;
    }
    this.show = function (mau_in = "") {
        var _instance = this;
        var arrData = this.source;
        var fn = this.onClickIem;
        ESUtil.genHTML("templateModalDocumentTab", "modalDocumentTabs", { source: arrData });
        ESUtil.genHTML("templateModalDocumentContent", "modalDocumentContents", { source: arrData });
        $(".modalDocumentTabItem").click(function () {
            $('#modalDocumentContents').parent().addClass('show_loading');
            var id = $(this).attr("document-id");
            if (fn) {
                _instance.tabActive = id;
                fn(id);
            }
        });
        if (mau_in != "") {
            $("#modalDocumentTabItem_" + mau_in).trigger("click");
        }
        $("#modalDocumentTitle").html(title);
        $("#modalDocument").modal('show');
        $('#modalDocument').on('hidden.bs.modal', function (e) {
            $('#modalDocumentContents').parent().removeClass('show_loading');
        });
    }
    this.viewFile = function (base64) {
        var _instance = this;
        if (_instance.tabActive !== undefined && _instance.tabActive !== null && _instance.tabActive !== "") {
            PDFObject.embed("data:application/pdf;base64," + base64,
                "#modalDocumentTabContent_" + _instance.tabActive, {
                pdfOpenParams: {
                    //view: "fit", toolbar: 0, navpanes: 0
                    navpanes: 1,
                    statusbar: 0,
                    toolbar: 1,
                    view: "FitH",
                    pagemode: "bookmarks"
                }
            }
            );
        }
    }

}