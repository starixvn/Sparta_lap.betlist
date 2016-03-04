/*global jqGridAdditional, WeeklyLiveMatchOption, Site */
/// <reference path="/@JSense.js" />
///
var WeeklyLiveMatch = (function ($) {
    "use strict";
    var VALID_MONTH_RANGE = 2;

    var ajaxRequest = null;
    var _resizeTimer = null;

    return {
        resizeGrid: function () {
            if ($(".ui-jqgrid-bdiv").length === 0) {
                return;
            }
            clearTimeout(_resizeTimer);
            _resizeTimer = setTimeout(function () {
                var $bdiv = $(".ui-jqgrid-bdiv");

                var gridTop = $bdiv.offset().top;
                var gridHeight = window.innerHeight - gridTop - $(".ui-jqgrid-sdiv:visible").height()
                            - $(".ui-jqgrid-pager:visible").height() - 30;
                gridHeight = Math.max(120, gridHeight);

                var gridContainer = $("#gview_ReportGrid");
                var bodyHeight = window.innerHeight - gridContainer.offset().top;
                var actualHeight = $("#ReportGrid").height();
                var contentHeight = actualHeight > bodyHeight ? bodyHeight : actualHeight;
                var tableWidth = $("#ReportGrid").width();
                var contentWidth = $bdiv.width();
                var newHeight = tableWidth > contentWidth ? contentHeight + 18 : contentHeight;
                $bdiv.height(Math.min(newHeight, gridHeight))
                                    .css("max-height", gridHeight);
            }, 100);
        },

        initialize: function () {
            WeeklyLiveMatch.initEvent();

            Site.addUserGuide({
                resourceName: "WeeklyLiveMatches",
                title: Site.getSingleResource("WeeklyLiveMatches"),
                height: 380
            });
        },

        initEvent: function () {
            Site.setConstraintMonthRange(VALID_MONTH_RANGE, WeeklyLiveMatchOption.urlExcelData);

            $('#SubmitButton').click(function () {
                $('#IsExportExcel').val(false);
                WeeklyLiveMatch.submit();
            });

            $('#ExportToExcel').click(function () {
                $('#IsExportExcel').val(true);
                var $mainForm = $("#mainform");
                var url = Site.resolveClientUrl(WeeklyLiveMatchOption.urlExcelData);
                $mainForm.attr("action", url);
                $mainForm.submit();
                $('#IsExportExcel').val(false);
                return false;
            });
        },

        submit: function () {
            var mainForm = $("#mainform");
            var data = mainForm.serializeObject();

            var option = {
                type: "POST",
                data: data,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(WeeklyLiveMatchOption.urlData),
                success: function (response) {
                    WeeklyLiveMatch.bindData(data.FromDate, data.ToDate, response);
                },
                beforeSend: Site.blockUI,
                complete: Site.unBlockUI
            };
            if (ajaxRequest != null) {
                ajaxRequest.abort();
            }

            ajaxRequest = $.ajax(option);

            return true;
        },

        bindData: function (fromDate, toDate, response) {
            jqGridAdditional.clearGrid("ReportGrid");

            var reportGrid = $("#ReportGrid");
            var numOfRow = response.Data.length;
            var userData = {};
            var totalData = response.TotalData;
            for (var i = 0; i < totalData.length; i++) {
                userData['Week' + i] = totalData[i];
            }

            var gridOption = WeeklyLiveMatchOption.getGridOption(response.NumberOfWeeks, response.WeekHeaders);
            gridOption.width = gridOption.colModel.length < 7 ? 600 : undefined;
            gridOption.data = response.Data;
            gridOption.userData = userData;
            gridOption.rowTotal = numOfRow;

            gridOption.gridComplete = function () {
                jqGridAdditional.gridComplete(this, gridOption, 1);
                if (response == null || numOfRow === 0) {
                    jqGridAdditional.setNoInfo(gridOption.colModel);
                }
                else {
                    WeeklyLiveMatch.resizeGrid();
                }

                Site.unBlockUI();
            };

            reportGrid.jqGrid(gridOption);

            reportGrid.jqGrid('showCol', 'rn');

            jqGridAdditional.resetLayout(reportGrid);

            var gridWidth = reportGrid.width();
            // for the vertical scrollbar not overlap the grid content
            if (gridWidth < 500) {
                gridWidth = 500;
            } else {
                gridWidth += 20;
            }

            gridWidth = Math.min(1040, gridWidth);
            $(".content-filter").width(gridWidth);
            reportGrid.jqGrid("setGridWidth", gridWidth);

            if (numOfRow > 0) {
                var totalText = Site.getSingleResource("Total");
                var $footerTable = $('.ui-jqgrid-ftable');
                var $firstCell = $footerTable.find('td:eq(0)');
                var $secondCell = $firstCell.next();

                $firstCell.attr('colspan', 2)
                    .css('width', $firstCell.width() + $secondCell.width() + 4)
                    .html(totalText);
                $secondCell.hide();
            }
        }

        //#end
    };
})(jQuery);

$(window).load(function () {
    "use strict";
    WeeklyLiveMatch.initialize();
});

$(window).resize(function () {
    "use strict";
    WeeklyLiveMatch.resizeGrid();
});

$(window).unload(function () {
    "use strict";
    $(window).unbind("resize");
});