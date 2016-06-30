/*global ThirdPartyWinLoss, ThirdPartyWinLossOption, Site */
/// <reference path="/@JSense.js" />
///
(function () {
    "use strict";
    var VALID_MONTH_RANGE = 3;
    function lastDateOfMonth(year, month) {
        return new Date(year, month + 1, 0);
    }

    ThirdPartyWinLoss.custom.initialize = function () {
        Site.addUserGuide({
            resourceName: "GoldDeluxeWinLoss",
            title: Site.getSingleResource("GoldDeluxeWinLoss")
        });
        Site.setConstraintMonthRange(VALID_MONTH_RANGE, ThirdPartyWinLossOption.urlExcelData);
    };

    ThirdPartyWinLoss.custom.getDrillDownDates = function (param, datetime) {
        var fdate = new Date(Date.parse(datetime));
        var tdate = new Date(Date.parse(datetime));
        if (Number(param.ReportBy) === 2) {
            var fday = 1;
            var month = fdate.getMonth();
            var year = fdate.getFullYear();
            var orignalFDate = new Date(param.FromDate);
            if (month === orignalFDate.getMonth()) {
                fday = orignalFDate.getDate();
            }
            fdate = new Date(year, month, fday, 0, 0, 0, 0);
            var originalTDate = new Date(param.ToDate);

            if (month === originalTDate.getMonth()) {
                var tday = originalTDate.getDate();
                tdate = new Date(year, month, tday, 0, 0, 0, 0);
            }
            else {
                tdate = lastDateOfMonth(year, month);
            }
        }

        return {
            FromDate: fdate,
            ToDate: tdate
        };
    };
})();