"use strict";
define(["jquery", "./constant"], function ($, Constant) {
    var Product = Constant.Product;

    var _me = {
        reset: function () {
            var productId = parseInt($("#ProductId").val());

            switch (productId) {
                case Product.LiveCasino:
                    _me.resetLiveCasinoBetSetting();
                    break;
                case Product.AgCasino:
                    _me.resetAgCasinoSetting();
                    break;
                case Product.GoldDeluxe:
                    _me.resetGoldDeluxeSetting();
                    break;
                case Product.BetTrade:
                    _me.resetBetTradeSetting();
                    break;
                case Product.MgcLiveCasino:
                case Product.MgcRngCasino:
                    _me.resetMicrogamingSetting();
                    break;
                case Product.Cashout:
                    _me.resetCashOutSetting();
                    break;
                default:
                    _me.resetValues();
                    break;
            }
        },

        resetLiveCasinoBetSetting: function () {
            var gameTypeCount = $("#gameTypeCount").val();

            for (var i = 1; i <= gameTypeCount; i++) {
                var oldCheck = $("#oldCheck" + i).val();
                $("input[name=gameTypeName" + i + "][data-stake-id=" + oldCheck + "]").prop("checked", true);
            }
        },

        resetAgCasinoSetting: function () {
            var $dropdown = $("#betSettingGroup");
            if ($dropdown.data("default") === "") {
                $dropdown.prop("selectedIndex", -1).change();
            } else {
                $dropdown.val($dropdown.data("default")).change();
            }
        },

        resetGoldDeluxeSetting: function () {
            var $dropdown = $("#betSettingGroup");
            if ($dropdown.data("default") === "") {
                $dropdown.prop("selectedIndex", -1).change();
            } else {
                $dropdown.val($dropdown.data("default")).change();
            }
        },

        resetBetTradeSetting: function () {
            var $checkbox = $("#enable-product");
            $checkbox.prop("checked", $checkbox.data("original") === 1);
        },

        resetMicrogamingSetting: function () {
            var $checkbox = $("#enable-product");
            $checkbox.prop("checked", $checkbox.data("original") === 1);
        },

        resetCashOutSetting: function () {
            var $checkbox = $("#enable-product");
            $checkbox.prop("checked", $checkbox.data("original") === 1);
        },

        resetValues: function () {
            $('#chkCopyAll').removeAttr('checked');
            $('input[id^="min"], input[id^="max-bet"], input[id^="max-per"]').each(function () {
                $(this).val($(this).attr('data-original')).prop('disabled', false);
            });
        },
    };

    return _me;
});