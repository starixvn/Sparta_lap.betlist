webpackJsonp([42],{

/***/ 0:
/***/ (function(module, exports, __webpack_require__) {

	module.exports = __webpack_require__(225);


/***/ }),

/***/ 225:
/***/ (function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(226), __webpack_require__(228)], __WEBPACK_AMD_DEFINE_RESULT__ = function (LastBetFilter, LastBetGrid) {
	    "use strict";
	
	    $(function () {
	        LastBetFilter.initialize();
	        LastBetGrid.initialize();
	
	        LastBetGrid.startMonitor();
	    });
	
	    $(window).on("unload", function () {
	        LastBetGrid.stopMonitor();
	    });
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ }),

/***/ 226:
/***/ (function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;"use strict";
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(227),
	    __webpack_require__(228), __webpack_require__(11), __webpack_require__(229)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, MultiSelect, LastBetGrid) {
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
	            $("#pagesize input").click(function () {
	                _me.setHighlightSubmit();
	
	                $("#pagesize input").prop("disabled", false);
	                $(this).prop("disabled", true);
	                LastBetGrid.numberParam("PageSize", $(this).val());
	            });
	        };
	
	        _me.initFreezeButton = function () {
	            $("#freeze-button").click(function () {
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
	                $("#submit-button").css("background-color", "rgb(211,211,211)");
	                setTimeout(function () {
	                    $("#submit-button").css("background-color", "buttonface");
	                }, 500);
	            }, 1000);
	        };
	        _me.initSubmit = function () {
	            $("#submit-button").click(function () {
	                clearInterval(_highLightSubmitIntervalId);
	                $("#submit-button").css("background-color", "");
	                var freeStatus = $("#freeze-button").attr("data-status");
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
	    }.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ 227:
/***/ (function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;"use strict";
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(11)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site) {
	    var _me = {
	        initProductList: function () {
	            var selector = "#grid-products-multiselect";
	            var data = $.parseJSON($("#productsData").val());
	            var length = data.length;
	            var dataUI = [];
	            var checkedCount = 0;
	            for (var i = 0; i < length; i++) {
	                var item = data[i];
	                var isChecked = item["Status"];
	                if (isChecked) {
	                    checkedCount++;
	                }
	
	                dataUI.push({
	                    Id: item["Code"],
	                    Value: item["Code"],
	                    Text: Site.getSingleResource(item["Name"]),
	                    Checked: isChecked,
	                    Enable: true
	                });
	            }
	
	            var isCheckAll = checkedCount === length;
	            $(selector).multiSelect({
	                minWidth: 200,
	                maxHeight: 400,
	                shadow: true,
	                showHeader: true,
	                isCheckAll: isCheckAll,
	                checkAllText: Site.getSingleResource('All'),
	                showFooter: false,
	                listData: dataUI,
	                onCheck: function () {
	                    var checkedItems = $(selector).multiSelect("getCheckedItems");
	                    $("#product-container input[type=checkbox]:checked").prop("checked", false);
	                    var hasItemsChecked = checkedItems.length > 0;
	                    if (hasItemsChecked) {
	                        for (var i = 0; i < checkedItems.length; i++) {
	                            $('#product-container #' + checkedItems[i]).prop("checked", true);
	                        }
	                    }
	                }
	            });
	        }
	    };
	
	    return _me;
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ }),

/***/ 228:
/***/ (function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;"use strict";
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site) {
	    var _me = {};
	
	    // Param
	    var SERVICE_URL = Site.resolveClientUrl('api/LastBetMonitoringBetList/Index');
	
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
	        Currency: "0",
	        BasedCurrency: $("#last-bet-monitor-base-currency").val(),
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
	            url: SERVICE_URL,
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
	        var hideColumns = JSLINQ(_me.columns).Where(function (item) { return Number(item.IsShow) === 0; })
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
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));


/***/ }),

