window.Kosystem = {
    /**
     * @param {HTMLSelectElement} selectElement
     * @param {string} value
     */
    selectValue(selectElement, value) {
        var opts = selectElement.options;
        if (!opts) {
            return false;
        }

        for (var i = selectElement.options.length - 1; i >= 0; i--) {
            var opt = opts[i];

            if (opt && opt.value === value) {
                selectElement.selectedIndex = i;
                return true;
            }
        }

        return false;
    }
};
