"use strict";
define(["jquery", "./constant", "./betsetting.validation"], function ($, Constant, BetSettingValidation) {
    var Product = Constant.Product;

    var _me = {
        checkCopyAllValues: function ($checkbox) {
            var enabled = $checkbox.prop("checked");

            _me.setCopyValueBetSetting($('input[id^="min"]'), enabled);
            _me.setCopyValueBetSetting($('input[id^="max-bet"]'), enabled);
            _me.setCopyValueBetSetting($('input[id^="max-per"]'), enabled);
        },

        setCopyValueBetSetting: function ($inputTextList, enabled) {
            var copyValue = $inputTextList.first().val();

            // Enable or disable textbox, except the first one
            $inputTextList.not(':eq(0)').each(function () {
                $(this).prop('disabled', enabled);
            });

            var productId = parseInt($("#ProductId").val());
            var firstSportId = productId === Product.Sportsbook ? 1 :
                            (productId === Product.Keno ? _sportIds[0] : "151_41");

            if (!BetSettingValidation.checkLimitMinMaxBet(firstSportId)) {
                return false;
            }

            if (enabled) {
                // Copy value to textboxes
                $inputTextList.not(':eq(0)').each(function () {
                    $(this).val(copyValue);
                });
            }
        }
    };

    return _me;
});