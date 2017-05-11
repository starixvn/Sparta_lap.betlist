define([
    "jquery",
    "common/querystring",
    "common/site", "../report",
    "./platformwinloss.option",
    "lib/jqgrid/jquery.jqgrid.additional",
    "components/pluginhelper/monthpicker",
    "components/pluginhelper/multiselect",
    "lib/jqgrid/jquery.jqGrid.en",
    "lib/jqgrid/jquery.jqGrid.min"],
    function ($, QueryString, Site, Report, PlatformWinLossOption, jqGridAdditional, MonthPicker, MultiSelect) {
        "use strict";

        var _me = {
            initialize: function () {
                Site.initialize();
                _me.initEvent();
                _me.initSetting();

                Site.addUserGuide({
                    resourceName: "PlatformWinLoss",
                    title: Site.getSingleResource("PlatformWinLoss")
                });

                MultiSelect.initialize('grid-products-multiselect', $("#productsData").val(), 'Code', 'Name', 'Status');
                MonthPicker.initMonthPicker();
            },

            initEvent: function () {
            },

            initSetting: function () {
            }
        };

        return _me;
    });