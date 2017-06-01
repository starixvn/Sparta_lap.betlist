define(["jquery", "common/site", "common/querystring"],
    function ($, Site, QueryString) {
        "use strict";

        var formSelector = "#CommissionGroupForm";
        var excelButtonSeletor = "#ExportToExcel";

        function initialize() {
            Site.initialize();
            Site.addUserGuide({
                resourceName: "CommissionGroupList",
                title: Site.getSingleResource("CommissionGroupList")
            });

            _me.initEvent();
        }

        function initEvent() {
            $(excelButtonSeletor).click(function () {
                $(formSelector).submit();
            });
        }

        var _me = {
            initialize: initialize,
            initEvent: initEvent
        };

        return _me;
    }
);