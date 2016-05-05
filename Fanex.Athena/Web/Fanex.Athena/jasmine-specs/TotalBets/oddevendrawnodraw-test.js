describe("OddEvenDND", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(Totalbets, "initialize");
            spyOn(OddEvenDND, "initEvent");
            spyOn(Site, "addUserGuide");

            OddEvenDND.initialize();
        });

        it("Totalbets.initialize() should be called", function () {
            expect(Totalbets.initialize).toHaveBeenCalled();
        });
        it("OddEvenDND.initEvent() should be called", function () {
            expect(OddEvenDND.initEvent).toHaveBeenCalled();
        });
        it("Site.addUserGuide() should be called with correct parameters", function () {
            var title = Site.getSingleResource("TotalBets") + " - " + Site.getSingleResource("OddEvenDND");
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "TotalBetsOddEven", title: title });
        });
    });

    describe("initEvent()", function () {
        it("OddEvenDND.submit() should be called when '#SubmitButton' fired click event", function () {
            var $submitButton = affix("#SubmitButton");
            OddEvenDND.initEvent();

            spyOn(OddEvenDND, "submit");

            $submitButton.click();

            expect(OddEvenDND.submit).toHaveBeenCalled();
        });

        it("OddEvenDND.viewForecast1X2(matchId, betType) should be called with correct parameters when '.button-add' fired click event", function () {
            affix("#Refresh-Content .button-add");
            var $team = $(".button-add");
            $team.attr("data-match-id", 1);
            $team.attr("data-bet-type", 2);
            OddEvenDND.initEvent();

            spyOn(OddEvenDND, "viewForecast1X2");

            $team.click();

            expect(OddEvenDND.viewForecast1X2).toHaveBeenCalledWith("1", "2");
        });
    });

    describe("submit()", function () {
        it("Site.blockUI() should be called", function () {
            spyOn(Site, "blockUI");

            OddEvenDND.submit();

            expect(Site.blockUI).toHaveBeenCalled();
        });

        it("submit function of jquery should be called", function () {
            affix("#OddEvenDrawNoDrawForm");
            spyOn($.fn, "submit");

            OddEvenDND.submit();

            expect($.fn.submit).toHaveBeenCalled();
        });
    });
});