/// <reference path="/@JSense.js" />
/*global Site, QueryString,jqGridAdditional, BetListOption*/
/*exported showCombMP, showCombMPDetail, showMP, ViewBetSlip, ViewResult*/

var BetList = (function ($) {
    "use strict";
    var _sortColumnName, _sortOrder;
    var _params, _url;

    return {
        submiting: true,
        initialize: function () {
            $(window).on('resize', function () {
                $("#BetListGrid").setGridWidth($(window).width());
            }).trigger('resize');
        },

        gridComplete: function (grid, gridOption, betListGrid, totalRowCount) {
            // add sort icon
            $(grid).closest("div.ui-jqgrid-view")
                    .find("div.ui-jqgrid-hdiv table.ui-jqgrid-htable tr.ui-jqgrid-labels > th.ui-th-column > div.ui-jqgrid-sortable")
                    .each(function (index, element) {
                        var $this = $(element);
                        $this.find("span.s-ico").remove();

                        if (element.id.indexOf(_sortColumnName) > -1) {
                            var sortClass = "sort-" + _sortOrder;
                            var sortSpan = $("<span style='display: inline; margin: 2px 0px 0px 2px'></span");
                            sortSpan.addClass(sortClass);
                            $this.append(sortSpan);
                            $this.addClass("sorted-heading");
                        }
                    });

            betListGrid.setGridWidth($("#BodyContent").width());
            // Resize
            jqGridAdditional.gridComplete(grid, gridOption, totalRowCount);
            // Remove unnecessary item
            $(".ui-jqgrid-ftable td[aria-describedby=BetListGrid_Stake]").html(Site.getSingleResource("Total")).removeAttr("title");
            $("table.ui-jqgrid-htable .ui-jqgrid-resize").css("height", "14px");
            $(".content-filter").attr("style", "width: " + $("#gbox_BetListGrid").css("width"));

            // Show no information if there is no record
            var rowCount = Number($("#BetListGrid").getGridParam("reccount"));
            if (rowCount === 0) {
                jqGridAdditional.setNoInfo(BetListOption.columnModel);
                $("#pager2").hide();
            }
            else {
                // Remove noinfo message if exist
                var contentTable = $("table.ui-jqgrid-htable");
                if (contentTable.find(".no-info-row").length > 0) {
                    $(".ui-jqgrid-htable tbody .no-info-row").remove();
                }
                $("#pager2").show();
            }

            Site.unBlockUI();
        },

        getGridOption: function () {
            var gridOption = $.extend(true, {}, BetListOption.gridOption);

            gridOption.onSortCol = function (index) {
                if (window.disableBetListSorting) {
                    return 'stop';
                }
                Site.blockUI();
                if (_sortColumnName === index) {
                    _sortOrder = _sortOrder === "asc" ? "desc" : "asc";

                } else {
                    _sortOrder = "asc";
                    _sortColumnName = index;
                }

                BetList.loadData(_params, _url);

                Site.unBlockUI();
                return 'stop';
            };

            // Specify the hidden column
            var hiddenColumnList = $("#HiddenColumnList").val();
            var hiddenColumnTokens = hiddenColumnList.split(",");
            var showSummary = hiddenColumnList.indexOf("licenseewl") < 0;

            $.each(hiddenColumnTokens, function (index, value) {
                jqGridAdditional.setColumnVisible(BetListOption.columnModel, value, true);
            });
            gridOption.colNames = BetListOption.columnNames;
            gridOption.colModel = BetListOption.columnModel;
            gridOption.footerrow = showSummary;
            var hasCustComm = $('#HasCustComm').val();
            if (hasCustComm === '1') {
                gridOption.colNames = $.map(gridOption.colNames, function (val) {
                    if (val === 'WinLossComm') {
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
            Site.blockUI();

            var rows = BetListOption.gridOption.rowList[0];
            BetList.submiting = true;

            var gridOption = BetList.getGridOption();
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

                // Show the grid
                var gridOption = BetList.getGridOption();
                gridOption.data = rawData;
                gridOption.userData = { WinLoss: sumWinLoss, LicenseeWl: sumLicenseeWl };

                if (response.IsShowTotal === undefined || response.IsShowTotal) {
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

        getSportBookResultUrl: function (matchid, bettype, raceNo, sporttype, league, isOutright) {
            var url = Site.resolveClientUrl('Report/ResultPopup');
            url = QueryString.setParam(url, "matchid", matchid);
            url = QueryString.setParam(url, "bettype", bettype);
            url = QueryString.setParam(url, "raceno", raceNo);
            url = QueryString.setParam(url, "sporttype", sporttype);
            url = QueryString.setParam(url, "isoutright", isOutright);

            return url;
        },

        setLeagueTitle: function (leagueName) {
            var leagueNameText = leagueName === '' || leagueName === undefined ? Site.getSingleResource('BetList') : leagueName;
            $(".league-name-title").html(leagueNameText + " - ");
        },

        showResult: function (matchid, raceNo, bettype, sporttype, refno, username, winlostdate, refnoMixParlay, league, isOutright) {
            bettype = Number(bettype);
            var url = bettype === 9 || bettype === 29
                    ? BetList.getMixParlayResultUrl(refnoMixParlay, winlostdate, bettype)
                    : BetList.getSportBookResultUrl(matchid, bettype, raceNo, sporttype, league, isOutright);

            var height = bettype === 9 || bettype === 29 ? 450 : 150;
            var width = 750;
            var title = Site.getSingleResource("Result");
            var dialogForm = '<div title="' + title + '"><iframe frameborder="0" width="' +
                         width + '" height="' + height + '" src="' + url + '" ></iframe></div>';

            $(dialogForm).dialog({
                modal: true,
                width: width + 25,
                height: height + 50
            });
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
                    // error: function (xhRequest, errorText, thrownError) { },
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

                        BetList.resetContentHeight();
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
                BetList.resetContentHeight();
            }
        },

        resetContentHeight: function () {
            var bodyHeight = 650;
            var actualHeight = $('.ui-jqgrid-bdiv .ui-jqgrid-btable').height();
            var bodyDiv = $('.ui-jqgrid-bdiv');
            bodyDiv.css({ 'height': actualHeight });
            bodyDiv.css({ 'max-height': bodyHeight });
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
        ValidProducts: function () {
            var temp = true;
            $("#ProductList input[type=checkbox]").checkboxgroup();
            if ($("input.childCheckbox:checked").length === 0) {
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