describe("Totalbets", function () {
    describe("initEvent()", function () {
        var $forcastnumber;
        beforeEach(function () {
            affix("#Refresh-Content .forcast-number");
            $forcastnumber = $(".forcast-number");

            spyOn(RefreshCountDown, "initialize");
            spyOn($.fn, "ajaxComplete");

            Totalbets.initEvent();
        });

        it("RefreshCountDown.initialize() should be called", function () {
            expect(RefreshCountDown.initialize).toHaveBeenCalled();
        });

        it("Totalbets.viewRunningBetListMatch(matchId, betType) should be called with correct parameters when '.forcast-number' fired click event", function () {
            $forcastnumber.attr("data-match-id", 1);
            $forcastnumber.attr("data-bet-type", 2);

            spyOn(Totalbets, "viewRunningBetListMatch");

            $forcastnumber.click();

            expect(Totalbets.viewRunningBetListMatch).toHaveBeenCalledWith("1", "2");
        });

        it("ajaxComplete function of jquery should be called", function () {
            expect($.fn.ajaxComplete).toHaveBeenCalled();
        });
    });

    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(Site, "unBlockUI");
            spyOn(Totalbets, "initEvent");
            spyOn(Totalbets, "resizeGrid");

            Totalbets.initialize();
        });

        it("Site.unBlockUI() should be called", function () {
            expect(Site.unBlockUI).toHaveBeenCalled();
        });
        it("Totalbets.initEvent() should be called", function () {
            expect(Totalbets.initEvent).toHaveBeenCalled();
        });
        it("Totalbets.resizeGrid() should be called", function () {
            expect(Totalbets.resizeGrid).toHaveBeenCalled();
        });
    });
});