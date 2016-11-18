"use strict";
define(['jquery', 'lib/jquery.tiny-draggable'], function ($) {
    var MIN_WIDTH = 640;
    var MAX_HEIGHT = 480;
    $.fn.flatPopup = function (options) {

        var $self = this;
        var defaultOptions = {
            width: MIN_WIDTH,
            height: MAX_HEIGHT,
            title: "&nbsp;"
        };

        options = $.extend(defaultOptions, options || {});

        options.width = options.width < MIN_WIDTH ? MIN_WIDTH : options.width;
        options.height = options.height > MAX_HEIGHT ? MAX_HEIGHT : options.height;
        $self.addClass("flat-popup");

        // add popup content to a wrapper
        $self.children().wrapAll("<div class='wrapContent'></div>");
        $self.find(".wrapContent").height(options.height - 40);

        $self.hide()
             .prepend('<div class="title-bar popup-title"><div class="txt-title">' + options.title + '</div><a class="popup-close" href="#"><i class="icon-close"></i></a></div>')
             .width(options.width)
             .height(options.height);

        $self.find('a.popup-close').on('click', function () {
            $self.fadeOut(300);
            return false;
        });

        $self.tinyDraggable({ handle: '.popup-title' });

        $(".wrapper").find(".flat-popup").remove();
        $(".wrapper").append($self);

        return {
            show: function () {
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
});

function keydown(e) {
    for (var i = keys.length; i--;) {
        if (e.keyCode === keys[i]) {
            preventDefault(e);
            return;
        }
    }
}

function wheel(e) {
  preventDefault(e);
}

function disable_scroll() {
  if (window.addEventListener) {
      window.addEventListener('DOMMouseScroll', wheel, false);
  }
  window.onmousewheel = document.onmousewheel = wheel;
  document.onkeydown = keydown;
}