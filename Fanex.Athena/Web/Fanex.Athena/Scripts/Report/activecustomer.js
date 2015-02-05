/*global jqGridAdditional, JSLINQ, ActiveCustomerOption, Report, Site */
/// <reference path="/@JSense.js" />
///
var ActiveCustomer = (function ($) {
    "use strict";
    
    var cacheData = {};
    var ajaxRequest = null;
    var totalRowCount = 0;
    var _sortColumnName = null;
    var _sortOrder = "desc";
    var _removeSorting = false;
    var _reportBy = null;
    var _resizeTimer = null;
    var _submitData; // cache submit data

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
        getTotalRowCount: function () {
            return totalRowCount;
        },
        initialize: function () {
            ActiveCustomer.initEvent();
            ActiveCustomer.initSetting();
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
                    ActiveCustomer.bindData(_reportBy, cacheData);

                    Site.unBlockUI();
                }, 100);
                return 'stop';
            };

            Site.addUserGuide({
                resourceName: "LicenseeWL",
                title: Site.getSingleResource("ActiveCustomer")
            });
            Site.addFilterCollapseExpand();
        },

        initSetting: function () {
            ActiveCustomerOption.hasCustomerComm = Site.asBool($("#IsAllowedCustomerCommission").val());
            ActiveCustomerOption.licenseeId = parseInt($("#drillDownLicenseeId").val());
        },

        initEvent: function () {
            /* open betlist popup*/
            $("body").on("click", ".bet-list-link", function () {
                var dataDrillDown = $(this).attr("data-drill-down");
                ActiveCustomer.openBetlistPopUp(dataDrillDown);
            });

            $('#SubmitButton').click(function () {
                // Site.blockUI();
                $('#IsExportExcel').val(false);
                ActiveCustomer.submit();
            });

            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                var mainForm = $("#mainform");
                var url = Site.resolveClientUrl(ActiveCustomerOption.urlExcelData);
                console.log(url);
                mainForm.attr("action", url);
                mainForm.submit();
                $('#IsExportExcel').val(false);
                return false;
            });
        },

        //#region Layout
        setExcelUrl: function () {
            ActiveCustomerOption.urlExcelData = 'api/report/ActiveCustomer';
        },

        setReportTitle: function (currencyId, fromdate, todate) {
            var titlePattern = Site.getSingleResource('ActiveCustomer') + ', {0} → {1} - {2}';
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

        //#Get Data
        ajaxComplete: function (response) {
            if (response != null) {
                var reportBy = $("input[name='ReportBy']:checked").val();
                _reportBy = reportBy;

                //reset layout

                ActiveCustomer.setReportTitle($("#CurrencyId").val(), $("#FromDate").val(), $("#ToDate").val());

                cacheData = response;
                ActiveCustomer.bindData(reportBy, response);
            }
        },

        submit: function () {
            Report.setOriginalProduct();
            Site.updateOriginalLicenseeOption();

            var validProduct = Report.ValidProducts();
            if (!validProduct) {
                return false;
            }

            var mainForm = $("#mainform");
            var data = mainForm.serializeObject();
            var option = {
                type: "POST",
                data: mainForm.serialize() +
                    (_sortColumnName ?
                        "&sortby=" + encodeURI(_sortColumnName) + "&sortOrder=" + _sortOrder : ""),
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(ActiveCustomerOption.urlData),
                success: function (response) {
                    _submitData = {
                        ProductOptions: response.ProductOptions,
                        DrawFilter: data.DrawFilter,
                        CurrencyId: data.CurrencyId,
                        ReportBy: Number(data.ReportBy),
                        FromDate: new Date(data.FromDate),
                        ToDate: new Date(data.ToDate),
                        CurrencyName: data.CurrencyName,
                        IsExcludedTestData: data.IsExcludedTestData
                    };

                    ActiveCustomer.setExcelUrl(0, 0, 0, "", "");
                    ActiveCustomer.ajaxComplete(response);
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

        lastDateOfMonth: function (year, month) {
            return new Date(year, month + 1, 0);
        },

        //#BindReport

        bindTotal: function (grantTotalCustomer) {
            var $footerTable = $(".ui-jqgrid-ftable");
            var colspan = $footerTable.find("td").length;
            $footerTable.find("td:eq(0)")
                .html(Site.getSingleResource("TotalActiveCustomer") + " = " + grantTotalCustomer)
                .attr("colspan", colspan)
                .css("text-align", "left")
                .nextAll().remove();
        },

        bindData: function (reportBy, response) {
            var $gridBody = $(".ui-jqgrid-bdiv");

            var scrollLeft = $gridBody.length > 0 ? $gridBody[0].scrollLeft : 0;

            jqGridAdditional.clearGrid("ReportGrid");

            var reportGrid = $("#ReportGrid");
            var productOptions = $.parseJSON(_submitData.ProductOptions);
            productOptions = productOptions.reverse(); //make orders of product columns like excel file
            var numOfRow = response.Data.length;

            var gridOption = ActiveCustomerOption.getGridOption(reportBy, productOptions);
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

                var gridWidth = reportGrid.width();

                if (gridWidth < 200) {
                    gridWidth = 200;
                } else {
                    gridWidth += 20;
                }
                gridWidth = Math.max(200, gridWidth);
                reportGrid.jqGrid("setGridWidth", gridWidth);

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

            $gridBody = $(".ui-jqgrid-bdiv");
            if ($gridBody.length > 0) {
                $gridBody[0].scrollLeft = scrollLeft;
            }


        }

        //#end
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    ActiveCustomer.initialize();
    //alert(Date.parse('Apr 5, 2013'));
});

$(window).resize(function () {
    "use strict";
    var reportGrid = $("#ReportGrid");
    jqGridAdditional.resetLayout(reportGrid);
    ActiveCustomer.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});