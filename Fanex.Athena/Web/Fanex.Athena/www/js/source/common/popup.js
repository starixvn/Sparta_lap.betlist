define(['jquery', 'components/jquery.flatpopup'], function ($) {
    "use strict";

    var _me = {
        openPopup: function (element, option) {
            return $(element).flatPopup(option);
        },

        openPopupWithIframe: function (url, option) {
            var $popUp = $("#" + option.id);
            var iframe = '<iframe id="popupFrame" src="' + url + '" ></iframe>';
            if ($popUp.length === 0) {
                $popUp = $('<div id="' + option.id + '" class="popup-frame">' + iframe + '</div>');
            }
            else {
                $popUp.append(iframe);
            }

            return $popUp.flatPopup(option);
        },

        updateCurrentPopup: function (url, option) {
            var $popUp;
            var $frame = parent.$("#popupFrame");
            if (option.id) {
                $popUp = $("#" + option.id);
            }

            if (!$popUp || $popUp.length === 0) {
                $popUp = $frame.closest(".popup-frame");
                option.id = $popUp.attr('id');
            }

            if ($popUp.length === 0) {
                return;
            }

            if (url && url !== '') {
                $frame.attr('src', url);
            }

            if (option.title !== "") {
                $popUp.find(".txt-title").html(option.title);
            }

            return $popUp.flatPopup(option);
        },

        show: function ($popup, isModal) {
            $popup.flatPopup("show", isModal);
        },

        openPopupWithButton: function(element, option, buttons) {
            var $element = $(element);

            if ($("#" + option.id).length === 0) {
                $element.append('<div class="right popup-button"></div>');
                var $blockButton = $element.find(".popup-button");

                $.each(buttons, function (i, e) {
                    var button = '<button type="button" id="' + e.id + '" class="' + e.classes + '">' + e.title + '</button>';
                    $blockButton.append(button);
                    $blockButton.find("#" + e.id).on("click", e.method);
                });
            }

            return $element.flatPopup(option);
        }
    };

    return _me;
});