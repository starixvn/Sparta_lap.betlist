define(
    ['jquery', 'common/site', 'common/querystring', 'lib/jqgrid/jquery.jqgrid.additional', 'pages/betlist/betlist.option', 'common/popup', 'pages/betlist/betlist'],
    function ($, Site, QueryString, jqGridAdditional, BetListOption, Popup, BetList, moment) {
        describe("BetList", function () {
            function transformQueryParamNameToLowerCase(url) {
                var paramNameRegex = /[&|?][^=]*/g;

                return url.replace(paramNameRegex, function (match) {
                    return match.toLowerCase();
                });
            }

            beforeAll(function () {
                appendSetFixtures('<input id="SortBy" value="Foo" />');
                appendSetFixtures('<input id="SortOrder" value="asc" />');
                spyOn(Site, "initialize");
                spyOn(Site, "getSingleResource").and.callFake(function (text) {
                    return text;
                });
                spyOn(Site, "resolveClientUrl").and.callFake(function (url) {
                    return url;
                });
                //BetList.initialize();
            });

            afterAll(function () {
                $("#SortBy").remove();
                $("#SortOrder").remove();
            });

            describe("getSortParams()", function () {
                it("should return null if _sortColumnName is undefined", function () {
                    $("#SortBy").remove();
                    BetList.initialize();

                    var result = BetList.getSortParams();

                    expect(result).toBe(null);
                });

                it("should return sorting data stored in inputs", function () {
                    appendSetFixtures('<input id="SortBy" value="Foo" />');
                    BetList.initialize();

                    var result = BetList.getSortParams();

                    expect(result).toEqual({ SortBy: 'Foo', SortOrder: 'asc' });
                });
            });

            describe("initialize()", function () {
                it("updateSortColumn should be called if bet list api url if MatchWinLossBetList", function () {
                    appendSetFixtures('<input id="ApiUrl" value="api/MatchWinLossBetList/Outright" />');

                    BetList.initialize();

                    expect(BetListOption.columnModel[1].sortable).toBe(true);
                    expect(BetListOption.columnModel[2].sortable).toBe(true);
                });
            });

            describe("gridComplete(grid)", function () {
                beforeEach(function () {
                    spyOn(jqGridAdditional, "addSortableHeader");
                    spyOn(jqGridAdditional, "gridComplete");
                    spyOn(jqGridAdditional, "updateGridHeight");

                    appendSetFixtures('<table class="ui-jqgrid-ftable" id="grid"></tbody>');

                    var grid = $("#grid");
                    grid[0].p = {
                        colModel: [{
                            name: 'Index',
                            index: 'Index',
                            width: 50,
                            align: 'center',
                            sortable: false,
                            resizable: false
                        }, {
                            name: 'ExternalId',
                            index: 'ExternalId',
                            width: 100,
                            align: 'left',
                            sortable: true
                        }, {
                            name: 'SystemId',
                            index: 'UserName',
                            width: 120,
                            align: 'left',
                            sortable: false
                        }]
                    };
                    grid[0].grid = {
                        headers: [{
                            el: '<th id="grid_Index" role="columnheader" class="ui-state-default ui-th-column ui-th-ltr">'
                                + '<div id="jqgh_grid_Index" class="ui-jqgrid-sortable"></div>'
                                + '</th>'
                        }, {
                            el: '<th id="grid_ExternalId" role="columnheader" class="ui-state-default ui-th-column ui-th-ltr">'
                                + '<div id="jqgh_grid_ExternalId" class="ui-jqgrid-sortable"></div>'
                                + '</th>'
                        }, {
                            el: '<th id="grid_SystemId" role="columnheader" class="ui-state-default ui-th-column ui-th-ltr">'
                                + '<div id="jqgh_grid_SystemId" class="ui-jqgrid-sortable"></div>'
                                + '</th>'
                        }]
                    };
                });

                it("Add text for summary odd cell if _isShowSumStake is 1", function () {
                    appendSetFixtures('<div id="HiddenColumnList"></div> <input id="IsShowSumStake" value="1" />');
                    BetList.initialize();
                    $("#grid").html('<tbody><tr><td aria-describedby="BetListGrid_Odds" title="Odd"></td><td aria-describedby="BetListGrid_Stake" title="Stake"></td></tr>');
                    var idHeadersHavePointerCursor = [];
                    spyOn($.fn, "css").and.callFake(function (propertyNames) {
                        if (propertyNames.cursor) {
                            idHeadersHavePointerCursor.push($(this).attr("id"));
                        }
                    });

                    BetList.gridComplete("#grid");

                    expect($(".ui-jqgrid-ftable td[aria-describedby=BetListGrid_Odds]").html(), "GrandTotal");
                    expect($(".ui-jqgrid-ftable td[aria-describedby=BetListGrid_Stake]").html(), "");
                    expect(idHeadersHavePointerCursor.length).toEqual(1);
                    expect(idHeadersHavePointerCursor[0]).toEqual("jqgh_grid_ExternalId");
                });

                it("Add text for summary stake cell if _isShowSumStake is 0", function () {
                    appendSetFixtures('<div id="HiddenColumnList"></div> <input id="IsShowSumStake" value="0" />');
                    BetList.initialize();
                    $("#grid").html('<tbody><tr><td aria-describedby="BetListGrid_Odds" title="Odd"></td><td aria-describedby="BetListGrid_Stake" title="Stake"></td></tr>');

                    BetList.gridComplete("#grid");

                    expect($(".ui-jqgrid-ftable td[aria-describedby=BetListGrid_Odds]").html(), "");
                    expect($(".ui-jqgrid-ftable td[aria-describedby=BetListGrid_Stake]").html(), "GrandTotal");
                });
            });

            describe("getGridOption()", function () {
                beforeEach(function () {
                    spyOn(Site, "blockUI");
                    spyOn(jqGridAdditional, "setColumnVisible");
                    spyOn(Site, "getResources").and.callFake(function (resources) {
                        return resources;
                    });
                    appendSetFixtures('<input type="hidden" id="HiddenColumnList" value="externalid,systemid">');
                    appendSetFixtures('<input type="hidden" id="ShowSummary" value="0">');
                });

                it("BetEffective should not be hidden if report has BetEffective column", function () {
                    appendSetFixtures('<input type="hidden" id="HasBetEffectiveColumn" value="1">');

                    BetListOption.gridOption = BetList.getGridOption();

                    expect(jqGridAdditional.setColumnVisible).not.toHaveBeenCalled;
                });

                it("BetEffective should be hidden if report does not have BetEffective column", function () {
                    appendSetFixtures('<input type="hidden" id="HasBetEffectiveColumn" value="1">');

                    BetListOption.gridOption = BetList.getGridOption();

                    expect(jqGridAdditional.setColumnVisible).not.toHaveBeenCalled;
                });

                it("return Customer WL column name if allowwing Customer Commission", function () {
                    appendSetFixtures('<input type="hidden" id="HasCustComm" value="1">');
                    var winLossColumnIndex = BetListOption.gridOption.colNames.indexOf("WinLoss");
                    var expectedName = "@[WinLoss]/@[Comm]";

                    BetListOption.gridOption = BetList.getGridOption();

                    var columnName = BetListOption.gridOption.colNames[winLossColumnIndex];
                    expect(columnName, expectedName);
                });

                describe("onSortCol", function () {
                    beforeEach(function () {
                        spyOn(BetList, "loadDataPaging");
                        spyOn(BetList, "loadData");

                        appendSetFixtures('<input type="hidden" id="SortBy" value="Foo" />');
                        appendSetFixtures('<input type="hidden" id="SortOrder" value="asc" />');
                    });

                    afterEach(function () {
                        $("#SortBy").remove();
                        $("#SortOrder").remove();
                    });

                    it("return 'stop' if sorting is disable", function () {
                        window.disableBetListSorting = true;

                        expect(BetListOption.gridOption.onSortCol(1)).toEqual("stop");
                        expect(BetList.loadData).not.toHaveBeenCalled();
                        expect(BetList.loadDataPaging).not.toHaveBeenCalled();
                    });

                    it("change _SortOrder when _sortColumnName equals to index param", function () {
                        window.disableBetListSorting = false;
                        window.CorrectScoreBetList = true;
                        var sortBy = $("#SortBy").val();
                        var sortOrder = $("#SortOrder").val();

                        BetListOption.gridOption.onSortCol(sortBy);

                        expect($("#SortBy").val()).toEqual(sortBy);
                        expect($("#SortOrder").val()).not.toEqual(sortOrder);
                    });

                    it("change _sortColumnName when _sortColumnName does not equals to index param", function () {
                        window.disableBetListSorting = false;
                        window.CorrectScoreBetList = true;
                        var sortBy = $("#SortBy").val();
                        var sortOrder = $("#SortOrder").val();
                        var index = "WinLoss";
                        var expectedSortColumnName = "Winlost", expectedSortOrder = "desc";

                        BetListOption.gridOption.onSortCol(index);

                        expect($("#SortBy").val()).toEqual(expectedSortColumnName);
                        expect($("#SortOrder").val()).toEqual(expectedSortOrder);
                    });

                    it("does not change $('SortBy').val() and $('#SortOrder').val() when CorrectScoreBetList and CancelledBetsBetList are false", function () {
                        window.disableBetListSorting = false;
                        window.CorrectScoreBetList = false;
                        window.CancelledBetsBetList = false;
                        var sortBy = $("#SortBy").val();
                        var sortOrder = $("#SortOrder").val();
                        var index = "WinLoss";
                        var expectedSortColumnName = "Winlost", expectedSortOrder = "desc";

                        BetListOption.gridOption.onSortCol(index);

                        expect($("#SortBy").val()).not.toEqual(expectedSortColumnName);
                        expect($("#SortOrder").val()).not.toEqual(expectedSortOrder);
                    });

                    it("loadDataPaging should be called if paging is on server", function () {
                        window.disableBetListSorting = false;
                        window.serverPaging = true;

                        BetListOption.gridOption.onSortCol("Foo");

                        expect(BetList.loadDataPaging).toHaveBeenCalled();
                        expect(BetList.loadData).not.toHaveBeenCalled();
                    });

                    it("loadData should be called if paging is not on server", function () {
                        window.disableBetListSorting = false;
                        window.serverPaging = false;

                        BetListOption.gridOption.onSortCol("Foo");

                        expect(BetList.loadData).toHaveBeenCalled();
                        expect(BetList.loadDataPaging).not.toHaveBeenCalled();
                    });
                });
            });

            describe("loadDataPaging(url)", function () {
                var gridOption = { rowNum: 2 };

                beforeEach(function () {
                    appendSetFixtures('<div id="HiddenColumnList"></div>');
                    spyOn(Site, 'blockUI');
                    spyOn($.fn, 'jqGrid');
                    spyOn(BetList, "getGridOption").and.returnValue(gridOption);
                });

                afterEach(function () {
                    $("#HiddenColumnList").remove();
                });

                it("url should be set params for sorting", function () {
                    BetListOption.gridOption.onSortCol("Index");
                    var url = "api/loadDataPaging";
                    var expectedUrl = "api/loadDataPaging?IsServerPaging=true&SortBy=Index&SortOrder=desc";

                    BetList.loadDataPaging(url);

                    expect(gridOption.url).toEqual(transformQueryParamNameToLowerCase(expectedUrl));
                });

                it("should call .jqGrid('GridUnload')", function () {
                    BetList.loadDataPaging('');

                    expect($.fn.jqGrid).toHaveBeenCalledWith('GridUnload');
                });

                it("should call .jqGrid() with a selector '#BetListGrid'", function () {
                    BetList.loadDataPaging('');

                    expect($.fn.jqGrid.calls.mostRecent().object.selector).toBe('#BetListGrid');
                });

                describe("gridComplete", function () {
                    it("jqGridAdditional.setNoInfo should be called if no record returned", function () {
                        spyOn(jqGridAdditional, "setNoInfo");
                        spyOn($.fn, "getGridParam").and.returnValue(0);

                        gridOption.gridComplete();

                        expect(jqGridAdditional.setNoInfo).toHaveBeenCalled();
                    });

                    it("BetList.gridComplete should be called if have record", function () {
                        appendSetFixtures('<table id="gbox_BetListGrid" style="display:none"></table>');
                        spyOn($.fn, "getGridParam").and.returnValue(1);
                        spyOn(BetList, "gridComplete").and.returnValue(1);

                        gridOption.gridComplete();

                        expect(BetList.gridComplete).toHaveBeenCalled();
                    });

                    it("$('.ui-pg-selbox').val() should not be change if submiting is false", function () {
                        appendSetFixtures('<table id="gbox_BetListGrid" style="display:none"></table>');
                        appendSetFixtures('<input class="ui-pg-selbox" style="display:none" value="3"/>');
                        spyOn($.fn, "getGridParam").and.returnValue(1);
                        spyOn(BetList, "gridComplete").and.returnValue(1);

                        gridOption.gridComplete();

                        expect($(".ui-pg-selbox").val()).toEqual("3");
                    });
                });
            });

            describe("loadData(params, url)", function () {
                beforeEach(function () {
                    jasmine.Ajax.install();
                    spyOn(Site, "blockUI");
                    spyOn(Site, "unBlockUI");
                    spyOn(BetList, 'bindData');
                    spyOn(BetList, 'setLeagueTitle');
                });

                afterEach(function () {
                    jasmine.Ajax.uninstall();
                });

                it("bindData should not be called when response is null", function () {
                    jasmine.Ajax
                            .stubRequest("api/loadData")
                            .andReturn({
                                responseText: JSON.stringify(null)
                            });

                    BetList.loadData({}, "api/loadData");

                    expect(BetList.bindData).not.toHaveBeenCalled();
                    expect(BetList.setLeagueTitle).not.toHaveBeenCalled();
                });

                it("bindData should not be called when response is null", function () {
                    var result = {
                        LeagueName: "League Title",
                        Main: {}
                    };
                    jasmine.Ajax
                            .stubRequest("api/loadData")
                            .andReturn({
                                responseText: JSON.stringify(result)
                            });

                    BetList.loadData("{}", "api/loadData");

                    expect(BetList.bindData).toHaveBeenCalledWith(result);
                    expect(BetList.setLeagueTitle).toHaveBeenCalledWith(result.LeagueName);
                });
            });

            describe("bindData(url)", function () {
                beforeAll(function () {
                    spyOn(BetList, "getGridOption").and.returnValue({ rowNum: 2, rowList: [5] });
                    spyOn(jqGridAdditional, "formatDecimal").and.callFake(function (value) { return value; });
                    spyOn(jqGridAdditional, "formatLicenseeWl").and.callFake(function (value) { return value; });
                });

                it("Parameter is valid when calling jqgrid", function () {
                    var gridOption;
                    appendSetFixtures('<div id="HiddenColumnList"></div> <input id="IsShowSumStake" value="1" />');
                    var totalData = { SumWinLoss: 10, SumLicenseeWl: 15, SumBetEffective: 0, SumStake: 100 };
                    spyOn($.fn, 'jqGrid').and.callFake(function (param) { gridOption = param; });

                    BetList.initialize();
                    BetList.bindData({ Main: { Value: "Test Value" }, Total: totalData, IsShowTotal: true, LeagueName: "Test league" });

                    expect(gridOption.rowNum).toBe(5);
                    expect(gridOption.data).toEqual({ Value: "Test Value" });
                    expect(gridOption.userData).toEqual({ WinLoss: 10, LicenseeWL: 15, BetEffective: 0, Stake: 100 });
                    expect(gridOption.userDataOnFooter).toBe(true);
                    expect(gridOption.footerrow).toBe(true);
                });

                it("jqGridAdditional.setNoInfo() should be called if response is null", function () {
                    spyOn(jqGridAdditional, "setNoInfo");

                    BetList.bindData(null);

                    expect(jqGridAdditional.setNoInfo).toHaveBeenCalled();
                });

                it("jqGridAdditional.setNoInfo() should be called if main response is empty", function () {
                    spyOn(jqGridAdditional, "setNoInfo");

                    BetList.bindData({ Main: [] });

                    expect(jqGridAdditional.setNoInfo).toHaveBeenCalled();
                });

                it("Parameter is valid when calling jqgrid with showing userDataOnFooter", function () {
                    var gridOption;
                    appendSetFixtures('<div id="HiddenColumnList"></div> <input id="IsShowSumStake" value="0" />');
                    var totalData = { SumWinLoss: 10, SumLicenseeWl: 15, SumBetEffective: 0, SumStake: 100 };
                    spyOn($.fn, 'jqGrid').and.callFake(function (param) { gridOption = param; });

                    BetList.initialize();
                    BetList.bindData({ Main: { Value: "Test Value" }, Total: totalData, IsShowTotal: true, LeagueName: "Test league" });

                    expect(gridOption.rowNum).toBe(5);
                    expect(gridOption.data).toEqual({ Value: "Test Value" });
                    expect(gridOption.userData.Stake).toBe(undefined);
                    expect(gridOption.userDataOnFooter).toBe(true);
                    expect(gridOption.footerrow).toBe(true);
                });

                it("Parameter is valid when calling jqgrid without showing userDataOnFooter", function () {
                    var gridOption;
                    appendSetFixtures('<div id="HiddenColumnList"></div> <input id="IsShowSumStake" value="0" />');
                    var totalData = { SumWinLoss: 10, SumLicenseeWl: 15, SumBetEffective: 0, SumStake: 100 };
                    spyOn($.fn, 'jqGrid').and.callFake(function (param) { gridOption = param; });

                    BetList.initialize();
                    BetList.bindData({ Main: { Value: "Test Value" }, Total: totalData, IsShowTotal: false, LeagueName: "Test league" });

                    expect(gridOption.rowNum).toBe(5);
                    expect(gridOption.data).toEqual({ Value: "Test Value" });
                    expect(gridOption.userData.Stake).toBe(undefined);
                    expect(gridOption.userDataOnFooter).toBe(false);
                    expect(gridOption.footerrow).toBe(false);
                });
            });

            describe("getMixParlayResultUrl(refno, winlostdate, bettype)", function () {
                it("should return an url with correct parameters", function () {
                    var result = BetList.getMixParlayResultUrl(1, 2, 3);
                    expect(result).toBe('ResultReport/ResultPopup?refno=1&winlossdate=2&bettype=3&matchid=1');
                });
            });

            describe("getSportBookResultUrl(matchid, bettype, raceNo, sporttype, league, isOutright, betid)", function () {
                it("should return an url with correct parameters", function () {
                    var result = BetList.getSportBookResultUrl(1, 2, 3, 4, 5, 6, 7);
                    expect(result).toBe('ResultReport/ResultPopup?matchid=1&bettype=2&raceno=3&sporttype=4&isoutright=6&betid=7');
                });
            });

            describe("getMicrogamingResultUrl(gameid, custid)", function () {
                it("should return an url with correct parameters", function () {
                    var result = BetList.getMicrogamingResultUrl(1, 2);
                    expect(result).toBe('ResultReport/MicrogamingResultPopup?gameid=1&custid=2');
                });
            });

            describe("setLeagueTitle(leagueName)", function () {
                beforeEach(function () {
                    appendSetFixtures('<div class="league-name-title"></div>');
                });

                it("'.league-name-title' should contain resource 'BetList' as default", function () {
                    BetList.setLeagueTitle();
                    var result = BetList.getMicrogamingResultUrl(1, 2);
                    expect($('.league-name-title').html()).toBe('BetList - ');
                });

                it("'.league-name-title' should contain the text passed as paramter", function () {
                    BetList.setLeagueTitle('Hello');
                    var result = BetList.getMicrogamingResultUrl(1, 2);
                    expect($('.league-name-title').html()).toBe('Hello - ');
                });
            });

            describe("showMicrogamingResultPopup(title, gameid, custid)", function () {
                it(".dialog() should be called with correct parameters", function () {
                    spyOn(Popup, 'openPopupWithIframe');
                    BetList.showMicrogamingResultPopup('Hello', 1, 2);
                    var url = "ResultReport/MicrogamingResultPopup?gameid=1&custid=2";
                    expect(Popup.openPopupWithIframe).toHaveBeenCalledWith(url, { width: 750, height: 400, title: 'Hello', id: 'betlist-popup-result' });
                });
            });

            describe("viewResult(matchid, raceNo, bettype, sporttype, refno, username, winlostdate, refnoMixParlay, league, isOutright, betid, betcheck, custid)", function () {
                beforeEach(function () {
                    spyOn(Popup, 'openPopupWithIframe');
                });

                it("BetList.showMicrogamingResultPopup() should be called if sporttype is Microgaming", function () {
                    var SPORT_MICROGAMMING = 203;
                    spyOn(BetList, 'showMicrogamingResultPopup');
                    BetList.viewResult(null, null, null, SPORT_MICROGAMMING);

                    expect(BetList.showMicrogamingResultPopup).toHaveBeenCalled();
                });

                it("BetList.getMixParlayResultUrl() should be called if bettype is MixParlay", function () {
                    var BETTYPE_MIXPARLAY = 9;
                    spyOn(BetList, 'getMixParlayResultUrl');
                    BetList.viewResult(null, null, BETTYPE_MIXPARLAY);

                    expect(BetList.getMixParlayResultUrl).toHaveBeenCalled();
                });

                it("BetList.getSportBookResultUrl() should be called if bettype is NOT MixParlay", function () {
                    spyOn(BetList, 'getSportBookResultUrl');
                    BetList.viewResult(null, null);

                    expect(BetList.getSportBookResultUrl).toHaveBeenCalled();
                });

                it("$.fn.dialog() should be called if sporttype is NOT Microgaming", function () {
                    spyOn(BetList, 'getSportBookResultUrl');
                    BetList.viewResult(null, null);

                    expect(Popup.openPopupWithIframe).toHaveBeenCalled();
                });

                it("BetList.viewRngCasinoResult() should be called if sporttype is SPORT_RNG", function () {
                    spyOn(BetList, 'viewRngCasinoResult');
                    var SPORT_RNG_GAME = 208;

                    BetList.viewResult(null, null, null, SPORT_RNG_GAME);

                    expect(BetList.viewRngCasinoResult).toHaveBeenCalled();
                });

                it("BetList.viewVoidbridgeResult() should be called if sporttype is SPORT_VOIDBRIDGE", function () {
                    spyOn(BetList, 'viewVoidbridgeResult');
                    var SPORT_VOIDBRIDGE = 212;

                    BetList.viewResult(null, null, null, SPORT_VOIDBRIDGE);

                    expect(BetList.viewVoidbridgeResult).toHaveBeenCalled();
                });

                it("BetList.viewAllBetResult() should be called if sporttype is SPORT_ALLBET", function () {
                    spyOn(BetList, 'viewAllBetResult');
                    var SPORT_ALLBET = 211;

                    BetList.viewResult(null, null, null, SPORT_ALLBET);

                    expect(BetList.viewAllBetResult).toHaveBeenCalled();
                });

                it("BetList.viewGlobalGamingResult() should be called if sporttype is SPORT_GLOBALGAMING", function () {
                    spyOn(BetList, 'viewGlobalGamingResult');
                    var SPORT_GLOBALGAMING = 219;

                    BetList.viewResult(null, null, null, SPORT_GLOBALGAMING);

                    expect(BetList.viewGlobalGamingResult).toHaveBeenCalled();
                });
            });

            describe("showMixParlayDetail(transId, divEvent)", function () {
                describe("'divEvent' element is visible", function () {
                    var $div;
                    beforeEach(function () {
                        $div = $('<div><div></div></div>');
                        setFixtures($div);
                        BetList.showMixParlayDetail(0, $div);
                    });

                    afterEach(function () {
                        $div.remove();
                    });

                    it("'divEvent' element should be hide", function () {
                        expect($div.is(':visible')).toBe(false);
                    });

                    it("'divEvent' element shoult not contain any child.", function () {
                        expect($div.html()).toBe('');
                    });
                });

                describe("'divEvent' element is hidden", function () {
                    var $div, request;
                    beforeEach(function (done) {
                        $div = $('<div style="display:none; width: 10px; height: 20px"></div>');
                        setFixtures($div);
                        jasmine.Ajax.install();

                        jasmine.Ajax
                        .stubRequest(/api\/MixParlayBetList\/Detail\?transId=0/)
                        .andReturn({
                            'status': 200,
                            'contentType': 'application/json',
                            'responseText': '[{ "Choice": 1 }, { "Choice": 2 }, { "Choice": 3 }]'
                        });

                        BetList.showMixParlayDetail(0, $div);
                        request = jasmine.Ajax.requests.mostRecent();

                        setTimeout(function () {
                            done();
                        }, 100);
                    });

                    afterEach(function () {
                        jasmine.Ajax.uninstall();
                        $div.remove();
                    });

                    it("request url should be correct", function () {
                        expect(request.url).toMatch(/api\/MixParlayBetList\/Detail\?transId=0/);
                    });

                    it("'divEvent' element should be visible", function () {
                        expect($div.is(':visible')).toBe(true);
                    });

                    it("'divEvent' element should contain correct html", function () {
                        expect($div.html()).toBe('1<div class="line"></div>2<div class="line"></div>3');
                    });
                });
            });

            describe("showSystemParlayDetail(transId, refNo, betId, custId, betTeam, baseCurrency)", function () {
                it("Popup.openPopupWithIframe(url, options) should be called with correct parameters", function () {
                    spyOn(Popup, 'openPopupWithIframe');
                    BetList.showSystemParlayDetail(1, 0, 0, 0, 2, "eur");
                    var url = 'SystemParlayBetList/Detail?refno=0&betteam=2&basecurrency=eur';
                    expect(Popup.openPopupWithIframe).toHaveBeenCalledWith(url, { width: 775, height: 500, title: 2, id: 'betlist-popup-detail' });
                });

                it("Popup.openPopupWithIframe(url, options) should not be called if transId is zero", function () {
                    spyOn(Popup, 'openPopupWithIframe');
                    BetList.showSystemParlayDetail(0, 0, 0, 0, 2, "eur");
                    var url = 'SystemParlayBetList/Detail?refno=0&betteam=2&basecurrency=eur';
                    expect(Popup.openPopupWithIframe).not.toHaveBeenCalled();
                });
            });

            describe("showMicroGamingRnGBetsDetails(transId, winlostDate, custId, transType)", function () {
                it("Popup.openPopupWithIframe(url, options) should be called with correct parameters", function () {
                    spyOn(Popup, 'openPopupWithIframe');
                    spyOn($.fn, 'height').and.returnValue(300);
                    BetList.showMicroGamingRnGBetsDetails(0, new Date('2015/2/1'), 0, 0);
                    var url = 'MicroGamingRngBetList/Detail?transid=0&custid=0&transType=0';
                    var expectedUrl = transformQueryParamNameToLowerCase(url);
                    var title = 'Microgaming - DetailedBetlist - 02/01/2015';

                    expect(Popup.openPopupWithIframe).toHaveBeenCalledWith(expectedUrl, { width: 870, height: 250, title: title, id: 'betlist-popup-detail' });
                });
            });

            describe("showAGCasinoDetails(transId, winlostDate, custId, transType)", function () {
                it("Popup.openPopupWithIframe(url, options) should be called with correct parameters", function () {
                    spyOn(Popup, 'openPopupWithIframe');
                    spyOn($.fn, 'height').and.returnValue(300);
                    BetList.showAGCasinoDetails(0, new Date('2015/2/1'), 0, 0);
                    var url = 'AGCasinoBetList/Detail?transid=0&custid=0&transType=0';
                    var expectedUrl = transformQueryParamNameToLowerCase(url);
                    var title = 'AGCasino - DetailedBetlist - 02/01/2015';

                    expect(Popup.openPopupWithIframe).toHaveBeenCalledWith(expectedUrl, { width: 870, height: 250, title: title, id: 'betlist-popup-detail' });
                });
            });

            describe("ValidProducts()", function () {
                beforeEach(function () {
                    appendSetFixtures('<div id="ProductList"></div>');
                    spyOn(Popup, 'openPopup');
                });

                afterEach(function () {
                    $("#ProductList").remove();
                });

                it("should return false if #ProductList does not contain checked input", function () {
                    expect(BetList.ValidProducts()).toBe(false);
                });

                it("Popup.openPopupWithIframe(url, options) should be called if #ProductList does not contain checked input", function () {
                    BetList.ValidProducts();
                    var dialogForm = '<div title="LrfNotification"><p class="notification">AlertChooseProductMsg</p></div>';

                    expect(Popup.openPopup).toHaveBeenCalledWith(dialogForm, { width: 350, height: 120, title: 'LrfNotification', className: "popup-notification" });
                });

                it("should return true if #ProductList contain a checked input", function () {
                    $('#ProductList').append('<input type="checkbox" checked=true />');

                    expect(BetList.ValidProducts()).toBe(true);
                });
            });

            describe("viewRngCasinoResult(title, refno)", function () {
                beforeEach(function () {
                    jasmine.Ajax.install();
                    spyOn(Site, "blockUI");
                });

                afterEach(function () {
                    jasmine.Ajax.uninstall();
                });

                it("shows alert when response.Success is false", function () {
                    jasmine.Ajax
                            .stubRequest("ResultReport/ShowRngCasinoSingleWalletResult")
                            .andReturn({
                                responseText: JSON.stringify({
                                    Success: false
                                })
                            });
                    spyOn(window, "alert");

                    BetList.viewRngCasinoResult("", 0);

                    expect(window.alert).toHaveBeenCalledWith("ErrorProcessingMsg");
                });

                it("shows showResultPopUp when response.Success is true and result is not empty", function () {
                    jasmine.Ajax
                            .stubRequest("ResultReport/ShowRngCasinoSingleWalletResult")
                            .andReturn({
                                responseText: JSON.stringify({
                                    Success: true,
                                    Result: "Result"
                                })
                            });
                    spyOn(Popup, "openPopupWithIframe");

                    BetList.viewRngCasinoResult("Rng Casino Result", 0);

                    expect(Popup.openPopupWithIframe).toHaveBeenCalledWith("Result", { width: 800, height: 380, title: "Rng Casino Result", id: "betlist-popup-result" });
                });

                it("does not show showResultPopUp when response.Success is true and result is not empty", function () {
                    jasmine.Ajax
                            .stubRequest("ResultReport/ShowRngCasinoSingleWalletResult")
                            .andReturn({
                                responseText: JSON.stringify({
                                    Success: true,
                                    Result: ""
                                })
                            });
                    spyOn(Popup, "openPopupWithIframe");

                    BetList.viewRngCasinoResult("Rng Casino Result", 0);

                    expect(Popup.openPopupWithIframe).not.toHaveBeenCalled();
                });
            });

            describe("viewAllBetResult(trasdesc, betcheck, bettype, title)", function () {
                it("shows showResultPopUp with correct params", function () {
                    spyOn(Popup, "openPopupWithIframe");
                    var trasdesc = "trasdesc", betcheck = "betcheck", bettype = "bettype", title = "title";
                    var url = "ResultReport/AllBetResult?transdesc=" + "trasdesc" + "&betcheck=" + betcheck + "&bettype=" + bettype;

                    BetList.viewAllBetResult(trasdesc, betcheck, bettype, title);

                    expect(Popup.openPopupWithIframe).toHaveBeenCalledWith(url, { width: 800, height: 380, title: title, id: "betlist-popup-result" });
                });
            });

            describe("showRnGCasinoDetails(transId, winlostDate, custId, transType)", function () {
                it("shows showResultPopUp with correct params", function () {
                    var transId = 1, winlostDate = "01/05/2017", custId = 2, transType = 3;
                    var url = "RnGCasinoBetList/Detail?transId=" + transId + "&winlostDate=" + winlostDate + "&custId=" + custId + "&transType=" + transType;
                    var expectedUrl = transformQueryParamNameToLowerCase(url);
                    spyOn(Popup, "openPopupWithIframe");
                    spyOn($.fn, "height").and.returnValue(700);

                    BetList.showRnGCasinoDetails(transId, winlostDate, custId, transType);

                    expect(Popup.openPopupWithIframe).toHaveBeenCalledWith(expectedUrl, { width: 890, height: 600, title: "ProductCS - DetailedBetlist - 01/05/2017", id: "betlist-popup-detail" });
                });
            });

            describe("showVoidBridgeDetails(transId, winlostDate, custId, transType)", function () {
                it("shows showResultPopUp with correct params", function () {
                    var transId = 1, winlostDate = "01/05/2017", custId = 2, transType = 3;
                    var url = "VoidbridgeBetList/Detail?transId=" + transId + "&winlostDate=" + winlostDate + "&custId=" + custId + "&transType=" + transType;
                    var expectedUrl = transformQueryParamNameToLowerCase(url);
                    spyOn(Popup, "openPopupWithIframe");
                    spyOn($.fn, "height").and.returnValue(700);

                    BetList.showVoidBridgeDetails(transId, winlostDate, custId, transType);

                    expect(Popup.openPopupWithIframe).toHaveBeenCalledWith(expectedUrl, { width: 890, height: 600, title: "ProductVB - DetailedBetlist - 01/05/2017", id: "betlist-popup-detail" });
                });
            });

            describe("showAllBetDetails(transId, winlostDate, custId, transType)", function () {
                it("shows showResultPopUp with correct params", function () {
                    var transId = 1, winlostDate = "01/05/2017", custId = 2, transType = 3;
                    var url = "AllBetBetList/Detail?transid=" + transId + "&custid=" + custId + "&transType=" + transType + "&date=" + winlostDate;
                    var expectedUrl = transformQueryParamNameToLowerCase(url);
                    spyOn(Popup, "openPopupWithIframe");
                    spyOn($.fn, "height").and.returnValue(700);

                    BetList.showAllBetDetails(transId, winlostDate, custId, transType);

                    expect(Popup.openPopupWithIframe).toHaveBeenCalledWith(expectedUrl, { width: 870, height: 600, title: "ProductAB - DetailedBetlist - 01/05/2017", id: "betlist-popup-detail" });
                });
            });

            describe("viewCashOutHistory(transId, refNo, betType, betdaqId)", function () {
                it("shows showResultPopUp with correct params", function () {
                    var refNo = 1, transId = 2, betType = 1, betdaqId = 0;
                    var url = "CashOutBetList/Detail?transId=" + transId + "&refNo=" + refNo + "&betType=" + betType + "&betdaqId=" + betdaqId + "&BaseCurrency=" + "usd" + "&isCashout=true";
                    var expectedUrl = transformQueryParamNameToLowerCase(url);
                    spyOn(Popup, "openPopupWithIframe");
                    spyOn($.fn, "height").and.returnValue(700);
                    appendSetFixtures('<input id="BaseCurrency" value="usd" />');

                    BetList.viewCashOutHistory(transId, refNo, betType, betdaqId);

                    expect(Popup.openPopupWithIframe).toHaveBeenCalledWith(expectedUrl, { width: 890, height: 600, title: "CashOutDetails", id: "betlist-popup-detail" });
                });
            });

            describe("viewVoidbridgeResult(custId, gamePlayId, title)", function () {
                it("shows showResultPopUp with correct params if result is not empty", function () {
                    var result = "viewVoidbridgeResult";
                    spyOn($, "get").and.callFake(function (url, success) {
                        success(result);
                    });
                    var title = "Void bridge Result";
                    spyOn(Popup, "openPopupWithIframe");

                    BetList.viewVoidbridgeResult(0, 0, title);

                    expect(Popup.openPopupWithIframe).toHaveBeenCalledWith(result, { width: 800, height: 380, title: title, id: 'betlist-popup-result' });
                });

                it("does not show showResultPopUp with correct params if result is empty", function () {
                    var result = "";
                    spyOn($, "get").and.callFake(function (url, success) {
                        success(result);
                    });
                    var title = "Void bridge Result";
                    spyOn(Popup, "openPopupWithIframe");

                    BetList.viewVoidbridgeResult(0, 0, title);

                    expect(Popup.openPopupWithIframe).not.toHaveBeenCalled();
                });
            });

            describe("viewGlobalGamingResult(custId, gameId, title)", function () {
                it("shows showResultPopUp with correct params if result is not empty", function () {
                    var result = "viewGlobalGamingResult";
                    spyOn($, "get").and.callFake(function (url, success) {
                        success(result);
                    });
                    var title = "Global Gaming Result";
                    spyOn(Popup, "openPopupWithIframe");

                    BetList.viewGlobalGamingResult(0, 0, title);

                    expect(Popup.openPopupWithIframe).toHaveBeenCalledWith(result, { width: 800, height: 380, title: title, id: 'betlist-popup-result' });
                });

                it("does not show showResultPopUp with correct params if result is empty", function () {
                    var result = "";
                    spyOn($, "get").and.callFake(function (url, success) {
                        success(result);
                    });
                    var title = "Void bridge Result";
                    spyOn(Popup, "openPopupWithIframe");

                    BetList.viewGlobalGamingResult(0, 0, title);

                    expect(Popup.openPopupWithIframe).not.toHaveBeenCalled();
                });
            });

            describe("showNewParlayDetail(transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam)", function () {
                it("Combination Offer Name should be shown if divEvent is not hidden", function () {
                    var transId = 1, transIndex = 2;
                    setFixtures('<div id="divEvent_12">');
                    appendSetFixtures('<div id="combinationOfferName_12" style="display:none">');

                    BetList.showNewParlayDetail(transId, transIndex);

                    expect($("#divEvent_12").is(":hidden")).toEqual(true);
                    expect($("#combinationOfferName_12").is(":hidden")).toEqual(false);
                });

                it("Combination Offer Name should be shown if divEvent is not hidden but reponse ajax data is empty", function () {
                    var transId = 1, transIndex = 2;
                    setFixtures('<div id="divEvent_12" style="display:none">');
                    appendSetFixtures('<div id="combinationOfferName_12">');
                    spyOn(QueryString, "setParam");
                    spyOn($, "ajax").and.callFake(function (option) {
                        option.success({ Data: [] });
                    });

                    BetList.showNewParlayDetail(transId, transIndex);

                    expect($("#divEvent_12").is(":hidden")).toEqual(true);
                    expect($("#combinationOfferName_12").is(":hidden")).toEqual(false);
                });

                it("divEvent should be shown correct content if divEvent is not hidden and reponse ajax data is not empty", function () {
                    var transId = 1, transIndex = 2;
                    setFixtures('<div id="divEvent_12" style="display:none">');
                    appendSetFixtures('<div id="combinationOfferName_12">');
                    spyOn(QueryString, "setParam");
                    spyOn($, "ajax").and.callFake(function (option) {
                        option.success({
                            Data: [{ Choice: "Choice1" }, { Choice: "Choice2" }],
                            Texts: ["text1", "text2"]
                        });
                    });
                    var expectedContent = 'text1text2Choice1<div class="line"></div>Choice2';

                    BetList.showNewParlayDetail(transId, transIndex);

                    expect($("#divEvent_12").is(":hidden")).toEqual(false);
                    expect($("#divEvent_12").html()).toEqual(expectedContent);
                    expect($("#combinationOfferName_12").is(":hidden")).toEqual(true);
                });
            });

            describe("showColossusBetsDetails(refNo, winlostDate, transId, custId, betCheck)", function () {
                it("shows showResultPopUp with correct params", function () {
                    spyOn(Popup, "openPopupWithIframe");
                    spyOn($, "parseJSON").and.returnValue({
                        BaseCurrency : "usd"
                    });
                    spyOn($.fn, "height").and.returnValue(700);
                    var refNo = "refNo", winlostDate = "winlostDate", transId = "transId", custId = "custId", betCheck = "betCheck";
                    var url = "ColossusBetBetList/Detail?refNo=" + refNo +
                        "&date=" + winlostDate +
                        "&transid=" + transId +
                        "&custid=" + custId +
                        "&betCheck=" + betCheck +
                        "&BaseCurrency=usd";
                    var expectedUrl = transformQueryParamNameToLowerCase(url);
                    var title = "TicketDetailOf";

                    BetList.showColossusBetsDetails(refNo, winlostDate, transId, custId, betCheck);

                    expect(Popup.openPopupWithIframe).toHaveBeenCalledWith(expectedUrl, { width: 890, height: 600, title: title, id: "betlist-popup-detail" });
                });
            });

            describe("showGlobalGamingDetails(transId, winlostDate, custId, transType)", function () {
                it("shows showResultPopUp with correct params", function () {
                    spyOn(Popup, "openPopupWithIframe");
                    spyOn($, "parseJSON").and.returnValue({
                        BaseCurrency: "usd"
                    });
                    spyOn($.fn, "height").and.returnValue(700);
                    var transId = "transId", winlostDate = "06/01/2017", custId = "custId", transType = "transType";
                    var url = "GlobalGamingBetList/Detail?transId=" + transId +
                        "&winlostDate=" + winlostDate +
                        "&custId=" + custId +
                        "&transType=" + transType +
                        "&BaseCurrency=usd";
                    var expectedUrl = transformQueryParamNameToLowerCase(url);
                    var title = "ProductFW - DetailedBetlist - 06/01/2017";

                    BetList.showGlobalGamingDetails(transId, winlostDate, custId, transType);

                    expect(Popup.openPopupWithIframe).toHaveBeenCalledWith(expectedUrl, { width: 890, height: 600, title: title, id: "betlist-popup-detail" });
                });
            });

            describe("registerBetListGlobalFunctions()", function () {
                describe("showMP(transId)", function () {
                    it("showMixParlayDetail should be called with correct params", function () {
                        var transId = 1;
                        setFixtures('<div id="divEvent_1">');
                        spyOn(BetList, "showMixParlayDetail");

                        BetList.registerBetListGlobalFunctions();
                        window.showMP(transId);

                        expect(BetList.showMixParlayDetail).toHaveBeenCalledWith(transId, $("#divEvent_1"));
                    });
                });

                describe("showCombMP(transId)", function () {
                    it("showMixParlayDetail should be called with correct params", function () {
                        var transId = 1, index = 2;
                        setFixtures('<div id="divEvent_12">');
                        spyOn(BetList, "showMixParlayDetail");

                        BetList.registerBetListGlobalFunctions();
                        window.showCombMP(transId, index);

                        expect(BetList.showMixParlayDetail).toHaveBeenCalledWith(transId, $("#divEvent_12"));
                    });
                });
            });
        });
    });