describe("WareHouseWinLoss", function () {
    describe("serializeParam", function () {
        it("'#ReportBy' should have correct value", function () {
            var $reportBy = affix("input#ReportBy");
            var $selectReportBy = affix("input#SelectReportBy[value=1]");

            WareHouseWinLoss.serializeParam();

            expect($selectReportBy.val()).toBe("1");
        });
    });

    describe("initExcelExporting()", function () {
        it("WareHouseWinLoss.serializeParam() should be called", function () {
            spyOn(WareHouseWinLoss, "serializeParam");

            WareHouseWinLoss.initExcelExporting();

            expect(WareHouseWinLoss.serializeParam).toHaveBeenCalled();
        });

        it("'#IsExportExcel' should has value true", function () {
            var $isExportExcel = affix("input#IsExportExcel");

            WareHouseWinLoss.initExcelExporting();

            expect($isExportExcel.val()).toBe("true");
        });

        it("submit function of jquery should be called", function () {
            spyOn($.fn, "submit");

            WareHouseWinLoss.initExcelExporting();

            expect($.fn.submit).toHaveBeenCalled();
        });
    });

    describe("initialize()", function () {
        var $submitButton, $exportExcel;
        beforeEach(function () {
            $submitButton = affix("#SubmitButton");
            $exportExcel = affix("#ExportToExcel");

            WareHouseWinLoss.initialize();
        });

        it("WareHouseWinLoss.submitForm() should be called when '#SubmitButton' fired click event", function () {
            spyOn(WareHouseWinLoss, "submitForm");

            $submitButton.click();

            expect(WareHouseWinLoss.submitForm).toHaveBeenCalled();
        });

        it("WareHouseWinLoss.initExcelExporting() should be called when '#ExportToExcel' fired click event", function () {
            spyOn(WareHouseWinLoss, "initExcelExporting");

            $exportExcel.click();

            expect(WareHouseWinLoss.initExcelExporting).toHaveBeenCalled();
        });
    });

    describe("submitForm()", function () {
        var $isExportExcel;
        beforeEach(function () {
            spyOn(WareHouseWinLoss, "checkDateRange").and.returnValue(true);
            spyOn(Site, "blockUI");
            spyOn(WareHouseWinLoss, "serializeParam");
            spyOn($.fn, "submit");

            affix("input#BetCountMin[value=1]");
            affix("input#WinLossTotalMin[value=2]");
            $isExportExcel = affix("input#IsExportExcel");

            WareHouseWinLoss.submitForm();
        });

        it("Site.blockUI() should be called", function () {
            expect(Site.blockUI).toHaveBeenCalled();
        });
        it("WareHouseWinLoss.serializeParam() should be called", function () {
            expect(WareHouseWinLoss.serializeParam).toHaveBeenCalled();
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

        it("WareHouseWinLoss.checkDateRange() function should be returned with value false when range over 3 months", function () {
            $fromDate.val("05/15/2015");
            $toDate.val("08/16/2015");

            var result = WareHouseWinLoss.checkDateRange();

            expect(result).toBe(false);
        });

        it("WareHouseWinLoss.checkDateRange() function should be returned with value true when range over 3 months", function () {
            $fromDate.val("05/15/2015");
            $toDate.val("07/16/2015");

            var result = WareHouseWinLoss.checkDateRange();

            expect(result).toBe(true);
        });
    });
});