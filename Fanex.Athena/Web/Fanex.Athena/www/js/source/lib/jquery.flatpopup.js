"use strict";
define(['jquery', 'lib/jquery.tiny-draggable'], function ($) {
    var DEFAULT_WIDTH = 640;
    var DEFAULT_HEIGHT = 480;
    var DEFAULT_CONTAINER = "body";
    $.fn.flatPopup = function (options) {
        var $self = this;
        var defaultOptions = {
            width: DEFAULT_WIDTH,
            height: DEFAULT_HEIGHT,
            id: "popup-content",
            title: "&nbsp;",
            container: DEFAULT_CONTAINER
        };

        options = $.extend(defaultOptions, options || {});
        $self.addClass("flat-popup");

        var $container = $(options.container);
        var isNew = $container.find("#" + options.id).length === 0;
        if (isNew) {
            $self.attr("id", options.id);
            $self.children().wrapAll("<div class='wrapContent'></div>");
            $self.hide()
                 .prepend('<div class="title-bar popup-title"><div class="txt-title">' + options.title + '</div><a class="popup-close" href="#"><i class="icon-close"></i></a></div>')

            $self.find('a.popup-close').on('click', function () {
                $self.fadeOut(300);
                return false;
            });
        }

        $self.find(".wrapContent").height(options.height - 40);
        $self.width(options.width);
        $self.height(options.height);
        $self.tinyDraggable({ handle: '.popup-title' });

        if (isNew) {
            $container.append($self);
        }

        return {
            show: function () {
                //Set the center alignment padding + border see css style
                var popMarginLeft = ($self.width() + 24) / 2;
                var top = parseInt($self.css("top"));

                $self.css({
                    'margin-top': -(top / 2),
                    'margin-left': -popMarginLeft
                });

                $self.fadeIn(300);
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