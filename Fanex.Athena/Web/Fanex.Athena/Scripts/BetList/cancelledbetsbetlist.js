/*global Site, QueryString, BetList*/
/// <reference path="/@JSense.js" />

var CancelledBetsBetList = (function ($) {
    "use strict";
    return {
        initialize: function () {
            CancelledBetsBetList.initEvent();
            CancelledBetsBetList.submit();
            Site.addUserGuide({
                resourceName: "CancelledBets",
                title: Site.getSingleResource("CancelledBets")
            });
        },

        initEvent: function () {
            $('#CustId').change(function () {
                CancelledBetsBetList.submit();
            });

            //Submit button
            $('#SubmitButton').click(function () {
                $('#IsExportExcel').val(false);
            });

            $("#ExportToExcel").click(function () {
                CancelledBetsBetList.exportToExcel();
            });
        },
        submit: function () {
            // Site.blockUI();
            BetList.initialize();

            // Load Data
            var params = { custId: $("#CustId").val() };
            params = Site.setLicenseeOptionParam(params);

            var url = Site.resolveClientUrl('api/CancelledBetsBetList/Index');
            BetList.loadData(params, url);
        },

        exportToExcel: function () {
            var url = window.location.href;
            url = Site.setLicenseeOptionParam(url);
            url = QueryString.setParam(url, "custId", $("#CustId").val());
            $('#IsExportExcel').val(true);
            $('form').submit();
        }
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    CancelledBetsBetList.initialize();
});