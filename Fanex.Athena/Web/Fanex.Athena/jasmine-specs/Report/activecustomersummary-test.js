describe("ActiveCustomer", function () {
    beforeEach(function () {
        spyOn(ProductListHelper, 'initProductList');
        spyOn($.fn, "dialog");
    });

    describe("initialize()", function () {
        it("initEvent() should be called when initialize() is called", function () {
            spyOn(ActiveCustomer, 'initEvent');

            ActiveCustomer.initialize();

            expect(ActiveCustomer.initEvent).toHaveBeenCalled();
        });
    });

    describe("initEvent()", function () {
        it("Site.setConstraintMonthRange() should be called with the first param = 3.", function () {
            var VALID_MONTH_RANGE = 3;
            spyOn(Site, 'setConstraintMonthRange');

            ActiveCustomer.initialize();

            expect(Site.setConstraintMonthRange).toHaveBeenCalledWith(VALID_MONTH_RANGE, ActiveCustomerOption.urlExcelData);
        });

        it("#IsExportExcel should have value 'false' when #SubmitButton is clicked.", function () {
            var $submit = affix('#SubmitButton');
            var $excelField = affix('#IsExportExcel');

            ActiveCustomer.initialize();
            $submit.click();
            expect($excelField.val()).toBe(false);
        });

        it("#mainform.submit() should be called if #ExportToExcel is clicked.", function () {
            var $export = affix('#ExportToExcel');
            var $mainform = affix('#mainform');

            var spyEvent = spyOnEvent($mainform, 'submit');
            
            ActiveCustomer.initialize();
            $export.click();
            expect(spyEvent).toHaveBeenTriggered();
        });
    });

    describe("setReportTitle()", function () {
        it("setReportTitle() should assign correct content to .report-title.", function () {
            var $title = affix('.report-title');
            
            var date = new Date();

            spyOn(Date.prototype, 'toString').and.callFake(function() {
                return '1/1/2016';
            });

            ActiveCustomer.setReportTitle('-1', new Date(), new Date());
            
            expect($title.html()).toBe('ActiveCustomerSummary, 1/1/2016 → 1/1/2016 - Viewin');
        });
    });
});
