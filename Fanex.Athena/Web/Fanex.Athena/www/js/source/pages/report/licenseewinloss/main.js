define(["./licenseewinloss"], function (LicenseeWinLoss) {
    "use strict";
    $(function () {
        LicenseeWinLoss.initialize();
        window.licenseewinlossDrilldown = LicenseeWinLoss.drillDown;

        $(window).on("resize", LicenseeWinLoss.onWindowResize);

        $(window).on("unload", function () {
            $(window).unbind("resize");
        });
    });
});