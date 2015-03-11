/*global jqGridAdditional, JSLINQ, ActiveCustomerOption, Report, Site */
/// <reference path="/@JSense.js" />
///
var ActiveCustomer = (function ($) {
    "use strict";

    var MIN_GRID_WIDTH = 500;
    var VALID_MONTH_RANGE = 3;

    var cacheData = {};
    var ajaxRequest = null;
    var _sortColumnName = null;
    var _sortOrder = "desc";
    var _removeSorting = false;
    var _resizeTimer = null;

    function sortData(arrayData) {
        var data = _sortOrder === "asc" ?
             JSLINQ(arrayData).OrderBy(function (x) { return x[_sortColumnName]; }).items
             : JSLINQ(arrayData).OrderByDescending(function (x) { return x[_sortColumnName]; }).items;
        for (var i = 0; i < data.length; i++) {
            data[i].Index = i + 1;
        }

        return data;
    }

    return {
        resizeGrid: function () {
            if ($(".ui-jqgrid-bdiv").length === 0) {
                return;
            }
            clearTimeout(_resizeTimer);
            _resizeTimer = setTimeout(function () {
                var $bdiv = $(".ui-jqgrid-bdiv");

                var gridTop = $bdiv.offset().top;
                var gridHeight = window.innerHeight - gridTop - $(".ui-jqgrid-sdiv:visible").height()
                            - $(".ui-jqgrid-pager:visible").height() - 30;
                gridHeight = Math.max(120, gridHeight);

                var gridContainer = $("#gview_ReportGrid");
                var bodyHeight = window.innerHeight - gridContainer.offset().top;
                var actualHeight = $("#ReportGrid").height();
                var contentHeight = actualHeight > bodyHeight ? bodyHeight : actualHeight;
                var tableWidth = $("#ReportGrid").width();
                var contentWidth = $bdiv.width();
                var newHeight = tableWidth > contentWidth ? contentHeight + 18 : contentHeight;
                $bdiv.height(Math.min(newHeight, gridHeight))
                                    .css("max-height", gridHeight);
            }, 200);
        },

        initialize: function () {
            ActiveCustomer.initEvent();

            ActiveCustomerOption.gridOption.onSortCol = function (index/*, columnIndex, sortOrder*/) {
                Site.blockUI();
                window.setTimeout(function () {
                    _removeSorting = false;
                    if (_sortColumnName === index) {
                        _sortOrder = _sortOrder === "asc" ? "desc" : "asc";
                    } else {
                        _sortOrder = "asc";
                        _sortColumnName = index;
                    }

                    cacheData.Data = sortData(cacheData.Data);
                    ActiveCustomer.bindData(cacheData);

                    Site.unBlockUI();
                }, 100);
                return 'stop';
            };

            Site.addUserGuide({
                resourceName: "ActiveCustomerSummary",
                title: Site.getSingleResource("ActiveCustomerSummary")
            });

            $("#ProductList").css("width", "auto");
        },

        initEvent: function () {
            /* open betlist popup*/
            $("body").on("click", ".bet-list-link", function () {
                var dataDrillDown = $(this).attr("data-drill-down");
                ActiveCustomer.openBetlistPopUp(dataDrillDown);
            });

            Site.setConstraintMonthRange(VALID_MONTH_RANGE, ActiveCustomerOption.urlExcelData);

            $('#SubmitButton').click(function () {
                $('#IsExportExcel').val(false);
                ActiveCustomer.submit();
            });

            $('#ExportToExcel').click(function () {

                $('#IsExportExcel').val(true);
                var $mainForm = $("#mainform");
                var url = Site.resolveClientUrl(ActiveCustomerOption.urlExcelData);
                $mainForm.attr("action", url);
                $mainForm.submit();
                $('#IsExportExcel').val(false);
                return false;
            });
        },

        //#region Layout

        setReportTitle: function (currencyId, fromdate, todate) {
            var titlePattern = Site.getSingleResource('ActiveCustomerSummary') + ', {0} → {1} - {2}';
            var viewinKey = Site.getSingleResource("Viewin");
            //get currencyName
            var currencyTextList =
            {
                "-3": viewinKey.replace("{0}", "GBP"),
                "-2": viewinKey.replace("{0}", "USD"),
                "-1": viewinKey.replace("{0}", "EUR"),
                "0": Site.getSingleResource('AllCurrencies')
            };

            var currencyStr = currencyTextList[currencyId] === undefined
                ? viewinKey.replace("{0}", $('#CurrencyId option[value="' + currencyId + '"]').text())
                : currencyTextList[currencyId];

            var title = $.format(titlePattern, fromdate.toString("M/dd/yyyy"), todate.toString("M/dd/yyyy"), currencyStr);
            $("div.report-title").show().html(title);
        },
        //#end

        submit: function () {
            Report.setOriginalProduct();
            Site.updateOriginalLicenseeOption();

            var validProduct = Report.ValidProducts();
            if (!validProduct) {
                return false;
            }

            var mainForm = $("#mainform");
            var data = mainForm.serializeObject();
            if (_sortColumnName) {
                data.sortBy = _sortColumnName;
                data.sortOrder = _sortOrder;
            }

            var option = {
                type: "POST",
                data: data,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(ActiveCustomerOption.urlData),
                success: function (response) {
                    if (response != null) {
                        ActiveCustomer.setReportTitle($("#CurrencyId").val(), $("#FromDate").val(), $("#ToDate").val());

                        cacheData = response;
                        ActiveCustomer.bindData(response);
                    }
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function (/*jqXhr, textStatus*/) {
                    Site.unBlockUI();
                }
            };
            if (ajaxRequest != null) {
                ajaxRequest.abort();
            }

            ajaxRequest = $.ajax(option);

            return true;
        },

        bindTotal: function (grantTotalCustomer) {
            var $footerTable = $(".ui-jqgrid-ftable");
            var colspan = $footerTable.find("td").length;
            $footerTable.find("td:eq(0)")
                .html(Site.getSingleResource("TotalActiveCustomer") + " = " + jqGridAdditional.formatDecimal(grantTotalCustomer))
                .attr("colspan", colspan)
                .css("text-align", "left")
                .nextAll().remove();
        },

        bindData: function (response) {
            jqGridAdditional.clearGrid("ReportGrid");

            var reportGrid = $("#ReportGrid");
            var productOptions = $.parseJSON(response.ProductOptions);
            var productCount = productOptions.length;
            var numOfRow = response.Data.length;

            var gridOption = ActiveCustomerOption.getGridOption(productOptions);
            gridOption.width = productCount > 3 ? undefined : MIN_GRID_WIDTH;
            gridOption.data = response.Data;
            gridOption.rowNum = ActiveCustomerOption.nonePagingSize;
            gridOption.rowList = ActiveCustomerOption.rowList;
            gridOption.rowTotal = numOfRow;

            gridOption.gridComplete = function () {
                jqGridAdditional.gridComplete(this, gridOption, 1);

                if (response == null || numOfRow === 0) {
                    jqGridAdditional.setNoInfo(gridOption.colModel);
                }
                else {

                    ActiveCustomer.bindTotal(response.GrantTotalCustomer);
                    ActiveCustomer.resizeGrid();
                }

                Site.unBlockUI();
            };

            // Show the grid
            reportGrid.jqGrid(gridOption);

            if (!_removeSorting) {
                reportGrid.closest("div.ui-jqgrid-view")
                    .find("div.ui-jqgrid-hdiv table.ui-jqgrid-htable tr.ui-jqgrid-labels > th.ui-th-column > div.ui-jqgrid-sortable")
                    .each(function (index, element) {
                        var $this = $(element);
                        $this.find("span.s-ico").remove();

                        if (element.id.indexOf(_sortColumnName) > -1) {
                            var sortClass = "sort-" + _sortOrder;
                            var sortSpan = $("<span style='display: inline; margin: 2px 0px 0px 2px'></span");
                            sortSpan.addClass(sortClass);
                            $this.append(sortSpan);
                            $this.addClass("sorted-heading");
                        }
                    });
            }

            reportGrid.jqGrid('showCol', 'rn');
            ActiveCustomerOption.setGroupHeaderColumns(reportGrid, productOptions);

            jqGridAdditional.showRecommendedBrowser(gridOption.pager);

            jqGridAdditional.resetLayout(reportGrid);

            $("div.container_26 div.grid_26.content-filter").width(940);

            var gridWidth = reportGrid.width();
            // for the vertical scrollbar not overlap the grid content
            if (gridWidth < 500) {
                gridWidth = 500;
            } else {
                gridWidth += 20;
            }
            gridWidth = Math.min(1040, gridWidth);
            reportGrid.jqGrid("setGridWidth", gridWidth);
            $(".report-title").width(gridWidth - 25);
        }

        //#end
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    ActiveCustomer.initialize();
});

$(window).resize(function () {
    "use strict";
    ActiveCustomer.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});