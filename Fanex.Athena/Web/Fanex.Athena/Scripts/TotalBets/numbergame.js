/*global Site, Totalbets, QueryString*/
/// <reference path="/@JSense.js" />
"use strict";
var NumberGame = (function ($) {
    return {
        initialize: function () {
            Totalbets.initialize();
            NumberGame.initEvent();
            Site.addUserGuide({
                resourceName: "TotalBetsNumberGame",
                title: Site.getSingleResource("TotalBets") + " - " + Site.getSingleResource("NumberGame")
            });
        },
        initEvent: function () {
            // Submit event.
            $("#SubmitButton").on("click", function () {
                NumberGame.submit();
            });

            // Home team click
            $('#Refresh-Content').on('click', '.forcast-number', function () {
                var matchId = $(this).attr("data-match-id");
                var betType = $(this).attr("data-bet-type");
                var isLive = $(this).attr("data-is-live");
                var betTeam = $(this).attr("data-bet-team");
                NumberGame.viewNumberGameBetList(matchId, betType, isLive, betTeam);
            });
        },
        submit: function () {
            Site.blockUI();
            $("#NumberGameForm").submit();
        },
        viewNumberGameBetList: function (matchId, betType, isLive, betTeam) {
            var url = Site.resolveClientUrl("RunningMatchBetList/Index");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "matchid", matchId);
            url = QueryString.setParam(url, "bettype", betType);
            url = QueryString.setParam(url, "LiveIndicator", isLive);
            url = QueryString.setParam(url, "betteam", betTeam);
            url = QueryString.setParam(url, "IsShowSumStake", true);

            window.location.href = url;
        }
    };
})(jQuery);