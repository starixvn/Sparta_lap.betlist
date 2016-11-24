define(['jquery'], function ($) {
    "use strict";

    var _me = {
        openPopup: function(element, option){
            $(element).flatPopup(option).show();
        },
        openBetListPopup: function (url, option) {
            var $popUp = $("#" + option.id);
            if ($popUp.length > 0) {
                $("#popupFrame").attr('src', url);
            } else {
                $popUp = $('<div id="' + option.id + '" class="betlist-popup"><iframe id="popupFrame" src="' + url + '" ></iframe></div>');
            }

            $popUp.flatPopup(option).show();
        }
    };

    return _me;
});