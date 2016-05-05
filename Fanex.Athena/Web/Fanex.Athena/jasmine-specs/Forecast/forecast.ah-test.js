describe("Forecast", function () {
    beforeEach(function () {
        var $forecastTable = affix("#forecast-table");
        $forecastTable.attr('data-match-id', 1);
        $forecastTable.attr('data-basecurrency', 1);
        $forecastTable.attr('data-licenseetype', 1);
        $forecastTable.attr('data-currencyid', 2);
        $forecastTable.attr('data-currencyname', "eur");
    });

    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(RefreshCountDown, "initialize");
            spyOn(Forecast, "initEvent");

            Forecast.initialize();
        });

        it("RefreshCountDown.initialize() should be called", function () {
            expect(RefreshCountDown.initialize).toHaveBeenCalled();
        });

        it("RefreshCountDown.initialize() should be called", function () {
            expect(Forecast.initEvent).toHaveBeenCalled();
        });
    });

    describe("initEvent()", function () {
        it("Forecast.loadForecast(selectedScore, bettype) should be called with correct parameters when '.forecast-link' fired click event", function () {
            affix("#Refresh-Content .forecast-link");
            var $forecastLink = $(".forecast-link");
            $forecastLink.attr("data-bettype", 1);
            $forecastLink.attr("data-current-score", 2);

            Forecast.initEvent();
            spyOn(Forecast, "loadForecast");

            $forecastLink.click();

            expect(Forecast.loadForecast).toHaveBeenCalledWith(2, 1);
        });

        it("Forecast.loadForecast(selectedScore, bettype) should be called with correct parameter when '.view-forecast-link' fired click event", function () {
            affix("#Refresh-Content .view-forecast-link");
            var $viewForecastLink = $(".view-forecast-link");
            $viewForecastLink.attr("data-bettype", 12);

            Forecast.initEvent();
            spyOn(Forecast, "loadForecast");

            $viewForecastLink.click();

            expect(Forecast.loadForecast).toHaveBeenCalledWith(null, 12);
        });

        it("Forecast.viewBetlist(bettype) should be called with correct parameter when '.view-betlist' fired click event", function () {
            affix("#Refresh-Content .view-betlist");
            var $viewBetlist = $(".view-betlist");
            $viewBetlist.attr("data-bettype", 5);

            Forecast.initEvent();
            spyOn(Forecast, "viewBetlist");

            $viewBetlist.click();

            expect(Forecast.viewBetlist).toHaveBeenCalledWith(5);
        });
    });

    describe("loadForecast(selectedScore, bettype)", function () {
        var request;
        beforeEach(function (done) {
            affix("#Refresh-Content");
            affix("#BetType");
            affix("#CurrentScore");

            spyOn(RefreshCountDown, "initialize");
            spyOn(RefreshCountDown, "initEvent");

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("Forecast/ForeCastAh?matchid=1&bettype=6&basecurrency=1&licenseetype=1&currencyid=2&currentscore=5")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': 'success'
                });

            Forecast.loadForecast(5, 6);
            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with correct url", function () {
            var expectedUrl = "Forecast/ForeCastAh?matchid=1&bettype=6&basecurrency=1&licenseetype=1&currencyid=2&currentscore=5";
            expect(request.url).toBe(expectedUrl);
        });

        it("RefreshCountDown.initialize() should be called", function () {
            expect(RefreshCountDown.initialize).toHaveBeenCalled();
        });

        it("RefreshCountDown.initEvent() should be called", function () {
            expect(RefreshCountDown.initEvent).toHaveBeenCalled();
        });

        it("'#Refresh-Content' should be contain correct responsed text", function () {
            expect($("#Refresh-Content")).toContainText('success');
        });
    });

    describe("viewBetlist(bettype)", function () {
        it("ForecastPopup.resizePopup(width, height, url) should be called with correct parameter", function () {
            spyOn(ForecastPopup, "resizePopup");
            Forecast.viewBetlist(5);

            var url = "BetList/RunningBetListOfMatch?matchid=1&bettype=5&basecurrency=1&currencyid=2&currencyname=eur&licenseetype=1";

            expect(ForecastPopup.resizePopup).toHaveBeenCalledWith(jasmine.any(Number), jasmine.any(Number), url);
        });
    });

    describe("setCurrentScore", function () {
        it("Forecast.loadForecast(selectedScore, bettype) should be called with correct parameter", function () {
            affix("input#txt-currentscore[value=1]");
            affix("#Refresh-Content a.view-betlist[data-bettype=15]");

            spyOn(Forecast, "loadForecast");

            Forecast.setCurrentScore();

            expect(Forecast.loadForecast).toHaveBeenCalledWith("1", "15");
        });
    });
});