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
        }
    };

    return _me;
});