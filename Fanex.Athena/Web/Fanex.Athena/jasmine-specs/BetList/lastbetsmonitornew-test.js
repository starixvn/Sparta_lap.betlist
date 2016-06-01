describe("MonitorBetList", function () {
    beforeEach(function () {
        monitor = new MonitorBetList();
    });

    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(Site, "addUserGuide");
            spyOn(monitor, "updateParams");
            spyOn(Site, "clone");

            monitor.initialize();
        });

        it("Site.addUserGuide() should be called with correct parameters", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "LastBetsMonitoring", title: "LastBetsMonitoring" });
        });

        it("MonitorBetList.updateParams() should be called", function () {
            expect(monitor.updateParams).toHaveBeenCalled();
        });

        it("Site.clone() should be called", function () {
            expect(Site.clone).toHaveBeenCalled();
        });
    });

    describe("numberParam(name, value)", function () {
        beforeEach(function () {
            monitor.testNumberValue = null;
        });

        it("MonitorBetList.testNumberValue should be 0 when 'value' param is undefined", function () {
            monitor.numberParam('testNumberValue');
            expect(monitor.testNumberValue).toBe(0);
        });

        it("MonitorBetList.testNumberValue should be 0 when 'value' param is null", function () {
            monitor.numberParam('testNumberValue', null);
            expect(monitor.testNumberValue).toBe(0);
        });

        it("MonitorBetList.testNumberValue should be 0 when 'value' param is empty", function () {
            monitor.numberParam('testNumberValue', '');
            expect(monitor.testNumberValue).toBe(0);
        });

        it("MonitorBetList.testNumberValue should be 1 when 'value' param is 1", function () {
            monitor.numberParam('testNumberValue', 1);
            expect(monitor.testNumberValue).toBe(1);
        });
    });

    describe("stringParam(name, value)", function () {
        var emptyString = "";
        beforeEach(function () {
            monitor.testStringValue = null;
        });

        it("MonitorBetList.testStringValue should be empty when 'value' param is undefined", function () {
            monitor.stringParam('testStringValue');
            expect(monitor.testStringValue).toBe(emptyString);
        });

        it("MonitorBetList.testStringValue should be empty when 'value' param is null", function () {
            monitor.stringParam('testStringValue', null);
            expect(monitor.testStringValue).toBe(emptyString);
        });

        it("MonitorBetList.testStringValue should be empty when 'value' param is empty", function () {
            monitor.stringParam('testStringValue', '');
            expect(monitor.testStringValue).toBe(emptyString);
        });

        it("MonitorBetList.testStringValue should be 'abc' when 'value' param is 'abc'", function () {
            monitor.stringParam('testStringValue', 'abc');
            expect(monitor.testStringValue).toBe('abc');
        });

        it("MonitorBetList.testStringValue should be 'abc' when 'value' param is '  abc   '", function () {
            monitor.stringParam('testStringValue', '  abc   ');
            expect(monitor.testStringValue).toBe('abc');
        });
    });

    describe("tryUpdateStack()", function () {
        beforeEach(function () {
            monitor.maxStake = 9999;
        });

        describe("$('#stake-from').val() and $('#stake-to').val() is empty", function () {
            beforeEach(function () {
                affix("input#stake-from");
                affix("input#stake-to");
                monitor.tryUpdateStack();
            });

            it("$this.params.StakeFrom should be 0", function () {
                expect(monitor.params.StakeFrom).toBe(0);
            });

            it("$this.params.StakeFrom should be equal to monitor.maxStake", function () {
                expect(monitor.params.StakeTo).toBe(monitor.maxStake);
            });
        });

        describe("$('#stake-from').val() is 1 and $('#stake-to').val() is 2", function () {
            beforeEach(function () {
                affix("input#stake-from[value=1]");
                affix("input#stake-to[value=2]");
                monitor.tryUpdateStack();
            });

            it("$this.params.StakeFrom should be 1", function () {
                expect(monitor.params.StakeFrom).toBe(1);
            });

            it("$this.params.StakeFrom should be 2", function () {
                expect(monitor.params.StakeTo).toBe(2);
            });
        });
    });

    describe("Submit(freeStatus)", function () {
        beforeEach(function () {
            spyOn(monitor, "tryUpdateStack");
            spyOn(monitor, "startMonitor");
            spyOn(monitor, "stopMonitor");
            spyOn(Site, "clone");
        });
        it("MonitorBetList.startMonitor() should be called with freeStatus is 1", function () {
            monitor.Submit(1);
            expect(monitor.startMonitor.calls.count()).toBe(1);
        });

        it("MonitorBetList.stopMonitor() should be called once with freeStatus is 1", function () {
            monitor.Submit(1);
            expect(monitor.stopMonitor).toHaveBeenCalled();
        });

        it("MonitorBetList.tryUpdateStack() should be called", function () {
            monitor.Submit(1);
            expect(monitor.tryUpdateStack).toHaveBeenCalled();
        });

        it("Site.clone(obj) should be called", function () {
            monitor.Submit(1);
            expect(Site.clone).toHaveBeenCalled();
        });

        it("MonitorBetList.stopMonitor() should be called twice with freeStatus is 0", function () {
            monitor.Submit(0);
            expect(monitor.stopMonitor.calls.count()).toBe(2);
        });
    });

    describe("reloadData()", function () {
        beforeEach(function () {
            affix("#div-loading");
            affix(".no-info-row");
        });

        describe("ajax not be called yet", function () {
            it("MonitorBetList.doMonitor() should be called when 'monitor.isLoading' is true", function () {
                monitor.isLoading = true;
                spyOn(monitor, "doMonitor");
                monitor.reloadData();
                expect(monitor.doMonitor).toHaveBeenCalled();
            });

            it("$.ajax() should not be called when 'isValidParam()' function return false", function () {
                spyOn(monitor, "isValidParam").and.returnValue(false);
                spyOn($, "ajax");
                monitor.reloadData();
                expect($.ajax).not.toHaveBeenCalled();
            });
        });

        describe("ajax has been called", function () {
            var request;
            beforeEach(function (done) {
                jasmine.Ajax.install();
                jasmine.Ajax.stubRequest('api/LastBetMonitoringBetList/LastMonitorBetListNew')
                        .andReturn({
                            status: 200,
                            contentType: 'application/json',
                            responseText: '{}'
                        });

                spyOn(monitor, "isValidParam").and.returnValue(true);
                spyOn(monitor, "bindNoInfoRow");

                spyOn(monitor, "bindData");

                monitor.reloadData();
                request = jasmine.Ajax.requests.mostRecent();
                setTimeout(done, 10);
            });
            afterEach(function () {
                jasmine.Ajax.uninstall();
            });

            it("url of ajax should be 'api/LastBetMonitoringBetList/LastMonitorBetListNew'", function () {
                expect(request.url).toEqual("api/LastBetMonitoringBetList/LastMonitorBetListNew");
            });

            it("MonitorBetList.bindData() should be called", function () {
                expect(monitor.bindData).toHaveBeenCalled();
            });
        });
    });

    describe("clearContent()", function () {
        beforeEach(function () {
            spyOn(monitor, "bindNoInfoRow");

            monitor.clearContent();
        });

        it("MonitorBetList.lastTransId should be 0", function () {
            expect(monitor.lastTransId).toBe(0);
        });

        it("MonitorBetList.bindNoInfoRow() should be called", function () {
            expect(monitor.bindNoInfoRow).toHaveBeenCalled();
        });
    });

    describe("startMonitor()", function () {
        beforeEach(function () {
            spyOn(monitor, "stopMonitor");
            spyOn(monitor, "doMonitor");

            monitor.startMonitor();
        });

        it("MonitorBetList.lastTransId should be 0", function () {
            expect(monitor.lastTransId).toBe(0);
        });

        it("MonitorBetList.stopMonitor() should be called", function () {
            expect(monitor.stopMonitor).toHaveBeenCalled();
        });

        it("MonitorBetList.doMonitor() should be called", function () {
            expect(monitor.doMonitor).toHaveBeenCalled();
        });
    });
});

