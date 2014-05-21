/// <reference path="/@JSense.js" />
var TotalGoal = (function ($) {
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
                Totalbets.viewRunningBetListMultiBettype(matchId, 6);
            });
        },
        submit: function () {
            Site.blockUI();
            $("#TotalGoalForm").submit();
        }
    };
})(jQuery);