describe("AgCasinoBalanceOption", function () {

    describe("AgCasinoBalanceOption", function () {
        it("AgCasinoBalanceOption.rowNum should be 50.", function () {
            expect(AgCasinoBalanceOption.rowNum).toBe(50);
        });

        it("AgCasinoBalanceOption.rowList should be a correct array.", function () {
            expect(JSON.stringify(AgCasinoBalanceOption.rowList)).toBe(JSON.stringify([50, 100, 200, 300, 400, 500, 1000, 2000]));
        });

        it("AgCasinoBalanceOption.totalColspan should be a correct array.", function () {
            expect(JSON.stringify(AgCasinoBalanceOption.totalColspan)).toBe(JSON.stringify([2, 2, 3]));
        });

        it("AgCasinoBalanceOption.urlData should be a correct url.", function () {
            expect(AgCasinoBalanceOption.urlData).toBe('api/report/AgCasinoBalanceSummary');
        });
    });

    describe("setGroupHeaderColumns(grid, productOptions)", function () {
        it("jqGrid() should be called with first param 'setGroupHeaders'.", function () {
            var methodName;
            spyOn($.fn, 'jqGrid').and.callFake(function (param) { methodName = param; })
            var $grid = affix('#grid');
            AgCasinoBalanceOption.setGroupHeaderColumns($grid, [{}]);

            expect(methodName).toBe('setGroupHeaders');
        });

        it("jqGrid() should be called with correct second param.", function () {
            var groupParams;
            spyOn($.fn, 'jqGrid').and.callFake(function (methodName, params) { groupParams = params; })
            var $grid = affix('#grid');
            AgCasinoBalanceOption.setGroupHeaderColumns($grid, [{ Code: 'SB' }]);

            expect(JSON.stringify(groupParams.groupHeaders)).toBe('[{"startColumnName":"CustomerCountSB","numberOfColumns":1,"titleText":"Product"}]');
        });
    });

    describe("getGridOption(productOptions)", function () {
        it("gridOption.colNames should be correct.", function () {
            var gridOption = AgCasinoBalanceOption.getGridOption([{ Name: 'SB', Code: 'SB', Status: false }]);

            expect(JSON.stringify(gridOption.colNames)).toBe('["Month","SB","Total"]');
        });

        it("gridOption.colNames.length should equals with gridOption.colModel.length.", function () {
            var gridOption = AgCasinoBalanceOption.getGridOption([{ Name: 'SB', Code: 'SB', Status: false }]);

            expect(gridOption.colNames.length).toBe(gridOption.colModel.length);
        });
    });
});