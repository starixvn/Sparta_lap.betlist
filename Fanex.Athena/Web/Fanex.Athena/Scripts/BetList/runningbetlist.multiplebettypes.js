/*global Site, QueryString, BetList */
/// <reference path="/@JSense.js" />
var RunningBetListMultipleBettype = (function ($) {
    "use strict";

    return {
        initialize: function () {
            RunningBetListMultipleBettype.initEvent();
            RunningBetListMultipleBettype.submit();
            Site.addUserGuide({
                resourceName: $('#userGuideName').val(),
                title: Site.getSingleResource("TotalBets") + " - " + $('#title').val()
            });
        },

        initEvent: function () {
            $('#SubmitButton').click(RunningBetListMultipleBettype.submit);
        },

        submit: function () {
            BetList.initialize();
            var actionName = window.location.pathname.split('/').pop();
            var params = Site.setLicenseeOptionParam();
            var url = Site.resolveClientUrl('api/RunningBettypesBetList/' + actionName);

            BetList.loadData(params, url);
        },

        showCombMPDetail: function (transId, refNo, betId, custId, betTeam, baseCurrency) {
            if (Number(custId) === 0) {
                return;
            }
            var url = Site.resolveClientUrl("_BetList/Popup/ViewSystemParlay.aspx");
            url = QueryString.setParam(url, "transid", transId);
            url = QueryString.setParam(url, "refno", refNo);
            url = QueryString.setParam(url, "betteam", betTeam);
            url = QueryString.setParam(url, "betid", betId);
            url = QueryString.setParam(url, "custid", custId);
            url = QueryString.setParam(url, "basecurrency", baseCurrency);

            Site.blockUI();
            $("#popupSystemParlay").load(url).dialog();
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
    RunningBetListMultipleBettype.initialize();
    RunningBetListMultipleBettype.exportToExcel();
});