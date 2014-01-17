/// <reference path="/@JSense.js" />
var WinLossByBetType = (function ($) {
    return {
        initialize: function () {
            WinLossByBetType.initEvent();
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
        }
    };
})(jQuery);

$(window).load(function () {
    WinLossByBetType.initialize();
});