define([
    "jquery",
    "common/querystring",
    "common/site",
    "../report",
    "./platformwinloss.option",
    "lib/jqgrid/jquery.jqgrid.additional",
    "components/pluginhelper/monthpicker",
    "components/pluginhelper/multiselect",
    "common/popup",
    "helpers/excel",
    "lib/jqgrid/jquery.jqGrid.en",
    "lib/jqgrid/jquery.jqGrid.min",
    "extensions/jquery.extension",
    "lib/linq"],
    function ($, QueryString, Site, Report, PlatformWinLossOption, jqGridAdditional, MonthPicker, MultiSelect, Popup, ExcelHelper) {
        "use strict";

        var BETLIST_IGNORED_PRODUCTS = ["AB"];
        var platforms = {};

        var _cacheData = {};
        var _filterData = {};

        var gridControl = {
            level: 0,
            pageSize: 50,
            sortColumnName: null,
            sortOrder: "desc",
            removerSorting: true
        };

        function initialize() {
            Site.initialize();
            Site.addUserGuide({
                resourceName: "PlatformWinLoss",
                title: Site.getSingleResource("PlatformWinLoss")
            });

            platforms = {
                "0": Site.getSingleResource("All"),
                "1": Site.getSingleResource("Asia"),
                "2": Site.getSingleResource("NewAsia"),
                "3": Site.getSingleResource("Euro"),
                "4": Site.getSingleResource("Wap"),
                "5": Site.getSingleResource("SmartSite")
            };

            _me.initEvent();
            _me.initSetting();

            MultiSelect.initProductList();
            MonthPicker.initMonthPicker({
                minDate: Site.getHistoryDate(),
                maxDate: $("#MaxDate").val()
            });

            $(window).on("resize", function () {
                resizeGrid();
            });

            $(window).on("unload", function () {
                $(window).off("resize");
            });
        }

        function initEvent() {
            $('#SubmitButton').click(function () {
                gridControl.removeSorting = true;
                _me.submit();
            });

            $("body").on("click", ".drilldown", function () {
                var dataDrillDown = $(this).data("drill-down");
                _me.drillDown(gridControl.level + 1, dataDrillDown);
            });

            $("body").on("click", ".bet-list-link", function () {
                var dataDrillDown = $(this).data("drill-down");
                _me.openBetlistPopUp(gridControl.level, dataDrillDown);
            });

            $("#level-link-container").on("click", ".level-link", function () {
                var level = Number($(this).attr("data-level"));
                gridControl.level = level;
                if (level < 1) {
                    Report.renderBreadCrumb(level);
                    gridControl.level = level;
                    _me.bindData(level, _cacheData[level], _filterData[level].ProductOptions);
                }
            });

            $('#ExportToExcel').click(function () {
                _me.exportToExcel(gridControl.level);
            });
        }

        function initSetting() {
            gridControl.pageSize = PlatformWinLossOption.pageSize;
            PlatformWinLossOption.hasCustomerComm = Site.asBool($("#IsAllowedCustomerCommission").val());

            PlatformWinLossOption.gridOption.onSortCol = function (index) {
                Site.blockUI();
                gridControl.pageSize = Number($("#ReportGrid").getGridParam("rowNum"));
                window.setTimeout(function () {
                    gridControl.removeSorting = false;
                    if (gridControl.sortColumnName === index) {
                        gridControl.sortOrder = gridControl.sortOrder === "asc" ? "desc" : "asc";
                    } else {
                        gridControl.sortOrder = "asc";
                        gridControl.sortColumnName = index;
                    }

                    _cacheData[gridControl.level].Data = sortData(_cacheData[gridControl.level].Data);

                    _me.bindData(gridControl.level, _cacheData[gridControl.level], _filterData[gridControl.level].ProductOptions);

                    Site.unBlockUI();
                }, 100);
                return "stop";
            };
        }

        function submit() {
            var validProduct = Report.ValidProducts();
            if (!validProduct) {
                return false;
            }

            gridControl.level = 0;
            var data = $("#platformwinloss").serializeObject();
            data.Level = gridControl.level;
            addDateParam(data);

            getData(data, Site.resolveClientUrl(PlatformWinLossOption.urlData), function (response) {
                var submitData = {
                    FromDay: data.FromDay,
                    ToDay: data.ToDay,
                    Month: data.Month,
                    Year: data.Year,
                    ProductOptions: $.parseJSON(response.ProductOptions),
                    DrawFilter: data.DrawFilter,
                    PlatformType: data.PlatformType,
                    CurrencyId: data.CurrencyId,
                    FromDate: data.FromDate,
                    ToDate: data.ToDate,
                    CurrencyName: data.CurrencyName,
                    IsExcludeTest: data.IsExcludeTest,
                    BaseCurrency: data.BaseCurrency
                };
                _filterData[gridControl.level] = submitData;
                _cacheData[gridControl.level] = response;
                _me.setReportTitle(submitData);
                Report.renderBreadCrumb(gridControl.level);
                _me.bindData(gridControl.level, response, submitData.ProductOptions);
            });

            return true;
        }

        function exportToExcel(level) {
            var data = Site.clone(_filterData[level]);
            data.IsExportExcel = true;
            data.Level = level;
            ExcelHelper.exportExcel(data, Site.resolveClientUrl(PlatformWinLossOption.urlExcelData));
            return false;
        }

        function bindData(level, response, productOptions) {
            jqGridAdditional.clearGrid("ReportGrid");

            var numOfRow = response.Data.length;
            if (numOfRow === 0) {
                jqGridAdditional.setNoInfo();
                return;
            }

            var reportGrid = $("#ReportGrid");
            var onPaging = false;
            var gridOption = PlatformWinLossOption.getGridOption(level, productOptions);
            gridOption.datastr = response.Data;
            gridOption.rowNum = level === 0 ? -1 : gridControl.pageSize;
            gridOption.treeGrid = level === 0;
            gridOption.onPaging = function (pgButton) {
                gridControl.pageSize = Number($("#ReportGrid").getGridParam("rowNum"));

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
                jqGridAdditional.gridComplete();

                var showPaging = level === 1 && numOfRow > PlatformWinLossOption.pageSize;
                $("#pg_ppdata").toggle(showPaging);

                if (onPaging) {
                    $(".ui-jqgrid-ftable").html($("#footer").html());
                }
                else {
                    _me.bindTotal(level, response.Total, response.SummaryTotal);
                    $("#footer").html($(".ui-jqgrid-ftable").html());
                }

                onPaging = false;
                resizeGrid();

                if (level === 0) {
                    $("th[role='columnheader']").find("div").removeClass("ui-jqgrid-sortable");
                }

                Site.unBlockUI();
            };

            reportGrid.jqGrid(gridOption);

            /*group header*/
            if (PlatformWinLossOption.hasCustomerComm) {
                jqGridAdditional.setGroupCustomerWLColumns(reportGrid);
            }

            if (level === 1 && !gridControl.removeSorting) {
                jqGridAdditional.addSortableHeader(reportGrid, gridControl.sortColumnName, gridControl.sortOrder);
            }
        }

        function bindTotal(level, totalData, summaryTotal) {
            var columnModel = PlatformWinLossOption.getColModel(level, _filterData[level].ProductOptions);

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
        }

        function setReportTitle(data) {
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
            var baseCurrencyText = baseCurrency[baseCurrencyKey] ?
                baseCurrency[baseCurrencyKey]
                : viewinKey.replace("{0}", selectedCurrency);

            var platformName = platforms[data.PlatformType];

            var productText = Site.getSingleResource('Product') + ': ';
            productText = data.ProductOptions && data.ProductOptions.length === 1 ?
                productText + data.ProductOptions[0].Code :
                getProductText(data.ProductOptions);

            var title = $.format(titlePattern, data.FromDate, data.ToDate,
                        currencyText, baseCurrencyText, platformName, ', ' + productText);

            $(".report-title").html(title);
            $(".panel-heading").removeClass("hide");
        }

        function drillDown(level, data) {
            gridControl.level = level;
            gridControl.pageSize = PlatformWinLossOption.pageSize;

            var submitData = _filterData[level - 1];
            var productOptions;
            if (data.ProductCode) {
                productOptions = [{ Code: data.ProductCode, Status: true }];
            }
            else {
                productOptions = submitData.ProductOptions;
            }

            var drilldownData = Site.clone(submitData);
            drilldownData.Level = level;
            drilldownData.PlatformType = data.PlatformType;
            drilldownData.ProductOptions = productOptions;

            getData(drilldownData, Site.resolveClientUrl(PlatformWinLossOption.urlData), function (response) {
                _cacheData[level] = response;
                _filterData[level] = drilldownData;
                _me.setReportTitle(drilldownData);
                Report.renderBreadCrumb(level);
                _me.bindData(level, response, productOptions);
            });
        }

        function getData(data, url, callback) {
            $.ajax({
                type: "POST",
                data: data,
                dataType: "json",
                cache: false,
                url: url,
                success: function (response) {
                    callback(response);
                },
                beforeSend: Site.blockUI,
                complete: Site.unBlockUI
            });
        }

        function openBetlistPopUp(onlevel, dataDrillDown) {
            var url = Site.resolveClientUrl("PlatformWinLossBetList/Index");
            var onLevelData = _filterData[onlevel];

            var param = {
                FromDate: onLevelData.FromDate,
                ToDate: onLevelData.ToDate,
                PlatformType: onLevelData.PlatformType,
                ExcludeTest: onLevelData.IsExcludeTest,
                CurrencyId: dataDrillDown.DrillDownCurrencyId,
                CurrencyName: dataDrillDown.DrillDownCurrencyName,
                DrawFilter: onLevelData.DrawFilter,
                CustId: dataDrillDown.CustomerId,
                SystemId: dataDrillDown.SystemId,
                ExternalId: dataDrillDown.ExternalId,
                BaseCurrency: onLevelData.BaseCurrency
            };

            url = QueryString.setParams(url, param);

            var productCodeList = [];
            for (var i = 0; i < onLevelData.ProductOptions.length; i++) {
                var product = onLevelData.ProductOptions[i];
                if (product.Status && $.inArray(product.Code, BETLIST_IGNORED_PRODUCTS)) {
                    productCodeList.push(product.Code);
                }
            }
            url = Site.setProductCodeListParam(url, productCodeList);

            var popH = Math.min($(window).height() - 100, 650), popW = 920;
            var popupOption = { id: "betlist-popup", width: popW + 50, height: popH + 50, title: "Customer Bet List" };
            Popup.openPopupWithIframe(url, popupOption);
        }

        function addDateParam(data) {
            var fromDate = new Date($("#FromDate").val());
            var toDate = new Date($("#ToDate").val());
            data.FromDay = fromDate.getDate();
            data.ToDay = toDate.getDate();
            data.Month = fromDate.getMonth() + 1;
            data.Year = fromDate.getFullYear();
        }

        function sortData(arrayData) {
            var sortType = gridControl.sortOrder === "asc" ? "OrderBy" : "OrderByDescending";
            var sortFunction = function (row) {
                return row[gridControl.sortColumnName];
            };

            var data = Enumerable.From(arrayData)[sortType](sortFunction).ToArray();

            for (var i = 0; i < data.length; i++) {
                data[i].Index = i + 1;
            }

            return data;
        }

        function getProductText(productOptions) {
            var productSeleted = Enumerable.From(productOptions)
                .Where(function (x) { return x.Status; })
                .Select(function (x) { return x.Code; })
                .ToArray();

            var productName = productSeleted.length === productOptions.length
                ? Site.getSingleResource('All')
                : productSeleted.join(", ");

            return Site.getSingleResource("Product") + ": " + productName;
        }

        function resizeGrid() {
            var reportGrid = $("#ReportGrid");
            jqGridAdditional.updateGridHeight();
            jqGridAdditional.updateGridWidth(reportGrid);
        }

        var _me = {
            initialize: initialize,
            initSetting: initSetting,
            initEvent: initEvent,
            submit: submit,
            bindData: bindData,
            bindTotal: bindTotal,
            setReportTitle: setReportTitle,
            drillDown: drillDown,
            openBetlistPopUp: openBetlistPopUp,
            exportToExcel: exportToExcel
        };

        return _me;
    });