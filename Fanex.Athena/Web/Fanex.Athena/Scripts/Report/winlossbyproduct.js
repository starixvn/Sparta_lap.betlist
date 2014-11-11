/*global Site, Report, QueryString, jqGridAdditional, WinLossByProductOption*/
/// <reference path="/@JSense.js" />

var WinLossByProduct = (function ($) {
    "use strict";
    var cacheDataLevel0 = {};
    var cacheDataLevel1 = {};
    var cacheDataLevel2 = {};
    var ajaxRequest = null;
    return {
        resizeGrid: function () {
            setTimeout(function () {
                var gridTop = $(".ui-jqgrid-bdiv").offset().top;
                var gridHeight = window.innerHeight - gridTop - $(".ui-jqgrid-sdiv").height() - $(".ui-jqgrid-pager:visible").height() - 30;
                gridHeight = Math.max(130, gridHeight);
                $(".ui-jqgrid-bdiv").css("height", "auto").css("max-height", gridHeight);
            }, 100);
        },
        //#region Init
        initialize: function () {
            WinLossByProduct.initEvent();
            WinLossByProduct.initSetting();

            Site.addFilterCollapseExpand();
            Site.addUserGuide({
                resourceName: "WinLossByProduct",
                title: Site.getSingleResource("WinLossByProduct")
            });
        },

        initSetting: function () {
            WinLossByProductOption.hasCustomerComm = Site.asBool($("#IsAllowedCustomerCommission").val());
            WinLossByProductOption.IsAllowBA = Site.asBool($("#IsAllowedBACommission").val());
            WinLossByProductOption.IsAllowRB = Site.asBool($("#IsAllowedRacing").val());
            WinLossByProductOption.licenseeId = parseInt($("#drillDownLicenseeId").val());
        },

        initEvent: function () {
            /// <summary>
            /// Inititalize events
            /// </summary>
            $("body").on("click", ".bet-list-link", function () {
                var dataDrillDown = $(this).attr("data-drill-down");
                WinLossByProduct.openBetlistPopUp(dataDrillDown);
            });

            $('#SubmitButton').click(function () {
                // Site.blockUI();
                $('#IsExportExcel').val(false);
                WinLossByProduct.submit();
            });

            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                var mainForm = $("#winlossbyproduct");
                var url = Site.resolveClientUrl(WinLossByProductOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                return false;
            });

            $("#level-link-container").on("click", ".level-link", function () {
                var level = $(this).attr("data-level");
                return WinLossByProduct.goBack(level);
            });
        },

        //#endregion

        //#region Utilities

        setExcelUrl: function (level, currencyId, custId, fdate, tdate) {
            var temp = 'api/report/WinLostByProduct';
            if (level > 0) {
                temp += "?level=" + level + "&currencyId=" + currencyId + "&custId=" + custId + "&fdate=" +
                fdate.toString("MM/dd/yyyy") + "&tdate=" + tdate.toString("MM/dd/yyyy");
            }
            WinLossByProductOption.urlExcelData = temp;
        },

        renderBreadCrumb: function (level) {
            level = Number(level);
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

                $(".level2").showHide(level === 2);
                $(".level1").showHide(WinLossByProductOption.licenseeId === 1);
                $(".level1").toggleClass("level-link", level !== 1);
            }
        },

        setReportTitle: function (currencyId, fromdate, todate) {
            var titlePattern = Site.getSingleResource('WinLossByProduct') + ', {0} → {1} - {2}{3}';
            var viewinKey = Site.getSingleResource("Viewin");

            //get currencyName
            var currencyTextList =
            {
                "-2": viewinKey.replace("{0}", "USD"),
                "-1": viewinKey.replace("{0}", "EUR"),
                "0": Site.getSingleResource('AllCurrencies')
            };

            var currencyStr = currencyTextList[currencyId] === undefined
                ? viewinKey.replace("{0}", $('#CurrencyId option[value="' + currencyId + '"]').text())
                : currencyTextList[currencyId];

            var licenseeWLNote = jqGridAdditional.getLicenseeWLNote($("#ProductOptionJson").val());
            var title = $.format(titlePattern, fromdate.toString("M/dd/yyyy"), todate.toString("M/dd/yyyy"),
           currencyStr, (licenseeWLNote.trim() === '' ? '' : ', ' + licenseeWLNote));
            $("div.report-title").html(title);
        },

        lastDateOfMonth: function (year, month) {
            return new Date(year, month + 1, 0);
        },

        //#endregion

        //#region Get Data

        ajaxComplete: function (level, response) {
            if (response != null) {
                var reportBy = $("input[name='reportby']:checked").val();
                //reset layout
                switch (level) {
                    case 0:
                        cacheDataLevel0 = response;
                        //update productOption
                        $("#ProductOptionJson").val(response.ProductOptions);
                        //update drildownValue
                        $("#OriginalDrawFilter").val($("input[name='drawfilter']:checked").val());
                        $("#OriginalIsExcludedTestData").val($("#IsExcludedTestData").is(":checked"));
                        $("#OriginalCurrencyId").val($("#CurrencyId").val());
                        $("#OriginalReportBy").val(reportBy);
                        $("#OriginalFromDate").val($("#FromDate").val());
                        $("#OriginalToDate").val($("#ToDate").val());
                        $("#OriginalCurrencyName").val($("#CurrencyId option:selected").text());
                        WinLossByProduct.setReportTitle($("#CurrencyId").val(), $("#FromDate").val(), $("#ToDate").val());
                        break;
                    case 1:
                        cacheDataLevel1 = response;

                        break;
                    case 2:

                        cacheDataLevel2 = response;
                        break;
                }

                WinLossByProduct.renderBreadCrumb(level);
                WinLossByProduct.bindData(level, reportBy, response);
            }
        },

        submit: function () {
            Report.setOriginalProduct();

            var validProduct = Report.ValidProducts();
            if (!validProduct) {
                return false;
            }

            Site.updateOriginalLicenseeOption();
            var mainForm = $("#winlossbyproduct");
            var option =
            {
                type: "POST",
                data: mainForm.serialize(),
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(WinLossByProductOption.urlData),
                success: function (response) {
                    WinLossByProduct.setExcelUrl(0, 0, 0, "", "");
                    WinLossByProduct.ajaxComplete(0, response);
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

        //#endregion

        //#region drillDown

        drillDown: function (level, currencyId, customerId, datetime) {
            var productOptions = $.parseJSON($("#ProductOptionJson").val());
            var param =
            {
                ProductOptions: productOptions,
                DrawFilter: $("#OriginalDrawFilter").val(),
                IsExcludedTestData: $("#OriginalIsExcludedTestData").val(),
                CurrencyId: $("#OriginalCurrencyId").val(),
                ReportBy: $("#OriginalReportBy").val(),
                FromDate: $("#OriginalFromDate").val(),
                ToDate: $("#OriginalToDate").val(),
                BaseCurrency: $("#BaseCurrency").attr("data-original")
            };
            var fdate = new Date(Date.parse(datetime));
            var tdate = new Date(Date.parse(datetime));
            if (param.ReportBy === '2') {
                var fday = 1;
                var month = fdate.getMonth();
                var year = fdate.getFullYear();
                var orignalFDate = new Date(param.FromDate);
                if (month === orignalFDate.getMonth()) {
                    fday = orignalFDate.getDate();
                }
                fdate = new Date(year, month, fday, 0, 0, 0, 0);
                var originalTDate = new Date(param.ToDate);

                if (month === originalTDate.getMonth()) {
                    var tday = originalTDate.getDate();
                    tdate = new Date(year, month, tday, 0, 0, 0, 0);
                }
                else {
                    tdate = WinLossByProduct.lastDateOfMonth(year, month);
                }
            }
            var option =
            {
                type: "POST",
                data: JSON.stringify(param),
                dataType: "json",
                contentType: "application/json",
                cache: false,

                url: Site.resolveClientUrl(WinLossByProductOption.urlData + "?level=" + level + "&currencyId=" + currencyId + "&custId=" + customerId +
                     "&fdate=" + fdate.toString("MM/dd/yyyy") + "&tdate=" + tdate.toString("MM/dd/yyyy")),
                success: function (response) {
                    WinLossByProduct.ajaxComplete(level, response);
                    if (response != null && response.Data.length > 0) {
                        if (level > 0 && param.CurrencyId === '0') {
                            WinLossByProduct.setReportTitle(currencyId, fdate, tdate);
                        } else {
                            WinLossByProduct.setReportTitle(param.CurrencyId, fdate, tdate);
                        }
                        //update excel ULR
                        WinLossByProduct.setExcelUrl(level, currencyId, customerId, fdate, tdate);
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
        },

        goBack: function (level) {
            var reportBy = $("#OriginalReportBy").val();
            level = Number(level);
            if (level === 0) {
                WinLossByProduct.renderBreadCrumb(level);
                WinLossByProduct.bindData(0, reportBy, cacheDataLevel0);
                //reset excel Link
                WinLossByProduct.setExcelUrl(0, 0, 0, "", "");
                //re-set title
                var fromdate = new Date($("#OriginalFromDate").val());
                var todate = new Date($("#OriginalToDate").val());
                var currencyId = $("#OriginalCurrencyId").val();
                WinLossByProduct.setReportTitle(currencyId, fromdate, todate);
            }
            else if (level === 1) {
                WinLossByProduct.bindData(1, reportBy, cacheDataLevel1);
                WinLossByProductOption.urlExcelData = WinLossByProductOption.urlExcelData.replace("level=2", "level=1");
                WinLossByProduct.renderBreadCrumb(level);
            }
        },

        //#endregion

        //#region Bind Data
        bindSummaryTotal: function (columnModel, summaryTotal) {
            /// <summary>
            /// Bind summary total data to footer rows
            /// </summary>
            /// <param name="columnModel"></param>
            /// <param name="summaryTotal"></param>
            /// <returns type="int">Number of iserted row</returns>
            if (summaryTotal === undefined || summaryTotal.length === 0) {
                return;
            }

            var mainFooter = $(".ui-jqgrid-ftable tr:last-child");
            var rowCount = 0;
            var currencyList = [];

            function reverseArray(temp) {
                /// reverse array before bind to browser (no change sorting)
                $(temp.reverse()).each(function (totalIndex, totalElement) {
                    var row = mainFooter.clone();
                    row.addClass("total-eur total-eur-none-top-se");
                    var cellList = $("td", row);
                    var firstCell = null;
                    $(columnModel).each(function (cellIndex, modelElement) {
                        var cell = cellList.get(cellIndex);
                        var isCustomFormater = typeof (modelElement.formatter) === "function";
                        var rawValue = totalElement[modelElement.name];
                        var value = isCustomFormater ? modelElement.formatter(rawValue, modelElement.formatoptions, row) :
                                $.fn.fmatter(modelElement.formatter, rawValue, modelElement.formatoptions);
                        var titleValue = value;
                        if ($.isNumeric(rawValue) && rawValue < 0) {
                            var wrapper = document.createElement('div');
                            wrapper.innerHTML = value;
                            titleValue = wrapper.outerHtml;
                        }

                        $(cell).html(value).attr("title", titleValue);
                        firstCell = !firstCell ? $(cell) : firstCell;
                    });

                    firstCell.html("Total in " + currency);
                    firstCell.addClass("group-" + currency);
                    row.insertAfter(mainFooter);
                    rowCount++;
                });
            }

            for (var currency in summaryTotal) {
                currencyList.push(currency);
                var summaryData = summaryTotal[currency];
                var temp = Site.clone(summaryData);
                reverseArray(temp);
            }

            $(".ui-jqgrid-ftable").attr("data-total-currency-list", currencyList.join());
            mainFooter.remove();

            return rowCount;
        },

        bindTotal: function (level, totalData, summaryTotal) {
            var productOptions = $.parseJSON($("#ProductOptionJson").val());
            var columnModel = WinLossByProductOption.getTotalColModel(level, productOptions);
            var colspan = WinLossByProductOption.totalColspan[level];
            var summaryColspan = colspan + 1;
            jqGridAdditional.assignTotal(totalData, columnModel);
            var summaryRowCount = WinLossByProduct.bindSummaryTotal(columnModel, summaryTotal);
            jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount, true);
        },

        bindData: function (level, reportBy, response) {
            level = Number(level);
            jqGridAdditional.clearGrid("ReportGrid");
            var reportGrid = $("#ReportGrid");
            // var productOptions = $.parseJSON($("#ProductOptionJson").val());

            var numOfRow = response.Data.length;
            var numOfTotalRow = response.Total.length;
            var onPaging = false;
            var gridOption = WinLossByProductOption.getGridOption(level, reportBy);
            gridOption.data = response.Data;
            gridOption.userData = {};
            gridOption.rowNum = level === 2 ? WinLossByProductOption.pageSize : WinLossByProductOption.nonePagingSize;
            gridOption.rowList = WinLossByProductOption.rowList;
            gridOption.rowTotal = numOfRow;
            gridOption.pager = '#ppdata';
            gridOption.onPaging = function (pgButton) {
                if (level === 2 && numOfRow > WinLossByProductOption.pageSize) {
                    if ($("#" + pgButton).hasClass("ui-state-disabled") === false) {
                        Site.blockUI();
                    }
                    onPaging = true;
                }
                else {
                    return 'stop';
                }
            };
            gridOption.gridComplete = function () {
                jqGridAdditional.gridComplete(this, gridOption, numOfTotalRow);
                switch (level) {
                    case 0:
                        WinLossByProductOption.setGrouping();
                        break;
                    case 1:

                        reportGrid.tableRowSpan([0, WinLossByProductOption.systemIdIndex], WinLossByProductOption.currencyIndex);
                        break;
                    case 2:
                        reportGrid.tableRowSpan([0, WinLossByProductOption.externalIdIndex, WinLossByProductOption.systemIdIndex],
                                             WinLossByProductOption.currencyIndex);

                        break;
                }
                if (level === 2 && numOfRow > WinLossByProductOption.pageSize) {
                    $("#pg_ppdata").show();
                }
                else {
                    $("#pg_ppdata").hide();
                }

                if (response == null || numOfRow === 0) {
                    jqGridAdditional.setNoInfo(gridOption.colModel);
                }
                else {
                    if (onPaging && level === 2) {
                        $(".ui-jqgrid-ftable").html($("#footer").html());
                    }
                    else {
                        WinLossByProduct.bindTotal(level, response.Total, response.SummaryTotal);
                        $("#footer").html($(".ui-jqgrid-ftable").html());
                    }

                    onPaging = false;
                }

                Site.unBlockUI();
            };

            // Show the grid

            reportGrid.jqGrid(gridOption);
            if (WinLossByProductOption.hasCustomerComm) {
                jqGridAdditional.setGroupCustomerWLColumns(reportGrid);
            }
            //set rowspan for footer row
            $("table.ui-jqgrid-ftable").tableRowSpan([WinLossByProductOption.currencyIndex]);
            if (level === 2) {
                jqGridAdditional.showRecommendedBrowser(gridOption.pager);
            }
            ///reset layout
            jqGridAdditional.resetLayout(reportGrid);
            WinLossByProduct.resizeGrid();
        },

        //#endregion

        //#region Betlist

        openBetlistPopUp: function (dataDrillDown) {
            dataDrillDown = $.parseJSON(dataDrillDown);
            dataDrillDown.DrawFilter = $("#OriginalDrawFilter").val();
            dataDrillDown.IsExcludedTestData = $("#OriginalIsExcludedTestData").val() === 'True';
            dataDrillDown.CurrencyId = $("#OriginalCurrencyId").val();
            dataDrillDown.CurrencyName = $("#OriginalCurrencyName").val();
            if (typeof dataDrillDown.DrillDownFromDate === 'string') {
                dataDrillDown.DrillDownFromDate = new Date(Date.parse(dataDrillDown.DrillDownFromDate));
                dataDrillDown.DrillDownToDate = new Date(Date.parse(dataDrillDown.DrillDownToDate));
            }
            var reportBy = Number($("#OriginalReportBy").val());
            if (reportBy === 2) {
                var fday = 1;
                var month = dataDrillDown.DrillDownFromDate.getMonth();
                var year = dataDrillDown.DrillDownFromDate.getFullYear();

                var orignalFDate = new Date($("#OriginalFromDate").val());
                var originalTDate = new Date($("#OriginalToDate").val());

                if (month === orignalFDate.getMonth()) {
                    fday = orignalFDate.getDate();
                }
                dataDrillDown.DrillDownFromDate = new Date(year, month, fday, 0, 0, 0, 0);
                if (month === originalTDate.getMonth()) {
                    var tday = originalTDate.getDate();
                    dataDrillDown.DrillDownToDate = new Date(year, month, tday, 0, 0, 0, 0);
                }
                else {
                    dataDrillDown.DrillDownToDate = WinLossByProduct.lastDateOfMonth(year, month);
                }
            }

            var currencyName = $.trim($("#CurrencyId option[value=" + dataDrillDown.DrillDownCurrencyId + "]").text());

            var url = Site.resolveClientUrl("BetList/LicenseeWl");
            url = Site.setProductOptionOriginalParamList(url);
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "fromdate", dataDrillDown.DrillDownFromDate.toString("MM/dd/yyyy"));
            url = QueryString.setParam(url, "todate", dataDrillDown.DrillDownToDate.toString("MM/dd/yyyy"));
            url = QueryString.setParam(url, "excludetest", dataDrillDown.IsExcludedTestData);
            url = QueryString.setParam(url, "CurrencyId", dataDrillDown.DrillDownCurrencyId);
            url = QueryString.setParam(url, "CurrencyName", currencyName);
            url = QueryString.setParam(url, "drawfilter", dataDrillDown.DrawFilter);
            url = QueryString.setParam(url, "custid", dataDrillDown.CustomerId);
            url = QueryString.setParam(url, "systemId", dataDrillDown.SystemId);
            url = QueryString.setParam(url, "externalId", dataDrillDown.ExternalId);
            url = QueryString.setParam(url, "OriginalCurrenceId", dataDrillDown.CurrencyId);
            url = QueryString.setParam(url, "OriginalCurrencyName", dataDrillDown.CurrencyName);

            var popH = Math.min($(window).height() - 100, 500), popW = 920;
            var dialogForm = '<div title="Customer Bet List"><iframe scrolling="no" frameborder="0" width="' + popW +
                         '" height="100%" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: popW + 30,
                height: popH + 50
            });
        }

        //#endregion
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    WinLossByProduct.initialize();
});

$(window).resize(function () {
    "use strict";
    var reportGrid = $("#ReportGrid");
    jqGridAdditional.resetLayout(reportGrid);
    WinLossByProduct.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});