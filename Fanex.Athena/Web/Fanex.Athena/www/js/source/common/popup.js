define(['jquery', 'components/jquery.flatpopup'], function ($) {
    "use strict";
    /*
    var defaultOptions = {
        width: 640,
        height: 480,
        id: "popup-content",
        className: "",
        title: "&nbsp;",
        container: "body",
        modal: DEFAULT_MODAL,
        onOpen: null,
        onClose: null
    };
    */

    var _me = {
        openPopup: function (element, option) {
            return $(element).flatPopup(option);
        },
        closePopup: function (id) {
            $("#" + id).fadeOut(300);
            $(".popup-block-body").hide();
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

        /*
        var buttons = [
                {
                    text: "OK",
                    click: function () { },
                    classes: "btn btn-primary",
                    id: "confirm-del-ok",
                    isClosedOnClick: false
                }];
        */
        openPopupWithButton: function (element, option, buttons) {
            var defaultOption = {
                padding: 10
            };

            $.extend(defaultOption, option || {});

            var $element = $(element);
            var $popup = $("#" + option.id);
            if ($popup.length === 0 || $popup.hasClass('flat-popup') === false) {
                $element.append('<div class="right popup-button"></div>');
                var $blockButton = $element.find(".popup-button");

                $.each(buttons, function (index, button) {
                    var $button = $('<button type="button"'
                        + (button.id ? 'id="' + button.id + '"' : '')
                        + ' class="' + button.classes + '">'
                        + button.text + '</button>');
                    $blockButton.append($button);
                    if (button.isClosedOnClick) {
                        $button.on("click", function () {
                            _me.closePopup(defaultOption.id);

                            if (typeof button.click === "function") {
                                button.click();
                            }                            
                        });
                    }
                    else {
                        $button.on("click", button.click);
                    }
                });
            }

            return $element.flatPopup(defaultOption);
        }
    };

    return _me;
});