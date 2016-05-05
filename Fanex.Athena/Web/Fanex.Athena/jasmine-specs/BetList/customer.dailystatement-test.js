describe("Customer.DailyStatement", function () {
    describe("loadData(param, url)", function () {
        var request;

        beforeEach(function (done) {
            affix("#BetListGrid");
            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest('localhost-test')
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{"Main": [], "Total": { "SumWinLoss": 0, "SumLicenseeWl": 0 } }'
                });

            spyOn(CustomerDailyStatement, 'bindData');

            CustomerDailyStatement.loadData({}, 'localhost-test');
            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("should return sorting data stored in inputs", function () {
            expect(request.url).toBe('localhost-test');
        });

        it("bindData() should be called with expected param", function () {
            expect(CustomerDailyStatement.bindData).toHaveBeenCalledWith({ Main: [], Total: { SumWinLoss: 0, SumLicenseeWl: 0} });
        });
    });
});