/*global Site, BetSetting*/
/// <reference path="/@JSense.js" />
"use strict";
var Exchange = (function ($) {
    var _count = 0;
    var _message = [
        "Has anyone told you not to click me ;(?",
        "Please stop it!",
        "I'm getting tired of you!",
        "Ok! I will not say anything anymore",
    ];
    function alertNotAvailable() {
        if(_count < _message.length)
        {
            alert(_message[_count]);
            _count++;
        }
    }
    return {
        initialize: function () {
            $("#btnSubmit").click(function () {
                alertNotAvailable();
                // _me.updateComm($(this).attr("data-cust-id"), $("#ddlCommission").val());
            });

            $("#btnReset").click(function () {
                var $dropdown = $("#ddlCommission");
                $dropdown.val($dropdown.data("original-value")).change();
            });
        },
        updateComm: function (custId, commission) {
            $.ajax({
                type: 'POST',
                url: Site.resolveClientUrl('ExchangeCommApi/UpdateComm'),
                cache: false,
                data: {
                    custid: custId,
                    commission: commission
                },
                dataType: "json",
                success: function (result) {
                    BetSetting.showUpdateSettingResult(result);
                }
            });
        },
    };
})(jQuery);

$(function () {
    Exchange.initialize();
});