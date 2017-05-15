define(['./lastbet.filter', './lastbet.grid'], function (LastBetFilter, LastBetGrid) {
    "use strict";

    $(window).load(function () {
        LastBetGrid.initialize();
        LastBetFilter.initialize();

        LastBetGrid.startMonitor();
    });

    $(window).unload(function () {
        LastBetGrid.stopMonitor();
    });
});