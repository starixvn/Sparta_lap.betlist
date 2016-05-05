describe("CustTurnoverOption", function () {

    describe("CustTurnoverOption", function () {
        it("totalColspan should be a correct value.", function () {
            expect(CustTurnoverOption.totalColspan).toEqual(6);
        });

        it("reportBy should be a correct url.", function () {
            expect(CustTurnoverOption.reportBy).toBe(1);
        });
    });

    describe("getGridOption()", function () {
        it("should return a correct colNames.", function () {
            CustTurnoverOption.hasCustomerComm = false;

            var gridOption = CustTurnoverOption.getGridOption();

            expect(gridOption.colNames).toEqual(['#', 'Date', 'Username', 'Currency', 'Sport', 'BetType', 'EffectiveBettingValue', 'TotalTransaction', 'Turnover', 'TotalTurnover', 'Win/Loss', 'Comm', 'CustomerW_L']);
        });
    });
});