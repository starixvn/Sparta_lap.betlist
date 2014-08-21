/*global BetSetting*/
/// <reference path="/@JSense.js" />
"use strict";
var AgCasino = (function ($) {
    var _bettingGroupLimitCache;
    return {
        initialize: function () {
            AgCasino.initEvent();
            _bettingGroupLimitCache = JSON.parse($("#BettingLimit").val());
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

                    $("#min-" + game.GameType).html($.formatNumber(group.Min, { format: "#,###" }));
                    $("#max-" + game.GameType).html($.formatNumber(group.Max, { format: "#,###" }));
                }
            });

            $("#btnUpdateStatus").click(BetSetting.updateStatus);
        }
    };
})(jQuery);

$(window).load(function () {
    AgCasino.initialize();
});