describe("FirstGoalLastGoal", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(Totalbets, "initialize");
            spyOn(FirstGoalLastGoal, "initEvent");
            spyOn(Site, "addUserGuide");

            FirstGoalLastGoal.initialize();
        });

        it("Totalbets.initialize() should be called", function () {
            expect(Totalbets.initialize).toHaveBeenCalled();
        });
        it("FirstGoalLastGoal.initEvent() should be called", function () {
            expect(FirstGoalLastGoal.initEvent).toHaveBeenCalled();
        });
        it("Site.addUserGuide() should be called with correct parameters", function () {
            var title = Site.getSingleResource("TotalBets") + " - " + Site.getSingleResource("FGLG");
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "TotalBetsFGLG", title: title });
        });
    });

    describe("initEvent()", function () {
        it("FirstGoalLastGoal.submit() should be called when '#SubmitButton' fired click event", function () {
            var $submitButton = affix("#SubmitButton");
            FirstGoalLastGoal.initEvent();

            spyOn(FirstGoalLastGoal, "submit");

            $submitButton.click();

            expect(FirstGoalLastGoal.submit).toHaveBeenCalled();
        });

        it("Totalbets.viewRunningBetListMultiBettype(matchId, bettypeParameter) should be called with correct parameters when '.team' fired click event", function () {
            affix("#Refresh-Content .team");
            var $team = $(".team");
            $team.attr("data-match-id", 1);
            FirstGoalLastGoal.initEvent();

            spyOn(Totalbets, "viewRunningBetListMultiBettype");

            $team.click();

            expect(Totalbets.viewRunningBetListMultiBettype).toHaveBeenCalledWith("1", "14,127");
        });
    });

    describe("submit()", function () {
        it("Site.blockUI() should be called", function () {
            spyOn(Site, "blockUI");

            FirstGoalLastGoal.submit();

            expect(Site.blockUI).toHaveBeenCalled();
        });

        it("submit function of jquery should be called", function () {
            affix("#FirstGoalLastGoalForm");
            spyOn($.fn, "submit");

            FirstGoalLastGoal.submit();

            expect($.fn.submit).toHaveBeenCalled();
        });
    });
});