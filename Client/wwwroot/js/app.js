window.downloadFile = function (path) {
    var link = document.createElement("a");
    link.href = "api/" + path;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};

window.persianDatePicker = function (selector) {
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
            calendarSwitch: {
                enabled: false
            },
            submitButton: {
                enabled: false
            }
        }
    });
};

window.masonry = function (selector) {
    $(selector).masonry({ 'percentPosition': true, 'originLeft': false });
};
