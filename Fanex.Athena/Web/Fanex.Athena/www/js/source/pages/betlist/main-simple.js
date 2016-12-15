define(['jquery', './betlist', './commonbetlist'], function ($, BetList, CommonBetList) {
    "use strict";

    if (!window._IsInJsTestingMode) {
        $(window).on("load", function () {
            CommonBetList.initialize();
        });
    }
});