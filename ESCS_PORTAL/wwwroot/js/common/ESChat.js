function ESChat() {
    this.dot_symbol = "_ESCSDOTSYMBOL_"
    this.at_symbol = "_ESCSATSYMBOL_"
    this.current_show = false;
    this.option = null;
    this.loadContent = function () {
        var _instance = this;
        _instance.option.data.ma_gdv = _instance.option.data.ma_gdv.replace(/\./g, _instance.dot_symbol);
        _instance.option.data.ma_gdv = _instance.option.data.ma_gdv.replace(/\@/g, _instance.at_symbol);
        var parentId = "#ESChat_" + _instance.option.data.ma_doi_tac + "_" + _instance.option.data.ma_gdv;
        var content = "#ESChatContent_" + _instance.option.data.ma_doi_tac + "_" + _instance.option.data.ma_gdv;
        var contentName = "ESChatContent_" + _instance.option.data.ma_doi_tac + "_" + _instance.option.data.ma_gdv;
        $.ajax({
            cache: false,
            url: _instance.option.ajax_load.url,
            data: JSON.stringify({
                ma_doi_tac: _instance.option.data.ma_doi_tac,
                so_id: _instance.option.data.so_id,
                ma_dtac_nhan: _instance.option.data.ma_doi_tac,
                nsd_nhan: _instance.option.nsd_nhan_tmp,
                trang: _instance.option.page,
                so_dong: 100
            }),
            type: _instance.option.ajax_load.type,
            datatype: 'json',
            beforeSend: function () {

            },
            success: function (res) {
                if (res.state_info.status !== "OK") {
                    return;
                }
                if (res.data_info.data.length > 0) {
                    $(parentId + " " + content).html("");
                    var arr = res.data_info.data;
                    for (var i = 0; i < arr.length; i++) {
                        var thong_bao = arr[i];
                        thong_bao.thoi_gian = ESUtil.getTimeChat(thong_bao.tg_gui);
                        if (thong_bao.nguoi_gui === "send") {
                            ESUtil.appendHTML("templateTinNhanDi", contentName, thong_bao);
                        }
                        if (thong_bao.nguoi_gui === "receive") {
                            ESUtil.appendHTML("templateTinNhanDen", contentName, thong_bao);
                        }
                    }
                }
                if (_instance.option.ajax_load.success_callback) {
                    _instance.option.ajax_load.success_callback(res);
                }
                var objDiv = document.getElementById(contentName);
                objDiv.scrollTop = objDiv.scrollHeight;
                $(parentId).show();
            },
            error: function (err) {
            },
            complete: function () {

            }
        });
    }
    this.showHide = function (option = undefined) {
        var _instance = this;
        if (!option.is_show) {
            $(parentId).attr("hien_thi", false);
            $(parentId).hide();
            return;
        }
        _instance.option = option;
        _instance.option.nsd_nhan_tmp = option.data.ma_gdv;
        _instance.option.data.ma_gdv = _instance.option.data.ma_gdv.replace(/\./g, _instance.dot_symbol);
        _instance.option.data.ma_gdv = _instance.option.data.ma_gdv.replace(/\@/g, _instance.at_symbol);
        var parentId = "#ESChat_" + _instance.option.data.ma_doi_tac + "_" + _instance.option.data.ma_gdv;
        var content = "#ESChatContent_" + _instance.option.data.ma_doi_tac + "_" + _instance.option.data.ma_gdv;
        var contentName = "ESChatContent_" + _instance.option.data.ma_doi_tac + "_" + _instance.option.data.ma_gdv;
        _instance.id = parentId;
        if ($(parentId).length <= 0) {
            ESUtil.appendHTML("templateEsChat", "bodyEscs", { ma_doi_tac: _instance.option.data.ma_doi_tac, nsd: _instance.option.data.ma_gdv }, () => {
                _instance.loadContent();
                $(parentId + " .move-item").draggable();
                $(parentId + " .ESChat_close").click(function () {
                    $(parentId).attr("hien_thi",false);
                    $(parentId).hide();
                    $(parentId).remove();
                });
                $(parentId + " .ESChatSubmit").click(function () {
                    var nd_chat = $(parentId + " .ESChatNotify").val();
                    $(parentId + " .ESChatNotify").val("");
                    var message = {
                        ma_dtac_nhan: _instance.option.data.ma_doi_tac,
                        nsd_nhan: _instance.option.data.ma_gdv.replace(/\_ESCSDOTSYMBOL_/g, ".").replace(/\_ESCSATSYMBOL_/g, "@"),
                        nsd_nhan_ten: _instance.option.data.ten_gdv,
                        ma_doi_tac: _instance.option.data.ma_doi_tac,
                        so_id: _instance.option.data.so_id,
                        nd: nd_chat,
                        loai_nd: "TEXT",
                        thoi_gian: ESUtil.getHoursSysDate()
                    };
                    if ($(parentId + " .ESChatNoneContent")) {
                        $(parentId + " .ESChatNoneContent").remove();
                    }
                    ESUtil.appendHTML("templateTinNhanDi", contentName, message);

                    connection.invoke("sendMessage", JSON.stringify(message));
                    var objDiv = document.getElementById(contentName);
                    objDiv.scrollTop = objDiv.scrollHeight;
                });
                $(parentId + " .ESChatNotify").keypress(function (event) {
                    if (event.keyCode === 13) {
                        var nd_chat = $(parentId + " .ESChatNotify").val();
                        $(parentId + " .ESChatNotify").val("");
                        var message = {
                            ma_dtac_nhan: _instance.option.data.ma_doi_tac,
                            nsd_nhan: _instance.option.data.ma_gdv.replace(/\_ESCSDOTSYMBOL_/g, ".").replace(/\_ESCSATSYMBOL_/g, "@"),
                            nsd_nhan_ten: _instance.option.data.ten_gdv,
                            ma_doi_tac: _instance.option.data.ma_doi_tac,
                            so_id: _instance.option.data.so_id,
                            nd: nd_chat,
                            loai_nd: "TEXT",
                            thoi_gian: ESUtil.getHoursSysDate()
                        };
                        if ($(parentId + " .ESChatNoneContent")) {
                            $(parentId + " .ESChatNoneContent").remove();
                        }
                        ESUtil.appendHTML("templateTinNhanDi", contentName, message);
                        connection.invoke("sendMessage", JSON.stringify(message));
                        var objDiv = document.getElementById(contentName);
                        objDiv.scrollTop = objDiv.scrollHeight;
                    }
                });
                $(parentId + " .ESChatMinimize").click(function () {
                    $(parentId + " " + content).hide();
                    $(parentId + " .ESChatFooter").hide();
                    $(parentId + " .ESChatMinimize").hide();
                    $(parentId + " .ESChatMaximize").show();
                });
                $(parentId + " .ESChatMaximize").click(function () {
                    $(parentId + " " + content).show();
                    $(parentId + " .ESChatFooter").show();
                    $(parentId + " .ESChatMinimize").show();
                    $(parentId + " .ESChatMaximize").hide();
                });
            });
        }
        $(parentId + " .ESChatDisplayName").hide();
        $(parentId + " .ESChatAttachFile").hide();
        $(parentId + " .ESChatMinimize").show();
        $(parentId + " .ESChatMaximize").hide();

        $(parentId).attr("hien_thi", true);
        $(parentId + " " + content).show();
        $(parentId + " .ESChatFooter").show();
        //Gán giá trị vào hộp chát
        option.dispay_name = option.dispay_name || "Không có dữ liệu hiển thị";
        $(parentId + " .ESChatDisplayName").html(option.dispay_name);
        option.is_call_video = option.is_call_video || false;
        if (option.is_call_video)
            $(parentId + " .ESChatDisplayName").show();
        option.is_attach_file = option.is_attach_file || false;
        if (option.is_attach_file)
            $(parentId + " .ESChatAttachFile").show();

        var objDiv = document.getElementById(contentName);
        objDiv.scrollTop = objDiv.scrollHeight;
        $(parentId).show();
        
    }
}
