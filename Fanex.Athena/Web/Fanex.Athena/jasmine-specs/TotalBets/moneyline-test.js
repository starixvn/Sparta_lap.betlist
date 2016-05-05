describe("MoneyLine", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(Totalbets, "initialize");
            spyOn(MoneyLine, "initEvent");
            spyOn(Site, "addUserGuide");

            MoneyLine.initialize();
        });

        it("Totalbets.initialize() should be called", function () {
            expect(Totalbets.initialize).toHaveBeenCalled();
        });
        it("MoneyLine.initEvent() should be called", function () {
            expect(MoneyLine.initEvent).toHaveBeenCalled();
        });
        it("Site.addUserGuide() should be called with correct parameters", function () {
            var title = Site.getSingleResource("TotalBets") + " - " + Site.getSingleResource("MoneyLine");
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "TotalBetsMoneyLine", title: title });
        });
    });

    describe("initEvent()", function () {
        it("MoneyLine.submit() should be called when '#SubmitButton' fired click event", function () {
            var $submitButton = affix("#SubmitButton");
            MoneyLine.initEvent();

            spyOn(MoneyLine, "submit");

            $submitButton.click();

            expect(MoneyLine.submit).toHaveBeenCalled();
        });
    });

    describe("submit()", function () {
        it("Site.blockUI() should be called", function () {
            spyOn(Site, "blockUI");

            MoneyLine.submit();

            expect(Site.blockUI).toHaveBeenCalled();
        });

        it("submit function of jquery should be called", function () {
            affix("#MoneyLineForm");
            spyOn($.fn, "submit");

            MoneyLine.submit();

            expect($.fn.submit).toHaveBeenCalled();
        });
    });
});