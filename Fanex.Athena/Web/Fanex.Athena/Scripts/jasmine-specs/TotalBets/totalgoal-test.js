describe("TotalGoal", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(Totalbets, "initialize");
            spyOn(TotalGoal, "initEvent");
            spyOn(Site, "addUserGuide");

            TotalGoal.initialize();
        });

        it("Totalbets.initialize() should be called", function () {
            expect(Totalbets.initialize).toHaveBeenCalled();
        });
        it("TotalGoal.initEvent() should be called", function () {
            expect(TotalGoal.initEvent).toHaveBeenCalled();
        });
        it("Site.addUserGuide() should be called with correct parameters", function () {
            var title = Site.getSingleResource("TotalBets") + " - " + Site.getSingleResource("TotalGoal");
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "TotalBetsTotalGoal", title: title });
        });
    });

    describe("initEvent()", function () {
        it("TotalGoal.submit() should be called when '#SubmitButton' fired click event", function () {
            var $submitButton = affix("#SubmitButton");
            TotalGoal.initEvent();

            spyOn(TotalGoal, "submit");

            $submitButton.click();

            expect(TotalGoal.submit).toHaveBeenCalled();
        });

        it("Totalbets.viewRunningBetListMultiBettype(matchId, bettypeParameter) should be called with correct parameters when '.team' fired click event", function () {
            affix("#Refresh-Content .team");
            var $team = $(".team");
            $team.attr("data-match-id", 1);
            TotalGoal.initEvent();

            spyOn(Totalbets, "viewRunningBetListMultiBettype");

            $team.click();

            expect(Totalbets.viewRunningBetListMultiBettype).toHaveBeenCalledWith("1", "6,126");
        });
    });

    describe("submit()", function () {
        it("Site.blockUI() should be called", function () {
            spyOn(Site, "blockUI");

            TotalGoal.submit();

            expect(Site.blockUI).toHaveBeenCalled();
        });

        it("submit function of jquery should be called", function () {
            affix("#TotalGoalForm");
            spyOn($.fn, "submit");

            TotalGoal.submit();

            expect($.fn.submit).toHaveBeenCalled();
        });
    });
});