"use strict";
define(["jquery", "common/site", "components/pluginhelper/autocomplete", "./loginlog.option",
    "lib/jqgrid/jquery.jqgrid.additional", "lib/jqgrid/jquery.jqGrid.en", "lib/jqgrid/jquery.jqGrid.min"],
    function ($, Site, AutoComplete, LoginLogOption, jqGridAdditional) {
        var _me = {
            initialize: function () {
                Site.initialize();
                _me.initEvent();
                AutoComplete.initialize({
                    url: Site.resolveClientUrl("CustomerAPI/SearchCustomer"),
                    target: "#Username"
                });
                Site.addUserGuide({
                    resourceName: "CustomerLog",
                    title: Site.getSingleResource("CustomerLog")
                });
                // Set menu active
                $('#loginlogTab').addClass('active');
            },

            initEvent: function () {
                $('#SubmitButton').on('click', function () {
                    _me.submit();
                });

                $("#Username").keyup(function (e) {
                    if (e.keyCode === 13) {
                        _me.submit();
                    }
                });
            },

            submit: function () {
                Site.blockUI();
                _me.loadGrid();
            },

            loadGrid: function () {
                var ReportId = "ReportGrid";
                jqGridAdditional.clearGrid(ReportId);
                var $reportGrid = $('#ReportGrid');

                var gridOption = LoginLogOption.gridOption;
                var colNames = Site.clone(LoginLogOption.getColNames());
                gridOption.colNames = Site.getResources(colNames);
                gridOption.url = _me.getUrl();

                gridOption.gridComplete = function () {
                    jqGridAdditional.gridComplete();

                    // Check noinfo
                    var recCount = $reportGrid.getGridParam("reccount");
                    if (recCount === 0) {
                        jqGridAdditional.clearGrid(ReportId);
                        jqGridAdditional.setNoInfo();
                    }
                    else {
                        jqGridAdditional.updateGridHeight();
                        jqGridAdditional.updateGridWidth($reportGrid);
                    }

                    Site.unBlockUI();
                };
                $reportGrid.jqGrid(gridOption);
            },

            getUrl: function () {
                var userName = $('#Username').val();
                return Site.resolveClientUrl('api/Log/LoginLog?UserName=' + encodeURIComponent(userName));
            }
        };

        return _me;
    });