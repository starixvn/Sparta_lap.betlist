describe("CasinoProgressiveWinsOption", function () {

    describe("CasinoProgressiveWinsOption", function () {
        it("gridOption.rowNum should be 5000.", function () {
            expect(CasinoProgressiveWinsOption.gridOption.rowNum).toBe(5000);
        });

        it("urlData should be a correct url.", function () {
            expect(CasinoProgressiveWinsOption.urlData).toBe('api/CasinoProgressiveWinsReport/index');
        });
    });

    describe("getColName()", function () {
        it("gridOption.colNames should be correct.", function () {
            var columnNames = CasinoProgressiveWinsOption.getColName();

            expect(columnNames).toEqual(['#', 'Date', 'ExternalId', 'SystemId', 'ReferenceNo', 'Choice', 'Amount']);
        });
    });

    describe("getColModel()", function () {
        it("columnModels.length should equal with columnNames.length.", function () {
            var columnNames = CasinoProgressiveWinsOption.getColName();
            var columnModels = CasinoProgressiveWinsOption.getColModel();

            expect(columnModels.length).toBe(columnNames.length);
        });
    });

    describe("getTotalColModel()", function () {
        it("totalColumnModels.length should equal with columnNames.length.", function () {
            var columnNames = CasinoProgressiveWinsOption.getColName();
            var totalColumnModels = CasinoProgressiveWinsOption.getTotalColModel();

            expect(totalColumnModels.length).toBe(columnNames.length);
        });
    });
});