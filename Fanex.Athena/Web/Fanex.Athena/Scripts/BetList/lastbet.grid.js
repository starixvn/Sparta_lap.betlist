/*global Site, JSLINQ*/
/*---------------------------------------------------------------------------------
* Report monitor bet list display lastest when user bet
---------------------------------------------------------------------------------*/
"use strict";

var LastBetGrid = (function () {
    var _me = {};

    // Param
    var SERVICE_URL = Site.resolveClientUrl('api/LastBetMonitoringBetList/Index');

    _me.betListResult = { NewRow: false, Rows: [], LastTransId: 0 };
    _me.sportId = 0;
    _me.leagueId = 0;
    _me.matchId = 0;
    _me.betType = "";
    _me.liveStatus = "3";
    _me.oddsType = "";
    _me.level = "";
    _me.timeZone = "8";
    _me.currency = "";
    _me.basedCurrency = $("#last-bet-monitor-base-currency").val();
    _me.stakeFrom = "0";
    _me.stakeTo = "1000000000";
    _me.pageSize = 10;
    _me.lastTransId = "0";
    _me.params = {};
    _me.currentParam = {};
    _me.intervalId = 0;
    _me.errorMessage = null;
    _me.isLoading = false;
    _me.isCriteriaChange = false;
    _me.intervalMiliseconds = 3000;
    _me.betFrom = 0;

    // Object
    var _$containerTable = null;
    var _$headerRow = null;

    // Config
    var MIN_LOADING_MILISECONDS = 1000;
    var MIN_INTERVAL = 3000;
    _me.maxStake = 999999999;
    _me.highlightClass = "monitor-hightlight";
    _me.highlightKeepClass = "monitor-keep";
    _me.columns =
        [{ Name: "Platform", IsShow: 1 },
         { Name: "ExternalId", IsShow: 1 },
         { Name: "SystemId", IsShow: 1 },
         { Name: "TransTime", IsShow: 1 },
         { Name: "Choice", IsShow: 1 },
         { Name: "Odds", IsShow: 1 },
         { Name: "Stake", IsShow: 1 },
         { Name: "Status", IsShow: 1 }];
    _me.initialize = function () {
        Site.addUserGuide({
            resourceName: "LastBetsMonitoring",
            title: Site.getSingleResource("LastBetsMonitoring")
        });

        _me.updateParams();
        _me.currentParam = Site.clone(_me.params);

        _$headerRow = $("#monitor-header");
        _$containerTable = $("#last-bets-monitoring");
        _$containerTable.on("click", "tr", function () {
            $(this).toggleClass("monitor-keep");
        });
    };

    _me.numberParam = function (name, value) {
        _me[name] = value === undefined || value == null || value === ""
                            ? 0
                            : parseInt(value);
        _me.updateParams();
    };

    _me.stringParam = function (name, value) {
        _me[name] = value === undefined || value == null || value === ""
                            ? ""
                            : $.trim(value);
        _me.updateParams();
    };

    _me.isValidParam = function () {
        return _me.stakeFrom <= _me.stakeTo
            && $("#currency").multiSelect("getCheckedItems").length > 0
            && $("#special-level").multiSelect("getCheckedItems").length > 0;
    };
    _me.tryUpdateStack = function () {
        var tempF = $("#stake-from").val();
        tempF = tempF.replace(/,/g, '');
        tempF = tempF === "" ? 0 : parseInt(tempF);

        _me.stakeFrom = tempF;

        var tempT = $("#stake-to").val();
        tempT = tempT.replace(/,/g, '');
        tempT = tempT === "" ? _me.maxStake : parseInt(tempT);
        _me.stakeTo = tempT;

        _me.params.StakeFrom = _me.stakeFrom;
        _me.params.StakeTo = _me.stakeTo;
    };
    _me.updateParams = function () {
        _me.params = {
            SportId: _me.sportId,
            LeagueId: _me.leagueId,
            MatchId: _me.matchId,
            BetType: _me.betType,
            LiveStatus: _me.liveStatus,
            OddsType: _me.oddsType,
            Level: _me.level,
            TimeZone: _me.timeZone,
            Currency: _me.currency,
            BasedCurrency: _me.basedCurrency,
            StakeFrom: _me.stakeFrom,
            StakeTo: _me.stakeTo,
            PageSize: _me.pageSize,
            TransId: _me.lastTransId,
            BetFrom: _me.betFrom
        };

        _me.currentParam = Site.clone(_me.params);
    };

    _me.Submit = function (freeStatus) {
        _me.stopMonitor();
        _me.tryUpdateStack();
        _me.currentParam = Site.clone(_me.params);
        _me.isCriteriaChange = true;

        if (parseInt(freeStatus) === 1) {
            _me.startMonitor();
        }
        else {
            _me.stopMonitor();
        }
    };

    /*------------------------------------------------------------------------------------------
    Get data from database
    ------------------------------------------------------------------------------------------*/
    _me.reloadData = function () {
        if (_me.isLoading) {
            _me.doMonitor();
            return;
        }

        if (!_me.isValidParam()) {
            return;
        }

        _me.isLoading = true;

        var loadingTickBegin = (new Date()).getTime();
        if (Number(_me.lastTransId) === 0 || _me.isCriteriaChange) {
            _me.clearContent();
        }

        $.ajax({
            url: SERVICE_URL,
            type: "POST",
            data: JSON.stringify(_me.currentParam),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            async: true,
            cache: false,
            timeout: 10000,
            beforeSend: function () {
                $("#div-loading").show();
                _me.isLoading = true;
                _me.isCriteriaChange = false;
            },
            complete: function () {
                $("#div-loading").hide();
                var numOfRow = $("tr", _$containerTable).length - 1 - $(".no-info-row").length;
                if (numOfRow > 0) {
                    $(".no-info-row").remove();
                }
            },
            success: function (result) {
                if (_me.isCriteriaChange) {
                    _me.isLoading = false;
                    _me.doMonitor();
                    return;
                }

                // Only bind data if current mode == mode at the begining of request
                _me.betListResult = result;
                _me.bindData();

                var loadingTime = (new Date()).getTime() - loadingTickBegin;
                _me.intervalMiliseconds = loadingTime > MIN_LOADING_MILISECONDS
                ? _me.intervalMiliseconds + 1000
                : MIN_INTERVAL;

                // Continue
                _me.doMonitor();
            },
            error: function (/*jqXHR, textStatus, errorThrown*/) {
                $("#div-loading").hide();

                _me.betListResult.Updated = false;
                _me.betListResult.Rows = "";
                _me.isLoading = false;
                _me.intervalMiliseconds += 1000;
                _me.doMonitor();
            }
        });
    };

    _me.bindData = function () {
        // If there is no new bet, process error message
        if (!_me.betListResult.NewRow) {
            _me.isLoading = false;

            var message = _me.betListResult.Message;
            if (message !== undefined && message.length > 0) {
                _me.bindNoInfoRow(message);
                return;
            }

            var rowCount = $("tr.content-row", _$containerTable).length;
            if (rowCount === 0) {
                _me.bindNoInfoRow(message);
            }

            return;
        }

        /*--------------------------------------------------------------------------------------------
        Insert new rows from database
        Remove old rows
        Update last transId
        --------------------------------------------------------------------------------------------*/
        //get hide columns
        var hideColumns = JSLINQ(_me.columns).Where(function (item) { return Number(item.IsShow) === 0; })
                                        .ToArray();
        if (hideColumns.length > 0) {
            var rows = _me.betListResult.Rows;
            var display = " style=\"display:none;\"";
            $.each(hideColumns, function (index, value) {
                var name = "data-name=\"" + value.Name + "\"";
                rows = rows.replaceAll(name, name + display);
            });
            _$headerRow.after(rows);
        } else {
            _$headerRow.after(_me.betListResult.Rows);
        }
        $("tr:gt(" + _me.pageSize + ")", _$containerTable).remove();

        if (_me.betListResult) {
            _me.lastTransId = _me.betListResult.LastTransId;
            _me.currentParam.TransId = _me.lastTransId;
        }
        setTimeout(function () {
            return _me.unHighlight(_me.lastTransId);
        },
         6000);

        _me.isLoading = false;
    };

    _me.bindNoInfoRow = function (text) {
        var noInfoMessage = text === undefined || text.length === 0
                        ? Site.getSingleResource("NoInfo")
                        : text;
        var colspan = JSLINQ(_me.columns).Count(function (item) { return Number(item.IsShow) === 1; });

        var row = "<tr class='content-row no-info-row'><td colspan='" + colspan + "'>" + noInfoMessage + "</td></tr>";
        $("tr:gt(0)", _$containerTable).remove();
        _$containerTable.append(row);
    };

    /*------------------------------------------------------------------------------------------
    Remove high light row after a duration
    ------------------------------------------------------------------------------------------*/
    _me.unHighlight = function (lastTransId) {
        $("tr.content-row", _$containerTable).each(function (i, e) {
            var transId = $(e).attr("data-transid");
            if (transId <= lastTransId) {
                $(e).removeClass(_me.highlightClass);
            }
        });
    };

    _me.clearContent = function () {
        _me.lastTransId = 0;
        _me.currentParam.TransId = _me.lastTransId;
        _me.params.TransId = _me.lastTransId;
        _me.bindNoInfoRow();
    };

    _me.startMonitor = function () {
        this.stopMonitor();

        _me.lastTransId = 0;
        _me.intervalMiliseconds = MIN_INTERVAL;
        _me.doMonitor();
    };

    _me.stopMonitor = function () {
        if (this.intervalId > 0) {
            clearTimeout(this.intervalId);
        }
    };

    _me.doMonitor = function () {
        if (_me.intervalId > 0) {
            clearTimeout(_me.intervalId);
        }

        _me.intervalId = setTimeout(function () { _me.reloadData(); }, _me.intervalMiliseconds);
    };

    return _me;
}());

/*******************/

$(window).load(function () {
    LastBetGrid.initialize();
    LastBetFilter.initialize();

    LastBetGrid.startMonitor();
});

$(window).unload(function () {
    LastBetGrid.stopMonitor();
});