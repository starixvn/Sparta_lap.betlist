/*global Site*/
/// <reference path="/@JSense.js" />
"use strict";
var RnGCasino = (function ($) {
    return {
        initialize: function () {
            RnGCasino.initEvent();
        },
        initEvent: function () {
            // Submit click
            $('#btnRngCasino').on('click', function () {
                var custId = $(this).attr('data-cust-id');
                var userName = $(this).attr('data-user-name');
                var nickName = $(this).attr('data-nick-name');
                RnGCasino.initParams(custId, userName, nickName);
            });
            // Reset click
            $('#btnResetRngCasino').on('click', function () {
                RnGCasino.resetBettingLimit();
            });
        },
        resetBettingLimit: function () {
            $("input[type=checkbox]").each(function () {
                this.checked = $(this).data("ischeck");
            });
        },
        initParams: function (custId, userName, nickName) {
            var gameTypeChangedList = '';
            var params = {
                custId: custId
            };

            $('.casino-row').each(function () {
                var gameTypeId = $(this).attr('data-gametypeid');

                // Original stake level list
                var originalStakeLevelList = $(this).attr('data-stake-original');

                // New stakelevel
                var newStakeLevelList = '';

                // Get stake level list on selecting
                $('.input-' + gameTypeId).each(function () {
                    if ($(this).is(':checked')) {
                        newStakeLevelList += ',' + $(this).attr('data-stakeid');
                    }
                });

                if (newStakeLevelList.length > 0) {
                    newStakeLevelList = newStakeLevelList.substring(1);
                }

                // Compare stake
                if (RnGCasino.compareStake(originalStakeLevelList, newStakeLevelList)) {
                    gameTypeChangedList += ',' + gameTypeId.toString();
                    params['stake' + gameTypeId] = newStakeLevelList;
                }
            });

            // Get game type changed list
            if (gameTypeChangedList.length > 0) {
                gameTypeChangedList = gameTypeChangedList.substring(1);
            }

            // Get params changed list
            params.gameTypeChangedList = gameTypeChangedList;            

            RnGCasino.showConfirmAction(Site.getSingleResource('UpdateRNGCasinoBetSettings'), custId, userName, nickName,
                        params, gameTypeChangedList, RnGCasino.updateStakeLevel);
        },
        updateStakeLevel: function (userName, params, gameTypeChangedList) {
            var url = Site.resolveClientUrl('BetsettingAPI/UpdateRngCasinoBetSetting');
            if (gameTypeChangedList.length > 0) {
                $.ajax({
                    url: url,
                    type: 'POST',
                    cache: false,
                    data: params,
                    dataType: 'json',
                    success: function (result) {
                        switch (result.Code) {
                            case 1:
                                result.Message = Site.getSingleResource('RNGCasinoUpdateSettingUnAvailableMsg');
                                break;
                            case 0:
                                result.Message = Site.getSingleResource('UpdateRNGCasinoBetSettingSuccessMsg').replace("{0}", userName);
                                break;
                            default:
                                result.Message = Site.getSingleResource('UpdateRNGCasinoBetSettingFailMsg').replace("{0}", userName);
                        }

                        RnGCasino.showUpdateSettingResult(result);
                    }
                });
            }
            else {
                RnGCasino.showDialogMesage("Success", Site.getSingleResource('UpdateRNGCasinoBetSettingSuccessMsg').replace("{0}", userName), true);
            }
        },
        compareStake: function (orignialStake, newStake) {
            return orignialStake !== newStake;
        },
        showConfirmAction: function (message, custId, userName, nickName, params, gameTypeChangedList, callback) {
            var options = {
                closeOnEscapeType: true,
                model: true,
                resizable: false,
                height: 150,
                open: function () {
                    $('#btnRngCasino, #btnResetRngCasino').attr('disabled', 'disabled');
                },
                buttons: {
                    OK: function () {
                        $("#bet-setting-confirm").dialog("close");
                        callback(userName, params, gameTypeChangedList);
                    },
                    Cancel: function () {
                        $('#btnRngCasino').removeAttr('disabled');
                        $("#bet-setting-confirm").dialog("close");
                    }
                },
                close: function () {
                    $("#bet-setting-confirm").remove();
                    $('#btnRngCasino, #btnResetRngCasino').removeAttr('disabled');
                }
            };

            var title = Site.getSingleResource("Confirmation");
            $('<div id="bet-setting-confirm" title="' + title + '" >' + message + '</div>').dialog(options);
        },
        showDialogMesage: function (title, message, closePopup) {
            var options = {
                closeOnEscapeType: true,
                model: true,
                resizable: false,
                height: 150,
                open: function () {
                    $('#btnRngCasino, #btnResetRngCasino').attr('disabled', 'disabled');
                },
                buttons: {
                    OK: function () {
                        $("#bet-setting-message").dialog("close");
                    }
                },
                close: function () {
                    $('#btnRngCasino, #btnResetRngCasino').removeAttr('disabled');
                    $("#bet-setting-message").remove();
                    if (closePopup) {
                        parent.$("#bet-setting-dialog").dialog("close");
                    }
                }
            };
            $('<div id="bet-setting-message" title="' + title + '">' + message + '</div>').dialog(options);
        },
        showUpdateSettingResult: function (result) {
            if (result.Code.toString() === "0") {
                RnGCasino.showDialogMesage("Success", result.Message, true);
            }
            else {
                RnGCasino.showDialogMesage("Fail", result.Message, false);
            }
        }
    };
})(jQuery);