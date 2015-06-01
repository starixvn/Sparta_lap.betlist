/*global Site, QueryString, RefreshCountDown*/
/// <reference path="/@JSense.js" />
"use strict";
var Totalbets = (function ($) {
    return {
        resizeGrid: function () {
            var $grid = $(".data-content");
            if ($grid.length === 0) {
                return;
            }

            var gridTop = $grid.offset().top;

            var gridHeight = window.innerHeight - gridTop - 35;
            gridHeight = Math.max(150, gridHeight);
            gridHeight = Math.min($(".data-content .report-table").height(), gridHeight);
            $grid.attr("style", "height: " + gridHeight + "px !important; max-height: 100% !important;");
        },
        initEvent: function () {
            // Init refresh countdown
            RefreshCountDown.initialize();

            // Forcast number click
            $("#Refresh-Content").on("click", '.forcast-number', function () {
                var matchId = $(this).attr("data-match-id");
                var betType = $(this).attr("data-bet-type");
                Totalbets.viewRunningBetListMatch(matchId, betType);
            });

            $(document).ajaxComplete(function (/*event, request, settings*/) {
                Totalbets.resizeGrid();
            });
        },
        viewRunningBetListMatch: function (matchId, betType) {
            var url = Site.resolveClientUrl("BetList/RunningBetListOfMatch");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "matchid", matchId);
            url = QueryString.setParam(url, "bettype", betType);
            url = QueryString.setParam(url, "isShowSumStake", true);

            window.location.href = url;
        },
        viewRunningBetListMultiBettype: function (matchId, betType) {
            var url = Site.resolveClientUrl("BetList/RunningBetListByMultipleBettypeOfMatch");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "matchid", matchId);
            url = QueryString.setParam(url, "multiplebettype", betType);
            url = QueryString.setParam(url, "isShowSumStake", true);

            window.location.href = url;
        },
        initialize: function () {
            Site.unBlockUI();
            this.initEvent();
            Totalbets.resizeGrid();
        }
    };
})(jQuery);

$(window).resize(function () {
    Totalbets.resizeGrid();
});

$(window).unload(function () {
    $(window).unbind("resize");
});