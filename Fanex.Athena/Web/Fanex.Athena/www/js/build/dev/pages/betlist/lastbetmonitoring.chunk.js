webpackJsonp([43],{

/***/ 0:
/***/ function(module, exports, __webpack_require__) {

	module.exports = __webpack_require__(216);


/***/ },

/***/ 216:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function($) {!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(217), __webpack_require__(219)], __WEBPACK_AMD_DEFINE_RESULT__ = function (LastBetFilter, LastBetGrid) {
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

/***/ },

/***/ 217:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;"use strict";
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7), __webpack_require__(218),
	    __webpack_require__(219), __webpack_require__(11), __webpack_require__(220)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site, MultiSelect, LastBetGrid) {
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
	        }
	
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
	                $matchDropdown.prop("disabled", true);
	
	                var isDisableLeague = sportId === 0 || sportId === mixParlaySportId;
	                $leagueDropdown.prop("disabled", isDisableLeague);
	
	                if(!isDisableLeague) {
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
	
	                $leagueDropdown.selectList("update");
	                $matchDropdown.selectList("update");
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
	
	                if (leagueId === 0) {
	                    $matchDropdown.prop("disabled", true);
	                }
	                else {
	                    var filteredLeague = _currentLeagueList.filter(function (item) { return item.LeagueId === leagueId; });
	                    var leagueItem = filteredLeague.length ? filteredLeague[0] : null;
	                    if (leagueItem && Site.asBool(leagueItem.IsOutRight) === false) {
	                        var matchList = _currentMatchList.filter(function (item) { return item.LeagueId === leagueId; });
	                        
	
	                        var htmlOption = "";
	                        $(matchList).each(function (i, e) {
	                            htmlOption += "<option value='" + e.MatchId + "'>" + e.MatchName + "</option>";
	                        });
	
	                        $matchDropdown.append(htmlOption);
	
	                        if (matchList.length > 0) {
	                            $matchDropdown.prop("disabled", false);
	                        }
	                    }
	                    else {
	                        $matchDropdown.prop("disabled", true);
	                    }
	                }
	
	                $matchDropdown.selectList("update");
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
	                _me.changeFilterParam(DataType.STRING, "BetType", $("#bet-type").val());
	            });
	        };
	
	        _me.initLiveStatus = function () {
	            $("#live-status").change(function () {
	                _me.changeFilterParam(DataType.STRING, "LiveStatus", $("#live-status").val());
	            });
	        };
	
	        _me.initOddsType = function () {
	            $("#odds-type").change(function () {
	                _me.changeFilterParam(DataType.STRING, "OddsType", $("#odds-type").val());
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
	                    Site.initSelectElements();
	                    LastBetGrid.updateParam(DataType.STRING, "BasedCurrency", $("#based-currency option:selected").text());
	                }
	            });
	        };
	
	        _me.initTimeZone = function () {
	            $("#time-zone").change(function () {
	                _me.changeFilterParam(DataType.NUMBER, "TimeZone", $("#time-zone").val());
	            });
	        };
	
	        _me.initBasedCurrency = function () {
	            $("#based-currency").change(function () {
	                _me.changeFilterParam(DataType.STRING, "BasedCurrency", $("#based-currency").val());
	            });
	        };
	
	        _me.initStake = function () {
	            var maxStake = LastBetGrid.getMaxStake();
	            $("#stake-from").numbericTextBox(0, maxStake);
	            $("#stake-to").numbericTextBox(0, maxStake);
	            $("#stake-from, #stake-to").keyup(function () {
	                _me.setHighlightSubmit();
	            });
	
	            function changeHandler(paramName, defaultValue) {
	                var stake = $(this).val();
	                stake = stake.replace(/,/g, '');
	                stake = stake === "" ? defaultValue : parseInt(stake);
	                _me.changeFilterParam(DataType.NUMBER, paramName, stake);
	            }
	
	            $("#stake-from").change(function () {
	                changeHandler.call(this, "StakeFrom", 0);
	            });
	
	            $("#stake-to").change(function () {
	                changeHandler.call(this, "StakeTo", maxStake);
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
	                    $.each(LastBetGrid.columns, function (i, e) {
	                        var showItem = selectedLevel.indexOf(e.Name) > -1;
	                        e.IsShow = isShowAll || showItem ? 1 : 0;
	                        $('tr').find('[data-name="' + e.Name + '"]').toggle(isShowAll || showItem);
	                    });
	                }
	            });
	        };
	        _me.initPlatForm = function () {
	            $("#bet-form").change(function () {
	                _me.changeFilterParam(DataType.NUMBER, "BetFrom", $(this).val());
	            });
	        };
	
	        _me.changeFilterParam = function (type, paramName, value) {
	            _me.setHighlightSubmit();
	            LastBetGrid.updateParam(type, paramName, value);
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

/***/ },

/***/ 218:
/***/ function(module, exports, __webpack_require__) {

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
	                    
	                    $("#ProductList").toggleClass("invalid", !hasItemsChecked);
	                    $("#SubmitButton").toggleClass("disabled", !hasItemsChecked);
	                }
	            });
	        }
	    };
	
	    return _me;
	}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));

