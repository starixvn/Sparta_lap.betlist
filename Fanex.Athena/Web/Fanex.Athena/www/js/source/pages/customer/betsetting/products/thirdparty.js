/* Macau, MgcLiveCasino, Colossusbets, MgcRngCasino, BetTrade, Cashout */
"use strict";
define(["jquery", "common/site", "../common/constant", "../common/betsettingbase", "../common/betsetting.status"],
    function ($, Site, Constant, BetSettingBase, BetSettingStatus) {
        var Product = Constant.Product;
        var CHECKBOX_SELECTOR = "#enable-product";
        var PRODUCT_CONTAIN_CHECKBOX = [Product.MgcLiveCasino, Product.MgcRngCasino, Product.BetTrade, Product.Cashout];

        var _me = {
            initialize: function () {
                Site.initialize();
                BetSettingBase.initialize();
                _me.initEvent();
            },

            initEvent: function () {
                $("#btnUpdateStatus").click(function () {
                    _me.updateStatus($(this));
                });

                $("#btnReset").on("click", function () {
                    _me.reset();
                });
            },

            reset: function () {
                var $checkbox = $(CHECKBOX_SELECTOR);
                $checkbox.prop("checked", $checkbox.data("original") === 1);
            },

            updateStatus: function ($context) {
                var productId = parseInt($("#ProductId").val());
                var url = _me.getUrlByProduct(productId);
                var params = BetSettingStatus.getCommonPostData($context);

                if (PRODUCT_CONTAIN_CHECKBOX.indexOf(productId) > -1) {
                    params.status = _me.getStatus();
                }

                BetSettingStatus.update(Site.resolveClientUrl(url), params);
            },

            getUrlByProduct: function (productId) {
                var url = null;
                switch (productId) {
                    case Product.Macau:
                        url = "ThirdPartyBetSetting/UpdateMacauStatus";
                        break;
                    case Product.MgcLiveCasino:
                        url = "ThirdPartyBetSetting/UpdateMicroGamingLiveCasinoStatus";
                        break;
                    case Product.Colossusbets:
                        url = "ThirdPartyBetSetting/UpdateColossusBetsStatus";
                        break;
                    case Product.MgcRngCasino:
                        url = "ThirdPartyBetSetting/UpdateMicroGamingRNGCasinoStatus";
                        break;
                    case Product.BetTrade:
                        url = "ThirdPartyBetSetting/UpdateBetTradeStatus";
                        break;
                    case Product.Cashout:
                        url = "ThirdPartyBetSetting/UpdateCashOutStatus";
                        break;
                }

                return url;
            },

            getStatus: function () {
                return $(CHECKBOX_SELECTOR).is(":checked") ? 1 : 0;
            }
        };

        return _me;
    });