"use strict";
define(["jquery", "common/site", "../common/betsettingbase", "../common/betsettingbygroup", "../common/betsetting.status"],
    function ($, Site, BetSettingBase, BetSettingByGroup, BetSettingStatus) {
        var _me = {
            initialize: function () {
                Site.initialize();
                BetSettingBase.initialize();
                BetSettingByGroup.initialize();
                _me.initEvent();
            },

            initEvent: function () {
                $("#btnSubmitBetSetting").on("click", function () {
                    var isValid = BetSettingByGroup.isValid();
                    if (!isValid) {
                        return;
                    }

                    BetSettingBase.showComfirm($(this), 'UpdateAgCasinoBetSettings', _me.updateBetSetting);
                });

                $("#btnReset").on("click", function () {
                    _me.resetSetting();
                });

                $("#btnUpdateStatus").click(function () {
                    _me.updateStatus($(this));
                });
            },

            resetSetting: function () {
                var $dropdown = $("#betSettingGroup");
                if ($dropdown.data("default") === "") {
                    $dropdown.prop("selectedIndex", -1).change();
                } else {
                    $dropdown.val($dropdown.data("default")).change();
                }
            },

            updateBetSetting: function (custId, userName, nickName) {
                var url = Site.resolveClientUrl('AgCasinoBetSetting/UpdateBetSetting');

                var params = {
                    custid: custId,
                    userName: userName,
                    nickName: nickName,
                    group: $("#betSettingGroup").val()
                };

                BetSettingBase.updateBetSetting(url, params);
            },

            updateStatus: function ($context) {
                var url = Site.resolveClientUrl("AgCasinoBetSetting/UpdateStatus");
                var params = BetSettingStatus.getCommonPostData($context);
                BetSettingStatus.update(url, params);
            }
        };

        return _me;
    });