/* AllBet, VoidBridge, FishingWorld */
"use strict";
define(["jquery", "common/site", "../common/constant","../common/betsettingbase", "../common/betsetting.status"],
    function ($, Site, Constant, BetSettingBase, BetSettingStatus) {
        var Product = Constant.Product;

        function getConfirmMessage(productId) {
            switch (productId) {
                case Product.VirtualSports:
                    return "UpdateVirtualSportsBetSettings";
                default:
                    return "";
            }
        }

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
                    BetSettingBase.resetInput();
                });

                $("#btnSubmitBetSetting").on("click", function () {
                    var productId = parseInt($("#ProductId").val());
                    var messageResource = getConfirmMessage(productId);

                    BetSettingBase.showComfirm($(this), messageResource, _me.updateBetSetting);
                });

                // Formating number in text box.
                BetSettingBase.formatInputText();
            },

            updateStatus: function ($context) {
                var url = Site.resolveClientUrl("DcsBetSetting/UpdateStatus");
                var params = BetSettingStatus.getCommonPostData($context);
                BetSettingStatus.update(url, params);
            },

            updateBetSetting: function (custId, userName, nickName) {
                var productId = parseInt($("#ProductId").val());
                
                var params = BetSettingBase.getBetSettingParamsForSports();
                params.custid = custId;
                params.userName = userName;
                params.nickName = nickName;
                params.productId = productId;

                BetSettingBase.updateBetSetting(Site.resolveClientUrl("DcsBetSetting/UpdateBetSetting"), params);
            }
        };

        return _me;
    });