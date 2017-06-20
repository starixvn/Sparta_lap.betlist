"use strict";
define(["jquery", "common/site", "./constant", "./betsetting.helper"], function ($, Site, Constant, BetSettingHelper) {
    var Product = Constant.Product;
    var _url = null;

    var _me = {
        update: function ($context) {
            var postData = _me.getPostData($context);

            $.ajax({
                type: 'POST',
                url: Site.resolveClientUrl(_url),
                cache: false,
                data: postData,
                dataType: "json",
                success: function (result) {
                    var messageResource = result.IsSuccessful ? "MessageSuccess" : "Fail";
                    BetSettingHelper.showDialogMesage(Site.getSingleResource(messageResource), result.Message, false);
                },
                error: function (request, status, err) {
                    BetSettingHelper.showDialogMesage(Site.getSingleResource("Error"), err, false);
                }
            });
        },

        getPostData: function ($context) {
            var productId = parseInt($("#ProductId").val());
            var nickName = $context.data("nick-name");
            // update value  (0: enable, 1: disable) is opposed with current value (1: enable, 0: disable)
            var status = $context.data("status");
            var custId = $context.data("cust-id");

            switch (productId) {
                case Product.LiveCasino:
                    _url = "LiveCasinoBetSetting/UpdateStatus";
                    break;
                case Product.RngCasino:
                    _url = "RngCasinoBetSetting/UpdateStatus";
                    break;
                case Product.AgCasino:
                    _url = "AgCasinoBetSetting/UpdateStatus";
                    break;
                case Product.GoldDeluxe:
                    _url = "GoldDeluxeBetSetting/UpdateStatus";
                    break;
                case Product.Macau:
                    _url = "ThirdPartyBetSetting/UpdateMacauStatus";
                    break;
                case Product.MgcLiveCasino:
                    _url = "ThirdPartyBetSetting/UpdateMicroGamingLiveCasinoStatus";
                    status = $("#enable-product").is(":checked") ? 1 : 0;
                    break;
                case Product.Colossusbets:
                    _url = "ThirdPartyBetSetting/UpdateColossusBetsStatus";
                    break;
                case Product.MgcRngCasino:
                    _url = "ThirdPartyBetSetting/UpdateMicroGamingRNGCasinoStatus";
                    status = $("#enable-product").is(":checked") ? 1 : 0;
                    break;
                case Product.BetTrade:
                    _url = "ThirdPartyBetSetting/UpdateBetTradeStatus";
                    status = $("#enable-product").is(":checked") ? 1 : 0;
                    break;
                case Product.AllBet:
                case Product.VoidBridge:
                case Product.FishingWorld:
                    _url = "DcsBetSetting/UpdateStatus";
                    break;
                case Product.Cashout:
                    _url = "ThirdPartyBetSetting/UpdateCashOutStatus";
                    status = $("#enable-product").is(":checked") ? 1 : 0;
                    break;
            }

            return {
                productId: productId,
                nickname: nickName,
                status: status,
                custid: custId
            }
        }
    };

    return _me;
});