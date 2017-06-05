"use strict";
define(["jquery", "common/site", "components/pluginhelper/autocomplete", "../common.option",
    "components/pluginhelper/monthpicker", "../customerlog.service"],
    function ($, Site, AutoComplete, CommonOption, MonthPicker, CustomerLog) {
        var _me = {
            initialize: function () {
                CustomerLog.initialize();
                _me.initEvent();

                AutoComplete.initialize({
                    url: Site.resolveClientUrl("CustomerAPI/SearchCustomer"),
                    target: "#Username"
                });

                MonthPicker.initMonthPicker({ minDate: $('#MinDate').val() });
                

                // Set menu active
                $('#customersettinglogTab').addClass('active');
            },

            initEvent: function () {
                $('#SubmitButton').on('click', function () {
                    _me.loadGrid();
                });

                $('#Username').keyup(function (e) {
                    if (e.keyCode === 13) {
                        _me.loadGrid();
                    }
                });

                $('#showall').on('change', function () {
                    CustomerLog.checkShowAll();
                });
            },

            loadGrid: function () {
                var gridOption = CommonOption.gridOption;
                var url = _me.getUrl();
                gridOption.url = url + '&ViewById=' + $('#ViewBy').val();
                
                var colNames = Site.clone(CommonOption.getColNames());
                gridOption.colNames = Site.getResources(colNames);

                var hasCheckbox = true;
                CustomerLog.loadGrid(gridOption, hasCheckbox);
            },

            getUrl: function () {
                var userName = $('#Username').val();
                var fromDate = new Date($('#FromDate').val());
                var toDate = new Date($('#ToDate').val());

                return Site.resolveClientUrl('api/Log/CustomerSetting?UserName=' + encodeURIComponent(userName)
                    + '&Year=' + fromDate.getFullYear() + '&Month=' + (fromDate.getMonth() + 1)
                    + '&FromDay=' + fromDate.getDate() + '&ToDay=' + toDate.getDate());
            }
        };

        return _me;
    });