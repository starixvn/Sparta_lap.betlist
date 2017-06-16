"use strict";
define(["jquery", "common/site"], function ($, Site) {
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
    var TAB_COLOSSUSBETS = 15;
    var TAB_MICROGAMINGRNGCASINO = 17;
    var TAB_ALLBET = 21;
    var TAB_VOIDBRIDGE = 22;
    var TAB_FISHINGWORLD = 24;
    var TAB_CASHOUT = 25;

    var _sportIds = null;

    var _me = {
        initialize: function () {
            Site.initialize();
            var $idList = $("#IDList");
            if ($idList.length > 0) {
                _sportIds = $idList.val().split(",");
            }

            _me.initMenu();
            //_me.initEvent();
        },
        initEvent: function () {
            // Check data available
            var dataCount = Number($("#dataCount").val());
            if (dataCount === 0) {
                $("#btnSubmitBetSetting, #btnReset").prop('disabled', true).addClass("disabled");
                return;
            }

            // Copy all sportsbook (sportsbook tab)
            $('#chkCopyAll').click(function () {
                _me.checkCopyAllValues($(this));
            });

            // Bind event
            $("#btnSubmitBetSetting").on("click", _me.updateBetSetting);
            $("#btnReset").on("click", _me.resetBetSetting);
            $("#btnUpdateStatus").click(_me.updateStatus);
            $("#btnDisableEsports").on("click", _me.updateEsportStatus);

            // Formating number in text box.
            _me.formatInputText();
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
            var enable = $("#editable").val() === "True" ? true : false;
            $("#product-dropdown").on('change', function (event) {
                var url = $(this).find("option:selected").data("href");
                Site.blockUI();
                Site.redirect(url);
            });
            _me.checkEnableBetSetting(productId, enable);
        },

        checkEnableBetSetting: function (tabId, enable) {
            if (tabId === TAB_LIVECASINO) {
                _me.checkEnableLiveCasino(enable);
                return;
            }

            _me.checkEnable(_sportIds, enable);

            if (tabId === TAB_NUMBERGAME) {
                $("#max-per-ball-161").prop('disabled', !enable);
            } else if (tabId === TAB_SPORTSBOOK) {
                if (!enable) {
                    $('#copy-settting').hide();
                }
            }
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
                    url = "LiveCasinoBetSetting/UpdateStatus";
                    break;
                case TAB_RNGCASINO:
                    url = "RngCasinoBetSetting/UpdateStatus";
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
                case TAB_COLOSSUSBETS:
                    url = "ThirdPartyBetSetting/UpdateColossusBetsStatus";
                    break;
                case TAB_MICROGAMINGRNGCASINO:
                    url = "ThirdPartyBetSetting/UpdateMicroGamingRNGCasinoStatus";
                    status = $("#enable-product").is(":checked") ? 1 : 0;
                    break;
                case TAB_BETTRADE:
                    url = "ThirdPartyBetSetting/UpdateBetTradeStatus";
                    status = $("#enable-product").is(":checked") ? 1 : 0;
                    break;
                case TAB_ALLBET:
                case TAB_VOIDBRIDGE:
                case TAB_FISHINGWORLD:
                    url = "DcsBetSetting/UpdateStatus";
                    break;
                case TAB_CASHOUT:
                    url = "ThirdPartyBetSetting/UpdateCashOutStatus";
                    status = $("#enable-product").is(":checked") ? 1 : 0;
                    break;
            }

            $.ajax({
                type: 'POST',
                url: Site.resolveClientUrl(url),
                cache: false,
                data: {
                    productId: productId,
                    nickname: nickName,
                    status: status,
                    custid: custId
                },
                dataType: "json",
                success: function (result) {
                    if (result.IsSuccessful) {
                        _me.showDialogMesage(Site.getSingleResource('MessageSuccess'), result.Message, false);
                    }
                    else {
                        _me.showDialogMesage(Site.getSingleResource('Fail'), result.Message, false);
                    }
                },
                error: function (request, status, err) {
                    _me.showDialogMesage(Site.getSingleResource("Error"), err, false);
                }
            });
        },
        updateEsportStatus: function () {
            var currentStatus = Site.asBool($(this).attr("data-current-status"));
            var eSportText = $("#esport-text-from-remotingref").val();
            var custId = $(this).data("cust-id");
            var url = Site.resolveClientUrl("SportbooksBetSetting/UpdateEsportStatus");
            $.ajax({
                type: 'POST',
                url: url,
                cache: false,
                data: {
                    isDisabled: !currentStatus,
                    custid: custId
                },
                dataType: "json",
                success: function (result) {
                    if (result.Code.toString() === "0") {
                        _me.showDialogMesage(Site.getSingleResource('MessageSuccess'), result.Message, false);
                        $("#btnDisableEsports").attr("data-current-status", !currentStatus);
                        $("#btnDisableEsports").val(Site.getSingleResource(!currentStatus ? 'Enable' : 'Disable') + " " + eSportText);
                    }
                    else {
                        _me.showDialogMesage(Site.getSingleResource('Fail'), result.Message, false);
                    }
                },
                error: function (request, status, err) {
                    _me.showDialogMesage(Site.getSingleResource("Error"), err, false);
                }
            });
        },
        
        checkEnable: function (sportIds, enable) {
            for (var i in sportIds) {
                var sportId = sportIds[i];
                $("#min-bet-" + sportId + "," + "#max-bet-" + sportId + "," + "#max-per-match-" + sportId).prop("disabled", !enable);
            }

            if (!enable) {
                $("#btnSubmitBetSetting, #btnReset").prop('disabled', true).addClass("disabled");
            }
        },
        checkEnableLiveCasino: function (enable) {
            var gameTypeCount = $("#gameTypeCount").val();
            if (!enable) {
                for (var i = 1; i <= gameTypeCount; i++) {
                    $("input[name=gameTypeName" + i + "]").prop('disabled', true);
                }
                $("#btnSubmitBetSetting, #btnReset").prop('disabled', true).addClass("disabled");
            }
        },
        updateBetSetting: function () {
            var custId, userName, nickName, productId;
            productId = parseInt($("#ProductId").val());
            var isValid = productId === TAB_LIVECASINO ? true :
                         (productId === TAB_NUMBERGAME ? _me.checkValidationNumberGame() : _me.checkValidation());
            if (!isValid) {
                return;
            }

            custId = $(this).attr("data-cust-id");
            userName = $(this).attr("data-user-name");
            nickName = $(this).attr("data-nick-name");

            var showComfirm = function (resourceMessage, callback) {
                _me.showConfirmAction(Site.getSingleResource(resourceMessage),
                            custId, userName, nickName, callback);
            };

            switch (productId) {
                case TAB_SPORTSBOOK:
                    showComfirm('UpdateSportsbookBetSettings', _me.updateSportsbookBetSetting);
                    break;
                case TAB_RACING:
                    showComfirm('UpdateRacingBetSettings', _me.updateRacingBetSetting);
                    break;
                case TAB_NUMBERGAME:
                    showComfirm('UpdateNumberGameBetSettings', _me.updateNumberGameBetSetting);
                    break;
                case TAB_VIRTUALSPORTS:
                    showComfirm('UpdateVirtualSportsBetSettings', _me.updateVirtualSportsBetSetting);
                    break;
                case TAB_LIVECASINO:
                    showComfirm('UpdateLiveCasinoBetSettings', _me.updateLiveCasinoBetSetting);
                    break;
                case TAB_KENO:
                    showComfirm('UpdateKenoBetSettings', _me.updateKenoBetSetting);
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

                    showComfirm('UpdateAgCasinoBetSettings', _me.updateAgCasinoBetSetting);
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

                    showComfirm('UpdateGoldDeluxeBetSettings', _me.updateGoldDeluxeBetSetting);
                    break;
            }
        },
        checkValidation: function () {
            var $checkbox = $('#chkCopyAll');
            if ($checkbox.length > 0) {
                _me.checkCopyAllValues($checkbox);
            }
            return _me.checkLimitMinMaxBets(_sportIds);
        },
        checkValidationNumberGame: function () {
            if (false === _me.checkLimitMinMaxBet(161)) { return false; }

            var maxPerBall = _me.convertToFloat($("#max-per-ball-161").val());
            var maxPerBallLimit = _me.convertToFloat($("#max-per-ball-161").attr('data-max-value'));

            if (isNaN(maxPerBall)) {
                _me.showBalloonMsg("#max-per-ball-161", Site.getSingleResource('IncorrectMaxPerBall'));
                return false;
            }

            if (maxPerBallLimit > 0 && maxPerBall > maxPerBallLimit) {
                _me.showBalloonMsg("#max-per-ball-161", Site.getSingleResource('MaxPerBallLessThanOrEqual') + " " + maxPerBallLimit);
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
                    _me.showUpdateSettingResult(result);
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
                    _me.showUpdateSettingResult(result);
                }
            });
        },
        updateKenoBetSetting: function (custId, userName, nickName) {
            var params = _me.getBetSettingParamsForSports(_sportIds);
            params.custid = custId;
            params.userName = userName;
            params.nickName = nickName;
            params.id = _sportIds.toString();

            $.ajax({
                type: 'POST',
                cache: false,
                url: Site.resolveClientUrl('KenoBetSetting/UpdateBetSetting?'),
                data: params,
                dataType: "json",
                success: function (result) {
                    _me.showUpdateSettingResult(result);
                }
            });
        },
        // Update Sportsbook
        updateSportsbookBetSetting: function (custId, userName, nickName) {
            var params = _me.getBetSettingParamsForSports(_sportIds);
            params.custid = custId;
            params.userName = userName;
            params.nickName = nickName;

            $.ajax({
                type: 'POST',
                cache: false,
                url: Site.resolveClientUrl('SportsbookBetSetting/UpdateBetSetting'),
                data: params,
                dataType: "json",
                success: function (result) {
                    _me.showUpdateSettingResult(result);
                }
            });
        },

        // Update Racing
        updateRacingBetSetting: function (custId, userName, nickName) {
            var params = _me.getBetSettingParamsForSports(_sportIds);
            params.custid = custId;
            params.userName = userName;
            params.nickName = nickName;

            $.ajax({
                type: 'POST',
                url: Site.resolveClientUrl('RacingBetSetting/updateBetSetting'),
                cache: false,
                data: params,
                dataType: "json",
                success: function (result) {
                    _me.showUpdateSettingResult(result);
                }
            });
        },
        // Update Number Game
        updateNumberGameBetSetting: function (custId, userName, nickName) {
            var maxperball = _me.convertToFloat($("#max-per-ball-161").val());

            var params = _me.getBetSettingParams(161);
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
                    _me.showUpdateSettingResult(result);
                }
            });
        },
        // Update Virtual Sports
        updateVirtualSportsBetSetting: function (custId, userName, nickName) {
            var params = _me.getBetSettingParamsForSports(_sportIds);
            params.custid = custId;
            params.userName = userName;
            params.nickName = nickName;

            $.ajax({
                type: 'POST',
                url: Site.resolveClientUrl('VirtualSportsBetSetting/updateBetSetting?'),
                cache: false,
                data: params,
                dataType: "json",
                success: function (result) {
                    _me.showUpdateSettingResult(result);
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
                url: Site.resolveClientUrl('LiveCasinoBetSetting/updateBetSetting'),
                cache: false,
                data: params,
                dataType: "json",
                success: function (result) {
                    if (result.Code.toString() === "0") {
                        _me.showDialogMesage(Site.getSingleResource('MessageSuccess'), result.Message, false);
                    }
                    else {
                        _me.showDialogMesage(Site.getSingleResource('Fail'), result.Message, false);
                    }
                },
                error: function (request, status, err) {
                    _me.showDialogMesage(Site.getSingleResource("Error"), err, false);
                }
            });
        },
        getBetSettingParamsForSports: function (sportsIds) {
            var params = {};
            for (var i in sportsIds) {
                params = $.extend(params, _me.getBetSettingParams(sportsIds[i]));
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
            var minBet = _me.convertToFloat(minBetId.val());
            var maxBet = _me.convertToFloat(maxBetId.val());
            var maxPerMatcth = _me.convertToFloat(maxPerMatchId.val());

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
                    _me.resetLiveCasinoBetSetting();
                    break;
                case TAB_AGCASINO:
                    _me.resetAgCasinoSetting();
                    break;
                case TAB_GOLDDELUXE:
                    _me.resetGoldDeluxeSetting();
                    break;
                case TAB_BETTRADE:
                    _me.resetBetTradeSetting();
                    break;
                case TAB_MICROGAMINGLIVECASINO:
                case TAB_MICROGAMINGRNGCASINO:
                    _me.resetMicrogamingSetting();
                    break;
                case TAB_CASHOUT:
                    _me.resetCashOutSetting();
                    break;
                default:
                    _me.resetValues();
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
        resetCashOutSetting: function () {
            var $checkbox = $("#enable-product");
            $checkbox.prop("checked", $checkbox.data("original") === 1);
        },
        checkCopyAllValues: function ($checkbox) {
            var arrayItemMinBet = $('input[id^="min"]');
            var arrayItemMaxBet = $('input[id^="max-bet"]');
            var arrayItemMaxPerBet = $('input[id^="max-per"]');
            var enable = $checkbox.is(":checked");

            _me.setCopyValueBetSetting(arrayItemMinBet, enable);
            _me.setCopyValueBetSetting(arrayItemMaxBet, enable);
            _me.setCopyValueBetSetting(arrayItemMaxPerBet, enable);
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
                if (!_me.checkLimitMinMaxBet(1)) {
                    return false;
                }
            }
            else if (productId === TAB_KENO) {
                if (!_me.checkLimitMinMaxBet(_sportIds[0])) {
                    return false;
                }
            }
            else {
                if (!_me.checkLimitMinMaxBet('151_41')) {
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
                if (_me.checkLimitMinMaxBet(sportIds[i]) === false) {
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
            var limitMinBet = _me.convertToFloat(minBetTextBox.attr('data-min-value'));
            var limitMaxBet = _me.convertToFloat(maxBetTextBox.attr('data-max-value'));
            var limitMaxPerMatch = _me.convertToFloat(maxPerMatchTextBox.attr('data-max-value'));
            var limitMaxPerMatchMin = _me.convertToFloat(maxPerMatchTextBox.attr('data-min-value'));

            var minBet = _me.convertToFloat(minBetTextBox.val());
            var maxBet = _me.convertToFloat(maxBetTextBox.val());
            var maxPerMatcth = _me.convertToFloat(maxPerMatchTextBox.val());

            // Check if minbet, maxbet, maxpermatch is valid number
            if (isNaN(minBet)) {
                _me.showBalloonMsg(minBetId, Site.getSingleResource('IncorrectMinBet'));
                return false;
            }
            if (isNaN(maxBet)) {
                _me.showBalloonMsg(maxBetId, Site.getSingleResource('IncorrectMaxBet'));
                return false;
            }
            if (isNaN(maxPerMatcth)) {
                _me.showBalloonMsg(maxPerMatchId, Site.getSingleResource('IncorrectMaxPerMatch'));
                return false;
            }

            if (minBet === 0 && maxBet === 0 && maxPerMatcth === 0) { return true; }

            // Check contrains
            // min bet <=maxbet <= max per match
            if (limitMinBet > 0 && minBet < _me.convertToFloat(limitMinBet.toString())) {
                _me.showBalloonMsg(minBetId, Site.getSingleResource('MinBetGreaterThanOrEqual') + " " + limitMinBet);
                return false;
            }
            if (limitMaxBet > 0 && maxBet > _me.convertToFloat(limitMaxBet.toString())) {
                _me.showBalloonMsg(maxBetId, Site.getSingleResource('MaxBetLessThanOrEqual') + " " + limitMaxBet);
                return false;
            }
            if (limitMaxPerMatchMin > 0 && maxPerMatcth < _me.convertToFloat(limitMaxPerMatchMin.toString())) {
                _me.showBalloonMsg(maxPerMatchId, Site.getSingleResource('MaxBetMatchMoreThanOrEqual') + " " + limitMaxPerMatchMin);
                return false;
            }
            if (limitMaxPerMatch > 0 && maxPerMatcth > _me.convertToFloat(limitMaxPerMatch.toString())) {
                _me.showBalloonMsg(maxPerMatchId, Site.getSingleResource('MaxBetMatchLessThanOrEqual') + " " + limitMaxPerMatch);
                return false;
            }
            if (minBet > maxBet) {
                _me.showBalloonMsg(minBetId, Site.getSingleResource('MinBetCanNotGreaterThanMaxBet'));
                return false;
            }
            if (maxBet > maxPerMatcth) {
                _me.showBalloonMsg(maxBetId, Site.getSingleResource('MaxBetCanNotGreaterThanMaxPerMatch'));
                return false;
            }

            return true;
        },
        convertToFloat: function (str) {
            var temp = str.replace(/,/g, "");
            var temp2 = temp.replace(".", "");

            if (_me.isInteger(temp2)) {
                return parseFloat(temp);
            }
            else {
                return NaN;
            }
        },
        isInteger: function (s) {
            var i;

            if (_me.isEmpty(s)) {
                return false;
            }
            else {
                for (i = 0; i < s.length; i++) {
                    var c = s.charAt(i);

                    if (!_me.isDigit(c)) { return false; }
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

            $(elementSelector).blur(function () { _me.hideBalloonMsg(elementSelector); })
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
                _me.showDialogMesage(Site.getSingleResource('MessageSuccess'), result.Message, false);
            }
            else {
                _me.showDialogMesage(Site.getSingleResource('Fail'), result.Message, false);
            }
        }
    };

    return _me;
});