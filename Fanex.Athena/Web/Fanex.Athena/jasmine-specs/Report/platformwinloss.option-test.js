describe("PlatformWinLossOption", function () {
    describe("Default Option", function () {
        it("Initial values should be correct", function () {
            expect(PlatformWinLossOption.pageSize).toBe(50);
            expect(PlatformWinLossOption.nonePagingSize).toBe(2000);
            expect(PlatformWinLossOption.rowNum).toBe(50);
            expect(PlatformWinLossOption.rowList).toEqual([50, 100, 200, 300, 400, 500, 1000, 2000]);
            expect(PlatformWinLossOption.totalColspan).toEqual([0, 4]);
            expect(PlatformWinLossOption.urlData).toBe("api/PlatformWinLossReport/Index");
            expect(PlatformWinLossOption.urlExcelData).toBe("api/PlatformWinLossReport/Index");
        });
    });

    describe("getColName()", function () {
        it("colNameList length should be correct", function () {
            expect(PlatformWinLossOption.getColName().length).toBe(19);
        });
    });

    describe("getColModel()", function () {
        it("colModelList length should equal 19", function () {
            expect(PlatformWinLossOption.getColModel().length).toBe(19);
        });
    });

    describe("getTotalColModel()", function () {
        it("should have the same returned value with getColModel().", function () {
            var colModel = PlatformWinLossOption.getColModel();
            var totalColModel = PlatformWinLossOption.getTotalColModel();
            expect(colModel).toBe(totalColModel);
        });
    });
});