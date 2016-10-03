/*global Site, QueryString*/
/// <reference path="/@JSense.js" />
define(
    ["querystring", "site", "moment", "daterangepicker"],
    function (QueryString, Site, moment, daterangepicker) {
        "use strict";

        initialize();

        function initialize() {
            initEvent();
            initMaterialSelect();
            initDateRangePicker();
            //initMonthPicker();
            Site.addUserGuide({
                resourceName: "LiveReport",
                title: Site.getSingleResource("LiveReport")
            });
        }

        function initMaterialSelect() {
            $('select').not('.disabled').material_select();
        }

        function initEvent() {
            $("#switchreport").click(function () {
                var mod = $(this).attr("data-history-mode");
                if (mod === "False") {
                    location.href = Site.resolveClientUrl("LiveReport?isViewedHistory=True");
                }
                else {
                    location.href = Site.resolveClientUrl("LiveReport?isViewedHistory=False");
                }
            });

            /*back button*/
            /*-affect the action excel*/
            window.onbeforeunload = function () {
                $("ProductCode").val("");
            };
            /**************/
            /*date-range*/

            /*drill down*/
            /* sumary detail*/
            $("body").on("click", ".product-detail-link", function () {
                viewProductDetail(this);
            });
            /* sumary detail*/
            /* detail*/
            $("body").on("click", ".sport-detailt-link", function () {
                viewSportDetail(this);
            });

            $('#exporttoexcel').click(function () {
                $('#IsExportExcel').val(true);
                $('#livereport').submit();
                $('#IsExportExcel').val(false);
            });
        }

        function initDateRangePicker() {
            $(function () {
                var start = moment().subtract(29, 'days');
                var end = moment();

                function setRangeDate(start, end) {
                    $('#FromDate').val(start.format('MM/DD/YYYY'));
                    $('#ToDate').val(end.format('MM/DD/YYYY'));
                    $('#DateRange').val(start.format('MM/DD/YYYY') + ' - ' + end.format('MM/DD/YYYY'));
                }

                $('#DateRange').daterangepicker({
                    startDate: start,
                    endDate: end,
                    ranges: {
                        'Today': [moment(), moment()],
                        'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                        'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                        'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                        'This Month': [moment().startOf('month'), moment().endOf('month')],
                        'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                    },
                    locale: {
                        format: 'MM/DD/YYYY'
                    },
                    showDropdowns: true,
                    alwaysShowCalendars: true
                }, setRangeDate);

                $('#DateRange').on('apply.daterangepicker', function (ev, picker) {
                    setRangeDate(picker.startDate, picker.endDate);
                });

                setRangeDate(start, end);
            });
        }
        function initMonthPicker() {
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
        }

        function viewProductDetail(target) {
            var productCode = $(target).attr("data-product-code");
            var data = {
                productCode: productCode,
                FromDate: $("#DateRange").attr('data-original-from').toString("MM/dd/yyyy"),
                ToDate: $("#DateRange").attr('data-original-to').toString("MM/dd/yyyy"),
                BaseCurrency: $("#BaseCurrency").attr('data-original'),
                LiveIndicator: $("#LiveIndicator").attr('data-original')
            };
            data = Site.setLicenseeOptionOriginalParam(data);
            var isSelected = $(target).parent().hasClass("selected");
            if (isSelected) { return; }

            $("a.product-detail-link").parent().removeClass("selected");
            $(target).parent().addClass("selected");

            $.ajax({
                type: "POST",
                data: data,
                dataType: "json",
                url: Site.resolveClientUrl("LiveReport/Detail"),
                success: function () {
                },
                beforeSend: Site.blockUI(),
                complete: function (jqXHR) {
                    if (jqXHR.readyState === 4) {
                        $("#ProductCode").val(data.productCode);
                        $("#detail-content").html(jqXHR.responseText);
                    }
                    Site.unBlockUI();
                }
            });
        }

        function setSumParams(url, totalValue) {
            url = QueryString.setParam(url, "totalrows", totalValue.BetCount);
            url = QueryString.setParam(url, "sumwinlost", totalValue.CustomerWinLoss);
            url = QueryString.setParam(url, "sumplayercomm", totalValue.CustomerComm);
            url = QueryString.setParam(url, "sumlicenseewl", totalValue.LicenseeWinLoss);
            url = QueryString.setParam(url, "sumcomm", totalValue.LicenseeComm);
            url = QueryString.setParam(url, "sumturnover", totalValue.TurnOver);
            url = QueryString.setParam(url, "sumamtdueto3rd", totalValue.AmountDue);

            return url;
        }

        function viewBetList(url, totalValue, productOptions) {
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "fromdate", $("#FromDate").attr('data-original-from'));
            url = QueryString.setParam(url, "todate", $("#ToDate").attr('data-original-to'));
            url = QueryString.setParam(url, "LiveIndicator", $("#LiveIndicator").attr('data-original'));
            url = setSumParams(url, totalValue);
            // [Jacob] Cheat for live report mix parlay, must send
            url = QueryString.setParam(url, "showsb", false);
            url = QueryString.setParam(url, "showba", false);
            $(productOptions).each(function (index, element) {
                url = Site.setProductOptionParam(url, index, element.toUpperCase(), true);
                url = QueryString.setParam(url, "show" + element, true);
            });

            Site.blockUI();
            location.href = url;
        }

        function getMatchListProductOption(sportId, productCode) {
            sportId = Number(sportId);
            var productOptions = [];
            if (sportId > 0) {
                if (sportId === 180 || sportId === 186) {
                    productOptions.push("VS");
                }
                else if (sportId === 162) {
                    productOptions.push("LC");
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
                    productOptions = ["SB", "BA", "NG", "RB", "LC", "VS", "CS", "BG"];
                }
                else {
                    productOptions.push(productCode);
                }
            }

            return productOptions;
        }

        function viewSportDetail(target) {
            var data = $(target).attr("data-drill-down");
            var jsonData = $.parseJSON(data);
            var productCode = $("#ProductCode").val();
            var productOptions = getMatchListProductOption(jsonData.SportId, productCode);
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
                Key185: "LiveReportBetList/Racing?SportType=185"
            };

            var url = urlMap["Key" + jsonData.SportId];
            var showBetList = url !== null && url !== undefined;
            url = url === null || url === undefined
                    ? Site.resolveClientUrl("LiveReport/MatchList")
                    : Site.resolveClientUrl(url);

            if (showBetList) {
                viewBetList(url, jsonData.TotalValue, productOptions);
            }
            else {
                // Match List
                url = Site.setLicenseeOptionOriginalParam(url);
                url = QueryString.setParam(url, "SportId", jsonData.SportId);
                url = QueryString.setParam(url, "ProductCode", productCode);
                url = QueryString.setParam(url, "FromDate", $("#FromDate").attr('data-original-from'));
                url = QueryString.setParam(url, "ToDate", $("#ToDate").attr('data-original-to'));
                url = QueryString.setParam(url, "showsb", true);
                url = QueryString.setParam(url, "showba", false);
                url = QueryString.setParam(url, "LiveIndicator", $("#LiveIndicator").attr('data-original'));

                $(productOptions).each(function (index, element) {
                    url = Site.setProductOptionParam(url, index, element, true);
                    url = QueryString.setParam(url, "show" + element, true);
                });

                Site.blockUI();
                location.href = url;
            }
        }
    }
);