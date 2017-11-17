"use strict";

define(["common/site", "moment", "common/popup", "extensions/jquery.extension"],
    function (Site, moment, Popup) {
        var NOTIFICATION_ITEM = "<li class='notification-item' id='notification-item-{0}' title='{1}'>" +
                                "<a href='javascript:void(0)'>{2}</a><span class='type'>{3}</span><span class='date-time'>{4}</span>" +
                                "</li>";
        var GET_NOTIFICATION_URL = "api/NotificationReport/GetNotificationMessages";
        var MARK_NOTIFICATION = "api/NotificationReport/MarkNotificationAsRead";

        var $notificationDdl = "";
        var $notificationLabel = "";
        var _newMsg = null;

        function openDetailPopup(message, index) {
            var options = {
                modal: true,
                title: Site.getSingleResource("ViewNotification"),
                height: "auto",
                id: "view-notifiation",
                width: 600,
                onClose: function () {
                    var postData = {
                        MessageType: message.Type,
                        MessageId: message.Id
                    };

                    markMessageAsRead(postData, index);
                }
            };

            var buttons = [
                {
                    text: Site.getSingleResource("Close"),
                    isClosedOnClick: true,
                    classes: "btn btn-close"
                }];

            var popupContent = '<div class="view-notification">'
                                + '<p class="subject">' + message.Subject + '</p>'
                                + '<p class="date-time">' + moment(message.DateTime).format("MM/DD/YYYY hh:mm:ss A") + '</p>'
                                + (Site.asNull(message.Content) === null ? "" : '<div class="content">' + message.Content + '</div>')
                                + '</div>';

            Popup.openPopupWithButton(popupContent, options, buttons);
        }

        function addNotificationItems(data) {
            $notificationDdl.children(".notification-item").remove();

            updateNumberLabel(data.TotalRows);

            if (data !== null && data.DataList.length > 0 ) {
                data.DataList.forEach(function (item, index) {
                    var time = moment(item.DateTime).format("hh:mm:ss A MM/DD/YYYY");
                    var message = $($.format(NOTIFICATION_ITEM, index, item.Subject, item.Subject, Site.getSingleResource(item.Type), time));
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
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                success: function (response) {
                    setTimeout(function () {
                        if (_newMsg === null && response.IsSuccessful) {
                            updateNumberLabel($notificationDdl.children(".notification-item").length - 1);
                            $notificationDdl.find("#notification-item-" + index).remove();
                        }
                    }, 100);
                }
            });
        }

        function updateNumberLabel(num) {
            var $numberOfMsg = $(".number-notifications", ".notifications-block");

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
                $.get(apiUrl, function (data) {
                    if ($notificationDdl.is(":visible")) {
                        _newMsg = data;
                        return;
                    }

                    _newMsg = null;
                    addNotificationItems(data);
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