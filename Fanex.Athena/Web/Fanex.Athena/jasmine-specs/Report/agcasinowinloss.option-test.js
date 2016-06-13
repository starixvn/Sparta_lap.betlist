describe("AgCasinoWinLossOption", function () {

    describe("AgCasinoWinLossOption", function () {
        it("gridOption.rowNum should be 2000.", function () {
            expect(AgCasinoWinLossOption.gridOption.rowNum).toBe(2000);
        });

        it("totalColspan should be a correct array.", function () {
            expect(AgCasinoWinLossOption.totalColspan).toEqual([2, 2, 3]);
        });

        it("urlData should be a correct url.", function () {
            expect(AgCasinoWinLossOption.urlData).toBe('api/AgCasinoWinLossReport/Index');
        });

        it("gridOption.pager should be #ppdata.", function () {
            expect(AgCasinoWinLossOption.gridOption.pager).toBe('#ppdata');
        });
    });

    describe("getColName()", function () {
        it("gridOption.colNames should be correct.", function () {
            var columnNames = AgCasinoWinLossOption.getColName();

            expect(columnNames).toEqual(["#", "Date", "ExternalId", "SystemId", "Currency", "Turnover", "TotalTransaction", "CustomerW_L", "LicenseeW_L", "CurrencyId", "CustomerId"]);
        });
    });

    describe("getColModel()", function () {
        it("columnModels.length should equal with columnNames.length.", function () {
            var columnNames = AgCasinoWinLossOption.getColName();
            var columnModels = AgCasinoWinLossOption.getColModel();

            expect(columnModels.length).toBe(columnNames.length);
        });
    });

    describe("getTotalColModel()", function () {
        it("totalColumnModels.length should equal with columnNames.length.", function () {
            var columnNames = AgCasinoWinLossOption.getColName();
            var totalColumnModels = AgCasinoWinLossOption.getTotalColModel();

            expect(totalColumnModels.length).toBe(columnNames.length);
        });
    });
});