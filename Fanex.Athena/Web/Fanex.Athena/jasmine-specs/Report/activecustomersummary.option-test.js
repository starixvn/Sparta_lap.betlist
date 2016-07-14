describe("ActiveCustomerOption", function () {

    describe("ActiveCustomerOption", function () {
        it("ActiveCustomerOption.rowNum should be 50.", function () {
            expect(ActiveCustomerOption.rowNum).toBe(50);
        });

        it("ActiveCustomerOption.rowList should be a correct array.", function () {
            expect(ActiveCustomerOption.rowList).toEqual([50, 100, 200, 300, 400, 500, 1000, 2000]);
        });

        it("ActiveCustomerOption.totalColspan should be a correct array.", function () {
            expect(ActiveCustomerOption.totalColspan).toEqual([2, 2, 3]);
        });

        it("ActiveCustomerOption.urlData should be a correct url.", function () {
            expect(ActiveCustomerOption.urlData).toBe('api/ActiveCustomerSummaryReport/Index');
        });
    });

    describe("setGroupHeaderColumns(grid, productOptions)", function () {
        it("jqGrid() should be called with first param 'setGroupHeaders'.", function () {
            var methodName;
            spyOn($.fn, 'jqGrid').and.callFake(function (param) { methodName = param; })
            var $grid = affix('#grid');
            ActiveCustomerOption.setGroupHeaderColumns($grid, [{}]);

            expect(methodName).toBe('setGroupHeaders');
        });

        it("jqGrid() should be called with correct second param.", function () {
            var groupParams;
            spyOn($.fn, 'jqGrid').and.callFake(function (methodName, params) { groupParams = params; })
            var $grid = affix('#grid');
            ActiveCustomerOption.setGroupHeaderColumns($grid, [{ Code: 'SB' }]);

            expect(JSON.stringify(groupParams.groupHeaders)).toBe('[{"startColumnName":"CustomerCountSB","numberOfColumns":1,"titleText":"Product"}]');
        });
    });

    describe("getGridOption(productOptions)", function () {
        it("gridOption.colNames should be correct.", function () {
            var gridOption = ActiveCustomerOption.getGridOption([{ Name: 'SB', Code: 'SB', Status: false }]);

            expect(gridOption.colNames).toEqual(["Month","SB","Total"]);
        });

        it("gridOption.colNames.length should equals with gridOption.colModel.length.", function () {
            var gridOption = ActiveCustomerOption.getGridOption([{ Name: 'SB', Code: 'SB', Status: false }]);

            expect(gridOption.colNames.length).toBe(gridOption.colModel.length);
        });
    });
});