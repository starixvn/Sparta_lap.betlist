/*global Site, Totalbets*/
/// <reference path="/@JSense.js" />
"use strict";
var Outright = (function ($) {
    return {
        initialize: function () {
            Totalbets.initialize();
            Outright.initEvent();
        },
        initEvent: function () {
            // Submit event.
            $("#SubmitButton").on("click", function () {
                Outright.submit();
            });

            // Team click
            $('#Refresh-Content').on('click', '.team', function () {
                var matchId = $(this).attr("data-match-id");
                Totalbets.viewRunningBetListMatch(matchId, 10);
            });
        },
        submit: function () {
            Site.blockUI();
            $("#OutrightForm").submit();
        }
    };
})(jQuery);