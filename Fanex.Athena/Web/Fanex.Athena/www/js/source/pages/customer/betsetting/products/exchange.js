"use strict";
define(["jquery", "common/site", "./dcsbetsetting", "../common/betsetting.helper"],
    function ($, Site, DcsBetSetting, BetSettingHelper) {
        var _count = 0;
        var _message = [
            "Has anyone told you not to click me ;(?",
            "Please stop it!",
            "I'm getting tired of you!",
            "Ok! I will not say anything anymore",
        ];
        function alertNotAvailable() {
            if (_count < _message.length) {
                alert(_message[_count]);
                _count++;
            }
        }
    var _me = {
        initialize: function () {
            DcsBetSetting.initialize();
            _me.initEvent();
        },

        initEvent: function () {
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
                    var messageResource = Number(result.Code) === 0 ? "MessageSuccess" : "Fail";
                    BetSettingHelper.showDialogMesage(Site.getSingleResource(messageResource), result.Message);
                }
            });
        },
    };

    return _me;
});