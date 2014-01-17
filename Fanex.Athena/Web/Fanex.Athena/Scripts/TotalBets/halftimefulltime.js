/// <reference path="/@JSense.js" />
var HalfTimeFullTime = (function ($) {
    return {
        initialize: function () {
            Totalbets.initialize();
            HalfTimeFullTime.initEvent();
        },
        initEvent: function () {
            // Submit event.
            $("#SubmitButton").on("click", function () {
                HalfTimeFullTime.submit();
            });

            // Home team click
            $('#Refresh-Content').on('click', '.team', function () {
                var matchId = $(this).attr("data-match-id");
                Totalbets.viewRunningBetListMatch(matchId, 16);
            });
        },
        submit: function () {
            Site.blockUI();
            $("#HalfTimeFullTimeForm").submit();
        }
    };
})(jQuery)