describe("CommonBetList", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn($, "parseJSON").and.returnValue({});
            spyOn(BetList, "loadData");
            affix('.content-filter');
            CommonBetList.initialize();
        });

        it("'.content-filter' should be removed", function () {
            expect($('.content-filter')).not.toExist();
        });

        it("BetList.loadData(params, url, callback) should be called", function () {
            expect(BetList.loadData).toHaveBeenCalled();
        });
    });
});