/***/ 229:
/***/ (function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(jQuery) {/// <reference path="/@JSense.js" />
	"use strict";
	(function ($) {
	    $.fn.checkboxgroup = function () {
	        var allChk = this.first();
	        var elseChk = this.slice(1);
	
	        allChk.prop('checked', elseChk.filter(":checked").length >= elseChk.length);
	
	        allChk.change(function () {
	            var thisIsChecked = $(this).is(':checked');
	            elseChk.prop('checked', thisIsChecked);
	        });
	
	        elseChk.change(function () {
	            if ($(this).is(':checked')) {
	                allChk.prop('checked', elseChk.filter(":checked").length >= elseChk.length);
	            } else {
	                allChk.prop('checked', false);
	            }
	        });
	
	        return this;
	    };
	
	    $.fn.showHide = function (isShow) {
	        if (isShow) {
	            this.show();
	        } else {
	            this.hide();
	        }
	
	        return this;
	    };
	
	    jQuery.fn.serializeObject = function () {
	        var o = {};
	        var a = this.serializeArray();
	        $.each(a, function () {
	            if (o[this.name] !== undefined) {
	                if (!o[this.name].push) {
	                    o[this.name] = [o[this.name]];
	                }
	                o[this.name].push(this.value || '');
	            } else {
	                o[this.name] = this.value || '';
	            }
	        });
	        return o;
	    };
	
	    $.fn.getStyleObject = function () {
	        /// <summary>
	        /// don't work on ie8
	        /// </summary>
	        /// <returns type=""></returns>
	        var dom = this.get(0);
	        if(!dom)
	        {
	            return;
	        }
	
	        var style;
	        var returns = {};
	
	        if (window.getComputedStyle) {
	            var camelize = function (a, b) {
	                return b.toUpperCase();
	            };
	            style = window.getComputedStyle(dom, null);
	            for (var i = 0, l = style.length; i < l; i++) {
	                var prop = style[i];
	                var camel = prop.replace(/\-([a-z])/, camelize);
	                var val = style.getPropertyValue(prop);
	                returns[camel] = val;
	            }
	            return returns;
	        }
	        try {
	            if (dom.currentStyle) {
	                style = dom.currentStyle;
	                for (var p1 in style) {
	                    returns[p1] = style[p1];
	                }
	                return returns;
	            }
	            if (style === dom.style) {
	                for (var p2 in style) {
	                    if (typeof style[p2] !== 'function') {
	                        returns[p2] = style[p2];
	                    }
	                }
	                return returns;
	            }
	        } catch (err) {
	        }
	        return returns;
	    };
	
	    jQuery.fn.tableRowSpan = function (colIndexs, effectToIndex) {
	        /// <summary>
	        /// Created by Alex@20130228
	        /// set table rowspan
	        /// </summary>
	        /// <param name="colIndexs">column indexs</param>
	        /// <param name="effectToIndex">set rowspan for this columns base on lasted column in [colIndexs]</param>
	        if (effectToIndex !== undefined) {
	            if ($.inArray(effectToIndex, colIndexs) > -1) {
	                return;
	            }
	        }
	
	        function process(table, colIdx) {
	            var that;
	            var ethat;
	            var ethis;
	            var rowspan = 1;
	            var len = table.rows.length - 1;
	            for(var index = 0; index <= len; index++)
	            {
	                var currentRow = table.rows[index];                
	                var td = currentRow.cells[colIdx];
	                if(td) {
	                    ethis = currentRow.cells[effectToIndex];
	                    var found = that != null && $(td).html() === $(that).html();
	                    if (found && index < len) {
	                        rowspan++;
	                        $(td).hide();
	                        if (effectToIndex !== undefined) {
	                            $(ethis).hide();
	                        }
	                        //.remove// do your action for the old cell here
	                    }
	                    else if (found && index === len) {
	                        //do your action for lasted row
	                        $(td).hide();
	                        rowspan++;
	                        $(that).attr("rowSpan", rowspan);
	                        that = td;
	                        if (effectToIndex !== undefined) {
	                            $(ethis).hide();
	                            $(ethat).attr("rowSpan", rowspan);
	                            ethat = ethis;
	                        }
	                        rowspan = 1;
	                        // do your action for the colSpan cell here
	                    }
	                    else {
	                        if (effectToIndex !== undefined) {
	                            $(ethat).attr("rowSpan", rowspan);
	                            ethat = ethis;
	                        }
	                        $(that).attr("rowSpan", rowspan);
	                        // do your action for the colSpan cell here
	                        that = td;
	                        rowspan = 1;
	                    }
	                    that = (that == null) ? td : that;
	                    if (effectToIndex !== undefined) {
	                        ethat = (ethat == null) ? ethis : ethat;
	                    }
	                    // set the that if not already set
	                }
	            }
	        }
	
	        return this.each(function () {
	            for (var i = 0; i < colIndexs.length; i++) {
	                process(this, colIndexs[i]);
	            }
	        });
	    };
	
	    jQuery.fn.numbericTextBox = function (min, max) {
	        var thousandFormater = function (x) {
	            x = x.toString();
	            var pattern = /(-?\d+)(\d{3})/;
	            while (pattern.test(x)) {
	                x = x.replace(pattern, "$1,$2");
	            }
	            return x;
	        };
	
	        this.each(function (i, e) {
	            $(e).keydown(function (event) {
	                // Allow: backspace, delete, tab, escape, and enter
	                if (event.keyCode === 46 || event.keyCode === 8 || event.keyCode === 9 || event.keyCode === 27 || event.keyCode === 13 ||
	                // Allow: Ctrl+A
	                    (event.keyCode === 65 && event.ctrlKey === true) ||
	                // Allow: home, end, left, right
	                    (event.keyCode >= 35 && event.keyCode <= 39)) {
	                    // let it happen, don't do anything
	                    return;
	                }
	                else {
	                    // Ensure that it is a number and stop the keypress
	                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
	                        event.preventDefault();
	                    }
	                }
	            }).keyup(function (event) {
	                var textBox = event.target;
	                var value = textBox.value;
	                value = value.replace(/,/g, '');
	
	                var isNotInRange = (value < min) || (value > max);
	                if (isNotInRange) {
	                    textBox.value = textBox.value.slice(0, -1);
	                    value = textBox.value.replace(/,/g, '');
	                }
	
	                textBox.value = thousandFormater(value);
	            });
	        });
	
	        return this;
	    };
	
	    $.fn.hasVerticalScrollBar = function () {
	        return this.get(0).scrollHeight > this.height();
	    };
	    $.fn.hasHorizontalScrollBar = function () {
	        return this.get(0).scrollWidth > this.width();
	    };
	})(jQuery);
	
	jQuery.extend({
	    format: function () {
	        /// <summary>
	        /// simply format method
	        /// </summary>
	        /// <returns type="string">formated string</returns>
	        var formatted = arguments[0];
	        var passValue = jQuery.grep(arguments, function (n, i) { return i > 0; });
	        for (var arg in passValue) {
	            formatted = formatted.replace("{" + arg + "}", passValue[arg]);
	        }
	        return formatted;
	    }
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(1)))

/***/ })

});
//# sourceMappingURL=lastbetmonitoring.chunk.js.map