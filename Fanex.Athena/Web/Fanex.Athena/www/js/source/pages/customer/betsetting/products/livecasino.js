"use strict";
define(["jquery", "common/site", "../common/betsettingbase", "../common/betsetting.status"], function ($, Site, BetSettingBase, BetSettingStatus) {
    var _me = {
        initialize: function () {
            Site.initialize();
            BetSettingBase.initialize();
            _me.initEvent();
            _me.checkEnable();
        },

        initEvent: function () {
            $("#btnReset").on("click", _me.reset);

            $("#btnSubmitBetSetting").on("click", function () {
                BetSettingBase.showComfirm($(this), 'UpdateLiveCasinoBetSettings', _me.updateBetSetting);
            });

            $("#btnUpdateStatus").click(function () {
                _me.updateStatus($(this));
            });
        },

        checkEnable: function () {
            var enable = $("#editable").val() === "True";

            if (!enable) {
                var gameTypeCount = parseInt($("#gameTypeCount").val());
                for (var i = 1; i <= gameTypeCount; i++) {
                    $("input[name=gameTypeName" + i + "]").prop('disabled', true);
                }
                $("#btnSubmitBetSetting, #btnReset").prop('disabled', true).addClass("disabled");
            }
        },

        reset: function () {
            var gameTypeCount = parseInt($("#gameTypeCount").val());

            for (var i = 1; i <= gameTypeCount; i++) {
                var oldCheck = $("#oldCheck" + i).val();
                $("input[name=gameTypeName" + i + "][data-stake-id=" + oldCheck + "]").prop("checked", true);
            }
        },

        updateBetSetting: function (custId, userName, nickName) {
            var url = Site.resolveClientUrl('LiveCasinoBetSetting/UpdateBetSetting');

            var gameTypeCount = parseInt($("#gameTypeCount").val());
            var params = {
                custId: custId,
                nickName: nickName,
                gameTypeCount: gameTypeCount
            };

            for (var i = 1; i <= gameTypeCount; i++) {
                var gametypeId = $("#gameTypeId" + i).val();
                var stakeLevelName = $("#stakeLevelName" + i).val();
                var betTypeId = $("#betTypeId" + i).val();
                var stakeId = $("input[name=gameTypeName" + i + "]:checked").data("stake-id");

                params["stakeId" + i] = stakeId;
                params["gameTypeId" + i] = gametypeId;
                params["stakeLevelName" + i] = stakeLevelName;
                params["betTypeId" + i] = betTypeId;
            }

            BetSettingBase.updateBetSetting(url, params);
        },

        updateStatus: function ($context) {
            var url = Site.resolveClientUrl("LiveCasinoBetSetting/UpdateStatus");
            var params = BetSettingStatus.getCommonPostData($context);

            BetSettingStatus.update(url, params);
        }
    };

    return _me;
});