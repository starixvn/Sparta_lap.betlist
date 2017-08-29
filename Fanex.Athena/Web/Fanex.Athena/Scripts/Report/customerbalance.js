/*global Site, ProductListHelper, CustomerBalanceOption, PagingHeper, jqGridAdditional, Report*/
/// <reference path="/@JSense.js" />
var CustomerBalance = (function ($) {
    "use strict";

    var VALID_MONTH_RANGE = 2;
    var FORM_SELECTOR = "#CustomerBalanceForm";
    var GRID_ID = "ReportGrid";
    var EXCEL_NAME_SELECTOR = "#IsExportExcel";
    var _ajaxRequest = null;
    var _sortColumnIndex = "";
    var _sortType = "desc";
    var _orderColumnCode = 0;
    var _isFirstLoad = true;
    var _submitData = null;
    var _pagingOption =
    {
        pageindex: 1,
        pagesize: 1000,
        pagelist: [1000, 2000],
        totalrow: 1,
        elementcontainer: ".paging",
        method: "AJAX",
        ajaxfunction: "CustomerBalance.ajaxPaging()",
        alwayshow: true
    };

    function buildPostData() {
        if (!_submitData) {
            _submitData = $(FORM_SELECTOR).serializeObject();
        }
        _submitData.PageIndex = _pagingOption.pageindex;
        _submitData.PageSize = _pagingOption.pagesize;
        _submitData.OrderItem = _orderColumnCode;
    }

    function createPaging(totalRows) {
        _pagingOption.totalrow = totalRows;
        if (_pagingOption.totalrow > _pagingOption.pagelist[0]) {
            $(_pagingOption.elementcontainer).show();
            PagingHeper.render(_pagingOption);
        }
        else {
            $(_pagingOption.elementcontainer).hide();
        }
    }

    function sort(index) {
        if (_pagingOption.totalrow > 1) {
            _sortType = index === _sortColumnIndex ? (_sortType === "desc" ? "asc" : "desc") : "desc";
            window.setTimeout(function () {
                _orderColumnCode = CustomerBalanceOption.getOrderColumnCode(index, _sortType);
                if (_orderColumnCode === 0) {
                    _sortColumnIndex = "";
                    _sortType = "";
                }
                else {
                    _sortColumnIndex = index;
                }
                $("#OrderItem").val(_orderColumnCode);
                CustomerBalance.submit();
            }, 100);
        }
    }

    function onGridComplete(grid, gridOption, response) {
        if (response.TotalRows <= 0) {
            jqGridAdditional.setNoInfo(gridOption.colModel);
        }
        else if (_isFirstLoad) {
            if (response.TotalRows > 1) {
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
            jqGridAdditional.setGroupFooters(CustomerBalanceOption.totalColspan);
            jqGridAdditional.resizeGrid();
            _isFirstLoad = false;
        }
    }

    return {
        initialize: function () {
            ProductListHelper.initProductList();
            CustomerBalance.initEvent();
            CustomerBalance.initSetting();
            Site.setConstraintMonthRange(VALID_MONTH_RANGE, CustomerBalanceOption.urlExcelData);
            Site.addUserGuide({
                resourceName: "CustomerBalance",
                title: Site.getSingleResource("CustomerBalance")
            });
            Site.addFilterCollapseExpand();
        },

        initEvent: function () {
            $("#SubmitButton").click(function () {
                var validProduct = Report.ValidProducts();
                if (!validProduct) {
                    return;
                }

                _pagingOption.pageindex = 1;
                _submitData = null;
                CustomerBalance.submit();
            });

            $("#ExportToExcel").click(function () {
                var validProduct = Report.ValidProducts();
                if (!validProduct) {
                    return false;
                }

                $(EXCEL_NAME_SELECTOR).val(true);
                var mainForm = $(FORM_SELECTOR);
                var url = Site.resolveClientUrl(CustomerBalanceOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                $(EXCEL_NAME_SELECTOR).val(false);
                return false;
            });
        },

        initSetting: function(){
            CustomerBalanceOption.hasCustomerComm = Site.asBool($("#IsAllowedCustomerCommission").val());
            CustomerBalanceOption.isAllowBA = Site.asBool($("#IsAllowedBACommission").val());
        },

        setReportTitle: function (fromDate, toDate, currencyName) {
            var titlePattern = "{0}, {1} → {2}, {3}";
            var title = $.format(titlePattern,
                Site.getSingleResource("CustomerBalance"),
                fromDate,
                toDate,
                Site.getSingleResource("Viewin").replace("{0}", currencyName));

            $("div.report-title").html(title).show();
        },

        submit: function () {
            buildPostData();

            var option =
            {
                type: "POST",
                data: _submitData,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(CustomerBalanceOption.urlData),
                success: function (response) {
                    CustomerBalance.setReportTitle(_submitData.FromDate, _submitData.ToDate, _submitData.CurrencyName);
                    CustomerBalance.bindData(response);
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
            CustomerBalance.submit();
        },

        bindData: function (response) {
            Site.blockUI();
            jqGridAdditional.clearGrid(GRID_ID);
            _isFirstLoad = true;

            var gridOption = CustomerBalanceOption.getGridOption(response.TotalRows);
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

            createPaging(response.TotalRows);

            var reportGrid = $("#" + GRID_ID);
            reportGrid.jqGrid(gridOption);

            CustomerBalanceOption.setGroupHeaderColumns(reportGrid);
        }
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    CustomerBalance.initialize();
});

$(window).resize(function () {
    "use strict";
    jqGridAdditional.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});