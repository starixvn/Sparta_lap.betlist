describe("RunningBetList", function () {
    beforeEach(function () {
        userGuideName = "User Guide Name test";
        popupTitle = "Popup Title test";
        spyOn(RunningBetList, "getUrlParams").and.returnValue("1");
    });
    describe("initialize", function () {
        beforeEach(function () {
            spyOn(RunningBetList, "submit");
            spyOn(Site, "addUserGuide");
            RunningBetList.initialize();
        });

        it("RunningBetList.getUrlParams(sParam) should be called with correct parameters", function () {
            expect(RunningBetList.getUrlParams).toHaveBeenCalledWith("bettype");
        });

        it("RunningBetList.submit(bettype) should be called with correct parameters", function () {
            expect(RunningBetList.submit).toHaveBeenCalledWith("1");
        });

        it("Site.addUserGuide(options) should be called", function () {
            expect(Site.addUserGuide).toHaveBeenCalled();
        });
    });

    describe("initEvent()", function () {
        beforeEach(function () {
            affix('button#SubmitButton');

            spyOn(RunningBetList, "submit");
            RunningBetList.initEvent();
            $("#SubmitButton").click();
        });

        it("RunningBetList.getUrlParams(bettype) should be called when $('#SubmitButton') fired click event", function () {
            expect(RunningBetList.getUrlParams).toHaveBeenCalledWith("bettype");
        });

        it("RunningBetList.submit(bettype) should be called when $('#SubmitButton') fired click event", function () {
            expect(RunningBetList.submit).toHaveBeenCalledWith("1");
        });
    });

    describe("submit(bettype)", function () {
        beforeEach(function () {
            spyOn(BetList, "initialize");
            spyOn(Site, "setLicenseeOptionParam").and.returnValue({ bettype: 1 });
            spyOn(BetList, "loadData");

            RunningBetList.submit(1);
        });

        it("BetList.initialize() should be called", function () {
            expect(BetList.initialize).toHaveBeenCalled();
        });

        it("Site.setLicenseeOptionParam(params) should be called with correct parameter", function () {
            expect(Site.setLicenseeOptionParam).toHaveBeenCalledWith({ bettype: 1 });
        });

        it("BetList.loadData(params, url) should be called with correct parameters", function () {
            expect(BetList.loadData).toHaveBeenCalledWith({ bettype: 1 }, 'api/betlist/runningbetlist');
        });
    });
});