describe("AgCasinoWinLoss", function () {
    beforeEach(function () {
        $.fn.monPickr = function () { };
    });

    describe("initialize()", function () {
        it("initEvent() should be call when initialize() is called", function () {
            spyOn(AgCasinoWinLoss, 'initEvent');

            AgCasinoWinLoss.initialize();

            expect(AgCasinoWinLoss.initEvent).toHaveBeenCalled();
        });

        it("Site.addUserGuide() should be call when initialize() is called with correct params.", function () {
            spyOn(Site, 'addUserGuide');

            AgCasinoWinLoss.initialize();

            expect(Site.addUserGuide).toHaveBeenCalledWith({
                resourceName: "AGCasinoWL",
                title: Site.getSingleResource("AGCasinoWinLoss")
            });
        });
    });

    describe("initEvent()", function () {
        it("#IsExportExcel should have value 'false' when #SubmitButton is clicked.", function () {
            var $submit = affix('#SubmitButton');
            var $excelField = affix('#IsExportExcel');

            AgCasinoWinLoss.initialize();
            $submit.click();
            expect($excelField.val()).toBe(false);
        });

        it("#AgCasinoWinLossForm.submit() should be called if #ExportToExcel is clicked.", function () {
            var $export = affix('#ExportToExcel');
            var $mainform = affix('#AgCasinoWinLossForm');

            var spyEvent = spyOnEvent($mainform, 'submit');
            
            AgCasinoWinLoss.initialize();
            $export.click();
            expect(spyEvent).toHaveBeenTriggered();
        });

        it("#AgCasinoWinLoss.goBack() should be called with correct param when .level-link is clicked.", function () {
            var LEVEL = 3;
            var $container = affix('#level-link-container');
            var $link = $container.affix('.level-link');
            $link.attr('data-level', LEVEL);
            spyOn(AgCasinoWinLoss, 'goBack');

            AgCasinoWinLoss.initialize();
            $link.click();

            expect(AgCasinoWinLoss.goBack).toHaveBeenCalledWith(LEVEL);
        });
    });

    describe("setExcelUrl(level, currencyId, custId, fdate, tdate)", function () {
        it("All input fields should have correct value.", function () {
            spyOn(Date.prototype, 'toString').and.callFake(function () {
                return '1/1/2016';
            });
            
            AgCasinoWinLoss.setExcelUrl(1, 2, 3, new Date(), new Date());

            expect(AgCasinoWinLossOption.urlExcelData).toBe('api/report/AgCasinoWL?level=1&currencyId=2&custId=3&fdate=1/1/2016&tdate=1/1/2016');
        });
    });

    describe("renderBreadCrumb(level)", function () {
        it(".breadCrumbHolder should be hide if param level is 0.", function () {
            var $breadCrumb = affix('.breadCrumbHolder');
            
            AgCasinoWinLoss.renderBreadCrumb(0);

            expect($breadCrumb.is(':visible')).toBe(false);
        });

        it(".level0 should show if param level > 0.", function () {
            var $level0 = affix('.level0');

            AgCasinoWinLoss.renderBreadCrumb(1);

            expect($level0.is(':visible')).toBe(true);
        });

        it(".level2 should be hide if param level = 1.", function () {
            var $level2 = affix('.level2');

            AgCasinoWinLoss.renderBreadCrumb(1);

            expect($level2.is(':visible')).toBe(false);
        });

        it(".level2 should show if param level = 2.", function () {
            var $level2 = affix('.level2');

            AgCasinoWinLoss.renderBreadCrumb(2);

            expect($level2.is(':visible')).toBe(true);
        });

        it(".level1 should have class 'level-link' if param level != 1.", function () {
            var $level2 = affix('.level1');

            AgCasinoWinLoss.renderBreadCrumb(2);

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

            AgCasinoWinLoss.setReportTitle('-1', new Date(), new Date());
            
            expect($title.html()).toBe('AGCasinoWinLoss, 1/1/2016 → 1/1/2016 - Viewin');
        });
    });

    describe("goBack(level)", function () {
        it("Correspondent functions should be called if level = 0.", function () {
            spyOn(AgCasinoWinLoss, 'renderBreadCrumb');
            spyOn(AgCasinoWinLoss, 'bindData');
            spyOn(AgCasinoWinLoss, 'setExcelUrl');
            spyOn(AgCasinoWinLoss, 'setReportTitle');

            AgCasinoWinLoss.goBack(0);

            expect(AgCasinoWinLoss.renderBreadCrumb).toHaveBeenCalled();
            expect(AgCasinoWinLoss.bindData).toHaveBeenCalled();
            expect(AgCasinoWinLoss.setExcelUrl).toHaveBeenCalled();
            expect(AgCasinoWinLoss.setReportTitle).toHaveBeenCalled();
        });

        it("AgCasinoWinLossOption.urlExcelData should be updated to correct value.", function () {
            expect(AgCasinoWinLossOption.urlExcelData).toBe('api/report/AgCasinoWL?level=1&currencyId=2&custId=3&fdate=1/1/2016&tdate=1/1/2016');
        });
    });

    describe("bindTotal(level, totalData, summaryTotal)", function () {
        it("Some helper functions should be called.", function () {
            spyOn(jqGridAdditional, 'assignTotal');
            spyOn(Report, 'bindSummaryTotal');
            spyOn(jqGridAdditional, 'setGroupFooters');

            AgCasinoWinLoss.bindTotal();

            expect(jqGridAdditional.assignTotal).toHaveBeenCalled();
            expect(Report.bindSummaryTotal).toHaveBeenCalled();
            expect(jqGridAdditional.setGroupFooters).toHaveBeenCalled();
        });
    });

    describe("bindData(level, response)", function () {
        it("jqGridAdditional.resetLayout() should be called.", function () {
            spyOn(jqGridAdditional, 'resetLayout');
            AgCasinoWinLoss.bindData(0, { Data: [], Total: [] });

            expect(jqGridAdditional.resetLayout).toHaveBeenCalled();
        });
    });
});
