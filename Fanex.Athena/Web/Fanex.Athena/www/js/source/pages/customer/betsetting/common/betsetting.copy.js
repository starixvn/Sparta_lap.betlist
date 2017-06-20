"use strict";
define(["jquery", "./constant", "./betsetting.validation"], function ($, Constant, BetSettingValidation) {
    var Product = Constant.Product;

    var _me = {
        checkCopyAllValues: function ($checkbox) {
            var arrayItemMinBet = $('input[id^="min"]');
            var arrayItemMaxBet = $('input[id^="max-bet"]');
            var arrayItemMaxPerBet = $('input[id^="max-per"]');
            var enable = $checkbox.prop("checked");

            _me.setCopyValueBetSetting(arrayItemMinBet, enable);
            _me.setCopyValueBetSetting(arrayItemMaxBet, enable);
            _me.setCopyValueBetSetting(arrayItemMaxPerBet, enable);
        },

        setCopyValueBetSetting: function ($elementList, enabled) {
            var copyValue = $elementList.first().val();

            // Enable or disable textbox, except the first
            $elementList.not(':eq(0)').each(function () {
                $(this).prop('disabled', enabled);
            });

            var productId = parseInt($("#ProductId").val());
            var firstSportId = productId === Product.Sportsbook ? 1 :
                            (productId === Product.Keno ? _sportIds[0] : "151_41");
            // Check limit min max bet
            if (!BetSettingValidation.checkLimitMinMaxBet(firstSportId)) {
                return false;
            }

            if (enabled) {
                // Copy value to textboxes
                $elementList.not(':eq(0)').each(function () {
                    $(this).val(copyValue);
                });
            }
        }
    };

    return _me;
});