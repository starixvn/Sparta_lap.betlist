describe("OutstandingBetList", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(OutstandingBetList, 'showHideProductsList');
            spyOn(OutstandingBetList, 'hiddenColList');
            spyOn(OutstandingBetList, 'initEvent');
            spyOn(OutstandingBetList, 'getParamsSubmit');
            spyOn(OutstandingBetList, 'submit');

            OutstandingBetList.initialize();
        });

        it("showHideProductsList() should be called", function () {
            expect(OutstandingBetList.showHideProductsList).toHaveBeenCalled();
        });

        it("hiddenColList() should be called", function () {
            expect(OutstandingBetList.hiddenColList).toHaveBeenCalled();
        });

        it("initEvent() should be called", function () {
            expect(OutstandingBetList.initEvent).toHaveBeenCalled();
        });

        it("getParamsSubmit() should be called", function () {
            expect(OutstandingBetList.getParamsSubmit).toHaveBeenCalled();
        });

        it("submit(params) should be called", function () {
            expect(OutstandingBetList.submit).toHaveBeenCalled();
        });
    });

    describe("initEvent()", function () {
        beforeEach(function () {
            affix("input#ViewOption");
            affix("button#SubmitButton");
            Report = jasmine.createSpyObj('Report', ['setOriginalProduct']);
            spyOn(OutstandingBetList, 'getParamsSubmit').and.callFake(function () { return {}; });
            spyOn(OutstandingBetList, 'submit');
            spyOn(Site, 'redirect');
            OutstandingBetList.initEvent();
        });
        it("OutstandingBetList.submit(params) should be called when fire click event on #SubmitButton", function () {

            $("#SubmitButton").click();
            expect(OutstandingBetList.submit).toHaveBeenCalled();
        });

        it("Site.redirect(url) should be called when fire change event on #ViewOption", function () {
            $("#ViewOption").change();
            expect(Site.redirect).toHaveBeenCalled();
        });
    });

    describe("submit()", function () {
        var params = {};
        it("OutstandingBetList.submitCompletedBetList(params) should be call BetList.loadData(params, url) with correct url", function () {
            spyOn(BetList, 'loadData');
            OutstandingBetList.submitCompletedBetList(params);
            expect(BetList.loadData).toHaveBeenCalledWith({}, 'api/betlist/OutstandingCompletedBetList');
        });

        it("OutstandingBetList.submitRunningBetList(params) should be call BetList.loadData(params, url) with correct url", function () {
            spyOn(BetList, 'loadData');
            OutstandingBetList.submitRunningBetList(params);
            expect(BetList.loadData).toHaveBeenCalledWith({}, 'api/betlist/OutstandingRunningBetList');
        });

        it("OutstandingBetList.submitDailyStatementBetList(params) should be call BetList.loadData(params, url) with correct url", function () {
            spyOn(CustomerDailyStatement, 'loadData');
            OutstandingBetList.submitDailyStatementBetList(params);
            expect(CustomerDailyStatement.loadData).toHaveBeenCalledWith({}, 'api/betlist/OutstandingDailyStatementBetList');
        });
    });
});