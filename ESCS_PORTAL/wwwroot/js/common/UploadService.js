function UploadService(configUpload = undefined) {
    this.id = "uploadFileEscsDropzone";
    this.previewNode = $("#" + this.id + " .dropzone-item");
    this.previewTemplate = null;
    this.accept = "";
    this.config = {};
    this.data = {};
    this.callback = function (response) {
        if(response.out_value != undefined && response.out_value != null &&
            response.out_value.action != undefined && response.out_value.action != null &&
            response.out_value.action == "ATTACH_FILE") {
            if ($("#modalSendEmailCommonDinhKemFile").length > 0) {
                var objNew = { id: response.out_value.file_id, ten: response.out_value.file_name };
                var file_id = $("#modalSendEmailCommonDinhKemFile").attr("data-file-id");
                var file_name = $("#modalSendEmailCommonDinhKemFile").attr("data-file-name");
                if (!file_id) {
                    file_id = "";
                    file_name = "";
                }
                var arrFileId = file_id.split(",").where(n => n != "");
                var arrFileName = file_name.split(",").where(n => n != "");
                var arr = [];
                if (arrFileId.length > 0) {
                    for (var i = 0; i < arrFileId.length; i++) {
                        var obj = { id: "", ten: "" };
                        if (arrFileId[i])
                            obj.id = arrFileId[i];
                        if (arrFileName[i])
                            obj.ten = arrFileName[i];
                        arr.push(obj)
                    }
                    arr.push(objNew);
                }
                else {
                    arr.push(objNew);
                }

                file_id = "";
                file_name = "";
                for (var i = 0; i < arr.length; i++) {
                    if (i == 0) {
                        file_id = arr[i].id;
                        file_name = arr[i].ten;
                    }
                    else {
                        file_id += "," + arr[i].id;
                        file_name += "," + arr[i].ten;
                    }
                }
                var so_file = arr.length;
                $("#modalSendEmailCommonDinhKemFile").html("File đính kèm thêm(" + so_file+")");
                $("#modalSendEmailCommonDinhKemFile").attr("data-file-id", file_id);
                $("#modalSendEmailCommonDinhKemFile").attr("data-file-name", file_name);
                ESUtil.genHTML("modalSendEmailCommonFileAttachTableTemplate", "modalSendEmailCommonFileAttachTable", { data: arr });
            }
        }
    }
    this.instanceDropzone = null;
    this.setParam = function (data, accept="") {
        this.data = data;
        this.accept = accept;
        var _instance = this;
        if (_instance.instanceDropzone && _instance.instanceDropzone != null) {
            if (this.data.url_api == undefined || this.data.url_api == null || this.data.url_api == "") 
                _instance.instanceDropzone.options.url = '/upload/uploadfile';
            else 
                _instance.instanceDropzone.options.url = this.data.url_api;
        }
        
    }
    var _item = this;
    this.OnInit = function () {
        var _instance = this;
        this.config = configUpload;
        if (this.config === undefined) {
            this.config = {};
        }
        
        this.config.url = '/upload/uploadfile';
        var config = this.config;

        var parallelUploads = 50;
        var maxFilesize = 50;

        var id = "#"+this.id;
        this.previewNode.id = "";
        this.previewTemplate = this.previewNode.parent('.dropzone-items').html();
        this.previewNode.remove();
        var acceptedFiles = this.accept;
        this.instanceDropzone = new Dropzone(id, {
            url: config.url,
            parallelUploads: parallelUploads,
            previewTemplate: this.previewTemplate,
            maxFilesize: maxFilesize,
            autoQueue: false,
            previewsContainer: id + " .dropzone-items",
            clickable: id + " .dropzone-select",
            resizeWidth: 1800,
            resizeMethod: 'contain',
            resizeQuality: 1.0,
            acceptedFiles: ".jpg, .png, .jpeg, .gif, .svg, .doc, .docx, .pdf, .xls, .xlsx, .xml"//acceptedFiles
        });
        $("#uploadConfigFileSize").html(maxFilesize);
        $("#uploadConfigFile").html(parallelUploads);
        this.instanceDropzone.on("addedfile", function (file) {
            file.previewElement.querySelector(id + " .dropzone-start").onclick = function () { _instance.instanceDropzone.enqueueFile(file); };
            $(document).find(id + ' .dropzone-item').css('display', '');
            $(id + " .dropzone-upload, " + id + " .dropzone-remove-all").css('display', 'inline-block');
            if (config.onAddFile) {
                config.onAddFile(file);
            }
        });
        this.instanceDropzone.on("totaluploadprogress", function (progress) {
            $(this).find(id + " .progress-bar").css('width', progress + "%");
        });
        this.instanceDropzone.on("sending", function (file, xhr, formData) {
            for (var pro in _item.data) {
                formData.append(pro, _item.data[pro]);
            }
            $(id + " .progress-bar").css('opacity', '1');
            file.previewElement.querySelector(id + " .dropzone-start").setAttribute("disabled", "disabled");
            if (config.onSending) {
                config.onSending(file);
            }
        });
        this.instanceDropzone.on("complete", function (progress) {
            var thisProgressBar = id + " .dz-complete";
            setTimeout(function () {
                $(thisProgressBar + " .progress-bar, " + thisProgressBar + " .progress, " + thisProgressBar + " .dropzone-start").css('opacity', '0');
            }, 300)
            if (config.onComplete) {
                config.onComplete(progress);
            }
        });
        this.instanceDropzone.on("success", function (file, response) {
            if (config.onSuccess) {
                config.onSuccess(file, response, _item.data);
            }
        });
        this.instanceDropzone.on("error", function (file, response) {
            if (config.onError) {
                config.onError(file, response);
            }
        });
        this.instanceDropzone.on("queuecomplete", function (progress) {
            $(id + " .dropzone-upload").css('display', 'none');
            if (config.onAllComplete) {
                config.onAllComplete(progress);
            }
        });

        this.instanceDropzone.on("removedfile", function (file) {
            if (_instance.instanceDropzone.files.length < 1) {
                $(id + " .dropzone-upload, " + id + " .dropzone-remove-all").css('display', 'none');
                if (config.onRemoveFile) {
                    config.onRemoveFile(file);
                }
            }
        });
        $("#btnUploadAllFile").unbind('click').bind('click', function () {
            _instance.instanceDropzone.enqueueFiles(_instance.instanceDropzone.getFilesWithStatus(Dropzone.ADDED));
        });
        $("#btnUploadAllFileAndClose").unbind('click').bind('click', function () {
            _instance.instanceDropzone.enqueueFiles(_instance.instanceDropzone.getFilesWithStatus(Dropzone.ADDED));
            $("#modalEscsUploadFile").modal("hide");
        });
        $("#btnCancelUpload").unbind('click').bind('click', function () {
            $(id + " .dropzone-upload, " + id + " .dropzone-remove-all").css('display', 'none');
            _instance.instanceDropzone.removeAllFiles(true);
        });
    }
    this.showPupup = function () {
        var _instance = this;
        Dropzone.forElement("#" + _instance.id).removeAllFiles(true);
        $("#modalEscsUploadFile").modal("show");
    }
    this.hidePupup = function () {
        $("#modalEscsUploadFile").modal("hide");
    }
    this.OnInit();
}