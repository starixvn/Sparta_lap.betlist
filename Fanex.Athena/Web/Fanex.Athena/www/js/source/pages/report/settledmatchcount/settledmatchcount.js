"use strict";
define(["jquery", "common/site"], function ($, Site) {
    var _me = {
        initialize: function () {
            Site.initialize();
            Site.addUserGuide({
                resourceName: "SettledMatchCount",
                title: Site.getSingleResource("SettledMatchCount"),
                height: 220
            });
        }
    };

    return _me;
});