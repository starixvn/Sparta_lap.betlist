/*global Site, QueryString, ProductListHelper, PagingHeper, jqGridAdditional, CustomerWinLossOption, Report*/
/// <reference path="/@JSense.js" />
var CustomerWinLoss = (function ($) {
    "use strict";

    var DATA_URL = "api/CustomerWinLossReport/Index";
    var FORM_SELECTOR = "#CustomerWinlossForm";
    var GRID_ID = "ReportGrid";

    var _ajaxRequest = null;
    var _submitData = null;

    var _sortColumnName = "";
    var _sortOrder = "desc";
    var _isFirstLoad = false;

    var _pagingOption =
    {
        pageindex: 1,
        pagesize: 1000,
        pagelist: [1000, 2000],
        totalrow: 1,
        elementcontainer: ".paging",
        method: "AJAX",
        ajaxfunction: "CustomerWinLoss.ajaxPaging()",
        alwayshow: true
    };

    function buildPostData() {
        if (!_submitData) {
            _submitData = $(FORM_SELECTOR).serializeObject();
        }
        _submitData.PageIndex = _pagingOption.pageindex;
        _submitData.PageSize = _pagingOption.pagesize;
        _submitData.sortColumn = _sortColumnName;
        _submitData.SortDirection = _sortOrder;
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
                CustomerWinLoss.submit();
            }, 100);
        }
    }

    function onGridComplete(grid, gridOption, response) {
        if (_pagingOption.totalrow <= 0) {
            jqGridAdditional.setNoInfo(gridOption.colModel);
        }
        else if (_isFirstLoad) {
            var reportGrid = $("#" + GRID_ID);
            if (_pagingOption.totalrow > 1) {
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
            if (gridOption.shrinkToFit) {
                jqGridAdditional.resetLayout(reportGrid);
            }
            jqGridAdditional.resizeGrid();
            _isFirstLoad = false;
        }
    }

    function getLicenseeWLNote() {
        var resultProducts = getProductList();
        if (resultProducts.length > 0) {
            return Site.getSingleResource("LicenseeW_L") + "(" + resultProducts.join(', ') + ")";
        }
        return "";
    }

    function getProductList() {
        var productCodeList = [];
        $(".childCheckbox", "#ProductList").each(function (index, element) {
            var productCode = $(element).attr('data-product-code');
            if (productCode && $(element).attr("data-original") === 'true') {
                productCodeList.push(productCode.toUpperCase());
            }
        });

        return productCodeList;
    }

    function resetParam() {
        _sortColumnName = "ExternalId";
        _sortOrder = "asc";
        _pagingOption.pageindex = 1;
        _submitData = null;
    }

    return {
        initialize: function () {
            ProductListHelper.initProductList();
            CustomerWinLoss.initEvent();
            CustomerWinLoss.initSetting();
            CustomerWinLoss.initMonthPicker();
            Site.addFilterCollapseExpand();
            Site.addUserGuide({
                resourceName: "CustomerWL",
                title: Site.getSingleResource("CustomerWinLoss")
            });
        },
        initMonthPicker: function () {
            if ($("#IsHistory").val() === "True") {
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
                var validProduct = Report.ValidProducts();
                if (!validProduct) {
                    return;
                }

                var fromday = $('#FromDay').val();
                var today = $('#ToDay').val();
                var year = $('#Year').val();
                var month = $('#Month').val();
                $('#FromDate').val(month + "/" + fromday + "/" + year);
                $('#ToDate').val(month + "/" + today + "/" + year);
                resetParam();
                CustomerWinLoss.submit();
            });

            $("#SubmitButton").click(function () {
                var validProduct = Report.ValidProducts();
                if (!validProduct) {
                    return;
                }

                resetParam();
                CustomerWinLoss.submit();
            });

            $('#ExportToExcel').click(function () {
                var tempSubmitData = Site.clone(_submitData);
                _submitData = null;
                buildPostData();
                _submitData.PageIndex = 1;
                Site.exportExcel(_submitData, Site.resolveClientUrl(DATA_URL));
                _submitData = tempSubmitData;
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

        initSetting: function () {
            CustomerWinLossOption.isAllowBA = Site.asBool($("#IsAllowedBA").val());
            CustomerWinLossOption.isAllowRB = Site.asBool($("#IsAllowedRB").val());
            CustomerWinLossOption.hasCustomerComm = Site.asBool($("#IsAllowedCustomerCommission").val());
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
            url = QueryString.setParam(url, "ProductCodeList", getProductList());
            url = QueryString.setParam(url, "currencyid", _submitData.CurrencyId);
            url = QueryString.setParam(url, "CurrencyName", _submitData.CurrencyName);
            url = QueryString.setParam(url, "BaseCurrency", _submitData.BaseCurrency);
            url = QueryString.setParam(url, "custid", custId);
            url = QueryString.setParam(url, "systemid", systemId);
            url = QueryString.setParam(url, "externalid", externalId);
            url = QueryString.setParam(url, "fromdate", _submitData.FromDate);
            url = QueryString.setParam(url, "todate", _submitData.ToDate);
            url = QueryString.setParam(url, "ishistoryreport", $('input#IsHistory').val());

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
            var titlePattern = "{0}, {1} → {2}, {3}{4}";
            var licenseeWLNote = getLicenseeWLNote(_submitData.ProductOptions);
            var title = $.format(titlePattern,
                Site.getSingleResource("CustomerWinLoss"),
                _submitData.FromDate,
                _submitData.ToDate,
                Site.getSingleResource("Viewin").replace("{0}",
                _submitData.BaseCurrency === "" ? _submitData.CurrencyName : _submitData.BaseCurrency.toUpperCase()),
                licenseeWLNote === "" ? "" : ", " + licenseeWLNote);

            $("div.report-title").html(title).show();
        },

        submit: function () {
            Report.setOriginalProduct();

            buildPostData();

            var option =
            {
                type: "POST",
                data: _submitData,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(DATA_URL),
                success: function (response) {
                    CustomerWinLoss.bindData(response);
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function () {
                    Site.unBlockUI();
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
            Site.blockUI();
            jqGridAdditional.clearGrid(GRID_ID);
            _isFirstLoad = true;

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
});
$(window).resize(function () {
    "use strict";
    jqGridAdditional.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});