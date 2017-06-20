"use strict";
define(["jquery", "common/site", "../common/betsettingbase", "../common/betsetting.copy"], function ($, Site, BetSettingBase, BetSettingCopy) {
    var _me = {
        initialize: function () {
            Site.initialize();
            BetSettingBase.initialize();
            BetSettingBase.setEnable();
            _me.initEvent();
        },

        initEvent: function () {
            // Copy all sportsbook (sportsbook tab)
            $('#chkCopyAll').click(function () {
                BetSettingCopy.checkCopyAllValues($(this));
            });

            // Bind event
            $("#btnReset").on("click", function () {
                BetSettingBase.resetInput();
            });

            $("#btnSubmitBetSetting").on("click", function () {
                BetSettingBase.showComfirm($(this), 'UpdateSportsbookBetSettings', _me.updateBetSetting);
            });

            // Formating number in text box.
            BetSettingBase.formatInputText();
        },

        updateBetSetting: function (custId, userName, nickName) {
            var url = Site.resolveClientUrl('SportsbookBetSetting/UpdateBetSetting');

            var params = BetSettingBase.getBetSettingParamsForSports();
            params.custid = custId;
            params.userName = userName;
            params.nickName = nickName;

            BetSettingBase.updateBetSetting(url, params);
        }
    };

    return _me;
});