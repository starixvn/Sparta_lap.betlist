describe("CustomerOption", function () {

    describe("gridOption", function () {
        it("gridOption.rowNum should be 50", function () {
            expect(CustomerOption.gridOption.rowNum).toBe(50);
        });

        it("gridOption.rowList should be a correct array", function () {
            expect(JSON.stringify(CustomerOption.gridOption.rowList)).toBe(JSON.stringify([50, 100, 200, 300, 400, 500, 1000, 2000]));
        });
    });

    describe("columnModel", function () {
        it("columnModel.length should be 7", function () {
            expect(CustomerOption.columnModel.length).toBe(7);
        });

        it("first column name should be 'Index'", function () {
            expect(CustomerOption.columnModel[0].name).toBe('Index');
        });

        it("last column name should be 'Balance'", function () {
            expect(CustomerOption.columnModel[6].name).toBe('Balance');
        });
    });

    it("columnNames should be a correct array", function () {
        expect(CustomerOption.columnNames).toEqual(['#', 'Date', 'Remark', 'Turnover', 'CreditDebit', 'Commission', 'Balance']);
    });

    it("maxHeight should be 720", function () {
        expect(CustomerOption.maxHeight).toBe(720);
    });
});