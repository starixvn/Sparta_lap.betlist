"use strict";
define(["jquery", "common/site", "../common/betsettingbase"],
    function ($, Site, BetSettingBase) {
        var _me = {
            initialize: function () {
                Site.initialize();
                BetSettingBase.initialize();
            },
        };

        return _me;
    });