/// <reference path="/@JSense.js" />
var WinLossByBetType = (function ($) {
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
        sortReport: function (sortColumn) {
            Site.blockUI();
            $('#IsExportExcel').val(false);
            $('#WinLossByBetTypeForm').submit();
            return false;
        }
    };
})(jQuery);

$(window).load(function () {
    WinLossByBetType.initialize();
});