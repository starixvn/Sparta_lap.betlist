/// <reference path="/@JSense.js" />
var CustomerSettingLog = (function ($) {
    return {
        initialize: function () {
            CustomerSettingLog.initMonthPicker();
            CustomerLog.initAutoComplete();
            CustomerSettingLog.initEvent();

            // Set menu active
            $('#customersettinglog').addClass('activeTab');

            // Set username test default
            $('#Username').val(Site.getSingleResource("ExternalIdOrSystemId"));
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
                log: function (msg) {
                  //  console.warn(msg);
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
                    CustomerSettingLog.loadGrid(isShowAll);
                }
                else {
                    alert(Site.getSingleResource("AlertLoginLogValidDayMsg"));
                }

            });

            // Show All click
            $('#showall').on('change', function () {
                $('#showall').is(':checked') ? $('.old-value').show() : $('.old-value').hide();
            });
        },
        loadGrid: function (isShowAll) {
            // Clear old grid before display new grid
            $('#CustomerSettingLogGrid').jqGrid('GridUnload');

            // Get param
            var url = CustomerSettingLog.initParam();

            // Get grid option
            var grid = CustomerSettingLogOption;
            var CustomerSettingLogGrid = $('#CustomerSettingLogGrid');

            var viewById = $('#ViewBy').val();
            grid.url = url + '&ViewById=' + viewById;

            // Get colList, colModels
            grid.colNames = grid.getColumnNames();
            grid.colModel = grid.getColModels();

            // Grid load complete
            grid.gridComplete = function () {
                Site.unBlockUI();

                CustomerLog.checkShowAll(isShowAll);

                jqGridAdditional.gridComplete(this, grid);

                // Check noinfo
                var recCount = CustomerSettingLogGrid.getGridParam("reccount");
                if (recCount == 0) {
                    jqGridAdditional.setNoInfo(grid.colModel);
                }
                else {
                    var contentTable = $("table.ui-jqgrid-htable");
                    if (contentTable.find(".no-info-row").length > 0) {
                        $(".ui-jqgrid-htable tbody .no-info-row").remove();
                    }
                }
            };

            CustomerSettingLogGrid.jqGrid(grid);
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
            var param = Site.resolveClientUrl('api/Log/CustomerSetting?UserName=' + encodeURIComponent(userName) + '&Year=' + year + '&Month=' + month + '&FromDay=' + fromDay + '&ToDay=' + toDay);
            return param;
        }
    };
})(jQuery);

$(window).load(function () {
    CustomerSettingLog.initialize();
});
