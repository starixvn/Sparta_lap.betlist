/// <reference path="/@JSense.js" />
var CustBalance = (function ($) {
    return {
        initialize: function () {

            CustBalance.initEvent();
        },
        initEvent: function () {
            $("#data-content").scroll(function () {
                $('#total-content').scrollLeft($('#data-content').scrollLeft());
                $('#header-content').scrollLeft($('#data-content').scrollLeft());
            });
        },
        ajaxStart: function () {
            Site.blockUI();
        },
        ajaxComplete: function () {
            Site.unBlockUI();
        },
        checkIsValidTimeRequest: function () {
            var isShowWarningPopup = $('#IsValidTimeRequest').val() == 'False';
            if (isShowWarningPopup) {
                var title = Site.getSingleResource("LrfNotification");
                var message = Site.getSingleResource("CustomerBalanceDateRangeNotification");
                $('<div title="' + title + '">' + message + '</div>').dialog();
            }
        }
    };

})(jQuery);


$(window).load(function () {
    CustBalance.initialize();
    CustBalance.checkIsValidTimeRequest();
});