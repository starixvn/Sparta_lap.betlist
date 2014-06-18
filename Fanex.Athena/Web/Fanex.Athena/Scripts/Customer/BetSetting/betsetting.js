/*global Site*/
/// <reference path="/@JSense.js" />
"use strict";
var BetSetting = (function ($) {
    var _ids = null;
    return {
        initialize: function () {
            var $idList = $("#IDList");
            if ($idList.length > 0) {
                _ids = $idList.val().split(",");
            }
            BetSetting.initMenu();
            BetSetting.initEvent();
        },
        initEvent: function () {
            // Check data available
            var dataCount = Number($("#dataCount").val());
            if (dataCount === 0) {
                $("#btnSubmitBetSetting, #btnReset").attr('disabled', 'disabled').addClass("button-disabled");
                return;
            }

            // Copy all sportsbook (sportsbook tab)
            $('#chkCopyAll').click(function () {
                BetSetting.checkCopyAllValues($('#chkCopyAll'));
            });

            // Bind event
            $("#btnSubmitBetSetting").on("click", BetSetting.updateBetSetting);
            $("#btnReset").on("click", BetSetting.resetBetSetting);
            $("#btnUpdateStatus").click(BetSetting.updateStatus);

            // Formating number in text box.
            BetSetting.formatInputText();
        },
        formatInputText: function () {
            $("input[type=text]").blur(function () {
                $(this).parseNumber({ format: "#,###,##0.##", locale: "us" });
                $(this).formatNumber({ format: "#,###,##0.##", locale: "us" });
            });
        },
        // Displaying selected menu tab
        initMenu: function () {
            var tabid = parseInt($("#TabId").val());
            var enable = $("#editable").val() === "False" ? false : true;
            switch (tabid) {
                case 1:
                    $('#sportsbook').addClass('activeTab');
                    break;
                case 2:
                    $('#racing').addClass('activeTab');
                    break;
                case 3:
                    $('#numbergame').addClass('activeTab');
                    break;
                case 4:
                    $('#virtualsports').addClass('activeTab');
                    break;
                case 5:
                    $('#livecasino').addClass('activeTab');
                    break;
                case 6:
                    $('#rngcasino').addClass('activeTab');
                    break;
                case 7:
                    $('#keno').addClass('activeTab');
                    break;
            }

            BetSetting.checkEnableBetSetting(tabid, enable);
        },
        updateStatus: function () {
            var tabid = parseInt($("#TabId").val());
            var nickName = $(this).data("nick-name");
            // update value  (0: enable, 1: disable) is opposed with current value (1: enable, 0: disable)
            var status = $(this).data("status");
            var custId = $(this).data("cust-id");
            var url = tabid === 5 ? "BetSettingAPI/UpdateLiveCasinoStatus" : "BetSettingAPI/UpdateRngCasinoStatus";

            $.ajax({
                type: 'POST',
                url: url,
                cache: false,
                data: {
                    nickname: nickName,
                    status: status,
                    custid: custId
                },
                dataType: "json",
                success: function (result) {
                    if (result.Code.toString() === "0") {
                        BetSetting.showDialogMesage(Site.getSingleResource('MessageSuccess'), result.Message, true);
                    }
                    else {
                        BetSetting.showDialogMesage(Site.getSingleResource('Fail'), result.Message, false);
                    }
                },
                error: function (request, status, err) {
                    BetSetting.showDialogMesage(Site.getSingleResource("Error"), err, false);
                }
            });
        },
        checkEnableBetSetting: function (tabId, enable) {
            switch (tabId) {
                case 1:
                    if (!enable) {
                        $('#copy-settting').hide();
                    }
                    BetSetting.checkEnable(1, enable);
                    BetSetting.checkEnable(2, enable);
                    BetSetting.checkEnable(3, enable);
                    BetSetting.checkEnable(5, enable);
                    BetSetting.checkEnable(8, enable);
                    BetSetting.checkEnable(10, enable);
                    BetSetting.checkEnable(11, enable);
                    BetSetting.checkEnable(99, enable);
                    BetSetting.checkEnable("99MP", enable);
                    BetSetting.checkEnable(154, enable);
                    break;
                case 2:
                    BetSetting.checkEnable("151_31", enable);
                    BetSetting.checkEnable("151_41", enable);
                    BetSetting.checkEnable("151_51", enable);
                    BetSetting.checkEnable("152_41", enable);
                    BetSetting.checkEnable("152_51", enable);
                    BetSetting.checkEnable("153_51", enable);
                    break;
                case 3:
                    BetSetting.checkEnable("161", enable);
                    if (enable) {
                        $("#max-per-ball-161").removeAttr('disabled');
                    }
                    else {
                        $("#max-per-ball-161").attr('disabled', 'disabled');
                    }
                    break;
                case 4:
                    BetSetting.checkEnable("180", enable);
                    BetSetting.checkEnable("181", enable);
                    BetSetting.checkEnable("182", enable);
                    BetSetting.checkEnable("183", enable);
                    BetSetting.checkEnable("184", enable);
                    BetSetting.checkEnable("185", enable);
                    BetSetting.checkEnable("186", enable);
                    break;
                case 5:
                    BetSetting.checkEnableLiveCasino(enable);
                    break;
                case 7:
                    {
                        if (!enable) {
                            $('#copy-settting').hide();
                        }
                        $.each(_ids, function (index, value) {
                            BetSetting.checkEnable(value, enable);
                        });
                        break;
                    }
            }
        },
        checkEnable: function (sportId, enable) {
            if (enable) {
                $("#min-bet-" + sportId).removeAttr('disabled');
                $("#max-bet-" + sportId).removeAttr('disabled');
                $("#max-per-match-" + sportId).removeAttr('disabled');
            }
            else {
                $("#min-bet-" + sportId).attr('disabled', 'disabled');
                $("#max-bet-" + sportId).attr('disabled', 'disabled');
                $("#max-per-match-" + sportId).attr('disabled', 'disabled');
                $("#btnSubmitBetSetting, #btnReset").attr('disabled', 'disabled').addClass("button-disabled");
            }
        },
        checkEnableLiveCasino: function (enable) {
            var gameTypeCount = $("#gameTypeCount").val();
            if (!enable) {
                for (var i = 1; i <= gameTypeCount; i++) {
                    // var oldCheck = $("#oldCheck" + i).val();
                    $("input[name=gameTypeName" + i + "]").attr('disabled', 'disabled');
                }
                $("#btnSubmitBetSetting, #btnReset").attr('disabled', 'disabled').addClass("button-disabled");
            }
        },
        updateBetSetting: function () {
            var custId, userName, nickName, tabid;

            tabid = parseInt($("#TabId").val());
            custId = $(this).attr("data-cust-id");
            userName = $(this).attr("data-user-name");
            nickName = $(this).attr("data-nick-name");

            switch (tabid) {
                case 1:
                    if (BetSetting.checkValidationSportsBook()) {
                        BetSetting.showConfirmAction(Site.getSingleResource('UpdateSportsbookBetSettings'),
                            custId, userName, nickName, BetSetting.updateSportsbookBetSetting);
                    }
                    break;
                case 2:
                    if (BetSetting.checkValidationRacing()) {
                        BetSetting.showConfirmAction(Site.getSingleResource('UpdateRacingBetSettings'),
                            custId, userName, nickName, BetSetting.updateRacingBetSetting);
                    }
                    break;
                case 3:
                    if (BetSetting.checkValidationNumberGame()) {
                        BetSetting.showConfirmAction(Site.getSingleResource('UpdateNumberGameBetSettings'),
                            custId, userName, nickName, BetSetting.updateNumberGameBetSetting);
                    }

                    break;
                case 4:
                    if (BetSetting.checkValidationVirtualSports()) {
                        BetSetting.showConfirmAction(Site.getSingleResource('UpdateVirtualSportsBetSettings'),
                            custId, userName, nickName, BetSetting.updateVirtualSportsBetSetting);
                    }

                    break;
                case 5:
                    BetSetting.showConfirmAction(Site.getSingleResource('UpdateLiveCasinoBetSettings'),
                            custId, userName, nickName, BetSetting.updateLiveCasinoBetSetting);
                    break;
                case 7:
                    if (BetSetting.checkValidationKeno()) {
                        BetSetting.showConfirmAction(Site.getSingleResource('UpdateKenoBetSettings'),
                            custId, userName, nickName, BetSetting.updateKenoBetSetting);
                    }
                    break;
            }
        },
        checkValidationSportsBook: function () {
            BetSetting.checkCopyAllValues($('#chkCopyAll'));

            if (false === BetSetting.checkLimitMinMaxBet(1)) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet(2)) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet(3)) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet(5)) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet(8)) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet(10)) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet(11)) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet(99)) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet("99MP")) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet(154)) { return false; }
            return true;
        },
        checkValidationRacing: function () {
            if (false === BetSetting.checkLimitMinMaxBet("151_31")) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet("151_41")) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet("151_51")) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet("152_41")) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet("152_51")) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet("153_51")) { return false; }
            return true;
        },
        checkValidationNumberGame: function () {
            if (false === BetSetting.checkLimitMinMaxBet(161)) { return false; }

            var maxPerBall = BetSetting.convertToFloat($("#max-per-ball-161").val());
            var maxPerBallLimit = BetSetting.convertToFloat($("#max-per-ball-161").attr('data-max-value'));

            if (isNaN(maxPerBall)) {
                BetSetting.showBalloonMsg("max-per-ball-161", Site.getSingleResource('IncorrectMaxPerBall'));
                return false;
            }

            if (maxPerBallLimit > 0 && maxPerBall > maxPerBallLimit) {
                BetSetting.showBalloonMsg("max-per-ball-161", Site.getSingleResource('MaxPerBallLessThanOrEqual') + " " + maxPerBallLimit);
                return false;
            }

            return true;
        },
        checkValidationKeno: function () {
            BetSetting.checkCopyAllValues($('#chkCopyAll'));
            for (var i in _ids) {
                if (false === BetSetting.checkLimitMinMaxBet(_ids[i])) {
                    return false;
                }
            }

            return true;
        },
        checkValidationVirtualSports: function () {
            if (false === BetSetting.checkLimitMinMaxBet(180)) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet(181)) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet(182)) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet(183)) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet(184)) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet(185)) { return false; }
            if (false === BetSetting.checkLimitMinMaxBet(186)) { return false; }
            return true;
        },
        updateKenoBetSetting: function (custId, userName, nickName) {
            var params = "";
            $.each(_ids, function (index, value) {
                params += BetSetting.getBetSettingParams(value);
            });
            params += "&custid=" + custId;
            params += "&userName=" + userName;
            params += "&nickName=" + nickName;
            params += "&ids=" + _ids.toString();
            params = params.substring(1);

            $.ajax({
                type: 'GET',
                url: Site.resolveClientUrl('BetSettingAPI/UpdateKenoBetSetting?'),
                data: params,
                dataType: "json",
                success: function (result) {
                    BetSetting.showUpdateSettingResult(result);
                }
            });
        },
        // Update Sportsbook
        updateSportsbookBetSetting: function (custId, userName, nickName) {
            var params = "";

            params += BetSetting.getBetSettingParams(1);
            params += BetSetting.getBetSettingParams(2);
            params += BetSetting.getBetSettingParams(3);
            params += BetSetting.getBetSettingParams(5);
            params += BetSetting.getBetSettingParams(8);
            params += BetSetting.getBetSettingParams(10);
            params += BetSetting.getBetSettingParams(11);
            params += BetSetting.getBetSettingParams(99);
            params += BetSetting.getBetSettingParams("99MP");
            params += BetSetting.getBetSettingParams(154);
            params += "&custid=" + custId;
            params += "&userName=" + userName;
            params += "&nickName=" + nickName;
            params = params.substring(1);

            $.ajax({
                type: 'GET',
                url: Site.resolveClientUrl('BetSettingAPI/UpdateSportBookBetSetting?'),
                data: params,
                dataType: "json",
                success: function (result) {
                    BetSetting.showUpdateSettingResult(result);
                }
            });
        },
        // Update Racing
        updateRacingBetSetting: function (custId, userName, nickName) {
            var params = "";

            params += BetSetting.getBetSettingParams("151_31");
            params += BetSetting.getBetSettingParams("151_41");
            params += BetSetting.getBetSettingParams("151_51");
            params += BetSetting.getBetSettingParams("152_41");
            params += BetSetting.getBetSettingParams("152_51");
            params += BetSetting.getBetSettingParams("153_51");
            params += "&custid=" + custId;
            params += "&userName=" + userName;
            params += "&nickName=" + nickName;
            params = params.substring(1);

            $.ajax({
                type: 'GET',
                url: 'BetSettingAPI/updateRacingBetSetting?',
                data: params,
                dataType: "json",
                success: function (result) {
                    BetSetting.showUpdateSettingResult(result);
                }
            });
        },
        // Update Number Game
        updateNumberGameBetSetting: function (custId, userName, nickName) {
            var maxperball = BetSetting.convertToFloat($("#max-per-ball-161").val());
            var params = "";

            params += BetSetting.getBetSettingParams(161);
            params += "&maxperball=" + maxperball;
            params += "&custid=" + custId;
            params += "&userName=" + userName;
            params += "&nickName=" + nickName;
            params = params.substring(1);

            $.ajax({
                type: 'GET',
                url: 'BetSettingAPI/updateNumberGameBetSetting?',
                cache: false,
                data: params,
                dataType: "json",
                success: function (result) {
                    BetSetting.showUpdateSettingResult(result);
                }
            });
        },
        // Update Virtual Sports
        updateVirtualSportsBetSetting: function (custId, userName, nickName) {
            var params = "";

            params += BetSetting.getBetSettingParams(180);
            params += BetSetting.getBetSettingParams(181);
            params += BetSetting.getBetSettingParams(182);
            params += BetSetting.getBetSettingParams(183);
            params += BetSetting.getBetSettingParams(184);
            params += BetSetting.getBetSettingParams(185);
            params += BetSetting.getBetSettingParams(186);
            params += "&custid=" + custId;
            params += "&userName=" + userName;
            params += "&nickName=" + nickName;
            params = params.substring(1);

            $.ajax({
                type: 'GET',
                url: 'BetSettingAPI/updateVirtualSportsBetSetting?',
                cache: false,
                data: params,
                dataType: "json",
                success: function (result) {
                    BetSetting.showUpdateSettingResult(result);
                }
            });
        },
        // Update Live Casino
        updateLiveCasinoBetSetting: function (custId, userName, nickName) {
            var gameTypeCount = $("#gameTypeCount").val();
            var params = "";
            params += "custId=" + custId;
            params += "&nickName=" + nickName;
            params += "&gameTypeCount=" + gameTypeCount;
            for (var i = 1; i <= gameTypeCount; i++) {
                var gametypeId = $("#gameTypeId" + i);
                var stakeLevelNameChecked = $("#stakeLevelName" + i);
                var betTypeId = $("#betTypeId" + i);
                var stakeId = $("input[name=gameTypeName" + i + "]:checked").attr("data-stake-id");

                params += "&stakeId" + i + "=" + stakeId;
                params += "&gameTypeId" + i + "=" + gametypeId.val();
                params += "&stakeLevelName" + i + stakeLevelNameChecked.val();
                params += "&betTypeId" + i + "=" + betTypeId.val();
            }
            $.ajax({
                type: 'GET',
                url: 'BetSettingAPI/updateLiveCasinoBetSetting?',
                cache: false,
                data: params,
                dataType: "json",
                success: function (result) {
                    if (result.Code.toString() === "0") {
                        BetSetting.showDialogMesage(Site.getSingleResource('MessageSuccess'), result.Message, true);
                    }
                    else {
                        BetSetting.showDialogMesage(Site.getSingleResource('Fail'), result.Message, false);
                    }
                },
                error: function (request, status, err) {
                    BetSetting.showDialogMesage(Site.getSingleResource("Error"), err, false);
                }
            });
        },
        getBetSettingParams: function (sportId) {
            var minBetId = $("#min-bet-" + sportId);
            var maxBetId = $("#max-bet-" + sportId);
            var maxPerMatchId = $("#max-per-match-" + sportId);

            if (minBetId.length === 0 ||
                maxBetId.length === 0 ||
                maxPerMatchId.length === 0) {
                return "";
            }

            var isUpdate = true;
            var originalMinBet = $('#min-bet-' + sportId).attr('data-original');
            var originalMaxBet = $('#max-bet-' + sportId).attr('data-original');
            var originalMaxPerMatch = $('#max-per-match-' + sportId).attr('data-original');

            // Check if data is changed
            if (minBetId.val() === originalMinBet
                && maxBetId.val() === originalMaxBet
                && maxPerMatchId.val() === originalMaxPerMatch) {
                isUpdate = false;
            }

            // Data is changed, update it

            var minBet = BetSetting.convertToFloat(minBetId.val());
            var maxBet = BetSetting.convertToFloat(maxBetId.val());
            var maxPerMatcth = BetSetting.convertToFloat(maxPerMatchId.val());

            var result = "&minbet" + sportId + "=" + minBet;
            result += "&maxbet" + sportId + "=" + maxBet;
            result += "&maxpermatch" + sportId + "=" + maxPerMatcth;
            result += "&update" + sportId + "=" + isUpdate;

            return result;
        },

        // Reset bet setting
        resetBetSetting: function () {
            var tabid = parseInt($("#TabId").val());
            if (tabid === 5) {
                BetSetting.resetLiveCasinoBetSetting();
            }
            else {
                BetSetting.resetValues();
            }
        },

        resetValues: function () {
            // Reset value for min bet
            $('input[id^="min"]').each(function () {
                $(this).val($(this).attr('data-original'));
                $(this).removeAttr('disabled');
                $('#chkCopyAll').removeAttr('checked');
            });

            // Reset value for max bet
            $('input[id^="max-bet"]').each(function () {
                $(this).val($(this).attr('data-original'));
                $(this).removeAttr('disabled');
                $('#chkCopyAll').removeAttr('checked');
            });

            // Reset value for max per match and max per ball
            $('input[id^="max-per"]').each(function () {
                $(this).val($(this).attr('data-original'));
                $(this).removeAttr('disabled');
                $('#chkCopyAll').removeAttr('checked');
            });
        },
        resetLiveCasinoBetSetting: function () {
            var gameTypeCount = $("#gameTypeCount").val();

            for (var i = 1; i <= gameTypeCount; i++) {
                var oldCheck = $("#oldCheck" + i).val();
                $("input[name=gameTypeName" + i + "][data-stake-id=" + oldCheck + "]").prop("checked", true);
            }
        },

        checkCopyAllValues: function (obj) {
            // Get values copy for min bet
            var firstItemMinBet = $('input[id^="min"]').first().val();
            var arrayItemMinBet = $('input[id^="min"]');

            // Get values copy for max bet
            var firstItemMaxBet = $('input[id^="max-bet"]').first().val();
            var arrayItemMaxBet = $('input[id^="max-bet"]');

            // Get values copy for max per bet
            var firstItemMaxPerBet = $('input[id^="max-per"]').first().val();
            var arrayItemMaxPerBet = $('input[id^="max-per"]');

            if ($(obj).is(":checked") === true) {
                BetSetting.setCopyValueBetSetting(arrayItemMinBet, firstItemMinBet, true);

                BetSetting.setCopyValueBetSetting(arrayItemMaxBet, firstItemMaxBet, true);

                BetSetting.setCopyValueBetSetting(arrayItemMaxPerBet, firstItemMaxPerBet, true);
            }
            else {
                BetSetting.setCopyValueBetSetting(arrayItemMinBet, firstItemMinBet, false);

                BetSetting.setCopyValueBetSetting(arrayItemMaxBet, firstItemMaxBet, false);

                BetSetting.setCopyValueBetSetting(arrayItemMaxPerBet, firstItemMaxPerBet, false);
            }
        },

        setCopyValueBetSetting: function (obj, copyValue, enabled) {
            // Check tab ids are sportsbook or racing
            var tabid = parseInt($("#TabId").val());

            // Enable or disable textbox
            BetSetting.enableMixMaxBetSetting(obj, copyValue, enabled);

            // Check limit min max bet
            if (tabid === 1) {
                if (!BetSetting.checkLimitMinMaxBet(1)) {
                    return false;
                }
            }
            else if (tabid === 7) {
                if (!BetSetting.checkLimitMinMaxBet(_ids[0])) {
                    return false;
                }
            }
            else {
                if (!BetSetting.checkLimitMinMaxBet('151_41')) {
                    return false;
                }
            }
            if (enabled) {
                // Copy value to textboxes
                obj.not(':eq(0)').each(function () {
                    $(this).val(copyValue);
                });
            }
        },

        enableMixMaxBetSetting: function (obj, copyValue, enabled) {
            // Don't disable first item
            obj.not(':eq(0)').each(function () {
                if (enabled) {
                    // Disable textbox
                    $(this).attr('disabled', 'disabled');
                }
                else {
                    $(this).removeAttr('disabled');
                }
            });
        },

        setMinMaxBet: function (sportId, minBet, maxBet, maxPerMatch) {
            var minBetId = "min-bet-" + sportId;
            var maxBetId = "max-bet-" + sportId;
            var maxPerMatchId = "max-per-match-" + sportId;

            if ($("#" + minBetId).length === 0 ||
                $("#" + maxBetId).length === 0 ||
                $("#" + maxPerMatchId).length === 0) {
                return true;
            }

            $("#" + minBetId).val(minBet);
            $("#" + maxBetId).val(maxBet);
            $("#" + maxPerMatchId).val(maxPerMatch);
        },
        checkLimitMinMaxBet: function (sportId) {
            var minBetId = "min-bet-" + sportId;
            var maxBetId = "max-bet-" + sportId;
            var maxPerMatchId = "max-per-match-" + sportId;

            var minBetTextBox = $("#min-bet-" + sportId);
            var maxBetTextBox = $("#max-bet-" + sportId);
            var maxPerMatchTextBox = $("#max-per-match-" + sportId);
            if (minBetTextBox.length === 0 ||
                maxBetTextBox.length === 0 ||
                maxPerMatchTextBox.length === 0) {
                return true;
            }

            // Get value in attribute
            var limitMinBet = BetSetting.convertToFloat(minBetTextBox.attr('data-min-value'));
            var limitMaxBet = BetSetting.convertToFloat(maxBetTextBox.attr('data-max-value'));
            var limitMaxPerMatch = BetSetting.convertToFloat(maxPerMatchTextBox.attr('data-max-value'));
            var limitMaxPerMatchMin = BetSetting.convertToFloat(maxPerMatchTextBox.attr('data-min-value'));

            var minBet = BetSetting.convertToFloat(minBetTextBox.val());
            var maxBet = BetSetting.convertToFloat(maxBetTextBox.val());
            var maxPerMatcth = BetSetting.convertToFloat(maxPerMatchTextBox.val());

            // Check if minbet, maxbet, maxpermatch is valid number
            if (isNaN(minBet)) {
                BetSetting.showBalloonMsg(minBetId, Site.getSingleResource('IncorrectMinBet'));
                return false;
            }
            if (isNaN(maxBet)) {
                BetSetting.showBalloonMsg(maxBetId, Site.getSingleResource('IncorrectMaxBet'));
                return false;
            }
            if (isNaN(maxPerMatcth)) {
                BetSetting.showBalloonMsg(maxPerMatchId, Site.getSingleResource('IncorrectMaxPerMatch'));
                return false;
            }

            if (minBet === 0 && maxBet === 0 && maxPerMatcth === 0) { return true; }

            // Check contrains
            // min bet <=maxbet <= max per match
            if (limitMinBet > 0 && minBet < BetSetting.convertToFloat(limitMinBet.toString())) {
                BetSetting.showBalloonMsg(minBetId, Site.getSingleResource('MinBetGreaterThanOrEqual') + " " + limitMinBet);
                return false;
            }
            if (limitMaxBet > 0 && maxBet > BetSetting.convertToFloat(limitMaxBet.toString())) {
                BetSetting.showBalloonMsg(maxBetId, Site.getSingleResource('MaxBetLessThanOrEqual') + " " + limitMaxBet);
                return false;
            }
            if (limitMaxPerMatchMin > 0 && maxPerMatcth < BetSetting.convertToFloat(limitMaxPerMatchMin.toString())) {
                BetSetting.showBalloonMsg(maxPerMatchId, Site.getSingleResource('MaxBetMatchMoreThanOrEqual') + " " + limitMaxPerMatchMin);
                return false;
            }
            if (limitMaxPerMatch > 0 && maxPerMatcth > BetSetting.convertToFloat(limitMaxPerMatch.toString())) {
                BetSetting.showBalloonMsg(maxPerMatchId, Site.getSingleResource('MaxBetMatchLessThanOrEqual') + " " + limitMaxPerMatch);
                return false;
            }
            if (minBet > maxBet) {
                BetSetting.showBalloonMsg(minBetId, Site.getSingleResource('MinBetCanNotGreaterThanMaxBet'));
                return false;
            }
            if (maxBet > maxPerMatcth) {
                BetSetting.showBalloonMsg(maxBetId, Site.getSingleResource('MaxBetCanNotGreaterThanMaxPerMatch'));
                return false;
            }

            return true;
        },
        convertToFloat: function (str) {
            var temp = str.replace(/,/g, "");
            var temp2 = temp.replace(".", "");

            if (BetSetting.isInteger(temp2)) {
                return parseFloat(temp);
            }
            else {
                return NaN;
            }
        },
        isInteger: function (s) {
            var i;

            if (BetSetting.isEmpty(s)) {
                return false;
            }
            else {
                for (i = 0; i < s.length; i++) {
                    var c = s.charAt(i);

                    if (!BetSetting.isDigit(c)) { return false; }
                }
            }

            return true;
        },
        isDigit: function (c) {
            return ((c >= "0") && (c <= "9"));
        },
        isEmpty: function (s) {
            var empty = ((s == null) || (s.length === 0));
            if (!empty) {
                var news = s.replace(" ", "");
                if (news.length > 0) {
                    return false;
                }
                else {
                    return true;
                }
            }
            return empty;
        },
        showBalloonMsg: function (id, strMsg) {
            var contents = '<div onclick="hideBalloonMsg(\'' + id + '\')" style="width:140px;" class="msgerr">' + strMsg + '</div>';

            $("#" + id).blur(function () { BetSetting.hideBalloonMsg(id); });
            $("#" + id).focus();
            $("#" + id).showBalloon({
                contents: contents
            });
        },
        hideBalloonMsg: function (id) {
            $("#" + id).hideBalloon();
        },
        showDialogMesage: function (title, message, closePopup) {
            var ok = Site.getSingleResource("OK");
            var options = {
                closeOnEscapeType: true,
                model: true,
                resizable: false,
                height: 150,
                open: function () {
                    $('#btnSubmitBetSetting, #btnReset').attr('disabled', 'disabled');
                },
                buttons: [
                {
                    text: ok,
                    click: function () {
                        $("#bet-setting-message").dialog("close");
                    }
                }
                ],
                close: function () {
                    $("#bet-setting-message").remove();
                    $('#btnSubmitBetSetting, #btnReset').removeAttr('disabled');
                    if (closePopup) {
                        parent.$("#bet-setting-dialog").dialog("close");
                    }
                }
            };
            $('<div id="bet-setting-message" title="' + title + '">' + message + '</div>').dialog(options);
        },
        showConfirmAction: function (message, custId, userName, nickName, callback) {
            var ok = Site.getSingleResource("OK");
            var cancel = Site.getSingleResource("Cancel");
            var options = {
                closeOnEscapeType: true,
                model: true,
                resizable: false,
                height: 150,
                open: function () {
                    $('#btnSubmitBetSetting, #btnReset').attr('disabled', 'disabled');
                },
                buttons: [
                {
                    text: ok,
                    click: function () {
                        $("#bet-setting-confirm").dialog("close");
                        callback(custId, userName, nickName);
                    }
                },
                    {
                        text: cancel,
                        click: function () {
                            $("#bet-setting-confirm").dialog("close");
                            $('#btnSubmitBetSetting').removeAttr('disabled');
                        }
                    }
                ],
                close: function () {
                    $("#bet-setting-confirm").remove();
                    $('#btnSubmitBetSetting, #btnReset').removeAttr('disabled');
                }
            };
            var title = Site.getSingleResource("Confirmation");
            $('<div id="bet-setting-confirm" title="' + title + '" >' + message + '</div>').dialog(options);
        },

        showUpdateSettingResult: function (result) {
            if (result.Code.toString() === "0") {
                BetSetting.showDialogMesage(Site.getSingleResource('MessageSuccess'), result.Message, true);
            }
            else {
                BetSetting.showDialogMesage(Site.getSingleResource('Fail'), result.Message, false);
            }
        }
    };
})(jQuery);

$(window).load(function () {
    BetSetting.initialize();
});