describe("Racing", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(Totalbets, "initialize");
            spyOn(Racing, "initEvent");
            spyOn(Site, "addUserGuide");

            Racing.initialize();
        });

        it("Totalbets.initialize() should be called", function () {
            expect(Totalbets.initialize).toHaveBeenCalled();
        });
        it("Racing.initEvent() should be called", function () {
            expect(Racing.initEvent).toHaveBeenCalled();
        });
        it("Site.addUserGuide() should be called with correct parameters", function () {
            var title = Site.getSingleResource("TotalBets") + " - " + Site.getSingleResource("Racing");
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "TotalBetsRacing", title: title });
        });
    });

    describe("initEvent()", function () {
        it("Racing.submit() should be called when '#SubmitButton' fired click event", function () {
            var $submitButton = affix("#SubmitButton");
            Racing.initEvent();

            spyOn(Racing, "submit");

            $submitButton.click();

            expect(Racing.submit).toHaveBeenCalled();
        });

        it("Totalbets.viewRunningBetListMatch(matchId, betType) should be called with correct parameters when '.forecast-number-none' fired click event", function () {
            affix("#Refresh-Content .forecast-number-none");
            var $forcastNumberNone = $(".forecast-number-none");
            $forcastNumberNone.attr("data-match-id", 1);
            $forcastNumberNone.attr("data-bet-type", 2);

            Racing.initEvent();
            spyOn(Totalbets, "viewRunningBetListMatch");

            $forcastNumberNone.click();

            expect(Totalbets.viewRunningBetListMatch).toHaveBeenCalledWith("1", "2");
        });
    });

    describe("submit()", function () {
        it("Site.blockUI() should be called", function () {
            spyOn(Site, "blockUI");

            Racing.submit();

            expect(Site.blockUI).toHaveBeenCalled();
        });

        it("submit function of jquery should be called", function () {
            affix("#RacingForm");
            spyOn($.fn, "submit");

            Racing.submit();

            expect($.fn.submit).toHaveBeenCalled();
        });
    });
});