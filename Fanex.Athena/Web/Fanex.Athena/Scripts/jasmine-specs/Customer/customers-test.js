describe("CustomerList", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            affix("input#ProductAccess[value={'ProductList': [{'ProductID': '0, 'ProductName': 'Sportsbook'}]}]");
            affix("input#AllowedGridColumns");
            spyOn(CustomerList, "initEvent");
            spyOn(CustomerList, "initPopup");
            spyOn(CustomerList, "initAutoComplete");
            spyOn(CustomerList, "initGridColumnOption");
            spyOn(CustomerList, "initOddGroupOption");
            spyOn(Site, "addUserGuide");

            CustomerList.initialize();
        });

        it("CustomerList.initEvent() should be called", function () {
            expect(CustomerList.initEvent).toHaveBeenCalled();
        });
    });
});