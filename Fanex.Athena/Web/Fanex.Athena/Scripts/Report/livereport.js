/*global Site, QueryString*/
/// <reference path="/@JSense.js" />
var LiveReport = (function ($) {
    "use strict";
    return {
        initialize: function () {
            LiveReport.initEvent();
            LiveReport.initMonthPicker();
            Site.addUserGuide({
                resourceName: "LiveReport",
                title: Site.getSingleResource("LiveReport")
            });
        },
        initEvent: function () {
            $("#switchreport").click(function () {
                var mod = $(this).attr("data-history-mode");
                if (mod === "False") {
                    Site.redirect(Site.resolveClientUrl("LiveReport?isViewedHistory=True"));
                }
                else {
                    Site.redirect(Site.resolveClientUrl("LiveReport?isViewedHistory=False"));
                }
            });

            $("#exporttoexcel").click(function(e){
                e.preventDefault();
                $('#IsExportExcel').val(true);
                $("#livereport").submit();
                $('#IsExportExcel').val(false);
            });

            /**************/
            /*date-range*/

            /*drill down*/
            /* sumary detail*/
            $("body").on("click", ".product-detail-link", function () {
                LiveReport.viewProductDetail(this);
            });
            /* sumary detail*/
            /* detail*/
            $("body").on("click", ".sport-detailt-link", function () {
                LiveReport.viewSportDetail(this);
            });            
        },
        initMonthPicker: function () {
            /* date-range*/
            var fromdate = new Date($('#FromDate').val());
            var todate = new Date($('#ToDate').val());

            $('#month-picker').monPickr({
                minDate: Site.getHistoryDate(),
                showDateRange: true,
                fromDayId: 'fDay',
                toDayId: 'tDay',
                format: 'MM/dd/yyyy',
                month: fromdate,
                fromDay: fromdate.getDate(),
                toDay: todate.getDate(),
                afterChangeMonth: function () {
                    $('#FromDate').val($("#month-picker").monPickr("getFromDate", "MM/dd/yyyy"));
                    $('#ToDate').val($("#month-picker").monPickr("getToDate", "MM/dd/yyyy"));
                },
                afterChangeDay: function () {
                    $('#FromDate').val($("#month-picker").monPickr("getFromDate", "MM/dd/yyyy"));
                    $('#ToDate').val($("#month-picker").monPickr("getToDate", "MM/dd/yyyy"));
                }
            });
        },

        viewProductDetail: function (target) {
            var productCode = $(target).attr("data-product-code");
            var data = {
                productCode: productCode,
                FromDate: $("#FromDate").attr('data-original').toString("MM/dd/yyyy"),
                ToDate: $("#ToDate").attr('data-original').toString("MM/dd/yyyy"),
                BaseCurrency: $("#BaseCurrency").attr('data-original'),
                LiveIndicator: $("#LiveIndicator").attr('data-original')
            };
            data = Site.setLicenseeOptionOriginalParam(data);
            /*selected*/
            var isSelected = $(target).parent().hasClass("selected");
            if (isSelected) { return; }
            /* selected*/

            $("a.product-detail-link").parent().removeClass("selected");
            $(target).parent().addClass("selected");

            /* make request*/
            $.ajax({
                type: "POST",
                data: data,
                dataType: "json",
                url: Site.resolveClientUrl("LiveReport/Detail"),
                success: function () {
                },
                beforeSend: function () {
                    LiveReport.ajaxStart();
                },
                complete: function (jqXHR) {
                    if (jqXHR.readyState === 4) {
                        $("#ProductCode").val(data.productCode);
                        $("#detail-content").html(jqXHR.responseText);
                    }
                    LiveReport.ajaxComplete();
                }
            });
        },

        setSumParams: function (url, totalValue) {
            url = QueryString.setParam(url, "totalrows", totalValue.BetCount);
            url = QueryString.setParam(url, "sumwinlost", totalValue.CustomerWinLoss);
            url = QueryString.setParam(url, "sumplayercomm", totalValue.CustomerComm);
            url = QueryString.setParam(url, "sumlicenseewl", totalValue.LicenseeWinLoss);
            url = QueryString.setParam(url, "sumcomm", totalValue.LicenseeComm);
            url = QueryString.setParam(url, "sumturnover", totalValue.TurnOver);
            url = QueryString.setParam(url, "sumamtdueto3rd", totalValue.AmountDue);

            return url;
        },

        viewBetList: function (url, totalValue, productOptions) {
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "fromdate", $("#FromDate").attr('data-original'));
            url = QueryString.setParam(url, "todate", $("#ToDate").attr('data-original'));
            url = QueryString.setParam(url, "LiveIndicator", $("#LiveIndicator").attr('data-original'));
            url = LiveReport.setSumParams(url, totalValue);
            url = Site.setProductCodeListParam(url, productOptions);
            Site.blockUI();
            Site.redirect(url);
        },

        getMatchListProductOption: function (sportId, productCode) {
            sportId = Number(sportId);
            var productOptions = [];
            if (sportId > 0) {
                if (sportId === 180 || sportId === 186) {
                    productOptions.push("VS");
                }
                else if (sportId === 162) {
                    productOptions.push("LC");
                }
                else if (sportId === 190 || sportId === 191 || sportId === 104) {
                    productOptions.push("VS2");
                }
                else {
                    if (productCode === "SB" || productCode === "ALL") {
                        productOptions.push("SB");
                    }
                    if (productCode === "BA" || productCode === "ALL") {
                        productOptions.push("BA");
                    }
                }
            }
            else {
                if (productCode === "ALL") {
                    productOptions = ["SB", "BA", "NG", "RB", "LC", "VS", "CS", "BG", "VS2"];
                }
                else {
                    productOptions.push(productCode);
                }
            }

            return productOptions;
        },

        viewSportDetail: function (target) {
            var data = $(target).attr("data-drill-down");
            var jsonData = $.parseJSON(data);
            var productCode = $("#ProductCode").val();
            // after users back from match list, 
            // the report does not show detailed data, 
            // so remove product code to not show detailed data in EXPORT EXCEL
            $("#ProductCode").val("");
            var productOptions = LiveReport.getMatchListProductOption(jsonData.SportId, productCode);
            var urlMap = {
                Key101: "LiveReportBetList/MixParlay?bettype=9",
                Key102: "LiveReportBetList/MixParlay?bettype=29",
                Key103: "LiveReportBetList/Outright",
                Key161: "LiveReportBetList/NumberGame",
                Key151: "LiveReportBetList/Racing?SportType=151",
                Key152: "LiveReportBetList/Racing?SportType=152",
                Key153: "LiveReportBetList/Racing?SportType=153",
                Key181: "LiveReportBetList/Racing?SportType=181",
                Key182: "LiveReportBetList/Racing?SportType=182",
                Key183: "LiveReportBetList/Racing?SportType=183",
                Key184: "LiveReportBetList/Racing?SportType=184",
                Key185: "LiveReportBetList/Racing?SportType=185",
                Key104: "LiveReportBetList/MixParlay?bettype=2799"
            };

            var url = urlMap["Key" + jsonData.SportId];
            
            if (url) {
                LiveReport.viewBetList(Site.resolveClientUrl(url), jsonData.TotalValue, productOptions);
            }
            else {
                url = Site.resolveClientUrl("LiveReport/MatchList");
                // Match List
                url = Site.setLicenseeOptionOriginalParam(url);
                url = QueryString.setParam(url, "SportId", jsonData.SportId);
                url = QueryString.setParam(url, "ProductCode", productCode);
                url = QueryString.setParam(url, "FromDate", $("#FromDate").attr('data-original'));
                url = QueryString.setParam(url, "ToDate", $("#ToDate").attr('data-original'));
                url = QueryString.setParam(url, "LiveIndicator", $("#LiveIndicator").attr('data-original'));

                url = Site.setProductCodeListParam(url, productOptions);
                Site.blockUI();
                Site.redirect(url);
            }
        },
        ajaxStart: function () {
            Site.blockUI();
        },
        ajaxComplete: function () {
            Site.unBlockUI();
        }
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    LiveReport.initialize();
});