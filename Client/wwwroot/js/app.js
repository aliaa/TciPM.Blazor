window.downloadFile = function (path) {
    var link = document.createElement("a");
    link.href = "api/" + path;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};

window.persianDatePicker = function (selector) {
    var lastFocusedElem = null;
    $(selector).focus(function () {
        lastFocusedElem = this;
    });

    $(selector).pDatepicker({
        initialValueType: 'persian',
        initialValue: false,
        format: 'YYYY/MM/DD',
        autoClose: true,
        observer: true,
        calendar: {
            persian: {
                locale: 'fa'
            }
        },
        toolbox: {
            enabled: false
        },
        onSelect: function (date) {
            if (lastFocusedElem)
                lastFocusedElem.dispatchEvent(new Event('change'));
        }
    });
    
};

window.masonry = function (selector) {
    $(selector).masonry({ 'percentPosition': true, 'originLeft': false });
};

window.initpopover = function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    });
};