"use strict";
define(
    ['jquery', 'common/site', "./lastbet.grid", "lib/jquery.multiselect", 'extensions/jquery.extension'],
    function ($, Site, LastBetGrid) {
        var _me = {};

        var _highLightSubmitIntervalId = 0;

        var _currentMatchList = [];
        var _currentLeagueList = [];

        var SPECIAL_LEVEL_LENGTH = 11;
        var DataType = {
            NUMBER: "number",
            STRING: "string"
        };

        var Selector = {
            League: "#league",
            Match: "#match"
        };

        _me.initSports = function () {
            $("#sports").change(function () {
                var sportId = Number($(this).val());
                _me.changeFilterParam(DataType.NUMBER, "SportId", sportId);

                var mixParlaySportId = 999;
                var $leagueDropdown = $(Selector.League);
                var $matchDropdown = $(Selector.Match);

                var htmlAllOption = "<option value='0'>" + Site.getSingleResource("All") + "</option>";
                $leagueDropdown.html(htmlAllOption);
                $matchDropdown.html(htmlAllOption);

                var canRequest = sportId !== 0 && sportId !== mixParlaySportId;
                $leagueDropdown.selectList("disabled", !canRequest);
                if (canRequest) {
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
                            _currentLeagueList = data.LeagueList;

                            var htmlOption = "";
                            $(data.LeagueList).each(function (i, e) {
                                htmlOption += "<option value='" + e.LeagueId + "'>" + e.LeagueName + "</option>";
                            });

                            $leagueDropdown.append(htmlOption);
                        }
                    });
                }

                $leagueDropdown.selectList("update", { isDisabled: !canRequest });
                $matchDropdown.selectList("update", { isDisabled: true });
                LastBetGrid.updateParam(DataType.NUMBER, "LeagueId", $leagueDropdown.val());
                LastBetGrid.updateParam(DataType.NUMBER, "MatchId", $matchDropdown.val());
            });
        };

        _me.initLeague = function () {
            $(Selector.League).change(function () {
                var leagueId = Number($(this).val());
                _me.changeFilterParam(DataType.NUMBER, "LeagueId", leagueId);

                var $matchDropdown = $(Selector.Match);
                $matchDropdown.html("<option value='0'>" + Site.getSingleResource("All") + "</option>");

                var isShowAllLeague = leagueId === 0;
                var canShowMatch = !isShowAllLeague;
                if (!isShowAllLeague) {
                    var filteredLeague = _currentLeagueList.filter(function (item) { return item.LeagueId === leagueId; });
                    var leagueItem = filteredLeague.length ? filteredLeague[0] : null;

                    canShowMatch = leagueItem && Site.asBool(leagueItem.IsOutRight) === false;
                    if (canShowMatch) {
                        var matchList = _currentMatchList.filter(function (item) { return item.LeagueId === leagueId; });

                        var htmlOption = "";
                        $(matchList).each(function (i, e) {
                            htmlOption += "<option value='" + e.MatchId + "'>" + e.MatchName + "</option>";
                        });

                        $matchDropdown.append(htmlOption);
                    }
                }

                $matchDropdown.selectList("disabled", isShowAllLeague && !canShowMatch);
                $matchDropdown.selectList("update", { isDisabled: isShowAllLeague && !canShowMatch });
                LastBetGrid.updateParam(DataType.NUMBER, "MatchId", $matchDropdown.val());
            });
        };

        _me.initMatch = function () {
            $(Selector.Match).change(function () {
                _me.changeFilterParam(DataType.NUMBER, "MatchId", $(this).val());
            });
        };

        _me.initBetType = function () {
            $("#bet-type").change(function () {
                _me.changeFilterParam(DataType.STRING, "BetType", $(this).val());
            });
        };

        _me.initLiveStatus = function () {
            $("#live-status").change(function () {
                _me.changeFilterParam(DataType.STRING, "LiveStatus", $(this).val());
            });
        };

        _me.initOddsType = function () {
            $("#odds-type").change(function () {
                _me.changeFilterParam(DataType.STRING, "OddsType", $(this).val());
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

            $("#special-level").multiSelect({
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
                    var selectedLevel = $("#special-level").multiSelect("getCheckedItems");
                    selectedLevel = selectedLevel.length === SPECIAL_LEVEL_LENGTH ? "" : selectedLevel.join(",");

                    _me.changeFilterParam(DataType.STRING, "Level", selectedLevel);
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
            $("#currency").multiSelect({
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
                    var selectedCurrency = $("#currency").multiSelect("getCheckedItems");
                    selectedCurrency = currencyCount === selectedCurrency.length ? "" : selectedCurrency.join(",");
                    _me.changeFilterParam(DataType.STRING, "Currency", selectedCurrency);

                    // Update based currency
                    var selectedCurrencyText = $("#currency").multiSelect("getCheckedItemsByText");
                    var baseCurrencyObj = $("#BaseCurrency");
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
                    baseCurrencyObj.selectList("update");
                    LastBetGrid.updateParam(DataType.STRING, "BasedCurrency", $("#BaseCurrency option:selected").text());
                }
            });
        };

        _me.initTimeZone = function () {
            $("#time-zone").change(function () {
                _me.changeFilterParam(DataType.NUMBER, "TimeZone", $(this).val());
            });
        };

        _me.initBasedCurrency = function () {
            $("#BaseCurrency").change(function () {
                _me.changeFilterParam(DataType.STRING, "BasedCurrency", $(this).val());
            });
        };

        _me.initStake = function () {
            var maxStake = LastBetGrid.getMaxStake();
            $("#stake-from").numbericTextBox(0, maxStake);
            $("#stake-to").numbericTextBox(0, maxStake);
            $("#stake-from, #stake-to").keyup(function () {
                _me.setHighlightSubmit();
            });

            function changeHandler(paramName, stake, defaultValue) {
                stake = stake.replace(/,/g, '');
                stake = stake === "" ? defaultValue : parseInt(stake);
                _me.changeFilterParam(DataType.NUMBER, paramName, stake);
            }

            $("#stake-from").change(function () {
                changeHandler("StakeFrom", $(this).val(), 0);
            });

            $("#stake-to").change(function () {
                changeHandler("StakeTo", $(this).val(), maxStake);
            });
        };

        _me.initPageSizeControl = function () {
            $("#pagesize a.label").click(function () {
                var $this = $(this);
                if ($this.hasClass("active")) {
                    return;
                }

                $("#pagesize a.label").removeClass("active");
                $this.addClass("active");

                _me.changeFilterParam(DataType.NUMBER, "PageSize", $this.text());
            });
        };

        _me.initFreezeButton = function () {
            $("#FreezeButton").click(function () {
                var $this = $(this);
                var currentStatus = Number($this.data("status"));
                var newStatus = currentStatus === 1 ? 0 : 1;
                var newValue = newStatus ? Site.getSingleResource("Freeze") : Site.getSingleResource("Unfreeze");
                if (newStatus) {
                    LastBetGrid.startMonitor();
                }
                else {
                    LastBetGrid.stopMonitor();
                }

                $this.text(newValue);
                $this.data("status", newStatus);
            });
        };

        _me.setHighlightSubmit = function () {
            clearInterval(_highLightSubmitIntervalId);
            _highLightSubmitIntervalId = setInterval(function () {
                $("#SubmitButton").addClass("btn-alert");
                setTimeout(function () {
                    $("#SubmitButton").removeClass("btn-alert");
                }, 500);
            }, 1000);
        };
        _me.initSubmit = function () {
            $("#SubmitButton").click(function () {
                clearInterval(_highLightSubmitIntervalId);
                $("#SubmitButton").removeClass("btn-alert");
                var freeStatus = $("#FreezeButton").data("status");
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
                    Checked: e.IsShow === 1 ? true : false,
                    Enable: true,
                    Css: "",
                    CssClass: ""
                });
                if (e.IsShow === 0) {
                    $("td[data-name=\"" + e.Name + "\"]").hide();
                }
                $("#se-column").append('<option value="' + e.Name + '">' + e.Name + " " + '</option>');
            });

            $("#se-column").multiSelect({
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
                    var isShowAll = selectedLevel.length === LastBetGrid.columns.length;
                    $.each(LastBetGrid.columns, function (index, item) {
                        var showItem = selectedLevel.indexOf(item.Name) > -1;
                        item.IsShow = isShowAll || showItem ? 1 : 0;
                        $('tr').find('[data-name="' + item.Name + '"]').toggle(isShowAll || showItem);
                    });
                }
            });
        };
        _me.initPlatForm = function () {
            $("#bet-from").change(function () {
                _me.changeFilterParam(DataType.NUMBER, "BetFrom", $(this).val());
            });
        };

        _me.changeFilterParam = function (type, paramName, value) {
            _me.setHighlightSubmit();
            LastBetGrid.updateParam(type, paramName, value);
        };

        _me.initialize = function () {
            Site.initialize();
            Site.addUserGuide({
                resourceName: "LastBetsMonitoring",
                title: Site.getSingleResource("LastBetsMonitoring")
            });

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

            $(Selector.League).selectList("disabled", true);
            $(Selector.Match).selectList("disabled", true);
        };

        return _me;
    });