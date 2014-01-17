/// <reference path="/@JSense.js" />
var HomeDrawAwayNoBet = (function ($) {
    return {
        initialize: function () {
            Totalbets.initialize();
            HomeDrawAwayNoBet.initEvent();
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