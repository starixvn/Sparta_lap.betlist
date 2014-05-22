/// <reference path="/@JSense.js" />
var TotalGoal = (function ($) {
    var totalGoalBettype = 6, firstHalfTotalGoal = 126;
    var bettypeParameter=totalGoalBettype+","+firstHalfTotalGoal;
    return {
        initialize: function () {
            Totalbets.initialize();
            TotalGoal.initEvent();
        },
        initEvent: function () {
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