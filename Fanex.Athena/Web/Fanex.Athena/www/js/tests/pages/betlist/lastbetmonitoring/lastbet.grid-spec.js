define(['jquery', 'common/site', 'pages/betlist/betlist', 'pages/betlist/lastbetmonitoring/lastbet.grid', 'lib/jquery.multiselect'],
    function ($, Site, BetList, LastBetGrid) {
        describe("LastBetGrid", function () {
            var initialHtml = '<table id="last-bets-monitoring"><tbody></tbody></table> <input id="last-bet-monitor-base-currency" value="eur" />';
            beforeAll(function () {
                spyOn(Site, "getSingleResource").and.callFake(function (resource) { return resource; });
                spyOn(Site, "resolveClientUrl").and.callFake(function (url) { return url; });
            });

            describe("columns", function () {
                it("default value should be correct", function () {
                    var exspectedResult =
                        [{ Name: "Platform", IsShow: 1 },
                         { Name: "ExternalId", IsShow: 1 },
                         { Name: "SystemId", IsShow: 1 },
                         { Name: "TransTime", IsShow: 1 },
                         { Name: "Choice", IsShow: 1 },
                         { Name: "Odds", IsShow: 1 },
                         { Name: "Stake", IsShow: 1 },
                         { Name: "Status", IsShow: 1 }];
                    expect(LastBetGrid.columns).toEqual(exspectedResult);
                });
            });

            describe("initialize()", function () {
                beforeEach(function () {
                    spyOn(Site, "clone");
                    spyOn(BetList, "registerBetListGlobalFunctions");

                    setFixtures(initialHtml);
                    LastBetGrid.initialize();
                });

                it("Site.clone should be called with correct parameter", function () {
                    var params = {
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
                        TransId: "0",
                        BetFrom: 0,
                        BasedCurrency: "eur"
                    };

                    expect(Site.clone).toHaveBeenCalledWith(params);
                });

                it("BetList.registerBetListGlobalFunctions should be called", function () {
                    expect(BetList.registerBetListGlobalFunctions).toHaveBeenCalled();
                });
            });

            describe("getMaxStake()", function () {
                it("return correct value", function () {
                    var result = LastBetGrid.getMaxStake();
                    expect(result).toBe(999999999);
                });
            });

            describe("Submit(freezeStatus)", function () {
                beforeEach(function () {
                    spyOn(LastBetGrid, "stopMonitor");
                    spyOn(LastBetGrid, "tryUpdateStake");
                    spyOn(LastBetGrid, "startMonitor");
                });

                it("LastBetGrid.stopMonitor and LastBetGrid.tryUpdateStake should be called", function () {
                    LastBetGrid.Submit(1);

                    expect(LastBetGrid.stopMonitor).toHaveBeenCalled();
                    expect(LastBetGrid.tryUpdateStake).toHaveBeenCalled();
                });

                it("LastBetGrid.startMonitor should be called when freezeStatus=1", function () {
                    LastBetGrid.Submit(1);

                    expect(LastBetGrid.startMonitor).toHaveBeenCalled();
                });
            });

            describe("reloadData", function () {
                var ajaxOption;
                function initAjaxCall() {
                    spyOn(LastBetGrid, "isValidParam").and.returnValues(true);
                    spyOn(LastBetGrid, "clearContent");
                    LastBetGrid.initialize();
                    LastBetGrid.reloadData();
                }

                beforeEach(function () {
                    spyOn(LastBetGrid, "doMonitor");
                    spyOn($, "ajax").and.callFake(function (option) { ajaxOption = option; });
                });

                afterEach(function () {
                    ajaxOption = null;
                });

                it("nothing happen if param is invalid", function () {
                    setFixtures('<input id="stake-from" value="10" /> <input id="stake-to" value="1" />');
                    spyOn($.fn, "multiSelect").and.callFake(function (methodName) { return [1]; });
                    LastBetGrid.Submit(0);

                    LastBetGrid.reloadData();

                    expect($.ajax).not.toHaveBeenCalled();
                });

                it("LastBetGrid.clearContent should be called", function () {
                    setFixtures('<input id="stake-from" value="" /> <input id="stake-to" value="" />');
                    spyOn(LastBetGrid, "clearContent");
                    spyOn($.fn, "multiSelect").and.callFake(function (methodName) { return [1]; });
                    LastBetGrid.Submit(0);

                    LastBetGrid.reloadData();

                    expect(LastBetGrid.clearContent).toHaveBeenCalled();
                });

                it("$.ajax should be called with correct options", function () {
                    setFixtures(initialHtml);
                    var params = {
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
                        StakeFrom: 0,
                        StakeTo: 999999999,
                        TransId: "0",
                        BetFrom: 0,
                        BasedCurrency: "eur"
                    };
                    initAjaxCall();

                    LastBetGrid.reloadData();

                    expect($.ajax).toHaveBeenCalled();
                    expect(ajaxOption.type).toBe("POST");
                    expect(ajaxOption.url).toBe("api/LastBetMonitoringBetList/Index");
                    expect(ajaxOption.data).toBe(JSON.stringify(params));
                    expect(ajaxOption.contentType).toBe("application/json; charset=utf-8");
                    expect(ajaxOption.timeout).toBe(10000);
                });

                it("beforeSend function call - '#monitorLoader' is visible", function () {
                    setFixtures('<div id="monitorLoader"></div>');
                    initAjaxCall();

                    LastBetGrid.reloadData();
                    ajaxOption.beforeSend();
                    var result = $("#monitorLoader").css("visibility");

                    expect(result).toBe("visible");
                });

                it("complete function call - '#monitorLoader' is hidden", function () {
                    setFixtures('<div id="monitorLoader"></div>');
                    initAjaxCall();

                    LastBetGrid.reloadData();
                    ajaxOption.complete();
                    var result = $("#monitorLoader").css("visibility");

                    expect(result).toBe("hidden");
                });

                it("success function call - bindData and doMonitor function should be called", function () {
                    spyOn(LastBetGrid, "bindData");
                    initAjaxCall();

                    LastBetGrid.reloadData();
                    ajaxOption.success({ NewRow: false, Rows: [], LastTransId: 0 });

                    expect(LastBetGrid.bindData).toHaveBeenCalled();
                    expect(LastBetGrid.doMonitor).toHaveBeenCalled();
                });
            });
        });
    });