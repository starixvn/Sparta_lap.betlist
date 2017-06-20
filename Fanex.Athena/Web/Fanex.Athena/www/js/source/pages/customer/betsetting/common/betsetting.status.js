"use strict";
define(["jquery", "common/site", "./betsetting.helper"], function ($, Site, BetSettingHelper) {
    var _me = {
        update: function (url, postData) {
            $.ajax({
                type: 'POST',
                url: url,
                cache: false,
                data: postData,
                dataType: "json",
                success: function (result) {
                    var messageResource = result.IsSuccessful ? "MessageSuccess" : "Fail";
                    BetSettingHelper.showDialogMesage(Site.getSingleResource(messageResource), result.Message, false);
                },
                error: function (request, status, err) {
                    BetSettingHelper.showDialogMesage(Site.getSingleResource("Error"), err, false);
                }
            });
        },

        getCommonPostData: function ($context) {
            var productId = parseInt($("#ProductId").val());
            var nickName = $context.data("nick-name");
            // update value  (0: enable, 1: disable) is opposed with current value (1: enable, 0: disable)
            var status = $context.data("status");
            var custId = $context.data("cust-id");

            return {
                productId: productId,
                nickname: nickName,
                status: status,
                custid: custId
            }
        }
    };

    return _me;
});