"use strict";
define(["jquery", "common/site", "lib/numberformat"], function ($, Site, NumberFormatter) {
    var _bettingGroupLimitCache;
    var _formatter = new NumberFormatter();

    function disableSubmitButtons(isDisabled) {
        var $button = $("#btnSubmitBetSetting, #btnReset").prop('disabled', isDisabled);
        $button.toggleClass("disabled", isDisabled);
    }

    var _me = {
        initialize: function () {
            _me.initEvent();
            _bettingGroupLimitCache = JSON.parse($("#BettingLimit").val());

            if ($('#betSettingGroup').data("default") === "") {
                $('#betSettingGroup').prop("selectedIndex", -1);
                disableSubmitButtons(true);
            }

            $('#betSettingGroup').change();
        },
        initEvent: function () {
            $('#betSettingGroup').on('change', function () {
                var selectedGroupType = this.value;
                var groupFilter = function (item) {
                    return item.GroupType === selectedGroupType;
                };
                var disableSubmit = false;

                for (var i = 0; i < _bettingGroupLimitCache.length; i++) {
                    var game = _bettingGroupLimitCache[i];
                    var group = game.Groups.filter(groupFilter)[0];
                    if (group) {
                        $("#min-" + game.GameType).html(_formatter.formatNumber(group.Min, { format: "#,###" }));
                        $("#max-" + game.GameType).html(_formatter.formatNumber(group.Max, { format: "#,###" }));
                    } else {
                        $("#min-" + game.GameType).html("");
                        $("#max-" + game.GameType).html("");
                        disableSubmit = true;
                    }
                }

                disableSubmitButtons(disableSubmit);
            });
        },

        checkApiContainGroup: function (groupType) {
            var groupFilter = function (item) {
                return item.GroupType === groupType;
            };
            return $(_bettingGroupLimitCache[0].Groups).filter(groupFilter).length > 0;
        },

        isValid: function () {
            var group = $("#betSettingGroup").val();
            if (!group) {
                return false;
            }

            if (_me.checkApiContainGroup(group) === false) {
                alert(Site.getSingleResource("AGCasinoNoGroupChosenErrMsg"));
                return false;
            }

            return true;
        }
    };

    return _me;
});