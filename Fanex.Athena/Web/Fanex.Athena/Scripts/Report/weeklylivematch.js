/*global jqGridAdditional, WeeklyLiveMatchOption, Site, JSLINQ */
/// <reference path="/@JSense.js" />
///
var WeeklyLiveMatch = (function ($) {
    "use strict";
    var VALID_MONTH_RANGE = 2;
    var SPORT_COLUMN_INDEX = 1;
    var START_WEEK_COLUMN_INDEX = 2;
    
    var ajaxRequest = null;
    var _resizeTimer = null;
    var _cacheData = null;
    var _sortColumn = null;
    var _sortDirection = null;
    var _submitData = {};

    function sortData(arrayData) {
        var columnIndex = SPORT_COLUMN_INDEX;
        if (_sortColumn.indexOf('Week') === 0) {
            columnIndex = Number(_sortColumn.replace('Week', '')) + START_WEEK_COLUMN_INDEX;
            arrayData = JSLINQ(arrayData).OrderBy(function (x) { return x.Cell[SPORT_COLUMN_INDEX]; }).items;
        }

        for (var i = 0; i < arrayData.length; i++) {
            arrayData[i].SortingValue = arrayData[i].Cell[columnIndex];
        }

        var data = _sortDirection === "asc" ?
     JSLINQ(arrayData).OrderBy(function (x) { return x.SortingValue; }).items
     : JSLINQ(arrayData).OrderByDescending(function (x) { return x.SortingValue; }).items;

        // Re-assign Index column.
        for (var j = 0; j < data.length; j++) {
            data[j].Cell[0] = j + 1;
        }

        return data;
    }

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
                height: 460
            });
            WeeklyLiveMatchOption.gridOption.onSortCol = function (index/*, columnIndex, sortOrder*/) {
                Site.blockUI();
                window.setTimeout(function () {
                    if (_sortColumn === index) {
                        _sortDirection = _sortDirection === "asc" ? "desc" : "asc";
                    } else {
                        _sortDirection = "asc";
                        _sortColumn = index;
                    }
                    _cacheData.Data = sortData(_cacheData.Data);

                    WeeklyLiveMatch.bindData(_submitData.FromDate, _submitData.ToDate, _cacheData);

                    Site.unBlockUI();
                }, 100);
                return 'stop';
            };
        },

        initEvent: function () {
            Site.setConstraintMonthRange(VALID_MONTH_RANGE, WeeklyLiveMatchOption.urlExcelData);

            $('#SubmitButton').click(function () {
                $('#IsExportExcel').val(false);
                WeeklyLiveMatch.submit();
            });

            $('#ExportToExcel').click(function () {
                var mainForm = $("#mainform");
                var data = mainForm.serializeObject();
                data.IsExportExcel = true;
                data.SortColumn = _sortColumn;
                data.SortDirection = _sortDirection;
                Site.exportExcel(data, Site.resolveClientUrl(WeeklyLiveMatchOption.urlExcelData));
                return false;
            });
        },

        submit: function () {
            var mainForm = $("#mainform");
            var data = mainForm.serializeObject();
            data.SortColumn = _sortColumn;
            data.SortDirection = _sortDirection;
            var option = {
                type: "POST",
                data: data,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(WeeklyLiveMatchOption.urlData),
                success: function (response) {
                    _cacheData = response;
                    _submitData = {
                        FromDate: data.FromDate,
                        ToDate: data.ToDate
                    };
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

            var gridWidth = reportGrid.width() + 18;

            gridWidth = Math.min($(window).width() - 20, gridWidth);
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

            reportGrid.closest(".ui-jqgrid-view")
                .find(".ui-jqgrid-hdiv .ui-jqgrid-htable .ui-jqgrid-labels > .ui-th-column > .ui-jqgrid-sortable")
                .each(function (index, element) {
                    var $this = $(element);
                    $this.find(".s-ico").remove();

                    if (element.id.indexOf(_sortColumn) > -1) {
                        var sortClass = "sort-" + _sortDirection;
                        var sortSpan = $("<span style='display: inline; margin: 2px 0px 0px 2px'></span");
                        sortSpan.addClass(sortClass);
                        $this.append(sortSpan);
                        $this.addClass("sorted-heading");
                    }
                });
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