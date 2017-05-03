
xdescribe("LastBetFilter", function () {
    
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(LastBetFilter, "initMainObject");
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
            spyOn(LastBetFilter, "initHighlight");
            spyOn(LastBetFilter, "initSubmit");
            spyOn(LastBetFilter, "initPlatForm");
            spyOn(LastBetFilter, "initColumns");

            LastBetFilter.initialize();
        });

        it("LastBetFilter.initMainObject() should be called", function () {
            expect(LastBetFilter.initMainObject).toHaveBeenCalled();
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

        it("LastBetFilter.initHighlight() should be called", function () {
            expect(LastBetFilter.initHighlight).toHaveBeenCalled();
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
    });

    describe("initSports()", function () {
        beforeEach(function () {
            affix("input#match");
            affix("input#league");
        });
        describe("SportId value is 0 or 999", function () {
            beforeEach(function () {
                affix("input#sports[value=0]");
                spyOn(LastBetFilter, "setHighlightSubmit");
                spyOn($, "ajax");
                spyOn(LastBetGrid, "numberParam");
                LastBetFilter.initSports();
                $("#sports").change();
            });

            it("LastBetFilter.setHighlightSubmit() should be called when $('#sports') fired change event", function () {
                expect(LastBetFilter.setHighlightSubmit).toHaveBeenCalled();
            });

            it("MonitorBetList.sportId should be 0 when $('#sports') fired change event", function () {
                expect(LastBetGrid.sportId).toBe(0);
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

            it("LastBetGrid.numberParam(name, value) should be called three times when $('#sports') fired change event", function () {
                expect(LastBetGrid.numberParam.calls.count()).toBe(3);
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

                LastBetFilter.initSports();
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

            it("LastBetFilter.currentLeagueList should be [{LeagueId: 1, 'LeagueName': 'League Name test'}]", function () {
                var leagueList = [{ LeagueId: 1, LeagueName: 'League Name test' }];
                expect(LastBetFilter.currentLeagueList).toEqual(jasmine.arrayContaining(leagueList));
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
                spyOn(LastBetFilter, "setHighlightSubmit");
                spyOn(LastBetGrid, "numberParam");
                LastBetFilter.initLeague();
                $("#league").change();
            });

            it("LastBetFilter.setHighlightSubmit() should be called", function () {
                expect(LastBetFilter.setHighlightSubmit).toHaveBeenCalled();
            });

            it("LastBetGrid.numberParam(name, value) should be called with correct parameters", function () {
                expect(LastBetGrid.numberParam.calls.argsFor(0)).toEqual(["LeagueId", 0]);
            });

            it("$('#match') should be disabled when $('#league') fired change event", function () {
                expect($('#match')).toBeDisabled();
            });

            it("LastBetGrid.numberParam(name, value) should be called twice when $('#league') fired change event", function () {
                expect(LastBetGrid.numberParam.calls.count()).toBe(2);
            });
        });

        describe("leagueId value not equal 0", function () {
            beforeEach(function () {
                affix("input#league[value=1]");
                LastBetFilter.currentLeagueList = [
                    { LeagueId: 1, LeagueName: 'League Name test 1', IsOutRight: false },
                    { LeagueId: 2, LeagueName: 'League Name test 2', IsOutRight: false }
                ];

                LastBetFilter.currentMatchList = [
                    { MatchId: 1, MatchName: 'Match Name test 1', LeagueId: 1 },
                    { MatchId: 2, MatchName: 'Match Name test 2', LeagueId: 1 }
                ];

                spyOn($.fn, "append");

                LastBetFilter.initLeague();
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
            spyOn(LastBetFilter, "setHighlightSubmit");
            spyOn(LastBetGrid, "numberParam");

            LastBetFilter.initMatch();
            $('#match').val(1);
            $('#match').change();
        });

        it("LastBetFilter.setHighlightSubmit() should be called", function () {
            expect(LastBetFilter.setHighlightSubmit).toHaveBeenCalled();
        });

        it("LastBetGrid.numberParam(name, value) should be called with correct parameters", function () {
            expect(LastBetGrid.numberParam).toHaveBeenCalledWith("MatchId", "1");
        });
    });

    describe("initBetType()", function () {
        beforeEach(function () {
            affix("input#bet-type[value=1]");
            spyOn(LastBetFilter, "setHighlightSubmit");
            spyOn(LastBetGrid, "stringParam");

            LastBetFilter.initBetType();
            $('#bet-type').change();
        });

        it("LastBetFilter.setHighlightSubmit() should be called", function () {
            expect(LastBetFilter.setHighlightSubmit).toHaveBeenCalled();
        });

        it("LastBetGrid.stringParam(name, value) should be called with correct parameters", function () {
            expect(LastBetGrid.stringParam).toHaveBeenCalledWith("BetType", "1");
        });
    });

    describe("initLiveStatus()", function () {
        beforeEach(function () {
            affix("input#live-status[value=1]");
            spyOn(LastBetFilter, "setHighlightSubmit");
            spyOn(LastBetGrid, "stringParam");

            LastBetFilter.initLiveStatus();
            $('#live-status').change();
        });

        it("LastBetFilter.setHighlightSubmit() should be called", function () {
            expect(LastBetFilter.setHighlightSubmit).toHaveBeenCalled();
        });

        it("LastBetGrid.stringParam(name, value) should be called with correct parameters", function () {
            expect(LastBetGrid.stringParam).toHaveBeenCalledWith("LiveStatus", "1");
        });
    });

    describe("initOddsType()", function () {
        beforeEach(function () {
            affix("input#odds-type[value=1]");
            spyOn(LastBetFilter, "setHighlightSubmit");
            spyOn(LastBetGrid, "stringParam");

            LastBetFilter.initOddsType();
            $('#odds-type').change();
        });

        it("LastBetFilter.setHighlightSubmit() should be called", function () {
            expect(LastBetFilter.setHighlightSubmit).toHaveBeenCalled();
        });

        it("LastBetGrid.stringParam(name, value) should be called with correct parameters", function () {
            expect(LastBetGrid.stringParam).toHaveBeenCalledWith("OddsType", "1");
        });
    });

    describe("initSpecialLevel()", function () {
        beforeEach(function () {
            affix("select#special-level");
            spyOn($.fn, "multiSelect");

            LastBetFilter.initSpecialLevel();
        });

        it("multiSelect function of jquery should be called", function () {
            expect($.fn.multiSelect).toHaveBeenCalled();
        });
    });

    describe("initCurrency()", function () {
        beforeEach(function () {
            affix("select#currency");
            spyOn($.fn, "multiSelect");

            LastBetFilter.initCurrency();
        });

        it("multiSelect function of jquery should be called", function () {
            expect($.fn.multiSelect).toHaveBeenCalled();
        });
    });

    describe("initTimeZone()", function () {
        beforeEach(function () {
            affix("input#time-zone[value=1]");
            spyOn(LastBetFilter, "setHighlightSubmit");
            spyOn(LastBetGrid, "numberParam");

            LastBetFilter.initTimeZone();
            $('#time-zone').change();
        });

        it("LastBetFilter.setHighlightSubmit() should be called", function () {
            expect(LastBetFilter.setHighlightSubmit).toHaveBeenCalled();
        });

        it("LastBetGrid.numberParam(name, value) should be called with correct parameters", function () {
            expect(LastBetGrid.numberParam).toHaveBeenCalledWith("TimeZone", "1");
        });
    });

    describe("initBasedCurrency()", function () {
        beforeEach(function () {
            affix("input#based-currency[value=1]");
            spyOn(LastBetFilter, "setHighlightSubmit");
            spyOn(LastBetGrid, "stringParam");

            LastBetFilter.initBasedCurrency();
            $('#based-currency').change();
        });

        it("LastBetFilter.setHighlightSubmit() should be called", function () {
            expect(LastBetFilter.setHighlightSubmit).toHaveBeenCalled();
        });

        it("LastBetGrid.stringParam(name, value) should be called with correct parameters", function () {
            expect(LastBetGrid.stringParam).toHaveBeenCalledWith("BasedCurrency", "1");
        });
    });

    describe("initStake()", function () {
        beforeEach(function () {
            affix("input#stake-from");
            affix("input#stake-to");

            spyOn(LastBetFilter, "setHighlightSubmit");
            spyOn(LastBetGrid, "numberParam");

            LastBetFilter.initStake();
        });

        it("LastBetFilter.setHighlightSubmit() should be called when $('#stake-from') fired keyup event", function () {
            $("#stake-from").keyup();
            expect(LastBetFilter.setHighlightSubmit).toHaveBeenCalled();
        });

        it("LastBetFilter.setHighlightSubmit() should be called when $('#stake-to') fired keyup event", function () {
            $("#stake-to").keyup();
            expect(LastBetFilter.setHighlightSubmit).toHaveBeenCalled();
        });

        it("LastBetFilter.setHighlightSubmit() should be called when $('#stake-from') fired change event", function () {
            $("#stake-from").change();
            expect(LastBetFilter.setHighlightSubmit).toHaveBeenCalled();
        });

        it("LastBetGrid.numberParam(name, value) should be called with correct parameters when $('#stake-from') fired change event", function () {
            $("#stake-from").val(1);
            $("#stake-from").change();
            expect(LastBetGrid.numberParam).toHaveBeenCalledWith("StakeFrom", 1);
        });

        it("LastBetFilter.setHighlightSubmit() should be called when $('#stake-to') fired change event", function () {
            $("#stake-to").change();
            expect(LastBetFilter.setHighlightSubmit).toHaveBeenCalled();
        });

        it("LastBetGrid.numberParam(name, value) should be called with correct parameters when $('#stake-to') fired change event", function () {
            $("#stake-to").val(2);
            $("#stake-to").change();
            expect(LastBetGrid.numberParam).toHaveBeenCalledWith("StakeTo", 2);
        });
    });

    describe("initPageSizeControl()", function () {
        beforeEach(function () {
            affix("#pagesize input[value=10]");

            spyOn(LastBetFilter, "setHighlightSubmit");
            spyOn(LastBetGrid, "numberParam");

            LastBetFilter.initPageSizeControl();

            $("#pagesize input").click();
        });

        it("LastBetFilter.setHighlightSubmit() should be called when $('#pagesize input') fired click event", function () {
            expect(LastBetFilter.setHighlightSubmit).toHaveBeenCalled();
        });

        it("LastBetGrid.numberParam(name, value) should be called with correct parameters when $('#pagesize input') fired click event", function () {
            expect(LastBetGrid.numberParam).toHaveBeenCalledWith("PageSize", "10");
        });

        it("$('#pagesize input') should be enabled when $('#pagesize input') fired change event", function () {
            expect($('#pagesize input')).toBeDisabled();
        });
    });

    describe("initFreezeButton()", function () {
        describe("currentStatus is 1", function () {
            beforeEach(function () {
                affix("button#freeze-button[data-status=1]");

                spyOn(LastBetGrid, "stopMonitor");
                spyOn(Site, "getSingleResource");

                LastBetFilter.initFreezeButton();
                $("#freeze-button").click();
            });

            if ("Site.getSingleResource(enghlishName) should be called with correct parameters", function () {
                expect(Site.getSingleResource).toHaveBeenCalledWith("Unfreeze");
            });

            it("MonitorBetList.stopMonitor() should be callled when $('#freeze-button') fired click event", function () {
                expect(LastBetGrid.stopMonitor).toHaveBeenCalled();
            });
        });

        describe("currentStatus not equal 1", function () {
            beforeEach(function () {
                affix("button#freeze-button[data-status=0]");

                spyOn(LastBetGrid, "startMonitor");
                spyOn(Site, "getSingleResource");

                LastBetFilter.initFreezeButton();
                $("#freeze-button").click();
            });

            if ("Site.getSingleResource(enghlishName) should be called with correct parameters", function () {
                expect(Site.getSingleResource).toHaveBeenCalledWith("Freeze");
            });

            it("MonitorBetList.startMonitor() should be callled when $('#freeze-button') fired click event", function () {
                expect(LastBetGrid.startMonitor).toHaveBeenCalled();
            });
        });
    });

    describe("initHighlight()", function () {
        beforeEach(function () {
            affix("table#last-bets-monitoring tr");
            LastBetGrid.containerTable = $("#last-bets-monitoring");

            spyOn($.fn, "toggleClass");

            LastBetFilter.initHighlight();
            LastBetGrid.containerTable.find("tr").click();
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

            LastBetFilter.setHighlightSubmit();
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

            spyOn(LastBetGrid, "Submit");
            LastBetFilter.initSubmit();
            $("#submit-button").click();
        });

        it("LastBetGrid.Submit shuold be called with correct parameters", function () {
            expect(LastBetGrid.Submit).toHaveBeenCalledWith("1");
        });
    });

    describe("initPlatForm()", function () {
        beforeEach(function () {
            affix("input#bet-form[value=1]");
            spyOn(LastBetFilter, "setHighlightSubmit");
            spyOn(LastBetGrid, "numberParam");

            LastBetFilter.initPlatForm();
            $('#bet-form').change();
        });

        it("LastBetFilter.setHighlightSubmit() should be called", function () {
            expect(LastBetFilter.setHighlightSubmit).toHaveBeenCalled();
        });

        it("LastBetGrid.numberParam(name, value) should be called with correct parameters", function () {
            expect(LastBetGrid.numberParam).toHaveBeenCalledWith("BetFrom", "1");
        });
    });
});