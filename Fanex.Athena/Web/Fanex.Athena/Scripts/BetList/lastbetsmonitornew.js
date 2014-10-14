/*global Site, JSLINQ*/
/*---------------------------------------------------------------------------------
* Report monitor bet list display lastest when user bet
---------------------------------------------------------------------------------*/
"use strict";
var monitor = null;
var monitorUi = null;
var MonitorBetList = function () {
    var $this = this;

    // Param
    $this.serviceUrl = Site.resolveClientUrl('api/betlist/LastMonitorBetListNew');
    $this.betListResult = { NewRow: false, Rows: [], LastTransId: 0 };
    $this.sportId = 0;
    $this.leagueId = 0;
    $this.matchId = 0;
    $this.betType = "";
    $this.liveStatus = "3";
    $this.oddsType = "";
    $this.level = "";
    $this.timeZone = "8";
    $this.currency = "";
    $this.basedCurrency = $("#last-bet-monitor-base-currency").val();
    $this.stakeFrom = "0";
    $this.stakeTo = "1000000000";
    $this.pageSize = 10;
    $this.lastTransId = "0";
    $this.params = {};
    $this.currentParam = {};
    $this.intervalId = 0;
    $this.errorMessage = null;
    $this.isLoading = false;
    $this.isCriteriaChange = false;
    $this.intervalMiliseconds = 3000;
    $this.betFrom = 0;

    // Object
    $this.containerTable = null;
    $this.headerRow = null;

    // Config
    $this.minLoadingMiliseconds = 1000;
    $this.minInterval = 3000;
    $this.maxStake = 999999999;
    $this.containerTableId = "#last-bets-monitoring";
    $this.headerRowId = "#monitor-header";
    $this.highlightClass = "monitor-hightlight";
    $this.highlightKeepClass = "monitor-keep";
    $this.columns =
        [{ Name: Site.getSingleResource("Platform"), IsShow: 1 },
            { Name: Site.getSingleResource("SystemId"), IsShow: 1 }, { Name: Site.getSingleResource("ExternalId"), IsShow: 1 },
        { Name: Site.getSingleResource("TransTime"), IsShow: 1 },
            { Name: Site.getSingleResource("Choice"), IsShow: 1 }, { Name: Site.getSingleResource("Odds"), IsShow: 1 },
        { Name: Site.getSingleResource("Stake"), IsShow: 1 }, { Name: Site.getSingleResource("Status"), IsShow: 1 }
        ];
    $this.initialize = function () {
        Site.addUserGuide({ 
            resourceName: "LastBetsMonitoring",
            title: Site.getSingleResource("LastBetsMonitoring")
        });
        $this.updateParams();
        $this.currentParam = Site.clone($this.params);
    };

    $this.numberParam = function (name, value) {
        $this[name] = value === undefined || value == null || value === ""
                            ? 0
                            : parseInt(value);
        $this.updateParams();
    };

    $this.stringParam = function (name, value) {
        $this[name] = value === undefined || value == null || value === ""
                            ? ""
                            : $.trim(value);
        $this.updateParams();
    };

    $this.isValidParam = function () {
        return $this.stakeFrom <= $this.stakeTo
            && $("#currency").multiSelect("getCheckedItems").length > 0
            && $("#special-level").multiSelect("getCheckedItems").length > 0;
    };
    $this.tryUpdateStack = function () {
        var tempF = $("#stake-from").val();
        tempF = tempF.replace(/,/g, '');
        tempF = tempF === undefined || tempF === "" ? 0 : parseInt(tempF);

        $this.stakeFrom = tempF;

        var tempT = $("#stake-to").val();
        tempT = tempT.replace(/,/g, '');
        tempT = tempT === undefined || tempT === "" ? $this.maxStake : parseInt(tempT);
        $this.stakeTo = tempT;

        $this.params.StakeFrom = $this.stakeFrom;
        $this.params.StakeTo = $this.stakeTo;
    };
    $this.updateParams = function () {
        $this.params = {
            SportId: $this.sportId,
            LeagueId: $this.leagueId,
            MatchId: $this.matchId,
            BetType: $this.betType,
            LiveStatus: $this.liveStatus,
            OddsType: $this.oddsType,
            Level: $this.level,
            TimeZone: $this.timeZone,
            Currency: $this.currency,
            BasedCurrency: $this.basedCurrency,
            StakeFrom: $this.stakeFrom,
            StakeTo: $this.stakeTo,
            PageSize: $this.pageSize,
            TransId: $this.lastTransId,
            BetFrom: $this.betFrom
        };

        $this.currentParam = Site.clone($this.params);
    };

    $this.Submit = function (freeStatus) {
        $this.stopMonitor();
        $this.tryUpdateStack();
        $this.currentParam = Site.clone($this.params);
        $this.isCriteriaChange = true;

        if (parseInt(freeStatus) === 1) {
            $this.startMonitor();
        }
        else {
            $this.stopMonitor();
        }
    };

    /*------------------------------------------------------------------------------------------
    Get data from database
    ------------------------------------------------------------------------------------------*/
    $this.reloadData = function () {
        if ($this.isLoading) {
            $this.doMonitor();
            return;
        }

        if (!$this.isValidParam()) {
            return;
        }

        $this.isLoading = true;

        var loadingTickBegin = (new Date()).getTime();
        if (Number($this.lastTransId) === 0 || $this.isCriteriaChange) {
            $this.clearContent();
        }

        $.ajax({
            url: $this.serviceUrl,
            type: "POST",
            data: JSON.stringify($this.currentParam),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            async: true,
            cache: false,
            timeout: 10000,
            beforeSend: function () {
                $("#div-loading").show();
                $this.isLoading = true;
                $this.isCriteriaChange = false;
            },
            complete: function () {
                $("#div-loading").hide();
                var numOfRow = $("tr", $this.containerTable).length - 1 - $(".no-info-row").length;
                if (numOfRow > 0) {
                    $(".no-info-row").remove();
                }
            },
            success: function (result) {
                if ($this.isCriteriaChange) {
                    $this.isLoading = false;
                    $this.doMonitor();
                    return;
                }

                // Only bind data if current mode == mode at the begining of request
                $this.betListResult = result;
                $this.bindData();

                var loadingTime = (new Date()).getTime() - loadingTickBegin;
                $this.intervalMiliseconds = loadingTime > $this.minLoadingMiliseconds
                ? $this.intervalMiliseconds + 1000
                : $this.minInterval;

                // Continue
                $this.doMonitor();
            },
            error: function (/*jqXHR, textStatus, errorThrown*/) {
                $("#div-loading").hide();

                $this.betListResult.Updated = false;
                $this.betListResult.Rows = "";
                $this.isLoading = false;
                $this.intervalMiliseconds += 1000;
                $this.doMonitor();
            }
        });
    };

    $this.bindData = function () {
        // If there is no new bet, process error message
        if (!$this.betListResult.NewRow) {
            $this.isLoading = false;

            var message = $this.betListResult.Message;
            if (message !== undefined && message.length > 0) {
                $this.bindNoInfoRow(message);
                return;
            }

            var rowCount = $("tr.content-row", $this.containerTable).length;
            if (rowCount === 0) {
                $this.bindNoInfoRow(message);
            }

            return;
        }

        /*--------------------------------------------------------------------------------------------
        Insert new rows from database
        Remove old rows
        Update last transId
        --------------------------------------------------------------------------------------------*/
        //get hide columns
        var hideColumns = JSLINQ($this.columns).Where(function (item) { return Number(item.IsShow) === 0; })
                                        .ToArray();
        if (hideColumns.length > 0) {
            var rows = $this.betListResult.Rows;
            var display = " style=\"display:none;\"";
            $.each(hideColumns, function (index, value) {
                var name = "data-name=\"" + value.Name + "\"";
                rows = rows.replaceAll(name, name + display);
            });
            $this.headerRow.after(rows);
        } else {
            $this.headerRow.after($this.betListResult.Rows);
        }
        $("tr:gt(" + $this.pageSize + ")", $this.containerTable).remove();

        if ($this.betListResult) {
            $this.lastTransId = $this.betListResult.LastTransId;
            $this.currentParam.TransId = $this.lastTransId;
        }
        setTimeout(function () {
            return $this.unHighlight($this.lastTransId);
        },
         6000);

        $this.isLoading = false;
    };

    $this.bindNoInfoRow = function (text) {
        var noInfoMessage = text === undefined || text.length === 0
                        ? Site.getSingleResource("NoInfo")
                        : text;
        var colspan = JSLINQ($this.columns).Count(function (item) { return Number(item.IsShow) === 1; });

        var row = "<tr class='content-row no-info-row'><td colspan='" + colspan + "'>" + noInfoMessage + "</td></tr>";
        $("tr:gt(0)", $this.containerTable).remove();
        $this.containerTable.append(row);
    };

    /*------------------------------------------------------------------------------------------
    Remove high light row after a duration
    ------------------------------------------------------------------------------------------*/
    $this.unHighlight = function (lastTransId) {
        $("tr.content-row", $this.containerTable).each(function (i, e) {
            var transId = $(e).attr("data-transid");
            if (transId <= lastTransId) {
                $(e).removeClass($this.highlightClass);
            }
        });
    };

    $this.clearContent = function () {
        $this.lastTransId = 0;
        $this.currentParam.TransId = $this.lastTransId;
        $this.params.TransId = $this.lastTransId;
        $this.bindNoInfoRow();
    };

    $this.startMonitor = function () {
        this.stopMonitor();

        $this.lastTransId = 0;
        $this.intervalMiliseconds = $this.minInterval;
        $this.doMonitor();
    };

    $this.stopMonitor = function () {
        if (this.intervalId > 0) {
            clearTimeout(this.intervalId);
        }
    };

    $this.doMonitor = function () {
        if ($this.intervalId > 0) {
            clearTimeout($this.intervalId);
        }

        $this.intervalId = setTimeout(function () { $this.reloadData(); }, $this.intervalMiliseconds);
    };
};

