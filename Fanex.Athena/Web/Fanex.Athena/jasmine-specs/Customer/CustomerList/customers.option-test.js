describe("CustomersOption", function () {
    it("CustomersOption.urlData property should has value is 'Customer/CustomerList'", function () {
        expect(CustomersOption.urlData).toBe("Customer/CustomerList");
    });
    it("CustomersOption.urlExcelData property should has value is 'Customer/ExportExcel'", function () {
        expect(CustomersOption.urlExcelData).toBe("Customer/ExportExcel");
    });

    describe("getGridOption(productAccess, isAllowedCustomerCommission)", function () {
        var productAccessData;
        var isAllowComission = true;
        beforeEach(function () {
            productAccessData = {
                IsAllowSB: true,
                IsAllowRB: false,              
                ProductList: [
                {
                    ProductID: 0,
                    ProductName: 'Sportsbook',
                    ProductCode: 'SB',
                    AllowCommission: true,
                    AllowWinloss: true
                },
                {
                    ProductID: 1,
                    ProductName: 'Racing',
                    ProductCode: 'RB',
                    AllowCommission: true,
                    AllowWinloss: true
                }]
            };

            CustomersOption.getGridOption(productAccessData, isAllowComission);
        });

        it("CustomersOption.gridOption.colNames property should has length is 12", function () {
            expect(CustomersOption.gridOption.colNames.length).toBe(12);
        });

        it("CustomersOption.gridOption.colNames property should has last item value is 'SB'", function () {
            var lastIndex = CustomersOption.gridOption.colNames.length - 1;
            expect(CustomersOption.gridOption.colNames[lastIndex]).toBe("SB");
        });

        it("CustomersOption.gridOption.colModel property should has length is 12", function () {
            expect(CustomersOption.gridOption.colModel.length).toBe(12);
        });
        it("CustomersOption.gridOption.colModel property should has last item's name property is SBComm", function () {
            var lastIndex = CustomersOption.gridOption.colModel.length - 1;
            expect(CustomersOption.gridOption.colModel[lastIndex].name).toBe("SBComm");
        });
    });
});