/*global Site, QueryString, ProductListHelper*/
/// <reference path="/@JSense.js" />
/*
There is a special rule in match win loss report

- If user choose SB & BA, and the match belong to SB. we have to pass the param showsb=1 and showba=1 when drilldown
- That's why we don't use function setSportTypeOriginalParam, data-original attribute

*/

var MatchWinLoss = (function ($) {
    "use strict";
    var _$productList;

    function isProductOriginalChecked(productCode) {
        _$productList = _$productList || $("#ProductList");
        return $('.childCheckbox[data-product-code="' + productCode + '"]', _$productList).attr("data-original") === 'true';
    }

    return {
        resizeGrid: function () {
            var $grid = $("#ContentMainContent");
            if ($grid.length > 0) {
                var gridTop = $grid.offset().top;
                var gridHeight = window.innerHeight - gridTop - $("#TableTotal").height() - 30;
                gridHeight = Math.max(150, gridHeight);
                $grid.css("height", "auto").css("max-height", gridHeight);
            }
        },
        initialize: function () {
            ProductListHelper.initProductList();
            MatchWinLoss.initProductList();
            MatchWinLoss.initEvent();
            MatchWinLoss.resizeGrid();
            Site.addFilterCollapseExpand();
            Site.addUserGuide({
                resourceName: "MatchWL",
                title: Site.getSingleResource("MatchWinLoss")
            });
        },

        initEvent: function () {
            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                $('#MatchWinLossForm').submit();
            });

            $('#SubmitButton').click(function () {
                MatchWinLoss.submit();
            });

            $("#data-content").on("click", ".match-handler", function () {
                MatchWinLoss.viewMatchWinlossDetail(this);
            })
            .on("click", ".match-link, .downline-link", function () {
                MatchWinLoss.viewMatchWinlossBetList(this);
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

        getProductCodeList: function (productCode) {
            var codeList = ["RB", "NG", "LC", "VS", "VS2"];
            var productCodeList= [];
            if ((productCode === 'SB' || productCode === 'BA') && isProductOriginalChecked('SB')) {
                productCodeList.push('SB');
            }
            if ((productCode === 'SB' || productCode === 'BA') && isProductOriginalChecked('BA')) {
                productCodeList.push('BA');
            }

            $(codeList).each(function (index, element) {
                if (productCode === element && isProductOriginalChecked(element)) {
                    productCodeList.push(element);
                }
            });

            return productCodeList.toString();
        },

        viewMatchWinlossDetail: function (element) {
            var $parentRow = $(element).closest("tr");
            var match = $parentRow.data('match');
            var submitData = {
                LeagueId: match.LeagueId,
                MatchId: match.MatchId,
                ProductCode: match.ProductCode
            };

            submitData = Site.setLicenseeOptionOriginalParam(submitData);
            submitData.Date = $("#Date").attr('data-original');
            
            submitData.ProductCodeList = MatchWinLoss.getProductCodeList(submitData.ProductCode);
            var isExpanded = $parentRow.find('td').attr('rowspan') > 1;
            var $table = $("#data-content");
            var $expandedRows = $table.find('.expand');
            if ($expandedRows.length) {
                $expandedRows.remove();
                $table.find('.main').removeClass('main');
                $table.find('.current-expanding-column').removeAttr('rowspan');
            }

            $table.find('.icon-collapse').addClass('icon-expand').removeClass('icon-collapse');
            
            if (isExpanded) {
                return;
            }

            // Expand.
            $(element).addClass('icon-collapse').removeClass('icon-expand');

            $.ajax({
                type: 'POST',
                url: "MatchWinLossReport/Detail",
                data: JSON.stringify(submitData),
                contentType: "application/json; charset=utf-8",
                dataType: 'html',
                success: function (result) {
                    $parentRow.after(result);
                    $parentRow.addClass('main');
                    var rowspan = result.split('<tr class="expand">').length;
                    var firstColumn = $parentRow.find('td:first');
                    firstColumn.attr("rowspan", rowspan);
                    firstColumn.addClass("current-expanding-column");
                },
                beforeSend: Site.blockUI,
                complete: Site.unBlockUI
            });
        },

        viewMatchWinlossBetList: function (element) {
            var $parentRow = $(element).closest('tr');
            
            var data = $parentRow.data('match') || $parentRow.prevAll('.main').data('match');

            var productCode = data.ProductCode;
            var date = $("#Date").attr('data-original');

            var bettype = Number($(element).data('bettype'));
            bettype = isNaN(bettype) ? 0 : bettype;
            if (bettype === 85 || bettype === 86) {
                data.IsLive = $(element).data('islive');
            }
            else {
                data.IsLive = '';
            }

            var url = Site.resolveClientUrl("MatchWinlossBetList/MatchBetType");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "fromdate", date);
            url = QueryString.setParam(url, "date", date);
            url = QueryString.setParam(url, "matchid", data.MatchId);
            url = QueryString.setParam(url, "leagueid", data.LeagueId);
            url = QueryString.setParam(url, "raceno", data.RaceNo);
            url = QueryString.setParam(url, "bettype", bettype);
            url = QueryString.setParam(url, "LiveIndicator", data.IsLive);
            url = QueryString.setParam(url, "homeid", data.HomeId);
            url = QueryString.setParam(url, "awayid", data.AwayId);

            var productCodeList = MatchWinLoss.getProductCodeList(productCode);

            url = Site.setProductCodeListParam(url, productCodeList);

            Site.blockUI();
            Site.redirect(url);
        },
        viewOutrightMixParlay: function (betType) {
            var date = $("#Date").attr("data-original");
            var url = betType === 10
                        ? Site.resolveClientUrl("MatchWinlossBetList/Outright")
                        : Site.resolveClientUrl("MatchWinlossBetList/MixParlay");
            url = Site.setLicenseeOptionOriginalParam(url);
            url = QueryString.setParam(url, "fromdate", date);
            url = QueryString.setParam(url, "todate", date);
            url = QueryString.setParam(url, "bettype", betType);

            var productCodeList = [];
            if (isProductOriginalChecked('SB')) {
                productCodeList.push("SB");
            }
            if (isProductOriginalChecked('BA')) {
                productCodeList.push("BA");
            }
            if (isProductOriginalChecked('VS2')) {
                productCodeList.push("VS2");
            }

            url = Site.setProductCodeListParam(url, productCodeList);

            Site.blockUI();
            Site.redirect(url);
        }
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    MatchWinLoss.initialize();
});

$(window).resize(function () {
    "use strict";
    MatchWinLoss.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});