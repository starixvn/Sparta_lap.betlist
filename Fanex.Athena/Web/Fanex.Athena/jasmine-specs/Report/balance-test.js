describe("Balance", function () {
    beforeEach(function () {
        spyOn(Site, 'getHistoryDate').and.callFake(function () {
            return new Date(2016, 1, 1);
        });
    });
    
    describe("initialize()", function () {
        it("Expected methods should be called when initialize() is called", function () {
            spyOn(Balance, 'initializeParam');
            spyOn(Balance, 'initEvent');

            Balance.initialize();

            expect(Balance.initializeParam).toHaveBeenCalled();
            expect(Balance.initEvent).toHaveBeenCalled();
        });
    });

    describe("initEvent()", function () {
        var $export, $excel;
        beforeEach(function () {
            $export = affix('#ExportToExcel');
            $excel = affix('#IsExportExcel');
        });

        it("Balance.initializeParam() should be called if #ExportToExcel is clicked.", function () {
            spyOn(Balance, 'initializeParam');

            Balance.initEvent();
            $export.click();

            expect(Balance.initializeParam).toHaveBeenCalled();
        });

        it("#IsExportExcel should have value 'false' if #ExportToExcel is clicked.", function () {
            Balance.initEvent();
            $export.click();

            expect($excel.val()).toBe(true);
        });

        it("#BalanceForm.submit() should be called if #ExportToExcel is clicked.", function () {
            var $form = affix('#BalanceForm');

            var spyEvent = spyOnEvent($form, 'submit');
            
            Balance.initEvent();
            $export.click();
            expect(spyEvent).toHaveBeenTriggered();
        });
    });

    describe("initializeParam()", function () {
        it("#FromDate, #ToDate should be assigned correct values.", function () {
            affix('input#Month[value=1]');
            affix('input#Year[value=2016]');
            var $fromDate = affix('input#FromDate');
            var $toDate = affix('input#ToDate');

            Balance.initializeParam();

            expect($fromDate.val()).toBe('Mon Feb 01 2016');
            expect($toDate.val()).toBe('Mon Feb 29 2016');
        });
    });

    describe("submit()", function () {
        it("#IsExportExcel should have value 'false'.", function () {
            var $excel = affix('#IsExportExcel');
            Balance.submit();

            expect($excel.val()).toBe(false);
        });

        it("#BalanceForm.submit() should be called if #ExportToExcel is clicked.", function () {
            var $form = affix('#BalanceForm');
            var spyEvent = spyOnEvent($form, 'submit');

            Balance.submit();

            expect(spyEvent).toHaveBeenTriggered();
        });
    });
});
