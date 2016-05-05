describe("Outright", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(Totalbets, "initialize");
            spyOn(Outright, "initEvent");
            spyOn(Site, "addUserGuide");

            Outright.initialize();
        });

        it("Totalbets.initialize() should be called", function () {
            expect(Totalbets.initialize).toHaveBeenCalled();
        });
        it("Outright.initEvent() should be called", function () {
            expect(Outright.initEvent).toHaveBeenCalled();
        });
        it("Site.addUserGuide() should be called with correct parameters", function () {
            var title = Site.getSingleResource("TotalBets") + " - " + Site.getSingleResource("Outright")
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "TotalBetsOutright", title: title });
        });
    });

    describe("initEvent()", function () {
        it("Outright.submit() should be called when '#SubmitButton' fired click event", function () {
            var $submitButton = affix("#SubmitButton");
            Outright.initEvent();

            spyOn(Outright, "submit");

            $submitButton.click();

            expect(Outright.submit).toHaveBeenCalled();
        });

        it("Totalbets.viewRunningBetListMatch(matchId, betType) should be called with correct parameters when '.team' fired click event", function () {
            affix("#Refresh-Content .team");
            var $team = $(".team");
            $team.attr("data-match-id", 1);
            Outright.initEvent();

            spyOn(Totalbets, "viewRunningBetListMatch");

            $team.click();

            expect(Totalbets.viewRunningBetListMatch).toHaveBeenCalledWith("1", 10);
        });
    });

    describe("submit()", function () {
        it("Site.blockUI() should be called", function () {
            spyOn(Site, "blockUI");

            Outright.submit();

            expect(Site.blockUI).toHaveBeenCalled();
        });

        it("submit function of jquery should be called", function () {
            affix("#OutrightForm");
            spyOn($.fn, "submit");

            Outright.submit();

            expect($.fn.submit).toHaveBeenCalled();
        });
    });
});