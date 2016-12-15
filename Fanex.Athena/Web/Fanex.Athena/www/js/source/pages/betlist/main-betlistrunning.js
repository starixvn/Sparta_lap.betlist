define(['jquery', './betlistrunning'], function ($, BetListRunning) {
    "use strict";

    $(function () {
        BetListRunning.initialize();
        BetListRunning.exportToExcel();
    });
});