define(["jquery", "lib/jquery.monthpicker"],
    function ($) {
        "use strict";
        var PICKER_SELECTOR = "#month-picker";
        var FORMAT = "MM/dd/yyyy";

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
                format: FORMAT,
                month: fromdate,
                fromDay: fromdate.getDate(),
                toDay: todate.getDate(),
                afterChangeMonth: function () {
                    $fromDate.val($picker.monPickr("getFromDate", FORMAT));
                    $todate.val($picker.monPickr("getToDate", FORMAT));
                },
                afterChangeDay: function () {
                    $fromDate.val($picker.monPickr("getFromDate", FORMAT));
                    $todate.val($picker.monPickr("getToDate", FORMAT));

                    if (option && typeof option.customAfterChangeDay === "function") {
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