"use strict";
define(["jquery", "common/site", "common/querystring", "components/pluginhelper/daterangepicker",
    "components/scrollablegrid", "components/pluginhelper/multiselect", "extensions/jquery.extension"],
    function ($, Site, QueryString, DatePicker, GridHelper, MultiSelect) {
        var _me = {
            initialize: function () {
                Site.initialize();
                DatePicker.initSingleDatePicker("Date");
                MultiSelect.initialize('grid-products-multiselect', $("#productsData").val(), 'Code', 'Name', 'Status');
                Site.addUserGuide({
                    resourceName: "MatchWL",
                    title: Site.getSingleResource("MatchWinLoss")
                });
                _me.initEvent();
                GridHelper.resizeGrid();
            },

            initEvent: function () {
                $('#ExportToExcel').click(function () {
                    $('#IsExportExcel').val(true);
                    $('#MatchWinLossForm').submit();
                });

                $('#SubmitButton').click(function () {
                    _me.submit();
                });

                $(".body-content").on("click", ".view-match-detail", function () {
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
                $('#IsExportExcel').val(false);
                $('#MatchWinLossForm').submit();
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
                submitData.Date = $("#Date").attr('data-original');
                submitData.ProductOptions = _me.getProductOption(submitData.ProductCode);

                var isExpanding = $this.closest("tr").hasClass("active");
                _me.clearDetailRows($parentRow);
                if (isExpanding) {
                    return;
                }

                $this.addClass("expanded");
                $.ajax({
                    type: 'POST',
                    url: "MatchWinLossReport/Detail",
                    data: JSON.stringify(submitData),
                    contentType: "application/json; charset=utf-8",
                    dataType: 'html',
                    success: function (result) {
                        $parentRow.after(result);
                        var rowspan = $(".expanding-row").length + 1;
                        $parentRow.find("td:first").attr("rowspan", rowspan);
                        $parentRow.addClass("active");
                    },
                    beforeSend: Site.blockUI,
                    complete: Site.unBlockUI
                });
            },

            clearDetailRows: function ($parentRow) {
                var $table = $parentRow.closest("table");
                var $expandingRows = $table.find(".expanding-row");
                if ($expandingRows.length) {
                    $expandingRows.remove();
                    $table.find(".active").removeClass("active")
                        .find("td:first").removeAttr("rowspan");
                }
                $(".view-match-detail").removeClass("expanded");
            },

            viewMatchWinlossBetList: function (element) {
                var $parentRow = $(element).closest('tr');
                var matchData = $parentRow.data('match') || $(".active").data('match');
                var productCode = matchData.ProductCode;
                var date = $("#Date").attr('data-original');

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

                var products = _me.getProductOption(productCode);
                for (var i = 0; i < products.length; i++) {
                    var item = products[i];
                    url = Site.setProductOptionParam(url, i, item.Code, item.Status);
                }

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

                var products = _me.getProductOption(["SB", "BA"]);
                for (var i = 0; i < products.length; i++) {
                    var item = products[i];
                    url = Site.setProductOptionParam(url, i, item.Code, item.Status);
                    url = QueryString.setParam(url, "show" + item.Code, item.Status);
                }

                Site.blockUI();
                Site.redirect(url);
            },

            getProductOption: function (productCode) {
                var specialProducts = ["SB", "BA"];
                var containSpecialProducts = _me.contain(specialProducts, productCode);
                var productOptions = JSON.parse($("#productsData").val());

                return productOptions.filter(function (item) {
                    if (containSpecialProducts) {
                        return _me.contain(specialProducts, item.Code) & item.Status;
                    }

                    var containCode = Array.isArray(productCode) ? _me.contain(productCode, item.Code) : item.Code === productCode;
                    return containCode && item.Status;
                });
            },

            contain: function (arrayData, value) {
                return arrayData.indexOf(value) > -1;
            }
        };

        return _me;
    });