/// <reference path="/@JSense.js" />

var DailyBetList = (function ($) {
    return {
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
        },

        submit: function () {
            // Site.blockUI();
            BetList.initialize();
            var url = Site.resolveClientUrl('api/betlist/DailyBetList');
            url = Site.setLicenseeOptionParam(url);
            url = QueryString.setParam(url, "date", $("#Date").val());

            BetList.loadDataPaging(url);
        },

        exportToExcel: function () {
            var url = window.location.href;
            var page = $('#BetListGrid').getGridParam('page');
            var size = $('#BetListGrid').getGridParam('rowNum');
            url = Site.setLicenseeOptionParam(url);
            url = QueryString.setParam(url, "IsExportExcel", true);
            url = QueryString.setParam(url, "date", $("#Date").val());
            url = QueryString.setParam(url, "date", $("#Date").val());
            url = QueryString.setParam(url, "date", $("#Date").val());
            url = QueryString.setParam(url, "PageIndex", page);
            url = QueryString.setParam(url, "PageSize", size);
            window.location.href = url;
        }
    };
})(jQuery);

$(window).load(function () {
    DailyBetList.initialize();
});