/***/ },

/***/ 219:
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;"use strict";
	!(__WEBPACK_AMD_DEFINE_ARRAY__ = [__webpack_require__(1), __webpack_require__(7)], __WEBPACK_AMD_DEFINE_RESULT__ = function ($, Site) {
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
	        var _$tableBody;
	        var _$tableHeader;
	
	        // Config
	        var MIN_LOADING_MILISECONDS = 1000;
	        var MIN_INTERVAL = 3000;
	        var HIGHLIGHT_CLASS = "monitor-hightlight";
	        var SELECTED_CLASS = "monitor-keep";
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
	
	            _$tableBody = null;
	            _$tableHeader = null;
	
	            Site.addUserGuide({
	                resourceName: "LastBetsMonitoring",
	                title: Site.getSingleResource("LastBetsMonitoring")
	            });
	
	            _$tableHeader = $("#last-bets-monitoring thead");
	            _$tableBody = $("#last-bets-monitoring tbody");
	            _$tableBody.on("click", "tr", function () {
	                $(this).toggleClass(SELECTED_CLASS);
	            });
	
	            _params.BasedCurrency = $("#last-bet-monitor-base-currency").val();
	        };
	        _me.getMaxStake = function () {
	            return MAX_STAKE;
	        };
	        _me.updateParam = function (type, name, value) {
	            if (type === "number") {
	                _params[name] = value ? parseInt(value) : 0;
	            } else {
	                _params[name] = value ? $.trim(value) : "";
	            }
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
	                    $(".loading").show();
	                    _isLoading = true;
	                    _isCriteriaChange = false;
	                },
	                complete: function () {
	                    $(".loading").hide();
	                    var rowCount = $("tr", _$tableBody).length;
	                    if (rowCount > 0) {
	                        $("#noInfoRow").remove();
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
	
	                    $(".loading").hide();
	
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
	                if (message) {
	                    _me.bindNoInfoRow(message);
	                    return;
	                }
	
	                var rowCount = $("tr.content-row", _$tableBody).length;
	                if (rowCount === 0) {
	                    _me.bindNoInfoRow();
	                }
	
	                return;
	            }
	
	            /*--------------------------------------------------------------------------------------------
	            Insert new rows from database
	            Remove old rows
	            Update last transId
	            --------------------------------------------------------------------------------------------*/
	            //get hide columns
	            var hideColumns = _me.columns.filter(function (item) { return item.IsShow === 0; });
	            var rows = _betListResult.Rows;
	            if (hideColumns.length > 0) {
	                var display = " style=\"display:none;\"";
	                $.each(hideColumns, function (index, value) {
	                    var name = "data-name=\"" + value.Name + "\"";
	                    rows = rows.replaceAll(name, name + display);
	                });
	            }
	
	            _$tableBody.append(rows);
	            $("tr:gt(" + (_params.PageSize - 1) + ")", _$tableBody).remove();
	
	            if (_betListResult) {
	                _lastTransId = _betListResult.LastTransId;
	                _params.TransId = _lastTransId;
	            }
	            setTimeout(function () {
	                _me.unHighlight(_lastTransId);
	            }, 6000);
	
	            _isLoading = false;
	        };
	
	        _me.bindNoInfoRow = function (text) {
	            var noInfoMessage = text ? text : Site.getSingleResource("NoInfo");
	
	            var htmlNoInfor = '<p id="noInfoRow" class="no-info-row">' + noInfoMessage + '</p>';
	            $('#noInfoRow').remove();
	            $('#betlist_content').append(htmlNoInfor);
	        };
	
	        /*------------------------------------------------------------------------------------------
	        Remove high light row after a duration
	        ------------------------------------------------------------------------------------------*/
	        _me.unHighlight = function (lastTransId) {
	            $("tr.content-row", _$tableBody).each(function (i, e) {
	                var transId = $(e).data("transid");
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

/***/ },

/***/ 220:
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(jQuery) {/// <reference path="/@JSense.js" />
	"use strict";
	(function ($) {
	    $.fn.checkboxgroup = function () {
	        var $parent = $(this).parent();
	        var allChk = this.first();
	        var elseChk = this.slice(1);
	
	        allChk.prop('checked', elseChk.filter(":checked").length === elseChk.length);
	
	        allChk.change(function () {
	            var thisIsChecked = $(this).is(':checked');
	            elseChk.prop('checked', thisIsChecked);
	            $parent.toggleClass("invalid", !thisIsChecked);
	        });
	
	        elseChk.change(function () {
	            if ($(this).is(':checked')) {
	                allChk.prop('checked', elseChk.filter(":checked").length === elseChk.length);
	            } else {
	                allChk.prop('checked', false);
	            }
	
	            $parent.toggleClass("invalid", elseChk.filter(":checked").length === 0);
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

/***/ }

});
//# sourceMappingURL=lastbetmonitoring.chunk.js.map