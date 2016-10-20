define(["./licenseewinloss"], function (LicenseeWinLoss) {
    $(function () {
        LicenseeWinLoss.initialize();
        window.licenseewinlossDrilldown = LicenseeWinLoss.drillDown;
    });
});