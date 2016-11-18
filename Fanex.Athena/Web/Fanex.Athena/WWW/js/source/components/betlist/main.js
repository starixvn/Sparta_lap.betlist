define(['jquery', './betlist', './commonbetlist'], function ($, BetList, CommonBetList) {
    "use strict";

    window.ViewResult = BetList.showResult;

    window.showMP = function (transId) {
        var divEvent = $('#divEvent_' + transId);
        divEvent.addClass("mx-parlay"); //to make line-through for void-parlay-ticket
        BetList.showMixParlayDetail(transId, divEvent);
    };

    window.showCombMP = function (transId, index) {
        var divEvent = $('#divEvent_' + transId + "" + index);
        divEvent.addClass("mx-parlay"); //to make line-through for void-parlay-ticket
        BetList.showMixParlayDetail(transId, divEvent);
    };

    window.showNewCombMP = BetList.showNewParlayDetail;
    window.showCombMPDetail = BetList.showSystemParlayDetail;
    window.showMicroGamingRnGBetsDetails = BetList.showMicroGamingRnGBetsDetails;
    window.showAGCasinoDetails = BetList.showAGCasinoDetails;
    window.showRnGCasinoDetails = BetList.showRnGCasinoDetails;
    window.showVoidBridgeDetails = BetList.showVoidBridgeDetails;
    window.showVoidbridgeResult = BetList.viewVoidbridgeResult;
    window.showTodayRunningBetsDetails = BetList.showAllBetDetails;
    window.showColossusBetsDetails = BetList.showColossusBetsDetails;

    if (!window._IsInJsTestingMode) {
        $(window).on("load", function () {
            CommonBetList.initialize();
        });
    }
});