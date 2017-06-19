"use strict";
define(["jquery", "common/site", "./constant", "./betsetting.helper"], function ($, Site, Constant, BetSettingHelper) {
    var Product = Constant.Product;
    var SPORTID_NUMBERGAME = 161;
    var _sportIds;

    var _me = {
        initialize: function (sportIds) {
            _sportIds = sportIds
        },

        checkValidationNumberGame: function () {
            if (false === _me.checkLimitMinMaxBet(SPORTID_NUMBERGAME)) { return false; }

            var maxPerBallSelector = "#max-per-ball-161";
            var maxPerBall = BetSettingHelper.convertToFloat($(maxPerBallSelector).val());
            var maxPerBallLimit = BetSettingHelper.convertToFloat($(maxPerBallSelector).attr('data-max-value'));

            if (isNaN(maxPerBall)) {
                BetSettingHelper.showBalloonMsg(maxPerBallSelector, Site.getSingleResource('IncorrectMaxPerBall'));
                return false;
            }

            if (maxPerBallLimit > 0 && maxPerBall > maxPerBallLimit) {
                BetSettingHelper.showBalloonMsg(maxPerBallSelector, Site.getSingleResource('MaxPerBallLessThanOrEqual') + " " + maxPerBallLimit);
                return false;
            }

            return true;
        },

        checkLimitMinMaxBet: function (sportId) {
            var minBetId = "#min-bet-" + sportId;
            var maxBetId = "#max-bet-" + sportId;
            var maxPerMatchId = "#max-per-match-" + sportId;

            var $minBetTextBox = $(minBetId);
            var $maxBetTextBox = $(maxBetId);
            var $maxPerMatchTextBox = $(maxPerMatchId);
            if ($minBetTextBox.length === 0 ||
                $maxBetTextBox.length === 0 ||
                $maxPerMatchTextBox.length === 0) {
                return true;
            }

            // Get value in attribute
            var limitMinBet = BetSettingHelper.convertToFloat($minBetTextBox.attr('data-min-value'));
            var limitMaxBet = BetSettingHelper.convertToFloat($maxBetTextBox.attr('data-max-value'));
            var limitMaxPerMatch = BetSettingHelper.convertToFloat($maxPerMatchTextBox.attr('data-max-value'));
            var limitMaxPerMatchMin = BetSettingHelper.convertToFloat($maxPerMatchTextBox.attr('data-min-value'));

            var minBet = BetSettingHelper.convertToFloat($minBetTextBox.val());
            var maxBet = BetSettingHelper.convertToFloat($maxBetTextBox.val());
            var maxPerMatcth = BetSettingHelper.convertToFloat($maxPerMatchTextBox.val());

            // Check if minbet, maxbet, maxpermatch is valid number
            if (isNaN(minBet)) {
                BetSettingHelper.showBalloonMsg(minBetId, Site.getSingleResource('IncorrectMinBet'));
                return false;
            }
            if (isNaN(maxBet)) {
                BetSettingHelper.showBalloonMsg(maxBetId, Site.getSingleResource('IncorrectMaxBet'));
                return false;
            }
            if (isNaN(maxPerMatcth)) {
                BetSettingHelper.showBalloonMsg(maxPerMatchId, Site.getSingleResource('IncorrectMaxPerMatch'));
                return false;
            }

            if (minBet === 0 && maxBet === 0 && maxPerMatcth === 0) { return true; }

            // Check contrains
            // min bet <=maxbet <= max per match
            if (limitMinBet > 0 && minBet < BetSettingHelper.convertToFloat(limitMinBet.toString())) {
                BetSettingHelper.showBalloonMsg(minBetId, Site.getSingleResource('MinBetGreaterThanOrEqual') + " " + limitMinBet);
                return false;
            }
            if (limitMaxBet > 0 && maxBet > BetSettingHelper.convertToFloat(limitMaxBet.toString())) {
                BetSettingHelper.showBalloonMsg(maxBetId, Site.getSingleResource('MaxBetLessThanOrEqual') + " " + limitMaxBet);
                return false;
            }
            if (limitMaxPerMatchMin > 0 && maxPerMatcth < BetSettingHelper.convertToFloat(limitMaxPerMatchMin.toString())) {
                BetSettingHelper.showBalloonMsg(maxPerMatchId, Site.getSingleResource('MaxBetMatchMoreThanOrEqual') + " " + limitMaxPerMatchMin);
                return false;
            }
            if (limitMaxPerMatch > 0 && maxPerMatcth > BetSettingHelper.convertToFloat(limitMaxPerMatch.toString())) {
                BetSettingHelper.showBalloonMsg(maxPerMatchId, Site.getSingleResource('MaxBetMatchLessThanOrEqual') + " " + limitMaxPerMatch);
                return false;
            }
            if (minBet > maxBet) {
                BetSettingHelper.showBalloonMsg(minBetId, Site.getSingleResource('MinBetCanNotGreaterThanMaxBet'));
                return false;
            }
            if (maxBet > maxPerMatcth) {
                BetSettingHelper.showBalloonMsg(maxBetId, Site.getSingleResource('MaxBetCanNotGreaterThanMaxPerMatch'));
                return false;
            }

            return true;
        },

        checkValidation: function () {
            var $checkbox = $('#chkCopyAll');
            if ($checkbox.length > 0) {
                _me.checkCopyAllValues($checkbox);
            }
            return _me.checkLimitMinMaxBets();
        },

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
            if (!_me.checkLimitMinMaxBet(firstSportId)) {
                return false;
            }

            if (enabled) {
                // Copy value to textboxes
                $elementList.not(':eq(0)').each(function () {
                    $(this).val(copyValue);
                });
            }
        },

        checkLimitMinMaxBets: function () {
            for (var i = 0; i < _sportIds.length; i++) {
                if (_me.checkLimitMinMaxBet(_sportIds[i]) === false) {
                    return false;
                }
            }

            return true;
        }
    };

    return _me;
});