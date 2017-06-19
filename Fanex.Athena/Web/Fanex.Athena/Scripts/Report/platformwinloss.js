/*global Site, Report, QueryString, PlatformWinLossOption, JSLINQ,jqGridAdditional, ProductListHelper*/
/// <reference path="/@JSense.js" />
/// //##
/*
* Note:in this report, parameter level have 3 value
*  "0" for master,"1" for agent and "2" for member
*/

var PlatformWinLoss = (function ($) {
    "use strict";
    var BETLIST_IGNORED_PRODUCTS = ["AB"];
    var cacheData = {};
    var _level = null;
    var _sortColumnName = null;
    var _sortOrder = "desc";
    var _removeSorting = false;
    var _pageSize = 0;
    var _submitData = {};
    var _drilldownData = {};

    function sortData(arrayData) {
        var data = _sortOrder === "asc" ?
             JSLINQ(arrayData).OrderBy(function (x) { return x[_sortColumnName]; }).items
             : JSLINQ(arrayData).OrderByDescending(function (x) { return x[_sortColumnName]; }).items;
        for (var i = 0; i < data.length; i++) {
            data[i].Index = i + 1;
        }
        return data;
    }

    function getProductText(productOptionJson) {
        var productOptions = $.parseJSON(productOptionJson);
        var productText = "";

        var selectedProductCount = 0;
        for (var i = 0; i < productOptions.length; i++) {
            if (productOptions[i].Status) {
                selectedProductCount++;
                productText += productText.length === 0 ? productOptions[i].Code : ", " + productOptions[i].Code;
            }
        }

        return Site.getSingleResource("Product") + ": " +
            (selectedProductCount === productOptions.length ? Site.getSingleResource('All') : productText);
    }

    return {
        //#region Init
        initialize: function () {
            ProductListHelper.initProductList();
            PlatformWinLoss.initSetting();
            PlatformWinLoss.initMonthPicker();
            PlatformWinLoss.initEvent();
            Site.addFilterCollapseExpand();
            PlatformWinLossOption.gridOption.onSortCol = function (index) {
                _pageSize = Number($("#ReportGrid").getGridParam("rowNum"));
                Site.blockUI();
                window.setTimeout(function () {
                    _removeSorting = false;
                    if (_sortColumnName === index) {
                        _sortOrder = _sortOrder === "asc" ? "desc" : "asc";
                    } else {
                        _sortOrder = "asc";
                        _sortColumnName = index;
                    }

                    cacheData[_level].Data = sortData(cacheData[_level].Data);

                    var productOptions = _level === 0 ? $.parseJSON(_submitData.ProductOptions) : _drilldownData.ProductOptions;

                    PlatformWinLoss.bindData(_level, cacheData[_level], productOptions);

                    Site.unBlockUI();
                }, 100);
                return 'stop';
            };
            Site.addUserGuide({
                resourceName: "PlatformWinLoss",
                title: Site.getSingleResource("PlatformWinLoss")
            });
        },
        initSetting: function () {
            _pageSize = PlatformWinLossOption.pageSize;
            PlatformWinLossOption.hasCustomerComm = Site.asBool($("#IsAllowedCustomerCommission").val());
        },
        initMonthPicker: function () {
            $('#DateControl').monPickr({
                minDate: Site.getHistoryDate(),
                maxDate: $("#MaxDate").val(),
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
                }
            });
        },
        initEvent: function () {
            $("body").on("click", ".bet-list-link", function () {
                var dataDrillDown = $.parseJSON($(this).attr("data-drill-down"));
                PlatformWinLoss.openBetlistPopUp(dataDrillDown);
            });

            $("body").on("click", ".drilldown", function () {
                var dataDrillDown = $.parseJSON($(this).attr("data-drill-down"));
                PlatformWinLoss.drillDown(dataDrillDown);
            });

            $("#SiteList input[type=checkbox]").checkboxgroup();

            $('#SubmitButton').click(function () {
                _removeSorting = true;
                PlatformWinLoss.submit();
            });

            $('#ExportToExcel').click(function () {
                var data = _level === 0 ? Site.clone(_submitData) : Site.clone(_drilldownData);
                data.IsExportExcel = true;
                data.Level = _level;
                Site.exportExcel(data, Site.resolveClientUrl(PlatformWinLossOption.urlExcelData));
                return false;
            });

            $("#level-link-container").on("click", ".level-link", function () {
                var level = Number($(this).attr("data-level"));
                return PlatformWinLoss.goBack(level);
            });
        },

        //#endregion

        //#region Layout

        renderBreadCrumb: function (level) {
            //hide history link
            if (level === 0) {
                $(".breadCrumbHolder").hide();
            }
            else {
                /*show history link*/
                $(".breadCrumbHolder").show();
                $("#history-link-container").width($("#ReportGrid").width() + 17);
                //add more 20px for scrolling
                $("div#history-link-container li.level0").show();

                /*show history link*/
                $(".breadCrumbHolder").show();
                $(".level0").show();
                $(".level1").toggleClass("level-link", level !== 1);
            }
        },
        setReportTitle: function (data) {
            var titlePattern = Site.getSingleResource('PlatformWinLoss')
                + ', {0} → {1} - {2} - {3} - '
                + Site.getSingleResource('Platform') + ': {4}{5}';

            var selectedCurrency = $('#CurrencyId option[value="' + data.CurrencyId + '"]').text();
            var currencyText = data.CurrencyId === "0" ?
                Site.getSingleResource('AllCurrencies') :
                Site.getSingleResource('Currency') + " " + selectedCurrency;

            var viewinKey = Site.getSingleResource("Viewin");
            var baseCurrency = {
                "gbp": viewinKey.replace("{0}", "GBP"),
                "usd": viewinKey.replace("{0}", "USD"),
                "eur": viewinKey.replace("{0}", "EUR")
            };

            var baseCurrencyKey = $('#BaseCurrency').val();
            var baseCurrencyText = baseCurrency[baseCurrencyKey] === undefined
                ? viewinKey.replace("{0}", selectedCurrency)
                : baseCurrency[baseCurrencyKey];

            var platformName = PlatformWinLossOption.Platforms[data.PlatformType];
            var productText = Site.getSingleResource('Product') + ': ';
            productText = (data.ProductOptions && data.ProductOptions.length === 1) ?
                productText + data.ProductOptions[0].Code :
                getProductText(_submitData.ProductOptions);
            var title = $.format(titlePattern, _submitData.FromDate.toString("M/d/yyyy"), _submitData.ToDate.toString("M/d/yyyy"),
                        currencyText, baseCurrencyText, platformName, ', ' + productText);

            $(".report-title").html(title);
        },
        //#endregion

        //#region BindReport

        bindTotal: function (level, totalData, summaryTotal) {
            var columnModel = PlatformWinLossOption.getTotalColModel();
            if (level === 0) {
                jqGridAdditional.assignTotal(totalData, columnModel);
                jqGridAdditional.setGroupFooters(1);
            }
            else {
                var colspan = PlatformWinLossOption.totalColspan[level];
                var summaryColspan = colspan + 1;
                jqGridAdditional.assignTotal(totalData, columnModel);
                var summaryRowCount = Report.bindSummaryTotal(columnModel, summaryTotal);
                jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount);
            }
        },
        bindData: function (level, response, productOptions) {
            jqGridAdditional.clearGrid("ReportGrid");
            var reportGrid = $("#ReportGrid");
            var onPaging = false;
            var gridOption = PlatformWinLossOption.getGridOption(level, productOptions);
            var numOfRow = response.Data.length;
            gridOption.datastr = response.Data;
            gridOption.rowList = PlatformWinLossOption.rowList;
            gridOption.rowNum = level === 1 ? _pageSize : -1;
            gridOption.treeGrid = level === 0;
            gridOption.onPaging = function (pgButton) {
                _pageSize = Number($("#ReportGrid").getGridParam("rowNum"));
                if (numOfRow > PlatformWinLossOption.pageSize) {
                    onPaging = true;
                    if ($("#" + pgButton).hasClass("ui-state-disabled") === false) {
                        Site.blockUI();
                    }
                }
                else {
                    return 'stop';
                }
            };

            gridOption.gridComplete = function () {
                jqGridAdditional.gridComplete(this, gridOption);
                if (level === 1 && numOfRow > PlatformWinLossOption.pageSize) {
                    $("#pg_ppdata").show();
                }
                else {
                    $("#pg_ppdata").hide();
                }

                if (response == null || numOfRow === 0) {
                    jqGridAdditional.setNoInfo(gridOption.colModel);
                }
                else {
                    if (onPaging) {
                        $(".ui-jqgrid-ftable").html($("#footer").html());
                    }
                    else {
                        PlatformWinLoss.bindTotal(level, response.Total, response.SummaryTotal);
                        $("#footer").html($(".ui-jqgrid-ftable").html());
                    }

                    onPaging = false;
                }

                jqGridAdditional.resizeGrid();
                $(".cell-wrapperleaf", "#ReportGrid").closest("tr").find("td").addClass("highlight");
                Site.unBlockUI();
            };
            reportGrid.jqGrid(gridOption);
            /*group header*/
            if (PlatformWinLossOption.hasCustomerComm) {
                jqGridAdditional.setGroupCustomerWLColumns(reportGrid);
            }
            else {
                jqGridAdditional.resetHeaderHeight(reportGrid);
            }
            jqGridAdditional.resetLayout(reportGrid);

            if (level === 1 && !_removeSorting) {
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
        },

        //#endregion

        //#region GetData

        submit: function () {
            _level = 0;
            Report.setOriginalProduct();
            Site.updateOriginalLicenseeOption();

            var validProduct = Report.ValidProducts();
            if (!validProduct) {
                return false;
            }

            var fromdate = new Date($("#Year").val(), $("#Month").val() - 1, $("#FromDay").val(), 0, 0, 0, 0);
            var todate = new Date($("#Year").val(), $("#Month").val() - 1, $("#ToDay").val(), 0, 0, 0, 0);

            var data = $("#MainForm").serializeObject();
            data.FromDate = fromdate;
            data.ToDate = todate;
            data.IsExcludeTest = $('#IsExcludeTest').is(':checked');
            data.Level = _level;
            $.ajax({
                type: "POST",
                data: data,
                dataType: "json",
                cache: false,
                url: Site.resolveClientUrl(PlatformWinLossOption.urlData),
                success: function (response) {
                    _submitData = {
                        FromDay: data.FromDay,
                        ToDay: data.ToDay,
                        Month: data.Month,
                        Year: data.Year,
                        ProductOptions: response.ProductOptions,
                        DrawFilter: data.DrawFilter,
                        PlatformType: data.PlatformType,
                        CurrencyId: data.CurrencyId,
                        FromDate: new Date(data.FromDate),
                        ToDate: new Date(data.ToDate),
                        CurrencyName: data.CurrencyName,
                        IsExcludeTest: data.IsExcludeTest,
                        BaseCurrency: data.BaseCurrency
                    };
                    cacheData[_level] = response;
                    PlatformWinLoss.setReportTitle(_submitData);
                    PlatformWinLoss.renderBreadCrumb(_level);
                    PlatformWinLoss.bindData(_level, response, $.parseJSON(_submitData.ProductOptions));
                },
                beforeSend: Site.blockUI,
                complete: Site.unBlockUI
            });

            return true;
        },

        drillDown: function (data) {
            _level = 1;
            _pageSize = PlatformWinLossOption.pageSize;

            var productOptions;
            if (data.ProductCode) {
                productOptions = [{ Code: data.ProductCode, Status: true }];
            }
            else {
                productOptions = $.parseJSON(_submitData.ProductOptions);
            }

            _drilldownData =
            {
                Level: _level,
                FromDay: _submitData.FromDay,
                ToDay: _submitData.ToDay,
                Month: _submitData.Month,
                Year: _submitData.Year,
                FromDate: _submitData.FromDate,
                ToDate: _submitData.ToDate,
                ProductOptions: productOptions,
                PlatformType: data.PlatformType,
                DrawFilter: _submitData.DrawFilter,
                IsExcludeTest: _submitData.IsExcludeTest,
                CurrencyId: _submitData.CurrencyId,
                BaseCurrency: _submitData.BaseCurrency,
                CurrencyName: _submitData.CurrencyName,
            };
            $.ajax({
                type: "POST",
                data: _drilldownData,
                dataType: "json",
                cache: false,
                url: Site.resolveClientUrl(PlatformWinLossOption.urlData),
                success: function (response) {
                    cacheData[_level] = response;
                    PlatformWinLoss.setReportTitle(_drilldownData);
                    PlatformWinLoss.renderBreadCrumb(_level);
                    PlatformWinLoss.bindData(_level, response, productOptions);
                },
                beforeSend: Site.blockUI,
                complete: Site.unBlockUI
            });
        },

        goBack: function (level) {
            _level = level;
            PlatformWinLoss.bindData(level, cacheData[level]);
            if (level === 0) {
                PlatformWinLoss.renderBreadCrumb(level);
                PlatformWinLoss.setReportTitle({
                    CurrencyId: _submitData.CurrencyId,
                    PlatformType: _submitData.PlatformType
                });
            }
            else if (level === 1) {
                PlatformWinLossOption.urlExcelData = PlatformWinLossOption.urlExcelData.replace("level=2", "level=1");
                PlatformWinLoss.renderBreadCrumb(level);
            }
        },

        lastDateOfMonth: function (year, month) {
            return new Date(year, month + 1, 0);
        },

        openBetlistPopUp: function (dataDrillDown) {
            var url = Site.resolveClientUrl("PlatformWinLossBetList/Index");

            url = QueryString.setParam(url, "fromdate", _submitData.FromDate.toString("M/d/yyyy"));
            url = QueryString.setParam(url, "todate", _submitData.ToDate.toString("M/d/yyyy"));
            url = QueryString.setParam(url, "PlatformType", _drilldownData.PlatformType);
            url = QueryString.setParam(url, "excludetest", _submitData.IsExcludeTest);
            url = QueryString.setParam(url, "CurrencyId", dataDrillDown.DrillDownCurrencyId);
            url = QueryString.setParam(url, "CurrencyName", dataDrillDown.DrillDownCurrencyName);
            url = QueryString.setParam(url, "drawfilter", _submitData.DrawFilter);
            url = QueryString.setParam(url, "custid", dataDrillDown.CustomerId);
            url = QueryString.setParam(url, "systemId", dataDrillDown.SystemId);
            url = QueryString.setParam(url, "externalId", dataDrillDown.ExternalId);
            url = QueryString.setParam(url, "BaseCurrency", _submitData.BaseCurrency);

            var productCodeList = [];
            for (var i = 0; i < _drilldownData.ProductOptions.length; i++) {
                var product = _drilldownData.ProductOptions[i];
                if ($.inArray(product.Code, BETLIST_IGNORED_PRODUCTS)) {
                    productCodeList.push(product.Code);
                }
            }

            var popH = Math.min(600, window.innerHeight - 100), popW = 920;
            var dialogForm = '<div title="Customer Bet List"><iframe frameborder="0" width="' + popW + '" height="100%" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: popW + 50,
                height: popH + 50
            });
        }
        //#endregion
    };
})(jQuery);
$(window).load(function () {
    "use strict";
    PlatformWinLoss.initialize();
});
//#end
$(window).resize(function () {
    "use strict";
    var reportGrid = $("#ReportGrid");
    jqGridAdditional.resetLayout(reportGrid);
    jqGridAdditional.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});