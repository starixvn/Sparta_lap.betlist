"use strict";
define(["jquery", "common/site", "lib/jquery.balloon"], function ($, Site) {
    var _me = {
        showDialogMesage: function (title, message, closePopup) {
            var ok = Site.getSingleResource("OK");
            var options = {
                closeOnEscapeType: true,
                model: true,
                resizable: false,
                height: 150,
                open: function () {
                    $('#btnSubmitBetSetting, #btnReset').prop('disabled', true);
                },
                buttons: [
                {
                    text: ok,
                    click: function () {
                        $("#bet-setting-message").dialog("close");
                    }
                }
                ],
                close: function () {
                    $("#bet-setting-message").remove();
                    $('#btnSubmitBetSetting, #btnReset').removeAttr('disabled');
                    if (closePopup) {
                        parent.$("#bet-setting-dialog").dialog("close");
                    } else {
                        setTimeout(function () {
                            Site.blockUI();
                            window.location.reload();
                        }, 500);
                    }
                }
            };
            $('<div id="bet-setting-message" title="' + title + '">' + message + '</div>').dialog(options);
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