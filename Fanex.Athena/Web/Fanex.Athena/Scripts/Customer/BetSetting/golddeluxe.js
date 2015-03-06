/*global BetSetting*/
/// <reference path="/@JSense.js" />
"use strict";
var GoldDeluxe = (function ($) {
    var _bettingGroupLimitCache;
    function disableSubmitButtons(value) {
        var $button = $("#btnSubmitBetSetting, #btnReset").prop('disabled', value);
        if (value) {
            $button.addClass("button-disabled");
        } else {
            $button.removeClass("button-disabled");
        }
    }
    return {
        initialize: function () {
            GoldDeluxe.initEvent();
            if ($("#BettingLimit").length > 0) {
                _bettingGroupLimitCache = JSON.parse($("#BettingLimit").val());
            }

            if ($('#betSettingGroup').data("default") === "") {
                $('#betSettingGroup').prop("selectedIndex", -1);
                disableSubmitButtons(true);
            }

            if($("#btnSubmitBetSetting").prop("disabled") === true)
            {
                disableSubmitButtons(true);
            }
        },
        initEvent: function () {
            // dropdown group
            $('#betSettingGroup').on('change', function () {
                var groupFilter = function (index, item) {
                    return item.GroupType === selectedGroupType;
                };
                var disableSubmit = false;
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
                        disableSubmit = true;
                    }
                }

                disableSubmitButtons(disableSubmit);
            });

           
            $("#btnUpdateStatus").off("click").click(BetSetting.updateStatus);
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
    GoldDeluxe.initialize();
});