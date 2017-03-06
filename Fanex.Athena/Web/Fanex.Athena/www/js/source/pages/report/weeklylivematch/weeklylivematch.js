"use strict";
define(["jquery", "common/site", "./weeklylivematch.option", "lib/jqgrid/jquery.jqgrid.additional", "components/pluginhelper/daterangepicker",
        "lib/jqgrid/jquery.jqGrid.en", "lib/jqgrid/jquery.jqGrid.min", "lib/linq", "extensions/jquery.extension"],
        function ($, Site, WeeklyLiveMatchOption, jqGridAdditional, DatePicker) {
            var VALID_MONTH_RANGE = 2;
            var SPORT_COLUMN_INDEX = 1;
            var START_WEEK_COLUMN_INDEX = 2;

            var ajaxRequest = null;
            var _cacheData = null;
            var _sortColumn = null;
            var _sortDirection = null;

            var _me = {
                initialize: function () {
                    Site.initialize();
                    _me.initEvent();

                    Site.addUserGuide({
                        resourceName: "WeeklyLiveMatches",
                        title: Site.getSingleResource("WeeklyLiveMatches"),
                        height: 380
                    });
                    WeeklyLiveMatchOption.gridOption.onSortCol = function (index) {
                        Site.blockUI();
                        window.setTimeout(function () {
                            if (_sortColumn === index) {
                                _sortDirection = _sortDirection === "asc" ? "desc" : "asc";
                            } else {
                                _sortDirection = "asc";
                                _sortColumn = index;
                            }
                            _cacheData.Data = _me.sortData(_cacheData.Data);

                            _me.bindData(_cacheData);

                            Site.unBlockUI();
                        }, 100);
                        return 'stop';
                    };

                    var firstDayOfWeekIsMonday = true;
                    DatePicker.initDateRangePicker(firstDayOfWeekIsMonday);
                },

                sortData: function (arrayData) {
                    var columnIndex = SPORT_COLUMN_INDEX;
                    if (_sortColumn.indexOf('Week') === 0) {
                        columnIndex = Number(_sortColumn.replace('Week', '')) + START_WEEK_COLUMN_INDEX;
                        arrayData = Enumerable.From(arrayData).OrderBy(function (x) { return x.Cell[SPORT_COLUMN_INDEX]; }).ToArray();
                    }

                    for (var i = 0; i < arrayData.length; i++) {
                        arrayData[i].SortingValue = arrayData[i].Cell[columnIndex];
                    }

                    var data = _sortDirection === "asc" ?
                 Enumerable.From(arrayData).OrderBy(function (x) { return x.SortingValue; }).ToArray()
                 : Enumerable.From(arrayData).OrderByDescending(function (x) { return x.SortingValue; }).ToArray();

                    // Re-assign Index column.
                    for (var j = 0; j < data.length; j++) {
                        data[j].Cell[0] = j + 1;
                    }

                    return data;
                },

                initEvent: function () {
                    Site.setConstraintMonthRange(VALID_MONTH_RANGE, WeeklyLiveMatchOption.urlExcelData);

                    $('#SubmitButton').click(function () {
                        $('#IsExportExcel').val(false);
                        _me.submit();
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
                            _me.bindData(response);
                        },
                        beforeSend: Site.blockUI,
                        complete: Site.unBlockUI
                    };
                    if (ajaxRequest !== null) {
                        ajaxRequest.abort();
                    }

                    ajaxRequest = $.ajax(option);

                    return true;
                },

                bindData: function (response) {
                    jqGridAdditional.clearGrid("ReportGrid");
                    var numOfRow = response.Data.length;
                    if (response === null || numOfRow === 0) {
                        jqGridAdditional.setNoInfo();
                        return;
                    }

                    var reportGrid = $("#ReportGrid");
                    var userData = {};
                    var totalData = response.TotalData;
                    for (var i = 0; i < totalData.length; i++) {
                        userData['Week' + i] = totalData[i];
                    }

                    var gridOption = WeeklyLiveMatchOption.getGridOption(response.NumberOfWeeks, response.WeekHeaders);
                    gridOption.data = response.Data;
                    gridOption.userData = userData;
                    gridOption.rowTotal = numOfRow;

                    gridOption.gridComplete = function () {
                        jqGridAdditional.gridComplete();
                        _me.resizeGrid();

                        Site.unBlockUI();
                    };

                    reportGrid.jqGrid(gridOption);

                    _me.setTotalRow();

                    jqGridAdditional.addSortableHeader(reportGrid, _sortColumn, _sortDirection, true);
                },

                setTotalRow: function () {
                    var $totalRow = $('.ui-jqgrid-ftable').find('.footrow');
                    $totalRow.addClass('total');
                    var $firstCell = $totalRow.find('td:eq(0)');
                    var $secondCell = $firstCell.next();
                    $secondCell.hide();
                    $firstCell.attr('colspan', 2).html(Site.getSingleResource("Total"));
                },

                resizeGrid: function () {
                    var reportGrid = $("#ReportGrid");
                    jqGridAdditional.updateGridHeight();
                    jqGridAdditional.updateGridWidth(reportGrid);
                }
            };

            return _me;
        });