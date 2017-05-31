define(["jquery", "common/site", "common/querystring", "components/scrollablegrid"],
    function ($, Site, QueryString, GridHelper) {
        "use strict";

        function initialize() {
            Site.initialize();
            Site.addUserGuide({
                resourceName: "CommissionGroupList",
                title: Site.getSingleResource("CommissionGroupList")
            });

            _me.initEvent();
        }

        function initEvent() {

        }

        var _me = {
            initialize: initialize,
            initEvent: initEvent
        };

        return _me;
    }
);