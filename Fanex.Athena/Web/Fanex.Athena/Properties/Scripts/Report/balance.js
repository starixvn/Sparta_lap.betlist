/// <reference path="/@JSense.js" />
var Balance = (function ($) {
    return {
        initialize: function () {
            Balance.initializeParam();
            Balance.initEvent();
        },

        initEvent: function () {

            $('#ExportToExcel').click(function () {
                Balance.initializeParam();
                $('#IsExportExcel').val(true);
                $('#BalanceForm').submit();
            });

            //Submit button
            $('#SubmitButton').click(function () {
                Balance.submit();
            });
        },
        initializeParam: function () {
            var month = $("#Month").val();
            var year = $("#Year").val();
            var fromDate = new Date(year, month, 1);
            var toDate = new Date(fromDate.getFullYear(), fromDate.getMonth() + 1, 0);
            $("#FromDate").val(fromDate.toDateString());
            $("#ToDate").val(toDate.toDateString());
        },
        submit: function () {
            Site.blockUI();
            Balance.initializeParam();
            $('#IsExportExcel').val(false);
            $('#BalanceForm').submit();
        }
    };
})(jQuery);


$(window).load(function () {
    Balance.initialize();
});