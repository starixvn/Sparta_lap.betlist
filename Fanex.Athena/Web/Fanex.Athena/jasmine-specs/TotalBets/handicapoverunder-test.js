describe("HandicapOU", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(Totalbets, "initialize");
            spyOn(HandicapOU, "initEvent");
            spyOn(Site, "addUserGuide");

            HandicapOU.initialize();
        });

        it("Totalbets.initialize() should be called", function () {
            expect(Totalbets.initialize).toHaveBeenCalled();
        });
        it("HandicapOU.initEvent() should be called", function () {
            expect(HandicapOU.initEvent).toHaveBeenCalled();
        });
        it("Site.addUserGuide() should be called with correct parameters", function () {
            var title = Site.getSingleResource("TotalBets") + " - " + Site.getSingleResource("HandicapOverUnderLive");
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "TotalBetsHdpOu", title: title });
        });
    });

    describe("initEvent()", function () {
        it("HandicapOU.submit() should be called when '#SubmitButton' fired click event", function () {
            var $submitButton = affix("#SubmitButton");
            HandicapOU.initEvent();

            spyOn(HandicapOU, "submit");

            $submitButton.click();

            expect(HandicapOU.submit).toHaveBeenCalled();
        });

        it("HandicapOU.viewForecast(matchId, betType) should be called with correct parameters when '.button-add' fired click event", function () {
            affix("#Refresh-Content .button-add");
            var $team = $(".button-add");
            $team.attr("data-match-id", 1);
            $team.attr("data-bet-type", 2);
            HandicapOU.initEvent();

            spyOn(HandicapOU, "viewForecast");

            $team.click();

            expect(HandicapOU.viewForecast).toHaveBeenCalledWith("1", "2");
        });
    });

    describe("viewForecast(matchId, betType)", function () {
        it("ForecastPopup.showPopup(width, height, url) should be called with correct parameters", function () {
            affix("#CurrencyId");
            affix("#BaseCurrency");
            $("#CurrencyId").append('<option value="2">uus</option>');

            $("#CurrencyId").attr('data-original', 2);
            $("#BaseCurrency").attr('data-original', 3);

            var urlRegex = /AhForecast\/Index\?matchid=1\&bettype=2\&currencyid=2\&currencyname=uus\&basecurrency=3/i;
            spyOn(ForecastPopup, "showPopup");

            HandicapOU.viewForecast(1, 2);

            expect(ForecastPopup.showPopup).toHaveBeenCalledWith(700, 250, jasmine.stringMatching(urlRegex));
        });
    });

    describe("submit()", function () {
        it("Site.blockUI() should be called", function () {
            spyOn(Site, "blockUI");

            HandicapOU.submit();

            expect(Site.blockUI).toHaveBeenCalled();
        });

        it("submit function of jquery should be called", function () {
            affix("#HandicapOUForm");
            spyOn($.fn, "submit");

            HandicapOU.submit();

            expect($.fn.submit).toHaveBeenCalled();
        });
    });
});