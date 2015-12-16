/*global Site */
/// <reference path="/@JSense.js" />
var CustBalance = (function ($) {
    "use strict";
    return {
        resizeGrid: function () {
            var $grid = $("#data-content");
            if ($grid.length === 0) {
                return;
            }

            var gridTop = $grid.offset().top;
            var pagerHeight = $(".pagingContainer").height();
            if (pagerHeight > 0) {
                pagerHeight += 15;
            }

            var gridHeight = window.innerHeight - gridTop - $("#total-content").height() - pagerHeight - 30;
            gridHeight = Math.max(120, gridHeight);
            $grid.css("max-height", gridHeight);
        },
        initialize: function () {
            CustBalance.initEvent();
            CustBalance.resizeGrid();
            Site.addUserGuide({
                resourceName: "CustomerBalance",
                title: Site.getSingleResource("CustomerBalance")
            });
            Site.addFilterCollapseExpand();
        },
        initEvent: function () {
            $("#data-content").scroll(function () {
                $('#total-content').scrollLeft($('#data-content').scrollLeft());
                $('#header-content').scrollLeft($('#data-content').scrollLeft());
            });
        },
        ajaxStart: function () {
            Site.blockUI();
        },
        ajaxComplete: function () {
            Site.unBlockUI();
        },
        checkIsValidTimeRequest: function () {
            var isShowWarningPopup = $('#IsValidTimeRequest').val() === 'False';
            if (isShowWarningPopup) {
                var title = Site.getSingleResource("LrfNotification");
                var message = Site.getSingleResource("CustomerBalanceDateRangeNotification");
                $('<div title="' + title + '">' + message + '</div>').dialog();
            }
        }
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    CustBalance.initialize();
    CustBalance.checkIsValidTimeRequest();
});

$(window).resize(function () {
    "use strict";
    CustBalance.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});