var MonitorBetListUI = function (monitorObj) {
    var $this = this;

    $this.highLightSubmitIntervalId = 0;
    $this.monitor = monitorObj;

    $this.currentMatchList = [];
    $this.currentLeagueList = [];

    $this.initMainObject = function () {
        $this.monitor.containerTable = $($this.monitor.containerTableId);
        $this.monitor.headerRow = $($this.monitor.headerRowId);
    };

    $this.initSports = function () {
        $("#sports").change(function () {
            $this.setHighlightSubmit();
            var sportId = Number($(this).val());
            $this.monitor.numberParam("sportId", sportId);

            var mixParlaySportId = 999;
            $("#league").html("<option value='0'>" + Site.getSingleResource("All") + "</option>");
            $("#match").prop("disabled", true);

            if (sportId === 0 || sportId === mixParlaySportId) {
                $("#league").prop("disabled", true);
            }
            else {
                $("#league").prop("disabled", false);

                // Reload league list & match list
                $.ajax({
                    url: Site.resolveClientUrl('api/betlist/GetRunningMatchList'),
                    data: "sportid=" + sportId,
                    dataType: "json",
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function () {
                        Site.blockUI();
                    },
                    complete: function () {
                        Site.unBlockUI();
                    },
                    success: function (data) {
                        $this.currentMatchList = data.MatchList;
                        var leagueDropdown = $("#league");
                        $this.currentLeagueList = data.LeagueList;
                        $(data.LeagueList).each(function (i, e) {
                            leagueDropdown.append("<option value='" + e.LeagueId + "'>" + e.LeagueName + "</option>");
                        });
                    }
                });
            }

            $("#match").html("<option value='0'>" + Site.getSingleResource("All") + "</option>");
            //update leagueId and matchId
            $this.monitor.numberParam("leagueId", $("#league").val());
            $this.monitor.numberParam("matchId", $("#match").val());
        });
    };

    $this.initLeague = function () {
        $("#league").change(function () {
            $this.setHighlightSubmit();
            var leagueId = Number($(this).val());
            $this.monitor.numberParam("leagueId", leagueId);
            $("#match").html("<option value='0'>" + Site.getSingleResource("All") + "</option>");

            if (leagueId === 0) {
                $("#match").prop("disabled", true);
            }
            else {
                // Bind match league
                var leagueItem = JSLINQ($this.currentLeagueList).First(function (item) { return Number(item.LeagueId) === leagueId; });
                if (leagueItem !== undefined && leagueItem !== null && Site.asBool(leagueItem.IsOutRight) === false) {
                    var matchList = JSLINQ($this.currentMatchList).Where(function (item) { return Number(item.LeagueId) === leagueId; })
                                        .ToArray();
                    var matchDropdown = $("#match");
                    $(matchList).each(function (i, e) {
                        matchDropdown.append("<option value='" + e.MatchId + "'>" + e.MatchName + "</option>");
                    });
                    if (matchList.length > 0) {
                        $("#match").prop("disabled", false);
                    }
                }
                else {
                    $("#match").prop("disabled", true);
                }
            }
            //update matchId
            $this.monitor.numberParam("matchId", $("#match").val());
        });
    };

    $this.initMatch = function () {
        $("#match").change(function () {
            $this.setHighlightSubmit();

            $this.monitor.numberParam("matchId", $("#match").val());
        });
    };

    $this.initBetType = function () {
        $("#bet-type").change(function () {
            $this.setHighlightSubmit();

            $this.monitor.stringParam("betType", $("#bet-type").val());
        });
    };

    $this.initLiveStatus = function () {
        $("#live-status").change(function () {
            $this.setHighlightSubmit();

            $this.monitor.stringParam("liveStatus", $("#live-status").val());
        });
    };

    $this.initOddsType = function () {
        $("#odds-type").change(function () {
            $this.setHighlightSubmit();

            $this.monitor.stringParam("oddsType", $("#odds-type").val());
        });
    };

    $this.initSpecialLevel = function () {
        var specialList = [];
        for (var i = 0; i <= 10; i++) {
            specialList.push({
                Id: "special-option-" + i,
                Value: i.toString(),
                Text: i === 0 ? Site.getSingleResource("None") : i.toString(),
                Checked: true,
                Enable: true,
                Css: "",
                CssClass: ""
            });
        }

        $("#special-level").multiSelect(
        {
            minWidth: 130,
            maxHeight: 400,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: Site.getSingleResource('All'),
            noneSelectedText: Site.getSingleResource("NoneLevel"), // Text show on Multiselect when it has no selected item
            selectedText: Site.getSingleResource("SelectedItemCount"), // Text show when item was selected
            listData: specialList,
            onCheck: function () {
                $this.setHighlightSubmit();

                var selectedLevel = $("#special-level").multiSelect("getCheckedItems");
                selectedLevel = selectedLevel.length === 11 ? "" : selectedLevel.join(",");
                $this.monitor.stringParam("level", selectedLevel);
            }
        });
    };

    $this.initCurrency = function () {
        var currencyList = [];
        $("#currency option").each(function (i, e) {
            currencyList.push({
                Id: "currency-" + i,
                Value: $(e).val(),
                Text: $(e).text(),
                Checked: true,
                Enable: true,
                Css: "",
                CssClass: ""
            });
        });

        var currencyCount = currencyList.length;
        $("#currency").html("");
        $("#currency").multiSelect(
        {
            minWidth: 100,
            maxHeight: 400,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: Site.getSingleResource('All'),
            noneSelectedText: Site.getSingleResource("NoneCurrency"), // Text show on Multiselect when it has no selected item
            selectedText: Site.getSingleResource("SelectedItemCount"), // Text show when item was selected
            listData: currencyList,
            onCheck: function () {
                $this.setHighlightSubmit();

                var selectedCurrency = $("#currency").multiSelect("getCheckedItems");
                selectedCurrency = currencyCount === selectedCurrency.length ? "" : selectedCurrency.join(",");

                // Update based currency
                var selectedCurrencyText = $("#currency").multiSelect("getCheckedItemsByText");
                var baseCurrencyObj = $("#based-currency");
                var noConvertOption = $("option[value='']", baseCurrencyObj);

                if (selectedCurrencyText.length > 1) {
                    noConvertOption.remove();
                    baseCurrencyObj.val("eur");
                }
                else if (selectedCurrencyText.length === 1) {
                    var currencyName = selectedCurrencyText[0];
                    if (currencyName.toLowerCase() === "eur" || currencyName.toLowerCase() === "usd") {
                        noConvertOption.remove();
                        baseCurrencyObj.val(currencyName.toLowerCase() === "usd" ? "usd" : "eur");
                    }
                    else {
                        if (noConvertOption.length === 0) {
                            $("<option value=''>" + currencyName + "</option>").appendTo(baseCurrencyObj);
                        } else {
                            $("option[value='']", baseCurrencyObj).text(currencyName);
                        }
                        baseCurrencyObj.val("");
                    }
                }
                $this.monitor.stringParam("currency", selectedCurrency);
                $this.monitor.stringParam("basedCurrency", $("#based-currency option:selected").text());
            }
        });
    };

    $this.initTimeZone = function () {
        $("#time-zone").change(function () {
            $this.setHighlightSubmit();

            $this.monitor.numberParam("timeZone", $("#time-zone").val());
        });
    };

    $this.initBasedCurrency = function () {
        $("#based-currency").change(function () {
            $this.setHighlightSubmit();

            $this.monitor.stringParam("basedCurrency", $("#based-currency").val());
        });
    };

    $this.initStake = function () {
        var maxStake = $this.monitor.maxStake;
        $("#stake-from").numbericTextBox(0, maxStake);
        $("#stake-to").numbericTextBox(0, maxStake);
        $("#stake-from", "#stake-to").keyup(function () {
            $this.setHighlightSubmit();
        });

        $("#stake-from").change(function () {
            $this.setHighlightSubmit();

            var stake = $("#stake-from").val();
            stake = stake.replace(/,/g, '');
            stake = stake === undefined || stake === "" ? 0 : parseInt(stake);
            $this.monitor.numberParam("stakeFrom", stake);
        });

        $("#stake-to").change(function () {
            $this.setHighlightSubmit();

            var stake = $("#stake-to").val();

            stake = stake.replace(/,/g, '');
            stake = stake === undefined || stake === "" ? maxStake : parseInt(stake);
            $this.monitor.numberParam("stakeTo", $("#stake-to").val());
        });
    };

    $this.initPageSizeControl = function () {
        $("#pagesize input").click(function () {
            $this.setHighlightSubmit();

            $("#pagesize input").prop("disabled", false);
            $(this).prop("disabled", true);
            $this.monitor.numberParam("pageSize", $(this).val());
        });
    };

    $this.initFreezeButton = function () {
        $("#freeze-button").click(function () {
            var currentStatus = Number($(this).attr("data-status"));
            var newStatus = currentStatus === 1 ? 0 : 1;
            var newValue = newStatus ? Site.getSingleResource("Freeze") : Site.getSingleResource("Unfreeze");
            if (newStatus) {
                monitor.startMonitor();
            }
            else {
                monitor.stopMonitor();
            }

            $(this).val(newValue);
            $(this).attr("data-status", newStatus);
        });
    };

    $this.initHighlight = function () {
        $this.monitor.containerTable.on("click", "tr", function () {
            $(this).toggleClass("monitor-keep");
        });
    };
    $this.setHighlightSubmit = function () {
        clearInterval($this.highLightSubmitIntervalId);
        $this.highLightSubmitIntervalId = setInterval(function () {
            $("#submit-button").css("background-color", "rgb(211,211,211)");
            setTimeout(function () {
                $("#submit-button").css("background-color", "buttonface");
            }, 500);
        }, 1000);
    };
    $this.initSubmit = function () {
        $("#submit-button").click(function () {
            clearInterval($this.highLightSubmitIntervalId);
            $("#submit-button").css("background-color", "");
            var freeStatus = $("#freeze-button").attr("data-status");
            $this.monitor.Submit(freeStatus);
        });
    };
    $this.initColumns = function () {
        var columns = [];

        $.each($this.monitor.columns, function (i, e) {
            columns.push({
                Id: "column-" + i,
                Value: e.Name,
                Text: e.Name,
                Checked: Number(e.IsShow) === 1 ? true : false,
                Enable: true,
                Css: "",
                CssClass: ""
            });
            if (Number(e.IsShow) === 0) {
                $("td[data-name=\"" + e.Name + "\"]").hide();
            }
            $("#se-column").append('<option value="' + e.Name + '">' + e.Name + " " + '</option>');
        });
        $("#se-column").multiSelect(
        {
            minWidth: 103,
            maxHeight: 400,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: Site.getSingleResource('All'),
            noneSelectedText: Site.getSingleResource("None"),
            selectedText: Site.getSingleResource("SelectedItemCount"),
            listData: columns,
            onCheck: function () {
                var selectedLevel = $("#se-column").multiSelect("getCheckedItems");
                if (selectedLevel.length === 8) {
                    $.each($this.monitor.columns, function (i, e) {
                        e.IsShow = 1; $('td[data-name="' + e.Name + '"]').show();
                    });
                } else {
                    $.each($this.monitor.columns, function (i, e) {
                        var showItem = $.inArray(e.Name, selectedLevel);
                        if (showItem > -1) {
                            e.IsShow = 1;
                            $('td[data-name="' + e.Name + '"]').show();
                        } else {
                            e.IsShow = 0;
                            //hide columns
                            $('td[data-name="' + e.Name + '"]').hide();
                        }
                    });
                }
            }
        });
    };
    $this.initPlatForm = function () {
        $("#bet-form").change(function () {
            $this.setHighlightSubmit();
            var value = $(this).val();
            $this.monitor.numberParam("betFrom", value);
        });
    };
    $this.initialize = function () {
        $this.initMainObject();
        $this.initSports();
        $this.initLeague();
        $this.initMatch();
        $this.initBetType();
        $this.initLiveStatus();
        $this.initOddsType();
        $this.initSpecialLevel();
        $this.initCurrency();
        $this.initTimeZone();
        $this.initBasedCurrency();
        $this.initStake();
        $this.initPageSizeControl();
        $this.initFreezeButton();
        $this.initHighlight();
        $this.initSubmit();
        $this.initPlatForm();
        $this.initColumns();
    };

    return $this;
};

/*******************/

$(window).load(function () {
    monitor = new MonitorBetList();
    monitor.initialize();
    monitorUi = new MonitorBetListUI(monitor);

    monitorUi.initialize();
    monitor.startMonitor();
});

$(window).unload(function () {
    monitor.stopMonitor();
    monitor = null;
});