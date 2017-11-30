"use strict";

define(["common/site", "moment", "common/popup", "extensions/jquery.extension"],
    function (Site, moment, Popup) {
        var NOTIFICATION_ITEM = "<li class='notification-item' id='notification-item-{0}' title='{1}'>" +
                                "<a href='javascript:void(0)'>{2}</a><span class='type'>{3}</span><span class='date-time'>{4}</span>" +
                                "</li>";
        var GET_NOTIFICATION_URL = "api/NotificationReport/GetNotificationMessages";
        var MARK_NOTIFICATION = "api/NotificationReport/MarkNotificationAsRead";
        var POPUP_ID = "view-notifiation";

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
                                + '<div class="content"><iframe src="data:text/html;charset=utf-8,'
                                + (Site.asNull(message.Content) === null ? '' : encodeURI(message.Content)) + '"></iframe></div>'
                                + '</div>';

            Popup.openPopupWithButton(popupContent, options, buttons);
        }

        function addNotificationItems(data) {
            $notificationDdl.children(".notification-item").remove();

            updateNumberLabel(data.TotalRows);

            if (data !== null && data.DataList.length > 0 ) {
                data.DataList.forEach(function (item, index) {
                    var time = moment(item.DateTime).format("hh:mm:ss A MM/DD/YYYY");
                    var message = $($.format(NOTIFICATION_ITEM, index, (item.Subject || ''), (item.Subject || ''), Site.getSingleResource(item.Type), time));
                    message.on("click", function () {
                        openDetailPopup(item, index);
                    });
                   
                    $notificationDdl.append(message);
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

        function addNotification() {
            $notificationDdl = $("#notificationmsg");

            if ($notificationDdl.length === 0) {
                return;
            }

            $notificationLabel = $(".dropdown-button", ".notifications-block");

            var apiUrl = Site.resolveClientUrl(GET_NOTIFICATION_URL);

            var processNotification = function () {
                $.ajax({
                    url: apiUrl,
                    async: false,
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
                        403: function () {}
                    }
                });
            };

            $(window).on('blur', function () {
                if ($notificationDdl.is(":visible")) {
                    if (_newMsg !== null) {
                        addNotificationItems(_newMsg);
                    }
                    $notificationLabel.dropdown('close');
                }
            });

            processNotification();
            setInterval(processNotification, 60000);
        }

        return {
            add: addNotification
        };
    });