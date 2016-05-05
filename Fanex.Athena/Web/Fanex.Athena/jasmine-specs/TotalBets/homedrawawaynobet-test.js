describe("HomeDrawAwayNoBet", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(Totalbets, "initialize");
            spyOn(HomeDrawAwayNoBet, "initEvent");
            spyOn(Site, "addUserGuide");

            HomeDrawAwayNoBet.initialize();
        });

        it("Totalbets.initialize() should be called", function () {
            expect(Totalbets.initialize).toHaveBeenCalled();
        });
        it("HomeDrawAwayNoBet.initEvent() should be called", function () {
            expect(HomeDrawAwayNoBet.initEvent).toHaveBeenCalled();
        });
        it("Site.addUserGuide() should be called with correct parameters", function () {
            var title = Site.getSingleResource("TotalBets") + " - " + Site.getSingleResource("HomeDrawAwayNoBet");
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "TotalBetsHomeDrawAwayNoBet", title: title });
        });
    });

    describe("initEvent()", function () {
        it("HomeDrawAwayNoBet.submit() should be called when '#SubmitButton' fired click event", function () {
            var $submitButton = affix("#SubmitButton");
            HomeDrawAwayNoBet.initEvent();

            spyOn(HomeDrawAwayNoBet, "submit");

            $submitButton.click();

            expect(HomeDrawAwayNoBet.submit).toHaveBeenCalled();
        });

        it("Totalbets.viewRunningBetListMultiBettype(matchId, betType) should be called with correct parameters when '.team' fired click event", function () {
            affix("#Refresh-Content .team");
            var $team = $(".team");
            $team.attr("data-match-id", 1);
            HomeDrawAwayNoBet.initEvent();

            spyOn(Totalbets, "viewRunningBetListMatch");

            $team.click();

            expect(Totalbets.viewRunningBetListMatch).toHaveBeenCalledWith("1", 14);
        });
    });

    describe("submit()", function () {
        it("Site.blockUI() should be called", function () {
            spyOn(Site, "blockUI");

            HomeDrawAwayNoBet.submit();

            expect(Site.blockUI).toHaveBeenCalled();
        });

        it("submit function of jquery should be called", function () {
            affix("#HomeDrawAwayNoBetForm");
            spyOn($.fn, "submit");

            HomeDrawAwayNoBet.submit();

            expect($.fn.submit).toHaveBeenCalled();
        });
    });
});