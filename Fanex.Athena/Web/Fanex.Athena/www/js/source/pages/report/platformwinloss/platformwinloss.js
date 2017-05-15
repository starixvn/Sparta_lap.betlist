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
    "lib/jqgrid/jquery.jqGrid.en",
    "lib/jqgrid/jquery.jqGrid.min",
    "extensions/jquery.extension",
    "lib/linq"],
    function ($, QueryString, Site, Report, PlatformWinLossOption, jqGridAdditional, MonthPicker, MultiSelect, Popup) {
        "use strict";

        var BETLIST_IGNORED_PRODUCTS = ["AB"];
        var platforms = {};

        var _cacheData = [];
        var _submitData = {}; // cache submit data of level 0
        var _drilldownData = {};

        var gridControl = {
            level: 0,
            pageSize: 50,
            sortColumnName: null,
            sortOrder: "desc",
            removerSorting: true
        };

        function initEvent() {
            $('#SubmitButton').click(function () {
                gridControl.removeSorting = true;
                submit();
            });

            $("body").on("click", ".drilldown", function () {
                var dataDrillDown = $.parseJSON($(this).attr("data-drill-down"));
                drillDown(dataDrillDown);
            });

            $("body").on("click", ".bet-list-link", function () {
                var dataDrillDown = $.parseJSON($(this).attr("data-drill-down"));
                openBetlistPopUp(dataDrillDown);
            });

            $("#level-link-container").on("click", ".level-link", function () {
                var level = Number($(this).attr("data-level"));
                gridControl.level = level;
                if (level === 0) {
                    goBackLevel0();
                }
            });

            $('#ExportToExcel').click(function () {
                var data = gridControl.level === 0 ? Site.clone(_submitData) : Site.clone(_drilldownData);
                data.IsExportExcel = true;
                data.Level = gridControl.level;
                Site.exportExcel(data, Site.resolveClientUrl(PlatformWinLossOption.urlExcelData));
                return false;
            });
        }

        function initSetting() {
            gridControl.pageSize = PlatformWinLossOption.pageSize;
            PlatformWinLossOption.hasCustomerComm = Site.asBool($("#IsAllowedCustomerCommission").val());
            PlatformWinLossOption.licenseeId = parseInt($("#drillDownLicenseeId").val());

            PlatformWinLossOption.gridOption.onSortCol = function (index) {
                Site.blockUI();
                window.setTimeout(function () {
                    gridControl.removeSorting = false;
                    if (gridControl.sortColumnName === index) {
                        gridControl.sortOrder = gridControl.sortOrder === "asc" ? "desc" : "asc";
                    } else {
                        gridControl.sortOrder = "asc";
                        gridControl.sortColumnName = index;
                    }

                    _cacheData[gridControl.level].Data = sortData(_cacheData[gridControl.level].Data);

                    bindData(gridControl.level, _cacheData[gridControl.level], _drilldownData.ProductOptions);

                    Site.unBlockUI();
                }, 100);
                return 'stop';
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
                        ProductOptions: $.parseJSON(response.ProductOptions),
                        DrawFilter: data.DrawFilter,
                        PlatformType: data.PlatformType,
                        CurrencyId: data.CurrencyId,
                        FromDate: new Date(data.FromDate),
                        ToDate: new Date(data.ToDate),
                        CurrencyName: data.CurrencyName,
                        IsExcludeTest: data.IsExcludeTest,
                        BaseCurrency: data.BaseCurrency
                    };
                    _cacheData[gridControl.level] = response;
                    setReportTitle(_submitData);
                    Report.renderBreadCrumb(gridControl.level, PlatformWinLossOption.licenseeId);
                    bindData(gridControl.level, response, _submitData.ProductOptions);
                },
                beforeSend: Site.blockUI,
                complete: Site.unBlockUI
            });

            return true;
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
            gridOption.rowNum = gridControl.pageSize;
            gridOption.treeGrid = level === 0;
            gridOption.rowTotal = numOfRow;
            gridOption.onPaging = function (pgButton) {
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

                if (numOfRow > PlatformWinLossOption.pageSize) {
                    $("#pg_ppdata").show();
                }
                else {
                    $("#pg_ppdata").hide();
                }

                if (onPaging) {
                    $(".ui-jqgrid-ftable").html($("#footer").html());
                }
                else {
                    bindTotal(level, response.Total, response.SummaryTotal);
                    $("#footer").html($(".ui-jqgrid-ftable").html());
                }

                onPaging = false;

                resizeGrid();

                $(".cell-wrapperleaf", "#ReportGrid").closest("tr").find("td").addClass("highlight");
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
            var columnModel = PlatformWinLossOption.getColModel(level, _submitData.ProductOptions);

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
            var baseCurrencyText = baseCurrency[baseCurrencyKey] === undefined
                ? viewinKey.replace("{0}", selectedCurrency)
                : baseCurrency[baseCurrencyKey];

            var platformName = platforms[data.PlatformType];
            var productText = Site.getSingleResource('Product') + ': ';
            productText = (data.ProductOptions && data.ProductOptions.length === 1) ?
                productText + data.ProductOptions[0].Code :
                getProductText(_submitData.ProductOptions);
            var title = $.format(titlePattern, _submitData.FromDate.toStringFormat("MM/dd/yyyy"), _submitData.ToDate.toStringFormat("MM/dd/yyyy"),
                        currencyText, baseCurrencyText, platformName, ', ' + productText);

            $(".report-title").html(title);
            $(".panel-heading").removeClass("hide");
        }

        function drillDown(data) {
            gridControl.level = 1;
            var productOptions;
            if (data.ProductCode) {
                productOptions = [{ Code: data.ProductCode, Status: true }];
            }
            else {
                productOptions = _submitData.ProductOptions;
            }

            _drilldownData =
            {
                Level: gridControl.level,
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
                    _cacheData[gridControl.level] = response;
                    setReportTitle(_drilldownData);
                    Report.renderBreadCrumb(gridControl.level, PlatformWinLossOption.licenseeId);
                    bindData(gridControl.level, response, productOptions);
                },
                beforeSend: Site.blockUI,
                complete: Site.unBlockUI
            });
        }

        function openBetlistPopUp(dataDrillDown) {
            var url = Site.resolveClientUrl("PlatformWinLossBetList/Index");

            url = QueryString.setParams(url, {
                fromdate: _submitData.FromDate.toStringFormat("MM/dd/yyyy"),
                todate: _submitData.ToDate.toStringFormat("MM/dd/yyyy"),
                PlatformType: _drilldownData.PlatformType,
                excludetest: _drilldownData.IsExcludeTest,
                CurrencyId: dataDrillDown.DrillDownCurrencyId,
                CurrencyName: dataDrillDown.DrillDownCurrencyName,
                drawfilter: _submitData.DrawFilter,
                custid: dataDrillDown.CustomerId,
                systemId: dataDrillDown.SystemId,
                externalId: dataDrillDown.ExternalId,
                BaseCurrency: _submitData.BaseCurrency
            });

            var count = 0;
            for (var i = 0; i < _drilldownData.ProductOptions.length; i++) {
                var product = _drilldownData.ProductOptions[i];
                if ($.inArray(product.Code, BETLIST_IGNORED_PRODUCTS)) {
                    url = Site.setProductOptionParam(url, count, product.Code, product.Status);
                    count++;
                }
            }

            var popH = Math.min($(window).height() - 100, 650), popW = 920;
            var popupOption = { id: "betlist-popup", width: popW + 50, height: popH + 50, title: "Customer Bet List" };
            Popup.openPopupWithIframe(url, popupOption);
        }

        function goBackLevel0() {
            Report.renderBreadCrumb(0, PlatformWinLossOption.licenseeId);
            bindData(0, _cacheData[0], _submitData.ProductOptions);
        }

        function sortData(arrayData) {
            var data = gridControl.sortOrder === "asc" ?
                Enumerable.From(arrayData).OrderBy(function (x) {
                    return x[gridControl.sortColumnName];
                }).ToArray()
                : Enumerable.From(arrayData).OrderByDescending(function (x) {
                    return x[gridControl.sortColumnName];
                }).ToArray();

            for (var i = 0; i < data.length; i++) {
                data[i].Index = i + 1;
            }

            return data;
        }

        function getProductText(productOptionJson) {
            var productOptions = productOptionJson;
            var productText = "";
            var selectedProductCount = 0;

            for (var i = 0; i < productOptions.length; i++) {
                if (productOptions[i].Status) {
                    selectedProductCount++;
                    productText += productText.length === 0 ? productOptions[i].Code : ", " + productOptions[i].Code;
                }
            }

            var productName = selectedProductCount === productOptions.length ? Site.getSingleResource('All') : productText;

            return Site.getSingleResource("Product") + ": " + productName;
        }

        function resizeGrid() {
            var reportGrid = $("#ReportGrid");
            jqGridAdditional.updateGridHeight();
            jqGridAdditional.updateGridWidth(reportGrid);
        }

        var _me = {
            initialize: function () {
                Site.initialize();
                Site.addUserGuide({
                    resourceName: "PlatformWinLoss",
                    title: Site.getSingleResource("PlatformWinLoss")
                });

                initEvent();
                initSetting();

                MultiSelect.initialize('grid-products-multiselect', $("#productsData").val(), 'Code', 'Name', 'Status');
                MonthPicker.initMonthPicker();

                platforms = {
                    "0": Site.getSingleResource("All"),
                    "1": Site.getSingleResource("Asia"),
                    "2": Site.getSingleResource("NewAsia"),
                    "3": Site.getSingleResource("Euro"),
                    "4": Site.getSingleResource("Wap"),
                    "5": Site.getSingleResource("SmartSite")
                };

                $(window).on("resize", function () {
                    resizeGrid();
                });

                $(window).on("unload", function () {
                    $(window).off("resize");
                });
            },
        };

        return _me;
    });