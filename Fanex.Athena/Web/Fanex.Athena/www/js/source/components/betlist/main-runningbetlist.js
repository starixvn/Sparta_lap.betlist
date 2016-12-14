define(['jquery', './betlistrunning'], function ($, RunningBetList) {
    "use strict";

    $(function () {
        RunningBetList.initialize();
        RunningBetList.exportToExcel();
    });
});