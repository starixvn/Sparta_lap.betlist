/// <reference path="/@JSense.js" />
var CustomerWinLoss = (function ($) {
    return {
        initialize: function () {
            CustomerWinLoss.initEvent();
            CustomerWinLoss.initMonthPicker();
            Site.createSortingIcon(CustomerWinLoss.sortReport);
        },
        initMonthPicker: function () {
            if ($("#IsHistoryReport").val() == "True") {
                $('#DateControl').monPickr({
                    minDate: '2010/01/01',
                    showDateRange: true,
                    fromDayId: 'MonthPickerFromDay',
                    toDayId: 'MonthPickerToDay',
                    month: new Date($('#FromDate').val()),
                    fromDay: new Date($('#FromDate').val()).getDate(),
                    toDay: new Date($('#ToDate').val()).getDate(),
                    afterChangeMonth: function () {
                        $('#Year').val(new Date($("#DateControl").monPickr("getFromDate", "MM/dd/yyyy")).getFullYear());
                        $('#Month').val(new Date($("#DateControl").monPickr("getFromDate", "MM/dd/yyyy")).getMonth() + 1);
                    },
                    afterChangeDay: function () {
                        $('#FromDay').val(new Date($("#DateControl").monPickr("getFromDate", "MM/dd/yyyy")).getDate());
                        $('#ToDay').val(new Date($("#DateControl").monPickr("getToDate", "MM/dd/yyyy")).getDate());
                    },
                    log: function (msg) {
                        //console.warn(msg);
                    }
                });
            }
        },
        initEvent: function () {
            $("#SubmitHistoryButton").click(function () {
                Site.blockUI();
                $('#IsExportExcel').val(false);
                var fromday = $('#FromDay').val();
                var today = $('#ToDay').val();
                var year = $('#Year').val();
                var month = $('#Month').val();
                $('#FromDate').val(month - 1 + "/" + fromday + "/" + year);
                $('#ToDate').val(month - 1 + "/" + today + "/" + year);
                $("#SortColumn").val("ExternalId");
                $("#SortDirection").val("ASC");
                $("#CustomerWinlossForm").submit();
                return false;
            });

            $("#SubmitButton").click(function () {
                Site.blockUI();
                $('#IsExportExcel').val(false);
                $("#SortColumn").val("ExternalId");
                $("#SortDirection").val("ASC");
                $("#CustomerWinlossForm").submit();
                return false;
            });

            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                $("#CustomerWinlossForm").submit();
                return false;
            });

            $("#history-report-link").click(function () {
                CustomerWinLoss.viewHistoryReport();
            });
            $("#current-report-link").click(function () {
                CustomerWinLoss.viewCurrentReport();
            });

            $("#data-content").on("click", ".bet-list-link", function (e) {
                var target = $(this);
                var custId = target.attr("data-cust-id");
                var systemId = target.attr("data-system-id");
                var externalId = target.attr("data-external-id");
                CustomerWinLoss.viewWinlossBetList(custId, systemId, externalId);
            });
        },

        viewHistoryReport: function () {
            Site.blockUI();
            window.location.href = resolveClientUrl("Report/CustomerWinlossHistory");
        },

        viewCurrentReport: function () {
            Site.blockUI();
            window.location.href = resolveClientUrl("Report/CustomerWinLoss");
        },

        viewWinlossBetList: function (custId, systemId, externalId) {
            var url = resolveClientUrl("BetList/CustomerWl");
            url = Site.setProductOptionOriginalParamList(url);
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "custid", custId);
            url = QueryString.setParam(url, "systemid", systemId);
            url = QueryString.setParam(url, "externalid", externalId);
            url = QueryString.setParam(url, "fromdate", $("#FromDate").attr("data-original"));
            url = QueryString.setParam(url, "todate", $("#ToDate").attr("data-original"));
            url = QueryString.setParam(url, "ishistoryreport", $('input#IsHistoryReport').val());

            Site.blockUI();
            window.location.href = url;
        }
    };
})(jQuery);

$(window).load(function () {
    CustomerWinLoss.initialize();
});