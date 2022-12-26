function LoadingService() {
    this.element = null;
    this.elementState = false;
    this.createElement = () => {
        if (this.elementState) return this;
        this.element = document.createElement('div');
        this.element.classList.add('ESCS_loading');
        this.element.innerHTML = `  <div class="spinner-border loading-icon" role="status"></div>
                                    <span class="loading-text fs-4">ESCS Portal</span>`
        document.body.appendChild(this.element);
        this.elementState = true;
        return this;
    }

    this.initState = false;
    this.init = () => {
        if (this.initState) return this;
        this.createElement();
        const loading = document.querySelector('.ESCS_loading');
        new mdb.Loading(loading, {
            scroll: false,
            parentSelector: 'body',
            backdropID: 'ESCS_loadingBackdrop'
        });
        this.initState = true;
        return this;
    }
    this.destroy = () => {
        if (!this.elementState) return this;
        if (!this.initState) return this;
        this.element.remove();
        document.getElementById('ESCS_loadingBackdrop').remove();
        this.elementState = false;
        this.initState = false;
        return this;
    }
}