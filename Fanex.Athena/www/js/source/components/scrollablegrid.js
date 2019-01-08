"use strict";
define(["jquery"], function ($) {
    var _me = {
        resizeGrid: function ($scrollableContent) {
            var $grid = $scrollableContent || $(".body-content");
            if ($grid.length === 0) {
                return;
            }

            _me.updateHeight($grid);
            _me.updateWidth($grid);
        },

        updateHeight: function ($grid) {
            var gridTop = $grid.offset().top;
            var footerSite = 18;
            var footerHeight = $(".footer-content").length > 0 ? $(".footer-content").height() : 0;
            var pagingHeight = $(".pagingContainer").length > 0 ? $(".pagingContainer").height() : 0;

            var gridHeight = window.innerHeight - gridTop - footerHeight - pagingHeight - footerSite - 10;
            gridHeight = Math.max(150, gridHeight);
            $grid.css("max-height", gridHeight);
        },

        updateWidth: function ($grid) {
            if (!$grid.hasVerticalScrollBar()) {
                $grid.addClass("noScroll").find("table").css("width", "");
            }
            else {
                $grid.removeClass("noScroll").find("table").css("width", $grid.width() - 17);
            }
        }
    };

    return _me;
});