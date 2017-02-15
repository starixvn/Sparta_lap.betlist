define(["jquery", "common/site", "lib/jquery.monthpicker"],
    function ($, Site) {
        "use strict";

        var _historyDate = new Date($("#HistoryDate").val());

        function initMonthPicker() {
            var $picker = $('#month-picker');
            var $fromDate = $('#FromDate');
            var $todate = $('#ToDate');

            var fromdate = new Date($fromDate.val());
            var todate = new Date($todate.val());

            $picker.monPickr({
                minDate: _historyDate,
                showDateRange: true,
                fromDayId: 'fDay',
                toDayId: 'tDay',
                format: 'MM/dd/yyyy',
                month: fromdate,
                fromDay: fromdate.getDate(),
                toDay: todate.getDate(),
                afterChangeMonth: function () {
                    $fromDate.val($picker.monPickr("getFromDate", "MM/dd/yyyy"));
                    $todate.val($picker.monPickr("getToDate", "MM/dd/yyyy"));
                },
                afterChangeDay: function () {
                    $fromDate.val($picker.monPickr("getFromDate", "MM/dd/yyyy"));
                    $todate.val($picker.monPickr("getToDate", "MM/dd/yyyy"));
                }
            });
        }

        return {
            initMonthPicker: initMonthPicker
        };
    });