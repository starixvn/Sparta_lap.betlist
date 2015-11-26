/// <reference path="/@JSense.js" />
/*global Site, QueryString,jqGridAdditional, BetListOption, moment*/
/*exported showCombMP, showCombMPDetail, showMP, ViewBetSlip, ViewResult, showMicroGamingRnGBetsDetails, showAGCasinoDetails*/

var BetList = (function ($) {
    "use strict";
    var BETTYPE_MIXPARLAY = 9, BETTYPE_SYSTEMPARLAY = 29;
    var SPORT_MICROGAMMING_RNG = 205;

    var _sortColumnName, _sortOrder;
    var _params, _url;
    var _isShowSumStake;

    return {
        resizeGrid: function () {
            if ($(".no-info-row").length > 0) {
                return;
            }
            var $container = $(".ui-jqgrid-bdiv");
            if ($container.length === 0) {
                return;
            }
            var gridTop = $container.offset().top;
            var gridHeight = window.innerHeight - gridTop - $(".ui-jqgrid-sdiv:visible").height()
                         - $(".ui-jqgrid-pager:visible").height() - 30;
            gridHeight = Math.max(150, gridHeight);

            var $child = $container.find(">div");
            $container.css("max-height", gridHeight);
            if ($child.height() < 150 || $child.height() > Math.min($container.height(), gridHeight)) {
                $container.css("height", Math.min($child.height() + 1, gridHeight));
            }
        },
        getSortParams: function () {
            if (!_sortColumnName) {
                return null;
            }

            return {
                SortBy: _sortColumnName,
                SortOrder: _sortOrder
            };
        },
        submiting: true,
        initialize: function () {
            $(window).on('resize', function () {
                $("#BetListGrid").setGridWidth($(window).width());
            }).trigger('resize');

            _isShowSumStake = $("#IsShowSumStake").val() === "1";

            _sortColumnName = _sortColumnName || $("#SortBy").val();
            _sortOrder = _sortOrder || $("#SortOrder").val();
        },

        gridComplete: function (grid, gridOption, betListGrid, totalRowCount) {
            // add sort icon
            var sortCol = _sortColumnName === "Winlost" ? "WinLoss" : _sortColumnName;
            if (sortCol) {
                $(grid).closest(".ui-jqgrid-view")
                    .find(".ui-jqgrid-sortable")
                    .each(function (index, element) {
                        var $this = $(element);
                        $this.find(".s-ico").remove();

                        if (element.id.indexOf(sortCol) > -1) {
                            var sortClass = "sort-" + _sortOrder;
                            var sortSpan = $("<span style='display: inline; margin: 2px 0px 0px 2px'></span");
                            sortSpan.addClass(sortClass);
                            $this.append(sortSpan);
                            $this.addClass("sorted-heading");
                        }
                    });
            }

            betListGrid.setGridWidth($("#BodyContent").width());
            // Resize
            jqGridAdditional.gridComplete(grid, gridOption, totalRowCount);
            // Remove unnecessary item
            if (_isShowSumStake) {
                $(".ui-jqgrid-ftable td[aria-describedby=BetListGrid_Odds]").html(Site.getSingleResource("GrandTotal")).removeAttr("title");
            } else {
                $(".ui-jqgrid-ftable td[aria-describedby=BetListGrid_Stake]").html(Site.getSingleResource("GrandTotal")).removeAttr("title");
            }

            $(".ui-jqgrid-htable .ui-jqgrid-resize").css("height", "14px");
            $(".content-filter").attr("style", "width: " + $("#gbox_BetListGrid").css("width"));

            // Show no information if there is no record
            var rowCount = Number($("#BetListGrid").getGridParam("reccount"));
            if (rowCount === 0) {
                jqGridAdditional.setNoInfo(BetListOption.columnModel);
                $("#pager2").hide();
            }
            else {
                // Remove noinfo message if exist
                var contentTable = $(".ui-jqgrid-htable");
                if (contentTable.find(".no-info-row").length > 0) {
                    $(".ui-jqgrid-htable .no-info-row").remove();
                }
                $("#pager2").show();
            }

            // fix cursor on non-sortable columns
            var cm = $(grid)[0].p.colModel;
            $.each($(grid)[0].grid.headers, function (index, value) {
                var cmi = cm[index];
                if (cmi.sortable) {
                    $('.ui-jqgrid-sortable', value.el).css({ cursor: "pointer" });
                }
            });

            setTimeout(function () {
                BetList.resizeGrid();
            }, 100);

            Site.unBlockUI();
        },

        getGridOption: function () {
            var gridOption = $.extend(true, {}, BetListOption.gridOption);

            gridOption.onSortCol = function (index) {
                if (window.disableBetListSorting) {
                    return 'stop';
                }
                Site.blockUI();
                index = index === "WinLoss" ? "Winlost" : index;
                if (_sortColumnName === index) {
                    _sortOrder = _sortOrder === "asc" ? "desc" : "asc";
                } else {
                    _sortOrder = "desc";
                    _sortColumnName = index;
                }

                if (window.CorrectScoreBetList || window.CancelledBetsBetList) {
                    $("#SortBy").val(_sortColumnName);
                    $("#SortOrder").val(_sortOrder);
                }

                if (window.serverPaging || $("#IsServerPaging").val() === "1") {
                    BetList.loadDataPaging(_url);
                }
                else {
                    BetList.loadData(_params, _url);
                }

                return 'stop';
            };

            // Specify the hidden column
            var hiddenColumnList = $("#HiddenColumnList").val();
            var hiddenColumnTokens = hiddenColumnList.split(",");
            var showSummary = $("#ShowSummary").val() === "0" ? false : hiddenColumnList.indexOf("licenseewl") < 0;

            $.each(hiddenColumnTokens, function (index, value) {
                jqGridAdditional.setColumnVisible(BetListOption.columnModel, value, true);
            });
            if ($("#HasBetEffectiveColumn").val() !== "1") {
                jqGridAdditional.setColumnVisible(BetListOption.columnModel, "BetEffective", true);
            }
            gridOption.colNames = BetListOption.columnNames;
            gridOption.colModel = BetListOption.columnModel;
            gridOption.footerrow = showSummary;
            var hasCustComm = $('#HasCustComm').val();
            if (hasCustComm === '1') {
                gridOption.colNames = $.map(gridOption.colNames, function (val) {
                    if (val === 'WinLoss') {
                        return '@[WinLoss]/@[Comm]';
                    }
                    return val;
                });
            }
            gridOption.colNames = Site.getResources(gridOption.colNames);
            return gridOption;
        },

        loadData: function (params, url) {
            if (typeof params === "string") {
                params = JSON.parse(params);
            }

            _params = params;
            _url = url;

            if (params && _sortColumnName) {
                params.SortBy = _sortColumnName;
                params.SortOrder = _sortOrder;
            }

            var paramString = typeof (params) === "object" ? JSON.stringify(params) : params;

            $.ajax({
                type: "POST",
                data: paramString,
                contentType: 'application/json',
                dataType: "json",
                cache: false,
                url: url,
                success: function (response) {
                    if (response != null) {
                        BetList.bindData(response);
                        BetList.setLeagueTitle(response.LeagueName);
                    }
                },
                beforeSend: function () {
                    Site.blockUI();
                },
                complete: function (/*jqXhr, textStatus*/) {
                    Site.unBlockUI();
                }
            });
        },

        loadDataPaging: function (url) {
            _url = url;
            Site.blockUI();
            if (url && _sortColumnName) {
                url = QueryString.setParam(url, "SortBy", _sortColumnName);
                url = QueryString.setParam(url, "SortOrder", _sortOrder);
            }

            BetList.submiting = true;

            var gridOption = BetList.getGridOption();
            var rows = Number($("#BetListGrid").getGridParam("rowNum")) || gridOption.rowNum;
            gridOption.url = url;
            gridOption.rowNum = rows;
            gridOption.height = 0;
            gridOption.loadonce = false;
            gridOption.datatype = "json";
            gridOption.gridComplete = function () {
                var totalRowCount = gridOption.footerrow ? 2 : 1;
                BetList.gridComplete(this, gridOption, $("#BetListGrid"), totalRowCount);
                if (BetList.submiting) {
                    $(".ui-pg-selbox").val(rows);
                }
                BetList.submiting = false;
            };

            // Show the grid
            $("#BetListGrid").jqGrid('GridUnload');
            $("#BetListGrid").jqGrid(gridOption);
        },

        bindData: function (response) {
            if (response != null) {
                var rawData = response.Main;
                var summary = response.Total;
                var sumWinLoss = jqGridAdditional.formatDecimal(summary.SumWinLoss);
                var sumLicenseeWl = jqGridAdditional.formatLicenseeWl(summary.SumLicenseeWl);
                var sumBetEffective = jqGridAdditional.formatDecimal(summary.SumBetEffective);
                var gridOption = BetList.getGridOption();

                var rowNum = Number($("#BetListGrid").getGridParam("rowNum")) || gridOption.rowNum;
                if (rowNum < gridOption.rowList[0]) {
                    rowNum = gridOption.rowList[0];
                }
                // Show the grid
                gridOption.rowNum = rowNum;
                gridOption.data = rawData;
                gridOption.userData = { WinLoss: sumWinLoss, LicenseeWL: sumLicenseeWl, BetEffective: sumBetEffective };

                if (_isShowSumStake) {
                    gridOption.userData.Stake = jqGridAdditional.formatDecimal(summary.SumStake);
                    gridOption.userDataOnFooter = true;
                    gridOption.footerrow = true;
                }
                else if (response.IsShowTotal === undefined || response.IsShowTotal) {
                    gridOption.userDataOnFooter = true;
                }
                else {
                    gridOption.userDataOnFooter = false;
                    gridOption.footerrow = false;
                }
                gridOption.gridComplete = function () {
                    var totalRowCount = gridOption.footerrow ? 2 : 1;
                    BetList.gridComplete(this, gridOption, $("#BetListGrid"), totalRowCount);
                };

                $("#BetListGrid").jqGrid('GridUnload');
                $("#BetListGrid").jqGrid(gridOption);
            }
        },

        getMixParlayResultUrl: function (refno, winlostdate, bettype) {
            var url = Site.resolveClientUrl('Report/ResultPopup');
            url = QueryString.setParam(url, "refno", refno);
            url = QueryString.setParam(url, "winlossdate", winlostdate);
            url = QueryString.setParam(url, "bettype", bettype);
            url = QueryString.setParam(url, "matchid", 1);

            return url;
        },

        getSportBookResultUrl: function (matchid, bettype, raceNo, sporttype, league, isOutright, betid) {
            var url = Site.resolveClientUrl('Report/ResultPopup');
            url = QueryString.setParam(url, "matchid", matchid);
            url = QueryString.setParam(url, "bettype", bettype);
            url = QueryString.setParam(url, "raceno", raceNo);
            url = QueryString.setParam(url, "sporttype", sporttype);
            url = QueryString.setParam(url, "isoutright", isOutright);
            url = QueryString.setParam(url, "betid", betid);

            return url;
        },

        getMicrogamingResultUrl: function (gameid, custid) {
            var url = Site.resolveClientUrl('Report/MicrogamingResultPopup');
            url = QueryString.setParam(url, "gameid", gameid);
            url = QueryString.setParam(url, "custid", custid);

            return url;
        },

        setLeagueTitle: function (leagueName) {
            var leagueNameText = leagueName === '' || leagueName === undefined ? Site.getSingleResource('BetList') : leagueName;
            $(".league-name-title").html(leagueNameText + " - ");
        },
        showMicrogamingResultPopup: function (title, gameid, custid) {
            var url = BetList.getMicrogamingResultUrl(gameid, custid);

            var dialogForm = '<div title="' + title + '"><iframe frameborder="0" width="720" height="350" src="' + url + '" ></iframe></div></div>';
            $(dialogForm).dialog({
                modal: true,
                width: 750,
                height: 400
            }) ;
        },
        showResult: function (matchid, raceNo, bettype, sporttype, refno,
            username, winlostdate, refnoMixParlay, league, isOutright, betid, betcheck, custid) {
            var title = Site.getSingleResource("Result");
            if (sporttype === SPORT_MICROGAMMING_RNG) {
                BetList.showMicrogamingResultPopup(title, betcheck, custid);
            } else {
                bettype = Number(bettype);
                var isParlay = bettype === BETTYPE_MIXPARLAY || bettype === BETTYPE_SYSTEMPARLAY;
                var url = isParlay
                    ? BetList.getMixParlayResultUrl(refnoMixParlay, winlostdate, bettype)
                    : BetList.getSportBookResultUrl(matchid, bettype, raceNo, sporttype, league, isOutright, betid, betcheck, custid);

                var height = isParlay ? 450 : 150;
                var width = 750;
                var dialogForm = '<div title="' + title + '"><iframe frameborder="0" width="' +
                         width + '" height="' + height + '" src="' + url + '" ></iframe></div>';
                $(dialogForm).dialog({
                    modal: true,
                    width: width + 25,
                    height: height + 70
                });
            }
        },

        showMixParlayDetail: function (transId, divEvent) {
            var url = Site.resolveClientUrl('api/betlist/MixParlayDetail?transId=' + transId);
            if (divEvent.css("display") === 'none') {
                $.ajax({
                    type: "GET",
                    contentType: 'application/json',
                    dataType: "json",
                    cache: false,
                    url: url,
                    success: function (response) {
                        var choice = "";
                        var len = response.length;
                        $.each(response, function (index, item) {
                            if (index === len - 1) {
                                choice += item.Choice;
                            }
                            else {
                                choice += item.Choice + "<div class='line'></div>";
                            }
                        });
                        divEvent.html(choice);
                        divEvent.css("display", "");
                        BetList.resizeGrid();
                    },
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function (/*jqXhr, textStatus*/) {
                        Site.unBlockUI();
                    }
                });
            }
            else {
                divEvent.css("display", "none");
                divEvent.html("");
                $(".ui-jqgrid-bdiv").css("height", 0);
                $(".ui-jqgrid-bdiv>div").css("height", "auto");
                BetList.resizeGrid();
            }
        },

        showSystemParlayDetail: function (transId, refNo, betId, custId, betTeam, baseCurrency) {
            if (Number(transId) === 0) { return; }

            var url = Site.resolveClientUrl('BetList/SystemParlayDetail');
            url = QueryString.setParam(url, "transId", transId);
            url = QueryString.setParam(url, "refno", refNo);
            url = QueryString.setParam(url, "betid", betId);
            url = QueryString.setParam(url, "custid", custId);
            url = QueryString.setParam(url, "betteam", betTeam);
            url = QueryString.setParam(url, "basecurrency", baseCurrency);

            var height = 450;
            var width = 750;

            var dialogForm = '<div title="' + betTeam + '"><iframe frameborder="0" width="' +
                         width + '" height="' + height + '" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: width + 25,
                height: height + 50
            });
        },

        showMicroGamingRnGBetsDetails: function (transId, winlostDate, custId, transType) {
            var url = Site.resolveClientUrl('BetList/MicroGamingRngDetail');
            url = QueryString.setParam(url, "transid", transId);
            url = QueryString.setParam(url, "custid", custId);
            url = QueryString.setParam(url, "transType", transType);

            var popH = Math.min($(window).height() - 100, 550), popW = 820;
            var title = Site.getSingleResource("Microgaming") + " - " +
                Site.getSingleResource("DetailedBetlist") + " - " + moment(new Date(winlostDate)).format("MM/DD/YYYY");
            var dialogForm = '<div title="' + title + '"><iframe scrolling="no" frameborder="0" width="' +
                             popW + '" height="100%" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: popW + 50,
                height: popH + 50
            });
        },

        showAGCasinoDetails: function (transId, winlostDate, custId, transType) {
            var url = Site.resolveClientUrl('BetList/AGCasinoDetail');
            url = QueryString.setParam(url, "transid", transId);
            url = QueryString.setParam(url, "custid", custId);
            url = QueryString.setParam(url, "transType", transType);

            var popH = Math.min($(window).height() - 100, 550), popW = 820;
            var title = Site.getSingleResource("AGCasino") + " - " +
                Site.getSingleResource("DetailedBetlist") + " - " + moment(new Date(winlostDate)).format("MM/DD/YYYY");
            var dialogForm = '<div title="' + title + '"><iframe scrolling="no" frameborder="0" width="' +
                             popW + '" height="100%" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: popW + 50,
                height: popH + 50
            });
        },

        ValidProducts: function () {
            var temp = true;
            if ($("#ProductList input:checked").length === 0) {
                temp = false;
                var title = Site.getSingleResource("LrfNotification");
                $('<div title="' + title + '">' + Site.getSingleResource("AlertChooseProductMsg") + '</div>').dialog({
                    open: function () {
                        $('#SubmitButton').prop('disabled', true);
                    },
                    close: function () {
                        $('#SubmitButton').removeAttr('disabled');
                    }
                });
            }

            return temp;
        },

        ViewCasinoHandHistory: function (refNo) {
            var url = Site.resolveClientUrl('HandHistory/RNGCasino');
            var urlParam = $("#casino-hand-history-" + refNo).attr('url-param');

            url += urlParam;
            window.location = url;
        }
    };
})(jQuery);

var ViewBetSlip = function () {
};

var ViewResult = BetList.showResult;

var showMP = function (transId) {
    "use strict";
    var divEvent = $('#divEvent_' + transId);
    divEvent.addClass("mx-parlay"); //to make line-through for void-parlay-ticket
    BetList.showMixParlayDetail(transId, divEvent);
};

var showCombMP = function (transId, index) {
    "use strict";
    var divEvent = $('#divEvent_' + transId + "" + index);
    divEvent.addClass("mx-parlay"); //to make line-through for void-parlay-ticket
    BetList.showMixParlayDetail(transId, divEvent);
};

var showCombMPDetail = BetList.showSystemParlayDetail;
var showMicroGamingRnGBetsDetails = BetList.showMicroGamingRnGBetsDetails;
var showAGCasinoDetails = BetList.showAGCasinoDetails;