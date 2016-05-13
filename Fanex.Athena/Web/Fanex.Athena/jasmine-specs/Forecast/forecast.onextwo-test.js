describe("ForecastOneXTwo", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(RefreshCountDown, "initialize");
            spyOn(ForecastOneXTwo, "initEvent");
            spyOn(ForecastOneXTwo, "resizeGrid");
            spyOn(Site, "addUserGuide");

            ForecastOneXTwo.initialize();
        });
        
        it("RefreshCountDown.initialize() should be called", function () {
            expect(RefreshCountDown.initialize).toHaveBeenCalled();
        });
        it("ForecastOneXTwo.initEvent() should be called", function () {
            expect(ForecastOneXTwo.initEvent).toHaveBeenCalled();
        });
        it("ForecastOneXTwo.resizeGrid() should be called", function () {
            expect(ForecastOneXTwo.resizeGrid).toHaveBeenCalled();
        });
        it("Site.addUserGuide() should be called with correct parameters", function () {
            var title = Site.getSingleResource("Forecast") + " - " + Site.getSingleResource("OneXTwo");
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "Forecast1x2", title: title });
        });
    });

    describe("initEvent()", function () {
        var $leagueList, $buttonAdd, $submitButton;
        beforeEach(function () {
            affix("input#IsPopUp[value=false]");
            $leagueList = affix("input.league-list[value=1]");
            affix("#Refresh-Content .button-add");
            $buttonAdd = $(".button-add");
            $submitButton = affix("#SubmitButton");

            ForecastOneXTwo.initEvent();
        });

        it("ForecastOneXTwo.loadSelectedLeague(leagueId) should be called with correct parameter when '.league-list' fired change event", function () {
            spyOn(ForecastOneXTwo, "loadSelectedLeague");

            $leagueList.change();

            expect(ForecastOneXTwo.loadSelectedLeague).toHaveBeenCalledWith("1");
        });
        it("ForecastOneXTwo.openForecast(matchId, betType) should be called with correct parameter when '.button-add' fired click event", function () {
            spyOn(ForecastOneXTwo, "openForecast");
            $buttonAdd.attr("data-match-id", 1);
            $buttonAdd.attr("data-bettype", 4);

            $buttonAdd.click();

            expect(ForecastOneXTwo.openForecast).toHaveBeenCalledWith("1", 4, "false");
        });
        it("ForecastOneXTwo.submit() should be called when '#SubmitButton' fired change event", function () {
            spyOn(ForecastOneXTwo, "submit");

            $submitButton.click();

            expect(ForecastOneXTwo.submit).toHaveBeenCalled();
        });
    });

    describe("openForecast(matchId, betType, isPopup)", function () {
        var queryString = "licenseetype=1&currencyid=2&currencyname=uus&basecurrency=3&matchid=1&bettype=2";
        var url = "BetList/RunningBetListOfMatch?" + queryString;

        beforeEach(function () {
            affix("#LicenseeType");
            affix("#CurrencyId");
            affix("#BaseCurrency");
            $("#CurrencyId").append('<option value="2">uus</option>');

            $("#LicenseeType").attr('data-original', 1);
            $("#CurrencyId").attr('data-original', 2);
            $("#BaseCurrency").attr('data-original', 3);
        });

        it("ForecastPopup.resizePopup(height, height, url) should be called with correct parameters", function () {
            spyOn(ForecastPopup, "resizePopup");

            ForecastOneXTwo.openForecast(1, 2, "true");

            expect(ForecastPopup.resizePopup).toHaveBeenCalledWith(900, jasmine.any(Number), url);
        });
    });

    describe("loadSelectedLeague(leagueId)", function () {
        it("ForecastOneXTwo.submit() should be called", function () {
            spyOn(ForecastOneXTwo, "submit");

            ForecastOneXTwo.loadSelectedLeague(1);

            expect(ForecastOneXTwo.submit).toHaveBeenCalled();
        });
    });

    describe("submit()", function () {
        it("submit function of jquery should be called", function () {
            affix("form#ForecastOneXTwoForm");

            spyOn($.fn, "submit");

            ForecastOneXTwo.submit();

            expect($.fn.submit).toHaveBeenCalled();
        });
    });

});