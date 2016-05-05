describe("AgCasinoBalanceOption", function () {

    describe("AgCasinoBalanceOption", function () {
        it("rowNum should be 10.", function () {
            expect(AgCasinoBalanceOption.rowNum).toBe(10);
        });

        it("totalColspan should be a correct array.", function () {
            expect(AgCasinoBalanceOption.totalColspan).toEqual([2, 2, 3]);
        });

        it("urlData should be a correct url.", function () {
            expect(AgCasinoBalanceOption.urlData).toBe('api/report/AgCasinoBalance');
        });

        it("gridOption.pager should be #ppdata.", function () {
            expect(AgCasinoBalanceOption.gridOption.pager).toBe('#ppdata');
        });
    });

    describe("getColName()", function () {
        it("gridOption.colNames should be correct.", function () {
            var columnNames = AgCasinoBalanceOption.getColName();

            expect(columnNames).toEqual(["#", "Date", "ExternalId", "SystemId", "Currency", "AGOpeningBalance", "TransferIn", "TransferOut", "Turnover", "TotalTransaction", "CustomerW_L", "LicenseeW_L", "AGClosingBalance", "CurrencyId", "CustomerId"]);
        });
    });

    describe("getColModel()", function () {
        it("columnModels.length should equal with columnNames.length.", function () {
            var columnNames = AgCasinoBalanceOption.getColName();
            var columnModels = AgCasinoBalanceOption.getColModel();

            expect(columnModels.length).toBe(columnNames.length);
        });
    });

    describe("getTotalColModel()", function () {
        it("totalColumnModels.length should equal with columnNames.length.", function () {
            var columnNames = AgCasinoBalanceOption.getColName();
            var totalColumnModels = AgCasinoBalanceOption.getTotalColModel();

            expect(totalColumnModels.length).toBe(columnNames.length);
        });
    });
});