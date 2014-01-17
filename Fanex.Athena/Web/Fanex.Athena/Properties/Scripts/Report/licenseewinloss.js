/// <reference path="/@JSense.js" />
///
var LicenseeWinLoss = (function ($) {
    var cacheDataLevel0 = {};
    var cacheDataLevel1 = {};
    var cacheDataLevel2 = {};

    var ajaxRequest = null;
    var totalRowCount = 0;

    return {
        getTotalRowCount: function () {
            return totalRowCount;
        },

        initialize: function () {

            LicenseeWinLoss.initEvent();
            LicenseeWinLoss.initSetting();

        },

        initSetting: function () {
            LicenseeWinLossOption.hasCustomerComm = Site.asBool($("#IsAllowedCustomerCommission").val());
            LicenseeWinLossOption.licenseeId = parseInt($("#drillDownLicenseeId").val());
        },

        initEvent: function () {
            /* open betlist popup*/
            $("body").on("click", ".bet-list-link", function () {
                var dataDrillDown = $(this).attr("data-drill-down");
                LicenseeWinLoss.openBetlistPopUp(dataDrillDown);
            });

            $('#SubmitButton').click(function () {
                // Site.blockUI();
                $('#IsExportExcel').val(false);
                LicenseeWinLoss.submit();
            });

            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                var mainForm = $("#licenseewinloss");
                var url = Site.resolveClientUrl(LicenseeWinLossOption.urlExcelData);
                mainForm.attr("action", url);
                mainForm.submit();
                return false;
            });

            $("#level-link-container").on("click", ".level-link", function () {
                var level = $(this).attr("data-level");
                return LicenseeWinLoss.goBack(level);
            });
        },

        //#region Layout
        setExcelUrl: function (level, currencyId, custId, fdate, tdate) {
            var temp = 'api/report/LicenseeWL';
            if (level > 0) {
                temp += "?level=" + level + "&currencyId="
                + currencyId + "&custId=" + custId + "&fdate=" + fdate.toString("MM/dd/yyyy")
                 + "&tdate=" + tdate.toString("MM/dd/yyyy");
            }
            LicenseeWinLossOption.urlExcelData = temp;
        },

        renderBreadCrumb: function (level) {
            //hide history link
            if (level == 0) {
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

                $(".level2").showHide(level == 2);
                $(".level1").showHide(LicenseeWinLossOption.licenseeId == 1);
                $(".level1").toggleClass("level-link", level != 1);
            }
        },

        setReportTitle: function (currencyId, fromdate, todate) {
            var titlePattern = Site.getSingleResource('LicenseeWinLoss') + ', {0} → {1} - {2}';
            var viewinKey = Site.getSingleResource("Viewin");
            //get currencyName
            var currencyTextList =
            {
                "-2": viewinKey + " USD",
                "-1": viewinKey + " EUR", 
                "0": Site.getSingleResource('AllCurrencies')
            };

            var currencyStr = currencyTextList[currencyId] === undefined
                ? viewinKey + " " + $('#CurrencyId option[value="' + currencyId + '"]').text()
                : currencyTextList[currencyId];

            var licenseeWLNote = jqGridAdditional.getLicenseeWLNote($("#ProductOptionJson").val());
            var title = $.format(titlePattern, fromdate.toString("M/dd/yyyy"), todate.toString("M/dd/yyyy"), currencyStr);
            $("div.report-title").html(title);
        },
        //#end

        //#Get Data
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
                        
                        LicenseeWinLoss.setReportTitle($("#CurrencyId").val(), $("#FromDate").val(), $("#ToDate").val());
                        break;
                    case 1:
                        cacheDataLevel1 = response;

                        break;
                    case 2:

                        cacheDataLevel2 = response;
                        break;
                }

                LicenseeWinLoss.renderBreadCrumb(level);
                LicenseeWinLoss.bindData(level, reportBy, response);
            }
        },

        submit: function () {
            Report.setOriginalProduct();
            Site.updateOriginalLicenseeOption();

            var validProduct = Report.ValidProducts();
            if (!validProduct) {
                return false;
            }

            var mainForm = $("#licenseewinloss");
            var option = {
                type: "POST",
                data: mainForm.serialize(),
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(LicenseeWinLossOption.urlData),
                error: function (xhRequest, errorText, thrownError) { },
                success: function (response) {
                    LicenseeWinLoss.setExcelUrl(0, 0, 0, "", "");
                    LicenseeWinLoss.ajaxComplete(0, response);
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function (jqXhr, textStatus) {
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
            if (param.ReportBy == 2) {
                var fday = 1;
                var month = fdate.getMonth();
                var year = fdate.getFullYear();
                var orignalFDate = new Date(param.FromDate);
                if (month == orignalFDate.getMonth()) {
                    fday = orignalFDate.getDate();
                }
                fdate = new Date(year, month, fday, 0, 0, 0, 0);
                var originalTDate = new Date(param.ToDate);

                if (month == originalTDate.getMonth()) {
                    var tday = originalTDate.getDate();
                    tdate = new Date(year, month, tday, 0, 0, 0, 0);
                }
                else {
                    tdate = LicenseeWinLoss.lastDateOfMonth(year, month);
                }
            }
            var option = {
                type: "POST",
                data: JSON.stringify(param),
                dataType: "json",
                contentType: "application/json",
                cache: false,

                url: Site.resolveClientUrl(LicenseeWinLossOption.urlData + "?level=" + level + "&currencyId=" + currencyId + "&custId=" + customerId + "&fdate=" + fdate.toString("MM/dd/yyyy") + "&tdate=" + tdate.toString("MM/dd/yyyy")),
                error: function (xhRequest, errorText, thrownError) { },
                success: function (response) {
                    LicenseeWinLoss.ajaxComplete(level, response);
                    if (response != null && response.Data.length > 0) {
                        LicenseeWinLoss.setReportTitle(param.CurrencyId, fdate, tdate);
                        //update excel ULR
                        LicenseeWinLoss.setExcelUrl(level, currencyId, customerId, fdate, tdate);
                    }
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function (jqXhr, textStatus) {
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

            if (level == 0) {
                LicenseeWinLoss.renderBreadCrumb(level);
                LicenseeWinLoss.bindData(0, reportBy, cacheDataLevel0);
                //reset excel Link
                LicenseeWinLoss.setExcelUrl(0, 0, 0, "", "");
                //re-set title
                var fromdate = new Date($("#OriginalFromDate").val());
                var todate = new Date($("#OriginalToDate").val());
                var currencyId = $("#OriginalCurrencyId").val();
                LicenseeWinLoss.setReportTitle(currencyId, fromdate, todate);
            }
            else if (level == 1) {
                LicenseeWinLoss.bindData(1, reportBy, cacheDataLevel1);
                LicenseeWinLossOption.urlExcelData = LicenseeWinLossOption.urlExcelData.replace("level=2", "level=1");
                LicenseeWinLoss.renderBreadCrumb(level);
            }
        },

        //#BindReport

        bindTotal: function (level, totalData, summaryTotal) {
            var productOptions = $.parseJSON($("#ProductOptionJson").val());
            productOptions = productOptions.reverse(); //make orders of product columns like excel file
            var columnModel = LicenseeWinLossOption.getTotalColModel(level, productOptions);
            var colspan = LicenseeWinLossOption.totalColspan[level];
            var summaryColspan = colspan + 1;

            jqGridAdditional.assignTotal(totalData, columnModel);
            var summaryRowCount = Report.bindSummaryTotal(columnModel, summaryTotal);
            jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount);
        },

        bindData: function (level, reportBy, response) {
            jqGridAdditional.clearGrid("ReportGrid");

            var reportGrid = $("#ReportGrid");

            var productOptions = $.parseJSON($("#ProductOptionJson").val());

            productOptions = productOptions.reverse(); //make orders of product columns like excel file

            var numOfRow = response.Data.length;

            var numOfTotalRow = response.Total.length;
            totalRowCount = numOfTotalRow;

            var onPaging = false;

            var gridOption = LicenseeWinLossOption.getGridOption(level, reportBy, productOptions);

            gridOption.data = response.Data;
            gridOption.userData = {};
            gridOption.rowNum = level == 2 ? LicenseeWinLossOption.pageSize : LicenseeWinLossOption.nonePagingSize;
            gridOption.rowList = LicenseeWinLossOption.rowList;
            gridOption.rowTotal = numOfRow;
            gridOption.pager = '#ppdata';
            gridOption.onPaging = function (pgButton) {

                if (level == 2 && numOfRow > LicenseeWinLossOption.pageSize) {
                    if ($("#" + pgButton).hasClass("ui-state-disabled") == false) {
                        Site.blockUI();
                    }
                    onPaging = true;
                }
                else {
                    return 'stop';
                }
            };
            gridOption.gridComplete = function () {
                jqGridAdditional.gridComplete(this, gridOption, numOfTotalRow + 1);

                if (level == 2 && numOfRow > LicenseeWinLossOption.pageSize) {
                    $("#pg_ppdata").show();
                }
                else {
                    $("#pg_ppdata").hide();
                }

                if (response == null || numOfRow == 0) {
                    jqGridAdditional.setNoInfo(gridOption.colModel);
                }
                else {
                    if (onPaging && level == 2) {
                        $(".ui-jqgrid-ftable").html($("#footer").html());
                    }
                    else {
                        LicenseeWinLoss.bindTotal(level, response.Total, response.SummaryTotal);
                        $("#footer").html($(".ui-jqgrid-ftable").html());
                    }

                    onPaging = false;
                }
                /*group header*/
                LicenseeWinLossOption.addProductButton(reportGrid, "jqgh_ReportGrid", "ReportGrid", productOptions);

                Site.unBlockUI();
            };

            // Show the grid
            reportGrid.jqGrid(gridOption);

            LicenseeWinLossOption.setGroupHeaderColumns(reportGrid, productOptions);

            jqGridAdditional.showRecommendedBrowser(gridOption.pager);

            jqGridAdditional.resetLayout(reportGrid);
        },

        //#end

        openBetlistPopUp: function (dataDrillDown) {
            dataDrillDown = $.parseJSON(dataDrillDown);
            dataDrillDown.DrawFilter = $("#OriginalDrawFilter").val();
            dataDrillDown.IsExcludedTestData = $("#OriginalIsExcludedTestData").val() == 'True';
            dataDrillDown.CurrencyId = $("#OriginalCurrencyId").val();
            dataDrillDown.CurrencyName = $("#OriginalCurrencyName").val();
            dataDrillDown.BaseCurrency = $("#BaseCurrency").attr("data-original")


            if (typeof dataDrillDown.DrillDownFromDate == 'string') {
                dataDrillDown.DrillDownFromDate = new Date(Date.parse(dataDrillDown.DrillDownFromDate));
                dataDrillDown.DrillDownToDate = new Date(Date.parse(dataDrillDown.DrillDownToDate));
            }
            var reportBy = $("#OriginalReportBy").val();
            if (reportBy == 2) {

                var fday = 1;
                var month = dataDrillDown.DrillDownFromDate.getMonth();
                var year = dataDrillDown.DrillDownFromDate.getFullYear();

                var orignalFDate = new Date($("#OriginalFromDate").val());
                var originalTDate = new Date($("#OriginalToDate").val());

                if (month == orignalFDate.getMonth()) {
                    fday = orignalFDate.getDate();
                }
                dataDrillDown.DrillDownFromDate = new Date(year, month, fday, 0, 0, 0, 0);
                if (month == originalTDate.getMonth()) {
                    var tday = originalTDate.getDate();
                    dataDrillDown.DrillDownToDate = new Date(year, month, tday, 0, 0, 0, 0);
                }
                else {
                    dataDrillDown.DrillDownToDate = LicenseeWinLoss.lastDateOfMonth(year, month);
                }
            }

            var currencyName = $.trim($("#CurrencyId option[value=" + dataDrillDown.DrillDownCurrencyId + "]").text());

            var url = Site.resolveClientUrl("BetList/LicenseeWl");
            url = QueryString.setParam(url, "fromdate", dataDrillDown.DrillDownFromDate.toString("MM/dd/yyyy"));
            url = QueryString.setParam(url, "todate", dataDrillDown.DrillDownToDate.toString("MM/dd/yyyy"));
            url = QueryString.setParam(url, "excludetest", dataDrillDown.IsExcludedTestData);
            url = QueryString.setParam(url, "CurrencyId", dataDrillDown.DrillDownCurrencyId);
            url = QueryString.setParam(url, "CurrencyName", currencyName);
            url = QueryString.setParam(url, "drawfilter", dataDrillDown.DrawFilter);
            url = QueryString.setParam(url, "custid", dataDrillDown.CustomerId);
            url = QueryString.setParam(url, "systemId", dataDrillDown.SystemId);
            url = QueryString.setParam(url, "externalId", dataDrillDown.ExternalId);
            url = QueryString.setParam(url, "BaseCurrency", dataDrillDown.BaseCurrency);
            url = QueryString.setParam(url, "OriginalCurrenceId", dataDrillDown.CurrencyId);
            url = QueryString.setParam(url, "OriginalCurrencyName", dataDrillDown.CurrencyName);
            
            url = Site.setProductOptionOriginalParamList(url);

            var popH = 650, popW = 920;
            var dialogForm = '<div title="Customer Bet List"><iframe frameborder="0" width="' + popW + '" height="100%" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: popW + 50,
                height: popH + 50
            });
        }
    };
})(jQuery);

$(window).load(function () {
    LicenseeWinLoss.initialize();
    //alert(Date.parse('Apr 5, 2013'));
});

$(window).resize(function () {
    var reportGrid = $("#ReportGrid");
    jqGridAdditional.resetLayout(reportGrid);
});

$(window).unload(function () {
    $(window).unbind("resize");
});
