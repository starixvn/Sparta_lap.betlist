/// <reference path="/@JSense.js" />
var FirstGoalLastGoal = (function ($) {
    return {
        initialize: function () {
            Totalbets.initialize();
            FirstGoalLastGoal.initEvent();
        },
        initEvent: function () {
            // Submit event.
            $("#SubmitButton").on("click", function () {
                FirstGoalLastGoal.submit();
            });

            // Home team click
            $('#Refresh-Content').on('click', '.team', function () {
                var matchId = $(this).attr("data-match-id");
                Totalbets.viewRunningBetListMultiBettype(matchId, 14);
            });
        },
        submit: function () {
            Site.blockUI();
            $("#FirstGoalLastGoalForm").submit();
        }
    };
})(jQuery)