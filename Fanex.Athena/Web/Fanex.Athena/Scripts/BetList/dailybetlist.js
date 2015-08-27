/*global Site, QueryString, BetList*/
/// <reference path="/@JSense.js" />

window.serverPaging = true; // used in betlist.js

var DailyBetList = (function ($) {
    "use strict";
    var KEENOCEAN_LICENSEEID = 96;
    
    var showOrHideExcelIcon = function () {
        var $exportExcel = $(".export-excel-area");
        if (Site.getLicenseeId() === KEENOCEAN_LICENSEEID) {
            $exportExcel.show();
            return;
        }

        var currency = parseInt($("#CurrencyId").val(), 10);
        $exportExcel.toggle(currency !== 0);
    };
    return {
        submitUrl: null,
        initialize: function () {

            DailyBetList.initEvent();
            showOrHideExcelIcon();
            Site.addUserGuide({
                resourceName: "DailyBetList",
                title: Site.getSingleResource("DailyBetList")
            });
        },

        initEvent: function () {
            //Submit button
            $('#SubmitButton').click(function () {
                DailyBetList.submit();
            });

            $("#ExportToExcel").on("click", function () {
                DailyBetList.exportToExcel();
            });

            $("#CurrencyId").change(function () {
                showOrHideExcelIcon();
            });
        },

        submit: function () {
            // Site.blockUI();
            BetList.initialize();
            var url = Site.resolveClientUrl('api/betlist/DailyBetList');
            url = Site.setLicenseeOptionParam(url);
            url = QueryString.setParam(url, "productCode", $("#ProductCode").val());
            url = QueryString.setParam(url, "status", $("#DailyBetListStatus").val());
            url = QueryString.setParam(url, "date", $("#Date").val());
            BetList.submitUrl = url;
            BetList.loadDataPaging(url);
        },

        exportToExcel: function () {
            /*export all data in one page*/
            var url = window.location.href;
            var size = $('#BetListGrid').getGridParam('rowNum');
            url = Site.setLicenseeOptionParam(url);
            url = QueryString.setParam(url, "IsExportExcel", true);
            url = QueryString.setParam(url, "date", $("#Date").val());
            url = QueryString.setParam(url, "productCode", $("#ProductCode").val());
            url = QueryString.setParam(url, "DailyBetListStatus", $("#DailyBetListStatus").val());
            url = QueryString.setParam(url, "PageIndex", 1);
            url = QueryString.setParam(url, "PageSize", size);
            var sortParams = BetList.getSortParams();
            if (sortParams) {
                url = QueryString.setParam(url, "SortBy", sortParams.SortBy);
                url = QueryString.setParam(url, "SortOrder", sortParams.SortOrder);
            }
            window.location.href = url;
        }
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    DailyBetList.initialize();
});