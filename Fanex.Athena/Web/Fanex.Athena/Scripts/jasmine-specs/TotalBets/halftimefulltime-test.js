describe("HalfTimeFullTime", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(Totalbets, "initialize");
            spyOn(HalfTimeFullTime, "initEvent");
            spyOn(Site, "addUserGuide");

            HalfTimeFullTime.initialize();
        });

        it("Totalbets.initialize() should be called", function () {
            expect(Totalbets.initialize).toHaveBeenCalled();
        });
        it("HalfTimeFullTime.initEvent() should be called", function () {
            expect(HalfTimeFullTime.initEvent).toHaveBeenCalled();
        });
        it("Site.addUserGuide() should be called", function () {
            var title = Site.getSingleResource("TotalBets") + " - " + Site.getSingleResource("HTFT")
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "TotalBetsHTFT", title: title });
        });
    });

    describe("initEvent()", function () {
        it("HalfTimeFullTime.submit() should be called when '#SubmitButton' fired click event", function () {
            var $submitButton = affix("#SubmitButton");
            HalfTimeFullTime.initEvent();

            spyOn(HalfTimeFullTime, "submit");

            $submitButton.click();

            expect(HalfTimeFullTime.submit).toHaveBeenCalled();
        });

        it("Totalbets.viewRunningBetListMultiBettype(matchId, betType) should be called with correct parameters when '.team' fired click event", function () {
            affix("#Refresh-Content .team");
            var $team = $(".team");
            $team.attr("data-match-id", 1);
            HalfTimeFullTime.initEvent();

            spyOn(Totalbets, "viewRunningBetListMatch");

            $team.click();

            expect(Totalbets.viewRunningBetListMatch).toHaveBeenCalledWith("1", 16);
        });
    });

    describe("submit()", function () {
        it("Site.blockUI() should be called", function () {
            spyOn(Site, "blockUI");

            HalfTimeFullTime.submit();

            expect(Site.blockUI).toHaveBeenCalled();
        });

        it("submit function of jquery should be called", function () {
            affix("#HalfTimeFullTimeForm");
            spyOn($.fn, "submit");

            HalfTimeFullTime.submit();

            expect($.fn.submit).toHaveBeenCalled();
        });
    });
});