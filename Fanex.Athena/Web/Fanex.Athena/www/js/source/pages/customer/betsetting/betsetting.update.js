"use strict";
define(["jquery", "common/site", "./constant", "./betsetting.validation", "./betsettingbygroup", "./betsetting.helper", "common/popup"],
    function ($, Site, Constant, BetSettingValidation, BetSettingByGroup, BetSettingHelper, Popup) {
        var Product = Constant.Product;
        var _sportIds;

        var _me = {
            initialize: function (sportIds) {
                _sportIds = sportIds;
                BetSettingValidation.initialize(sportIds);
            },

            toggleCopyAllValue: function ($checkbox) {
                BetSettingValidation.checkCopyAllValues($checkbox);
            },

            update: function () {
                var custId, userName, nickName;
                var productId = parseInt($("#ProductId").val());
                var isValid = productId === Product.LiveCasino ? true :
                             (productId === Product.NumberGame ? BetSettingValidation.checkValidationNumberGame() : BetSettingValidation.checkValidation());
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
                    case Product.Sportsbook:
                        showComfirm('UpdateSportsbookBetSettings', _me.updateSportsbookBetSetting);
                        break;
                    case Product.Racing:
                        showComfirm('UpdateRacingBetSettings', _me.updateRacingBetSetting);
                        break;
                    case Product.NumberGame:
                        showComfirm('UpdateNumberGameBetSettings', _me.updateNumberGameBetSetting);
                        break;
                    case Product.VirtualSports:
                        showComfirm('UpdateVirtualSportsBetSettings', _me.updateVirtualSportsBetSetting);
                        break;
                    case Product.LiveCasino:
                        showComfirm('UpdateLiveCasinoBetSettings', _me.updateLiveCasinoBetSetting);
                        break;
                    case Product.Keno:
                        showComfirm('UpdateKenoBetSettings', _me.updateKenoBetSetting);
                        break;
                    case Product.AgCasino:
                        var isValid = _me.checkBetSettingByGroup();
                        if (!isValid) {
                            return;
                        }

                        showComfirm('UpdateAgCasinoBetSettings', _me.updateAgCasinoBetSetting);
                        break;
                    case Product.GoldDeluxe:
                        var isValid = _me.checkBetSettingByGroup();
                        if (!isValid) {
                            return;
                        }

                        showComfirm('UpdateGoldDeluxeBetSettings', _me.updateGoldDeluxeBetSetting);
                        break;
                }
            },

            checkBetSettingByGroup: function () {
                var group = $("#betSettingGroup").val();
                if (!group) {
                    return false;
                }

                if (BetSettingByGroup.checkApiContainGroup(group) === false) {
                    alert(Site.getSingleResource("AGCasinoNoGroupChosenErrMsg"));
                    return false;
                }

                return true;
            },

            updateSportsbookBetSetting: function (custId, userName, nickName) {
                var params = _me.getBetSettingParamsForSports();
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

            updateRacingBetSetting: function (custId, userName, nickName) {
                var params = _me.getBetSettingParamsForSports();
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

            updateNumberGameBetSetting: function (custId, userName, nickName) {
                var maxperball = BetSettingHelper.convertToFloat($("#max-per-ball-161").val());

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

            updateVirtualSportsBetSetting: function (custId, userName, nickName) {
                var params = _me.getBetSettingParamsForSports();
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

            updateLiveCasinoBetSetting: function (custId, userName, nickName) {
                var gameTypeCount = $("#gameTypeCount").val();
                var params = {};
                params.custId = custId;
                params.nickName = nickName;
                params.gameTypeCount = gameTypeCount;

                for (var i = 1; i <= gameTypeCount; i++) {
                    var $gametypeId = $("#gameTypeId" + i);
                    var $stakeLevelNameChecked = $("#stakeLevelName" + i);
                    var $betTypeId = $("#betTypeId" + i);
                    var stakeId = $("input[name=gameTypeName" + i + "]:checked").data("stake-id");

                    params["stakeId" + i] = stakeId;
                    params["gameTypeId" + i] = $gametypeId.val();
                    params["stakeLevelName" + i] = $stakeLevelNameChecked.val();
                    params["betTypeId" + i] = $betTypeId.val();
                }

                $.ajax({
                    type: 'POST',
                    url: Site.resolveClientUrl('LiveCasinoBetSetting/updateBetSetting'),
                    cache: false,
                    data: params,
                    dataType: "json",
                    success: function (result) {
                        _me.showUpdateSettingResult(result);
                    },
                    error: function (request, status, err) {
                        BetSettingHelper.showDialogMesage(Site.getSingleResource("Error"), err, false);
                    }
                });
            },

            updateKenoBetSetting: function (custId, userName, nickName) {
                var params = _me.getBetSettingParamsForSports();
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

            getBetSettingParamsForSports: function () {
                var params = {};
                for (var i = 0; i < _sportIds.length; i++) {
                    $.extend(params, _me.getBetSettingParams(_sportIds[i]));
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
                var minBet = BetSettingHelper.convertToFloat(minBetId.val());
                var maxBet = BetSettingHelper.convertToFloat(maxBetId.val());
                var maxPerMatcth = BetSettingHelper.convertToFloat(maxPerMatchId.val());

                var result = {};
                result["minbet" + sportId] = minBet;
                result["maxbet" + sportId] = maxBet;
                result["maxpermatch" + sportId] = maxPerMatcth;
                result["update" + sportId] = isUpdate;

                return result;
            },

            showConfirmAction: function (message, custId, userName, nickName, callback) {
                var options = {
                    title: Site.getSingleResource("Confirmation"),
                    height: "auto",
                    id: "confirm-update-betsetting",
                    onOpen: function () {
                        $('#btnSubmitBetSetting, #btnReset').prop('disabled', true);
                    }
                };

                var buttons = [
                    {
                        text: Site.getSingleResource("OK"),
                        isClosedOnClick: true,
                        click: function () {
                            callback(custId, userName, nickName);
                        },
                        classes: "btn btn-submit"
                    }, {
                        text: Site.getSingleResource("Cancel"),
                        isClosedOnClick: true,
                        click: function () {
                            $('#btnSubmitBetSetting, #btnReset').removeAttr('disabled');
                            $("#confirm-update-betsetting").remove();
                        },
                        classes: "btn btn-cancel"
                    }];

                Popup.openPopupWithButton('<div><p>' + message + '</p></div>', options, buttons);
            },

            showUpdateSettingResult: function (result) {
                var messageResource = Number(result.Code) === 0 ? "MessageSuccess" : "Fail";
                BetSettingHelper.showDialogMesage(Site.getSingleResource(messageResource), result.Message, false);
            }
        };

        return _me;
    });