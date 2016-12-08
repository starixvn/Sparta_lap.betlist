define(['jquery', 'lib/jquery.flatpopup'], function ($) {
    "use strict";

    var _me = {
        openPopup: function (element, option) {
            return $(element).flatPopup(option);
        },

        openBetListPopup: function (url, option) {
            var $popUp = $("#" + option.id);
            if ($popUp.length > 0) {
                $("#popupFrame").attr('src', url);
            } else {
                $popUp = $('<div id="' + option.id + '" class="betlist-popup"><iframe id="popupFrame" src="' + url + '" ></iframe></div>');
            }

            return $popUp.flatPopup(option);
        }
    };

    return _me;
});