/*global Site, BetSetting*/
/// <reference path="/@JSense.js" />
"use strict";
var Exchange = (function ($) {
    return {
        initialize: function () {
            $("#btnSubmit").click(function () {
                Exchange.updateComm($(this).attr("data-cust-id"), $("#ddlCommission").val());
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