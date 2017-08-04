/*global Site, ProductListHelper, CustomerBalanceOption, PagingHeper, jqGridAdditional */
/// <reference path="/@JSense.js" />
var CustomerBalance = (function ($) {
    "use strict";

    var VALID_MONTH_RANGE = 2;
    var FORM_SELECTOR = "#CustomerBalanceForm";
    var GRID_ID = "ReportGrid";
    var EXCEL_NAME_SELECTOR = "#IsExportExcel";
    var ajaxRequest = null;
    var sortColumnIndex = "";
    var sortType = "desc";
    var isFirstLoad = true;
    var pagingOption =
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

    function sort(index) {
        if (pagingOption.totalrow > 0) {
            Site.blockUI();
            sortType = index === sortColumnIndex ? (sortType === "desc" ? "asc" : "desc") : "desc";
            window.setTimeout(function () {
                var orderColumnCode = CustomerBalanceOption.getOrderColumnCode(index, sortType);
                if (orderColumnCode === 0) {
                    sortColumnIndex = "";
                    sortType = "";
                }
                else {
                    sortColumnIndex = index;
                }
                $("#OrderItem").val(orderColumnCode);
                CustomerBalance.submit();
                Site.unBlockUI();
            }, 100);
        }
    }

    function onGridComplete(grid, gridOption, response) {
        if (pagingOption.totalrow <= 0) {
            jqGridAdditional.setNoInfo(gridOption.colModel);
        }
        else if (isFirstLoad) {
            if (pagingOption.totalrow > 1) {
                var reportGrid = $("#" + GRID_ID);
                var cm = reportGrid[0].p.colModel;
                $.each(reportGrid[0].grid.headers, function (index, value) {
                    var cmi = cm[index];
                    if (cmi.sortable) {
                        $(".ui-jqgrid-sortable", value.el).css({ cursor: "pointer" }).find(".s-ico").remove();
                    }
                    if (cmi.index === sortColumnIndex && sortType !== "") {
                        var header = $(".ui-jqgrid-sortable", value.el);
                        header.addClass("sorted-heading");
                        if (sortType === "desc") {
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
            CustomerBalance.resizeGrid();
            isFirstLoad = false;
        }
    }

    return {
        resizeGrid: function () {
            jqGridAdditional.resizeGrid();
        },

        initialize: function () {
            ProductListHelper.initProductList();
            CustomerBalance.initEvent();
            Site.setConstraintMonthRange(VALID_MONTH_RANGE, CustomerBalanceOption.urlExcelData);
            Site.addUserGuide({
                resourceName: "CustomerBalance",
                title: Site.getSingleResource("CustomerBalance")
            });
            Site.addFilterCollapseExpand();
        },

        initEvent: function () {
            $("#SubmitButton").click(function () {
                pagingOption.pageindex = 1;
                CustomerBalance.submit();
            });

            $("#ExportToExcel").click(function () {
                $(EXCEL_NAME_SELECTOR).val(true);
                var mainForm = $(FORM_SELECTOR);
                var url = Site.resolveClientUrl(CustomerBalanceOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                $(EXCEL_NAME_SELECTOR).val(false);
                return false;
            });
        },

        setReportTitle: function () {
            var titlePattern = "{0}, {1} → {2}, {3}";
            var title = $.format(titlePattern,
                Site.getSingleResource("CustomerBalance"),
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
                url: Site.resolveClientUrl(CustomerBalanceOption.urlData),
                success: function (response) {
                    CustomerBalance.bindData(response);
                },
                beforeSend: function () {
                    Site.blockUI();
                }
            };

            if (ajaxRequest != null) {
                ajaxRequest.abort();
            }

            ajaxRequest = $.ajax(option);
            return true;
        },

        ajaxPaging: function () {
            pagingOption = PagingHeper.getSetting();
            CustomerBalance.submit();
        },

        bindData: function (response) {
            jqGridAdditional.clearGrid(GRID_ID);
            isFirstLoad = true;

            CustomerBalanceOption.hasCustomerComm = response.HasCustomerComm;
            CustomerBalanceOption.isAllowBA = response.IsAllowBA;                       
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

            CustomerBalance.setReportTitle();
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
    CustomerBalance.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});