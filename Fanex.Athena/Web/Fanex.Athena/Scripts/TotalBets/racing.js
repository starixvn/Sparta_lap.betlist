/*global Site, Totalbets*/
/// <reference path="/@JSense.js" />
"use strict";
var Racing = (function ($) {
    return {
        initialize: function () {
            Totalbets.initialize();
            Racing.initEvent();
            Site.addUserGuide({
                resourceName: "TotalBetsRacing",
                title: Site.getSingleResource("TotalBets") + " - " + Site.getSingleResource("Racing")
            });
        },
        initEvent: function () {
            // Submit event.
            $("#SubmitButton").on("click", function () {
                Racing.submit();
            });

            // Forcast number click
            $('#Refresh-Content').on('click', '.forecast-number-none', function () {
                var matchId = $(this).attr("data-match-id");
                var bettypeId = $(this).attr("data-bet-type");
                Totalbets.viewRunningBetListMatch(matchId, bettypeId);
            });
        },
        submit: function () {
            Site.blockUI();
            $("#HorseRacingForm").submit();
        }
    };
})(jQuery);