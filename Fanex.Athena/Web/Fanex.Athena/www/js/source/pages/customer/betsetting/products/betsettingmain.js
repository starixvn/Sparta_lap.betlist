"use strict";
define(["jquery", "common/site", "../common/constant", "../common/betsettingbase", "../common/betsetting.copy"], function ($, Site, Constant, BetSettingBase, BetSettingCopy) {
    var Product = Constant.Product;

    var _me = {
        initialize: function () {
            Site.initialize();
            BetSettingBase.initialize();
            BetSettingBase.setEnable();
            _me.initEvent();
        },

        initEvent: function () {
            // Copy all sportsbook (sportsbook tab)
            $('#chkCopyAll').click(function () {
                BetSettingCopy.checkCopyAllValues($(this));
            });

            // Bind event
            $("#btnReset").on("click", function () {
                BetSettingBase.resetInput();
            });

            $("#btnSubmitBetSetting").on("click", function () {
                var productId = parseInt($("#ProductId").val());
                var messageResource = productId === Product.Sportsbook ? "UpdateSportsbookBetSettings" :
                        productId === Product.Racing ? "UpdateRacingBetSettings" : "";

                BetSettingBase.showComfirm($(this), messageResource, _me.updateBetSetting);
            });

            // Formating number in text box.
            BetSettingBase.formatInputText();
        },

        updateBetSetting: function (custId, userName, nickName) {
            var productId = parseInt($("#ProductId").val());
            var url = productId === Product.Sportsbook ? "SportsbookBetSetting/UpdateBetSetting" :
                    productId === Product.Racing ? "RacingBetSetting/UpdateBetSetting" : null;

            if (url === null) {
                return;
            }

            var params = BetSettingBase.getBetSettingParamsForSports();
            params.custid = custId;
            params.userName = userName;
            params.nickName = nickName;

            BetSettingBase.updateBetSetting(Site.resolveClientUrl(url), params);
        }
    };

    return _me;
});