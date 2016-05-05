describe("NumberGame", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(Totalbets, "initialize");
            spyOn(NumberGame, "initEvent");
            spyOn(Site, "addUserGuide");

            NumberGame.initialize();
        });

        it("Totalbets.initialize() should be called", function () {
            expect(Totalbets.initialize).toHaveBeenCalled();
        });
        it("NumberGame.initEvent() should be called", function () {
            expect(NumberGame.initEvent).toHaveBeenCalled();
        });
        it("Site.addUserGuide() should be called with correct parameters", function () {
            var title = Site.getSingleResource("TotalBets") + " - " + Site.getSingleResource("NumberGame");
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "TotalBetsNumberGame", title: title });
        });
    });

    describe("initEvent()", function () {
        it("NumberGame.submit() should be called when '#SubmitButton' fired click event", function () {
            var $submitButton = affix("#SubmitButton");
            NumberGame.initEvent();

            spyOn(NumberGame, "submit");

            $submitButton.click();

            expect(NumberGame.submit).toHaveBeenCalled();
        });

        it("NumberGame.viewNumberGameBetList(matchId, betType, isLive, betTeam) should be called with correct parameters when '.forcast-number' fired click event", function () {
            affix("#Refresh-Content .forcast-number");
            var $forcastnumber = $(".forcast-number");
            $forcastnumber.attr("data-match-id", 1);
            $forcastnumber.attr("data-bet-type", 2);
            $forcastnumber.attr("data-is-live", true);
            $forcastnumber.attr("data-bet-team", 4);

            NumberGame.initEvent();

            spyOn(NumberGame, "viewNumberGameBetList");

            $forcastnumber.click();

            expect(NumberGame.viewNumberGameBetList).toHaveBeenCalledWith("1", "2", "true", "4");
        });
    });

    describe("submit()", function () {
        it("Site.blockUI() should be called", function () {
            spyOn(Site, "blockUI");

            NumberGame.submit();

            expect(Site.blockUI).toHaveBeenCalled();
        });

        it("submit function of jquery should be called", function () {
            affix("#NumberGameForm");
            spyOn($.fn, "submit");

            NumberGame.submit();

            expect($.fn.submit).toHaveBeenCalled();
        });
    });
});