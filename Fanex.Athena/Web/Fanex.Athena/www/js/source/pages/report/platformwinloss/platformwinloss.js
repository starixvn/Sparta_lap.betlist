define([
    "jquery",
    "common/querystring",
    "common/site",
    "../report",
    "./platformwinloss.option",
    "lib/jqgrid/jquery.jqgrid.additional",
    "components/pluginhelper/monthpicker",
    "components/pluginhelper/multiselect",
    "lib/jqgrid/jquery.jqGrid.en",
    "lib/jqgrid/jquery.jqGrid.min",
    "extensions/jquery.extension"],
    function ($, QueryString, Site, Report, PlatformWinLossOption, jqGridAdditional, MonthPicker, MultiSelect) {
        "use strict";

        var cacheData = {};
        var _submitData = {}; // cache submit data of level 0
        var _level = 0;
        var platforms;
        var _pageSize = 0;

        function submit() {
            var validProduct = Report.ValidProducts();
            if (!validProduct) {
                return false;
            }

            var data = $("#platformwinloss").serializeObject();
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
                    cacheData = response;
                    setReportTitle(_submitData);
                    renderBreadCrumb(_level);
                    bindData(_level, response, $.parseJSON(_submitData.ProductOptions));
                },
                beforeSend: Site.blockUI,
                complete: Site.unBlockUI
            });

            return true;
        };

        function bindData(level, response, productOptions) {
            jqGridAdditional.clearGrid("ReportGrid");
            var reportGrid = $("#ReportGrid");
            var gridOption = PlatformWinLossOption.getGridOption(level, productOptions);
            var numOfRow = response.Data.length;
            var onPaging = false;
            gridOption.datastr = response.Data;
            gridOption.rowList = PlatformWinLossOption.rowList;
            gridOption.rowNum = _pageSize;
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
                jqGridAdditional.gridComplete(this, gridOption);
                if (numOfRow > PlatformWinLossOption.pageSize) {
                    _pageSize = Number($("#ReportGrid").getGridParam("rowNum"));
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
                        bindTotal(level, response.Total, response.SummaryTotal);
                        $("#footer").html($(".ui-jqgrid-ftable").html());
                    }

                    onPaging = false;
                }

                resizeGrid();
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
        };

        function bindTotal() { };

        function renderBreadCrumb() { };

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
        };

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
            var title = $.format(titlePattern, _submitData.FromDate.toString("M/d/yyyy"), _submitData.ToDate.toString("M/d/yyyy"),
                        currencyText, baseCurrencyText, platformName, ', ' + productText);

            $(".report-title").html(title);
            $(".panel-heading").removeClass("hide");
        };
        function initEvent () {
            $('#SubmitButton').click(function () {
                //_removeSorting = true;
                submit();
            });
        };

        function initSetting() {
            _pageSize = PlatformWinLossOption.pageSize;
            PlatformWinLossOption.hasCustomerComm = Site.asBool($("#IsAllowedCustomerCommission").val());
        };

        function resizeGrid() {
            var reportGrid = $("#ReportGrid");
            jqGridAdditional.updateGridHeight();
            jqGridAdditional.updateGridWidth(reportGrid);
        };

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

            
        }

        return _me;
    });