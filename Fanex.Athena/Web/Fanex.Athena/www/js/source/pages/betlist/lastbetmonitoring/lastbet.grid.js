"use strict";
define(
    ['jquery', 'common/site', "lib/linq"],
    function ($, Site) {
    var _me = {};

    // Param
    var _betListResult = { NewRow: false, Rows: [], LastTransId: 0 };
    var _lastTransId = "0";

    var _params = {
        SportId: 0,
        LeagueId: 0,
        MatchId: 0,
        BetType: "",
        LiveStatus: "3",
        OddsType: "",
        Level: "",
        TimeZone: "8",
        Currency: "",
        BasedCurrency: "",
        PageSize: 10,
        StakeFrom: "0",
        StakeTo: "1000000000",
        TransId: _lastTransId,
        BetFrom: 0
    };
    var _intervalId;
    var _errorMessage;
    var _isLoading;
    var _isCriteriaChange;
    var _intervalMiliseconds;

    // Object
    var _$containerTable;
    var _$headerRow;

    // Config
    var MIN_LOADING_MILISECONDS = 1000;
    var MIN_INTERVAL = 3000;
    var HIGHLIGHT_CLASS = "monitor-hightlight";
    var MAX_STAKE = 999999999;
    
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
        _intervalId = 0;
        _errorMessage = null;
        _isLoading = false;
        _isCriteriaChange = false;
        _intervalMiliseconds = 3000;

        _$containerTable = null;
        _$headerRow = null;

        Site.addUserGuide({
            resourceName: "LastBetsMonitoring",
            title: Site.getSingleResource("LastBetsMonitoring")
        });

        _$headerRow = $("#monitor-header");
        _$containerTable = $("#last-bets-monitoring");
        _$containerTable.on("click", "tr", function () {
            $(this).toggleClass("monitor-keep");
        });

        _params.BasedCurrency = $("#last-bet-monitor-base-currency").val();
    };
    _me.getMaxStake = function () {
        return MAX_STAKE;
    };
    _me.numberParam = function (name, value) {
        _params[name] = value === undefined || value == null || value === ""
                            ? 0
                            : parseInt(value);
    };

    _me.stringParam = function (name, value) {
        _params[name] = value === undefined || value == null || value === ""
                            ? ""
                            : $.trim(value);
    };

    _me.isValidParam = function () {
        return _params.StakeFrom <= _params.StakeTo
            && $("#currency").multiSelect("getCheckedItems").length > 0
            && $("#special-level").multiSelect("getCheckedItems").length > 0;
    };
    _me.tryUpdateStake = function () {
        var tempF = $("#stake-from").val();
        tempF = tempF.replace(/,/g, '');
        tempF = tempF === "" ? 0 : parseInt(tempF);

        _params.StakeFrom = tempF;

        var tempT = $("#stake-to").val();
        tempT = tempT.replace(/,/g, '');
        tempT = tempT === "" ? MAX_STAKE : parseInt(tempT);

        _params.StakeTo = tempT;
    };

    _me.Submit = function (freezeStatus) {
        _me.stopMonitor();
        _me.tryUpdateStake();
        _isCriteriaChange = true;

        if (parseInt(freezeStatus) === 1) {
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
        if (_isLoading) {
            _me.doMonitor();
            return;
        }

        if (!_me.isValidParam()) {
            return;
        }

        _isLoading = true;

        var loadingTickBegin = (new Date()).getTime();
        if (Number(_lastTransId) === 0 || _isCriteriaChange) {
            _me.clearContent();
        }

        $.ajax({
            url: Site.resolveClientUrl('api/LastBetMonitoringBetList/Index'),
            type: "POST",
            data: JSON.stringify(_params),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            async: true,
            cache: false,
            timeout: 10000,
            beforeSend: function () {
                $("#div-loading").show();
                _isLoading = true;
                _isCriteriaChange = false;
            },
            complete: function () {
                $("#div-loading").hide();
                var numOfRow = $("tr", _$containerTable).length - 1 - $(".no-info-row").length;
                if (numOfRow > 0) {
                    $(".no-info-row").remove();
                }
            },
            success: function (result) {
                if (_isCriteriaChange) {
                    _isLoading = false;
                    _me.doMonitor();
                    return;
                }

                // Only bind data if current mode == mode at the begining of request
                _betListResult = result;
                _me.bindData();

                var loadingTime = (new Date()).getTime() - loadingTickBegin;
                _intervalMiliseconds = loadingTime > MIN_LOADING_MILISECONDS
                ? _intervalMiliseconds + 1000
                : MIN_INTERVAL;

                // Continue
                _me.doMonitor();
            },
            error: function (jqXHR) {
                if (jqXHR.status === 403) {
                    Site.reload();
                    return;
                }

                $("#div-loading").hide();

                _betListResult.Updated = false;
                _betListResult.Rows = "";
                _isLoading = false;
                _intervalMiliseconds += 1000;
                _me.doMonitor();
            }
        });
    };

    _me.bindData = function () {
        // If there is no new bet, process error message
        if (!_betListResult.NewRow) {
            _isLoading = false;

            var message = _betListResult.Message;
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
        var hideColumns = Enumerable.From(_me.columns).Where(function (item) { return Number(item.IsShow) === 0; })
                                        .ToArray();
        if (hideColumns.length > 0) {
            var rows = _betListResult.Rows;
            var display = " style=\"display:none;\"";
            $.each(hideColumns, function (index, value) {
                var name = "data-name=\"" + value.Name + "\"";
                rows = rows.replaceAll(name, name + display);
            });
            _$headerRow.after(rows);
        } else {
            _$headerRow.after(_betListResult.Rows);
        }
        $("tr:gt(" + _params.PageSize + ")", _$containerTable).remove();

        if (_betListResult) {
            _lastTransId = _betListResult.LastTransId;
            _params.TransId = _lastTransId;
        }
        setTimeout(function () {
            return _me.unHighlight(_lastTransId);
        },
         6000);

        _isLoading = false;
    };

    _me.bindNoInfoRow = function (text) {
        var noInfoMessage = text === undefined || text.length === 0
                        ? Site.getSingleResource("NoInfo")
                        : text;
        var colspan = Enumerable.From(_me.columns).Count(function (item) { return Number(item.IsShow) === 1; });

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
                $(e).removeClass(HIGHLIGHT_CLASS);
            }
        });
    };

    _me.clearContent = function () {
        _lastTransId = 0;
        _params.TransId = _lastTransId;
        _me.bindNoInfoRow();
    };

    _me.startMonitor = function () {
        _me.stopMonitor();

        _lastTransId = 0;
        _intervalMiliseconds = MIN_INTERVAL;
        _me.doMonitor();
    };

    _me.stopMonitor = function () {
        if (_intervalId > 0) {
            clearTimeout(_intervalId);
        }
    };

    _me.doMonitor = function () {
        if (_intervalId > 0) {
            clearTimeout(_intervalId);
        }

        _intervalId = setTimeout(_me.reloadData, _intervalMiliseconds);
    };

    return _me;
});
