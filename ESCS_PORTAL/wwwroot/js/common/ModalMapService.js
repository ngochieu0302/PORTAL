function ModalMapService(modalId, fullScreenId = undefined, title = undefined) {
    this.styles = {
        default: [],
        hide: [
            {
                featureType: "poi",
                stylers: [{ visibility: "off" }],
            },
            {
                featureType: "transit",
                elementType: "labels.icon",
                stylers: [{ visibility: "off" }],
            },
        ],
    };
    this.modalId = null;
    this.title = title;
    this.fullScreenId = fullScreenId;
    this.OnInit = function () {
        this.modalId = modalId;
        if (title !== undefined && $("#" + modalId + " .modal-title") !== undefined) {
            $("#" + modalId + " .modal-title").html(title);
        }
        if (fullScreenId !== undefined) {
            $("#" + modalId).data('fullscreen', fullScreenId);
        } 
    };
    this.setTitle = function (title) {
        var id = this.modalId;
        if (title !== undefined && $("#" + id + " .modal-title") !== undefined) {
            $("#" + id + " .modal-title").html(title);
        }
    };
    this.show = function () {
        $('#' + this.modalId + ' .modal-content').css('height', 'auto');
        $('#' + this.modalId).modalFullscreen('show');
    };
    this.hide = function () {
        //$('#' + this.modalId).removeClass("in");
        //$('#' + this.modalId).css("display", "none");
        //$('.modal-backdrop').remove();
        //$('#' + this.modalId).modal('hide');

        $('#' + this.modalId).removeClass("in");
        $('#' + this.modalId).css("display", "none");
        $('.modal-backdrop').remove();
        $('#' + this.modalId).modalFullscreen('hide');
    };
    this.dismiss = function (callback = undefined) {
        $('#' + this.modalId).on('hidden.bs.modal', callback);
    };
    this.css = function (attribute, value) {
        $('#' + this.modalId).css(attribute, value);
    };

    this.hienThiMapTheoToaDo = function (lat, lng, ten_vi_tri = "") {
        var key = $("#escs_dv_google").val();
        if (key == undefined || key == null || key == "") {
            new NotifyService().error("Chưa đăng ký sử dụng dịch vụ google api");
            return;
        }
        if (lat == 0 && lng==0) {
            var _notifyService = new NotifyService();
            _notifyService.error("Không xác định được vị trí");
            return;
        }
        lat = parseFloat(lat);
        lng = parseFloat(lng);
        var _instance = this;
        var map = new google.maps.Map(document.getElementById("modalMap_map"), {
            center: { lat: lat, lng: lng },
            zoom: 15,
        });
        map.setOptions({ styles: _instance.styles["hide"] });
        const myLatLng = { lat: lat, lng: lng };
        new google.maps.Marker({
            position: myLatLng,
            map,
            title: ten_vi_tri,
        });
        _instance.show();
    };
    this.layToaDo = function (dia_chi, callback = undefined) {
        var toa_do = { lat: 0, lng: 0 };
        var key = $("#escs_dv_google").val();
        if (key == undefined || key == null || key == "") {
            if (callback) {
                callback(toa_do);
            }
            return;
        }
        $.ajax({
            url: "https://maps.googleapis.com/maps/api/geocode/json?address=" + dia_chi + "&key=" + key,
            type: "get",
            cache: false,
            datatype: 'json',
            success: function (res) {
                if (res.status != "OK" || res.results == undefined || res.results == null || res.results.length <= 0) {
                    if (callback) {
                        callback(toa_do);
                    }
                    return;
                }
                toa_do.lat = res.results[0].geometry.location.lat;
                toa_do.lng = res.results[0].geometry.location.lng;
                if (callback) {
                    callback(toa_do);
                }
            }
        });
    }
    this.hienThiMapTheoDiaChi = function (dia_chi, ten_vi_tri = "") {
        var key = $("#escs_dv_google").val();
        if (key == undefined || key == null || key == "") {
            new NotifyService().error("Chưa đăng ký sử dụng dịch vụ google api");
            return;
        }
        var _instance = this;
        $.ajax({
            url: "https://maps.googleapis.com/maps/api/geocode/json?address=" + dia_chi + "&key=" + key,
            type: "get",
            cache: false,
            datatype: 'json',
            success: function (res) {
                if (res.status != "OK" || res.results == undefined || res.results == null|| res.results.length <= 0) {
                    var _notifyService = new NotifyService();
                    _notifyService.error("Không xác định được vị trí");
                    return;
                }
                var lat = res.results[0].geometry.location.lat;
                var lng = res.results[0].geometry.location.lng;
                var map = new google.maps.Map(document.getElementById("modalMap_map"), {
                    center: { lat: lat, lng: lng },
                    zoom: 15,
                });
                map.setOptions({ styles: _instance.styles["hide"] });
                const myLatLng = { lat: lat, lng: lng };
                new google.maps.Marker({
                    position: myLatLng,
                    map,
                    title: ten_vi_tri,
                });
                _instance.show();
            }
        })

    }
    //a.lattitude, a.longitude
    this.hienThiViTriGDVHT = function (arr_vi_tri, dia_chi, eventMarKerClick = undefined) {
        var key = $("#escs_dv_google").val();
        if (key == undefined || key == null || key == "") {
            new NotifyService().error("Chưa đăng ký sử dụng dịch vụ google api");
            return;
        }

        var _instance = this;
        $.ajax({
            url: "https://maps.googleapis.com/maps/api/geocode/json?address=" + dia_chi + "&key=" + key,
            type: "get",
            cache: false,
            datatype: 'json',
            success: function (res) {
                if (res.status != "OK" || res.results == undefined || res.results == null || res.results.length <= 0) {
                    var _notifyService = new NotifyService();
                    _notifyService.error("Không xác định được vị trí");
                    return;
                }
                var center = res.results[0].geometry.location;
                var map = new google.maps.Map(document.getElementById("modalMap_map"), {
                    center: center,
                    zoom: 14,
                });
                map.setOptions({ styles: _instance.styles["hide"] });

                for (var i = 0; i < arr_vi_tri.length; i++) {
                    var vi_tri = arr_vi_tri[i];
                    console.log(vi_tri);
                    var vi_tri_LatLng = new google.maps.LatLng(parseFloat(vi_tri.latitude), parseFloat(vi_tri.longitude));
                    var marker = new google.maps.Marker({
                        position: vi_tri_LatLng,
                        map: map,
                        title: vi_tri.ten,
                        icon: "/images/icon_location/phonegif.gif",
                        ma_doi_tac: vi_tri.ma_doi_tac,
                        ma_chi_nhanh: vi_tri.ma_chi_nhanh,
                        gdv: vi_tri.nsd
                    });
                    if (eventMarKerClick) {
                        new google.maps.event.addListener(marker, 'click', function () {
                            var ma_chi_nhanh = this.ma_chi_nhanh;
                            var gdv = this.gdv;
                            eventMarKerClick(ma_chi_nhanh, gdv)
                        });
                    }
                }
                var vi_tri_ton_that = new google.maps.LatLng(parseFloat(center.lat), parseFloat(center.lng));
                new google.maps.Marker({ position: vi_tri_ton_that, map: map, title: "Địa điểm giám định", icon: "/images/icon_location/cargif.gif"});
                _instance.show();
            }
        })
        _instance.show();
    }
    this.hienThiDanDuong = function (vi_tri_1, vi_tri_2)
    {
        var key = $("#escs_dv_google").val();
        if (key == undefined || key == null || key == "") {
            new NotifyService().error("Chưa đăng ký sử dụng dịch vụ google api");
            return;
        }

        var vi_tri_1_LatLng = new google.maps.LatLng(vi_tri_1.latitude, vi_tri_1.longitude);
        var vi_tri_2_LatLng = new google.maps.LatLng(vi_tri_2.latitude, vi_tri_2.longitude);
        var lat_lng = new Array();
        lat_lng.push(vi_tri_1_LatLng);
        lat_lng.push(vi_tri_2_LatLng);
        var map = new google.maps.Map(document.getElementById("modalMap_map"));
        map.setOptions({ styles: _instance.styles["hide"] });

        var latlngbounds = new google.maps.LatLngBounds();
        var marker1 = new google.maps.Marker({position: vi_tri_1_LatLng, map: map, title: vi_tri_1.title, label:"A" }); 
		var marker2 = new google.maps.Marker({position: vi_tri_2_LatLng, map: map, title: vi_tri_2.title, label:"B" });
        latlngbounds.extend(marker1.position);
        latlngbounds.extend(marker2.position);
        map.setCenter(latlngbounds.getCenter());
        map.fitBounds(latlngbounds);
        var path = new google.maps.MVCArray();
        var service = new google.maps.DirectionsService();
        var poly = new google.maps.Polyline({ map: map, strokeColor: '#4986E7' });

        for (var i = 0; i < lat_lng.length; i++) {
            if ((i + 1) < lat_lng.length) {
                var src = lat_lng[i];
                var des = lat_lng[i + 1];
                path.push(src);
                poly.setPath(path);
                service.route({
                    origin: src,
                    destination: des,
                    travelMode: google.maps.DirectionsTravelMode.DRIVING
                }, function (result, status) {
                    if (status == google.maps.DirectionsStatus.OK) {
                        for (var i = 0, len = result.routes[0].overview_path.length; i < len; i++) {
                            path.push(result.routes[0].overview_path[i]);
                        }
                    } else {
                        alert("Invalid location.");
                        window.location.href = window.location.href;
                    }
                });
            }
        }
    }
    this.OnInit();
}