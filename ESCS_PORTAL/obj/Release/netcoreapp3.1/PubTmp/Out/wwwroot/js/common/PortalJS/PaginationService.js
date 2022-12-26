function PaginationService(soDong = 10) {
    this.soDong = soDong;
    this.dataArr = [];
    this.pageCount = () => {
        if (this.dataArr.length === 0) return 1;
        const soTrang = this.dataArr.length / this.soDong;
        return Math.ceil(soTrang);
    }
    this.currentPage = 1;
    this.getPage = (trang) => {
        this.currentPage = trang;
        const firstIndex = (trang - 1) * this.soDong;
        if (firstIndex + this.soDong > this.dataArr.length - 1) return this.dataArr.slice(firstIndex);
        return this.dataArr.slice(firstIndex, firstIndex + this.soDong);
    }
    this.canNext = () => {
        return this.currentPage < this.pageCount();
    }
    this.canPrev = () => {
        return this.currentPage > 1;
    }
    this.deleteData = () => {
        this.dataArr = [];
        this.currentPage = 1;
    }
    return this;
}