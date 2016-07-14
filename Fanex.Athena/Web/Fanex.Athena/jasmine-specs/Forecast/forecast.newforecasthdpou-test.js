describe("NewForecastHdpOu", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(RefreshCountDown, "initialize");
            spyOn(NewForecastHdpOu, "initEvent");
            spyOn(Site, "addUserGuide");

            NewForecastHdpOu.initialize();
        });

        it("RefreshCountDown.initialize() should be called", function () {
            expect(RefreshCountDown.initialize).toHaveBeenCalled();
        });
        it("NewForecastHdpOu.initEvent() should be called", function () {
            expect(NewForecastHdpOu.initEvent).toHaveBeenCalled();
        });
        it("Site.addUserGuide() should be called with correct parameter", function () {
            var title = Site.getSingleResource("Forecast") + " " + Site.getSingleResource("HandicapOverUnderLive");
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "ForecastNewHdpOu", title: title });
        });
    });

    describe("initEvent()", function () {
        var $leagueList, $submitButton;
        beforeEach(function () {
            $leagueList = affix("input.league-list[value=1]");
            $submitButton = affix("#SubmitButton");

            NewForecastHdpOu.initEvent();
        });

        it("NewForecastHdpOu.loadMatchList(leagueId) should be called with correct parameter when '.league-list' fired change event", function () {
            spyOn(NewForecastHdpOu, "loadMatchList");

            $leagueList.change();

            expect(NewForecastHdpOu.loadMatchList).toHaveBeenCalledWith("1");
        });

        it("NewForecastHdpOu.submit() should be called when '#SubmitButton' fired change event", function () {
            spyOn(NewForecastHdpOu, "submit");

            $submitButton.click();

            expect(NewForecastHdpOu.submit).toHaveBeenCalled();
        });
    });

    describe("collapseExpand(context, statusElement)", function () {
        var $context;
        beforeEach(function () {
            $context = affix("#context[data-block-id=1]");

            affix("input#status");
        });

        it("$context should has correct class when current state is collapsing", function () {
            $context.addClass("expand-icon");

            NewForecastHdpOu.collapseExpand($context, "status");

            expect($context).toHaveClass("collapse-icon");
            expect($context).not.toHaveClass("expand-icon");
        });

        it("$context should has correct class when current state is expanding", function () {
            $context.addClass("collapse-icon");

            NewForecastHdpOu.collapseExpand($context, "status");

            expect($context).toHaveClass("expand-icon");
            expect($context).not.toHaveClass("collapse-icon");           
        });
    });

    describe("loadMatchList(leagueId)", function () {
        var $ddlmatch;
        var request;
        beforeEach(function (done) {
            $ddlmatch = affix("select#DdlMatchId");
            affix("button#SubmitButton[disabled=disabled]");

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("Forecast/GetMatchListByLeague?leagueid=1")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '<option>Test</option>'
                });

            NewForecastHdpOu.loadMatchList(1);
            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with correct url", function () {
            expect(request.url).toBe("Forecast/GetMatchListByLeague?leagueid=1");
        });
        it("'#DdlMatchId' should be contain correct responsed text", function () {
            expect($ddlmatch).toContainHtml("<option>Test</option>");
        });
        it("'#SubmitButton' should not be disable", function () {
            expect($("#SubmitButton")).not.toBeDisabled();
        });
    });

    describe("submit()", function () {
        var $league, $match, $showFt, $showFh;
        beforeEach(function () {
            affix("input#DdlLeagueId[value=2]");
            affix("input#DdlMatchId[value=1]");
            $league = affix("input#LeagueId");
            $match = affix("input#MatchId");
            $showFt = affix("input#IsShowFt");
            $showFh = affix("input#IsShowFh");
            affix("form#NewForecastHdpOuForm");

            spyOn($.fn, "submit");

            NewForecastHdpOu.submit();
        });

        it("Values should be assign correctly", function () {
            expect($league.val()).toBe("2");
            expect($match.val()).toBe("1");
            expect($showFt.val()).toBe("true");
            expect($showFh.val()).toBe("true");
        });

        it("submit function of jquery should be called", function () {
            expect($.fn.submit).toHaveBeenCalled();
        });
    });

    describe("openBetlist(betChoice, param, betType, handicap)", function () {
        var queryString = "betchoice=1&matchid=11&leagueid=12&currencyid=13&currencyname=14&basecurrency=15&bettype=2&handicap=3";
        var url = "RunningMatchChoiceBetList/Index?" + queryString;

        it("window.open should be called with correct url", function () {
            var param = {
                MatchId: 11,
                LeagueId: 12,
                CurrencyId: 13,
                CurrencyName: 14,
                BaseCurrency: 15
            };
            spyOn(window, "open");

            NewForecastHdpOu.openBetlist(1, param, 2, 3);

            expect(window.open.calls.argsFor(0)[0]).toBe(url);
        });
    });
});