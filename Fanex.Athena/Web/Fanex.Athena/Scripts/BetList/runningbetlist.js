/// <reference path="/@JSense.js" />
var RunningBetList = (function ($) {
    return {
        initialize: function () {
            RunningBetList.initEvent();
            RunningBetList.switchItemSelectedMixParlay();
            var bettype = RunningBetList.getUrlParams("bettype");
            RunningBetList.submit(bettype);
        },

        initEvent: function () {
            //Submit button
            $('#SubmitButton').click(function () {
                var bettype = RunningBetList.getUrlParams("bettype");
                RunningBetList.submit(bettype);
            });
        },

        submit: function (bettype) {
            // Site.blockUI();
            BetList.initialize();
            // Load Data
            var params = { bettype: bettype };
            params = Site.setLicenseeOptionParam(params);
            var url = Site.resolveClientUrl('api/betlist/runningbetlist');
            BetList.loadData(params, url);
        },

        switchMixParlayTotalBet: function (bettype) {
            if (bettype.length == 0) return;
            var url = document.location.href;
            url = QueryString.setParam(url, "bettype", bettype);
            window.location.href = url;
        },

        showCombMPDetail: function (transId, refNo, betId, custId, betTeam, baseCurrency) {
            if (custId == 0) return;
            var url = Site.resolveClientUrl("_BetList/Popup/ViewSystemParlay.aspx");
            url = QueryString.setParam(url, "transid", transId);
            url = QueryString.setParam(url, "refno", refNo);
            url = QueryString.setParam(url, "betteam", betTeam);
            url = QueryString.setParam(url, "betid", betId);
            url = QueryString.setParam(url, "custid", custId);
            url = QueryString.setParam(url, "basecurrency", baseCurrency);

            Site.blockUI();
            $("#popupSystemParlay").load(url).dialog();
            //window.location.href = url;
        },

        switchItemSelectedMixParlay: function () {
            var betType = RunningBetList.getUrlParams("bettype");
            var mixParlay = $("#mixparlay_9");
            var systemParlay = $("#mixparlay_29");
            if (betType == 9) {
                mixParlay.removeClass("active-switch-link");
                mixParlay.addClass("inactive-switch-link");

                systemParlay.removeClass("inactive-switch-link");
                systemParlay.addClass("active-switch-link");
            }
            else {
                mixParlay.removeClass("inactive-switch-link");
                mixParlay.addClass("active-switch-link");

                systemParlay.removeClass("active-switch-link");
                systemParlay.addClass("inactive-switch-link");
            }
        },

        getUrlParams: function (sParam) {
            var sPageURL = window.location.search.substring(1);
            var sURLVariables = sPageURL.split('&');
            for (var i = 0; i < sURLVariables.length; i++) {
                var sParameterName = sURLVariables[i].split('=');
                if (sParameterName[0] == sParam) {
                    return sParameterName[1];
                }
            }
        },

        exportToExcel: function () {
            $("#ExportToExcel").click(function () {
                var url = window.location.href;
                url = Site.setLicenseeOptionParam(url);
                url = QueryString.setParam(url, "IsExportExcel", true);
                window.location.href = url;
            });
        }
    };
})(jQuery);

$(window).load(function () {
    RunningBetList.initialize();
    RunningBetList.exportToExcel();
});