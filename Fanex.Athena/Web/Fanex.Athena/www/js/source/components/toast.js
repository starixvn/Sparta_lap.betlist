"use strict";
define(["jquery"], function ($) {
    var defaultOption = {
        text: "Data is being downloaded",
        position: "top right",
        itemStyle: "default",
        timeout: 6000
    };

    function generatePagePortal(position) {
        var positionClassName = generateClassByPosition(position);

        return '<div class="page-portal">' +
        '<div class="toast-container' + positionClassName + '">' +
                '<div class="toast-align">' +
                '</div>' +
            '</div>' +
        '</div>';
    }

    function generateClassByPosition(position) {
        var positions = position.trim().split(' ');
        if (positions.length < 2) {
            return "";
        }

        return positions.map(function (item) {
            return " toast-container-" + item;
        });
    }

    function generateToastItem(styleName, text) {
        return '<div class="toast-item toast-item-' + styleName + '">' +
                    '<p class="toast-message">' + text + '</p>' +
                    generateButtonGroup() +
                '</div>';
    }

    function generateButtonGroup() {
        return '<div class="toast-button-group">' +
                '<button type="button" class="icon icon-close toast-item-close"></button>' +
            '</div>';
    }

    var _me = {
        show: function (options) {
            var toastOption = $.extend({}, defaultOption, options || {});

            var existed = $("body").find(".page-portal .toast-container .toast-align").length > 0;
            var $pagePortal = existed ? $(".page-portal") : $(generatePagePortal(toastOption.position));

            var $toastAlign = $pagePortal.find(".toast-align");
            var $toastItem = $(generateToastItem(toastOption.itemStyle, toastOption.text));

            $toastItem.on("click", ".toast-item-close", function () {
                $toastItem.remove();
            });

            $toastAlign.append($toastItem);

            if (!existed) {
                $("body").append($pagePortal);
            }

            setTimeout(function () {
                $toastItem.fadeOut("fast");
            }, toastOption.timeout);
        }
    };

    return _me;
});