define(['jquery', './runningbetlist', './main'], function ($, RunningBetList) {
    "use strict";

    $(function () {
        RunningBetList.initialize();
        RunningBetList.exportToExcel();
    });
});