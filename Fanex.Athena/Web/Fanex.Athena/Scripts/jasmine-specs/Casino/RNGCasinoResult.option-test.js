describe("RNGCasinoOption", function () {
    it("urlData value should be 'HandHistory/RNGCasinoData'", function () {
        expect(RNGCasinoOption.urlData).toEqual("HandHistory/RNGCasinoData");
    });

    describe("getColName()", function () {
        it("RNGCasinoOption.getColName() function should be return array with length is 15", function () {
            var colNameList = RNGCasinoOption.getColName();

            expect(colNameList.length).toEqual(15);
        });
    });

    describe("getColModel()", function () {
        it("RNGCasinoOption.getColModel() function should be return array with length is 15", function () {
            var colModelList = RNGCasinoOption.getColModel();

            expect(colModelList.length).toEqual(15);
        });
    });

    describe("getGridOption()", function () {
        it("gridOption.colNames should be array with length is 15", function () {
            var gridOption = RNGCasinoOption.getGridOption();
            expect(gridOption.colNames.length).toEqual(15);
        });

        it("gridOption.colModel should be array with length is 15", function () {
            var gridOption = RNGCasinoOption.getGridOption();
            expect(gridOption.colModel.length).toEqual(15);
        });

        it("rollingCommColumn.hidden value of gridOption.colModel should be false when hasCustomerComm is true", function () {
            RNGCasinoOption.hasCustomerComm = true;
            var gridOption = RNGCasinoOption.getGridOption();
            expect(gridOption.colModel[13].hidden).toBe(false);
        });

        it("rollingCommColumn.hidden value of gridOption.colModel should be true when hasCustomerComm is false", function () {
            RNGCasinoOption.hasCustomerComm = false;
            var gridOption = RNGCasinoOption.getGridOption();            
            expect(gridOption.colModel[13].hidden).toBe(true);
        });
    });
});