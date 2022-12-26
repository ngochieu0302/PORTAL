function MultitabService() {
    this.arrTab = [];
    this.currentTabIndex = 0;
    this.addTab = (tabId) => {
        const tab = document.getElementById(tabId);
        if (tab === null || tab === undefined) return this;
        if (this.arrTab.length > 0) tab.classList.add('d-none');
        this.arrTab.push(tab);
        return this;
    }
    this.clearTabs = () => {
        this.arrTab = [];
        this.currentTabIndex = 0;
        return this;
    }
    this.currentTabId = () => {
        if (this.arrTab.length == 0) return null;
        return this.arrTab[this.currentTabIndex].id;
    }
    this.showTabByIndex = (index) => {
        if (index >= this.arrTab.length) return this;
        if (index == this.currentTabIndex) return this;
        this.arrTab.forEach(tab => {
            tab.classList.add('d-none');
        })
        this.arrTab[index].classList.remove('d-none');
        this.currentTabIndex = index;
        return this;
    }
    this.showTabById = (tabId) => {
        if (tabId == this.currentTabId()) return this;
        this.arrTab.forEach(tab => {
            tab.classList.add('d-none');
        });
        const tab = document.getElementById(tabId);
        if (tab === null || tab === undefined) return this;
        tab.classList.remove('d-none');
        this.currentTabIndex = this.arrTab.indexOf(tab);
        return this;
    }
    return this;
}