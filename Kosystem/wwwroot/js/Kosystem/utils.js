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

        for (var opt, j = 0; opt = opts[j]; j++) {
            if (opt.value === value) {
                selectElement.selectedIndex = j;
                return true;
            }
        }

        return false;
    }
}
