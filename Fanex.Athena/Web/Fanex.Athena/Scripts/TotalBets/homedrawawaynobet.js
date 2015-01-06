/*global Site, Totalbets*/
/// <reference path="/@JSense.js" />
"use strict";
var HomeDrawAwayNoBet = (function ($) {
    return {
        initialize: function () {
            Totalbets.initialize();
            HomeDrawAwayNoBet.initEvent();
            Site.addUserGuide({
                resourceName: "TotalBetsHomeDrawAwayNoBet",
                title: Site.getSingleResource("TotalBets") + " - " + Site.getSingleResource("HomeDrawAwayNoBet")
            });
        },
        initEvent: function () {
            // Submit event.
            $("#SubmitButton").on("click", function () {
                HomeDrawAwayNoBet.submit();
            });

            // Home team click
            $('#Refresh-Content').on('click', '.team', function () {
                var matchId = $(this).attr("data-match-id");
                Totalbets.viewRunningBetListMatch(matchId, 14);
            });
        },
        submit: function () {
            Site.blockUI();
            $("#HomeDrawAwayNoBetForm").submit();
        }
    };
})(jQuery);