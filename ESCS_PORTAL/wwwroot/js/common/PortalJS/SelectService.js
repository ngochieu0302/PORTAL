function SelectService(idSelect, enableSearch = false, size = 'default') {
    this.id = idSelect;
    this.element = document.getElementById(this.id);
    this.options = {
        autoSelect: false,
        container: 'body',
        clearButton: false,
        disabled: false,
        displayedLabels: 5,
        filter: enableSearch,
        filterDebounce: 300,
        formWhite: false,
        invalidFeedback: 'Sai thông tin',
        noResultText: 'Không tìm thấy',
        placeholder: '',
        size: size,
        optionsSelectedLabel: 'đã chọn',
        optionHeight: 38,
        selectAll: true,
        selectAllLabel: 'Chọn tất cả',
        searchPlaceholder: 'Tìm kiếm',
        validation: false,
        validFeedback: 'Hợp lệ',
        visibleOptions: 5
    };

    this.instance = null;
    this.getInstance = function () {
        if (this.element === null) {
            return this;
        }
        this.instance = mdb.Select.getOrCreateInstance(this.element);
        return this;
    }
    this.createInstance = function () {
        if (this.element === null) {
            return this;
        }
        if (this.instance !== null) this.instance.dispose();
        this.instance = new mdb.Select(this.element, this.options);
        return this;
    }

    this.dataArray = [];
    this.valueField = null;
    this.displayField = null;

    this.setDataSource = function (dataArray, valueField, displayField, firstItem = null) {
        this.dataArray = [...dataArray];
        this.valueField = valueField;
        this.displayField = displayField;
        if (firstItem != null) this.dataArray.splice(0, 0, firstItem);
        let innerHTML = '';
        this.dataArray.forEach((item) => innerHTML += `<option value="${item[this.valueField]}">${item[this.displayField]}</option>`);
        this.element.innerHTML = innerHTML;
        return this;
    }
    this.onChange = function (func) {
        this.element.addEventListener('valueChange.mdb.select', func);
    }
    this.setValue = function (value) {
        this.element.querySelectorAll('option').forEach((opt) => {
            opt.removeAttribute("selected");
        })
        var querystr = 'option[value="' + value + '"]';
        var option = this.element.querySelector(querystr);
        if (option === null) return this;
        option.setAttribute("selected", "selected");
        return this;
    }
    return this;
}