/*global Site, QueryString, ProductListHelper*/
/// <reference path="/@JSense.js" />
var CustomerWinLoss = (function ($) {
    "use strict";
    return {
        resizeGrid: function () {
            var $grid = $("#data-content");
            if ($grid.length === 0) {
                return;
            }

            var gridTop = $grid.offset().top;
            var pagerHeight = $(".pagingContainer").height() || 0;
            if (pagerHeight > 0) {
                pagerHeight += 10;
            }
            var gridHeight = window.innerHeight - gridTop - $("#total-content").height()
                             - pagerHeight - 35;
            gridHeight = Math.max(120, gridHeight);

            $grid.css("height", "auto").css("max-height", gridHeight);
            $(".report-content").css("max-height", "");
        },
        initialize: function () {
            ProductListHelper.initProductList();
            CustomerWinLoss.initEvent();
            CustomerWinLoss.initMonthPicker();
            Site.createSortingIcon(CustomerWinLoss.sortReport);
            Site.addFilterCollapseExpand();
            Site.addUserGuide({
                resourceName: "CustomerWL",
                title: Site.getSingleResource("CustomerWinLoss")
            });
        },
        initMonthPicker: function () {
            if ($("#IsHistoryReport").val() === "True") {
                $('#DateControl').monPickr({
                    minDate: Site.getHistoryDate(),
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
                    log: function (/*msg*/) {
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

                var pageSize = $('.pageSizeSelected').val();
                if (pageSize !== undefined) {
                    var pageSizeHtml = '<input type="hidden" class="page-size-param" value="' + pageSize + '" name="PageSize" />';
                    $("#CustomerWinlossForm").append(pageSizeHtml);
                }

                $("#CustomerWinlossForm").submit();
                $(".page-size-param").remove();
                return false;
            });

            $("#history-report-link").click(function () {
                CustomerWinLoss.viewHistoryReport();
            });
            $("#current-report-link").click(function () {
                CustomerWinLoss.viewCurrentReport();
            });

            $("#data-content").on("click", ".bet-list-link", function () {
                var target = $(this);
                var custId = target.attr("data-cust-id");
                var systemId = target.attr("data-system-id");
                var externalId = target.attr("data-external-id");
                CustomerWinLoss.viewWinlossBetList(custId, systemId, externalId);
            });

            $("#data-content").scroll(function () {
                $('#total-content').scrollLeft($('#data-content').scrollLeft());
                $('#header-content').scrollLeft($('#data-content').scrollLeft());
            });
        },

        viewHistoryReport: function () {
            Site.blockUI();
            window.location.href = Site.resolveClientUrl("CustomerWinlossReport/History");
        },

        viewCurrentReport: function () {
            Site.blockUI();
            window.location.href = Site.resolveClientUrl("CustomerWinLossReport/Index");
        },

        viewWinlossBetList: function (custId, systemId, externalId) {
            var url = Site.resolveClientUrl("CustomerWinlossBetList/Index");
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
        },
        sortReport: function () {
            Site.blockUI();
            $('#IsExportExcel').val(false);
            $("#CustomerWinlossForm").submit();
            return false;
        }
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    CustomerWinLoss.initialize();
    CustomerWinLoss.resizeGrid();
});
$(window).resize(function () {
    "use strict";
    CustomerWinLoss.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});