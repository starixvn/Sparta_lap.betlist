describe("ThirdPartyWinLossOption", function () {
    describe("Default Option", function () {
        it("Initial values should be correct", function () {
            expect(ThirdPartyWinLossOption.canDrilldown).toBe(true);
            expect(ThirdPartyWinLossOption.LICENSEE_12BET).toBe(1);
            expect(ThirdPartyWinLossOption.totalColspan).toEqual([2,2,3]);
            expect(ThirdPartyWinLossOption.urlData).toContain("api/report/ThirdPartyWinLoss");
            expect(ThirdPartyWinLossOption.urlExcelData).toContain("api/report/ThirdPartyWinLoss");
        });
    });

    describe("getColName()", function () {
        it("colNames value should be correct", function () {
            var expected = ['#', 'Date', 'ExternalId', "SystemId", "Currency",
                'Turnover', 'TotalTransaction', 'CustomerW_L', 'LicenseeW_L', 'CurrencyId', 'CustomerId'];

            var result = ThirdPartyWinLossOption.getColName();

            expect(result).toEqual(expected);
        });
    });

    describe("getColModel(level, canDrilldown)", function () {
        it("Columns of model should show/hide correctly", function () {
            var result = ThirdPartyWinLossOption.getColModel(1, true);

            var dateColumn = result[1];
            var externalIdColumn = result[2];
            var systemIdColumn = result[3];

            expect(dateColumn.hidden).toBe(true);
            expect(externalIdColumn.hidden).toBe(true);
            expect(systemIdColumn.hidden).toBe(false);
        });
    });

    describe("getGridOption(level, canDrilldown)", function () {
        it("colNames and colModel should have correct length", function () {
            var gridOption = ThirdPartyWinLossOption.getGridOption(1, true);

            expect(gridOption.colNames.length).toBe(11);
            expect(gridOption.colModel.length).toBe(11);
        });
    });

    describe("getTotalColModel(level)", function () {
        it("currencyColumn of colModel should has formatter equal null", function () {
            var colModel = ThirdPartyWinLossOption.getTotalColModel(1);
            var currencyColumn = colModel[4];

            expect(currencyColumn.formatter).toBe(null);
        });
    });
});