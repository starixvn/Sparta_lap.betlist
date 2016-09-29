/*global ThirdPartyWinLoss, ThirdPartyWinLossOption, Site */
/// <reference path="/@JSense.js" />
///
(function () {
    "use strict";
    var VALID_MONTH_RANGE = 3;
    
    ThirdPartyWinLoss.custom.initialize = function () {
        Site.addUserGuide({
            resourceName: "GoldDeluxeWinLoss",
            title: Site.getSingleResource("GoldDeluxeWinLoss")
        });
        Site.setConstraintMonthRange(VALID_MONTH_RANGE, ThirdPartyWinLossOption.urlExcelData);
    };
})();