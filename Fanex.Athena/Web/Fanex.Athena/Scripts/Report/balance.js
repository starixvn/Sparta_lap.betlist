/*global Site */
/// <reference path="/@JSense.js" />
var Balance = (function ($) {
    "use strict";
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

            $("#Year").change(function () {
                var historyDate = Site.getHistoryDate();
                var historyMonth = historyDate.getMonth();
                var $month = $("#Month");

                if (parseInt(this.value) === historyDate.getFullYear()) {
                    if ($month.val() < historyMonth) {
                        $month.val(historyMonth);
                    }

                    $month.find("option").filter(function () {
                        return this.value < historyMonth;
                    }).hide();
                } else {
                    $month.children().show();
                }
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
    "use strict";
    Balance.initialize();
});