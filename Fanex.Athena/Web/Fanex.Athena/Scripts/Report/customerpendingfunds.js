/*global Site, QueryString, ProductListHelper, PagingHeper, jqGridAdditional, CustPendingFundsOption*/
/// <reference path="/@JSense.js" />

var CustPendingFunds = (function ($) {
    "use strict";

    var FORM_SELECTOR = "#CustomerPendingFundsForm";
    var EXCEL_NAME_SELECTOR = "#IsExportExcel";
    var GRID_ID = "ReportGrid";
    var _ajaxRequest;
    var _isFirstLoad = false;
    var _sortDirection = "";
    var _sortColumn = "";
    var _pagingOption =
    {
        pageindex: 1,
        pagesize: 1000,
        pagelist: [1000, 2000],
        totalrow: 1,
        elementcontainer: ".paging",
        method: "AJAX",
        ajaxfunction: "CustPendingFunds.ajaxPaging()",
        alwayshow: true
    };

    function getPostData() {
        var data = $(FORM_SELECTOR).serializeObject();
        data.PageIndex = _pagingOption.pageindex;
        data.PageSize = _pagingOption.pagesize;

        return data;
    }

    function sort(index) {
        if (_pagingOption.totalrow > 1) {
            _sortDirection = index === _sortColumn ? (_sortDirection === "asc" ? "desc" : "asc") : "asc";
            _sortColumn = index;
            window.setTimeout(function () {
                $("#SortColumn").val(_sortColumn);
                $("#SortDirection").val(_sortDirection);
                CustPendingFunds.submit();
            }, 100);
        }
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

    function onGridComplete(grid, gridOption, response) {
        if (response.TotalRows <= 0) {
            jqGridAdditional.setNoInfo(gridOption.colModel);
        }
        else if (_isFirstLoad) {
            var reportGrid = $("#" + GRID_ID);

            if (response.TotalRows > 1) {                
                var cm = reportGrid[0].p.colModel;
                $.each(reportGrid[0].grid.headers, function (index, value) {
                    var cmi = cm[index];
                    if (cmi.sortable) {
                        $(".ui-jqgrid-sortable", value.el).css({ cursor: "pointer" }).find(".s-ico").remove();
                    }
                    if (cmi.index === _sortColumn && _sortDirection !== "") {
                        var header = $(".ui-jqgrid-sortable", value.el);
                        header.addClass("sorted-heading");
                        if (_sortDirection === "desc") {
                            header.append('<span class="sort-desc"></span>');
                        }
                        else {
                            header.append('<span class="sort-asc"></span>');
                        }
                    }
                });
            }

            jqGridAdditional.assignTotal(response.Total, gridOption.colModel);
            jqGridAdditional.setGroupFooters(CustPendingFundsOption.totalColspan);
            jqGridAdditional.resizeGrid();
            jqGridAdditional.resetLayout(reportGrid);
            _isFirstLoad = false;
        }
    }

    return {
        initialize: function () {
            ProductListHelper.initProductList();
            CustPendingFunds.initEvent();
            CustPendingFunds.initSetting();
            Site.addUserGuide({
                resourceName: "CustomerPendingFunds",
                title: Site.getSingleResource("CustomerPedingFunds")
            });
        },
        initEvent: function () {
            $(".content-main").on("click", ".bet-list-link", function () {
                var data = $(this).data("drill-down");
                CustPendingFunds.viewPendingFundsBetList(data.CustId, data.SystemId, data.ExternalId);
            });

            $("#ExportToExcel").click(function () {
                $(EXCEL_NAME_SELECTOR).val(true);
                var mainForm = $(FORM_SELECTOR);
                var url = Site.resolveClientUrl(CustPendingFundsOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                $(EXCEL_NAME_SELECTOR).val(false);
                return false;
            });

            $("#SubmitButton").click(function () {
                CustPendingFunds.submit();
                return false;
            });
        },

        initSetting: function () {
            _sortColumn = $("#SortColumn").val();
            _sortDirection = $("#SortDirection").val();
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
                url: Site.resolveClientUrl(CustPendingFundsOption.urlData),
                success: function (response) {
                    CustPendingFunds.bindData(response);
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

        bindData: function (response) {
            Site.blockUI();
            jqGridAdditional.clearGrid(GRID_ID);
            _isFirstLoad = true;

            var gridOption = CustPendingFundsOption.getGridOption(response.TotalRows);
            gridOption.data = response.Data;
            gridOption.onSortCol = function (index) {
                sort(index);
                return "stop";
            };
            gridOption.gridComplete = function () {
                onGridComplete(this, gridOption, response);
                Site.unBlockUI();
            };

            createPaging(response.TotalRows);

            var reportGrid = $("#" + GRID_ID);
            reportGrid.jqGrid(gridOption);
        },

        viewPendingFundsBetList: function (custid, systemId, externalId) {
            var url = Site.resolveClientUrl("PendingFundsBetList/Index");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = Site.setProductOptionOriginalParamList(url);
            url = QueryString.setParam(url, "custid", custid);
            url = QueryString.setParam(url, "systemid", systemId);
            url = QueryString.setParam(url, "externalid", externalId);
            
            var popH = Math.min($(window).height() - 100, 650), popW = 920;
            var dialogForm = '<div title="Customer Pending Funds Bet List"><iframe scrolling="no" frameborder="0" width="' +
                             popW + '" height="100%" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: popW + 50,
                height: popH + 50
            });
        },
        ajaxPaging: function () {
            _pagingOption = PagingHeper.getSetting();
            CustPendingFunds.submit();
        },
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    CustPendingFunds.initialize();
});
$(window).resize(function () {
    "use strict";
    jqGridAdditional.resizeGrid();
    jqGridAdditional.resetLayout($("#ReportGrid"));
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});