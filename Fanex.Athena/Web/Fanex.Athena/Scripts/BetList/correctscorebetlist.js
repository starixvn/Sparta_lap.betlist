/*global Site, BetList*/
/// <reference path="/@JSense.js" />
"use strict";
var CorrectScoreBetList = (function ($) {
    return {
        initialize: function () {
            CorrectScoreBetList.initEvent();
            CorrectScoreBetList.submit();
            Site.addUserGuide({
                resourceName: "TotalBetsCorrectScore",
                title: Site.getSingleResource("TotalBets") + " - " + Site.getSingleResource("CorrectScore")
            });
        },

        initEvent: function () {
            //Submit button
            $('#SubmitButton').click(function () {
                CorrectScoreBetList.submit();
            });
        },

        submit: function () {
            // Site.blockUI();
            BetList.initialize();
            // Load Data
            var params = {};
            params = Site.setLicenseeOptionParam(params);
            var url = Site.resolveClientUrl('api/betlist/CorrectScoreBetList');
            BetList.loadData(params, url);
        }
    };
})(jQuery);

$(window).load(function () {
    CorrectScoreBetList.initialize();
});