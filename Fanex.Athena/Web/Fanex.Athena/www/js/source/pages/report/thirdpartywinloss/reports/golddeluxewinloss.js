define(["jquery", "../common/thirdpartywinloss", "./golddeluxewinloss.option"],
    function ($, ThirdPartyWinLoss, GoldDeluxeWinLossOption) {
        "use strict";
        function initialize() {
            GoldDeluxeWinLossOption.licenseeId = parseInt($("#drillDownLicenseeId").val());

            ThirdPartyWinLoss.initialize({
                UserGuideResourceName: "GoldDeluxeWL",
                UserGuideTitle: "GoldDeluxeWinLoss",
                ValidMonthRange: 3,
                ReportTitle: "GoldDeluxeWinLoss",
                GridOption: GoldDeluxeWinLossOption
            });
        }

        var _me = {
            initialize: initialize
        };

        return _me;
    });