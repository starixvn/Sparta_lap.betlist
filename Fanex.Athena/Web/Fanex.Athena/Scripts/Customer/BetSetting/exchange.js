/*global Site, BetSetting*/
/// <reference path="/@JSense.js" />
"use strict";
var Exchange = (function ($) {
    return {
        initialize: function () {
            $("#btnSubmit").click(function () {
                Exchange.updateComm($(this).attr("data-cust-id"), $("#ddlCommission").val());
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
                    Site.blockUI();
                },
                beforeSend: Site.blockUI,
                complete: Site.unBlockUI
            });
        }
    };
})(jQuery);

$(function () {
    Exchange.initialize();
});