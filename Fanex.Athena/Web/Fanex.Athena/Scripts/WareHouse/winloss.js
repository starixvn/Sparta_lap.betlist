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
            Site.blockUI();
            WareHouseWinLoss.serializeParam();
            $('#IsExportExcel').val(false);
        }
    };
})(jQuery);

$(window).load(function () {
    WareHouseWinLoss.initialize();
});