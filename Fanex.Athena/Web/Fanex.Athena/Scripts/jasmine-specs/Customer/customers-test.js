describe("CustomerList", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            var productAccessData = {
                ProductList: [
                                {
                                    ProductID: 0,
                                    ProductName: 'Sportsbook',
                                    ProductCode: 'SB',
                                    AllowCommission: true,
                                    AllowWinloss: true
                                }]
            };
            var productAccessValue = JSON.stringify(productAccessData);
            affix("input#ProductAccess");
            $("#ProductAccess").val(productAccessValue);
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

        it("CustomerList.initPopup() should be called", function () {
            expect(CustomerList.initPopup).toHaveBeenCalled();
        });

        it("CustomerList.initAutoComplete() should be called", function () {
            expect(CustomerList.initAutoComplete).toHaveBeenCalled();
        });

        it("CustomerList.initGridColumnOption() should be called", function () {
            expect(CustomerList.initGridColumnOption).toHaveBeenCalled();
        });

        it("CustomerList.initOddGroupOption() should be called", function () {
            expect(CustomerList.initOddGroupOption).toHaveBeenCalled();
        });

        it("Site.addUserGuide() should be called with correct parameters", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "CustomerList", title: "CustomerList" });
        });
    });
});