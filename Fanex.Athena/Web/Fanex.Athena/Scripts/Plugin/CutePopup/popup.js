"use strict";
(function ($) {
    var MIN_WIDTH = 640;
    var MIN_HEIGHT = 480;
    $.fn.cutePopup = function (options) {

        var $self = this;
        var defaultOptions = {
            width: MIN_WIDTH,
            height: MIN_HEIGHT
        };

        options = $.extend(options || {}, defaultOptions);

        options.width = options.width < MIN_WIDTH ? MIN_WIDTH : options.width;
        options.height = options.height < MIN_HEIGHT ? MIN_HEIGHT : options.height;
        $self.addClass("cute-popup");
        // add popup content to a wrapper
        $self.children().wrapAll("<div class='wrapContent'></div>");

        $self.find(".wrapContent").width(options.width - 10).height(options.height - 10);

        $self.find(".wrapContent").customScrollbar({
            skin: 'default-skin'
        });

        $self.hide()
             .prepend("<a href='#' class='popup-close'></a>")
             .width(options.width)
             .height(options.height);

        $self.find('a.popup-close').on('click', function () {
            $self.fadeOut(300);
            return false;
        });

        return {
            show: function () {
                //Fade in the Popup
                $self.fadeIn(300);

                //Set the center alignment padding + border see css style
                var popMarginTop = ($self.height() + 24) / 2;
                var popMarginLeft = ($self.width() + 24) / 2;

                $self.css({
                    'margin-top': -popMarginTop,
                    'margin-left': -popMarginLeft
                });
            }
        };
    };

} (jQuery));