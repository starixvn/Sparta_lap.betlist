"use strict";
define(
    ['jquery', 'common/site', "components/pluginhelper/multiselect",
    "./lastbet.grid", "lib/jquery.multiselect", 'extensions/jquery.extension'],
    function ($, Site, MultiSelect, LastBetGrid) {
        var _me = {};

        var _highLightSubmitIntervalId = 0;

        var _currentMatchList = [];
        var _currentLeagueList = [];

        _me.initSports = function () {
            $("#sports").change(function () {
                _me.setHighlightSubmit();
                var sportId = Number($(this).val());
                LastBetGrid.numberParam("SportId", sportId);

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
                        url: Site.resolveClientUrl('api/LastBetMonitoringBetList/GetRunningMatchList'),
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
                            _currentMatchList = data.MatchList;
                            var leagueDropdown = $("#league");
                            _currentLeagueList = data.LeagueList;
                            $(data.LeagueList).each(function (i, e) {
                                leagueDropdown.append("<option value='" + e.LeagueId + "'>" + e.LeagueName + "</option>");
                            });
                        }
                    });
                }

                $("#match").html("<option value='0'>" + Site.getSingleResource("All") + "</option>");
                //update leagueId and matchId
                LastBetGrid.numberParam("LeagueId", $("#league").val());
                LastBetGrid.numberParam("MatchId", $("#match").val());
            });
        };

        _me.initLeague = function () {
            $("#league").change(function () {
                _me.setHighlightSubmit();
                var leagueId = Number($(this).val());
                LastBetGrid.numberParam("LeagueId", leagueId);
                $("#match").html("<option value='0'>" + Site.getSingleResource("All") + "</option>");

                if (leagueId === 0) {
                    $("#match").prop("disabled", true);
                }
                else {
                    // Bind match league
                    var leagueItem = JSLINQ(_currentLeagueList).First(function (item) { return Number(item.LeagueId) === leagueId; });
                    if (leagueItem !== undefined && leagueItem !== null && Site.asBool(leagueItem.IsOutRight) === false) {
                        var matchList = JSLINQ(_currentMatchList).Where(function (item) { return Number(item.LeagueId) === leagueId; })
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
                LastBetGrid.numberParam("MatchId", $("#match").val());
            });
        };

        _me.initMatch = function () {
            $("#match").change(function () {
                _me.setHighlightSubmit();

                LastBetGrid.numberParam("MatchId", $("#match").val());
            });
        };

        _me.initBetType = function () {
            $("#bet-type").change(function () {
                _me.setHighlightSubmit();

                LastBetGrid.stringParam("BetType", $("#bet-type").val());
            });
        };

        _me.initLiveStatus = function () {
            $("#live-status").change(function () {
                _me.setHighlightSubmit();

                LastBetGrid.stringParam("LiveStatus", $("#live-status").val());
            });
        };

        _me.initOddsType = function () {
            $("#odds-type").change(function () {
                _me.setHighlightSubmit();

                LastBetGrid.stringParam("OddsType", $("#odds-type").val());
            });
        };

        _me.initSpecialLevel = function () {
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
                    _me.setHighlightSubmit();

                    var selectedLevel = $("#special-level").multiSelect("getCheckedItems");
                    selectedLevel = selectedLevel.length === 11 ? "" : selectedLevel.join(",");
                    LastBetGrid.stringParam("Level", selectedLevel);
                }
            });
        };

        _me.initCurrency = function () {
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
                    _me.setHighlightSubmit();

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
                        if (Site.isBaseCurrency(currencyName)) {
                            noConvertOption.remove();
                            baseCurrencyObj.val(currencyName.toLowerCase());
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
                    LastBetGrid.stringParam("Currency", selectedCurrency);
                    LastBetGrid.stringParam("BasedCurrency", $("#based-currency option:selected").text());
                }
            });
        };

        _me.initTimeZone = function () {
            $("#time-zone").change(function () {
                _me.setHighlightSubmit();

                LastBetGrid.numberParam("TimeZone", $("#time-zone").val());
            });
        };

        _me.initBasedCurrency = function () {
            $("#based-currency").change(function () {
                _me.setHighlightSubmit();

                LastBetGrid.stringParam("BasedCurrency", $("#based-currency").val());
            });
        };

        _me.initStake = function () {
            var maxStake = LastBetGrid.getMaxStake();
            $("#stake-from").numbericTextBox(0, maxStake);
            $("#stake-to").numbericTextBox(0, maxStake);
            $("#stake-from, #stake-to").keyup(function () {
                _me.setHighlightSubmit();
            });

            $("#stake-from").change(function () {
                _me.setHighlightSubmit();

                var stake = $("#stake-from").val();
                stake = stake.replace(/,/g, '');
                stake = stake === "" ? 0 : parseInt(stake);
                LastBetGrid.numberParam("StakeFrom", stake);
            });

            $("#stake-to").change(function () {
                _me.setHighlightSubmit();

                var stake = $("#stake-to").val();

                stake = stake.replace(/,/g, '');
                stake = stake === "" ? maxStake : parseInt(stake);
                LastBetGrid.numberParam("StakeTo", stake);
            });
        };

        _me.initPageSizeControl = function () {
            $("#pagesize a").click(function () {
                var $this = $(this);
                if ($this.hasClass("active")) {
                    return;
                }

                _me.setHighlightSubmit();

                $("#pagesize a").removeClass("active");
                $this.addClass("active");
                LastBetGrid.numberParam("PageSize", $this.html());
            });
        };

        _me.initFreezeButton = function () {
            $("#FreezeButton").click(function () {
                var currentStatus = Number($(this).attr("data-status"));
                var newStatus = currentStatus === 1 ? 0 : 1;
                var newValue = newStatus ? Site.getSingleResource("Freeze") : Site.getSingleResource("Unfreeze");
                if (newStatus) {
                    LastBetGrid.startMonitor();
                }
                else {
                    LastBetGrid.stopMonitor();
                }

                $(this).val(newValue);
                $(this).attr("data-status", newStatus);
            });
        };

        _me.setHighlightSubmit = function () {
            clearInterval(_highLightSubmitIntervalId);
            _highLightSubmitIntervalId = setInterval(function () {
                $("#SubmitButton").css("background-color", "rgb(211,211,211)");
                setTimeout(function () {
                    $("#SubmitButton").css("background-color", "buttonface");
                }, 500);
            }, 1000);
        };
        _me.initSubmit = function () {
            $("#SubmitButton").click(function () {
                clearInterval(_highLightSubmitIntervalId);
                $("#SubmitButton").css("background-color", "");
                var freeStatus = $("#FreezeButton").attr("data-status");
                LastBetGrid.Submit(freeStatus);
            });
        };
        _me.initColumns = function () {
            var columns = [];

            $.each(LastBetGrid.columns, function (i, e) {
                columns.push({
                    Id: "column-" + i,
                    Value: e.Name,
                    Text: Site.getSingleResource(e.Name),
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
                minWidth: 200,
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
                        $.each(LastBetGrid.columns, function (i, e) {
                            e.IsShow = 1; $('td[data-name="' + e.Name + '"]').show();
                        });
                    } else {
                        $.each(LastBetGrid.columns, function (i, e) {
                            var showItem = $.inArray(e.Name, selectedLevel);
                            if (showItem > -1) {
                                e.IsShow = 1;
                                $('td[data-name="' + e.Name + '"]').show();
                            } else {
                                e.IsShow = 0;
                                $('td[data-name="' + e.Name + '"]').hide();
                            }
                        });
                    }
                }
            });
        };
        _me.initPlatForm = function () {
            $("#bet-form").change(function () {
                _me.setHighlightSubmit();
                var value = $(this).val();
                LastBetGrid.numberParam("BetFrom", value);
            });
        };
        _me.initialize = function () {
            Site.initialize();
            _me.initSports();
            _me.initLeague();
            _me.initMatch();
            _me.initBetType();
            _me.initLiveStatus();
            _me.initOddsType();
            _me.initSpecialLevel();
            _me.initCurrency();
            _me.initTimeZone();
            _me.initBasedCurrency();
            _me.initStake();
            _me.initPageSizeControl();
            _me.initFreezeButton();
            _me.initSubmit();
            _me.initPlatForm();
            _me.initColumns();
        };

        return _me;
    });