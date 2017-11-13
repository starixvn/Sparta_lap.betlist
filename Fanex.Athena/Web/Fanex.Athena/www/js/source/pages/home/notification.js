"use strict";

define(["common/site", "moment", "common/popup", "extensions/jquery.extension"],
    function (Site, moment, Popup) {
        const NOTIFICATION_ITEM = "<li class='notification-item' title={0}><a href='javascript:void(0)'>{1}</a><span class='type'>{2}</span><span class='date-time'>{3}</span></li>";
        const GET_NOTIFICATION_URL = "api/NotificationReport/GetNotificationMessages";
        const MARK_NOTIFICATION = "api/NotificationReport/MarkNotificationAsRead";
        var $notificationDdl = "";

        function openDetailPopup(message) {
            var options = {
                modal: true,
                title: Site.getSingleResource("ViewNotification"),
                height: "auto",
                id: "view-notifiation",
                width: 600,
                onClose: function () {
                    markMessageAsRead(message);
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
                                + '<div class="content">' + message.Content + '</div>'
                                + '</div>';

            Popup.openPopupWithButton(popupContent, options, buttons);
        };

        function addNotificationItems($parent, $label, data) {
            var now = new Date();
            var $numberOfMsg = $(".number-notifications", ".notifications-block");

            $parent.children(".notification-item").remove();

            if (data === null || data.DataList.length === 0) {
                $numberOfMsg.remove();
            }
            else {
                var total = data.TotalRows;

                if ($numberOfMsg.length) {
                    $numberOfMsg.text(total);
                }
                else {
                    $label.append("<span class='number-notifications'>" + total + "</span>");
                }
                data.DataList.forEach(function (item) {
                    var message = $($.format(NOTIFICATION_ITEM, item.Subject, item.Subject, Site.getSingleResource(item.Type), moment(item.DateTime).format("hh:mm:ss A MM/DD/YYYY")));
                    message.on("click", function () {
                        openDetailPopup(item);
                    });
                    $parent.append(message);
                });
            }
        }

        function markMessageAsRead(message) {
            $.ajax({
                url: Site.resolveClientUrl(MARK_NOTIFICATION),
                type: "POST",
                data: message,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false
            });
        }

        function addNotification() {
            var $notificationDdl = $("#notificationmsg");

            if ($notificationDdl.length === 0) {
                return;
            }

            var apiUrlMarkAsRead = Site.resolveClientUrl("api/NotificationReport/GetNotificationMessages");
            var $notificationLabel = $(".dropdown-button", ".notifications-block");
            var newMsg = null;          
            var apiUrl = Site.resolveClientUrl(GET_NOTIFICATION_URL);

            var processNotification = function () {
                $.get(apiUrl, function (data) {
                    if ($notificationDdl.is(":visible")) {
                        newMsg = data;
                        return;
                    }

                    newMsg = null;
                    addNotificationItems($notificationDdl, $notificationLabel, data);
                });
            };

            $(window).on('blur', function () {
                if ($notificationDdl.is(":visible")) {
                    if (newMsg != null) {
                        addNotificationItems($notificationDdl, $notificationLabel, newMsg);
                    }
                    $notificationLabel.dropdown('close');
                }
            });

            processNotification();
            setInterval(processNotification, 60000);
        }

        return {
            add: addNotification
        }
    });