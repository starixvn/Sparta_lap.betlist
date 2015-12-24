describe("Customer.DailyStatement", function () {
    describe("loadData(param, url)", function () {
        var request;
        beforeEach(function () {
            affix("#BetListGrid");
            spyOn(CustomerDailyStatement, 'loadData');
            CustomerDailyStatement.loadData('', '');
        });

        it("loadData(param, url) should be called", function () {
            expect(CustomerDailyStatement.loadData).toHaveBeenCalled();
        });
    });
});