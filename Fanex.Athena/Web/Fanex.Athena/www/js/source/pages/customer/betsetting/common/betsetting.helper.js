"use strict";
define(["jquery", "common/site", "common/popup", "lib/jquery.balloon"], function ($, Site, Popup) {
    var _me = {
        showDialogMesage: function (title, message) {
            var options = {
                title: Site.getSingleResource("Confirmation"),
                height: "auto",
                id: "result-update-betsetting",
                onOpen: function () {
                    $('#btnSubmitBetSetting, #btnReset').prop('disabled', true);
                },
                onClose: function () {
                    $("#result-update-betsetting").remove();
                    $('#btnSubmitBetSetting, #btnReset').removeAttr('disabled');
                    setTimeout(function () {
                        Site.blockUI();
                        Site.getLocation().reload();
                    }, 500);
                }
            };

            var buttons = [
                {
                    text: Site.getSingleResource("OK"),
                    isClosedOnClick: true,
                    classes: "btn btn-submit"
                }];

            Popup.openPopupWithButton('<div><p>' + message + '</p></div>', options, buttons);
        },

        convertToFloat: function (str) {
            var temp = str.replace(/,/g, "");
            var temp2 = temp.replace(".", "");

            if (_me.isInteger(temp2)) {
                return parseFloat(temp);
            }
            else {
                return NaN;
            }
        },

        isInteger: function (s) {
            var i;

            if (_me.isEmpty(s)) {
                return false;
            }
            else {
                for (i = 0; i < s.length; i++) {
                    var c = s.charAt(i);

                    if (!_me.isDigit(c)) { return false; }
                }
            }

            return true;
        },

        isDigit: function (c) {
            return ((c >= "0") && (c <= "9"));
        },

        isEmpty: function (s) {
            var empty = ((s == null) || (s.length === 0));
            if (!empty) {
                var news = s.replace(" ", "");
                if (news.length > 0) {
                    return false;
                }
                else {
                    return true;
                }
            }
            return empty;
        },

        showBalloonMsg: function (elementSelector, strMsg) {
            var contents = '<div onclick="hideBalloonMsg(\'' + elementSelector + '\')" style="width:140px;" class="msgerr">' + strMsg + '</div>';

            $(elementSelector).blur(function () { _me.hideBalloonMsg(elementSelector); })
                              .focus()
                              .showBalloon({
                                  contents: contents
                              });
        },
        hideBalloonMsg: function (elementSelector) {
            $(elementSelector).hideBalloon();
        },
    };

    return _me;
});