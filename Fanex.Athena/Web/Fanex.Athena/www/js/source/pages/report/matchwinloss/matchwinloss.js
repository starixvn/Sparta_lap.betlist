"use strict";
define(["jquery", "common/site", "common/querystring", "components/pluginhelper/daterangepicker",
    "components/scrollablegrid", "components/pluginhelper/multiselect", "extensions/jquery.extension"],
    function ($, Site, QueryString, DatePicker, GridHelper, MultiSelect) {
        var ClassName = {
            active: "active",
            expanded: "expanded"
        };

        var Selector = {
            detailRow: ".detail-row",
            viewDetail: ".view-match-detail",
            active: "." + ClassName.active,
            date: "#Date",
            isExportExcel: "#IsExportExcel",
            matchWinLossForm: "#MatchWinLossForm"
        };

        var _$productList;

        function isProductOriginalChecked(productCode) {
            _$productList = _$productList || $("#ProductList");
            return $('.childCheckbox[data-product-code="' + productCode + '"]', _$productList).attr("data-original") === 'true';
        }

        var _me = {
            initialize: function () {
                Site.initialize();
                DatePicker.initSingleDatePicker("Date");
                MultiSelect.initProductList();
                Site.addUserGuide({
                    resourceName: "MatchWL",
                    title: Site.getSingleResource("MatchWinLoss")
                });
                _me.initEvent();
                GridHelper.resizeGrid();
            },

            initEvent: function () {
                $('#ExportToExcel').click(function () {
                    $(Selector.isExportExcel).val(true);
                    $(Selector.matchWinLossForm).submit();
                });

                $('#SubmitButton').click(function () {
                    _me.submit();
                });

                $(".body-content").on("click", Selector.viewDetail, function () {
                    _me.viewMatchWinlossDetail(this);
                })
                .on("click", ".bet-list-link", function () {
                    _me.viewMatchWinlossBetList(this);
                });

                $(".footer-content").on("click", ".outright-link", function () {
                    var bettype = $(this).data("bettype");
                    _me.viewOutrightMixParlay(bettype);
                });

                $(window).on("resize", function () {
                    GridHelper.resizeGrid();
                });

                $(window).on("unload", function () {
                    $(window).off("resize");
                });
            },

            submit: function () {
                Site.blockUI();
                $(Selector.isExportExcel).val(false);
                $(Selector.matchWinLossForm).submit();
            },

            viewMatchWinlossDetail: function (element) {
                var $this = $(element);
                var $parentRow = $($this).closest("tr");
                var matchData = $parentRow.data('match');
                var submitData = {
                    LeagueId: matchData.LeagueId,
                    MatchId: matchData.MatchId,
                    ProductCode: matchData.ProductCode
                };

                submitData = Site.setLicenseeOptionOriginalParam(submitData);
                submitData.Date = $(Selector.date).attr('data-original');
                submitData.ProductCodeList = _me.getProductCodeList(submitData.ProductCode);

                var isExpanding = $this.closest("tr").hasClass(ClassName.active);
                _me.clearDetailRows($parentRow);
                if (isExpanding) {
                    return;
                }

                $this.addClass(ClassName.expanded);
                $.ajax({
                    type: 'POST',
                    url: "MatchWinLossReport/Detail",
                    data: JSON.stringify(submitData),
                    contentType: "application/json; charset=utf-8",
                    dataType: 'html',
                    success: function (result) {
                        $parentRow.after(result);
                        var rowspan = $(Selector.detailRow).length + 1;
                        $parentRow.find("td:first").attr("rowspan", rowspan);
                        $parentRow.addClass(ClassName.active);
                    },
                    beforeSend: Site.blockUI,
                    complete: Site.unBlockUI
                });
            },

            clearDetailRows: function ($parentRow) {
                var $table = $parentRow.closest("table");
                var $detailRows = $table.find(Selector.detailRow);
                if ($detailRows.length) {
                    $detailRows.remove();
                    $table.find(Selector.active).removeClass(ClassName.active)
                        .find("td:first").removeAttr("rowspan");
                }
                $(Selector.viewDetail).removeClass(ClassName.expanded);
            },

            viewMatchWinlossBetList: function (element) {
                var $parentRow = $(element).closest('tr');
                var matchData = $parentRow.data('match') || $parentRow.siblings(Selector.active).data('match');
                var productCode = matchData.ProductCode;
                var date = $(Selector.date).attr('data-original');

                var bettype = Number($(element).data('bettype'));
                bettype = isNaN(bettype) ? 0 : bettype;
                if (bettype === 85 || bettype === 86) {
                    matchData.IsLive = $(element).data('islive');
                }
                else {
                    matchData.IsLive = '';
                }

                var url = Site.resolveClientUrl("MatchWinlossBetList/MatchBetType");
                url = Site.setLicenseeOptionOriginalParam(url);
                url = QueryString.setParam(url, "fromdate", date);
                url = QueryString.setParam(url, "date", date);
                url = QueryString.setParam(url, "matchid", matchData.MatchId);
                url = QueryString.setParam(url, "leagueid", matchData.LeagueId);
                url = QueryString.setParam(url, "raceno", matchData.RaceNo);
                url = QueryString.setParam(url, "bettype", bettype);
                url = QueryString.setParam(url, "LiveIndicator", matchData.IsLive);
                url = QueryString.setParam(url, "homeid", matchData.HomeId);
                url = QueryString.setParam(url, "awayid", matchData.AwayId);

                var productCodeList = _me.getProductCodeList(productCode);
                url = Site.setProductCodeListParam(url, productCodeList);

                Site.blockUI();
                Site.redirect(url);
            },

            viewOutrightMixParlay: function (betType) {
                var date = $(Selector.date).attr("data-original");
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
                url = Site.setProductCodeListParam(url, productCodeList);

                Site.blockUI();
                Site.redirect(url);
            },

            getProductCodeList: function (productCode) {
                var codeList = ["RB", "NG", "LC", "VS", "BRVS"];
                var productCodeList = [];
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
            }
        };

        return _me;
    });