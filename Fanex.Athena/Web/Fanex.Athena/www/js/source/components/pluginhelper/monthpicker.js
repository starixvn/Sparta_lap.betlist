define(["jquery", "lib/jquery.monthpicker"],
    function ($) {
        "use strict";
        var PICKER_SELECTOR = "#month-picker";

        var _historyDate = new Date($("#HistoryDate").val());

        function initMonthPicker(option) {
            var $picker = $(PICKER_SELECTOR);
            var $fromDate = $('#FromDate');
            var $todate = $('#ToDate');

            var fromdate = new Date($fromDate.val());
            var todate = new Date($todate.val());

            var defaultOption = {
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
                    if (typeof (option.customAfterChangeDay) === "function") {
                        option.customAfterChangeDay();
                    }
                }
            };

            if (option) {
                $.extend(defaultOption, option);
            }

            $picker.monPickr(defaultOption);
        }

        function getFromDateString() {
            return $(PICKER_SELECTOR).monPickr("getFromDate", "dd MMM yyyy");
        }

        function getToDateString() {
            return $(PICKER_SELECTOR).monPickr("getToDate", "dd MMM yyyy");
        }

        return {
            initMonthPicker: initMonthPicker,
            getFromDateString: getFromDateString,
            getToDateString: getToDateString
        };
    });