/*global Site, QueryString, BetList */
/// <reference path="/@JSense.js" />
var RunningBetList = (function ($) {
    "use strict";
    return {
        initialize: function () {
            RunningBetList.initEvent();
            RunningBetList.submit();
            Site.addUserGuide({
                resourceName: $('#userGuideName').val(),
                title: Site.getSingleResource("TotalBets") + " - " + $('#title').val()
            });
        },

        initEvent: function () {
            $('#SubmitButton').click(function () {
                RunningBetList.submit();
            });
        },

        submit: function (bettype) {
            BetList.initialize();
            var actionName = window.location.pathname.split('/').pop();
            var url = Site.resolveClientUrl('api/RunningBetList/' + actionName);
            var params = Site.setLicenseeOptionParam();
            
            BetList.loadData(params, url);
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