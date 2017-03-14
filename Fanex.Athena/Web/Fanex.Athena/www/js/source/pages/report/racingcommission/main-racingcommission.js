"use strict";
define(['./racingcommission'], function (RacingCommission) {
    $(function () {
        RacingCommission.initialize();

        $(window).on("resize", RacingCommission.resizeGrid);

        $(window).on("unload", function () {
            $(window).off("resize");
        });
    });
});