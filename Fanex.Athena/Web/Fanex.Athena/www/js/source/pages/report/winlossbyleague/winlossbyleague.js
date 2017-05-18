define(["jquery", "common/site", "common/querystring", "lib/jqgrid/jquery.jqgrid.additional", "./winlossbyleague.option",
    "./winlossbyleague.filter", "../report", "moment", "components/paging",
    "extensions/jquery.extension", "lib/jqgrid/jquery.jqGrid.en", "lib/jqgrid/jquery.jqGrid.min"],
    function ($, Site, QueryString, jqGridAdditional, WinLossByLeagueOption, WinLossByLeagueFilter, Report, moment, PagingHeper) {
        "use strict";

        var ajaxRequest = null;

        var _level = 0;
        var _leagueid = -1;
        var _leaguename = null;
        var _postdata = {};
        var _currencyName = null;

        var _pagingOption = {
            pageindex: 1,
            pagesize: 50,
            pagelist: [50, 100, 1000],
            totalrow: 1,
            elementcontainer: "#paging",
            method: "AJAX",
            ajaxfunction: "window.WinLossByLeague.ajaxPaging()",
            alwayshow: true
        };

        var _me = {
            //#region Init
            initialize: function () {
                Site.initialize();
                _me.initEvent();
                WinLossByLeagueFilter.initialize();

                window.WinLossByLeague = {
                    ajaxPaging: _me.ajaxPaging
                };
            },

            initEvent: function () {
                $('#SubmitButton').click(function () {
                    _me.submit();
                });

                $("body").on("click", "#leagueHeader", function () {
                    _pagingOption.pageindex = 1;
                    var leaguename = $(this).html();
                    _me.drillDown($(this).data("leagueid"), leaguename);
                });

                $("body").on("click", ".drilldown", function () {
                    var custid = $(this).data("custid");
                    var externalid = $(this).data("externalid");
                    var systemid = $(this).data("systemid");
                    _me.drillDownBetList(custid, externalid, systemid);
                });

                $("#level-link-container").on("click", ".level-link", function () {
                    var level = Number($(this).attr("data-level"));
                    if (level === 0) {
                        _me.submit();
                    }
                });

                $("#ExportToExcel").click(function (e) {
                    _postdata.LeagueName = _leaguename;
                    _postdata.Level = _level;
                    Site.exportExcel(_postdata, Site.resolveClientUrl("api/WinLossByLeagueReport/Excel"));
                    e.preventDefault();
                });

                $(window).on("resize", function () {
                    _me.resizeGrid();
                });

                $(window).on("unload", function () {
                    $(window).off("resize");
                });
            },

            //#endregion

            //#region Utilities
            resizeGrid: function () {
                var reportGrid = $("#ReportGrid");
                jqGridAdditional.updateGridHeight();
                jqGridAdditional.updateGridWidth(reportGrid);
            },

            setReportTitle: function (level, minOdds, fromdate, todate) {
                var caption = null;
                if (level === 0) {
                    caption = Site.getSingleResource('WinLossByLeague');
                }
                else {
                    caption = _leaguename + " > " + Site.getSingleResource('CustomerWinLoss');
                }

                var titlePattern = caption + ', {0} → {1} - {2} - {3}';
                var viewinKey = Site.getSingleResource("Currency") + ": ";
                var currencyStr = viewinKey + " " + (_postdata.BaseCurrency === "" ? _currencyName : _postdata.BaseCurrency);

                var minOddStr = Site.getSingleResource("MinimumOdds") + " : " + minOdds;
                var title = $.format(titlePattern, moment(fromdate).format("M/DD/YYYY"), moment(todate).format("M/DD/YYYY"), currencyStr, minOddStr);

                _postdata.ReportTitle = title;
                $(".report-title").html(title);
                $(".panel-heading").removeClass("hide");
            },
            //#endregion

            //#region Get Data
            ajaxComplete: function (level, response) {
                if (response !== null) {
                    if (level === 0) {
                        _me.setReportTitle(0, _postdata.Odds, _postdata.FromDate, _postdata.ToDate);
                    }

                    _me.bindData(level, response);
                    Report.renderBreadCrumb(level);
                }
            },

            submit: function () {
                if (!WinLossByLeagueFilter.isValid()) {
                    return;
                }
                Site.blockUI();
                _level = 0;

                _postdata = $("#SearchForm").serializeObject();
                _currencyName = $("#CurrencyName").val().toUpperCase();
                _postdata.BaseCurrency = _postdata.BaseCurrency.toUpperCase();
                _leaguename = $("#League option:selected").text();

                var option = {
                    type: "POST",
                    data: _postdata,
                    dataType: "json",
                    contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                    cache: false,
                    url: Site.resolveClientUrl(WinLossByLeagueOption.urlData),
                    success: function (response) {
                        _me.ajaxComplete(0, response);
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                };
                if (ajaxRequest !== null) {
                    ajaxRequest.abort();
                }
                ajaxRequest = $.ajax(option);

                return true;
            },

            //#endregion

            //#region drillDown
            drillDownBetList: function (custId, externalId, systemId) {
                var temp = {
                    IsConvert: _postdata.BaseCurrency !== "",
                    CurrencyName: _currencyName,
                    CustId: custId,
                    Level: 2,
                    ExternalId: externalId,
                    SystemId: systemId,
                    LeagueId: _leagueid
                };
                var param = $.extend({}, _postdata, temp);

                WinLossByLeagueFilter.buildProductsParams(param, _postdata);

                var url = Site.resolveClientUrl("WinlossByLeagueBetList/Index?") + $.param(param);
                window.open(url, "", "modal=no");
            },

            drillDown: function (leagueid, leaguename) {
                _level = 1;
                _leagueid = leagueid;
                _leaguename = leaguename;

                var temp = {
                    Level: _level,
                    League: leagueid,
                    pageindex: _pagingOption.pageindex,
                    pagesize: _pagingOption.pagesize
                };
                var param = $.extend({}, _postdata, temp);

                WinLossByLeagueFilter.buildProductsParams(param, _postdata);

                var option = {
                    type: "POST",
                    data: param,
                    dataType: "json",
                    cache: false,
                    url: Site.resolveClientUrl(WinLossByLeagueOption.urlData),
                    success: function (response) {
                        _me.ajaxComplete(_level, response);
                        if (response !== null && response.Data.length > 0) {
                            _me.setReportTitle(1, _postdata.Odds, _postdata.FromDate, _postdata.ToDate);
                            _pagingOption.totalrow = parseInt(response.TotalRows);
                            if (_pagingOption.totalrow > 0) {
                                $("#paging").show();
                                PagingHeper.render(_pagingOption);
                                _me.resizeGrid();
                            }
                        }
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    }
                };
                if (ajaxRequest !== null) {
                    ajaxRequest.abort();
                }
                ajaxRequest = $.ajax(option);
            },
            //#endregion

            //#region Bind Data
            bindData: function (level, response) {
                jqGridAdditional.clearGrid("ReportGrid");
                var numOfRow = response.Data.length;
                if (numOfRow === 0) {
                    jqGridAdditional.setNoInfo();
                    return;
                }

                var reportGrid = $("#ReportGrid");
                $("#paging").hide();
                var numOfTotalRow = response.Total.length;
                var gridOption = WinLossByLeagueOption.getGridOption(level);

                gridOption.data = response.Data;
                gridOption.rowNum = level === 2 ? WinLossByLeagueOption.pageSize : WinLossByLeagueOption.nonePagingSize;
                gridOption.rowTotal = numOfRow;

                gridOption.gridComplete = function () {
                    jqGridAdditional.gridComplete();
                    _me.bindTotal(level, response.Total);
                    _me.resizeGrid();
                    Site.unBlockUI();
                };

                // Show the grid
                reportGrid.jqGrid(gridOption);

                //set rowspan for footer row
                $("table.ui-jqgrid-ftable").tableRowSpan([WinLossByLeagueOption.currencyIndex]);

                if (level === 0) {
                    WinLossByLeagueOption.addLeagueGroupRow($("#League").val(), $("#League option:selected").text());
                }
            },

            bindTotal: function (level, totalData) {
                var columnModel = WinLossByLeagueOption.getColModel(level);
                var colspan = level === 0 ? 2 : 3;
                jqGridAdditional.assignTotal(totalData, columnModel);
                jqGridAdditional.setGroupFooters(colspan, 0, 0);
            },
            //#endregion

            ajaxPaging: function () {
                _pagingOption = PagingHeper.getSetting();
                _me.drillDown(_leagueid, _leaguename);
            }
        };

        return _me;
    });