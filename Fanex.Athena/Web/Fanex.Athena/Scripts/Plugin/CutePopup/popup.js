"use strict";
(function ($) {
    var MIN_WIDTH = 640;
    var MAX_HEIGHT = 480;
    $.fn.cutePopup = function (options) {

        var $self = this;
        var defaultOptions = {
            width: MIN_WIDTH,
            height: MAX_HEIGHT
        };
        
        options = $.extend(defaultOptions, options || {});

        options.width = options.width < MIN_WIDTH ? MIN_WIDTH : options.width;
        options.height = options.height > MAX_HEIGHT ? MAX_HEIGHT : options.height;
        $self.addClass("cute-popup");

        // add popup content to a wrapper
        $self.children().wrapAll("<div class='wrapContent'></div>");
        $self.find(".wrapContent").width(options.width - 10).height(options.height - 10);

        $self.show().find(".wrapContent").customScrollbar({
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
                // var popMarginTop = ($self.height() + 24) / 2;                
                var popMarginLeft = ($self.width() + 24) / 2;
                var top = parseInt($self.css("top"));
                
                $self.css({
                    'margin-top': -(top / 2),
                    'margin-left': -popMarginLeft
                });
            }
        };
    };

} (jQuery));