"use strict";
define(['jquery'], function ($) {
    var defaultOptions = {
        width: 640,
        height: 480,
        id: "popup-content",
        title: "&nbsp;",
        container: "body",
        modal: true
    };

    $.fn.flatPopup = function (options) {
        var $self = this;
        var $blockBody = $(".block-body");

        options = $.extend(defaultOptions, options || {});
        $self.addClass("flat-popup");

        var $container = $(options.container);

        if (options.modal && $blockBody.length === 0) {
            $blockBody = $('<div class="block-body" style="display: none"></div>');
            $container.append($blockBody);
        }

        var isNew = $container.find("#" + options.id).length === 0;
        if (isNew) {
            $self.attr("id", options.id);
            $self.children().wrapAll("<div class='wrapContent'></div>");
            $self.hide()
                 .prepend('<div class="title-bar popup-title"><div class="txt-title">' + options.title + '</div><a class="popup-close" href="#"><i class="icon-close"></i></a></div>')

            $self.find('a.popup-close').on('click', function () {
                $self.fadeOut(300);
                $blockBody.hide();
                return false;
            });

            $container.append($self);
        }

        $self.width(options.width);
        $self.height(options.height);
        $self.find(".wrapContent").height(options.height - 40);

        return {
            show: function () {
                //Set the center alignment padding + border see css style
                var popMarginLeft = ($self.width() + 24) / 2;
                var top = parseInt($self.css("top"));

                $self.css({
                    'margin-top': -(top / 2),
                    'margin-left': -popMarginLeft
                });

                if (options.modal) {
                    $blockBody.show();
                }

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