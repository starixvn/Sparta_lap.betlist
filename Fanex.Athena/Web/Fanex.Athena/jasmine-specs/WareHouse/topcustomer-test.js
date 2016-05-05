describe("WareHouseTopCustomer", function () {
    describe("serializeParam()", function () {
        it("'#ActiveMin' and 'SportId' should have correct value", function () {
            var $activeMin = affix("input#ActiveMin");
            var $SportId = affix("input#SportId");
            affix("input#SelectReportBy[value=1]");
            affix("input#SelectSport[value=2]");

            WareHouseTopCustomer.serializeParam();

            expect($activeMin.val()).toBe("1");
            expect($SportId.val()).toBe("2");
        });
    });

    describe("initExcelExporting()", function () {
        it("WareHouseTopCustomer.serializeParam() should be called", function () {
            spyOn(WareHouseTopCustomer, "serializeParam");

            WareHouseTopCustomer.initExcelExporting();

            expect(WareHouseTopCustomer.serializeParam).toHaveBeenCalled();
        });

        it("'#IsExportExcel' should has value true", function () {
            var $isExportExcel = affix("input#IsExportExcel");

            WareHouseTopCustomer.initExcelExporting();

            expect($isExportExcel.val()).toBe("true");
        });

        it("submit function of jquery should be called", function () {
            spyOn($.fn, "submit");

            WareHouseTopCustomer.initExcelExporting();

            expect($.fn.submit).toHaveBeenCalled();
        });
    });

    describe("initialize()", function () {
        var $selectSport, $winlossTotalMin, $selectedReportBy, $note1, $note3, $exportExcel, $submitButton;
        beforeEach(function () {
            $selectSport = affix("input#SelectSport[value=1]");
            $winlossTotalMin = affix("input#WinLossTotalMin");
            $selectedReportBy = affix("input#SelectReportBy");
            $note1 = affix("input#note1");
            $note3 = affix("input#note3");
            $exportExcel = affix("#ExportToExcel");
            $submitButton = affix("#SubmitButton");

            spyOn(Site, "currencyConstraint");

            WareHouseTopCustomer.initialize();
        });

        it("UI elements should be displayed correctly", function () {
            expect($winlossTotalMin).toBeDisabled();
            expect($selectedReportBy).toBeDisabled();
            expect($note1).not.toBeVisible();
            expect($note3).toBeVisible();
        });

        describe("'#SelectSport' change event", function () {
            it("UI elements should be displayed correctly when value of '#SelectSport' equal 0", function () {
                $selectSport.val(0);
                $selectSport.change();

                expect($winlossTotalMin).not.toBeDisabled();
                expect($selectedReportBy).not.toBeDisabled();
                expect($note1).toBeVisible();
                expect($note3).not.toBeVisible();
            });

            it("UI elements should be displayed correctly when value of '#SelectSport' not equal 0", function () {
                $selectSport.val(8);
                $selectSport.change();

                expect($winlossTotalMin).toBeDisabled();
                expect($selectedReportBy).toBeDisabled();
                expect($note1).not.toBeVisible();
                expect($note3).toBeVisible();
            });
        });

        it("Site.currencyConstraint() should be called", function () {
            expect(Site.currencyConstraint).toHaveBeenCalled();
        });

        it("WareHouseTopCustomer.initExcelExporting() should be called when '#ExportToExcel' fired click event", function () {
            spyOn(WareHouseTopCustomer, "initExcelExporting");

            $exportExcel.click();

            expect(WareHouseTopCustomer.initExcelExporting).toHaveBeenCalled();
        });

        it("WareHouseTopCustomer.submitForm() should be called when '#SubmitButton' fired click event", function () {
            spyOn(WareHouseTopCustomer, "submitForm");

            $submitButton.click();

            expect(WareHouseTopCustomer.submitForm).toHaveBeenCalled();
        });
    });

    describe("submitForm()", function () {
        beforeEach(function () {
            spyOn(WareHouseTopCustomer, "checkDateRange").and.returnValue(true);
            spyOn(Site, "blockUI");
            spyOn(WareHouseTopCustomer, "serializeParam");
            spyOn($.fn, "submit");

            affix("input#BetCountMin[value=1]");
            affix("input#WinLossTotalMin[value=2]");
            affix("input#IsExportExcel");

            WareHouseTopCustomer.submitForm();
        });

        it("Site.blockUI() should be called", function () {
            expect(Site.blockUI).toHaveBeenCalled();
        });
        it("WareHouseTopCustomer.serializeParam() should be called", function () {
            expect(WareHouseTopCustomer.serializeParam).toHaveBeenCalled();
        });
        it("'#IsExportExcel' should has value false", function () {
            expect($("#IsExportExcel").val()).toBe("false");
        });
        it("submit function of jquery should be called", function () {
            expect($.fn.submit).toHaveBeenCalled();
        });
    });

    describe("checkDateRange()", function () {
        var $fromDate, $toDate;
        beforeEach(function () {
            spyOn(window, "alert");
            $fromDate = affix("input#FromDate");
            $toDate = affix("input#ToDate");
        });

        it("WareHouseTopCustomer.checkDateRange() function should be returned with value false when range over 3 months", function () {
            $fromDate.val("01/15/2015");
            $toDate.val("04/16/2015");

            var result = WareHouseTopCustomer.checkDateRange();

            expect(result).toBe(false);
        });

        it("WareHouseTopCustomer.checkDateRange() function should be returned with value true when range over 3 months", function () {
            $fromDate.val("02/15/2015");
            $toDate.val("04/16/2015");

            var result = WareHouseTopCustomer.checkDateRange();

            expect(result).toBe(true);
        });
    });
});