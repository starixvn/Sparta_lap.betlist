/* AllBet, VoidBridge, FishingWorld */
"use strict";
define(["jquery", "common/site", "../common/betsettingbase", "../common/betsetting.status"],
    function ($, Site, BetSettingBase, BetSettingStatus) {
        var _me = {
            initialize: function () {
                Site.initialize();
                BetSettingBase.initialize();
                _me.initEvent();
            },

            initEvent: function () {
                $("#btnUpdateStatus").click(function () {
                    _me.updateStatus($(this));
                });
            },

            updateStatus: function ($context) {
                var url = Site.resolveClientUrl("DcsBetSetting/UpdateStatus");
                var params = BetSettingStatus.getCommonPostData($context);
                BetSettingStatus.update(url, params);
            }
        };

        return _me;
    });