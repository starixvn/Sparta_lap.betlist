/*global Site*/
/// <reference path="/@JSense.js" />
var WinLossByBetType = (function ($) {
    "use strict";
    return {
        initialize: function () {
            WinLossByBetType.initEvent();
            Site.createSortingIcon(WinLossByBetType.sortReport);            
        },

        initEvent: function () {
            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                $('#WinLossByBetTypeForm').submit();
            });

            $('#SubmitButton').click(function () {
                Site.blockUI();
                $('#IsExportExcel').val(false);
                $('#WinLossByBetTypeForm').submit();
            });
        },
        sortReport: function () {
            Site.blockUI();
            $('#IsExportExcel').val(false);
            $('#WinLossByBetTypeForm').submit();
            return false;
        }
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    WinLossByBetType.initialize();
});