/*global Site, QueryString*/
/// <reference path="/@JSense.js" />
/*
There is a special rule in match win loss report

- If user choose SB & BA, and the match belong to SB. we have to pass the param showsb=1 and showba=1 when drilldown
- That's why we don't use function setSportTypeOriginalParam, data-original attribute

*/

var MatchWinLoss = (function ($) {
    "use strict";
    return {
        initialize: function () {
            MatchWinLoss.initProductList();
            MatchWinLoss.initEvent();

            var $grid = $("#ContentMainContent");
            if ($grid.length > 0) {
                var gridTop = $grid.offset().top;
                var gridHeight = $(window).height() - gridTop - $("#TableTotal").height() - 10;
                gridHeight = Math.max(160, gridHeight);
                $grid.css("max-height", gridHeight);
            }
        },

        initEvent: function () {
            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                $('#MatchWinLossForm').submit();
            });

            $('#SubmitButton').click(function () {
                MatchWinLoss.submit();
            });
        },

        initProductList: function () {
            if ($('#ProductList').hasClass('all-products') === false) {
                $('.product-lable').remove();
            }
        },

        submit: function () {
            Site.blockUI();
            $('#IsExportExcel').val(false);
            $('#MatchWinLossForm').submit();
        },

        getProductOption: function (productCode) {
            var codeList = ["RB", "NG", "LC", "VS"];
            var productList = $("#ProductList");
            var productOptions = [];
            var showSb = (productCode === 'SB' || productCode === 'BA') && $('input[data-product-code="SB"]', productList).attr("data-original") === 'True';
            var showBa = (productCode === 'SB' || productCode === 'BA') && $('input[data-product-code="BA"]', productList).attr("data-original") === 'True';
            if (showSb) { productOptions.push({ Code: "SB", Status: true }); }
            if (showBa) { productOptions.push({ Code: "BA", Status: true }); }

            $(codeList).each(function (index, element) {
                var show = productCode === element && $('input[data-product-code="' + element + '"]', productList).attr("data-original") === 'True';
                if (show) { productOptions.push({ Code: element, Status: true }); }
            });

            return productOptions;
        },

        viewMatchWinlossDetail: function (element, data) {
            var parentRow = $(element).closest("tr");
            var reportTable = $("#data-content");
            data = Site.setLicenseeOptionOriginalParam(data);
            data.Date = $("#Date").attr('data-original');
            data.ProductOptions = MatchWinLoss.getProductOption(data.ProductCode);

            // Expand & collapse
            var isExpanding = parentRow.find('td').attr('rowspan') > 1;

            if ($('tr', reportTable).hasClass('expand')) {
                $('.main', reportTable).removeClass('main');
                $('.expand', reportTable).remove();
                $('.current-expanding-column', reportTable).removeAttr('rowspan');
            }

            if ($('td', reportTable).hasClass('arrow-down')) {
                $('.arrow-down', reportTable).addClass('arrow-right');
                $('.arrow-down', reportTable).removeClass('arrow-down');
            }

            if (isExpanding) {
                return;
            }

            $(parentRow).find('td.arrow-right').addClass('arrow-down');
            $(parentRow).find('td.arrow-down').removeClass('arrow-right');

            $.ajax({
                type: 'POST',
                url: "MatchWinLossDetail",
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: 'html',
                success: function (result) {
                    parentRow.after(result);
                    parentRow.addClass('main');
                    var rowspan = result.split('<tr class="expand">').length;
                    var firstColumn = $(parentRow).find('td:first');
                    firstColumn.attr("rowspan", rowspan);
                    firstColumn.addClass("current-expanding-column");
                    firstColumn.attr("data-product-code", data.ProductCode);
                },
                error: function () {
                    alert('error');
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function () {
                    Site.unBlockUI();
                }
            });
        },

        viewMatchWinlossBetList: function (data) {
            var reportTable = $("#data-content");
            var productCode = $(".current-expanding-column", reportTable).attr("data-product-code");
            var date = $("#Date").attr('data-original');

            var bettype = Number(data.BetType);
            if (bettype === 85 || bettype === 86) {
                data.IsLive = data.IsLive ? '1' : '0';
            }
            else {
                data.IsLive = '';
            }
            
            var url = Site.resolveClientUrl("BetList/MatchWlMatchBetType");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "fromdate", date);
            url = QueryString.setParam(url, "fromdate", date);
            url = QueryString.setParam(url, "date", date);
            url = QueryString.setParam(url, "matchid", data.MatchId);
            url = QueryString.setParam(url, "leagueid", data.LeagueId);
            url = QueryString.setParam(url, "raceno", data.RaceNo);
            url = QueryString.setParam(url, "bettype", data.BetType);
            url = QueryString.setParam(url, "LiveIndicator", data.IsLive);
            url = QueryString.setParam(url, "homeid", data.HomeId);
            url = QueryString.setParam(url, "awayid", data.AwayId);

            // Sport Type params
            var productList = $("#ProductList");
            var showSb = (productCode === 'SB' || productCode === 'BA') && $('input[data-product-code="SB"]', productList).attr("data-original") === 'True';
            var showBa = (productCode === 'SB' || productCode === 'BA') && $('input[data-product-code="BA"]', productList).attr("data-original") === 'True';
            url = Site.setProductOptionParam(url, 0, "SB", showSb);
            url = Site.setProductOptionParam(url, 1, "BA", showBa);

            var codeList = ["RB", "NG", "LC", "VS"];
            $(codeList).each(function (index, element) {
                var show = productCode === element && $('input[data-product-code="' + element + '"]', productList).attr("data-original") === 'True';
                url = Site.setProductOptionParam(url, index + 2, element, show);
            });

            Site.blockUI();
            window.location.href = url;
        },
        viewOutrightMixParlay: function (betType) {
            var date = $("#Date").attr("data-original");
            var url = betType === 10
                        ? Site.resolveClientUrl("BetList/MatchWlOutright")
                        : Site.resolveClientUrl("BetList/MatchWlMixParlay");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "fromdate", date);
            url = QueryString.setParam(url, "todate", date);
            url = QueryString.setParam(url, "bettype", betType);

            // Sport type
            var productList = $("#ProductList");
            var showSb = $('input[data-product-code="SB"]', productList).attr("data-original") === 'True';
            var showBa = $('input[data-product-code="BA"]', productList).attr("data-original") === 'True';
            url = Site.setProductOptionParam(url, 0, "SB", showSb);
            url = Site.setProductOptionParam(url, 1, "BA", showBa);
            url = QueryString.setParam(url, "showsb", showSb);
            url = QueryString.setParam(url, "showba", showBa);

            Site.blockUI();
            window.location.href = url;
        }
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    MatchWinLoss.initialize();
});