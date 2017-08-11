/*global Site, QueryString, ProductListHelper*/
/// <reference path="/@JSense.js" />
var CustomerWinLoss = (function ($) {
    "use strict";

    var DATA_URL = "api/CustomerWinLossReport/Index";
    var FORM_SELECTOR = "#MainForm";
    var GRID_ID = "ReportGrid";

    var _ajaxRequest = null;

    var _sortColumnIndex = "";
    var _sortType = "desc";
    var isFirstLoad = true;

    var pagingOption =
    {
        pageindex: 1,
        pagesize: 5,
        pagelist: [5, 10],
        totalrow: 1,
        elementcontainer: ".paging",
        method: "AJAX",
        ajaxfunction: "CustomerWinLoss.ajaxPaging()",
        alwayshow: true
    };

    function getPostData() {
        var data = $(FORM_SELECTOR).serializeObject();
        data.PageIndex = pagingOption.pageindex;
        data.PageSize = pagingOption.pagesize;

        return data;
    }

    function createPaging(totalRows) {
        pagingOption.totalrow = totalRows;
        if (pagingOption.totalrow > 0 && pagingOption.totalrow > pagingOption.pagelist[0]) {
            $(pagingOption.elementcontainer).show();
            PagingHeper.render(pagingOption);
        }
        else {
            $(pagingOption.elementcontainer).hide();
        }
    }

    function onGridComplete(grid, gridOption, response) {
        if (pagingOption.totalrow <= 0) {
            jqGridAdditional.setNoInfo(gridOption.colModel);
        }
        else if (true) {
            if (pagingOption.totalrow > 1) {
                var reportGrid = $("#" + GRID_ID);
                var cm = reportGrid[0].p.colModel;
                $.each(reportGrid[0].grid.headers, function (index, value) {
                    var cmi = cm[index];
                    if (cmi.sortable) {
                        $(".ui-jqgrid-sortable", value.el).css({ cursor: "pointer" }).find(".s-ico").remove();
                    }
                    if (cmi.index === _sortColumnIndex && _sortType !== "") {
                        var header = $(".ui-jqgrid-sortable", value.el);
                        header.addClass("sorted-heading");
                        if (_sortType === "desc") {
                            header.append('<span class="sort-desc"></span>');
                        }
                        else {
                            header.append('<span class="sort-asc"></span>');
                        }
                    }
                });
            }

            jqGridAdditional.assignTotal(response.Total, gridOption.colModel);
            jqGridAdditional.setGroupFooters(CustomerWinLossOption.totalColspan);
            CustomerWinLoss.resizeGrid();
        }
    }

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
            Site.addFilterCollapseExpand();
            Site.addUserGuide({
                resourceName: "CustomerWL",
                title: Site.getSingleResource("CustomerWinLoss")
            });
            CustomerWinLossOption.hasCustomerComm = $("#IsAllowedCustomerCommission").val() === "1";
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
                CustomerWinLoss.submit();
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
        setReportTitle: function () {
            var titlePattern = "{0}, {1} → {2}, {3}";
            var title = $.format(titlePattern,
                Site.getSingleResource("CustomerWinLoss"),
                $("#FromDate").val().toString("M/dd/yyyy"),
                $("#ToDate").val().toString("M/dd/yyyy"),
                Site.getSingleResource("Viewin").replace("{0}",
                $("#CurrencyName").val()));

            $("div.report-title").html(title).show();
        },
        submit: function () {
            var data = getPostData();

            var option =
            {
                type: "POST",
                data: data,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(DATA_URL),
                success: function (response) {
                    CustomerWinLoss.bindData(response);
                },
                beforeSend: function () {
                    Site.blockUI();
                }
            };

            if (_ajaxRequest != null) {
                _ajaxRequest.abort();
            }

            _ajaxRequest = $.ajax(option);
            return true;
        },
        ajaxPaging: function () {
            pagingOption = PagingHeper.getSetting();
            CustomerWinLoss.submit();
        },

        bindData: function (response) {
            jqGridAdditional.clearGrid(GRID_ID);
            
            CustomerWinLossOption.isAllowBA = response.IsAllowBA;
            var gridOption = CustomerWinLossOption.getGridOption(response.TotalRows);
            gridOption.data = response.Data;
            gridOption.onSortCol = function (index) {
                sort(index);
                return "stop";
            };
            gridOption.gridComplete = function () {
                onGridComplete(this, gridOption, response);
                Site.unBlockUI();
            };
            gridOption.shrinkToFit = response.TotalRows <= 0;

            CustomerWinLoss.setReportTitle();
            createPaging(response.TotalRows);

            var reportGrid = $("#" + GRID_ID);
            reportGrid.jqGrid(gridOption);

            CustomerWinLossOption.setGroupHeaderColumns(reportGrid);
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