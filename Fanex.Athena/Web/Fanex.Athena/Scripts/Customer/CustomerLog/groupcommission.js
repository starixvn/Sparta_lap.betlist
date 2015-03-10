/*global Site, jqGridAdditional, CustomerSettingLogOption, CustomerLog*/
/// <reference path="/@JSense.js" />
"use strict";
var GroupCommLog = (function ($) {
    return {
        resizeGrid: function () {
            if ($("#GroupCommLogGrid").length === 0) {
                return;
            }
            var gridTop = $("#GroupCommLogGrid").offset().top;
            var gridHeight = window.innerHeight - gridTop - $(".ui-jqgrid-pager").height() - 30;
            gridHeight = Math.max(150, gridHeight);
            $(".ui-jqgrid-bdiv").css("height", "auto").css("max-height", gridHeight);
        },
        initialize: function () {
            GroupCommLog.initMonthPicker();
            CustomerLog.initAutoComplete();
            GroupCommLog.initEvent();

            // Set menu active
            $('#groupcommlog').addClass('activeTab');

            // Set username test default
            $('#Username').val(Site.getSingleResource("ExternalIdOrSystemId"));
            Site.addUserGuide({
                resourceName: "CustomerLog",
                title: Site.getSingleResource("CustomerLog")
            });
        },
        initMonthPicker: function () {
            $('#DateControl').monPickr({
                minDate: $('#MinDate').val(),
                showDateRange: true,
                fromDayId: 'MonthPickerFromDay',
                toDayId: 'MonthPickerToDay',
                month: new Date($('#FromDate').val()),
                fromDay: new Date($('#FromDate').val()).getDate(),
                toDay: new Date($('#ToDate').val()).getDate(),
                afterChangeMonth: function () {
                    $('#Year').val(new Date($("#DateControl").monPickr("getFromDate", "MM/dd/yyyy")).getFullYear());
                    $('#Month').val(new Date($("#DateControl").monPickr("getFromDate", "MM/dd/yyyy")).getMonth() + 1);
                },
                afterChangeDay: function () {
                    $('#FromDay').val(new Date($("#DateControl").monPickr("getFromDate", "MM/dd/yyyy")).getDate());
                    $('#ToDay').val(new Date($("#DateControl").monPickr("getToDate", "MM/dd/yyyy")).getDate());
                },
                log: function (/*msg*/) {
                    // console.warn(msg);
                }
            });
        },
        initEvent: function () {
            // username click
            $("#Username").click(function () {
                CustomerLog.setUsernameOnFocus();
            }).blur(function () {
                CustomerLog.setDefaultUsername();
            });

            // Submit button click
            $('#SubmitButton').on('click', function () {
                var isShowAll = $('#showall').is(':checked');
                if (CustomerLog.dateConstraint()) {
                    Site.blockUI();
                    GroupCommLog.loadGrid(isShowAll);
                }
                else {
                    alert(Site.getSingleResource('AlertLoginLogValidDayMsg'));
                }
            });

            // Show All click
            $('#showall').on('change', function () {
                $('#showall').is(':checked') ? $('.old-value').show() : $('.old-value').hide();
            });
        },
        loadGrid: function (isShowAll) {
            // Clear old grid before display new grid
            $('#GroupCommLogGrid').jqGrid('GridUnload');

            // Get param
            var url = GroupCommLog.initParam();

            // Get grid option
            var grid = CustomerSettingLogOption;
            var groupCommLogGrid = $('#GroupCommLogGrid');

            // Grid url
            grid.url = url;

            // Get colList, colModels
            grid.colNames = grid.getColumnNames();
            grid.colModel = grid.getColModels();

            // Grid pager
            grid.pager = '#GroupCommLogPager';

            // Grid load complete
            grid.gridComplete = function () {
                Site.unBlockUI();

                CustomerLog.checkShowAll(isShowAll);

                jqGridAdditional.gridComplete(this, groupCommLogGrid);

                // Check noinfo
                var recCount = groupCommLogGrid.getGridParam("reccount");
                if (recCount === 0) {
                    jqGridAdditional.setNoInfo(grid.colModel);
                }
                else {
                    var contentTable = $("table.ui-jqgrid-htable");
                    if (contentTable.find(".no-info-row").length > 0) {
                        $(".ui-jqgrid-htable tbody .no-info-row").remove();
                    }
                }

                setTimeout(GroupCommLog.resizeGrid, 100);
            };

            groupCommLogGrid.jqGrid(grid);
        },
        initParam: function () {
            var userName = $('#Username').val();
            if (userName === Site.getSingleResource("ExternalIdOrSystemId")) {
                userName = '';
            }
            var fromDay = $('#FromDay').val();
            var toDay = $('#ToDay').val();
            var year = $('#Year').val();
            var month = $('#Month').val();
            var param = Site.resolveClientUrl('api/Log/GroupCommission?Year=' + year + '&Month=' + month + '&FromDay=' + fromDay + '&ToDay=' + toDay);
            return param;
        }
    };
})(jQuery);

$(window).load(function () {
    GroupCommLog.initialize();
});