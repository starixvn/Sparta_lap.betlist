"use strict";
define(["jquery", "common/querystring", "common/site", "components/pluginhelper/daterangepicker"], function ($, QueryString, Site, DatePicker) {
    var _me = {
        initialize: function () {
            Site.initialize();
            DatePicker.initDateRangePicker();
        }
    };

    return _me;
});