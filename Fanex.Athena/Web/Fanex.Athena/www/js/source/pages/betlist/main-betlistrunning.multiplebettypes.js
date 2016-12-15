define(['jquery', './betlistrunning.multiplebettypes'], function ($, BetListRunningMultipleBettypes) {
    "use strict";

    $(function () {
        BetListRunningMultipleBettypes.initialize();
        BetListRunningMultipleBettypes.exportToExcel();
    });
});