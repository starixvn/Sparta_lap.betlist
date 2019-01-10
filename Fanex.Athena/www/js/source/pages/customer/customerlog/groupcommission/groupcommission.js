"use strict";
define(["jquery", "common/site", "components/pluginhelper/autocomplete", "../common.option",
    "components/pluginhelper/monthpicker", "../customerlog.service"],
    function ($, Site, AutoComplete, CommonOption, MonthPicker, CustomerLogService) {
        var _me = {
            initialize: function () {
                CustomerLogService.initialize();
                MonthPicker.initMonthPicker({ minDate: $('#MinDate').val() });
                _me.initEvent();

                // Set menu active
                $('#groupcommlogTab').addClass('active');
            },

            initEvent: function () {
                $('#SubmitButton').on('click', function () {
                    _me.loadGrid();
                });

                $('#showall').on('change', function () {
                    CustomerLogService.checkShowAll();
                });
            },

            loadGrid: function () {
                var gridOption = CommonOption.gridOption;
                var url = _me.getUrl();

                var colNames = Site.clone(CommonOption.getColNames());
                gridOption.colNames = Site.getResources(colNames);

                var hasCheckbox = true;
                CustomerLogService.loadGrid(url, gridOption, hasCheckbox);
            },

            getUrl: function () {
                var fromDate = new Date($('#FromDate').val());
                var toDate = new Date($('#ToDate').val());

                return Site.resolveClientUrl('api/Log/GroupCommission?'
                    + '&Year=' + fromDate.getFullYear() + '&Month=' + (fromDate.getMonth() + 1)
                    + '&FromDay=' + fromDate.getDate() + '&ToDay=' + toDate.getDate());
            }
        };

        return _me;
    });