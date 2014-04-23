/*jshint strict: false */
/*global Site */
var WareHouseWinLoss = (function ($) {
    return {
        serializeParam: function () {
            $('#ReportBy').val($('#SelectReportBy').val());
        },

        initExcelExporting: function () {
            WareHouseWinLoss.serializeParam();
            $('#IsExportExcel').val(true);
            $('#WarehouseWinLossForm').submit();
        },
        initialize: function () {
            $('#SubmitButton').click(function () {
                WareHouseWinLoss.submitForm();
            });

            $('#ExportToExcel').click(function () {
                WareHouseWinLoss.initExcelExporting();
            });
        },

        submitForm: function () {
            if (WareHouseWinLoss.checkDateRange()) {
                Site.blockUI();
                WareHouseWinLoss.serializeParam();
                $('#IsExportExcel').val(false);
                $('#WarehouseWinLossForm').submit();
            }
        },

        checkDateRange: function () {
            var fromDate = Date.parse($("#FromDate").val());
            var toDate = Date.parse($("#ToDate").val());

            // Check date rang limit
            var daysInMonth = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
            var timeInOneDay = 86400000; // milisecond in 1 day    
            var fromMonth = fromDate.getMonth();
            var maxDaysLimit = daysInMonth[fromMonth++ % 11] + daysInMonth[fromMonth++ % 11] + daysInMonth[fromMonth % 11];
            var dateDiff = Math.round(toDate.getTime() - fromDate.getTime()) / timeInOneDay;
            if (dateDiff > maxDaysLimit) { // over 3 months
                alert('Support 3 month data per submit!');
                return false;
            };

            return true;
        }
    };
})(jQuery);

$(window).load(function () {
    WareHouseWinLoss.initialize();
});