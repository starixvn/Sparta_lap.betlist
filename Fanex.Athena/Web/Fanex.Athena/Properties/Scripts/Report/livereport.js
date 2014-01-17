/// <reference path="/@JSense.js" />
var LiveReport = (function ($) {
    return {
        initialize: function () {
            LiveReport.initEvent();
            LiveReport.initMonthPicker();
        },
        initEvent: function () {

            $("#switchreport").click(function () {
                var mod = $(this).attr("data-history-mode");
                if (mod == "False") {
                    location.href = Site.resolveClientUrl("Report/LiveReport?isViewedHistory=True");
                }
                else {
                    location.href = Site.resolveClientUrl("Report/LiveReport?isViewedHistory=False");
                }
            });


            /*back button*/
            /*-affect the action excel*/
            window.onbeforeunload = function () {
                $("ProductCode").val("");
            }
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
            /* detail*/
            /*drill down*/
        },
        initMonthPicker: function () {
            /* date-range*/
            var fromdate = new Date($('#FromDate').val());
            var todate = new Date($('#ToDate').val());

            $('#month-picker').monPickr({
                minDate: '2010/1/1',
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
                BaseCurrency: $("#BaseCurrency").attr('data-original')
            };
            data = Site.setLicenseeOptionOriginalParam(data);
            /*selected*/
            var isSelected = $(target).parent().hasClass("selected");
            if (isSelected) return;
            /* selected*/

            $("a.product-detail-link").parent().removeClass("selected");
            $(target).parent().addClass("selected");


            /* make request*/
            $.ajax({
                type: "POST",
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                url: Site.resolveClientUrl("Report/LiveReportDetail"),
                error: function (xhRequest, ErrorText, thrownError) {
                    LiveReport.ajaxComplete();
                },
                success: function (response) {


                },
                beforeSend: function () {
                    LiveReport.ajaxStart();
                },
                complete: function (jqXHR, textStatus) {
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
            url = LiveReport.setSumParams(url, totalValue);
            // [Jacob] Cheat for live report mix parlay, must send 
            url = QueryString.setParam(url, "showsb", false);
            url = QueryString.setParam(url, "showba", false);
            $(productOptions).each(function (index, element) {
                url = Site.setProductOptionParam(url, index, element.toUpperCase(), true)
                url = QueryString.setParam(url, "show" + element, true);
            });

            Site.blockUI();
            location.href = url;
        },

        getMatchListProductOption: function (sportId, productCode) {

            var productOptions = new Array();
            if (sportId > 0) {
                if (sportId == 180) {
                    productOptions.push("VS");
                }
                else if (sportId == 162) {
                    productOptions.push("LC");
                }
                else {
                    if (productCode == "SB" || productCode == "ALL") {
                        productOptions.push("SB");
                    }
                    if (productCode == "BA" || productCode == "ALL") {
                        productOptions.push("BA");
                    }
                }
            }
            else {
                if (productCode == "ALL") {
                    productOptions = ["SB", "BA", "NG", "RB", "LC", "VS","CS","BG"];
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
            var productOptions = LiveReport.getMatchListProductOption(jsonData.SportId, productCode);
            var urlMap = {
                Key101: "BetList/LiveReportMixParlay?bettype=9",
                Key102: "BetList/LiveReportMixParlay?bettype=29",
                Key103: "BetList/LiveReportOutright",
                Key161: "BetList/LiveReportNumberGame",
                Key151: "BetList/LiveReportRacing?SportType=151",
                Key152: "BetList/LiveReportRacing?SportType=152",
                Key153: "BetList/LiveReportRacing?SportType=153"
            };

            var url = urlMap["Key" + jsonData.SportId];
            var showBetList = url != null && url != undefined;
            url = url == null || url == undefined
                    ? resolveClientUrl("Report/LiveReportMatchList")
                    : resolveClientUrl(url);

            if (showBetList) {
                LiveReport.viewBetList(url, jsonData.TotalValue, productOptions);
            }
            else {
                // Match List
                url = Site.setLicenseeOptionOriginalParam(url);
                url = QueryString.setParam(url, "SportId", jsonData.SportId);
                url = QueryString.setParam(url, "ProductCode", productCode);
                url = QueryString.setParam(url, "FromDate", $("#FromDate").attr('data-original'));
                url = QueryString.setParam(url, "ToDate", $("#ToDate").attr('data-original'));
                url = QueryString.setParam(url, "showsb", true);
                url = QueryString.setParam(url, "showba", false);

                $(productOptions).each(function (index, element) {
                    url = Site.setProductOptionParam(url, index, element, true);
                    url = Site.setProductOptionParam(url, index, element, true);
                    url = QueryString.setParam(url, "show" + element, true);
                });

                Site.blockUI();
                location.href = url;
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
    LiveReport.initialize();
});