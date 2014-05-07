/// <reference path="/@JSense.js" />

var ManageAccount = (function ($) {
    return {
        resetPassword: function (sender) {
            var target = $(sender);
            var url = Site.resolveClientUrl("Account/ResetPassword");
            url += "?UserId=" + target.attr("data-custid");
            url += "&UserName=" + target.attr("data-username");
            url += "&FirstName=" + target.attr("data-firstname");
            url += "&LastName=" + target.attr("data-lastname");
            var width = 400, height = 250;
            var title = Site.getSingleResource("ResetPassword");
            ManageAccount.showPopup(title, width, height, url);
        },

        editAccount: function (sender) {
            var target = $(sender);
            var url = Site.resolveClientUrl("Account/EditAccount");
            url += "?UserId=" + target.attr("data-custid");

            var width = 400, height = 250;
            var title = Site.getSingleResource("EditAccount");
            ManageAccount.showPopup(title, width, height, url, function () {
                var url = document.location.href;
                window.location.href = url;
            });
        },

        editPermission: function (sender) {
            var target = $(sender);
            var url = Site.resolveClientUrl("Account/PermissionPage");
            url += "?UserId=" + target.attr("data-custid");
          
            window.location.href = url
        },

        showPopup: function (title, width, height, url, callback) {
            $('#manage-dialog').remove();
            var dialogForm = '<div title="' + title
                + '" id="manage-dialog"><iframe id="myframe" frameborder="0" width="'
                + width + '" height="' + height + '" src="' + url + '" style="overflow: hidden;"></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: width + 15,
                height: height + 50,
                close: function () {
                    if (callback !== undefined) {
                        callback();
                    }
                }
            });
        }
    };
} (jQuery));