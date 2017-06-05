"use strict";
define(["jquery", "common/site", "components/pluginhelper/autocomplete", "../common.option", "components/pluginhelper/monthpicker",
    "lib/jqgrid/jquery.jqgrid.additional", "lib/jqgrid/jquery.jqGrid.en", "lib/jqgrid/jquery.jqGrid.min"],
    function ($, Site, AutoComplete, CommonOption, MonthPicker, jqGridAdditional) {
        var _me = {
            initialize: function () {
                Site.initialize();
                Site.addUserGuide({
                    resourceName: "CustomerLog",
                    title: Site.getSingleResource("CustomerLog")
                });
                AutoComplete.initialize({
                    url: Site.resolveClientUrl("CustomerAPI/SearchCustomer"),
                    target: "#Username"
                });

                MonthPicker.initMonthPicker({ minDate: $('#MinDate').val() });

                _me.initEvent();

                // Set menu active
                $('#customersettinglogTab').addClass('active');
            },

            initEvent: function () {
                $('#SubmitButton').on('click', function () {
                    _me.submit();
                });

                $('#Username').keyup(function (e) {
                    if (e.keyCode === 13) {
                        _me.submit();
                    }
                });

                $('#showall').on('change', function () {
                    _me.checkShowAll();
                });
            },

            checkShowAll: function(){
                var checked = $('#showall').is(':checked');
                $('.old-value').toggle(checked);
            },

            submit: function () {
                Site.blockUI();
                _me.loadGrid();
            },

            loadGrid: function () {
                var ReportId = "ReportGrid";
                jqGridAdditional.clearGrid(ReportId);
                var $reportGrid = $('#ReportGrid');

                var colNames = Site.clone(CommonOption.getColNames());
                CommonOption.colNames = Site.getResources(colNames);

                var gridOption = CommonOption.gridOption;
                var url = _me.getUrl();
                gridOption.url = url + '&ViewById=' + $('#ViewBy').val();

                gridOption.gridComplete = function () {
                    jqGridAdditional.gridComplete();

                    // Check noinfo
                    var recCount = $reportGrid.getGridParam("reccount");
                    if (recCount === 0) {
                        jqGridAdditional.clearGrid(ReportId);
                        jqGridAdditional.setNoInfo();
                    }
                    else {
                        _me.checkShowAll();
                        jqGridAdditional.updateGridHeight();
                        jqGridAdditional.updateGridWidth($reportGrid);
                    }

                    Site.unBlockUI();
                };
                $reportGrid.jqGrid(gridOption);
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