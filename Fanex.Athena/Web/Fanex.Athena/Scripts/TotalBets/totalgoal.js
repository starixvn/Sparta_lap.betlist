/*global Site,Totalbets */
/// <reference path="/@JSense.js" />
"use strict";
var TotalGoal = (function ($) {
    return {
        initialize: function () {
            Totalbets.initialize();
            TotalGoal.initEvent();
            Site.addUserGuide({
                resourceName: "TotalBetsTotalGoal",
                title: Site.getSingleResource("TotalBets") + " - " + Site.getSingleResource("TotalGoal")
            });
        },
        initEvent: function () {
            var totalGoalBettype = 6, firstHalfTotalGoal = 126;
            var bettypeParameter = totalGoalBettype + "," + firstHalfTotalGoal;

            // Submit event.
            $("#SubmitButton").on("click", function () {
                TotalGoal.submit();
            });

            // Home team click
            $('#Refresh-Content').on('click', '.team', function () {
                var matchId = $(this).attr("data-match-id");
                Totalbets.viewRunningBetListMultiBettype(matchId, bettypeParameter);
            });
        },
        submit: function () {
            Site.blockUI();
            $("#TotalGoalForm").submit();
        }
    };
})(jQuery);