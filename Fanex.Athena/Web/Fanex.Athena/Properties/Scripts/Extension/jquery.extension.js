/// <reference path="/@JSense.js" />
(function ($) {
    $.fn.checkboxgroup = function () {

        var allChk = this.first();
        var elseChk = this.slice(1);

        allChk.prop('checked', elseChk.filter(":checked").length >= elseChk.length);

        allChk.change(function () {
            var thisIsChecked = $(this).is(':checked');
            elseChk.prop('checked', thisIsChecked);
        });

        elseChk.change(function () {
            if ($(this).is(':checked')) {
                allChk.prop('checked', elseChk.filter(":checked").length >= elseChk.length);
            } else {
                allChk.prop('checked', false);
            }
        });

        return this;
    };

    $.fn.showHide = function (isShow) {
        if (isShow) {
            this.show();
        } else {
            this.hide();
        }

        return this;
    };

    jQuery.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name] !== undefined) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };

    $.fn.getStyleObject = function () {
        /// <summary>
        /// don't work on ie8
        /// </summary>
        /// <returns type=""></returns>
        var dom = this.get(0);
        var style;
        var returns = {};

        if (window.getComputedStyle) {
            var camelize = function (a, b) {
                return b.toUpperCase();
            }
            style = window.getComputedStyle(dom, null);
            for (var i = 0, l = style.length; i < l; i++) {
                var prop = style[i];
                var camel = prop.replace(/\-([a-z])/, camelize);
                var val = style.getPropertyValue(prop);
                returns[camel] = val;
            }
            return returns;
        }
        try {
            if (dom.currentStyle) {
                style = dom.currentStyle;
                for (var prop in style) {
                    returns[prop] = style[prop];
                }
                return returns;
            }
            if (style = dom.style) {
                for (var prop in style) {
                    if (typeof style[prop] != 'function') {
                        returns[prop] = style[prop];
                    }
                }
                return returns;
            }
        } catch (err) {

        }
        return returns;
    }

    jQuery.fn.tableRowSpan = function (colIndexs, effectToIndex) {
        /// <summary>
        /// Created by Alex@20130228
        /// set table rowspan
        /// </summary>
        /// <param name="colIndexs">column indexs</param>
        /// <param name="effectToIndex">set rowspan for this columns base on lasted column in [colIndexs]</param>
        if (effectToIndex !== undefined) {
            if ($.inArray(effectToIndex, colIndexs) > -1) {
                return;
            }
        }
        return this.each(function () {
            for (var i = 0; i < colIndexs.length; i++) {
                var colIdx = colIndexs[i];
                var that;
                var ethat;
                var ethis;
                var rowspan = 1;
                var len = $('tbody tr', this).length - 1;
                $('tbody tr', this).each(function (index, row) {
                    var currentRow = this;
                    $('td:eq(' + colIdx + ')', this).each(function (col) {
                        ethis = $('td:eq(' + effectToIndex + ')', currentRow);
                        var found = that != null && $(this).html() == $(that).html();
                        if (found && index < len) {
                            rowspan += 1;
                            $(this).hide();
                            if (effectToIndex !== undefined) {
                                $(ethis).hide();
                            }
                            //.remove// do your action for the old cell here
                        }
                        else if (found && index == len) {
                            //do your action for lasted row
                            $(this).hide();
                            rowspan += 1;
                            $(that).attr("rowSpan", rowspan);
                            that = this;
                            if (effectToIndex !== undefined) {
                                $(ethis).hide();
                                $(ethat).attr("rowSpan", rowspan);
                                ethat = ethis;
                            }
                            rowspan = 1;
                            // do your action for the colSpan cell here
                        }
                        else {
                            if (effectToIndex !== undefined) {
                                $(ethat).attr("rowSpan", rowspan);
                                ethat = ethis;
                            }
                            $(that).attr("rowSpan", rowspan);
                            // do your action for the colSpan cell here
                            that = this;
                            rowspan = 1;
                        }
                        that = (that == null) ? this : that;
                        if (effectToIndex !== undefined) {
                            ethat = (ethat == null) ? ethis : ethat;
                        }
                        // set the that if not already set
                    });
                });
            }
        });
    };



})(jQuery);

jQuery.extend({
    format: function (pattern) {
        /// <summary>
        /// simply format method
        /// </summary>
        /// <returns type="string">formated string</returns>
        var formatted = arguments[0];
        var passValue = $.grep(arguments, function (n, i) { return i > 0; });
        for (arg in passValue) {
            formatted = formatted.replace("{" + arg + "}", passValue[arg]);
        }
        return formatted;
    }
});

