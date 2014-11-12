/*global Site, QueryString*/
/// <reference path="/@JSense.js" />
var CustPendingFunds = (function ($) {
    "use strict";
    return {
        resizeGrid: function () {
            var $grid = $("#data-content");
            if ($grid.length === 0) {
                return;
            }
            var $pager = $(".pagingContainer");
            var pagerHeight = $pager.length > 0 ? $pager.height() * $pager.length : 0;
            var gridTop = $grid.offset().top;
            var gridHeight = window.innerHeight - gridTop - $("#report-total").height() - pagerHeight - 30;
            gridHeight = Math.max(120, gridHeight);
            $grid.css("max-height", gridHeight);
        },
        initialize: function () {
            CustPendingFunds.initEvent();
            Site.createSortingIcon(CustPendingFunds.sortReport);
            CustPendingFunds.resizeGrid();
            Site.addUserGuide({
                resourceName: "CustomerPendingFunds",
                title: Site.getSingleResource("CustomerPendingFunds"),
                height: 400
            });
        },
        initEvent: function () {
            $("#data-content").on("click", ".bet-list-link", function () {
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
            var url = Site.resolveClientUrl("BetList/PendingFunds");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = Site.setProductOptionOriginalParamList(url);
            url = QueryString.setParam(url, "custid", custid);
            url = QueryString.setParam(url, "systemid", systemId);
            url = QueryString.setParam(url, "externalid", externalId);
            window.location.href = url;
        },
        sortReport: function () {
            Site.blockUI();
            $('#IsExportExcel').val(false);
            $("#CustomerPendingFundsForm").submit();
            return false;
        }
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    CustPendingFunds.initialize();
});
$(window).resize(function () {
    "use strict";
    CustPendingFunds.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});