describe("MonitorBetList", function () {
    beforeEach(function () {
        monitor = new MonitorBetList();
        monitorUi = new MonitorBetListUI(monitor);
    });

    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(monitorUi, "initMainObject");
            spyOn(monitorUi, "initSports");
            spyOn(monitorUi, "initLeague");
            spyOn(monitorUi, "initMatch");
            spyOn(monitorUi, "initBetType");
            spyOn(monitorUi, "initLiveStatus");
            spyOn(monitorUi, "initOddsType");
            spyOn(monitorUi, "initSpecialLevel");
            spyOn(monitorUi, "initCurrency");
            spyOn(monitorUi, "initTimeZone");
            spyOn(monitorUi, "initBasedCurrency");
            spyOn(monitorUi, "initStake");
            spyOn(monitorUi, "initPageSizeControl");
            spyOn(monitorUi, "initFreezeButton");
            spyOn(monitorUi, "initHighlight");
            spyOn(monitorUi, "initSubmit");
            spyOn(monitorUi, "initPlatForm");
            spyOn(monitorUi, "initColumns");

            monitorUi.initialize();
        });

        it("MonitorBetListUI.initMainObject() should be called", function () {
            expect(monitorUi.initMainObject).toHaveBeenCalled();
        });

        it("MonitorBetListUI.initSports() should be called", function () {
            expect(monitorUi.initSports).toHaveBeenCalled();
        });

        it("MonitorBetListUI.initLeague() should be called", function () {
            expect(monitorUi.initLeague).toHaveBeenCalled();
        });

        it("MonitorBetListUI.initMatch() should be called", function () {
            expect(monitorUi.initMatch).toHaveBeenCalled();
        });

        it("MonitorBetListUI.initBetType() should be called", function () {
            expect(monitorUi.initBetType).toHaveBeenCalled();
        });

        it("MonitorBetListUI.initLiveStatus() should be called", function () {
            expect(monitorUi.initLiveStatus).toHaveBeenCalled();
        });

        it("MonitorBetListUI.initOddsType() should be called", function () {
            expect(monitorUi.initOddsType).toHaveBeenCalled();
        });

        it("MonitorBetListUI.initSpecialLevel() should be called", function () {
            expect(monitorUi.initSpecialLevel).toHaveBeenCalled();
        });

        it("MonitorBetListUI.initCurrency() should be called", function () {
            expect(monitorUi.initCurrency).toHaveBeenCalled();
        });

        it("MonitorBetListUI.initTimeZone() should be called", function () {
            expect(monitorUi.initTimeZone).toHaveBeenCalled();
        });

        it("MonitorBetListUI.initBasedCurrency() should be called", function () {
            expect(monitorUi.initBasedCurrency).toHaveBeenCalled();
        });

        it("MonitorBetListUI.initStake() should be called", function () {
            expect(monitorUi.initStake).toHaveBeenCalled();
        });

        it("MonitorBetListUI.initPageSizeControl() should be called", function () {
            expect(monitorUi.initPageSizeControl).toHaveBeenCalled();
        });

        it("MonitorBetListUI.initFreezeButton() should be called", function () {
            expect(monitorUi.initFreezeButton).toHaveBeenCalled();
        });

        it("MonitorBetListUI.initHighlight() should be called", function () {
            expect(monitorUi.initHighlight).toHaveBeenCalled();
        });

        it("MonitorBetListUI.initSubmit() should be called", function () {
            expect(monitorUi.initSubmit).toHaveBeenCalled();
        });

        it("MonitorBetListUI.initPlatForm() should be called", function () {
            expect(monitorUi.initPlatForm).toHaveBeenCalled();
        });

        it("MonitorBetListUI.initColumns() should be called", function () {
            expect(monitorUi.initColumns).toHaveBeenCalled();
        });
    });

    describe("initSports()", function () {
        beforeEach(function () {
            affix("input#match");
            affix("input#league");
        });
        describe("SportId value is 0 or 999", function () {
            beforeEach(function () {
                affix("input#sports[value=0]");
                spyOn(monitorUi, "setHighlightSubmit");
                spyOn($, "ajax");
                spyOn(monitorUi.monitor, "numberParam");
                monitorUi.initSports();
                $("#sports").change();
            });

            it("MonitorBetListUI.setHighlightSubmit() should be called when $('#sports') fired change event", function () {
                expect(monitorUi.setHighlightSubmit).toHaveBeenCalled();
            });

            it("MonitorBetList.sportId should be 0 when $('#sports') fired change event", function () {
                expect(monitor.sportId).toBe(0);
            });

            it("$('#league') should be disabled when $('#sports') fired change event with sportId is 0 or 999", function () {
                expect($('#league')).toBeDisabled();
            });

            it("$('#match') should be disabled when $('#sports') fired change event", function () {
                expect($('#match')).toBeDisabled();
            });

            it("$.ajax should not be called when $('#sports') fired change event with sportId is 0 or 999", function () {
                expect($.ajax).not.toHaveBeenCalled();
            });

            it("MonitorBetListUI.monitor.numberParam(name, value) should be called three times when $('#sports') fired change event", function () {
                expect(monitorUi.monitor.numberParam.calls.count()).toBe(3);
            });
        });

        describe("SportId value not equal 0 and 999", function () {
            var request;
            beforeEach(function (done) {
                affix("input#sports[value=1]");
                spyOn(Site, "blockUI");
                spyOn(Site, "unBlockUI");
                spyOn($.fn, "append");

                jasmine.Ajax.install();
                jasmine.Ajax
                .stubRequest(/api\/LastBetMonitoringBetList\/GetRunningMatchList/)
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ "MatchList": {}, "LeagueList": [{"LeagueId": 1, "LeagueName": "League Name test"}] }'
                });

                monitorUi.initSports();
                $("#sports").change();

                request = jasmine.Ajax.requests.mostRecent();

                setTimeout(done, 10);
            });

            afterEach(function () {
                jasmine.Ajax.uninstall();
            });

            it("$('#league') should be enabled when $('#sports') fired change event with sportId not equal 0 and 999", function () {
                expect($('#league')).not.toBeDisabled();
            });

            it("Site.blockUI() should be called", function () {
                expect(Site.blockUI).toHaveBeenCalled();
            });

            it("Site.unBlockUI() should be called", function () {
                expect(Site.unBlockUI).toHaveBeenCalled();
            });

            it("MonitorBetListUI.currentLeagueList should be [{LeagueId: 1, 'LeagueName': 'League Name test'}]", function () {
                var leagueList = [{ LeagueId: 1, LeagueName: 'League Name test'}];
                expect(monitorUi.currentLeagueList).toEqual(jasmine.arrayContaining(leagueList));
            });
        });
    });

    describe("initLeague()", function () {
        beforeEach(function () {
            affix("input#match");
        });

        describe("leagueId value is 0", function () {
            beforeEach(function () {
                affix("input#league[value=0]");
                spyOn(monitorUi, "setHighlightSubmit");
                spyOn(monitorUi.monitor, "numberParam");
                monitorUi.initLeague();
                $("#league").change();
            });

            it("MonitorBetListUI.setHighlightSubmit() should be called", function () {
                expect(monitorUi.setHighlightSubmit).toHaveBeenCalled();
            });

            it("MonitorBetListUI.monitor.numberParam(name, value) should be called with correct parameters", function () {
                expect(monitorUi.monitor.numberParam.calls.argsFor(0)).toEqual(["leagueId", 0]);
            });

            it("$('#match') should be disabled when $('#league') fired change event", function () {
                expect($('#match')).toBeDisabled();
            });

            it("MonitorBetListUI.monitor.numberParam(name, value) should be called twice when $('#league') fired change event", function () {
                expect(monitorUi.monitor.numberParam.calls.count()).toBe(2);
            });
        });

        describe("leagueId value not equal 0", function () {
            beforeEach(function () {
                affix("input#league[value=1]");
                monitorUi.currentLeagueList = [
                    { LeagueId: 1, LeagueName: 'League Name test 1', IsOutRight: false },
                    { LeagueId: 2, LeagueName: 'League Name test 2', IsOutRight: false }
                ];

                monitorUi.currentMatchList = [
                    { MatchId: 1, MatchName: 'Match Name test 1', LeagueId: 1 },
                    { MatchId: 2, MatchName: 'Match Name test 2', LeagueId: 1 }
                ];

                spyOn($.fn, "append");

                monitorUi.initLeague();
                $("#league").change();
            });

            it("append function of jquery should be called twice", function () {
                expect($.fn.append).toHaveBeenCalled();
            });

            it("$('#match') should be enabled when $('#league') fired change event", function () {
                expect($('#match')).not.toBeDisabled();
            });
        });
    });

    describe("initMatch()", function () {
        beforeEach(function () {
            affix("input#match");
            spyOn(monitorUi, "setHighlightSubmit");
            spyOn(monitorUi.monitor, "numberParam");

            monitorUi.initMatch();
            $('#match').val(1);
            $('#match').change();
        });

        it("MonitorBetListUI.setHighlightSubmit() should be called", function () {
            expect(monitorUi.setHighlightSubmit).toHaveBeenCalled();
        });

        it("MonitorBetListUI.monitor.numberParam(name, value) should be called with correct parameters", function () {
            expect(monitorUi.monitor.numberParam).toHaveBeenCalledWith("matchId", "1");
        });
    });

    describe("initBetType()", function () {
        beforeEach(function () {
            affix("input#bet-type[value=1]");
            spyOn(monitorUi, "setHighlightSubmit");
            spyOn(monitorUi.monitor, "stringParam");

            monitorUi.initBetType();
            $('#bet-type').change();
        });

        it("MonitorBetListUI.setHighlightSubmit() should be called", function () {
            expect(monitorUi.setHighlightSubmit).toHaveBeenCalled();
        });

        it("MonitorBetListUI.monitor.stringParam(name, value) should be called with correct parameters", function () {
            expect(monitorUi.monitor.stringParam).toHaveBeenCalledWith("betType", "1");
        });
    });

    describe("initLiveStatus()", function () {
        beforeEach(function () {
            affix("input#live-status[value=1]");
            spyOn(monitorUi, "setHighlightSubmit");
            spyOn(monitorUi.monitor, "stringParam");

            monitorUi.initLiveStatus();
            $('#live-status').change();
        });

        it("MonitorBetListUI.setHighlightSubmit() should be called", function () {
            expect(monitorUi.setHighlightSubmit).toHaveBeenCalled();
        });

        it("MonitorBetListUI.monitor.stringParam(name, value) should be called with correct parameters", function () {
            expect(monitorUi.monitor.stringParam).toHaveBeenCalledWith("liveStatus", "1");
        });
    });

    describe("initOddsType()", function () {
        beforeEach(function () {
            affix("input#odds-type[value=1]");
            spyOn(monitorUi, "setHighlightSubmit");
            spyOn(monitorUi.monitor, "stringParam");

            monitorUi.initOddsType();
            $('#odds-type').change();
        });

        it("MonitorBetListUI.setHighlightSubmit() should be called", function () {
            expect(monitorUi.setHighlightSubmit).toHaveBeenCalled();
        });

        it("MonitorBetListUI.monitor.stringParam(name, value) should be called with correct parameters", function () {
            expect(monitorUi.monitor.stringParam).toHaveBeenCalledWith("oddsType", "1");
        });
    });

    describe("initSpecialLevel()", function () {
        beforeEach(function () {
            affix("select#special-level");
            spyOn($.fn, "multiSelect");

            monitorUi.initSpecialLevel();
        });

        it("multiSelect function of jquery should be called", function () {
            expect($.fn.multiSelect).toHaveBeenCalled();
        });
    });

    describe("initCurrency()", function () {
        beforeEach(function () {
            affix("select#currency");
            spyOn($.fn, "multiSelect");

            monitorUi.initCurrency();
        });

        it("multiSelect function of jquery should be called", function () {
            expect($.fn.multiSelect).toHaveBeenCalled();
        });
    });

    describe("initTimeZone()", function () {
        beforeEach(function () {
            affix("input#time-zone[value=1]");
            spyOn(monitorUi, "setHighlightSubmit");
            spyOn(monitorUi.monitor, "numberParam");

            monitorUi.initTimeZone();
            $('#time-zone').change();
        });

        it("MonitorBetListUI.setHighlightSubmit() should be called", function () {
            expect(monitorUi.setHighlightSubmit).toHaveBeenCalled();
        });

        it("MonitorBetListUI.monitor.numberParam(name, value) should be called with correct parameters", function () {
            expect(monitorUi.monitor.numberParam).toHaveBeenCalledWith("timeZone", "1");
        });
    });

    describe("initBasedCurrency()", function () {
        beforeEach(function () {
            affix("input#based-currency[value=1]");
            spyOn(monitorUi, "setHighlightSubmit");
            spyOn(monitorUi.monitor, "stringParam");

            monitorUi.initBasedCurrency();
            $('#based-currency').change();
        });

        it("MonitorBetListUI.setHighlightSubmit() should be called", function () {
            expect(monitorUi.setHighlightSubmit).toHaveBeenCalled();
        });

        it("MonitorBetListUI.monitor.stringParam(name, value) should be called with correct parameters", function () {
            expect(monitorUi.monitor.stringParam).toHaveBeenCalledWith("basedCurrency", "1");
        });
    });

    describe("initStake()", function () {
        beforeEach(function () {
            affix("input#stake-from");
            affix("input#stake-to");

            spyOn(monitorUi, "setHighlightSubmit");
            spyOn(monitorUi.monitor, "numberParam");

            monitorUi.initStake();
        });

        it("MonitorBetListUI.setHighlightSubmit() should be called when $('#stake-from') fired keyup event", function () {
            $("#stake-from").keyup();
            expect(monitorUi.setHighlightSubmit).toHaveBeenCalled();
        });

        it("MonitorBetListUI.setHighlightSubmit() should be called when $('#stake-to') fired keyup event", function () {
            $("#stake-to").keyup();
            expect(monitorUi.setHighlightSubmit).toHaveBeenCalled();
        });

        it("MonitorBetListUI.setHighlightSubmit() should be called when $('#stake-from') fired change event", function () {
            $("#stake-from").change();
            expect(monitorUi.setHighlightSubmit).toHaveBeenCalled();
        });

        it("MonitorBetListUI.monitor.numberParam(name, value) should be called with correct parameters when $('#stake-from') fired change event", function () {
            $("#stake-from").val(1);
            $("#stake-from").change();
            expect(monitorUi.monitor.numberParam).toHaveBeenCalledWith("stakeFrom", 1);
        });

        it("MonitorBetListUI.setHighlightSubmit() should be called when $('#stake-to') fired change event", function () {
            $("#stake-to").change();
            expect(monitorUi.setHighlightSubmit).toHaveBeenCalled();
        });

        it("MonitorBetListUI.monitor.numberParam(name, value) should be called with correct parameters when $('#stake-to') fired change event", function () {
            $("#stake-to").val(2);
            $("#stake-to").change();
            expect(monitorUi.monitor.numberParam).toHaveBeenCalledWith("stakeTo", 2);
        });
    });

    describe("initPageSizeControl()", function () {
        beforeEach(function () {
            affix("#pagesize input[value=10]");

            spyOn(monitorUi, "setHighlightSubmit");
            spyOn(monitorUi.monitor, "numberParam");

            monitorUi.initPageSizeControl();

            $("#pagesize input").click();
        });

        it("MonitorBetListUI.setHighlightSubmit() should be called when $('#pagesize input') fired click event", function () {
            expect(monitorUi.setHighlightSubmit).toHaveBeenCalled();
        });

        it("MonitorBetListUI.monitor.numberParam(name, value) should be called with correct parameters when $('#pagesize input') fired click event", function () {
            expect(monitorUi.monitor.numberParam).toHaveBeenCalledWith("pageSize", "10");
        });

        it("$('#pagesize input') should be enabled when $('#pagesize input') fired change event", function () {
            expect($('#pagesize input')).toBeDisabled();
        });
    });

    describe("initFreezeButton()", function () {
        describe("currentStatus is 1", function () {
            beforeEach(function () {
                affix("button#freeze-button[data-status=1]");

                spyOn(monitor, "stopMonitor");
                spyOn(Site, "getSingleResource");

                monitorUi.initFreezeButton();
                $("#freeze-button").click();
            });

            if ("Site.getSingleResource(enghlishName) should be called with correct parameters", function () {
                expect(Site.getSingleResource).toHaveBeenCalledWith("Unfreeze");
            });

            it("MonitorBetList.stopMonitor() should be callled when $('#freeze-button') fired click event", function () {
                expect(monitor.stopMonitor).toHaveBeenCalled();
            });
        });

        describe("currentStatus not equal 1", function () {
            beforeEach(function () {
                affix("button#freeze-button[data-status=0]");

                spyOn(monitor, "startMonitor");
                spyOn(Site, "getSingleResource");

                monitorUi.initFreezeButton();
                $("#freeze-button").click();
            });

            if ("Site.getSingleResource(enghlishName) should be called with correct parameters", function () {
                expect(Site.getSingleResource).toHaveBeenCalledWith("Freeze");
            });

            it("MonitorBetList.startMonitor() should be callled when $('#freeze-button') fired click event", function () {
                expect(monitor.startMonitor).toHaveBeenCalled();
            });
        });
    });

    describe("initHighlight()", function () {
        beforeEach(function () {
            affix("table#last-bets-monitoring tr");
            monitorUi.monitor.containerTable = $("#last-bets-monitoring");

            spyOn($.fn, "toggleClass");

            monitorUi.initHighlight();
            monitorUi.monitor.containerTable.find("tr").click();
        });

        it("toggleClass function of jquery shuold be called with correct parameters", function () {
            expect($.fn.toggleClass).toHaveBeenCalledWith("monitor-keep");
        });
    });

    describe("setHighlightSubmit()", function () {
        var $button;
        beforeEach(function () {
            $button = affix("button#submit-button");
            jasmine.clock().install();

            monitorUi.setHighlightSubmit();
        });
        afterEach(function () {
            jasmine.clock().uninstall();
        });

        it("$('#submit-button') should not have 'background-color' css with value is 'rgb(211,211,211)'", function () {
            expect($button).not.toHaveCss({ "background-color": "rgb(211,211,211)" });
        });

        it("$('#submit-button') should have 'background-color' css with value is 'rgb(211,211,211)'", function () {
            jasmine.clock().tick(1000);
            expect($button.css("background-color")).toBe('rgb(211, 211, 211)');
        });
    });

    describe("initSubmit()", function () {
        beforeEach(function () {
            affix("button#submit-button");
            affix("button#freeze-button[data-status=1]");

            spyOn(monitorUi.monitor, "Submit");
            monitorUi.initSubmit();
            $("#submit-button").click();
        });

        it("MonitorBetList.monitor.Submit shuold be called with correct parameters", function () {
            expect(monitorUi.monitor.Submit).toHaveBeenCalledWith("1");
        });
    });

    describe("initPlatForm()", function () {
        beforeEach(function () {
            affix("input#bet-form[value=1]");
            spyOn(monitorUi, "setHighlightSubmit");
            spyOn(monitorUi.monitor, "numberParam");

            monitorUi.initPlatForm();
            $('#bet-form').change();
        });

        it("MonitorBetListUI.setHighlightSubmit() should be called", function () {
            expect(monitorUi.setHighlightSubmit).toHaveBeenCalled();
        });

        it("MonitorBetListUI.monitor.numberParam(name, value) should be called with correct parameters", function () {
            expect(monitorUi.monitor.numberParam).toHaveBeenCalledWith("betFrom", "1");
        });
    });
});