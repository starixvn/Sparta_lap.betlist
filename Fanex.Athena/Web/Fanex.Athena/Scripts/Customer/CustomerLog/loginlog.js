/*global Site, LogInLogOption, jqGridAdditional, CustomerLog*/
/// <reference path="/@JSense.js" />
"use strict";
var LogInLog = (function ($) {
    return {
        initialize: function () {
            CustomerLog.initAutoComplete();
            LogInLog.initEvent();
            // Set menu active
            $('#loginlog').addClass('activeTab');
            // Set username test default
            $('#Username').val(Site.getSingleResource("ExternalIdOrSystemId"));
            Site.addUserGuide({
                resourceName: "CustomerLog",
                title: Site.getSingleResource("CustomerLog")
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
                if (LogInLog.dateConstraint()) {
                    Site.blockUI();
                    LogInLog.loadGrid();
                }
                else {
                    alert(Site.getSingleResource("AlertLoginLogValidDayMsg"));
                }
            });
        },
        dateConstraint: function () {
            var fromDay = parseInt($('#MonthPickerFromDay .selectedDay').html());
            var toDay = parseInt($('#MonthPickerToDay .selectedDay').html());
            return fromDay > toDay ? false : true;
        },
        loadGrid: function () {
            $('#LogInLogGrid').jqGrid('GridUnload');
            var logInLogGrid = $('#LogInLogGrid');

            var gridOption = LogInLogOption.gridOption;
            gridOption.url = LogInLog.initParam();

            // Get colList, colModels
            gridOption.colNames = LogInLogOption.getColumnNames();
            gridOption.colModel = LogInLogOption.getColModels();

            // Grid load complete
            gridOption.gridComplete = function () {
                Site.unBlockUI();

                jqGridAdditional.gridComplete(this, gridOption);

                // Check noinfo
                var recCount = logInLogGrid.getGridParam("reccount");
                if (recCount === 0) {
                    jqGridAdditional.setNoInfoWithColspan(5);
                }
                else {
                    var contentTable = $("table.ui-jqgrid-htable");
                    if (contentTable.find(".no-info-row").length > 0) {
                        $(".ui-jqgrid-htable tbody .no-info-row").remove();
                    }
                }
            };
            logInLogGrid.jqGrid(gridOption);
        },
        initParam: function () {
            var userName = $('#Username').val();
            if (userName === Site.getSingleResource("ExternalIdOrSystemId")) {
                userName = '';
            }
            var param = Site.resolveClientUrl('api/Log/LoginLog?UserName=' + encodeURIComponent(userName));

            return param;
        }
    };
})(jQuery);

$(window).load(function () {
    LogInLog.initialize();
});