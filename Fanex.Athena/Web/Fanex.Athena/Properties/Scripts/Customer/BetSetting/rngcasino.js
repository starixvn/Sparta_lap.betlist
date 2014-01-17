/// <reference path="/@JSense.js" />
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
            $('.casino-row').each(function () {
                var gameTypeId = $(this).attr('data-gametypeid');
                $('.input-' + gameTypeId).each(function () {
                    var originalCheck = $(this).attr('data-ischeck') === 'true' ? true : false;
                    $(this).attr('checked', originalCheck);
                });
            });
        },
        initParams: function (custId, userName, nickName) {
            var gameTypeChangedList = '';
            var params = 'custId=' + custId;

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
                    params += '&stake' + gameTypeId + '=' + newStakeLevelList;
                }
            });

            // Get game type changed list
            if (gameTypeChangedList.length > 0) {
                gameTypeChangedList = gameTypeChangedList.substring(1);
            }

            // Get params changed list
            if (params.length > 0) {
                params += '&gameTypeChangedList=' + gameTypeChangedList;
            }

            RnGCasino.showConfirmAction("Are you sure you want to update RnG Casino bet settings?", custId, userName, nickName, params, gameTypeChangedList, RnGCasino.updateStakeLevel);
        },
        updateStakeLevel: function (userName, params, gameTypeChangedList) {
            var url = resolveClientUrl('BetsettingAPI/UpdateRngCasinoBetSetting?');
            if (gameTypeChangedList.length > 0) {
                $.ajax({
                    url: url,
                    type: 'GET',
                    cache: false,
                    data: params,
                    dataType: 'json',
                    success: function (result) {
                        switch (result.Code) {
                            case 1:
                                result.Message = 'RNG Casino Database is not currently available. Please try again later.';
                                break;
                            case 0:
                                result.Message = 'The RNG Casino bet setting of ' + userName + ' was updated successfully';
                                break;
                            default:
                                result.Message = 'The RNG Casino bet setting of ' + userName + ' updated is failed';
                        }

                        RnGCasino.showUpdateSettingResult(result);
                    }
                });
            }
            else {
                RnGCasino.showDialogMesage("Success", 'The RnG Casino bet setting of ' + userName + ' was updated successfully', true);
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
            $('<div id="bet-setting-confirm" title="">' + message + '</div>').dialog(options);
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
            if (result.Code == 0) {
                RnGCasino.showDialogMesage("Success", result.Message, true);
            }
            else {
                RnGCasino.showDialogMesage("Fail", result.Message, false);
            }
        }
    };
})(jQuery);