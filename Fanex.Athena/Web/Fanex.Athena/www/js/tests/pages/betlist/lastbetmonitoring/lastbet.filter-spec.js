define(['jquery', 'common/site', "components/pluginhelper/multiselect",
    "pages/betlist/lastbetmonitoring/lastbet.grid", "pages/betlist/lastbetmonitoring/lastbet.filter", "lib/jquery.multiselect", 'extensions/jquery.extension'],
    function ($, Site, MultiSelect, LastBetGrid, LastBetFilter) {
        describe("LastBetFilter", function () {
            beforeAll(function () {
                spyOn(Site, "getSingleResource").and.callFake(function (resource) { return resource; });
                spyOn(Site, "resolveClientUrl").and.callFake(function (url) { return url; });
            });

            describe("initialize", function () {
                beforeAll(function () {
                    spyOn(Site, "initialize");
                    spyOn(Site, "addUserGuide");

                    spyOn(LastBetFilter, "initSports");
                    spyOn(LastBetFilter, "initLeague");
                    spyOn(LastBetFilter, "initMatch");
                    spyOn(LastBetFilter, "initBetType");
                    spyOn(LastBetFilter, "initLiveStatus");
                    spyOn(LastBetFilter, "initOddsType");
                    spyOn(LastBetFilter, "initSpecialLevel");
                    spyOn(LastBetFilter, "initCurrency");
                    spyOn(LastBetFilter, "initTimeZone");
                    spyOn(LastBetFilter, "initBasedCurrency");
                    spyOn(LastBetFilter, "initStake");
                    spyOn(LastBetFilter, "initPageSizeControl");
                    spyOn(LastBetFilter, "initFreezeButton");
                    spyOn(LastBetFilter, "initSubmit");
                    spyOn(LastBetFilter, "initPlatForm");
                    spyOn(LastBetFilter, "initColumns");

                    spyOn($.fn, "selectList");

                    LastBetFilter.initialize();
                });

                it("Site.initialize() should be called", function () {
                    expect(Site.initialize).toHaveBeenCalled();
                });

                it("Site.addUserGuide() should be called with correct parameter", function () {
                    expect(Site.addUserGuide).toHaveBeenCalledWith({
                        resourceName: "LastBetsMonitoring",
                        title: "LastBetsMonitoring"
                    });
                });

                it("LastBetFilter.initSports() should be called", function () {
                    expect(LastBetFilter.initSports).toHaveBeenCalled();
                });

                it("LastBetFilter.initLeague() should be called", function () {
                    expect(LastBetFilter.initLeague).toHaveBeenCalled();
                });

                it("LastBetFilter.initMatch() should be called", function () {
                    expect(LastBetFilter.initMatch).toHaveBeenCalled();
                });

                it("LastBetFilter.initBetType() should be called", function () {
                    expect(LastBetFilter.initBetType).toHaveBeenCalled();
                });

                it("LastBetFilter.initLiveStatus() should be called", function () {
                    expect(LastBetFilter.initLiveStatus).toHaveBeenCalled();
                });

                it("LastBetFilter.initOddsType() should be called", function () {
                    expect(LastBetFilter.initOddsType).toHaveBeenCalled();
                });

                it("LastBetFilter.initSpecialLevel() should be called", function () {
                    expect(LastBetFilter.initSpecialLevel).toHaveBeenCalled();
                });

                it("LastBetFilter.initCurrency() should be called", function () {
                    expect(LastBetFilter.initCurrency).toHaveBeenCalled();
                });

                it("LastBetFilter.initTimeZone() should be called", function () {
                    expect(LastBetFilter.initTimeZone).toHaveBeenCalled();
                });

                it("LastBetFilter.initBasedCurrency() should be called", function () {
                    expect(LastBetFilter.initBasedCurrency).toHaveBeenCalled();
                });

                it("LastBetFilter.initStake() should be called", function () {
                    expect(LastBetFilter.initStake).toHaveBeenCalled();
                });

                it("LastBetFilter.initPageSizeControl() should be called", function () {
                    expect(LastBetFilter.initPageSizeControl).toHaveBeenCalled();
                });

                it("LastBetFilter.initFreezeButton() should be called", function () {
                    expect(LastBetFilter.initFreezeButton).toHaveBeenCalled();
                });

                it("LastBetFilter.initSubmit() should be called", function () {
                    expect(LastBetFilter.initSubmit).toHaveBeenCalled();
                });

                it("LastBetFilter.initPlatForm() should be called", function () {
                    expect(LastBetFilter.initPlatForm).toHaveBeenCalled();
                });

                it("LastBetFilter.initColumns() should be called", function () {
                    expect(LastBetFilter.initColumns).toHaveBeenCalled();
                });

                it("selectList jquery function should be called with correct parameter", function () {
                    expect($.fn.selectList).toHaveBeenCalledWith("disabled", true);
                });

                it("selectList jquery function should be called twice", function () {
                    expect($.fn.selectList.calls.count()).toBe(2);
                });
            });

            describe("initSports()", function () {
                var ajaxOption;
                beforeEach(function () {
                    setFixtures('<input id="sports" value="0" /> <input id="match" /><input id="league" />');
                    spyOn($, "ajax").and.callFake(function (option) { ajaxOption = option; });
                    spyOn(LastBetFilter, "changeFilterParam");
                    spyOn(LastBetGrid, "updateParam");
                    spyOn($.fn, "selectList");
                    LastBetFilter.initSports();
                });

                describe("SportId value is 0 (All) or 999 (mixParlaySportId)", function () {
                    beforeEach(function () {
                        $("#sports").change();
                    });

                    it("LastBetFilter.changeFilterParam should be called with correct parameter when $('#sports') fired change event", function () {
                        expect(LastBetFilter.changeFilterParam).toHaveBeenCalledWith("number", "SportId", 0);
                    });

                    it("$.ajax should not be called when $('#sports') fired change event with sportId is 0 or 999", function () {
                        expect($.ajax).not.toHaveBeenCalled();
                    });

                    it("LastBetGrid.updateParam should be called twice when $('#sports') fired change event", function () {
                        expect(LastBetGrid.updateParam.calls.count()).toBe(2);
                    });

                    it("LastBetGrid.updateParam should be called with correct parameter when $('#sports') fired change event", function () {
                        var argForLeague = ["number", "LeagueId", ""];
                        var argForMatch = ["number", "MatchId", ""];

                        expect(LastBetGrid.updateParam.calls.argsFor(0)).toEqual(argForLeague);
                        expect(LastBetGrid.updateParam.calls.argsFor(1)).toEqual(argForMatch);
                    });

                    it("selectList jquery function should be called three times", function () {
                        expect($.fn.selectList.calls.count()).toBe(3);
                    });

                    it("selectList jquery function should be called by $leagueDropdown with correct parameter", function () {
                        expect($.fn.selectList).toHaveBeenCalledWith("disabled", true);
                    });

                    it("selectList jquery function should be called by $leagueDropdown and $matchDropdown with correct parameter", function () {
                        var argForUpdatedLeague = ["update", { isDisabled: true }];
                        var argForUpdatedMatch = ["update", { isDisabled: true }];

                        expect($.fn.selectList.calls.argsFor(1)).toEqual(argForUpdatedLeague);
                        expect($.fn.selectList.calls.argsFor(2)).toEqual(argForUpdatedMatch);
                    });

                    it("LastBetGrid.updateParam should be called twice with correct parameter", function () {
                        var argForUpdatedLeague = ["number", "LeagueId", ""];
                        var argForUpdatedMatch = ["number", "MatchId", ""];

                        expect(LastBetGrid.updateParam.calls.argsFor(0)).toEqual(argForUpdatedLeague);
                        expect(LastBetGrid.updateParam.calls.argsFor(1)).toEqual(argForUpdatedMatch);
                    });
                });

                describe("SportId value is not 0 (All) or 999 (mixParlaySportId)", function () {
                    beforeEach(function () {
                        $("#sports").val(1);
                        $("#sports").change();
                    });

                    afterEach(function () {
                        ajaxOption = null;
                    });

                    it("ajax function should be called with correct parameter", function () {
                        expect(ajaxOption.url).toBe("api/LastBetMonitoringBetList/GetRunningMatchList");
                        expect(ajaxOption.data).toBe("sportid=1");
                        expect(ajaxOption.dataType).toBe("json");
                        expect(ajaxOption.type).toBe("GET");
                        expect(ajaxOption.contentType).toBe("application/json; charset=utf-8");
                    });

                    it("call beforeSend function - Site.blockUI() is called", function () {
                        spyOn(Site, "blockUI");
                        ajaxOption.beforeSend();

                        expect(Site.blockUI).toHaveBeenCalled();
                    });

                    it("call complete function - Site.unBlockUI() is called", function () {
                        spyOn(Site, "unBlockUI");
                        ajaxOption.complete();

                        expect(Site.unBlockUI).toHaveBeenCalled();
                    });

                    it("call success function - append jquery function should be called with correct parameter", function () {
                        spyOn($.fn, "append");
                        var response = {
                            LeagueList: [{LeagueId: 1, LeagueName: "Soccer"}]
                        };

                        ajaxOption.success(response);
                        var expectedParam = "<option value='1'>Soccer</option>";

                        expect($.fn.append).toHaveBeenCalledWith(expectedParam);
                    });
                });
            });

            describe("initLeague()", function () {
                var ajaxOption;
                beforeEach(function () {
                    setFixtures('<input id="league" value="0" /> <select id="match"></select>');
                    spyOn(LastBetFilter, "changeFilterParam");                    
                    spyOn(LastBetGrid, "updateParam");
                    spyOn($.fn, "selectList");
                    LastBetFilter.initLeague();
                });

                describe("LeagueId value is 0 (All)", function () {
                    beforeEach(function () {
                        $("#league").change();
                    });

                    it("LastBetFilter.changeFilterParam should be called with correct parameter when $('#sports') fired change event", function () {
                        expect(LastBetFilter.changeFilterParam).toHaveBeenCalledWith("number", "LeagueId", 0);
                    });

                    it("#match should has correct html content", function () {
                        var expectedResult = '<option value="0">All</option>';

                        var result = $("#match").html();

                        expect(result).toBe(expectedResult);
                    });

                    it("selectList jquery should be called with disabled function by $matchDropdown with correct parameter", function () {
                        expect($.fn.selectList).toHaveBeenCalledWith("disabled", true);
                    });

                    it("selectList jquery should be called with update by $matchDropdown with correct parameter", function () {
                        var argForUpdatedMatch = ["update", { isDisabled: true }];

                        expect($.fn.selectList.calls.argsFor(1)).toEqual(argForUpdatedMatch);
                    });

                    it("LastBetGrid.updateParam should be called twice with correct parameter", function () {
                        expect(LastBetGrid.updateParam).toHaveBeenCalledWith("number", "MatchId", "0");
                    });
                });

                describe("LeagueId value is not 0 (All)", function () {
                    var ajaxOption;
                    beforeEach(function () {
                        appendSetFixtures('<input id="sports" value="1" />');
                        LastBetFilter.initSports();
                        spyOn($, "ajax").and.callFake(function (option) { ajaxOption = option; });
                        $("#sports").change();
                        
                        $("#league").val(1);
                    });

                    it("$matchDropdown should has correct html when it can be showed", function () {
                        var response = {
                            LeagueList: [{ LeagueId: 1, LeagueName: "Soccer", IsOutRight: false }],
                            MatchList: [{ LeagueId: 1, MatchId: 1, MatchName: "MU-Arsenal" }]
                        };

                        ajaxOption.success(response);
                        $("#league").change();
                        var expectedResult = '<option value="0">All</option><option value="1">MU-Arsenal</option>';
                        var result = $("#match").html();

                        expect(result).toEqual(expectedResult);
                    });

                    it("$matchDropdown should has correct html when it can not be showed", function () {
                        var response = {
                            LeagueList: [{ LeagueId: 2, LeagueName: "Soccer", IsOutRight: true }],
                            MatchList: [{ LeagueId: 1, MatchId: 1, MatchName: "MU-Arsenal" }]
                        };

                        ajaxOption.success(response);
                        $("#league").change();
                        var expectedResult = '<option value="0">All</option>';
                        var result = $("#match").html();

                        expect(result).toEqual(expectedResult);
                    });
                });
            });
        });
    });