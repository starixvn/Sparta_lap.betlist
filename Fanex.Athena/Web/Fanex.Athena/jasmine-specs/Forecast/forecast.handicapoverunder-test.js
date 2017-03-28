describe("ForecastHDPOU", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(RefreshCountDown, "initialize");
            spyOn(ForecastHDPOU, "initEvent");
            spyOn(ForecastHDPOU, "resizeGrid");
            spyOn(Site, "addUserGuide");

            ForecastHDPOU.initialize();
        });

        it("RefreshCountDown.initialize() should be called", function () {
            expect(RefreshCountDown.initialize).toHaveBeenCalled();
        });
        it("ForecastHDPOU.initEvent() should be called", function () {
            expect(ForecastHDPOU.initEvent).toHaveBeenCalled();
        });
        it("ForecastHDPOU.resizeGrid() should be called", function () {
            expect(ForecastHDPOU.resizeGrid).toHaveBeenCalled();
        });
        it("Site.addUserGuide() should be called with correct parameters", function () {
            var title = Site.getSingleResource("Forecast") + " - " + Site.getSingleResource("HandicapOverUnderLive");
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "ForecastHdpOu", title: title });
        })
    });

    describe("initEvent()", function () {
        var $leagueList, $buttonAdd, $submitButton;
        beforeEach(function () {
            $leagueList = affix("input.league-list[value=1]");

            affix("#Refresh-Content .button-add");
            $buttonAdd = $(".button-add");

            $submitButton = affix("#SubmitButton");

            ForecastHDPOU.initEvent();
        });

        it("ForecastHDPOU.loadSelectedLeague(leagueId) should be called with correct parameter when '.league-list' fired change event", function () {
            spyOn(ForecastHDPOU, "loadSelectedLeague");

            $leagueList.change();

            expect(ForecastHDPOU.loadSelectedLeague).toHaveBeenCalledWith("1");
        });

        it("ForecastHDPOU.openForecast(matchId, betType) should be called with correct parameter when '.button-add' fired click event", function () {
            spyOn(ForecastHDPOU, "openForecast");
            $buttonAdd.attr("data-match-id", 1);
            $buttonAdd.attr("data-bettype", 4);

            $buttonAdd.click();

            expect(ForecastHDPOU.openForecast).toHaveBeenCalledWith("1", 4);
        });

        it("ForecastHDPOU.submit() should be called when '#SubmitButton' fired change event", function () {
            spyOn(ForecastHDPOU, "submit");

            $submitButton.click();

            expect(ForecastHDPOU.submit).toHaveBeenCalled();
        });
    });

    describe("openForecast(matchId, betType)", function () {
        it("ForecastPopup.showPopup(width, height, url) should be called with correct parameters", function () {
            affix("#CurrencyId");
            affix("#BaseCurrency");
            $("#CurrencyId").append('<option value="2">uus</option>');

            $("#CurrencyId").attr('data-original', 2);
            $("#BaseCurrency").attr('data-original', 3);

            var url = "AhForecast/Index?matchid=1&bettype=2&currencyid=2&currencyname=uus&basecurrency=3";
            spyOn(ForecastPopup, "showPopup");

            ForecastHDPOU.openForecast(1, 2);

            expect(ForecastPopup.showPopup).toHaveBeenCalledWith(700, 250, url);
        });
    });

    describe("loadSelectedLeague(leagueId)", function () {
        var url = "HandicapOverUnderForecast/Index?currencyid=2&currencyname=uus&basecurrency=3&leagueid=12";
        var request;
        beforeEach(function (done) {
            affix("#Refresh-Content");
            affix("#CurrencyId");
            affix("#BaseCurrency");
            $("#CurrencyId").append('<option value="2">uus</option>');

            $("#CurrencyId").attr('data-original', 2);
            $("#BaseCurrency").attr('data-original', 3);

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest(url)
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': 'success'
                });

            ForecastHDPOU.loadSelectedLeague(12);
            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with correct url", function () {
            expect(request.url).toBe(url);
        });

        it("'#Refresh-Content' should be contain correct responsed text", function () {
            expect($("#Refresh-Content")).toContainText('success');
        });
    });

    describe("submit", function () {
        var request;
        beforeEach(function (done) {
            affix("#Refresh-Content");
            spyOn($.fn, "serializeObject").and.returnValue({ data: "test" });

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("HandicapOverUnderForecast/Index")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': ''
                });

            ForecastHDPOU.submit();
            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with correct url", function () {
            expect(request.url).toBe("HandicapOverUnderForecast/Index");
        });
    });
});