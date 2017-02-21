"use strict";
define(["jquery"], function ($) {
    var _me = {
        resizeGrid: function () {
            var $grid = $(".body-content");
            if ($grid.length === 0) {
                return;
            }

            var gridTop = $grid.offset().top;
            var footerSite = 18;
            var pagingHeight = $(".pagingContainer").length > 0 ? $(".pagingContainer").height() : 0;

            var gridHeight = window.innerHeight - gridTop - $(".footer-content").height() - pagingHeight - footerSite - 10;
            gridHeight = Math.max(150, gridHeight);
            $grid.css("max-height", gridHeight);

            if (!$grid.hasVerticalScrollBar()) {
                $grid.addClass("noScroll");
            }
            else {
                $grid.find("table").width($grid.width() - 17);
            }
        }
    };

    return _me;
});