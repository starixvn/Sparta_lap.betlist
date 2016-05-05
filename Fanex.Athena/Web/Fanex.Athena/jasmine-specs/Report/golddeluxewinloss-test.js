describe("GoldDeluxeWinLoss", function () {
    beforeEach(function () {
        $.fn.monPickr = function () { };
        spyOn($.fn, "dialog");
    });

    describe("initialize()", function () {
        it("GoldDeluxeWinLossOption.licenseeId should be assigned data from #drillDownLicenseeId.", function () {
            var $input = affix('input#drillDownLicenseeId[value=1]');

            GoldDeluxeWinLoss.initialize();

            expect(GoldDeluxeWinLossOption.licenseeId).toBe(1);
        });

        it("initEvent() should be call when initialize() is called", function () {
            spyOn(GoldDeluxeWinLoss, 'initEvent');

            GoldDeluxeWinLoss.initialize();

            expect(GoldDeluxeWinLoss.initEvent).toHaveBeenCalled();
        });

        it("Site.addUserGuide() should be call when initialize() is called with correct params.", function () {
            spyOn(Site, 'addUserGuide');

            GoldDeluxeWinLoss.initialize();

            expect(Site.addUserGuide).toHaveBeenCalledWith({
                resourceName: "GoldDeluxeWinLoss",
                title: Site.getSingleResource("GoldDeluxeWinLoss")
            });
        });
    });

    describe("initEvent()", function () {
        it("#IsExportExcel should have value 'false' when #SubmitButton is clicked.", function () {
            var $submit = affix('#SubmitButton');
            var $excelField = affix('#IsExportExcel');

            GoldDeluxeWinLoss.initEvent();
            $submit.click();
            expect($excelField.val()).toBe(false);
        });

        it("#GoldDeluxeWinLossForm.submit() should be called if #ExportToExcel is clicked.", function () {
            var $export = affix('#ExportToExcel');
            var $mainform = affix('#GoldDeluxeWinLossForm');

            var spyEvent = spyOnEvent($mainform, 'submit');
            
            GoldDeluxeWinLoss.initEvent();
            $export.click();
            expect(spyEvent).toHaveBeenTriggered();
        });

        it("#GoldDeluxeWinLoss.goBack() should be called with correct param when .level-link is clicked.", function () {
            var LEVEL = 3;
            var $container = affix('#level-link-container');
            var $link = $container.affix('.level-link');
            $link.attr('data-level', LEVEL);
            spyOn(GoldDeluxeWinLoss, 'goBack');

            GoldDeluxeWinLoss.initEvent();
            $link.click();

            expect(GoldDeluxeWinLoss.goBack).toHaveBeenCalledWith(LEVEL);
        });
    });

    describe("setExcelUrl(level, currencyId, custId, fdate, tdate)", function () {
        it("All input fields should have correct value.", function () {
            spyOn(Date.prototype, 'toString').and.callFake(function () {
                return '1/1/2016';
            });
            
            GoldDeluxeWinLoss.setExcelUrl(1, 2, 3, new Date(), new Date());

            expect(GoldDeluxeWinLossOption.urlExcelData).toBe('api/report/GoldDeluxeWinLoss?level=1&currencyId=2&custId=3&fdate=1/1/2016&tdate=1/1/2016');
        });
    });

    describe("renderBreadCrumb(level)", function () {
        it(".breadCrumbHolder should be hide if param level is 0.", function () {
            var $breadCrumb = affix('.breadCrumbHolder');
            
            GoldDeluxeWinLoss.renderBreadCrumb(0);

            expect($breadCrumb.is(':visible')).toBe(false);
        });

        it(".level0 should show if param level > 0.", function () {
            var $level0 = affix('.level0');

            GoldDeluxeWinLoss.renderBreadCrumb(1);

            expect($level0.is(':visible')).toBe(true);
        });

        it(".level2 should be hide if param level = 1.", function () {
            var $level2 = affix('.level2');

            GoldDeluxeWinLoss.renderBreadCrumb(1);

            expect($level2.is(':visible')).toBe(false);
        });

        it(".level2 should show if param level = 2.", function () {
            var $level2 = affix('.level2');

            GoldDeluxeWinLoss.renderBreadCrumb(2);

            expect($level2.is(':visible')).toBe(true);
        });

        it(".level1 should have class 'level-link' if param level != 1.", function () {
            var $level2 = affix('.level1');

            GoldDeluxeWinLoss.renderBreadCrumb(2);

            expect($level2.hasClass('level-link')).toBe(true);
        });
    });

    describe("setReportTitle(currencyId, fromdate, todate)", function () {
        it("setReportTitle() should assign correct content to .report-title.", function () {
            var $title = affix('.report-title');
            var date = new Date();
            spyOn(Date.prototype, 'toString').and.callFake(function() {
                return '1/1/2016';
            });

            GoldDeluxeWinLoss.setReportTitle('-1', new Date(), new Date());
            
            expect($title.html()).toBe('GoldDeluxeWinLoss, 1/1/2016 → 1/1/2016 - Viewin');
        });
    });

    describe("goBack(level)", function () {
        it("Correspondent functions should be called if level = 0.", function () {
            spyOn(GoldDeluxeWinLoss, 'renderBreadCrumb');
            spyOn(GoldDeluxeWinLoss, 'bindData');
            spyOn(GoldDeluxeWinLoss, 'setExcelUrl');
            spyOn(GoldDeluxeWinLoss, 'setReportTitle');

            GoldDeluxeWinLoss.ajaxComplete(0, {}); // initilize _submitData variable.
            GoldDeluxeWinLoss.goBack(0);

            expect(GoldDeluxeWinLoss.renderBreadCrumb).toHaveBeenCalled();
            expect(GoldDeluxeWinLoss.bindData).toHaveBeenCalled();
            expect(GoldDeluxeWinLoss.setExcelUrl).toHaveBeenCalled();
            expect(GoldDeluxeWinLoss.setReportTitle).toHaveBeenCalled();
        });

        it("GoldDeluxeWinLossOption.urlExcelData should be updated to correct value.", function () {
            expect(GoldDeluxeWinLossOption.urlExcelData).toBe('api/report/GoldDeluxeWinLoss?level=1&currencyId=2&custId=3&fdate=1/1/2016&tdate=1/1/2016');
        });
    });

    describe("bindTotal(level, totalData, summaryTotal)", function () {
        it("Some helper functions should be called.", function () {
            spyOn(jqGridAdditional, 'assignTotal');
            spyOn(Report, 'bindSummaryTotal');
            spyOn(jqGridAdditional, 'setGroupFooters');

            GoldDeluxeWinLoss.bindTotal();

            expect(jqGridAdditional.assignTotal).toHaveBeenCalled();
            expect(Report.bindSummaryTotal).toHaveBeenCalled();
            expect(jqGridAdditional.setGroupFooters).toHaveBeenCalled();
        });
    });

    describe("bindData(level, response)", function () {
        it("jqGridAdditional.resetLayout() should be called.", function () {
            spyOn(jqGridAdditional, 'resetLayout');
            GoldDeluxeWinLoss.bindData(0, { Data: [], Total: [] });

            expect(jqGridAdditional.resetLayout).toHaveBeenCalled();
        });
    });
});
