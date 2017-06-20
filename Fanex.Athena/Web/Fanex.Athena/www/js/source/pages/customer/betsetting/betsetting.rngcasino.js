define(["jquery", "common/site", "common/popup"], function ($, Site, Popup) {
    "use strict";

    function initialize() {
        _me.initEvent();
        var isEnabled = $("#isenabled").val() === "True";

        if (!isEnabled) {
            $("#btnRngCasino, #btnResetRngCasino").prop("disabled", true);
            $(".body-content input").prop("disabled", true);
        }
    }

    function initEvent() {
        $('#btnRngCasino').on('click', function () {
            var custId = $(this).attr('data-cust-id');
            var userName = $(this).attr('data-user-name');
            var nickName = $(this).attr('data-nick-name');
            _me.initParams(custId, userName, nickName);
        });

        $('#btnResetRngCasino').on('click', function () {
            _me.resetBettingLimit();
        });
    }

    function resetBettingLimit() {
        $("input[type=checkbox]").each(function () {
            this.checked = $(this).data("ischeck");
        });
    }

    function initParams(custId, userName, nickName) {
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
            if (compareStake(originalStakeLevelList, newStakeLevelList)) {
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

        showConfirmAction(Site.getSingleResource('UpdateRNGCasinoBetSettings'), custId, userName, nickName,
                    params, gameTypeChangedList, _me.updateStakeLevel);
    }

    function updateStakeLevel(userName, params, gameTypeChangedList) {
        var url = Site.resolveClientUrl('RngCasinoBetsetting/UpdateBetSetting');
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

                    showUpdateSettingResult(result);
                }
            });
        }
        else {
            showDialogMesage("Success", Site.getSingleResource('UpdateRNGCasinoBetSettingSuccessMsg').replace("{0}", userName), false);
        }
    }

    function compareStake(orignialStake, newStake) {
        return orignialStake !== newStake;
    }

    function showConfirmAction(message, custId, userName, nickName, params, gameTypeChangedList, callback) {
        var options = {
            modal: true,
            height: 150,
            id: "bet-setting-confirm",
            title: Site.getSingleResource("Confirmation"),
            onOpen: function () {
                $('#btnRngCasino, #btnResetRngCasino').attr('disabled', 'disabled');
            },
            onClose: function () {
                $('#btnRngCasino, #btnResetRngCasino').removeAttr('disabled');
                $("#bet-setting-confirm").remove();
            }
        };
        var buttons = [
        {
            text: Site.getSingleResource("OK"),
            click: function () { 
                callback(userName, params, gameTypeChangedList);
            },
            classes: "btn btn-submit",
            isClosedOnClick: true
        },
        {
            text: Site.getSingleResource("Cancel"),
            click: function () { 
                $('#btnRngCasino').removeAttr('disabled');
            },
            classes: "btn btn-cancel",
            isClosedOnClick: true
        }];

        Popup.openPopupWithButton('<div><div>' + message + '</div></div>', options, buttons);
    }

    function showDialogMesage(title, message, reloadPage) {
        var options = {
            modal: true,
            height: 150,
            id: "bet-setting-message",
            title: title,
            onOpen: function () {
                $('#btnRngCasino, #btnResetRngCasino').attr('disabled', 'disabled');
            },
            onClose: function () {
                $('#btnRngCasino, #btnResetRngCasino').removeAttr('disabled');
                $("#bet-setting-message").remove();
                if (reloadPage) {
                    setTimeout(function () {
                        Site.blockUI();
                        Site.reload();
                    }, 500);
                }
            }
        };
        var buttons = [
        {
            text: Site.getSingleResource("OK"),
            classes: "btn btn-submit",
            isClosedOnClick: true
        }];

        Popup.openPopupWithButton('<div><div>' + message + '</div></div>', options, buttons);
    }

    function showUpdateSettingResult(result) {
        showDialogMesage(result.Code.toString() === "0" ? "Success" : "Fail", result.Message, true);
    }

    var _me = {
        initialize: initialize,
        initEvent: initEvent,
        updateStakeLevel: updateStakeLevel,
        resetBettingLimit: resetBettingLimit,
        initParams: initParams
    };

    return _me;
});