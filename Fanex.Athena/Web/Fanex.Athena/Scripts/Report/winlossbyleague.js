/// <reference path="/@JSense.js" />

var WinLossByLeague = (function ($) {
    var ajaxRequest = null;
    var ajaxLeagues = null;
    var ajaxMatches = null;

    var _level = 0;
    var _leagueid = -1;
    var _leaguename = null;
    var _fromDate = null;
    var _toDate = null;
    var _minOdds = -1;
    var _postdata = null;
    var _currencyName = null;

    var _pagingOption =
        {
            pageindex: 1,
            pagesize: 50,
            pagelist: [50, 100, 1000],
            totalrow: 1,
            elementcontainer: "#paging",
            method: "AJAX",
            ajaxfunction: "WinLossByLeague.ajaxPaging()",
            alwayshow: true
        };

    function initMonthPicker() {
        var year = $("#Year").val();
        var month = $("#Month").val();
        var fromDay = $("#FromDay").val();
        var toDay = $("#ToDay").val();
        var fromDate = new Date(year, month - 1, fromDay);

        $('#DateControl').monPickr({
            minDate: $('#MinDate').val(),
            showDateRange: true,
            fromDayId: 'MonthPickerFromDay',
            toDayId: 'MonthPickerToDay',
            month: fromDate,
            fromDay: fromDay,
            toDay: toDay,
            afterChangeMonth: function () {
                $('#Year').val(new Date($("#DateControl").monPickr("getFromDate")).getFullYear());
                $('#Month').val(new Date($("#DateControl").monPickr("getFromDate")).getMonth() + 1);
                loadLeagues();
            },
            afterChangeDay: function () {
                $('#FromDay').val(new Date($("#DateControl").monPickr("getFromDate")).getDate());
                $('#ToDay').val(new Date($("#DateControl").monPickr("getToDate")).getDate());
                loadLeagues();
            },
            log: function (msg) {
                //console.warn(msg);
            }
        });
    }

    function loadLeagues() {
        if (ajaxLeagues != null) {
            ajaxLeagues.abort();
        }
        Site.blockUI();
        $("#SubmitButton").prop("disabled", true);
        $("#League").html("");
        ajaxLeagues = $.ajax({
            type: "POST",
            url: Site.resolveClientUrl("report/GetLeaguesBySport"),
            data: {
                currencyId: $("#CurrencyId").val(),
                sportid: $("#Sport").val(),
                fromDate: $("#DateControl").monPickr("getFromDate").toString("dd MMM yyyy"),
                toDate: $("#DateControl").monPickr("getToDate").toString("dd MMM yyyy")
            },
            success: function (data) {
                var str = "";
                $.each(data, function () {
                    str += "<option value=\"" + this.Key + "\">" + this.Value + "</option>";
                });
                $("#League").html(str);
                loadMatches(false);

            }
        });
    }

    function loadMatches(isBlockUI) {
        if (ajaxMatches != null) {
            ajaxMatches.abort();
        }

        if (isBlockUI)
            Site.blockUI();
        $("#SubmitButton").prop("disabled", true);
        $("#Match").html("");
        var leagueid = $("#League").val();
        if (!leagueid) {
            Site.unBlockUI();
            return;
        }
        ajaxMatches = $.ajax({
            type: "POST",
            datatype: "json",
            url: Site.resolveClientUrl("report/GetMatchesByLeague"),
            data: {
                leagueid: leagueid,
                fromDate: $("#DateControl").monPickr("getFromDate").toString("dd MMM yyyy"),
                toDate: $("#DateControl").monPickr("getToDate").toString("dd MMM yyyy")
            },
            success: function (data) {
                var str = "";
                $.each(data, function () {
                    str += "<option value=\"" + this.Key + "\">" + this.Value + "</option>";
                });
                $("#Match").html(str);
                if (data.length > 0)
                    $("#SubmitButton").prop("disabled", false);
                Site.unBlockUI();
            }
        })
    }

    var me = {
        ajaxPaging: function () {
            _pagingOption = pagingHeper.getSetting();
            me.drillDown(_level, _leagueid, _leaguename);
        },
        //#region Init
        initialize: function () {
            WinLossByLeague.initEvent();
            WinLossByLeague.initSetting();

            initMonthPicker();

            $("#searchForm").submit(function () {
                me.submit();
                return false;
            });
            $("#League").change(function () {
                loadMatches(true);
            });
            $("#Sport, #CurrencyId").change(function () {
                loadLeagues();
            });

            $("#TickerFilter input[type=checkbox]").checkboxgroup();

            $("body").on("click", "#leagueHeader", function () {
                var leaguename = $(this).html();
                me.drillDown(1, $(this).attr("data-leagueid"), leaguename.substr(2));
            });

            $("body").on("click", ".drilldown", function () {
                var custid = $(this).data("custid");
                var externalid = $(this).data("externalid");
                var systemid = $(this).data("systemid");
                me.drillDownBetList(custid, externalid, systemid);
            });

            $("#BackLink").click(me.goBack);

            $("#ExportToExcel").click(function (e) {
                _postdata.LeagueName = _leaguename;
                _postdata.Level = _level;
                window.location.href = (Site.resolveClientUrl("api/Report/WinLossByLeagueExcel?") + $.param(_postdata));
                e.preventDefault();
            });

            $("#Odds").keypress(function (event) {
                /*--------------------------------------------------------------
                Backspace, tab, enter, end, home, left, right
                Don't support the del key in Opera because del == . == 46.
                --------------------------------------------------------------*/
                var controlKeys = [8, 9, 13, 35, 36, 37, 39, 46];

                /*--------------------------------------------------------------
                IE doesn't support indexOf
                --------------------------------------------------------------*/
                var isControlKey = controlKeys.join(",").match(new RegExp(event.which));

                /*--------------------------------------------------------------
                Some browsers just don't raise events for control keys. Easy.
                e.g. Safari backspace.
                --------------------------------------------------------------*/
                if (!event.which ||
                // Control keys in most browsers. e.g. Firefox tab is 0
                 (48 <= event.which && event.which <= 57) ||
                // Always 0 through 9
                //(48 == event.which && $(this).attr("value")) ||   // No 0 first digit
                    isControlKey) {
                    // Opera assigns values for control keys.
                    return;
                }
                else {
                    event.preventDefault();
                }
            });
        },

        initSetting: function () {
            WinLossByLeagueOption.licenseeId = parseInt($("#drillDownLicenseeId").val());
        },

        initEvent: function () {
            /// <summary>
            /// Inititalize events
            /// </summary>
            $("body").on("click", ".bet-list-link", function () {
                var dataDrillDown = $(this).attr("data-drill-down");
                WinLossByLeague.openBetlistPopUp(dataDrillDown);
            });

            $('#SubmitButton').click(function () {
                WinLossByLeague.submit();
            });

            $("#level-link-container").on("click", ".level-link", function () {
                var level = $(this).attr("data-level");
                return WinLossByLeague.goBack(level);
            });
        },

        //#endregion

        //#region Utilities

        goBack: function () {
            if (_level == 1) {
                me.submit();
            } else {
                me.drillDown(1, _leagueid, _leaguename);
            }
        },
        setReportTitle: function (level, minOdds, fromdate, todate) {
            var caption = null;
            if (level == 0)
                caption = Site.getSingleResource('WinLossByLeague');
            else
                caption = _leaguename + " > " + Site.getSingleResource('CustomerWinLoss');

            var titlePattern = caption + ', {0} → {1} - {2} - {3}';
            var viewinKey = Site.getSingleResource("Currency") + ": ";

            var currencyStr = viewinKey + " " + (_postdata.BaseCurrency == "" ? _currencyName : _postdata.BaseCurrency);
            var minOddStr = Site.getSingleResource("MinimumOdds") + " : " + minOdds;
            var title = $.format(titlePattern, fromdate.toString("M/dd/yyyy"), todate.toString("M/dd/yyyy"),
            currencyStr, minOddStr);
            _postdata.ReportTitle = title;
            $("div.report-title #caption").html(title);
        },

        lastDateOfMonth: function (year, month) {
            return new Date(year, month + 1, 0);
        },

        //#endregion

        //#region Get Data

        ajaxComplete: function (level, response) {
            if (response != null) {
                //reset layout
                if (level == 0) {
                    WinLossByLeague.setReportTitle(0, _postdata.Odds, _fromDate, _toDate);
                }
                // WinLossByLeague.renderBreadCrumb(level);
                WinLossByLeague.bindData(level, response);
            }
        },

        submit: function () {

            if ($("#TickerFilter input:checked").length == 0) {
                isValid = false;
                return $("#ticket-null").removeClass("hide");
            } else {
                $("#ticket-null").addClass("hide");
            }

            var odds = parseFloat($('#Odds').val());
            if (isNaN(odds) || odds == null || odds < 0) {
                odds = 0;
            }

            $("#Odds").val(odds);

            Site.blockUI();
            _level = 0;

            var mainForm = $("#searchForm");
            var postdata = mainForm.serializeObject();
            _fromDate = new Date(postdata.Year, postdata.Month - 1, postdata.FromDay);
            _toDate = new Date(postdata.Year, postdata.Month - 1, postdata.ToDay);
            _currencyName = $("#CurrencyName").val().toUpperCase();
            postdata.BaseCurrency = postdata.BaseCurrency.toUpperCase();
            _baseCurrency = postdata.BaseCurrency;
            _minOdds = postdata.Odds;
            postdata.FromDate = _fromDate.toDateString();
            postdata.ToDate = _toDate.toDateString();
            _postdata = postdata;
            _leaguename = $("#League option:selected").text();
            var option =
            {
                type: "POST",
                data: postdata,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                cache: false,
                url: Site.resolveClientUrl(WinLossByLeagueOption.urlData),
                error: function (xhRequest, errorText, thrownError) { },
                success: function (response) {
                    $("#BackLink").hide();
                    WinLossByLeague.ajaxComplete(0, response);
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function (jqXhr, textStatus) {
                    $(".report-title").show();
                    Site.unBlockUI();
                }
            };
            if (ajaxRequest != null) {
                ajaxRequest.abort();
            }
            ajaxRequest = $.ajax(option);

            return true;
        },

        //#endregion

        //#region drillDown
        drillDownBetList: function (custId, externalId, systemId) {

            var param =
            {
                IsConvert: _postdata.BaseCurrency != "",
                Odds: _postdata.Odds,
                FromDate: _fromDate.toDateString(),
                ToDate: _toDate.toDateString(),
                BaseCurrency: _postdata.BaseCurrency,
                CurrencyId: _postdata.CurrencyId,
                CurrencyName: _currencyName,
                CustId: custId,
                Level: 2,
                ExternalId: externalId,
                SystemId: systemId,
                LeagueId: _leagueid,
                ShowWin: _postdata.ShowWin,
                ShowLoss: _postdata.ShowLoss,
                ShowDraw: _postdata.ShowDraw
            };

            var url = Site.resolveClientUrl("BetList/WinlossByLeague?") + $.param(param);
            window.open(url, "abc", "modal=yes");
        },

        drillDown: function (level, leagueid, leaguename) {
            _level = 1;
            $("#BackLink").show();
            _leagueid = leagueid;
            _leaguename = leaguename;

            var param =
            {
                FromDate: _fromDate.toDateString(),
                ToDate: _toDate.toDateString(),
                BaseCurrency: _postdata.BaseCurrency,
                Level: level,
                Odds: _postdata.Odds,
                CurrencyId: _postdata.CurrencyId,
                League: leagueid,
                ShowWin: _postdata.ShowWin,
                ShowLoss: _postdata.ShowLoss,
                ShowDraw: _postdata.ShowDraw,
                pageindex: _pagingOption.pageindex,
                pagesize: _pagingOption.pagesize
            };

            var option =
            {
                type: "POST",
                data: param,
                dataType: "json",
                cache: false,
                url: Site.resolveClientUrl(WinLossByLeagueOption.urlData),
                error: function (xhRequest, errorText, thrownError) { },
                success: function (response) {
                    WinLossByLeague.ajaxComplete(level, response);
                    if (response != null && response.Data.length > 0) {
                        WinLossByLeague.setReportTitle(1, _minOdds, _fromDate, _toDate);
                        _pagingOption.totalrow = parseInt(response.TotalRows);
                        if (_pagingOption.totalrow > 0) {
                            $("#paging").show();
                              pagingHeper.render(_pagingOption);                            
                        }
                    }
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function (jqXhr, textStatus) {
                    Site.unBlockUI();
                }
            };
            if (ajaxRequest != null) {
                ajaxRequest.abort();
            }
            ajaxRequest = $.ajax(option);
        },

        //#endregion

        //#region Bind Data
        bindTotal: function (level, totalData) {
            var columnModel = WinLossByLeagueOption.getColModel(level);            
            var colspan = level == 0 ? 2 : 3;
            jqGridAdditional.assignTotal(totalData, columnModel);
            jqGridAdditional.setGroupFooters(colspan, 0, 0);
        },

        bindData: function (level, response) {

            jqGridAdditional.clearGrid("ReportGrid");
            var reportGrid = $("#ReportGrid");
            $("#paging").hide();
            var numOfRow = response.Data.length;
            var numOfTotalRow = response.Total.length;
            var onPaging = false;
            var gridOption = WinLossByLeagueOption.getGridOption(level);


            gridOption.footerrow = true;
            gridOption.userDataOnFooter = true;

            gridOption.data = response.Data;
            gridOption.userData = {};
            gridOption.rowNum = level == 2 ? WinLossByLeagueOption.pageSize : WinLossByLeagueOption.nonePagingSize;
            gridOption.rowList = WinLossByLeagueOption.rowList;
            gridOption.rowTotal = numOfRow;
            gridOption.pager = '';
            gridOption.onPaging = function (pgButton) {
                if (level == 2 && numOfRow > WinLossByLeagueOption.pageSize) {
                    if ($("#" + pgButton).hasClass("ui-state-disabled") == false) {
                        Site.blockUI();
                    }
                    onPaging = true;
                }
                else {
                    return 'stop';
                }
            };
            gridOption.gridComplete = function () {
                jqGridAdditional.gridComplete(this, gridOption, numOfTotalRow);

                if (response == null || numOfRow == 0) {
                    jqGridAdditional.setNoInfo(gridOption.colModel);
                }
                else {
                    if (onPaging && level == 2) {
                        $(".ui-jqgrid-ftable").html($("#footer").html());
                    }
                    else {
                        WinLossByLeague.bindTotal(level, response.Total);
                        $("#footer").html($(".ui-jqgrid-ftable").html());
                    }

                    onPaging = false;
                }
                Site.unBlockUI();
            };

            // Show the grid

            reportGrid.jqGrid(gridOption);

            //set rowspan for footer row
            $("table.ui-jqgrid-ftable").tableRowSpan([WinLossByLeagueOption.currencyIndex]);
            if (level == 2) {
                jqGridAdditional.showRecommendedBrowser(gridOption.pager);
            }
            if (level == 0)
                WinLossByLeagueOption.addLeagueGroupRow($("#League").val(), $("#League option:selected").text());
            ///reset layout
            jqGridAdditional.resetLayout(reportGrid);
        }

        //#endregion
    }

    return me;
})(jQuery);

$(window).load(function () {
    WinLossByLeague.initialize();
});

$(window).resize(function () {
    var reportGrid = $("#ReportGrid");
    jqGridAdditional.resetLayout(reportGrid);
});

$(window).unload(function () {
    $(window).unbind("resize");
});