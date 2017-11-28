describe("LicenseeWinLoss", function () {
    beforeEach(function () {
        $.fn.monPickr = function () { };
        spyOn(ProductListHelper, 'initProductList');
        spyOn($.fn, "dialog");
    });

    describe("initialize()", function () {
        it("LicenseeWinLossOption.licenseeId should be assigned data from #drillDownLicenseeId.", function () {
            var $input = affix('input#drillDownLicenseeId[value=1]');

            LicenseeWinLoss.initialize();

            expect(LicenseeWinLossOption.licenseeId).toBe(1);
        });

        it("initEvent() should be call when initialize() is called", function () {
            spyOn(LicenseeWinLoss, 'initEvent');

            LicenseeWinLoss.initialize();

            expect(LicenseeWinLoss.initEvent).toHaveBeenCalled();
        });

        it("Site.addUserGuide() should be call when initialize() is called with correct params.", function () {
            spyOn(Site, 'addUserGuide');

            LicenseeWinLoss.initialize();

            expect(Site.addUserGuide).toHaveBeenCalledWith({
                resourceName: "LicenseeWL",
                title: Site.getSingleResource("LicenseeWinLoss")
            });
        });
    });

    describe("initEvent()", function () {
        beforeEach(function () {
            $("body").off("click", "**");
        });

        it("#IsExportExcel should have value 'false' when #SubmitButton is clicked.", function () {
            var $submit = affix('#SubmitButton');
            var $excelField = affix('#IsExportExcel');

            LicenseeWinLoss.initEvent();
            $submit.click();
            expect($excelField.val()).toBe(false);
        });

        describe("Export excel", function () {
            var ajaxOption;

            beforeEach(function () {
                setFixtures('<div id="ExportToExcel"></div>');
                appendSetFixtures('<form id="licenseewinloss"><input type="hidden" name="CurrencyId" value="0" /></form>');

                spyOn(Downloader, "download");
                spyOn(Toast, "show");

                spyOn($, "ajax").and.callFake(function (option) {
                    option.beforeSend();
                    ajaxOption = option;
                });

                LicenseeWinLoss.initEvent();
            });

            it("#ExportToExcel click - ajax settings should be correct", function () {
                var postData = { CurrencyId: '0' };
                $('#ExportToExcel').click();

                expect(ajaxOption.type).toBe("POST");
                expect(ajaxOption.dataType).toBe("binary");
                expect(ajaxOption.responseType).toBe("blob");
                expect(ajaxOption.url).toBe(LicenseeWinLossOption.urlExcelData);
                expect(ajaxOption.data).toEqual(postData);
            });

            it("#ExportToExcel click - beforeSend - Toast.show should be called", function () {
                $('#ExportToExcel').click();

                expect(Toast.show).toHaveBeenCalled();
            });

            it("#ExportToExcel click - call success os ajax function - Downloader.download should be called with correct parameter", function () {
                $('#ExportToExcel').click();

                var fileName = "LicenseeWinLoss.xls";
                var response = "Binary Data";
                ajaxOption.success(response);

                expect(Downloader.download).toHaveBeenCalledWith(response, fileName);
            });
        });

        it("#LicenseeWinLoss.goBack() should be called with correct param when .level-link is clicked.", function () {
            var LEVEL = 3;
            var $container = affix('#level-link-container');
            var $link = $container.affix('.level-link');
            $link.attr('data-level', LEVEL);
            spyOn(LicenseeWinLoss, 'goBack');

            LicenseeWinLoss.initEvent();
            $link.click();

            expect(LicenseeWinLoss.goBack).toHaveBeenCalledWith(LEVEL);
        });

        it("LicenseeWinLoss.openBetlistPopUp(datadrilldown) should be called if .bet-list-link is clicked.", function () {
            var $betlistLink = affix(".bet-list-link");

            spyOn(LicenseeWinLoss, "openBetlistPopUp");
            LicenseeWinLoss.initEvent();
            $betlistLink.click();
            expect(LicenseeWinLoss.openBetlistPopUp).toHaveBeenCalled();
        });
    });

    describe("setExcelUrl(level, currencyId, custId, fdate, tdate, currencyName)", function () {
        it("All input fields should have correct value.", function () {
            spyOn(Date.prototype, 'toString').and.callFake(function () {
                return '1/1/2016';
            });
            
            LicenseeWinLoss.setExcelUrl(1, 2, 3, new Date(), new Date(), 'UUS');

            expect(LicenseeWinLossOption.urlExcelData).toBe('api/LicenseeWinLossReport/Index?level=1&currencyId=2&custId=3&fdate=1/1/2016&tdate=1/1/2016&currencyName=UUS');
        });
    });

    describe("renderBreadCrumb(level)", function () {
        it(".breadCrumbHolder should be hide if param level is 0.", function () {
            var $breadCrumb = affix('.breadCrumbHolder');
            
            LicenseeWinLoss.renderBreadCrumb(0);

            expect($breadCrumb.is(':visible')).toBe(false);
        });

        it(".level0 should show if param level > 0.", function () {
            var $level0 = affix('.level0');

            LicenseeWinLoss.renderBreadCrumb(1);

            expect($level0.is(':visible')).toBe(true);
        });

        it(".level2 should be hide if param level = 1.", function () {
            var $level2 = affix('.level2');

            LicenseeWinLoss.renderBreadCrumb(1);

            expect($level2.is(':visible')).toBe(false);
        });

        it(".level2 should show if param level = 2.", function () {
            var $level2 = affix('.level2');

            LicenseeWinLoss.renderBreadCrumb(2);

            expect($level2.is(':visible')).toBe(true);
        });

        it(".level1 should have class 'level-link' if param level != 1.", function () {
            var $level2 = affix('.level1');

            LicenseeWinLoss.renderBreadCrumb(2);

            expect($level2.hasClass('level-link')).toBe(true);
        });
    });

    describe("setReportTitle(currencyId, fromdate, todate)", function () {
        it("setReportTitle() should assign correct content to .report-title.", function () {
            var $title = affix('.report-title');
            spyOn(Date.prototype, 'toString').and.callFake(function() {
                return '1/1/2016';
            });

            LicenseeWinLoss.setReportTitle('-1', new Date(), new Date());
            
            expect($title.html()).toBe('LicenseeWinLoss, 1/1/2016 → 1/1/2016 - Viewin');
        });
    });

    describe("goBack(level)", function () {
        it("LicenseeWinLossOption.urlExcelData should be updated to correct value.", function () {
            expect(LicenseeWinLossOption.urlExcelData).toBe('api/LicenseeWinLossReport/Index?level=1&currencyId=2&custId=3&fdate=1/1/2016&tdate=1/1/2016&currencyName=UUS');
        });
    });
});
