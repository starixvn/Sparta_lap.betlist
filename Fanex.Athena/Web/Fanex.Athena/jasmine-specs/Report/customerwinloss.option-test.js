describe("CustomerWinLossOption", function () {

    describe("CustomerWinLossOption", function () {
        it("totalColspan should be a correct value.", function () {
            expect(CustomerWinLossOption.totalColspan).toEqual(3);
        });

        it("totalColspanFakeLicensee should be a correct url.", function () {
            expect(CustomerWinLossOption.totalColspanFakeLicensee).toBe(2);
        });
    });

    describe("getGridOption()", function () {
        it("should return a correct colNames.", function () {
            CustomerWinLossOption.hasCustomerComm = false;

            var gridOption = CustomerWinLossOption.getGridOption();

            expect(gridOption.colNames).toEqual([
            "#",
            "ExternalId",
            "SystemId",
            "Turnover",
            "TotalTransaction",
            'CustomerW_L',
            'Comm',
            'Total',
            "EffectiveBettingValue",
            "LicenseeW_L",
            "BACommission",
            "RacingRevenue",
            "AmtDueToBA",
            "AmtDueToRacing"
            ]);
        });
    });
});