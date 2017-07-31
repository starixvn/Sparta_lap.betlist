/* AllBet, VoidBridge, FishingWorld */
"use strict";
define(["jquery", "common/site", "../common/constant","../common/betsettingbase", "../common/betsetting.status"],
    function ($, Site, Constant, BetSettingBase, BetSettingStatus) {
        var Product = Constant.Product;
        var ProductCodeMap = {};
        ProductCodeMap[Product.VirtualSports] = "VS";
        ProductCodeMap[Product.Lottery] = "LT";

        function getConfirmMessage(productId) {
            var productNameResource = "Product" + ProductCodeMap[productId];
            var message = Site.getSingleResource("UpdateProductBetSettings");
            return message.replace("{0}", Site.getSingleResource(productNameResource));
        }

        var _me = {
            initialize: function () {
                Site.initialize();
                BetSettingBase.initialize();
                _me.initEvent();
                _me.checkToDisableInput();
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
                    Site.getSingleResource("UpdateProductBetSettings");
                    var messageResource = getConfirmMessage(productId);

                    BetSettingBase.showComfirm($(this), messageResource, _me.updateBetSetting);
                });

                // Formating number in text box.
                BetSettingBase.formatInputText();
            },
            checkToDisableInput: function () {
                var enabled = $("#IsEnabled").val() === "1";

                if (!enabled) {
                    $(".table-content input[type=text]").prop('disabled', true).addClass("disabled");
                    $("#btnSubmitBetSetting, #btnReset").prop('disabled', true).addClass("disabled");
                }
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