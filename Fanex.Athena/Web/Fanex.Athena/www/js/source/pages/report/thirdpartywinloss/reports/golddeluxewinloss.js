define(["jquery", "../common/thirdpartywinloss"],
    function ($, ThirdPartyWinLoss) {
        "use strict";
        function initialize() {
            ThirdPartyWinLoss.initialize({
                UserGuideResourceName: "GoldDeluxeWinLoss",
                UserGuideTitle: "GoldDeluxeWinLoss",
                ValidMonthRange: 3,
                ReportTitle: "GoldDeluxeWinLoss"
            });
        }

        var _me = {
            initialize: initialize
        };

        return _me;
    });