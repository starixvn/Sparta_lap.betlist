/*global BetSetting*/
/// <reference path="/@JSense.js" />
"use strict";
var VoidBridge = (function ($) {
    return {
        initEvent: function () {
            $("#btnUpdateStatus").off("click").click(BetSetting.updateStatus);
        }
    };
})(jQuery);

$(window).load(function () {
    VoidBridge.initEvent();
});