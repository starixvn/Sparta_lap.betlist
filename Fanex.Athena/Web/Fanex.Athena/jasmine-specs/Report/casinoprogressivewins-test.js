describe("CasinoProgressiveWins", function () {
    beforeEach(function () {
        spyOn($.fn, "dialog");
    });

    describe("initialize()", function () {
        it("initEvent() should be call when initialize() is called", function () {
            spyOn(CasinoProgressiveWins, 'initEvent');

            CasinoProgressiveWins.initialize();

            expect(CasinoProgressiveWins.initEvent).toHaveBeenCalled();
        });

        it("Site.addUserGuide() should be call when initialize() is called with correct params.", function () {
            spyOn(Site, 'addUserGuide');

            CasinoProgressiveWins.initialize();

            expect(Site.addUserGuide).toHaveBeenCalledWith({
                resourceName: "CasinoProgressiveWins",
                title: Site.getSingleResource("CasinoProgressiveWins")
            });
        });
    });

    describe("initEvent()", function () {
        it("#IsExportExcel should have value 'false' when #SubmitButton is clicked.", function () {
            var $submit = affix('#SubmitButton');
            var $excel = affix('#IsExportExcel');

            CasinoProgressiveWins.initEvent();
            $submit.click();
            expect($excel.val()).toBe(false);
        });

        it("#CasinoProgressiveWinsForm.submit() should be called if #ExportToExcel is clicked.", function () {
            var $export = affix('#ExportToExcel');
            var $mainform = affix('#mainForm');
            var spyEvent = spyOnEvent($mainform, 'submit');
            
            CasinoProgressiveWins.initEvent();
            $export.click();

            expect(spyEvent).toHaveBeenTriggered();
        });

        it("A dialog should be opened with with correct src param when .refno-link is clicked.", function () {
            var $container = affix('.report-content');
            var $link = $container.affix('.refno-link');

            $link.attr('data-gameid', 1);
            $link.attr('data-custid', 2);

            CasinoProgressiveWins.initEvent();
            $link.click();

            expect($.fn.dialog.calls.mostRecent().object.children().attr('src')).toBe('Report/MicrogamingResultPopup?gameid=1&custid=2');
        });
    });

    describe("setExcelUrl(currencyId, custId, fdate, tdate, currencyName)", function () {
        it("CasinoProgressiveWinsOption.urlExcelData should be assigned correct value.", function () {
            spyOn(Date.prototype, 'toString').and.callFake(function () {
                return '1/1/2016';
            });

            CasinoProgressiveWins.setExcelUrl(1, 2, new Date(), new Date(), 'USD');

            expect(CasinoProgressiveWinsOption.urlExcelData).toBe('api/CasinoProgressiveWinsReport/index?currencyId=1&custId=2&fdate=1/1/2016&tdate=1/1/2016&currencyName=USD');
        });
    });

    describe("bindData(level, response)", function () {
        it("jqGridAdditional.resetLayout() should be called.", function () {
            spyOn(jqGridAdditional, 'resetLayout');
            CasinoProgressiveWins.bindData(0, { Data: [], Total: [] });

            expect(jqGridAdditional.resetLayout).toHaveBeenCalled();
        });

        it("$.fn.jqGrid() should be called on #ReportGrid.", function () {
            var $grid = affix('#ReportGrid');
            
            spyOn($.fn, 'jqGrid');

            CasinoProgressiveWins.bindData(0, { Data: [], Total: [] });

            expect($.fn.jqGrid.calls.mostRecent().object.selector).toBe('#ReportGrid');
        });
    });
});
