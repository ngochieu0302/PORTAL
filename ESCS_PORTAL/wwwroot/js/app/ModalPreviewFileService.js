function ModalPreviewFileService() {
    this.viewFile = function (base64, tieu_de = "Xem tài liệu") {
        var _instance = this;
        var fileURL = URL.createObjectURL(_instance.base64toBlob(base64));
        var obj = { url: fileURL, title: tieu_de, w: 1000, h: 650 }
        ESUtil.popupCenter(obj);
    }
    this.showPdfInNewTab = function (base64Data, fileName) {
        let pdfWindow = window.open("");
        pdfWindow.document.write("<html<head><title>" + fileName + "</title><style>body{margin: 0px;}iframe{border-width: 0px;}</style></head>");
        pdfWindow.document.write("<body><embed width='100%' height='100%' src='data:application/pdf;base64, " + encodeURI(base64Data) + "#toolbar=0&navpanes=0&scrollbar=0'></embed></body></html>");
    }
    this.downloadPDF = function (pdf) {
        const linkSource = `data:application/pdf;base64,${pdf}`;
        const downloadLink = document.createElement("a");
        const fileName = "file.pdf";
        downloadLink.href = linkSource;
        downloadLink.target = "_blank";
        downloadLink.download = fileName;
        downloadLink.click();
    }
    this.openBase64NewTab = function (base64Pdf) {
        var _instance = this;
        var blob = _instance.base64toBlob(base64Pdf);
        if (window.navigator && window.navigator.msSaveOrOpenBlob) {
            window.navigator.msSaveOrOpenBlob(blob, "pdfBase64.pdf");
        } else {
            const blobUrl = URL.createObjectURL(blob);
            window.open(blobUrl);
        }
    }
    this.base64toBlob = function (base64Data) {
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
        return new Blob(byteArrays, { type: "application/pdf" });
    }
}