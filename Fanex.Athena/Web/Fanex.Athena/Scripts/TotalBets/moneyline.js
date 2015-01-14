/*global Site, Totalbets*/
/// <reference path="/@JSense.js" />
"use strict";
var MoneyLine = (function ($) {
    return {
        initialize: function () {
            Totalbets.initialize();
            MoneyLine.initEvent();
            Site.addUserGuide({
                resourceName: "TotalBetsMoneyLine",
                title: Site.getSingleResource("TotalBets") + " - " + Site.getSingleResource("MoneyLine")
            });
        },
        initEvent: function () {
            // Submit event.
            $("#SubmitButton").on("click", function () {
                MoneyLine.submit();
            });
        },
        submit: function () {
            Site.blockUI();
            $("#MoneyLineForm").submit();
        }
    };
})(jQuery);