describe("RunningBetList", function () {
    describe("initialize", function () {
        beforeEach(function () {
            spyOn(RunningBetList, "submit");
            spyOn(Site, "addUserGuide");
            RunningBetList.initialize();
        });

        it("RunningBetList.submit() should be called", function () {
            expect(RunningBetList.submit).toHaveBeenCalled();
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

        it("RunningBetList.submit() should be called when $('#SubmitButton') fired click event", function () {
            expect(RunningBetList.submit).toHaveBeenCalled();
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

        it("Site.setLicenseeOptionParam() should be called.", function () {
            expect(Site.setLicenseeOptionParam).toHaveBeenCalled();
        });

        it("BetList.loadData(params, url) should be called with correct parameters", function () {
            expect(BetList.loadData).toHaveBeenCalledWith({ bettype: 1 }, jasmine.any(String));
        });
    });

    describe("exportToExcel()", function () {
        beforeEach(function () {
            affix("#ExportToExcel");
            spyOn(Site, "setLicenseeOptionParam");
            spyOn(QueryString, "setParam");
            spyOn(BetList, "getSortParams");
            spyOn(Site, "redirect");

            RunningBetList.exportToExcel();
            $("#ExportToExcel").click();
        });

        it("Site.setLicenseeOptionParam() should be called.", function () {
            expect(Site.setLicenseeOptionParam).toHaveBeenCalled();
        });

        it("QueryString.setParam() should be called", function () {
            expect(QueryString.setParam).toHaveBeenCalled();
        });

        it("BetList.getSortParams() should be called", function () {
            expect(BetList.getSortParams).toHaveBeenCalled();
        });

        it("Site.redirect(url) should be called", function () {
            expect(Site.redirect).toHaveBeenCalled();
        });
    });
});