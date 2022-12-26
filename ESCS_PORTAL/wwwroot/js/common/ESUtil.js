"use strict";
var ESUtil = function () {
    /** @type {object} breakpoints The device width breakpoints **/
    var breakpoints = {
        sm: 544, // Small screen / phone
        md: 768, // Medium screen / tablet
        lg: 992, // Large screen / desktop
        xl: 1200 // Extra large screen / wide desktop
    };

    const TRANSITION_END = 'transitionend';
    const MAX_UID = 1000000;
    const MILLISECONDS_MULTIPLIER = 1000;

    return {
        popupCenter: ({ url, title, w, h }) => {
            // Fixes dual-screen position                             Most browsers      Firefox
            const dualScreenLeft = window.screenLeft !== undefined ? window.screenLeft : window.screenX;
            const dualScreenTop = window.screenTop !== undefined ? window.screenTop : window.screenY;

            const width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            const height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

            const systemZoom = width / window.screen.availWidth;
            const left = (width - w) / 2 / systemZoom + dualScreenLeft
            const top = (height - h) / 2 / systemZoom + dualScreenTop
            const newWindow = window.open(url, title,
                `
              scrollbars=yes,
              width=${w / systemZoom}, 
              height=${h / systemZoom}, 
              top=${top}, 
              left=${left}
              `
            )
            if (window.focus) newWindow.focus();
        },
        executeAsync: function (func) {
            setTimeout(func, 0);
        },
        executeWithTimeAsync: function (func, time) {
            setTimeout(func, time);
        },
        anHienControl: function (arrTrangThai, obj, ma_trang_thai = undefined) {
            $(".escs_pquyen").addClass("d-none");
            if (arrTrangThai === undefined || arrTrangThai === null || arrTrangThai.length <= 0) {
                return;
            }
            var ma_trang_thai_tmp = JSON.parse(JSON.stringify(obj.ma_trang_thai));;
            if (ma_trang_thai != undefined) {
                ma_trang_thai_tmp = ma_trang_thai;
            }
            var tthai = arrTrangThai.where(n => n.ma_doi_tac === obj.ma_doi_tac && n.ma_trang_thai === ma_trang_thai_tmp).firstOrDefault();
            if (tthai === undefined || tthai === null || tthai.btn_hien === "") {
                return;
            }
            var arrControl;
            if (tthai.btn_hien !== null && tthai.btn_hien !== "") {
                arrControl = tthai.btn_hien.split(',');
                for (var i = 0; i < arrControl.length; i++) {
                    if ($("#" + arrControl[i].trim()).hasClass("escs_pquyen")) {
                        $("#" + arrControl[i].trim()).removeClass("d-none");
                    }
                }
            }
            else {
                if (tthai.btn_an != null) {
                    arrControl = tthai.btn_an.split(',');
                    $(".escs_pquyen").removeClass("d-none");
                    for (var i = 0; i < arrControl.length; i++) {
                        if ($("#" + arrControl[i].trim()).hasClass("escs_pquyen")) {
                            $("#" + arrControl[i].trim()).addClass("d-none");
                        }
                    }
                }
            }
        },
        anHienDashboard: function (arrDashboard) {
            $(".pquyen_dashboard").addClass("d-none");
            for (let i = 1; i <= 12; i++) {
                $(".pquyen_dashboard").removeClass("order-" + i);
            }
            if (arrDashboard === undefined || arrDashboard === null || arrDashboard.length <= 0) {
                return;
            }
            var stt = {};
            arrDashboard.forEach(function (cfg) {
                var btn_hien = cfg.btn_hien;
                stt[btn_hien] = cfg.stt;
                if (cfg.stt > 0) {
                    if ($("h4[data-id='" + btn_hien + "']").hasClass("pquyen_dashboard")) {
                        $("h4[data-id='" + btn_hien + "']").removeClass("d-none");
                        if (cfg.stt > 0 && cfg.stt <= 12) {
                            $("h4[data-id='" + btn_hien + "']").addClass("order-" + cfg.stt);
                        }
                    }
                }
            });
            if ($('.current-dashboard').hasClass('d-none')) {
                window.location.href = ((stt.dashboard_ng < 1 ? 9 : stt.dashboard_ng) < (stt.dashboard_ttgq < 1 ? 9 : stt.dashboard_ttgq)) ? '/home/IndexH' : '/home/IndexTTGQ';
            }
        },
        dataAutoComplete: [],
        autoComplete: function (inp, arr, valueMember, displayMember, top = 10, maxHeight = undefined, callback = undefined) {
            var currentFocus;
            ESUtil.dataAutoComplete = arr;
            var eventClick = inp.getAttribute("data-event-click");
            if (eventClick == undefined || eventClick == null) {
                inp.addEventListener("click", function (e) {
                    console.log("autoComplete click");
                    setTimeout(() => {
                        var a, b, i, val = this.value;
                        if (val !== undefined && val !== null && val.trim() !== "") {
                            return;
                        }
                        var arrTop = ESUtil.dataAutoComplete.slice(0, top);
                        closeAllLists();
                        currentFocus = -1;
                        a = document.createElement("DIV");
                        a.setAttribute("id", this.id + "autocomplete-list");
                        a.setAttribute("class", "autocomplete-items scrollable");
                        a.style.zIndex = "999999";
                        if (maxHeight !== undefined) {
                            a.style.maxHeight = maxHeight;
                        }
                        this.parentNode.appendChild(a);
                        var isEmpty = true;
                        for (i = 0; i < arrTop.length; i++) {
                            b = document.createElement("DIV");
                            b.innerHTML = "<strong style='font-weight: bold;'>" + arrTop[i][displayMember].substr(0, val.length) + "</strong>";
                            b.innerHTML += arrTop[i][displayMember].substr(val.length);
                            b.innerHTML += "<input type='hidden' auto-complete-val='" + arrTop[i][valueMember] + "' value='" + arrTop[i][displayMember] + "'>";
                            b.addEventListener("click", function (e) {
                                inp.value = this.getElementsByTagName("input")[0].value;
                                var val = this.getElementsByTagName("input")[0].getAttribute("auto-complete-val");
                                inp.setAttribute("complete-val", val);
                                closeAllLists();
                                if (callback) {
                                    callback(val);
                                }
                            });
                            a.appendChild(b);
                            isEmpty = false;
                        }
                        if (isEmpty) {
                            closeAllLists();
                        }
                    }, 100);
                });
                inp.setAttribute("data-event-click","added");
            }
            
            inp.addEventListener("input", function (e) {
                setTimeout(() => {
                    var a, b, i, val = this.value;
                    closeAllLists();
                    if (!val) {
                        return false;
                    }
                    currentFocus = -1;
                    a = document.createElement("DIV");
                    a.setAttribute("id", this.id + "autocomplete-list");
                    a.setAttribute("class", "autocomplete-items scrollable");
                    a.style.zIndex = "999999";
                    if (maxHeight !== undefined) {
                        a.style.maxHeight = maxHeight;
                    }
                    this.parentNode.appendChild(a);
                    var isEmpty = true;
                    var arrTmp = [];
                    var textSearch = "%"+val.replace(/ /g, '%')+"%";
                    for (var i = 0; i < arr.length; i++) {
                        var check = arr[i][displayMember].toUpperCase().like(textSearch.toUpperCase());
                        if (check) {
                            var objSearch = arr[i];
                            var arrKyTu = val.toUpperCase().split(' ').where(n => n != " ");
                            objSearch.orderTmp = [];
                            for (var j = 0; j < arrKyTu.length; j++) {
                                objSearch.orderTmp.push(arr[i][displayMember].toUpperCase().indexLike(arrKyTu[j].toUpperCase()));
                            }
                            arrTmp.push(objSearch);
                        }
                    }
                    arrTmp.sort(compareAutoComplete);
                    for (var i = 0; i < arrTmp.length; i++) {
                        b = document.createElement("DIV");
                        b.innerHTML = arrTmp[i][displayMember];
                        b.innerHTML += "<input type='hidden' auto-complete-val='" + arrTmp[i][valueMember] + "' value='" + arrTmp[i][displayMember] + "'>";
                        b.addEventListener("click", function (e) {
                            inp.value = this.getElementsByTagName("input")[0].value;
                            var val = this.getElementsByTagName("input")[0].getAttribute("auto-complete-val");
                            inp.setAttribute("complete-val", val);
                            closeAllLists();
                            if (callback) {
                                callback(val);
                            }
                        });
                        a.appendChild(b);
                        isEmpty = false;
                    }
                    //for (i = 0; i < arr.length; i++) {
                    //    if (arr[i][displayMember].toUpperCase().indexOf(val.toUpperCase()) != -1) {
                    //        b = document.createElement("DIV");
                    //        b.innerHTML = arr[i][displayMember];
                    //        b.innerHTML += "<input type='hidden' auto-complete-val='" + arr[i][valueMember] + "' value='" + arr[i][displayMember] + "'>";
                    //        b.addEventListener("click", function (e) {
                    //            inp.value = this.getElementsByTagName("input")[0].value;
                    //            var val = this.getElementsByTagName("input")[0].getAttribute("auto-complete-val");
                    //            inp.setAttribute("complete-val", val);

                    //            closeAllLists();
                    //            if (callback) {
                    //                callback(val);
                    //            }
                    //        });
                    //        a.appendChild(b);
                    //        isEmpty = false;
                    //    }
                    //}
                    if (isEmpty) {
                        closeAllLists();
                    }
                }, 1000);

            });
            inp.addEventListener("keydown", function (e) {
                var x = document.getElementById(this.id + "autocomplete-list");
                if (x) x = x.getElementsByTagName("div");
                if (e.keyCode === 40) {
                    currentFocus++;
                    addActive(x);
                } else if (e.keyCode === 38) {
                    currentFocus--;
                    addActive(x);
                } else if (e.keyCode === 13) {
                    e.preventDefault();
                    if (currentFocus > -1) {
                        if (x) x[currentFocus].click();
                    }
                }
            });
            
            function compareAutoComplete(a, b) {
                var count = a.orderTmp.length;
                var bang_tat_ca = 0;
                for (var i = 0; i < count; i++) {
                    if ((a.orderTmp[i] - b.orderTmp[i]) > 0) {
                        return 1
                    }
                    if (a.orderTmp[i] == b.orderTmp[i]) {
                        bang_tat_ca = bang_tat_ca + 1;
                    }
                    if ((a.orderTmp[i] - b.orderTmp[i]) < 0) {
                        return -1
                    }
                }
                if (bang_tat_ca == count) {
                    return 0;
                }
                return 1;
            }
            function addActive(x) {
                if (!x) return false;
                removeActive(x);
                if (currentFocus >= x.length) currentFocus = 0;
                if (currentFocus < 0) currentFocus = (x.length - 1);
                x[currentFocus].classList.add("autocomplete-active");
            }
            function removeActive(x) {
                for (var i = 0; i < x.length; i++) {
                    x[i].classList.remove("autocomplete-active");
                }
            }
            function closeAllLists(elmnt) {
                var x = document.getElementsByClassName("autocomplete-items");
                for (var i = 0; i < x.length; i++) {
                    if (elmnt != x[i] && elmnt != inp) {
                        x[i].parentNode.removeChild(x[i]);
                    }
                }
            }
            document.addEventListener("click", function (e) {
                closeAllLists(e.target);
            });
        },
        cloneObject: function (objClone) {
            return Object.assign({}, objClone);
        },
        formatMoney: function (value) {
            if (value == undefined) {
                return 0;
            }
            if (isNaN(parseInt(value)))
                return 0;
            return value.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        },
        /**
         * Get GET parameter value from URL.
         * @param {string} paramName Parameter name.
         * @returns {string}
         */
        getURLParam: function (paramName) {
            var searchString = window.location.search.substring(1),
                i, val, params = searchString.split("&");
            for (i = 0; i < params.length; i++) {
                val = params[i].split("=");
                if (val[0] == paramName) {
                    return unescape(val[1]);
                }
            }
            return null;
        },
        /**
         * Get an image name from an image url.
         * @param {string} url - The target url.
         * @example
         * // picture.jpg
         * getImageNameFromURL('https://domain.com/path/to/picture.jpg?size=1280×960')
         * @returns {string} A string contains the image name.
         */
        getImageNameFromURL: function (url) {
            return isString(url) ? decodeURIComponent(url.replace(/^.*\//, '').replace(/[?&#].*$/, '')) : '';
        },

        /**
         * Checks whether current device is mobile touch.
         * @returns {boolean}
         */
        isMobileDevice: function () {
            var test = (this.getViewPort().width < this.getBreakpoint('lg') ? true : false);
            if (test === false) {
                // For use within normal web clients
                test = navigator.userAgent.match(/iPad/i) != null;
            }
            return test;
        },

        /**
         * Checks whether current device is desktop.
         * @returns {boolean}
         */
        isDesktopDevice: function () {
            return this.isMobileDevice() ? false : true;
        },

        /**
         * Gets browser window viewport size. Ref:
         * http://andylangton.co.uk/articles/javascript/get-viewport-size-javascript/
         * @returns {object}
         */
        getViewPort: function () {
            var e = window,
                a = 'inner';
            if (!('innerWidth' in window)) {
                a = 'client';
                e = document.documentElement || document.body;
            }
            return {
                width: e[a + 'Width'],
                height: e[a + 'Height']
            };
        },

        /**
         * Checks whether given device mode is currently activated.
         * @param {string} mode Responsive mode name(e.g: desktop,
         *     desktop-and-tablet, tablet, tablet-and-mobile, mobile)
         * @returns {boolean}
         */
        isInResponsiveRange: function (mode) {
            var breakpoint = this.getViewPort().width;
            if (mode == 'general') {
                return true;
            } else if (mode == 'desktop' && breakpoint >= (this.getBreakpoint('lg') + 1)) {
                return true;
            } else if (mode == 'tablet' && (breakpoint >= (this.getBreakpoint('md') + 1) && breakpoint < this.getBreakpoint('lg'))) {
                return true;
            } else if (mode == 'mobile' && breakpoint <= this.getBreakpoint('md')) {
                return true;
            } else if (mode == 'desktop-and-tablet' && breakpoint >= (this.getBreakpoint('md') + 1)) {
                return true;
            } else if (mode == 'tablet-and-mobile' && breakpoint <= this.getBreakpoint('lg')) {
                return true;
            } else if (mode == 'minimal-desktop-and-below' && breakpoint <= this.getBreakpoint('xl')) {
                return true;
            }
            return false;
        },

        /**
         * Checks whether given device mode is currently activated.
         * @param {string} mode Responsive mode name(e.g: desktop,
         *     desktop-and-tablet, tablet, tablet-and-mobile, mobile)
         * @returns {boolean}
         */
        isBreakpointUp: function (mode) {
            var width = this.getViewPort().width;
            var breakpoint = this.getBreakpoint(mode);

            return (width >= breakpoint);
        },

        isBreakpointDown: function (mode) {
            var width = this.getViewPort().width;
            var breakpoint = this.getBreakpoint(mode);

            return (width < breakpoint);
        },

        /**
         * Generates unique ID for give prefix.
         * @param {string} prefix Prefix for generated ID
         * @returns {boolean}
         */
        getUniqueID: function (prefix) {
            return prefix + Math.floor(Math.random() * (new Date()).getTime());
        },

        /**
         * Gets window width for give breakpoint mode.
         * @param {string} mode Responsive mode name(e.g: xl, lg, md, sm)
         * @returns {number}
         */
        getBreakpoint: function (mode) {
            return breakpoints[mode];
        },

        /**
         * Checks whether object has property matchs given key path.
         * @param {object} obj Object contains values paired with given key path
         * @param {string} keys Keys path seperated with dots
         * @returns {object}
         */
        isset: function (obj, keys) {
            var stone;
            keys = keys || '';
            if (keys.indexOf('[') !== -1) {
                throw new Error('Unsupported object path notation.');
            }
            keys = keys.split('.');
            do {
                if (obj === undefined) {
                    return false;
                }
                stone = keys.shift();
                if (!obj.hasOwnProperty(stone)) {
                    return false;
                }
                obj = obj[stone];
            }
            while (keys.length);
            return true;
        },

        /**
         * Gets highest z-index of the given element parents
         * @param {object} el jQuery element object
         * @returns {number}
         */
        getHighestZindex: function (el) {
            var position, value;
            while (el && el !== document) {
                // Ignore z-index if position is set to a value where z-index is ignored by the browser
                // This makes behavior of this function consistent across browsers
                // WebKit always returns auto if the element is positioned
                position = this.css(el, 'position');
                if (position === "absolute" || position === "relative" || position === "fixed") {
                    // IE returns 0 when zIndex is not specified
                    // other browsers return a string
                    // we ignore the case of nested elements with an explicit value of 0
                    // <div style="z-index: -10;"><div style="z-index: 0;"></div></div>
                    value = parseInt(this.css(el, 'z-index'));
                    if (!isNaN(value) && value !== 0) {
                        return value;
                    }
                }
                el = el.parentNode;
            }
            return null;
        },

        /**
         * Checks whether the element has any parent with fixed positionfreg
         * @param {object} el jQuery element object
         * @returns {boolean}
         */
        hasFixedPositionedParent: function (el) {
            var position;
            while (el && el !== document) {
                position = this.css(el, 'position');
                if (position === "fixed") {
                    return true;
                }
                el = el.parentNode;
            }
            return false;
        },

        /**
         * Simulates delay
         */
        sleep: function (milliseconds) {
            var start = new Date().getTime();
            for (var i = 0; i < 1e7; i++) {
                if ((new Date().getTime() - start) > milliseconds) {
                    break;
                }
            }
        },

        /**
         * Gets randomly generated integer value within given min and max range
         * @param {number} min Range start value
         * @param {number} max Range end value
         * @returns {number}
         */
        getRandomInt: function (min, max) {
            return Math.floor(Math.random() * (max - min + 1)) + min;
        },

        /**
         * Checks whether Angular library is included
         * @returns {boolean}
         */
        isAngularVersion: function () {
            return window.Zone !== undefined ? true : false;
        },
        // jQuery Workarounds

        // Deep extend:  $.extend(true, {}, objA, objB);
        deepExtend: function (out) {
            out = out || {};
            for (var i = 1; i < arguments.length; i++) {
                var obj = arguments[i];
                if (!obj)
                    continue;
                for (var key in obj) {
                    if (obj.hasOwnProperty(key)) {
                        if (typeof obj[key] === 'object')
                            out[key] = ESUtil.deepExtend(out[key], obj[key]);
                        else
                            out[key] = obj[key];
                    }
                }
            }
            return out;
        },

        // extend:  $.extend({}, objA, objB);
        extend: function (out) {
            out = out || {};
            for (var i = 1; i < arguments.length; i++) {
                if (!arguments[i])
                    continue;
                for (var key in arguments[i]) {
                    if (arguments[i].hasOwnProperty(key))
                        out[key] = arguments[i][key];
                }
            }
            return out;
        },

        getById: function (el) {
            if (typeof el === 'string') {
                return document.getElementById(el);
            }
            else {
                return el;
            }
        },

        getByTag: function (query) {
            return document.getElementsByTagName(query);
        },

        getByTagName: function (query) {
            return document.getElementsByTagName(query);
        },

        getByClass: function (query) {
            return document.getElementsByClassName(query);
        },

        getBody: function () {
            return document.getElementsByTagName('body')[0];
        },

        /**
         * Checks whether the element has given classes
         * @param {object} el jQuery element object
         * @param {string} Classes string
         * @returns {boolean}
         */
        hasClasses: function (el, classes) {
            if (!el) {
                return;
            }
            var classesArr = classes.split(" ");
            for (var i = 0; i < classesArr.length; i++) {
                if (ESUtil.hasClass(el, ESUtil.trim(classesArr[i])) == false) {
                    return false;
                }
            }
            return true;
        },

        hasClass: function (el, className) {
            if (!el) {
                return;
            }
            return el.classList ? el.classList.contains(className) : new RegExp('\\b' + className + '\\b').test(el.className);
        },

        addClass: function (el, className) {
            if (!el || typeof className === 'undefined') {
                return;
            }
            var classNames = className.split(' ');
            if (el.classList) {
                for (var i = 0; i < classNames.length; i++) {
                    if (classNames[i] && classNames[i].length > 0) {
                        el.classList.add(ESUtil.trim(classNames[i]));
                    }
                }
            }
            else if (!ESUtil.hasClass(el, className)) {
                for (var x = 0; x < classNames.length; x++) {
                    el.className += ' ' + ESUtil.trim(classNames[x]);
                }
            }
        },

        removeClass: function (el, className) {
            if (!el || typeof className === 'undefined') {
                return;
            }
            var classNames = className.split(' ');
            if (el.classList) {
                for (var i = 0; i < classNames.length; i++) {
                    el.classList.remove(this.trim(classNames[i]));
                }
            } else if (ESUtil.hasClass(el, className)) {
                for (var x = 0; x < classNames.length; x++) {
                    el.className = el.className.replace(new RegExp('\\b' + ESUtil.trim(classNames[x]) + '\\b', 'g'), '');
                }
            }
        },

        triggerCustomEvent: function (el, eventName, data) {
            var event;
            if (window.CustomEvent) {
                event = new CustomEvent(eventName, {
                    detail: data
                });
            } else {
                event = document.createEvent('CustomEvent');
                event.initCustomEvent(eventName, true, true, data);
            }
            el.dispatchEvent(event);
        },

        triggerEvent: function (node, eventName) {
            // Make sure we use the ownerDocument from the provided node to avoid cross-window problems
            var doc;
            if (node.ownerDocument) {
                doc = node.ownerDocument;
            } else if (node.nodeType == 9) {
                // the node may be the document itself, nodeType 9 = DOCUMENT_NODE
                doc = node;
            } else {
                throw new Error("Invalid node passed to fireEvent: " + node.id);
            }

            if (node.dispatchEvent) {
                // Gecko-style approach (now the standard) takes more work
                var eventClass = "";

                // Different events have different event classes.
                // If this switch statement can't map an eventName to an eventClass,
                // the event firing is going to fail.
                switch (eventName) {
                    case "click": // Dispatching of 'click' appears to not work correctly in Safari. Use 'mousedown' or 'mouseup' instead.
                    case "mouseenter":
                    case "mouseleave":
                    case "mousedown":
                    case "mouseup":
                        eventClass = "MouseEvents";
                        break;

                    case "focus":
                    case "change":
                    case "blur":
                    case "select":
                        eventClass = "HTMLEvents";
                        break;

                    default:
                        throw "fireEvent: Couldn't find an event class for event '" + eventName + "'.";
                        break;
                }
                var event = doc.createEvent(eventClass);

                var bubbles = eventName == "change" ? false : true;
                event.initEvent(eventName, bubbles, true); // All events created as bubbling and cancelable.

                event.synthetic = true; // allow detection of synthetic events
                // The second parameter says go ahead with the default action
                node.dispatchEvent(event, true);
            } else if (node.fireEvent) {
                // IE-old school style
                var event = doc.createEventObject();
                event.synthetic = true; // allow detection of synthetic events
                node.fireEvent("on" + eventName, event);
            }
        },

        index: function (el) {
            var c = el.parentNode.children, i = 0;
            for (; i < c.length; i++)
                if (c[i] == el) return i;
        },

        trim: function (string) {
            return string.trim();
        },

        eventTriggered: function (e) {
            if (e.currentTarget.dataset.triggered) {
                return true;
            } else {
                e.currentTarget.dataset.triggered = true;

                return false;
            }
        },

        remove: function (el) {
            if (el && el.parentNode) {
                el.parentNode.removeChild(el);
            }
        },

        find: function (parent, query) {
            parent = this.getById(parent);
            if (parent) {
                return parent.querySelector(query);
            }
        },

        findAll: function (parent, query) {
            parent = this.getById(parent);
            if (parent) {
                return parent.querySelectorAll(query);
            }
        },

        insertAfter: function (el, referenceNode) {
            return referenceNode.parentNode.insertBefore(el, referenceNode.nextSibling);
        },

        parents: function (elem, selector) {
            // Element.matches() polyfill
            if (!Element.prototype.matches) {
                Element.prototype.matches =
                    Element.prototype.matchesSelector ||
                    Element.prototype.mozMatchesSelector ||
                    Element.prototype.msMatchesSelector ||
                    Element.prototype.oMatchesSelector ||
                    Element.prototype.webkitMatchesSelector ||
                    function (s) {
                        var matches = (this.document || this.ownerDocument).querySelectorAll(s),
                            i = matches.length;
                        while (--i >= 0 && matches.item(i) !== this) { }
                        return i > -1;
                    };
            }

            // Set up a parent array
            var parents = [];

            // Push each parent element to the array
            for (; elem && elem !== document; elem = elem.parentNode) {
                if (selector) {
                    if (elem.matches(selector)) {
                        parents.push(elem);
                    }
                    continue;
                }
                parents.push(elem);
            }

            // Return our parent array
            return parents;
        },

        children: function (el, selector, log) {
            if (!el || !el.childNodes) {
                return;
            }
            var result = [],
                i = 0,
                l = el.childNodes.length;

            for (var i; i < l; ++i) {
                if (el.childNodes[i].nodeType == 1 && ESUtil.matches(el.childNodes[i], selector, log)) {
                    result.push(el.childNodes[i]);
                }
            }
            return result;
        },

        child: function (el, selector, log) {
            var children = ESUtil.children(el, selector, log);
            return children ? children[0] : null;
        },

        matches: function (el, selector, log) {
            var p = Element.prototype;
            var f = p.matches || p.webkitMatchesSelector || p.mozMatchesSelector || p.msMatchesSelector || function (s) {
                return [].indexOf.call(document.querySelectorAll(s), this) !== -1;
            };
            if (el && el.tagName) {
                return f.call(el, selector);
            } else {
                return false;
            }
        },

        data: function (el) {
            return {
                set: function (name, data) {
                    if (!el) {
                        return;
                    }

                    if (el.customDataTag === undefined) {
                        window.ESUtilElementDataStoreID++;
                        el.customDataTag = window.ESUtilElementDataStoreID;
                    }

                    if (window.ESUtilElementDataStore[el.customDataTag] === undefined) {
                        window.ESUtilElementDataStore[el.customDataTag] = {};
                    }

                    window.ESUtilElementDataStore[el.customDataTag][name] = data;
                },

                get: function (name) {
                    if (!el) {
                        return;
                    }

                    if (el.customDataTag === undefined) {
                        return null;
                    }

                    return this.has(name) ? window.ESUtilElementDataStore[el.customDataTag][name] : null;
                },

                has: function (name) {
                    if (!el) {
                        return false;
                    }

                    if (el.customDataTag === undefined) {
                        return false;
                    }

                    return (window.ESUtilElementDataStore[el.customDataTag] && window.ESUtilElementDataStore[el.customDataTag][name]) ? true : false;
                },

                remove: function (name) {
                    if (el && this.has(name)) {
                        delete window.ESUtilElementDataStore[el.customDataTag][name];
                    }
                }
            };
        },

        outerWidth: function (el, margin) {
            var width;

            if (margin === true) {
                width = parseFloat(el.offsetWidth);
                width += parseFloat(ESUtil.css(el, 'margin-left')) + parseFloat(ESUtil.css(el, 'margin-right'));

                return parseFloat(width);
            } else {
                width = parseFloat(el.offsetWidth);

                return width;
            }
        },

        offset: function (el) {
            var rect, win;

            if (!el) {
                return;
            }

            // Return zeros for disconnected and hidden (display: none) elements (gh-2310)
            // Support: IE <=11 only
            // Running getBoundingClientRect on a
            // disconnected node in IE throws an error

            if (!el.getClientRects().length) {
                return { top: 0, left: 0 };
            }

            // Get document-relative position by adding viewport scroll to viewport-relative gBCR
            rect = el.getBoundingClientRect();
            win = el.ownerDocument.defaultView;

            return {
                top: rect.top + win.pageYOffset,
                left: rect.left + win.pageXOffset
            };
        },

        height: function (el) {
            return ESUtil.css(el, 'height');
        },

        outerHeight: function (el, withMargic = false) {
            var height = el.offsetHeight;
            var style;

            if (withMargic) {
                style = getComputedStyle(el);
                height += parseInt(style.marginTop) + parseInt(style.marginBottom);

                return height;
            } else {
                return height;
            }
        },

        visible: function (el) {
            return !(el.offsetWidth === 0 && el.offsetHeight === 0);
        },

        attr: function (el, name, value) {
            if (el == undefined) {
                return;
            }

            if (value !== undefined) {
                el.setAttribute(name, value);
            } else {
                return el.getAttribute(name);
            }
        },

        hasAttr: function (el, name) {
            if (el == undefined) {
                return;
            }

            return el.getAttribute(name) ? true : false;
        },

        removeAttr: function (el, name) {
            if (el == undefined) {
                return;
            }

            el.removeAttribute(name);
        },

        animate: function (from, to, duration, update, easing, done) {
            /**
             * TinyAnimate.easings
             *  Adapted from jQuery Easing
             */
            var easings = {};
            var easing;

            easings.linear = function (t, b, c, d) {
                return c * t / d + b;
            };

            easing = easings.linear;

            // Early bail out if called incorrectly
            if (typeof from !== 'number' ||
                typeof to !== 'number' ||
                typeof duration !== 'number' ||
                typeof update !== 'function') {
                return;
            }

            // Create mock done() function if necessary
            if (typeof done !== 'function') {
                done = function () { };
            }

            // Pick implementation (requestAnimationFrame | setTimeout)
            var rAF = window.requestAnimationFrame || function (callback) {
                window.setTimeout(callback, 1000 / 50);
            };

            // Animation loop
            var canceled = false;
            var change = to - from;

            function loop(timestamp) {
                var time = (timestamp || +new Date()) - start;

                if (time >= 0) {
                    update(easing(time, from, change, duration));
                }
                if (time >= 0 && time >= duration) {
                    update(to);
                    done();
                } else {
                    rAF(loop);
                }
            }

            update(from);

            // Start animation loop
            var start = window.performance && window.performance.now ? window.performance.now() : +new Date();

            rAF(loop);
        },

        actualCss: function (el, prop, cache) {
            var css = '';

            if (el instanceof HTMLElement === false) {
                return;
            }

            if (!el.getAttribute('es-hidden-' + prop) || cache === false) {
                var value;

                // the element is hidden so:
                // making the el block so we can meassure its height but still be hidden
                css = el.style.cssText;
                el.style.cssText = 'position: absolute; visibility: hidden; display: block;';

                if (prop == 'width') {
                    value = el.offsetWidth;
                } else if (prop == 'height') {
                    value = el.offsetHeight;
                }

                el.style.cssText = css;

                // store it in cache
                el.setAttribute('es-hidden-' + prop, value);

                return parseFloat(value);
            } else {
                // store it in cache
                return parseFloat(el.getAttribute('es-hidden-' + prop));
            }
        },

        actualHeight: function (el, cache) {
            return this.actualCss(el, 'height', cache);
        },

        actualWidth: function (el, cache) {
            return this.actualCss(el, 'width', cache);
        },

        getScroll: function (element, method) {
            // The passed in `method` value should be 'Top' or 'Left'
            method = 'scroll' + method;
            return (element == window || element == document) ? (
                self[(method == 'scrollTop') ? 'pageYOffset' : 'pageXOffset'] ||
                (browserSupportsBoxModel && document.documentElement[method]) ||
                document.body[method]
            ) : element[method];
        },

        css: function (el, styleProp, value) {
            if (!el) {
                return;
            }

            if (value !== undefined) {
                el.style[styleProp] = value;
            } else {
                var defaultView = (el.ownerDocument || document).defaultView;
                // W3C standard way:
                if (defaultView && defaultView.getComputedStyle) {
                    // sanitize property name to css notation
                    // (hyphen separated words eg. font-Size)
                    styleProp = styleProp.replace(/([A-Z])/g, "-$1").toLowerCase();
                    return defaultView.getComputedStyle(el, null).getPropertyValue(styleProp);
                } else if (el.currentStyle) { // IE
                    // sanitize property name to camelCase
                    styleProp = styleProp.replace(/\-(\w)/g, function (str, letter) {
                        return letter.toUpperCase();
                    });
                    value = el.currentStyle[styleProp];
                    // convert other units to pixels on IE
                    if (/^\d+(em|pt|%|ex)?$/i.test(value)) {
                        return (function (value) {
                            var oldLeft = el.style.left,
                                oldRsLeft = el.runtimeStyle.left;
                            el.runtimeStyle.left = el.currentStyle.left;
                            el.style.left = value || 0;
                            value = el.style.pixelLeft + "px";
                            el.style.left = oldLeft;
                            el.runtimeStyle.left = oldRsLeft;
                            return value;
                        })(value);
                    }
                    return value;
                }
            }
        },

        slide: function (el, dir, speed, callback, recalcMaxHeight) {
            if (!el || (dir == 'up' && this.visible(el) === false) || (dir == 'down' && this.visible(el) === true)) {
                return;
            }

            speed = (speed ? speed : 600);
            var calcHeight = this.actualHeight(el);
            var calcPaddingTop = false;
            var calcPaddingBottom = false;

            if (this.css(el, 'padding-top') && this.data(el).has('slide-padding-top') !== true) {
                this.data(el).set('slide-padding-top', this.css(el, 'padding-top'));
            }

            if (this.css(el, 'padding-bottom') && this.data(el).has('slide-padding-bottom') !== true) {
                this.data(el).set('slide-padding-bottom', this.css(el, 'padding-bottom'));
            }

            if (this.data(el).has('slide-padding-top')) {
                calcPaddingTop = parseInt(this.data(el).get('slide-padding-top'));
            }

            if (this.data(el).has('slide-padding-bottom')) {
                calcPaddingBottom = parseInt(this.data(el).get('slide-padding-bottom'));
            }

            if (dir == 'up') { // up
                el.style.cssText = 'display: block; overflow: hidden;';

                if (calcPaddingTop) {
                    this.animate(0, calcPaddingTop, speed, function (value) {
                        el.style.paddingTop = (calcPaddingTop - value) + 'px';
                    }, 'linear');
                }

                if (calcPaddingBottom) {
                    this.animate(0, calcPaddingBottom, speed, function (value) {
                        el.style.paddingBottom = (calcPaddingBottom - value) + 'px';
                    }, 'linear');
                }

                this.animate(0, calcHeight, speed, function (value) {
                    el.style.height = (calcHeight - value) + 'px';
                }, 'linear', function () {
                    el.style.height = '';
                    el.style.display = 'none';

                    if (typeof callback === 'function') {
                        callback();
                    }
                });


            } else if (dir == 'down') { // down
                el.style.cssText = 'display: block; overflow: hidden;';

                if (calcPaddingTop) {
                    this.animate(0, calcPaddingTop, speed, function (value) {//
                        el.style.paddingTop = value + 'px';
                    }, 'linear', function () {
                        el.style.paddingTop = '';
                    });
                }

                if (calcPaddingBottom) {
                    this.animate(0, calcPaddingBottom, speed, function (value) {
                        el.style.paddingBottom = value + 'px';
                    }, 'linear', function () {
                        el.style.paddingBottom = '';
                    });
                }

                this.animate(0, calcHeight, speed, function (value) {
                    el.style.height = value + 'px';
                }, 'linear', function () {
                    el.style.height = '';
                    el.style.display = '';
                    el.style.overflow = '';

                    if (typeof callback === 'function') {
                        callback();
                    }
                });
            }
        },

        slideUp: function (el, speed, callback) {
            this.slide(el, 'up', speed, callback);
        },

        slideDown: function (el, speed, callback) {
            this.slide(el, 'down', speed, callback);
        },

        show: function (el, display) {
            if (typeof el !== 'undefined') {
                el.style.display = (display ? display : 'block');
            }
        },

        hide: function (el) {
            if (typeof el !== 'undefined') {
                el.style.display = 'none';
            }
        },

        addEvent: function (el, type, handler, one) {
            if (typeof el !== 'undefined' && el !== null) {
                el.addEventListener(type, handler);
            }
        },

        removeEvent: function (el, type, handler) {
            if (el !== null) {
                el.removeEventListener(type, handler);
            }
        },

        on: function (element, selector, event, handler) {
            if (!selector) {
                return;
            }

            var eventId = this.getUniqueID('event');

            window.ESUtilDelegatedEventHandlers[eventId] = function (e) {
                var targets = element.querySelectorAll(selector);
                var target = e.target;

                while (target && target !== element) {
                    for (var i = 0, j = targets.length; i < j; i++) {
                        if (target === targets[i]) {
                            handler.call(target, e);
                        }
                    }

                    target = target.parentNode;
                }
            }

            this.addEvent(element, event, window.ESUtilDelegatedEventHandlers[eventId]);

            return eventId;
        },

        off: function (element, event, eventId) {
            if (!element || !window.ESUtilDelegatedEventHandlers[eventId]) {
                return;
            }

            ESUtil.removeEvent(element, event, window.ESUtilDelegatedEventHandlers[eventId]);

            delete window.ESUtilDelegatedEventHandlers[eventId];
        },

        one: function onetime(el, type, callback) {
            el.addEventListener(type, function callee(e) {
                // remove event
                if (e.target && e.target.removeEventListener) {
                    e.target.removeEventListener(e.type, callee);
                }
                // need to verify from https://themeforest.net/author_dashboard#comment_23615588
                if (el && el.removeEventListener) {
                    e.currentTarget.removeEventListener(e.type, callee);
                }
                // call handler
                return callback(e);
            });
        },

        hash: function (str) {
            var hash = 0,
                i, chr;
            if (str.length === 0) return hash;
            for (i = 0; i < str.length; i++) {
                chr = str.charCodeAt(i);
                hash = ((hash << 5) - hash) + chr;
                hash |= 0; // Convert to 32bit integer
            }
            return hash;
        },

        animateClass: function (el, animationName, callback) {
            var animation;
            var animations = {
                animation: 'animationend',
                OAnimation: 'oAnimationEnd',
                MozAnimation: 'mozAnimationEnd',
                WebkitAnimation: 'webkitAnimationEnd',
                msAnimation: 'msAnimationEnd',
            };
            for (var t in animations) {
                if (el.style[t] !== undefined) {
                    animation = animations[t];
                }
            }

            this.addClass(el, 'animated ' + animationName);

            this.one(el, animation, function () {
                this.removeClass(el, 'animated ' + animationName);
            });
            if (callback) {
                this.one(el, animation, callback);
            }
        },

        transitionEnd: function (el, callback) {
            var transition;
            var transitions = {
                transition: 'transitionend',
                OTransition: 'oTransitionEnd',
                MozTransition: 'mozTransitionEnd',
                WebkitTransition: 'webkitTransitionEnd',
                msTransition: 'msTransitionEnd'
            };

            for (var t in transitions) {
                if (el.style[t] !== undefined) {
                    transition = transitions[t];
                }
            }

            this.one(el, transition, callback);
        },

        animationEnd: function (el, callback) {
            var animation;
            var animations = {
                animation: 'animationend',
                OAnimation: 'oAnimationEnd',
                MozAnimation: 'mozAnimationEnd',
                WebkitAnimation: 'webkitAnimationEnd',
                msAnimation: 'msAnimationEnd'
            };

            for (var t in animations) {
                if (el.style[t] !== undefined) {
                    animation = animations[t];
                }
            }

            this.one(el, animation, callback);
        },

        animateDelay: function (el, value) {
            var vendors = ['webkit-', 'moz-', 'ms-', 'o-', ''];
            for (var i = 0; i < vendors.length; i++) {
                this.css(el, vendors[i] + 'animation-delay', value);
            }
        },

        animateDuration: function (el, value) {
            var vendors = ['webkit-', 'moz-', 'ms-', 'o-', ''];
            for (var i = 0; i < vendors.length; i++) {
                this.css(el, vendors[i] + 'animation-duration', value);
            }
        },

        scrollTo: function (target, offset, duration) {
            var duration = duration ? duration : 500;
            var targetPos = target ? this.offset(target).top : 0;
            var scrollPos = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0;
            var from, to;

            if (offset) {
                scrollPos += offset;
            }

            from = scrollPos;
            to = targetPos;

            this.animate(from, to, duration, function (value) {
                document.documentElement.scrollTop = value;
                document.body.parentNode.scrollTop = value;
                document.body.scrollTop = value;
            }); //, easing, done
        },

        scrollTop: function (offset, duration) {
            this.scrollTo(null, offset, duration);
        },

        isArray: function (obj) {
            return obj && Array.isArray(obj);
        },

        ready: function (callback) {
            if (document.attachEvent ? document.readyState === "complete" : document.readyState !== "loading") {
                callback();
            } else {
                document.addEventListener('DOMContentLoaded', callback);
            }
        },

        isEmpty: function (obj) {
            for (var prop in obj) {
                if (obj.hasOwnProperty(prop)) {
                    return false;
                }
            }
            return true;
        },

        numberString: function (nStr) {
            nStr += '';
            var x = nStr.split('.');
            var x1 = x[0];
            var x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            return x1 + x2;
        },

        detectIE: function () {
            var ua = window.navigator.userAgent;
            // Test values; Uncomment to check result …
            // IE 10
            // ua = 'Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)';
            // IE 11
            // ua = 'Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko';
            // Edge 12 (Spartan)
            // ua = 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36 Edge/12.0';
            // Edge 13
            // ua = 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Safari/537.36 Edge/13.10586';
            var msie = ua.indexOf('MSIE ');
            if (msie > 0) {
                // IE 10 or older => return version number
                return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
            }
            var trident = ua.indexOf('Trident/');
            if (trident > 0) {
                // IE 11 => return version number
                var rv = ua.indexOf('rv:');
                return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
            }
            var edge = ua.indexOf('Edge/');
            if (edge > 0) {
                // Edge (IE 12+) => return version number
                return parseInt(ua.substring(edge + 5, ua.indexOf('.', edge)), 10);
            }
            // other browser
            return false;
        },

        // Scroller
        scrollInit: function (element, options) {
            if (!element) {
                return;
            }
            // Learn more: https://github.com/mdbootstrap/perfect-scrollbar#options
            var pluginDefOptions = {
                wheelSpeed: 0.5,
                swipeEasing: true,
                wheelPropagation: false,
                minScrollbarLength: 40,
                maxScrollbarLength: 300,
                suppressScrollX: true
            };
            options = this.deepExtend({}, pluginDefOptions, options);
            // Define init function
            function init() {
                var ps;
                var height;

                // Get extra options via data attributes
                var attrs = element.getAttributeNames();
                if (attrs.length > 0) {
                    attrs.forEach(function (attrName) {
                        // more options; https://github.com/ganlanyuan/tiny-slider#options
                        if ((/^data-.*/g).test(attrName)) {
                            if (['scroll', 'height', 'mobile-height'].includes(optionName) == false) {
                                var optionName = attrName.replace('data-', '').toLowerCase().replace(/(?:[\s-])\w/g, function (match) {
                                    return match.replace('-', '').toUpperCase();
                                });

                                options[optionName] = ESUtil.filterBoolean(element.getAttribute(attrName));
                            }
                        }
                    });
                }

                if (options.height instanceof Function) {
                    height = options.height.call();
                } else {
                    if (ESUtil.isMobileDevice() === true && options.mobileHeight) {
                        height = parseInt(options.mobileHeight);
                    } else {
                        height = parseInt(options.height);
                    }
                }

                if (height === false) {
                    ESUtil.scrollDestroy(element, true);

                    return;
                }

                height = parseInt(height);

                // Destroy scroll on table and mobile modes
                if ((options.mobileNativeScroll || options.disableForMobile) && ESUtil.isMobileDevice() === true) {
                    ps = ESUtil.data(element).get('ps');
                    if (ps) {
                        if (options.resetHeightOnDestroy) {
                            ESUtil.css(element, 'height', 'auto');
                        } else {
                            ESUtil.css(element, 'overflow', 'auto');
                            if (height > 0) {
                                ESUtil.css(element, 'height', height + 'px');
                            }
                        }

                        ps.destroy();
                        ps = ESUtil.data(element).remove('ps');
                    } else if (height > 0) {
                        ESUtil.css(element, 'overflow', 'auto');
                        ESUtil.css(element, 'height', height + 'px');
                    }

                    return;
                }

                if (height > 0) {
                    ESUtil.css(element, 'height', height + 'px');
                }

                if (options.desktopNativeScroll) {
                    ESUtil.css(element, 'overflow', 'auto');
                    return;
                }

                // Pass options via HTML Attributes
                if (ESUtil.attr(element, 'data-window-scroll') == 'true') {
                    options.windowScroll = true;
                }

                // Init scroll
                ps = ESUtil.data(element).get('ps');

                if (ps) {
                    ps.update();
                } else {
                    ESUtil.css(element, 'overflow', 'hidden');
                    ESUtil.addClass(element, 'scroll');

                    ps = new PerfectScrollbar(element, options);

                    ESUtil.data(element).set('ps', ps);
                }

                // Remember scroll position in cookie
                var uid = ESUtil.attr(element, 'id');
                // Consider using Localstorage
                //if (options.rememberPosition === true && Cookies && uid) {
                //    if (ESCookie.getCookie(uid)) {
                //        var pos = parseInt(ESCookie.getCookie(uid));
                //
                //        if (pos > 0) {
                //            element.scrollTop = pos;
                //        }
                //    }
                //
                //    element.addEventListener('ps-scroll-y', function() {
                //        ESCookie.setCookie(uid, element.scrollTop);
                //    });
                //}
            }

            // Init
            init();

            // Handle window resize
            if (options.handleWindowResize) {
                this.addResizeHandler(function () {
                    init();
                });
            }
        },

        scrollUpdate: function (element) {
            var ps = ESUtil.data(element).get('ps');
            if (ps) {
                ps.update();
            }
        },

        scrollUpdateAll: function (parent) {
            var scrollers = ESUtil.findAll(parent, '.ps');
            for (var i = 0, len = scrollers.length; i < len; i++) {
                ESUtil.scrollUpdate(scrollers[i]);
            }
        },

        scrollDestroy: function (element, resetAll) {
            var ps = ESUtil.data(element).get('ps');
            if (ps) {
                ps.destroy();
                ps = ESUtil.data(element).remove('ps');
            }
            if (element && resetAll) {
                element.style.setProperty('overflow', '');
                element.style.setProperty('height', '');
            }
        },

        filterBoolean: function (val) {
            // Convert string boolean
            if (val === true || val === 'true') {
                return true;
            }
            if (val === false || val === 'false') {
                return false;
            }
            return val;
        },

        setHTML: function (el, html) {
            el.innerHTML = html;
        },

        getHTML: function (el) {
            if (el) {
                return el.innerHTML;
            }
        },

        getDocumentHeight: function () {
            var body = document.body;
            var html = document.documentElement;

            return Math.max(body.scrollHeight, body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight);
        },

        getScrollTop: function () {
            return (document.scrollingElement || document.documentElement).scrollTop;
        },

        // Throttle function: Input as function which needs to be throttled and delay is the time interval in milliseconds
        throttle: function (timer, func, delay) {
            // If setTimeout is already scheduled, no need to do anything
            if (timer) {
                return;
            }
            // Schedule a setTimeout after delay seconds
            timer = setTimeout(function () {
                func();
                // Once setTimeout function execution is finished, timerId = undefined so that in <br>
                // the next scroll event function execution can be scheduled by the setTimeout
                timer = undefined;
            }, delay);
        },

        // Debounce function: Input as function which needs to be debounced and delay is the debounced time in milliseconds
        debounce: function (timer, func, delay) {
            // Cancels the setTimeout method execution
            clearTimeout(timer)
            // Executes the func after delay time.
            timer = setTimeout(func, delay);
        },

        isOffscreen: function (el, direction, offset = 0) {
            var windowWidth = this.getViewPort().width;
            var windowHeight = this.getViewPort().height;
            var top = this.offset(el).top;
            var height = this.outerHeight(el) + offset;
            var left = this.offset(el).left;
            var width = this.outerWidth(el) + offset;
            if (direction == 'bottom') {
                if (windowHeight < top + height) {
                    return true;
                } else if (windowHeight > top + height * 1.5) {
                    return true;
                }
            }
            if (direction == 'top') {
                if (top < 0) {
                    return true;
                } else if (top > height) {
                    return true;
                }
            }
            if (direction == 'left') {
                if (left < 0) {
                    return true;
                } else if (left * 2 > width) {
                    //console.log('left 2');
                    //return true;
                }
            }
            if (direction == 'right') {
                if (windowWidth < left + width) {
                    return true;
                } else {
                    //console.log('right 2');
                    //return true;
                }
            }
            return false;
        },
        /**
         * Check if the given variable is a function
         * @method
         * @memberof Popper.Utils
         * @argument {Any} functionToCheck - variable to check
         * @returns {Boolean} answer to: is a function?
         */
        isFunction: function (functionToCheck) {
            var getType = {};
            return functionToCheck && getType.toString.call(functionToCheck) === '[object Function]';
        },

        /**
         * Returns the parentNode or the host of the element
         * @method
         * @memberof Popper.Utils
         * @argument {Element} element
         * @returns {Element} parent
         */
        getParentNode: function (element) {
            if (element.nodeName === 'HTML') {
                return element;
            }
            return element.parentNode || element.host;
        },
        /**
         * Get the outer sizes of the given element (offset size + margins)
         * @method
         * @memberof Popper.Utils
         * @argument {Element} element
         * @returns {Object} object containing width and height properties
         */
        getOuterSizes: function (element) {
            var window = element.ownerDocument.defaultView;
            var styles = window.getComputedStyle(element);
            var x = parseFloat(styles.marginTop || 0) + parseFloat(styles.marginBottom || 0);
            var y = parseFloat(styles.marginLeft || 0) + parseFloat(styles.marginRight || 0);
            var result = {
                width: element.offsetWidth + y,
                height: element.offsetHeight + x
            };
            return result;
        },
        /**
         * Tells if a given input is a number
         * @method
         * @memberof Popper.Utils
         * @param {*} input to check
         * @return {Boolean}
         */
        isNumeric: function (n) {
            return n !== '' && !isNaN(parseFloat(n)) && isFinite(n);
        },

        genHTML: function (templateId, appendId, objValue = undefined, callback = undefined) {
            var temp = _.template($("#" + templateId).html());
            var html = $(temp(objValue));
            $("#" + appendId).html(html);
            updateWrap();
            if (typeof callback === "function") {
                callback();
            }
        },
        appendHTML: function (templateId, appendId, objValue = undefined, callback = undefined) {
            var temp = _.template($("#" + templateId).html());
            var html = $(temp(objValue));
            $("#" + appendId).append(html);
            if (typeof callback === "function") {
                callback();
            }
        },
        prependHTML: function (templateId, appendId, objValue = undefined, callback = undefined) {
            var temp = _.template($("#" + templateId).html());
            var html = $(temp(objValue));
            $("#" + appendId).prepend(html);
            if (typeof callback === "function") {
                callback();
            }
        },
        convertBase64ToFile: function (base64data, filename, type = "image/png") {
            var bs = atob(base64data);
            var buffer = new ArrayBuffer(bs.length);
            var ba = new Uint8Array(buffer);
            for (var i = 0; i < bs.length; i++) {
                ba[i] = bs.charCodeAt(i);
            }
            var blob = new Blob([ba], { type: type });

            if (window.navigator.msSaveBlob) {
                window.navigator.msSaveOrOpenBlob(blob, filename);
            }
            else {
                var a = window.document.createElement("a");
                a.href = window.URL.createObjectURL(blob, { type: type });
                a.download = filename;
                document.body.appendChild(a);
                a.click();
                document.body.removeChild(a);
            }

        },
        getSelectorFromElement: function (element) {
            let selector = element.getAttribute('data-target');

            if (!selector || selector === '#') {
                const hrefAttr = element.getAttribute('href');
                selector = hrefAttr && hrefAttr !== '#' ? hrefAttr.trim() : '';
            }

            try {
                return document.querySelector(selector) ? selector : null;
            }
            catch (_) {
                return null;
            }
        },
        isElement: function (obj) {
            return (obj[0] || obj).nodeType;
        },
        toType: function (obj) {
            if (obj === null || typeof obj === 'undefined') {
                return `${obj}`;
            }
            return {}.toString.call(obj).match(/\s([a-z]+)/i)[1].toLowerCase();
        },
        reflow: function (element) {
            return element.offsetHeight;
        },
        getTransitionDurationFromElement: function (element) {
            if (!element) {
                return 0;
            }

            // Get transition-duration of the element
            let transitionDuration = $(element).css('transition-duration');
            let transitionDelay = $(element).css('transition-delay');

            const floatTransitionDuration = parseFloat(transitionDuration);
            const floatTransitionDelay = parseFloat(transitionDelay);

            // Return 0 if element or transition duration is not found
            if (!floatTransitionDuration && !floatTransitionDelay) {
                return 0;
            }

            // If multiple durations are defined, take the first
            transitionDuration = transitionDuration.split(',')[0];
            transitionDelay = transitionDelay.split(',')[0];

            return (parseFloat(transitionDuration) + parseFloat(transitionDelay)) * MILLISECONDS_MULTIPLIER;
        },

        getYear: function (number) {
            number = number.toString();
            return number.substr(0, 4);
        },
        getMonth: function (number) {
            number = number.toString();
            return number.substr(4, 2);
        },
        getDay: function (number) {
            number = number.toString();
            return number.substr(6, 2);
        },
        getHours: function (number) {
            number = number.toString();
            return number.substr(6, 2);
        },
        getMinute: function (number) {
            number = number.toString();
            return number.substr(8, 2);
        },
        getTimeChat: function (number) {
            number = number.toString();
            return number.substr(8, 2) + ":" + number.substr(10, 2);
        },
        getHoursSysDate: function () {
            var date = new Date();
            var HH = date.getHours();
            var mm = date.getMinutes();
            return [
                (HH > 9 ? '' : '0') + HH,
                (mm > 9 ? '' : '0') + mm
            ].join(':');
        },
        renameJsonKey: function (obj, oldKey, newKey) {
            obj[newKey] = obj[oldKey];
            delete obj[oldKey];
        },
        selected: function (val1, val2) {
            return val1 == val2 ? 'selected' : ''
        },
        readURL: function (input, img_default) {
            $('#preview_' + $(input).attr("name")).attr('src', img_default);
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#preview_' + $(input).attr("name")).attr('src', e.target.result);
                }
                reader.readAsDataURL(input.files[0]);
            }
        },
        checkLoadImage: function (url, fn_success, fn_error = undefined) {
            var tester = new Image();
            tester.onload = fn_success;
            if (fn_error) {
                tester.onerror = fn_error;
            }
            tester.src = url;
        },
        compareText: function (text1, text2) {
            if (text1 == null || text2 == null) {
                return false;
            }
            text1 = text1.toString().trim().toLowerCase();
            text2 = text2.toString().trim().toLowerCase();
            return text1 == text2;
        },
        compareStringDate: function (text1, text2) {
            var arrText1 = text1.split('/');
            var arrText2 = text2.split('/');
            if (arrText1.length < 3 || arrText2.length < 3) {
                return false;
            }
            text1 = (arrText1[0].length < 2 ? "0" + arrText1[0] : arrText1[0]) + "/" + (arrText1[1].length < 2 ? "0" + arrText1[1] : arrText1[1]) + "/" + arrText1[2];
            text2 = (arrText2[0].length < 2 ? "0" + arrText2[0] : arrText2[0]) + "/" + (arrText2[1].length < 2 ? "0" + arrText2[1] : arrText2[1]) + "/" + arrText2[2];
            if (text1 == null || text2 == null) {
                return false;
            }
            text1 = text1.toString().trim().toLowerCase();
            text2 = text2.toString().trim().toLowerCase();
            return text1 == text2;
        },
        tinh_tuoi: function (ngay_sinh, ngay_hl) {
            var datearray = ngay_sinh.split("/");
            var today;
            var newdate = datearray[1] + '/' + datearray[0] + '/' + datearray[2];
            if (ngay_hl !== '' && ngay_hl !== undefined) {
                var datearray_ngayhl = ngay_hl.split("/");
                var newdatehl = datearray_ngayhl[1] + '/' + datearray_ngayhl[0] + '/' + datearray_ngayhl[2];
                today = new Date(newdatehl);
            } else {
                today = new Date();
            }
            var dob = new Date(newdate);
            var year1 = dob.getFullYear();
            var year2 = today.getFullYear();
            var month1 = dob.getMonth();
            var month2 = today.getMonth();
            var day1 = dob.getDate();
            var day2 = today.getDate();
            if (month1 === 0) {
                month1++;
                month2++;
            }
            var numberOfMonths = (year2 - year1) * 12 + (month2 - month1);

            if (month2 === month1) {
                if (day2 < day1) {
                    numberOfMonths = numberOfMonths - 1;
                }
            }
            var age = Math.floor(numberOfMonths / 12);
            return age;
        },
        tinh_ngay_tuoi: function (ngay_sinh, ngay_hl) {
            var datearray = ngay_sinh.split("/");
            var today;
            var newdate = datearray[1] + '/' + datearray[0] + '/' + datearray[2];
            if (ngay_hl !== '' && ngay_hl !== undefined) {
                var datearray_ngayhl = ngay_hl.split("/");
                var newdatehl = datearray_ngayhl[1] + '/' + datearray_ngayhl[0] + '/' + datearray_ngayhl[2];
                today = new Date(newdatehl);
            } else {
                today = new Date();
            }
            var dob = new Date(newdate);
            return Math.round((today - dob) / (1000 * 60 * 60 * 24));
        },
        voiceCall: function (phone, appToApp = false) {
            if (phone == undefined || phone == null || phone == "") {
                return;
            }
            if (appToApp) {
                _stringeeService.callApp(phone);
            }
            else {
                _stringeeService.callDefault(phone);
            }
        },
        pagingHTML: function (funcGoPageName, currentPage, totalRow, rowOnPage) {
            var numberOfPage;
            var prePage;
            var nextPage;
            var strHTML = "";
            var strStyle = 'onmouseover = "this.style.cursor=\'pointer\'; this.style.textDecoration = \'none\'\ " onmouseout = "this.style.cursor = \'default\'"';
            if (totalRow % rowOnPage !== 0)
                numberOfPage = parseInt(totalRow / rowOnPage) + 1;
            else
                numberOfPage = parseInt(totalRow / rowOnPage);

            //console.log("totalRow", totalRow);
            //console.log("rowOnPage", rowOnPage);
            //console.log("numberOfPage", numberOfPage);

            prePage = parseInt(currentPage) - 1;
            nextPage = parseInt(currentPage) + 1;

            if (numberOfPage === 0 || numberOfPage === 1)
                strHTML += "<span class=\"label2\" style=\"padding: 2px; font-size:11px;\"><b> " + totalRow + "</b> bản ghi";
            else
                strHTML += "<span class=\"label2\" style=\"padding: 2px; font-size:11px;\"><b>" + totalRow +
                    "</b> bản ghi | <b>" + numberOfPage + "</b> trang &nbsp;&nbsp;";

            if (totalRow !== 0 && numberOfPage !== 1) {
                if (currentPage > 1) {
                    strHTML += "<a style=\"font-weight:bold\" onclick=\"" + funcGoPageName + "(" + 1 + ")\" " + strStyle + ">" + "|< </a>";
                    strHTML += "<a style=\"font-weight:bold\" onclick=\"" + funcGoPageName + "(" + prePage + ")\" " + strStyle + ">" + "<< </a>";
                }

                strHTML = strHTML + "<span style=\"padding: 2px; font-size:11px;\">Trang </span><select id=\"" + funcGoPageName + "_page\"" +
                    "onchange=\"" + funcGoPageName + "(this.value)\" style=\"width: 45px; font-size:8pt\">\n";

                for (var i = 1; i <= numberOfPage; ++i) {
                    if (i == currentPage)
                        strHTML = strHTML + "<option value=\"" + i.toString() + "\" selected=\"selected\">" + i + "</option>\n";
                    else
                        strHTML = strHTML + "<option value=\"" + i.toString() + "\">" + i.toString() + "</option>\n";
                }
                strHTML = strHTML + "</select>\n";

                if (currentPage < numberOfPage) {
                    strHTML += "<a style=\"font-weight:bold\" onclick=\"" + funcGoPageName + "(" + nextPage + ")\" " + strStyle + "> >> </a>";
                    strHTML += "<a style=\"font-weight:bold\" onclick=\"" + funcGoPageName + "(" + numberOfPage + ")\" " + strStyle + "> >| </a>";
                }
            }

            strHTML = strHTML + "</span>";
            return strHTML;
        },
        removeVietnameseTones: function (str) {
            if (str == undefined || str == null || str == "") {
                return "";
            }
            str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
            str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
            str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
            str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
            str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
            str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
            str = str.replace(/đ/g, "d");
            str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A");
            str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E");
            str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I");
            str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "O");
            str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "U");
            str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "Y");
            str = str.replace(/Đ/g, "D");
            // Some system encode vietnamese combining accent as individual utf-8 characters
            // Một vài bộ encode coi các dấu mũ, dấu chữ như một kí tự riêng biệt nên thêm hai dòng này
            str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, ""); // ̀ ́ ̃ ̉ ̣  huyền, sắc, ngã, hỏi, nặng
            str = str.replace(/\u02C6|\u0306|\u031B/g, ""); // ˆ ̆ ̛  Â, Ê, Ă, Ơ, Ư
            // Remove extra spaces
            // Bỏ các khoảng trắng liền nhau
            str = str.replace(/ + /g, " ");
            str = str.trim();
            // Remove punctuations
            // Bỏ dấu câu, kí tự đặc biệt
            str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|\$|_|`|-|{|}|\||\\/g, " ");
            return str;
        },
        xoaKhoangTrangText: function (str) {
            if (str == undefined || str == null || str == "") {
                return "";
            }
            str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
            str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
            str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
            str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
            str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
            str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
            str = str.replace(/đ/g, "d");
            str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A");
            str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E");
            str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I");
            str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "O");
            str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "U");
            str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "Y");
            str = str.replace(/Đ/g, "D");
            // Some system encode vietnamese combining accent as individual utf-8 characters
            // Một vài bộ encode coi các dấu mũ, dấu chữ như một kí tự riêng biệt nên thêm hai dòng này
            str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, ""); // ̀ ́ ̃ ̉ ̣  huyền, sắc, ngã, hỏi, nặng
            str = str.replace(/\u02C6|\u0306|\u031B/g, ""); // ˆ ̆ ̛  Â, Ê, Ă, Ơ, Ư
            // Remove extra spaces
            // Bỏ các khoảng trắng liền nhau
            str = str.replace(/ + /g, " ");
            str = str.replace("\n", " ");
            str = str.trim().toLowerCase();
            // Remove punctuations
            // Bỏ dấu câu, kí tự đặc biệt
            str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|\$|_|`|-|{|}|\||\\/g, " ");
            str = str.split(' ').join('');
            return str;
        },
        delay: function (callback, ms) {
            var timer = 0;
            return function () {
                var context = this, args = arguments;
                clearTimeout(timer);
                timer = setTimeout(function () {
                    callback.apply(context, args);
                }, ms || 0);
            };
        },
        getDifferenceInDays: function (date1Str, date2Str) {
            var d1 = date1Str.split("/");
            var d2 = date2Str.split("/");
            var date1 = new Date(d1[1] + "/" + d1[0] + "/" + d1[2]);
            var date2 = new Date(d2[1] + "/" + d2[0] + "/" + d2[2]);
            const diffInMs = date2 - date1;
            return diffInMs / (1000 * 60 * 60 * 24);
        },
        getDifferenceInDaysAndHours: function (date1Str, date2Str, hour1Str, hour2Str) {
            var d1 = date1Str.split("/");
            var d2 = date2Str.split("/");
            var h1 = hour1Str.split(":");
            var h2 = hour2Str.split(":");
            if (hour1Str == "" || hour1Str == undefined || hour1Str == null) {
                h1 = ['00', '00'];
            }
            if (hour2Str == "" || hour2Str == undefined || hour2Str == null) {
                h2 = ['00', '00'];
            }

            var date1 = new Date(`${d1[1]}/${d1[0]}/${d1[2]} ${h1[0]}:${h1[1]}`);
            var date2 = new Date(`${d2[1]}/${d2[0]}/${d2[2]} ${h2[0]}:${h2[1]}`);
            const diffInMs = date2 - date1;
            return diffInMs / (1000 * 60 * 60 * 24);
        },
        chuanHoaGioPhut: function (time) {
            if (time == undefined || time == null || time == "")
                return "00:00";
            var arr = time.split(":");
            if (arr.length != 2) {
                return "00:00";
            }
            if (arr[0].length < 2) {
                arr[0] = "0" + arr[0];
            }
            if (arr[1].length < 2) {
                arr[1] = "0" + arr[1];
            }
            return arr[0] + ":" + arr[1];
        },
        rutGonText: function (soKyTu, text) {
            if (text == undefined || text == null || text.trim() == "") {
                return "";
            }
            return text.slice(0, soKyTu) + (text.length > soKyTu ? '...' : '');
        },
        checkDateIsValid: function (dateStr) {
            const regex = /^\d{2}\/\d{2}\/\d{4}$/;
            if (dateStr.match(regex) === null) {
                return false;
            }
            const [day, month, year] = dateStr.split('/');
            const isoFormattedStr = `${year}-${month}-${day}`;
            const date = new Date(isoFormattedStr);
            const timestamp = date.getTime();
            if (typeof timestamp !== 'number' || Number.isNaN(timestamp)) {
                return false;
            }
            return date.toISOString().startsWith(isoFormattedStr);
        },
        replacerImg: function (key, value) {
            if (key == "duong_dan")
                return undefined;
            return value;
        },
        hienThiHuongDanSuDung: function (man_hinh) {
            if (ESConstants) {
                if (man_hinh == "HUONG_DAN_CHUNG_XE") {
                    $("#btnHuongDanSuDungLayout").attr("href", ESConstants.HDSD_CHUNG);
                }
                if (man_hinh == "HUONG_DAN_CHUNG_NG") {
                    $("#btnHuongDanSuDungLayout").attr("href", ESConstants.HDSD_CHUNG_NG);
                }
                if (man_hinh == "CONTACT") {
                    $("#btnHuongDanSuDungLayout").attr("href", ESConstants.HDSD_CONTACT);
                }
                if (man_hinh == "GIAM_DINH") {
                    $("#btnHuongDanSuDungLayout").attr("href", ESConstants.HDSD_GIAM_DINH);
                }
                if (man_hinh == "BOI_THUONG") {
                    $("#btnHuongDanSuDungLayout").attr("href", ESConstants.HDSD_BOI_THUONG);
                }
                if (man_hinh == "THANH_TOAN") {
                    $("#btnHuongDanSuDungLayout").attr("href", ESConstants.HDSD_THANH_TOAN);
                }

                if (man_hinh == "BLVP") {
                    $("#btnHuongDanSuDungLayout").attr("href", ESConstants.HDSD_BLVP);
                }
                if (man_hinh == "HSTT") {
                    $("#btnHuongDanSuDungLayout").attr("href", ESConstants.HDSD_HSTT);
                }
                if (man_hinh == "TINH_TOAN_NG") {
                    $("#btnHuongDanSuDungLayout").attr("href", ESConstants.HDSD_TINH_TOAN_NG);
                }
                if (man_hinh == "THANH_TOAN_NG") {
                    $("#btnHuongDanSuDungLayout").attr("href", ESConstants.HDSD_THANH_TOAN_NG);
                }
            }
        }
    }
}();