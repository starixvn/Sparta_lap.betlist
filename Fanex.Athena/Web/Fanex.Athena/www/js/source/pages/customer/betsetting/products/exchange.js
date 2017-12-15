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
                _me.updateComm($(this).attr("data-cust-id"), $("#ddlCommission").val());
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
                    var messageResource = Number(result.Code) === 0 ? "MessageSuccess" : "Fail";
                    BetSettingHelper.showDialogMesage(Site.getSingleResource(messageResource), result.Message);
                }
            });
        },
    };

    return _me;
});