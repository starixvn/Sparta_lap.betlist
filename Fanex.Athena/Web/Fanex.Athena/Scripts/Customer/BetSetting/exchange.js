/*global Site, BetSetting*/
/// <reference path="/@JSense.js" />
"use strict";
var Exchange = (function ($) {
    return {
        initialize: function () {
            $("#btnSubmit").click(function () {
                var $dropdown = $("#ddlCommission");
                Exchange.updateComm($(this).attr("data-cust-id"), $dropdown.data("original-value"), $dropdown.val());
            });

            $("#btnReset").click(function () {
                var $dropdown = $("#ddlCommission");
                $dropdown.val($dropdown.data("original-value")).change();
            });
        },
        updateComm: function (custId, oldCommission, newCommission) {
            $.ajax({
                type: 'POST',
                url: Site.resolveClientUrl('ExchangeDcsBetSetting/UpdateComm'),
                cache: false,
                data: {
                    custid: custId,
                    oldCommission: oldCommission,
                    newCommission: newCommission
                },
                dataType: "json",
                success: function (result) {
                    BetSetting.showUpdateSettingResult(result);
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