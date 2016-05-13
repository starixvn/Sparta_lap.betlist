describe("WinLossByProductOption", function () {
    describe("Default Option", function () {
        it("Initial values should be correct", function () {
            expect(WinLossByProductOption.currencyIndex).toBe(4);
            expect(WinLossByProductOption.systemIdIndex).toBe(3);
            expect(WinLossByProductOption.externalIdIndex).toBe(2);
            expect(WinLossByProductOption.customerWlTotalIndex).toBe(10);

            expect(WinLossByProductOption.pageSize).toBe(50);
            expect(WinLossByProductOption.nonePagingSize).toBe(2000);
            expect(WinLossByProductOption.rowNum).toBe(50);
            expect(WinLossByProductOption.rowList).toEqual([50, 100, 200, 300, 400, 500, 1000, 2000]);
            expect(WinLossByProductOption.totalColspan).toEqual([1, 2, 3]);
            expect(WinLossByProductOption.urlData).toBe("api/WinLossByProductReport/WinLossByProduct");
            expect(WinLossByProductOption.urlExcelData).toBe("api/WinLossByProductReport/WinLossByProduct");
        });
    });

    describe("getColName()", function () {
        it("colNameList length should equal 16", function () {
            expect(WinLossByProductOption.getColName().length).toBe(16);
        });

        it("index of customerWlTotalIndex should has value 'Total' when 'hasCustomerComm' equal true", function () {
            WinLossByProductOption.hasCustomerComm = true;
            var colNameList = WinLossByProductOption.getColName();
            expect(colNameList[WinLossByProductOption.customerWlTotalIndex]).toBe("Total");
        });

        it("index of customerWlTotalIndex should has value 'CustomerW_L' when 'hasCustomerComm' equal false", function () {
            WinLossByProductOption.hasCustomerComm = false;
            var colNameList = WinLossByProductOption.getColName();
            expect(colNameList[WinLossByProductOption.customerWlTotalIndex]).toBe("CustomerW_L");
        });
    });

    describe("getColModel(level, reportBy)", function () {
        it("colModelList length should equal 16", function () {
            expect(WinLossByProductOption.getColModel(0, 2).length).toBe(16);
        });

        it("dateColumn of colModel should be hidden when level greater than 0", function () {
            var colModel = WinLossByProductOption.getColModel(1, 2);
            var dateColumn = colModel[1];
            expect(dateColumn.hidden).toBe(true);
        });

        it("dateColumn of colModel should be showed when level equal 0", function () {
            var colModel = WinLossByProductOption.getColModel(0, 2);
            var dateColumn = colModel[1];
            expect(dateColumn.hidden).toBe(false);
        });

        it("format option of dateColumn should be 'm/Y' when reportBy equal 2", function () {
            var colModel = WinLossByProductOption.getColModel(0, 2);
            var dateColumn = colModel[1];
            expect(dateColumn.formatoptions.newformat).toBe("m/Y");
        });

        it("format option of dateColumn should be 'm/d/Y' when reportBy not equal 2", function () {
            var colModel = WinLossByProductOption.getColModel(0, 1);
            var dateColumn = colModel[1];
            expect(dateColumn.formatoptions.newformat).toBe("m/d/Y");
        });
    });

    describe("getGridOption(level, reportBy)", function () {
        it("gridOption.grouping should be true when level equal 0", function () {
            var gridOption = WinLossByProductOption.getGridOption(0, 1);

            expect(gridOption.grouping).toBe(true);
        });

        it("gridOption.groupingView should be correct object when level equal 0", function () {
            var gridOption = WinLossByProductOption.getGridOption(0, 1);

            expect(gridOption.groupingView).toEqual({ groupField: ['Date'], groupColumnShow: [false] });
        });

        it("gridOption.grouping should be false when level not equal 0", function () {
            var gridOption = WinLossByProductOption.getGridOption(1, 1);

            expect(gridOption.grouping).toBe(false);
        });

        it("gridOption.groupingView should be null when level not equal 0", function () {
            var gridOption = WinLossByProductOption.getGridOption(1, 1);

            expect(gridOption.groupingView).toEqual(null);
        });
    });

    describe("getTotalColModel(level)", function () {
        it("formatter of currencyColumn should be null", function () {
            var colModel = WinLossByProductOption.getTotalColModel(0);

            expect(colModel[4].formatter).toBe(null);
        });
    });
});