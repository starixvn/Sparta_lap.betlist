/// <reference path="/@JSense.js" />
var Racing = (function ($) {
    return {
        initialize: function () {
            Totalbets.initialize();
            Racing.initEvent();
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
})(jQuery)