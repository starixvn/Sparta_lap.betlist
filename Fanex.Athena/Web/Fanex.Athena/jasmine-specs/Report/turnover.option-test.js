describe("TurnoverOption", function () {
    describe("Default Option", function () {
        it("Initial values should be correct", function () {
            var sites = {
                "1": Site.getSingleResource("Wap"),
                "2": Site.getSingleResource("SmartPhone"),
                "3": Site.getSingleResource("Others")
            };

            expect(TurnoverOption.sites).toEqual(sites);
            expect(TurnoverOption.totalColspan).toEqual([3]);
            expect(TurnoverOption.urlData).toBe("reportapi/Turnover");
            expect(TurnoverOption.urlExcelData).toBe("TurnoverReport/Excel");
        });
    });

    describe("getColName()", function () {
        it("gridOption.colNames should be correct when TurnoverOption.hasCustomerComm equal true", function () {
            var colNameList = ['#', 'Day', "OpeningBalance", "Deposit", 'Withdrawal', 'Bonus', 'Adjustment',
                'Turnover', 'DrawGame', 'NetTurnover', 'PendingFunds',
                'WinLoss', 'Comm', 'Total', 'ClosingBalance',
                 'AffiliateTransfer', 'CasinoTransfer', 'Turnover', 'Comm', 'AmountDueTo'];
            TurnoverOption.hasCustomerComm = true;

            var result = TurnoverOption.getColName();
            expect(result).toEqual(colNameList);
        });

        it("gridOption.colNames should be correct when TurnoverOption.hasCustomerComm equal false", function () {
            var colNameList = ['#', 'Day', "OpeningBalance", "Deposit", 'Withdrawal', 'Bonus', 'Adjustment',
                'Turnover', 'DrawGame', 'NetTurnover', 'PendingFunds',
                'CustomerW_L', 'Comm', 'Total', 'ClosingBalance',
                 'AffiliateTransfer', 'CasinoTransfer', 'Turnover', 'Comm', 'AmountDueTo'];
            TurnoverOption.hasCustomerComm = false;

            var result = TurnoverOption.getColName();
            expect(result).toEqual(colNameList);
        })
    });

    describe("getColModel()", function () {
        it("Columns of model should show/hide correctly", function () {
            TurnoverOption.hasCustomerComm = false;
            TurnoverOption.isAllowBa = true;
            TurnoverOption.hasCasinoTransfer = false;

            var result = TurnoverOption.getColModel();

            expect(result[12].hidden).toBe(true);
            expect(result[13].hidden).toBe(true);
            expect(result[16].hidden).toBe(true);
            expect(result[17].hidden).toBe(false);
            expect(result[18].hidden).toBe(false);
            expect(result[19].hidden).toBe(false);
        });
    });

    describe("getGridOption()", function () {
        it("GridOption should have correct values", function () {
            TurnoverOption.hasCustomerComm = true;
            var colNameList = ['#', 'Day', "OpeningBalance", "Deposit", 'Withdrawal', 'Bonus', 'Adjustment',
                'Turnover', 'DrawGame', 'NetTurnover', 'PendingFunds',
                'WinLoss', 'Comm', 'Total', 'ClosingBalance',
                 'AffiliateTransfer', 'CasinoTransfer', 'Turnover', 'Comm', 'AmountDueTo'];

            var result = TurnoverOption.getGridOption();

            expect(result.colNames).toEqual(colNameList);
            expect(result.colModel.length).toBe(20);
        });
    });
});