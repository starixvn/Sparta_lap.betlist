describe("WeeklyLiveMatchOption", function () {
    describe("Default Option", function () {
        it("Initial values should be correct", function () {
            expect(WeeklyLiveMatchOption.urlExcelData).toBe("api/WeeklyLiveMatchReport/index");
            expect(WeeklyLiveMatchOption.gridOption.rowNum).toBe(500);
            expect(WeeklyLiveMatchOption.gridOption.autowidth).toBe(false);
            expect(WeeklyLiveMatchOption.gridOption.forceFit).toBe(true);
            expect(WeeklyLiveMatchOption.gridOption.shrinkToFit).toBe(true);
            expect(WeeklyLiveMatchOption.gridOption.footerrow).toBe(true);
        });
    });

    describe("getGridOption(numOfWeek, weekHeaders)", function () {
        var gridOption;
        beforeEach(function () {
            gridOption = WeeklyLiveMatchOption.getGridOption(2, ["19 Jun - 19 Jun 2016", "20 Jun - 26 Jun 2016"]);
        });

        it("colNames should has length equal 4", function () {
            expect(gridOption.colNames.length).toBe(4);
        });
        it("colNames should has correct values", function () {
            var expected = ['#', 'Sport', '19 Jun - 19 Jun 2016', '20 Jun - 26 Jun 2016'];
            expect(gridOption.colNames).toEqual(expected);
        });

        it("colModel should has length equal 4", function () {
            expect(gridOption.colModel.length).toBe(4);
        });
        it("colModel should has correct values", function () {
            var colModels = gridOption.colModel;

            expect(colModels[0].name).toBe("Index");
            expect(colModels[1].name).toBe("SportName");
            expect(colModels[2].name).toBe("Week0");
            expect(colModels[3].name).toBe("Week1");
        });
    })
})