/*global Site,Totalbets */

/// <reference path="/@JSense.js" />
var FirstGoalLastGoal = (function ($) {
    "use strict";
    return {
        initialize: function () {
            Totalbets.initialize();
            FirstGoalLastGoal.initEvent();
        },
        initEvent: function () {
            var firstGoalLastGoal = 14, firstHalfFirsGoalLastGoal = 127;
            var bettypeParameter = firstGoalLastGoal + "," + firstHalfFirsGoalLastGoal;

            // Submit event.
            $("#SubmitButton").on("click", function () {
                FirstGoalLastGoal.submit();
            });

            // Home team click
            $('#Refresh-Content').on('click', '.team', function () {
                var matchId = $(this).attr("data-match-id");
                Totalbets.viewRunningBetListMultiBettype(matchId, bettypeParameter);
            });
        },
        submit: function () {
            Site.blockUI();
            $("#FirstGoalLastGoalForm").submit();
        }
    };
})(jQuery);