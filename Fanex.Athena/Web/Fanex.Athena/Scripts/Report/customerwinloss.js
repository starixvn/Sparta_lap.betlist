/*global Site, QueryString, ProductListHelper*/
/// <reference path="/@JSense.js" />
var CustomerWinLoss = (function ($) {
    "use strict";

    var DATA_URL = "api/CustomerWinLossReport/Index";
    var FORM_SELECTOR = "#MainForm";
    var GRID_ID = "ReportGrid";

    var _ajaxRequest = null;
    var _submitData = null;

    var _sortColumnName = "";
    var _sortOrder = "desc";
    
    var _pagingOption =
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
        data.PageIndex = _pagingOption.pageindex;
        data.PageSize = _pagingOption.pagesize;
        data.sortColumn = _sortColumnName;
        data.SortDirection = _sortOrder;

        return data;
    }

    function createPaging(totalRows) {
        _pagingOption.totalrow = totalRows;
        if (_pagingOption.totalrow > 0 && _pagingOption.totalrow > _pagingOption.pagelist[0]) {
            $(_pagingOption.elementcontainer).show();
            PagingHeper.render(_pagingOption);
        }
        else {
            $(_pagingOption.elementcontainer).hide();
        }
    }

    function sort(index) {
        if (_pagingOption.totalrow > 1) {
            setTimeout(function () {
                if (_sortColumnName === index) {
                    _sortOrder = _sortOrder === "asc" ? "desc" : "asc";
                } else {
                    _sortOrder = "asc";
                    _sortColumnName = index;
                }
                CustomerWinLoss.submit(_submitData);
            }, 100);
        }
    }

    function onGridComplete(grid, gridOption, response) {
        if (_pagingOption.totalrow <= 0) {
            jqGridAdditional.setNoInfo(gridOption.colModel);
        }
        else {
            if (_pagingOption.totalrow > 1) {
                var reportGrid = $("#" + GRID_ID);
                var cm = reportGrid[0].p.colModel;
                $.each(reportGrid[0].grid.headers, function (index, value) {
                    
                    var cmi = cm[index];
                    
                    if (cmi.sortable) {
                        $(".ui-jqgrid-sortable", value.el).css({ cursor: "pointer" }).find(".s-ico").remove();
                    }
                    if (cmi.index === _sortColumnName && _sortOrder !== "") {
                        
                        var header = $(".ui-jqgrid-sortable", value.el);
                        header.addClass("sorted-heading");
                        if (_sortOrder === "desc") {
                            header.append('<span class="sort-desc" style="display: inline; margin: 2px 0px 0px 0px"></span>');
                        }
                        else {
                            header.append('<span class="sort-asc" style="display: inline; margin: 2px 0px 0px 0px"></span>');
                        }
                    }
                });
            }

            jqGridAdditional.assignTotal(response.Total, gridOption.colModel);
            jqGridAdditional.setGroupFooters(CustomerWinLossOption.totalColspan);
            jqGridAdditional.resizeGrid();
        }
    }

    return {
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
                _pagingOption.pageindex = 1;
                $('#IsExportExcel').val(false);
                $("#SortColumn").val("ExternalId");
                $("#SortDirection").val("ASC");
                CustomerWinLoss.submit();
                return false;
            });

            $('#ExportToExcel').click(function () {
                var data = getPostData();
                data.pageSize = $('.pageSizeSelected').val();
                Site.exportExcel(data, DATA_URL);
                return false;
            });

            $("#history-report-link").click(function () {
                CustomerWinLoss.viewHistoryReport();
            });
            $("#current-report-link").click(function () {
                CustomerWinLoss.viewCurrentReport();
            });

            $(".report-content").on("click", ".bet-list-link", function () {
                var target = $(this);
                var custId = target.attr("data-cust-id");
                var systemId = target.attr("data-system-id");
                var externalId = target.attr("data-external-id");
                CustomerWinLoss.viewWinlossBetList(custId, systemId, externalId);
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
            url = QueryString.setParam(url, "currencyid", _submitData.CurrencyId);
            url = QueryString.setParam(url, "CurrencyName", _submitData.CurrencyName);
            url = QueryString.setParam(url, "BaseCurrency", _submitData.BaseCurrency);
            url = QueryString.setParam(url, "custid", custId);
            url = QueryString.setParam(url, "systemid", systemId);
            url = QueryString.setParam(url, "externalid", externalId);
            url = QueryString.setParam(url, "fromdate", _submitData.FromDate);
            url = QueryString.setParam(url, "todate", _submitData.ToDate);
            url = QueryString.setParam(url, "ishistoryreport", $('input#IsHistoryReport').val());

            var popH = Math.min($(window).height() - 100, 650), popW = 920;
            var dialogForm = '<div title="Customer Bet List"><iframe scrolling="no" frameborder="0" width="' +
                             popW + '" height="100%" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: popW + 50,
                height: popH + 50
            });
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
        submit: function (data) {
            if (!data) {
                data = getPostData();
                _submitData = data;
            }
            
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
            _pagingOption = PagingHeper.getSetting();
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
    jqGridAdditional.resizeGrid();
});
$(window).resize(function () {
    "use strict";
    jqGridAdditional.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});