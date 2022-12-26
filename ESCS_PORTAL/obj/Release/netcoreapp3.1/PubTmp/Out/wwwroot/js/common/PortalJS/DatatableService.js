function DatatableService(idDatatable, enableRowClick = false, pageRows = 15) {
    this.id = idDatatable;
    this.element = document.getElementById(this.id);
    this.options = {
        allText: 'Tất cả',
        bordered: true,
        borderless: false,
        borderColor: null,
        color: null,
        dark: null,
        defaultValue: '&nbsp;',
        edit: false,
        entries: 15,
        entriesOptions: ['All'],
        fixedHeader: true,
        forceSort: false,
        fullPagination: false,
        hover: true,
        loading: true,
        loaderClass: 'bg-primary',
        loadingMessage: 'Đang tải dữ liệu',
        maxWidth: null,
        maxHeight: '100%',
        selectable: false,
        multi: false,
        noFoundMessage: 'Không có dữ liệu',
        pagination: false,
        sm: true,
        striped: false,
        rowsText: 'Rows per page',
        ofText: 'of',
        clickableRows: enableRowClick,
        sortField: null,
        sortOrder: 'asc',
    };
    this.columns = [];
    this.keyColumn = '';
    this.rows = [];

    this.addCol = function (colIndex, colId, colName, colSize = null, format = null) {
        this.columns.push({
            colIndex: colIndex,
            label: colName,
            field: colId,
            fixed: false,
            width: colSize,
            sort: false,
            format: format
        });
        this.columns = this.columns.sort((a, b) => { return a.colIndex - b.colIndex })
        return this;
    }
    this.instance = null;
    this.createInstance = function () {
        if (this.element === null) {
            return this;
        }
        if (this.instance !== null) this.instance.dispose();
        const tblCols = this.columns.map(row => {
            return {
                label: row.label,
                field: row.field,
                fixed: row.fixed,
                width: row.width,
                sort: row.sort,
                format: row.format
            }
        });
        const displayRows = this.rows;
        while (displayRows.length < pageRows) displayRows.push({});
        this.instance = new mdb.Datatable(this.element, { columns: this.columns, rows: displayRows }, this.options);
        return this;
    }
    this.update = function () {
        if (this.instance === null) {
            return this.createInstance();
        }
        const displayRows = this.rows;
        while (displayRows.length < pageRows) displayRows.push({});
        this.instance.update({ columns: this.columns, rows: displayRows }, this.options);
        return this;
    }
    this.rowClick = function (func) {
        this.element.addEventListener('rowClick.mdb.datatable', func);
    }
    this.onRender = function (func) {
        this.element.addEventListener('render.mdb.datatable', func);
    }
    return this;
}