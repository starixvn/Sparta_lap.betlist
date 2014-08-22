/*global BetSetting*/
/// <reference path="/@JSense.js" />
"use strict";
var AgCasino = (function ($) {
    var _bettingGroupLimitCache;
    return {
        initialize: function () {
            AgCasino.initEvent();
            if ($("#BettingLimit").length > 0) {
                _bettingGroupLimitCache = JSON.parse($("#BettingLimit").val());
            }

            if ($('#betSettingGroup').data("default") === "") {
                $('#betSettingGroup').prop("selectedIndex", -1);
            }
        },
        initEvent: function () {
            // dropdown group
            $('#betSettingGroup').on('change', function () {
                var groupFilter = function (index, item) {
                    return item.GroupType === selectedGroupType;
                };

                var selectedGroupType = this.value;
                for (var i = 0; i < _bettingGroupLimitCache.length; i++) {
                    var game = _bettingGroupLimitCache[i];
                    var group = $(game.Groups).filter(groupFilter)[0];
                    if (group) {
                        $("#min-" + game.GameType).html($.formatNumber(group.Min, { format: "#,###" }));
                        $("#max-" + game.GameType).html($.formatNumber(group.Max, { format: "#,###" }));
                    } else {
                        $("#min-" + game.GameType).html("");
                        $("#max-" + game.GameType).html("");
                    }
                }
            });

            $("#btnUpdateStatus").click(BetSetting.updateStatus);
        },
        checkApiContainGroup: function (groupType) {
            var groupFilter = function (index, item) {
                return item.GroupType === groupType;
            };
            return $(_bettingGroupLimitCache[0].Groups).filter(groupFilter).length > 0;
        }
    };
})(jQuery);

$(window).load(function () {
    AgCasino.initialize();
});