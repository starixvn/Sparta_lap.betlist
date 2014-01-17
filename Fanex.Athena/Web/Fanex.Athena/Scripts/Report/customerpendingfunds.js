/// <reference path="/@JSense.js" />
var CustPendingFunds = (function ($) {
    return {
        initialize: function () {
            CustPendingFunds.initEvent();
            Site.createSortingIcon(CustPendingFunds.sortReport);
        },
        initEvent: function () {
            $("#data-content").on("click", ".bet-list-link", function (e) {
                var target = $(this);
                var custId = target.attr("data-cust-id");
                var systemId = target.attr("data-system-id");
                var externalId = target.attr("data-external-id");
                CustPendingFunds.viewPendingFundsBetList(custId, systemId, externalId);
            });

            $("#ExportToExcel").click(function () {
                $('#IsExportExcel').val(true);
                $("#CustomerPendingFundsForm").submit();
                return false;
            });

            $("#SubmitButton").click(function () {
                Site.blockUI();
                $("#SortColumn").val("ExternalId");
                $("#SortDirection").val("ASC");
                $('#IsExportExcel').val(false);
                $("#CustomerPendingFundsForm").submit();
                return false;
            });
        },

        viewPendingFundsBetList: function (custid, systemId, externalId) {
            Site.blockUI();
            var url = resolveClientUrl("BetList/PendingFunds");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = Site.setProductOptionOriginalParamList(url);
            url = QueryString.setParam(url, "custid", custid);
            url = QueryString.setParam(url, "systemid", systemId);
            url = QueryString.setParam(url, "externalid", externalId);
            window.location.href = url;
        },
        sortReport: function (sortColumn) {
            Site.blockUI();
            $('#IsExportExcel').val(false);
            $("#CustomerPendingFundsForm").submit();
            return false;
        }
    };
})(jQuery);

$(window).load(function () {
    CustPendingFunds.initialize();
});