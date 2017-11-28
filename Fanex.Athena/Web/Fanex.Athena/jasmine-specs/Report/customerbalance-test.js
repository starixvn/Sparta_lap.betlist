describe("CustomerBalance", function () {
    
    describe("initialize()", function () {
        it("initEvent() should be call when initialize() is called", function () {
            spyOn(ProductListHelper, 'initProductList');
            spyOn(CustomerBalance, 'initEvent');

            CustomerBalance.initialize();

            expect(CustomerBalance.initEvent).toHaveBeenCalled();
        });

        it("Site.addUserGuide() should be call when initialize() is called with correct params.", function () {
            spyOn(Site, 'addUserGuide');

            CustomerBalance.initialize();

            expect(Site.addUserGuide).toHaveBeenCalledWith({
                resourceName: "CustomerBalance",
                title: Site.getSingleResource("CustomerBalance")
            });
        });
    });

    describe("initEvent()", function () {
        var $submitButton, $exportToExcel, $isExportExcel;
        beforeEach(function () {
            $submitButton = affix("#SubmitButton");
            $exportToExcel = affix("#ExportToExcel");

            $isExportExcel = affix("input#IsExportExcel");
            spyOn(Report, "ValidProducts").and.returnValues(true);
            CustomerBalance.initEvent();
        });

        describe("'#SubmitButton' fired click event", function () {
            it("'#IsExportExcel' should has value equal empty", function () {
                spyOn(CustomerBalance, 'submit');
                $submitButton.click();

                expect($isExportExcel.val()).toBe("");
            });

            it("CustomerBalance.submit() should be called", function () {
                spyOn(CustomerBalance, "submit");

                $submitButton.click();

                expect(CustomerBalance.submit).toHaveBeenCalled();
            });
        });

        describe("'#ExportToExcel' fired click event", function () {
            it("'#IsExportExcel' should has value equal false", function () {
                $exportToExcel.click();

                expect($isExportExcel.val()).toBe("false");
            });

            it("submit function of jquery should be called", function () {
                spyOn($.fn, "submit");

                $exportToExcel.click();

                expect($.fn.submit).toHaveBeenCalled();
            });
        });
    });

    describe("setReportTitle(fromDate, toDate, currencyName)", function () {
        it("setReportTitle() should assign correct content to .report-title.", function () {
            var $title = affix('.report-title');
            
            CustomerBalance.setReportTitle("1/1/2016", "1/1/2017", "UUS");

            expect($title.html()).toBe('CustomerBalance, 1/1/2016 → 1/1/2017, Viewin');
        });
    });
});
