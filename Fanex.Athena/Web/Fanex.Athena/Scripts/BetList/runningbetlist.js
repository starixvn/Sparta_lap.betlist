/*global Site, QueryString, BetList */
/// <reference path="/@JSense.js" />
var RunningBetList = (function ($) {
    "use strict";
    return {
        initialize: function () {
            RunningBetList.initEvent();
            var bettype = RunningBetList.getUrlParams("bettype");
            RunningBetList.submit(bettype);
            Site.addUserGuide({
                resourceName: $('#userGuideName').val(),
                title: Site.getSingleResource("TotalBets") + " - " + $('#title').val()
            });
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
            var url = Site.resolveClientUrl('api/RunningBetList/Index');
            BetList.loadData(params, url);
        },

        getUrlParams: function (sParam) {
            var sPageURL = window.location.search.substring(1);
            var sURLVariables = sPageURL.split('&');
            for (var i = 0; i < sURLVariables.length; i++) {
                var sParameterName = sURLVariables[i].split('=');
                if (sParameterName[0] === sParam) {
                    return sParameterName[1];
                }
            }
        },

        exportToExcel: function () {
            $("#ExportToExcel").click(function () {
                var url = window.location.href;
                url = Site.setLicenseeOptionParam(url);
                url = QueryString.setParam(url, "IsExportExcel", true);
                var sortParams = BetList.getSortParams();
                if (sortParams) {
                    url = QueryString.setParam(url, "SortBy", sortParams.SortBy);
                    url = QueryString.setParam(url, "SortOrder", sortParams.SortOrder);
                }
                Site.redirect(url);
            });
        }
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    RunningBetList.initialize();
    RunningBetList.exportToExcel();
});