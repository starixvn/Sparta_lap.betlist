describe("GoldDeluxeWinLossOption", function () {

    describe("GoldDeluxeWinLossOption", function () {
        it("gridOption.rowNum should be correct.", function () {
            expect(GoldDeluxeWinLossOption.gridOption.rowNum).toBe(50000);
        });

        it("totalColspan should be a correct array.", function () {
            expect(GoldDeluxeWinLossOption.totalColspan).toEqual([2, 2, 3]);
        });

        it("urlData should be a correct url.", function () {
            expect(GoldDeluxeWinLossOption.urlData).toBe('api/report/GoldDeluxeWinLoss');
        });

        it("gridOption.pager should be #ppdata.", function () {
            expect(GoldDeluxeWinLossOption.gridOption.pager).toBe('#ppdata');
        });
    });

    describe("getColName()", function () {
        it("gridOption.colNames should be correct.", function () {
            var columnNames = GoldDeluxeWinLossOption.getColName();

            expect(columnNames).toEqual(["#", "Date", "ExternalId", "SystemId", "Currency", "Turnover", "TotalTransaction", "CustomerW_L", "LicenseeW_L", "CurrencyId", "CustomerId"]);
        });
    });

    describe("getColModel()", function () {
        it("columnModels.length should equal with columnNames.length.", function () {
            var columnNames = GoldDeluxeWinLossOption.getColName();
            var columnModels = GoldDeluxeWinLossOption.getColModel();

            expect(columnModels.length).toBe(columnNames.length);
        });
    });

    describe("getTotalColModel()", function () {
        it("totalColumnModels.length should equal with columnNames.length.", function () {
            var columnNames = GoldDeluxeWinLossOption.getColName();
            var totalColumnModels = GoldDeluxeWinLossOption.getTotalColModel();

            expect(totalColumnModels.length).toBe(columnNames.length);
        });
    });
});