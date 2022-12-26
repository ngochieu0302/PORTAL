function ViewerService(serviceID) {
    this.options = {
        "backdrop": true,
        "button": false,
        "navbar": false,
        "title": true,
        "toolbar": {
            prev: 0,
            rotateLeft: {
                show: 1,
                size: 'large',
            },
            rotateRight: {
                show: 1,
                size: 'large',
            },
            zoomIn: {
                show: 1,
                size: 'large',
            },
            zoomOut: {
                show: 1,
                size: 'large',
            },
            oneToOne: {
                show: 1,
                size: 'large',
            },
            reset: {
                show: 1,
                size: 'large',
            },
            next: 0,

            play: {
                show: 0,
                size: 'large',
            },
            flipHorizontal: 0,
            flipVertical: 0,
        },
        "className": "",
        "container": "body",
        "filter": null,
        "fullscreen": true,
        "inheritedAttributes": [
            "crossOrigin",
            "decoding",
            "isMap",
            "loading",
            "referrerPolicy",
            "sizes",
            "srcset",
            "useMap"
        ],
        "initialViewIndex": 0,
        "inline": false,
        "interval": 5000,
        "keyboard": true,
        "focus": true,
        "loading": true,
        "loop": true,
        "minWidth": 200,
        "minHeight": 100,
        "movable": true,
        "rotatable": true,
        "scalable": true,
        "zoomable": true,
        "zoomOnTouch": true,
        "zoomOnWheel": true,
        "slideOnTouch": true,
        "toggleOnDblclick": true,
        "tooltip": true,
        "transition": true,
        "zIndex": 2015,
        "zIndexInline": 0,
        "zoomRatio": 0.3,
        "minZoomRatio": 0.01,
        "maxZoomRatio": 100,
        "url": "src",
        "ready": null,
        "show": null,
        "shown": null,
        "hide": null,
        "hidden": null,
        "view": null,
        "viewed": null,
        "move": null,
        "moved": null,
        "rotate": null,
        "rotated": null,
        "scale": null,
        "scaled": null,
        "zoom": null,
        "zoomed": null,
        "play": null,
        "stop": null
    }
    this.container = null;
    this.containerState = false;
    this.createContainer = (classList = []) => {
        if (this.containerState) return this;

        this.container = document.getElementById(serviceID);
        if (this.container !== null) this.container.remove();
        this.container = document.createElement('div');
        this.container.id = serviceID;
        classList.forEach(item => {
            this.container.classList.add(item);
        });
        this.container.style.display = 'none';
        document.body.appendChild(this.container);
        this.containerState = true;
        this.elementList = [];
        return this;
    }

    this.elementList = [];
    this.createViewElement = (elementId) => {
        this.createContainer();
        if (this.elementList.find(item => item.id === elementId) != undefined) return;
        const newElement = document.createElement('div');
        newElement.id = elementId;
        this.container.appendChild(newElement);

        this.elementList.push({
            id: elementId,
            instance: null,
            instanceState: false,
            data: []
        });
        return this;
    }

    this.pushImage = (elementId, image = document.createElement('img')) => {
        this.createViewElement(elementId);
        const element = this.elementList.find(item => item.id === elementId)
        if (element.data.find(item => item === image.dataset.ma) !== undefined) return this;
        document.getElementById(element.id).appendChild(image);
        element.data.push(image.dataset.ma);
        return this;
    }
    this.checkData = (elementId, imageID) => {
        const element = this.elementList.find(item => item.id === elementId);
        if (element == undefined) return false;
        if (element.data.length === 0) {
            return false;
        }
        let img = element.data.find(item => item == imageID);
        if (img === undefined) {
            return false;
        }
        return true;
    }
    this.reqestImage = (elementId, imageID) => {
        if (!this.checkData(elementId, imageID)) return this;
        const element = this.elementList.find(item => item.id === elementId);
        let img = element.data.find(item => item == imageID);
        if (element.instanceState) element.instance.destroy();
        element.instance = new Viewer(document.getElementById(element.id), this.options);
        element.instanceState = true;
        element.instance.view(element.data.indexOf(img));
        return this;
    }
    this.deleteAllData = () => {
        this.elementList = [];
        this.elementList.forEach(el => {
            if(el.instanceState)  el.instance.destroy();
        })
        this.container.innerHTML = '';
    }
    this.init = () => {
        this.createContainer();
        return this;
    }
    return this.init();
}
function getFileType(extension) {
    switch (extension) {
        case '.jpg': return 'img';
        case '.jpeg': return 'img';
        case '.png': return 'img';
        case '.pdf': return 'pdf';
        case '.xlsx': return 'xls';
        case '.xls': return 'xls';
    }
    return 'unknown';
}
function createImage(obj, srcpdf = '/images/PDF_icon.svg', srcxls = '/images/XLS_icon.svg', src = '/images/default.png') {
    const image = document.createElement('img');
    switch (getFileType(obj.extension)) {
        case 'img':
            if (obj.duong_dan == '' || obj.duong_dan == null) image.setAttribute('src', src);
            else image.setAttribute('src', `data:image/png;base64, ${obj.duong_dan}`);
            break;
        case 'pdf': image.setAttribute('src', srcpdf); break;
        case 'xls': image.setAttribute('src', srcxls); break;
        case 'unknown': image.setAttribute('src', src); break;
    }
    //for (const data in obj) {
    //    if (data == 'duong_dan') continue;
    //    image.dataset[data] = obj[data];
    //}
    return image;
}

function base64toBlob(base64Data,type = 'pdf') {
    const sliceSize = 1024;
    const byteCharacters = atob(base64Data);
    const bytesLength = byteCharacters.length;
    const slicesCount = Math.ceil(bytesLength / sliceSize);
    const byteArrays = new Array(slicesCount);

    for (let sliceIndex = 0; sliceIndex < slicesCount; ++sliceIndex) {
        const begin = sliceIndex * sliceSize;
        const end = Math.min(begin + sliceSize, bytesLength);

        const bytes = new Array(end - begin);
        for (let offset = begin, i = 0; offset < end; ++i, ++offset) {
            bytes[i] = byteCharacters[offset].charCodeAt(0);
        }
        byteArrays[sliceIndex] = new Uint8Array(bytes);
    }
    return new Blob(byteArrays, { type: `application/${type}` });
}