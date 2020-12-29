
window.Kosystem = {
    /**
     * @param {HTMLSelectElement} selectElement
     * @param {string} value
     */
    selectValue: function (selectElement, value) {
        var opts = selectElement.options;
        if (!opts) {
            return false;
        }

        var index = Array.from(opts)
            .findIndex(function (opt) { return opt.value === value });

        if (index !== -1) {
            selectElement.selectedIndex = index;
            return true;
        } else {
            return false;
        }
    }
};
