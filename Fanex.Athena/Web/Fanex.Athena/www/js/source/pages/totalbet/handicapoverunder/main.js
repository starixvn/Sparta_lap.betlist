define(['./handicapoverunder'], function (HandicapOU) {
    "use strict";
    $(function () {
        HandicapOU.initialize();

        window.viewNewForecast = HandicapOU.viewNewForecast;
    });
});