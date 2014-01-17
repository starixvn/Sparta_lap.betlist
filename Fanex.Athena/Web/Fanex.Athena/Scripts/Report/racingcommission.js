/// <reference path="/@JSense.js" />
var RacingCommission = (function ($) {
    return {
        initialize: function () {
            RacingCommission.initEvent();
            Site.createSortingIcon(RacingCommission.sortReport);
        },
        initEvent: function () {
            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                $('#ReportForm').submit();
            });
            $('#SubmitButton').click(function () {
                $("#SortColumn").val("ExternalID");
                $("#SortDirection").val("ASC");
                RacingCommission.submitForm();
            });

            $('#sortReport').click(function () {
                RacingCommission.sortReport();
            })

            $("#data-content").on("click", ".bet-list-link", function (e) {
                var target = $(this);
                var custId = target.attr("data-cust-id");
                var systemId = target.attr("data-system-id");
                var externalId = target.attr("data-external-id");
                RacingCommission.viewRacingBetList(custId, systemId, externalId);
            });
        },

        viewRacingBetList: function (custid, systemId, externalId) {
            var url = resolveClientUrl("BetList/CustomerWl");
            url = Site.setProductOptionParam(url, 0, "RB", true);
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "fromdate", $("#FromDate").attr("data-original"));
            url = QueryString.setParam(url, "todate", $("#ToDate").attr("data-original"));
            url = QueryString.setParam(url, "custid", custid);
            url = QueryString.setParam(url, "systemId", systemId);
            url = QueryString.setParam(url, "externalId", externalId);

            Site.blockUI();
            window.location.href = url;
        },
        submitForm: function () {
            Site.blockUI();
            $('#IsExportExcel').val(false);
            $('#ReportForm').submit();
        },
        sortReport: function (sortColumn) {
            Site.blockUI();
            $('#IsExportExcel').val(false);
            $('#ReportForm').submit();
        }
    }
})(jQuery);

$(window).load(function () {
    RacingCommission.initialize();
});