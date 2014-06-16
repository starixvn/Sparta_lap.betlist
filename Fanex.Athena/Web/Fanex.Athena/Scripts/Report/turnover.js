/*global Site*/
/// <reference path="/@JSense.js" />
"use strict";
var Turnover = (function ($) {
    
    return {
        defaultUserName: Site.getSingleResource('ExternalIdOrSystemId'),
        resizeGrid: function () {
            var $grid = $("#data-content");

            if ($grid.length === 0) {
                return;
            }
            
            var gridTop = $grid.offset().top;
            var gridHeight = window.innerHeight - gridTop - $("#total-content").height() - 10;
            gridHeight = Math.max(100, gridHeight);
            $grid.css("max-height", gridHeight);
        },
        initialize: function () {
            
                Turnover.initMonthPicker();
                Turnover.initEvent();
                Turnover.initAutoComplete();
                Turnover.setDefaultUsername();
            
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
                    //console.warn(msg);
                }
            });
        },

        initEvent: function () {
            $('#Username').focus(function () {
                Turnover.setUsernameOnFocus();
            });

            $('#Username').blur(function () {
                Turnover.setDefaultUsername();
            });

            $("#SubmitButton").click(function () {
                Site.blockUI();
                $('#IsExportExcel').val(false);
                $("#TurnoverForm").submit();
            });
            return;
            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                $("#TurnoverForm").submit();
            });

            $("#data-content").scroll(function () {
                $('#header-content').scrollLeft($('#data-content').scrollLeft());
                $('#total-content').scrollLeft($('#data-content').scrollLeft());
            });
        },

        initAutoComplete: function () {
            var txtUserName = $('#Username');
            var serUrl = Site.resolveClientUrl("CustomerAPI/SearchCustomer?licenseeType=&currencyId=0");
            if (txtUserName.length > 0) {
                txtUserName.autocomplete({ serviceUrl: serUrl, minChars: 2 });
            }
        },

        setDefaultUsername: function () {
            var username = $('#Username').val();
            if (username === '' || username === Turnover.defaultUserName) {
                $('#Username').val(Turnover.defaultUserName);
                $('#Username').attr('class', 'default-username');
            }
            else {
                $('#Username').attr('class', '');
            }
        },

        setUsernameOnFocus: function () {
            var username = $('#Username').val();
            if (username === Turnover.defaultUserName) {
                $('#Username').val('');
                $('#Username').removeAttr('class');
            }
        }
    };
})(jQuery);

$(window).load(function () {
    Turnover.initialize();
  //  Turnover.resizeGrid();
});
$(window).resize(function () {
   // Turnover.resizeGrid();
});

$(window).unload(function () {
    $(window).unbind("resize");
});