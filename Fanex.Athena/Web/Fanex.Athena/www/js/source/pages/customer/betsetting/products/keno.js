"use strict";
define(["jquery", "common/site", "../common/betsettingbase"], function ($, Site, BetSettingBase) {
    var _me = {
        initialize: function () {
            Site.initialize();
            BetSettingBase.initialize();
            _me.initEvent();
        },

        initEvent: function () {
            $("#btnReset").on("click", function () {
                BetSettingBase.resetInput();
            });

            $("#btnSubmitBetSetting").on("click", function () {
                BetSettingBase.showComfirm($(this), 'UpdateKenoBetSettings', _me.updateBetSetting);
            });

            // Formating number in text box.
            BetSettingBase.formatInputText();
        },

        updateBetSetting: function (custId, userName, nickName) {
            var url = Site.resolveClientUrl('KenoBetSetting/UpdateBetSetting');
            var params = BetSettingBase.getBetSettingParamsForSports();
            params.custid = custId;
            params.userName = userName;
            params.nickName = nickName;
            params.id = $("#IDList").val();

            BetSettingBase.updateBetSetting(url, params);
        }
    };

    return _me;
});