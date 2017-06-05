"use strict";
define(["jquery", "common/site", "components/pluginhelper/autocomplete", "./loginlog.option", "../customerlog.service"],
    function ($, Site, AutoComplete, LoginLogOption, CustomerLog) {
        var _me = {
            initialize: function () {
                CustomerLog.initialize();
                _me.initEvent();
                AutoComplete.initialize({
                    url: Site.resolveClientUrl("CustomerAPI/SearchCustomer"),
                    target: "#Username"
                });

                // Set menu active
                $('#loginlogTab').addClass('active');
            },

            initEvent: function () {
                $('#SubmitButton').on('click', function () {
                    _me.loadGrid();
                });

                $("#Username").keyup(function (e) {
                    if (e.keyCode === 13) {
                        _me.loadGrid();
                    }
                });
            },

            loadGrid: function () {
                var gridOption = LoginLogOption.gridOption;
                var colNames = Site.clone(LoginLogOption.getColNames());
                gridOption.colNames = Site.getResources(colNames);
                gridOption.url = _me.getUrl();

                CustomerLog.loadGrid(gridOption);
            },

            getUrl: function () {
                var userName = $('#Username').val();
                return Site.resolveClientUrl('api/Log/LoginLog?UserName=' + encodeURIComponent(userName));
            }
        };

        return _me;
    });