/// <reference path="/@JSense.js" />
var MoneyLine = (function ($) {
    return {
        initialize: function () {
            Totalbets.initialize();
            MoneyLine.initEvent();
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