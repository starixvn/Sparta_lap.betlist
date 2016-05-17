describe("MicroGamingWinLossOption", function () {
    describe("Default Option", function () {
        it("Initial values should be correct", function () {
            expect(MicroGamingWinLossOption.totalColspan).toEqual(2);
            expect(MicroGamingWinLossOption.urlData).toBe("api/report/MicrogamingWinloss");
            expect(MicroGamingWinLossOption.urlExcelData).toBe("api/report/MicrogamingWinloss");
        });
    });

    describe("getColName()", function () {
        it("MicroGamingWinLossOption.getColName() should be return correct value", function () {
            var expected = ['#', 'TypeName', 'Date', 'Currency', 'Turnover', 'TotalTransaction', 'CustomerW_L', 'ProgressiveContribution', 'ProgressivePayout', 'LicenseeW_L', 'LicenseeNetWin', 'AmountDueToMGC', 'ProgressiveFee'];

            var result = MicroGamingWinLossOption.getColName();

            expect(result).toEqual(expected);
        });
    });

    describe("getColModel()", function () {
        var $reportByDaily;
        beforeEach(function () {
            $reportByDaily = affix("input#reportbydaily[type=checkbox]");
        });

        it("dateColumn column should has format equal 'm/d/Y' when '#reportbydaily' was checked", function () {
            $reportByDaily.prop("checked", true);

            var dateColumn = MicroGamingWinLossOption.getColModel()[2];

            expect(dateColumn.formatoptions.newformat).toBe("m/d/Y");
        });

        it("dateColumn column should has format equal 'm/Y' when '#reportbydaily' was unchecked", function () {
            $reportByDaily.prop("checked", false);

            var dateColumn = MicroGamingWinLossOption.getColModel()[2];

            expect(dateColumn.formatoptions.newformat).toBe("m/Y");
        });
    });

    describe("getGridOption()", function () {
        beforeEach(function () {
            spyOn(MicroGamingWinLossOption, "getColName").and.returnValue(['#', 'Date']);
            spyOn(MicroGamingWinLossOption, "getColModel").and.returnValue([{Index: 0, Name: '#'}, {Index: 1, Name: 'Date'}]);
        });

        it("MicroGamingWinLossOption.gridOption.colNames should be ['#', 'Date']", function () {
            var gridOption = MicroGamingWinLossOption.getGridOption();

            expect(gridOption.colNames).toEqual(['#', 'Date']);
        });
        it("MicroGamingWinLossOption.gridOption.colModel should be [{Index: 0, Name: '#'}, {Index: 1, Name: 'Date'}]", function () {
            var gridOption = MicroGamingWinLossOption.getGridOption();

            expect(gridOption.colModel).toEqual([{ Index: 0, Name: '#' }, { Index: 1, Name: 'Date' }]);
        });
    });

    describe("getTotalColModel()", function () {
        it("MicroGamingWinLossOption.getTotalColModel() should be called", function () {
            spyOn(MicroGamingWinLossOption, "getColModel");

            MicroGamingWinLossOption.getTotalColModel();

            expect(MicroGamingWinLossOption.getColModel).toHaveBeenCalled();
        });
    });
});