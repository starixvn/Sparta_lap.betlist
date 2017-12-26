"use strict";
define(["jquery", "common/site", "./dcsbetsetting", "../common/betsetting.helper"],
    function ($, Site, DcsBetSetting, BetSettingHelper) {
       var _me = {
        initialize: function () {
            DcsBetSetting.initialize();
            _me.initEvent();
        },

        initEvent: function () {
            $("#btnSubmit").click(function () {
                var $dropdown = $("#ddlCommission");
                _me.updateComm($(this).attr("data-cust-id"), $dropdown.data("original-value"), $("#ddlCommission").val());
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
                    var messageResource = Number(result.Code) === 0 ? "MessageSuccess" : "Fail";
                    BetSettingHelper.showDialogMesage(Site.getSingleResource(messageResource), result.Message);
                },
                beforeSend: Site.blockUI,
                complete: Site.unBlockUI
            });
        },
    };

    return _me;
});