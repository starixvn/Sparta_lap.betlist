"use strict";

define(["common/site", "moment", "common/popup", "extensions/jquery.extension"],
    function (Site, moment, Popup) {
        var NOTIFICATION_ITEM = "<li class='notification-item' id='notification-item-{0}' title='{1}'>" +
                                "<a href='javascript:void(0)'>{2}</a><span class='type'>{3}</span><span class='date-time'>{4}</span>" +
                                "</li>";
        var GET_NOTIFICATION_URL = "api/NotificationReport/GetNotificationMessages";
        var MARK_NOTIFICATION = "api/NotificationReport/MarkNotificationAsRead";
        var POPUP_ID = "view-notification";
        var POPUP_CONTENT_MIN_HEIGHT = 300;

        var $notificationDdl = "";
        var $notificationLabel = "";
        var _newMsg = null;
        var _totalMsg = 0;

        function openDetailPopup(message, index) {
            var popup = $("#" + POPUP_ID);
            if (popup.is(":visible")) {
                popup.find('a.popup-close').click();
            }

            var options = {
                modal: true,
                title: Site.getSingleResource("ViewNotification"),
                height: "80%",
                id: POPUP_ID,
                width: "55%",
                onOpened: function () {
                    var popupHeight = $(".wrapContent", ".view-notification").height();
                    var subjectHeight = $(".subject", ".view-notification").height();
                    var dateTimeHeight = $(".date-time", ".view-notification").height();
                    var height = popupHeight - subjectHeight - dateTimeHeight - 60;

                    if (height > POPUP_CONTENT_MIN_HEIGHT) {
                        $(".content", ".view-notification").height(height);
                    }
                },
                onClose: function () {
                    var postData = {
                        MessageType: message.Type,
                        MessageId: message.Id
                    };

                    markMessageAsRead(postData, index);
                    $("#" + POPUP_ID).remove();
                }
            };

            var buttons = [
                {
                    text: Site.getSingleResource("Close"),
                    isClosedOnClick: true,
                    classes: "btn btn-close"
                }];

            var popupContent = '<div class="view-notification">'
                                + '<p class="subject">' + Site.getSingleResource("Subject") + ": " + message.Subject + '</p>'
                                + '<p class="date-time">' + moment(message.DateTime).format("MM/DD/YYYY hh:mm:ss A") + '</p>'
                                + '<div class="content">' + (message.Content || '') + '</div>'
                                + '</div>';

            Popup.openPopupWithButton(popupContent, options, buttons);
        }

        function addNotificationItems(data) {
            $notificationDdl.find(".notification-item").remove();
            updateNumberLabel(data.TotalRows);

            var $container = $notificationDdl.find(".first-notification").parent();
            if (data.DataList !== null && data.DataList.length > 0) {
                data.DataList.forEach(function (item, index) {
                    var time = moment(item.DateTime).format("hh:mm:ss A MM/DD/YYYY");
                    var subject = item.Subject || '';
                    var message = $($.format(NOTIFICATION_ITEM,
                        index,
                        subject,
                        (subject.length > 100 ? subject.substr(0, 97) + '...' : subject),
                        Site.getSingleResource(item.Type),
                        time));
                    message.on("click", function () {
                        openDetailPopup(item, index);
                    });

                    $container.append(message);
                });
            }
        }

        function markMessageAsRead(postData, index) {
            $.ajax({
                url: Site.resolveClientUrl(MARK_NOTIFICATION),
                type: "POST",
                data: postData,
                dataType: "json",
                cache: false,
                success: function (response) {
                    setTimeout(function () {
                        if (_newMsg === null && response.IsSuccessful) {
                            updateNumberLabel(_totalMsg - 1);
                            $notificationDdl.find("#notification-item-" + index).remove();
                        }
                    }, 100);
                }
            });
        }

        function updateNumberLabel(num) {
            var $numberOfMsg = $(".number-notifications", ".notifications-block");

            _totalMsg = num;

            if (num <= 0) {
                $numberOfMsg.remove();
            }
            else {
                if ($numberOfMsg.length) {
                    $numberOfMsg.text(num);
                }
                else {
                    $notificationLabel.append("<span class='number-notifications'>" + num + "</span>");
                }
            }
        }

        function showNotification(isShow) {
            if (isShow) {
                $notificationDdl.show();
            }
            else {
                $notificationDdl.hide();
                if (_newMsg !== null) {
                    addNotificationItems(_newMsg);
                }
            }
        }

        function addNotification() {
            $notificationDdl = $("#notificationmsg");

            if ($notificationDdl.length === 0) {
                return;
            }

            $notificationLabel = $(".notification-button", ".notifications-block");

            $notificationLabel.click(function (e) {
                e.stopPropagation();
                showNotification(!$notificationDdl.is(":visible"));
            });

            var apiUrl = Site.resolveClientUrl(GET_NOTIFICATION_URL);

            var processNotification = function () {
                $.ajax({
                    url: apiUrl,
                    type: "GET",
                    cache: false,
                    success: function (data) {
                        if ($notificationDdl.is(":visible")) {
                            _newMsg = data;
                            return;
                        }

                        _newMsg = null;
                        addNotificationItems(data);
                    },
                    statusCode: {
                        403: function () { }
                    }
                });
            };

            $('html').on('click', function () {
                if ($notificationDdl.is(":visible")) {
                    showNotification(false);
                }
            });
            $(window).on('blur', function () {
                if ($notificationDdl.is(":visible")) {
                    showNotification(false);
                }
            });

            processNotification();
            setInterval(processNotification, 60000);
            $notificationDdl.mCustomScrollbar({ theme: "minimal" });
        }

        return {
            add: addNotification
        };
    });