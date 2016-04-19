/*global Site, AgCasino, GoldDeluxe */
/// <reference path="/@JSense.js" />
"use strict";

var BetSetting = (function ($) {
    var TAB_SPORTSBOOK = 0;
    var TAB_RACING = 1;
    var TAB_NUMBERGAME = 2;
    var TAB_LIVECASINO = 4;
    var TAB_VIRTUALSPORTS = 5;
    var TAB_RNGCASINO = 6;
    var TAB_KENO = 8;
    var TAB_AGCASINO = 10;
    var TAB_GOLDDELUXE = 11;
    var TAB_MACAU = 12;
    var TAB_BETTRADE = 13;
    var TAB_MICROGAMINGLIVECASINO = 14;
    var TAB_MICROGAMINGRNGCASINO = 17;
    var TAB_VOIDBRIDGE = 22;

    var _sportIds = null;
    return {
        initialize: function () {
            var $idList = $("#IDList");
            if ($idList.length > 0) {
                _sportIds = $idList.val().split(",");
            }

            BetSetting.initMenu();
            BetSetting.initEvent();
        },
        initEvent: function () {
            // Check data available
            var dataCount = Number($("#dataCount").val());
            if (dataCount === 0) {
                $("#btnSubmitBetSetting, #btnReset").prop('disabled', true).addClass("button-disabled");
                return;
            }

            // Copy all sportsbook (sportsbook tab)
            $('#chkCopyAll').click(function () {
                BetSetting.checkCopyAllValues($(this));
            });

            // Bind event

            $("#btnSubmitBetSetting").on("click", BetSetting.updateBetSetting);
            $("#btnReset").on("click", BetSetting.resetBetSetting);
            $("#btnUpdateStatus").click(BetSetting.updateStatus);
            $("#btnDisableEsports").on("click", BetSetting.updateEsportStatus);

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
            var productId = parseInt($("#ProductId").val());
            var enable = $("#editable").val() === "False" ? false : true;
            $("#product-dropdown").on('click', function (event) {
                $(this).toggleClass('active');
                event.stopPropagation();
            });
            $("#product-dropdown").on("click", "a", function () {
                Site.blockUI();
            });
            BetSetting.checkEnableBetSetting(productId, enable);
        },
        updateStatus: function () {
            var productId = parseInt($("#ProductId").val());
            var nickName = $(this).data("nick-name");
            // update value  (0: enable, 1: disable) is opposed with current value (1: enable, 0: disable)
            var status = $(this).data("status");
            var custId = $(this).data("cust-id");
            var url = null;

            switch (productId) {
                case TAB_LIVECASINO:
                    url = "BetSettingAPI/UpdateLiveCasinoStatus";
                    break;
                case TAB_RNGCASINO:
                    url = "BetSettingAPI/UpdateRngCasinoStatus";
                    break;
                case TAB_AGCASINO:
                    url = "AgCasinoBetSetting/UpdateStatus";
                    break;
                case TAB_GOLDDELUXE:
                    url = "GoldDeluxeBetSetting/UpdateStatus";
                    break;
                case TAB_MACAU:
                    url = "ThirdPartyBetSetting/UpdateMacauStatus";
                    break;
                case TAB_MICROGAMINGLIVECASINO:               
                    url = "ThirdPartyBetSetting/UpdateMicroGamingLiveCasinoStatus";
                    status = $("#enable-product").is(":checked") ? 1 : 0;
                    break;
                case TAB_MICROGAMINGRNGCASINO:
                    url = "ThirdPartyBetSetting/UpdateMicroGamingRNGCasinoStatus";
                    status = $("#enable-product").is(":checked") ? 1 : 0;
                    break;
                case TAB_BETTRADE:
                    url = "ThirdPartyBetSetting/UpdateBetTradeStatus";
                    status = $("#enable-product").is(":checked") ? 1 : 0;
                    break;
                case TAB_VOIDBRIDGE:
                    url = "VoidbridgeBetSetting/UpdateStatus";
                    break;
            }

            $.ajax({
                type: 'POST',
                url: Site.resolveClientUrl(url),
                cache: false,
                data: {
                    nickname: nickName,
                    status: status,
                    custid: custId
                },
                dataType: "json",
                success: function (result) {
                    if (result.IsSuccessful) {
                        BetSetting.showDialogMesage(Site.getSingleResource('MessageSuccess'), result.Message, false);
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
        updateEsportStatus: function () {
            var currentStatus = Site.asBool($(this).attr("data-current-status"));
            var eSportText = $("#esport-text-from-remotingref").val();
            var custId = $(this).data("cust-id");
            var url = "BetSettingAPI/UpdateEsportStatus";
            $.ajax({
                type: 'POST',
                url: Site.resolveClientUrl(url),
                cache: false,
                data: {
                    isDisabled: !currentStatus,
                    custid: custId
                },
                dataType: "json",
                success: function (result) {
                    if (result.Code.toString() === "0") {
                        BetSetting.showDialogMesage(Site.getSingleResource('MessageSuccess'), result.Message, false);
                        $("#btnDisableEsports").attr("data-current-status", !currentStatus);
                        $("#btnDisableEsports").val(Site.getSingleResource(!currentStatus ? 'Enable' : 'Disable') + " " + eSportText);
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
            if (tabId === TAB_LIVECASINO) {
                BetSetting.checkEnableLiveCasino(enable);
                return;
            }

            BetSetting.checkEnable(_sportIds, enable);

            if (tabId === TAB_NUMBERGAME) {
                $("#max-per-ball-161").prop('disabled', !enable);
            } else if (tabId === TAB_SPORTSBOOK) {
                if (!enable) {
                    $('#copy-settting').hide();
                }
            }
        },
        checkEnable: function (sportIds, enable) {
            for (var i in sportIds) {
                var sportId = sportIds[i];
                $("#min-bet-" + sportId + "," + "#max-bet-" + sportId + "," + "#max-per-match-" + sportId).prop("disabled", !enable);
            }

            if (!enable) {
                $("#btnSubmitBetSetting, #btnReset").prop('disabled', true).addClass("button-disabled");
            }
        },
        checkEnableLiveCasino: function (enable) {
            var gameTypeCount = $("#gameTypeCount").val();
            if (!enable) {
                for (var i = 1; i <= gameTypeCount; i++) {
                    $("input[name=gameTypeName" + i + "]").prop('disabled', true);
                }
                $("#btnSubmitBetSetting, #btnReset").prop('disabled', true).addClass("button-disabled");
            }
        },
        updateBetSetting: function () {
            var custId, userName, nickName, productId;
            productId = parseInt($("#ProductId").val());
            var isValid = productId === TAB_LIVECASINO ? true :
                         (productId === TAB_NUMBERGAME ? BetSetting.checkValidationNumberGame() : BetSetting.checkValidation());
            if (!isValid) {
                return;
            }

            custId = $(this).attr("data-cust-id");
            userName = $(this).attr("data-user-name");
            nickName = $(this).attr("data-nick-name");

            var showComfirm = function (resourceMessage, callback) {
                BetSetting.showConfirmAction(Site.getSingleResource(resourceMessage),
                            custId, userName, nickName, callback);
            };

            switch (productId) {
                case TAB_SPORTSBOOK:
                    showComfirm('UpdateSportsbookBetSettings', BetSetting.updateSportsbookBetSetting);
                    break;
                case TAB_RACING:
                    showComfirm('UpdateRacingBetSettings', BetSetting.updateRacingBetSetting);
                    break;
                case TAB_NUMBERGAME:
                    showComfirm('UpdateNumberGameBetSettings', BetSetting.updateNumberGameBetSetting);
                    break;
                case TAB_VIRTUALSPORTS:
                    showComfirm('UpdateVirtualSportsBetSettings', BetSetting.updateVirtualSportsBetSetting);
                    break;
                case TAB_LIVECASINO:
                    showComfirm('UpdateLiveCasinoBetSettings', BetSetting.updateLiveCasinoBetSetting);
                    break;
                case TAB_KENO:
                    showComfirm('UpdateKenoBetSettings', BetSetting.updateKenoBetSetting);
                    break;
                case TAB_AGCASINO:
                    var group = $("#betSettingGroup").val();
                    if (group === null || group === "") {
                        return;
                    }

                    if (AgCasino.checkApiContainGroup(group) === false) {
                        alert(Site.getSingleResource("AGCasinoNoGroupChosenErrMsg"));
                        return;
                    }

                    showComfirm('UpdateAgCasinoBetSettings', BetSetting.updateAgCasinoBetSetting);
                    break;
                case TAB_GOLDDELUXE:
                    group = $("#betSettingGroup").val();
                    if (group === null || group === "") {
                        return;
                    }

                    if (GoldDeluxe.checkApiContainGroup(group) === false) {
                        alert(Site.getSingleResource("AGCasinoNoGroupChosenErrMsg"));
                        return;
                    }

                    showComfirm('UpdateGoldDeluxeBetSettings', BetSetting.updateGoldDeluxeBetSetting);
                    break;
            }
        },
        checkValidation: function () {
            var $checkbox = $('#chkCopyAll');
            if ($checkbox.length > 0) {
                BetSetting.checkCopyAllValues($checkbox);
            }
            return BetSetting.checkLimitMinMaxBets(_sportIds);
        },
        checkValidationNumberGame: function () {
            if (false === BetSetting.checkLimitMinMaxBet(161)) { return false; }

            var maxPerBall = BetSetting.convertToFloat($("#max-per-ball-161").val());
            var maxPerBallLimit = BetSetting.convertToFloat($("#max-per-ball-161").attr('data-max-value'));

            if (isNaN(maxPerBall)) {
                BetSetting.showBalloonMsg("#max-per-ball-161", Site.getSingleResource('IncorrectMaxPerBall'));
                return false;
            }

            if (maxPerBallLimit > 0 && maxPerBall > maxPerBallLimit) {
                BetSetting.showBalloonMsg("#max-per-ball-161", Site.getSingleResource('MaxPerBallLessThanOrEqual') + " " + maxPerBallLimit);
                return false;
            }

            return true;
        },
        updateGoldDeluxeBetSetting: function (custId, userName, nickName) {
            var params = {
                custid: custId,
                userName: userName,
                nickName: nickName,
                group: $("#betSettingGroup").val()
            };

            $.ajax({
                type: 'POST',
                cache: false,
                url: Site.resolveClientUrl('GoldDeluxeBetSetting/UpdateBetSetting'),
                data: params,
                dataType: "json",
                success: function (result) {
                    BetSetting.showUpdateSettingResult(result);
                }
            });
        },
        updateAgCasinoBetSetting: function (custId, userName, nickName) {
            var params = {
                custid: custId,
                userName: userName,
                nickName: nickName,
                group: $("#betSettingGroup").val()
            };

            $.ajax({
                type: 'POST',
                cache: false,
                url: Site.resolveClientUrl('AgCasinoBetSetting/UpdateBetSetting'),
                data: params,
                dataType: "json",
                success: function (result) {
                    BetSetting.showUpdateSettingResult(result);
                }
            });
        },
        updateKenoBetSetting: function (custId, userName, nickName) {
            var params = BetSetting.getBetSettingParamsForSports(_sportIds);
            params.custid = custId;
            params.userName = userName;
            params.nickName = nickName;
            params.id = _sportIds.toString();

            $.ajax({
                type: 'POST',
                cache: false,
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
            var params = BetSetting.getBetSettingParamsForSports(_sportIds);
            params.custid = custId;
            params.userName = userName;
            params.nickName = nickName;

            $.ajax({
                type: 'POST',
                cache: false,
                url: Site.resolveClientUrl('BetSettingAPI/UpdateSportBookBetSetting'),
                data: params,
                dataType: "json",
                success: function (result) {
                    BetSetting.showUpdateSettingResult(result);
                }
            });
        },

        // Update Racing
        updateRacingBetSetting: function (custId, userName, nickName) {
            var params = BetSetting.getBetSettingParamsForSports(_sportIds);
            params.custid = custId;
            params.userName = userName;
            params.nickName = nickName;

            $.ajax({
                type: 'POST',
                url: Site.resolveClientUrl('BetSettingAPI/updateRacingBetSetting?'),
                cache: false,
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

            var params = BetSetting.getBetSettingParams(161);
            params.maxperball = maxperball;
            params.custid = custId;
            params.userName = userName;
            params.nickName = nickName;

            $.ajax({
                type: 'POST',
                url: Site.resolveClientUrl('NumberGameBetSetting/UpdateBetSetting'),
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
            var params = BetSetting.getBetSettingParamsForSports(_sportIds);
            params.custid = custId;
            params.userName = userName;
            params.nickName = nickName;

            $.ajax({
                type: 'POST',
                url: Site.resolveClientUrl('BetSettingAPI/updateVirtualSportsBetSetting?'),
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
            var params = {};
            params.custId = custId;
            params.nickName = nickName;
            params.gameTypeCount = gameTypeCount;
            for (var i = 1; i <= gameTypeCount; i++) {
                var gametypeId = $("#gameTypeId" + i);
                var stakeLevelNameChecked = $("#stakeLevelName" + i);
                var betTypeId = $("#betTypeId" + i);
                var stakeId = $("input[name=gameTypeName" + i + "]:checked").attr("data-stake-id");

                params["stakeId" + i] = stakeId;
                params["gameTypeId" + i] = gametypeId.val();
                params["stakeLevelName" + i] = stakeLevelNameChecked.val();
                params["betTypeId" + i] = betTypeId.val();
            }
            $.ajax({
                type: 'POST',
                url: Site.resolveClientUrl('BetSettingAPI/updateLiveCasinoBetSetting?'),
                cache: false,
                data: params,
                dataType: "json",
                success: function (result) {
                    if (result.Code.toString() === "0") {
                        BetSetting.showDialogMesage(Site.getSingleResource('MessageSuccess'), result.Message, false);
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
        getBetSettingParamsForSports: function (sportsIds) {
            var params = {};
            for (var i in sportsIds) {
                params = $.extend(params, BetSetting.getBetSettingParams(sportsIds[i]));
            }

            return params;
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

            var result = {};
            result["minbet" + sportId] = minBet;
            result["maxbet" + sportId] = maxBet;
            result["maxpermatch" + sportId] = maxPerMatcth;
            result["update" + sportId] = isUpdate;

            return result;
        },

        // Reset bet setting
        resetBetSetting: function () {
            var productId = parseInt($("#ProductId").val());

            switch (productId) {
                case TAB_LIVECASINO:
                    BetSetting.resetLiveCasinoBetSetting();
                    break;
                case TAB_AGCASINO:
                    BetSetting.resetAgCasinoSetting();
                    break;
                case TAB_GOLDDELUXE:
                    BetSetting.resetGoldDeluxeSetting();
                    break;
                case TAB_BETTRADE:
                    BetSetting.resetBetTradeSetting();
                    break;
                case TAB_MICROGAMINGLIVECASINO:
                case TAB_MICROGAMINGRNGCASINO:
                    BetSetting.resetMicrogamingSetting();
                    break;
                default:
                    BetSetting.resetValues();
                    break;
            }
        },

        resetValues: function () {
            $('#chkCopyAll').removeAttr('checked');
            $('input[id^="min"], input[id^="max-bet"], input[id^="max-per"]').each(function () {
                $(this).val($(this).attr('data-original')).prop('disabled', false);
            });
        },
        resetBetTradeSetting: function () {
            var $checkbox = $("#enable-product");
            $checkbox.prop("checked", $checkbox.data("original") === 1);
        },
        resetMicrogamingSetting: function () {
            var $checkbox = $("#enable-product");
            $checkbox.prop("checked", $checkbox.data("original") === 1);
        },
        resetGoldDeluxeSetting: function () {
            var $dropdown = $("#betSettingGroup");
            if ($dropdown.data("default") === "") {
                $dropdown.prop("selectedIndex", -1).change();
            } else {
                $dropdown.val($dropdown.data("default")).change();
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
        resetLiveCasinoBetSetting: function () {
            var gameTypeCount = $("#gameTypeCount").val();

            for (var i = 1; i <= gameTypeCount; i++) {
                var oldCheck = $("#oldCheck" + i).val();
                $("input[name=gameTypeName" + i + "][data-stake-id=" + oldCheck + "]").prop("checked", true);
            }
        },

        checkCopyAllValues: function ($checkbox) {
            var arrayItemMinBet = $('input[id^="min"]');
            var arrayItemMaxBet = $('input[id^="max-bet"]');
            var arrayItemMaxPerBet = $('input[id^="max-per"]');
            var enable = $checkbox.is(":checked");

            BetSetting.setCopyValueBetSetting(arrayItemMinBet, enable);
            BetSetting.setCopyValueBetSetting(arrayItemMaxBet, enable);
            BetSetting.setCopyValueBetSetting(arrayItemMaxPerBet, enable);
        },

        setCopyValueBetSetting: function ($element, enabled) {
            var copyValue = $element.first().val();
            // Check tab ids are sportsbook or racing
            var productId = parseInt($("#ProductId").val());

            // Enable or disable textbox, except the first
            $element.not(':eq(0)').each(function () {
                $(this).prop('disabled', enabled);
            });

            // Check limit min max bet
            if (productId === TAB_SPORTSBOOK) {
                if (!BetSetting.checkLimitMinMaxBet(1)) {
                    return false;
                }
            }
            else if (productId === TAB_KENO) {
                if (!BetSetting.checkLimitMinMaxBet(_sportIds[0])) {
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
                $element.not(':eq(0)').each(function () {
                    $(this).val(copyValue);
                });
            }
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
        checkLimitMinMaxBets: function (sportIds) {
            for (var i in sportIds) {
                if (BetSetting.checkLimitMinMaxBet(sportIds[i]) === false) {
                    return false;
                }
            }

            return true;
        },
        checkLimitMinMaxBet: function (sportId) {
            var minBetId = "#min-bet-" + sportId;
            var maxBetId = "#max-bet-" + sportId;
            var maxPerMatchId = "#max-per-match-" + sportId;

            var minBetTextBox = $(minBetId);
            var maxBetTextBox = $(maxBetId);
            var maxPerMatchTextBox = $(maxPerMatchId);
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
        showBalloonMsg: function (elementSelector, strMsg) {
            var contents = '<div onclick="hideBalloonMsg(\'' + elementSelector + '\')" style="width:140px;" class="msgerr">' + strMsg + '</div>';

            $(elementSelector).blur(function () { BetSetting.hideBalloonMsg(elementSelector); })
                              .focus()
                              .showBalloon({
                                  contents: contents
                              });
        },
        hideBalloonMsg: function (elementSelector) {
            $(elementSelector).hideBalloon();
        },
        showDialogMesage: function (title, message, closePopup) {
            var ok = Site.getSingleResource("OK");
            var options = {
                closeOnEscapeType: true,
                model: true,
                resizable: false,
                height: 150,
                open: function () {
                    $('#btnSubmitBetSetting, #btnReset').prop('disabled', true);
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
                    } else {
                        setTimeout(function () {
                            Site.blockUI();
                            window.location.reload();
                        }, 500);
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
                    $('#btnSubmitBetSetting, #btnReset').prop('disabled', true);
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
            if (Number(result.Code) === 0) {
                BetSetting.showDialogMesage(Site.getSingleResource('MessageSuccess'), result.Message, false);
            }
            else {
                BetSetting.showDialogMesage(Site.getSingleResource('Fail'), result.Message, false);
            }
        }
    };
})(jQuery);

$(window).load(function () {
    BetSetting.initialize();

    $(document).click(function () {
        $('.wrapper-dropdown-5').removeClass('active');
    });
});