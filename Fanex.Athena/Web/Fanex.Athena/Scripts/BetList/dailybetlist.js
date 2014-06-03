/*global Site, QueryString, BetList*/
/// <reference path="/@JSense.js" />

window.serverPaging = true; // used in betlist.js

var DailyBetList = (function ($) {
    "use strict";
    return {
        submitUrl: null,
        initialize: function () {
            DailyBetList.initEvent();
        },

        initEvent: function () {
            //Submit button
            $('#SubmitButton').click(function () {
                DailyBetList.submit();
            });

            $("#ExportToExcel").click(function () {
                DailyBetList.exportToExcel();
            });

            $("#CurrencyId").change(function () {
                var currency = parseInt($("#CurrencyId").val(), 10);
                var currencyArea = $(".export-excel-area");
                if (currency !== 0) {
                    currencyArea.show();
                } else {
                    currencyArea.hide();
                }
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
            var url = window.location.href;
            var page = $('#BetListGrid').getGridParam('page');
            var size = $('#BetListGrid').getGridParam('rowNum');
            url = Site.setLicenseeOptionParam(url);
            url = QueryString.setParam(url, "IsExportExcel", true);
            url = QueryString.setParam(url, "date", $("#Date").val());
            url = QueryString.setParam(url, "productCode", $("#ProductCode").val());
            url = QueryString.setParam(url, "DailyBetListStatus", $("#DailyBetListStatus").val());
            url = QueryString.setParam(url, "PageIndex", page);
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