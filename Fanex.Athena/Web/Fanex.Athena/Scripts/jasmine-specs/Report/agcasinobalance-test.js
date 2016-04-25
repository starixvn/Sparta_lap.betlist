describe("AgCasinoBalance", function () {
    beforeEach(function () {
        $.fn.monPickr = function () { };
    });

    describe("initialize()", function () {
        it("initEvent() should be call when initialize() is called", function () {
            spyOn(AgCasinoBalance, 'initEvent');

            AgCasinoBalance.initialize();

            expect(AgCasinoBalance.initEvent).toHaveBeenCalled();
        });

        it("Site.addUserGuide() should be call when initialize() is called", function () {
            spyOn(Site, 'addUserGuide');

            AgCasinoBalance.initialize();

            expect(Site.addUserGuide).toHaveBeenCalled();
        });
    });

    describe("initEvent()", function () {
        it("#IsExportExcel should have value 'false' when #SubmitButton is clicked.", function () {
            var $submit = affix('#SubmitButton');
            var $excelField = affix('#IsExportExcel');

            AgCasinoBalance.initEvent();
            $submit.click();
            expect($excelField.val()).toBe(false);
        });

        it("#AgCasinoBalanceForm.submit() should be called if #ExportToExcel is clicked.", function () {
            var $export = affix('#ExportToExcel');
            var $mainform = affix('#AgCasinoBalanceForm');

            var spyEvent = spyOnEvent($mainform, 'submit');
            
            AgCasinoBalance.initEvent();
            $export.click();
            expect(spyEvent).toHaveBeenTriggered();
        });

        it("#AgCasinoBalance.goBack() should be called with correct param when .level-link is clicked.", function () {
            var LEVEL = 3;
            var $container = affix('#level-link-container');
            var $link = $container.affix('.level-link');
            $link.attr('data-level', LEVEL);
            spyOn(AgCasinoBalance, 'goBack');

            AgCasinoBalance.initEvent();
            $link.click();

            expect(AgCasinoBalance.goBack).toHaveBeenCalledWith(LEVEL);
        });
    });

    describe("setExcelUrl(level, currencyId, custId, fdate, tdate)", function () {
        it("All input fields should have correct value.", function () {
            var $currencyId = affix('input#DrillDownCurrencyId');
            var $customerId = affix('input#DrillDownCustomerId');
            var $fromDate = affix('input#DrillDownFromDate');
            var $toDate = affix('input#DrillDownToDate');
            var $level = affix('input#DrillDownLevel');
            var $pageIndex = affix('input#PageIndex');
            var $pageSize = affix('input#PageSize');

            var fromDate = new Date(2015, 1, 1);
            var toDate = new Date(2016, 1, 1);
            AgCasinoBalance.setExcelUrl(1, 2, 3, fromDate, toDate);

            expect($currencyId.val()).toBe('2');
            expect($customerId.val()).toBe('3');
            expect($level.val()).toBe('1');
            expect($fromDate.val().substr(0, 15)).toBe('02/01/2015');
            expect($toDate.val().substr(0, 15)).toBe('02/01/2016');
            expect($pageIndex.val()).toBe('1');
            expect($pageSize.val()).toBe('1000');
        });
    });

    describe("renderBreadCrumb(level)", function () {
        it(".breadCrumbHolder should be hide if param level is 0.", function () {
            var $breadCrumb = affix('.breadCrumbHolder');
            
            AgCasinoBalance.renderBreadCrumb(0);

            expect($breadCrumb.is(':visible')).toBe(false);
        });

        it(".level0 should show if param level > 0.", function () {
            var $level0 = affix('.level0');

            AgCasinoBalance.renderBreadCrumb(1);

            expect($level0.is(':visible')).toBe(true);
        });

        it(".level2 should be hide if param level = 1.", function () {
            var $level2 = affix('.level2');

            AgCasinoBalance.renderBreadCrumb(1);

            expect($level2.is(':visible')).toBe(false);
        });

        it(".level2 should show if param level = 2.", function () {
            var $level2 = affix('.level2');

            AgCasinoBalance.renderBreadCrumb(2);

            expect($level2.is(':visible')).toBe(true);
        });

        it(".level1 should have class 'level-link' if param level != 1.", function () {
            var $level2 = affix('.level1');

            AgCasinoBalance.renderBreadCrumb(2);

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

            AgCasinoBalance.setReportTitle('-1', new Date(), new Date());
            
            expect($title.html()).toBe('AGCasinoBalance, 1/1/2016 → 1/1/2016 - Viewin');
        });
    });

    describe("submit()", function () {
        var request;
        beforeEach(function (done) {
            affix('input#DrillDownLevel')
            jasmine.Ajax.install();

            jasmine.Ajax
                .stubRequest('api/report/AgCasinoBalance')
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ }'
                });

            spyOn(AgCasinoBalance, 'setExcelUrl');
            spyOn(AgCasinoBalance, 'ajaxComplete');

            AgCasinoBalance.submit();
            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("request.url should be 'api/report/AgCasinoBalance'", function () {
            expect(request.url).toBe('api/report/AgCasinoBalance');
        });

        it("#DrillDownLevel should have value 0.", function () {
            expect($('#DrillDownLevel').val()).toBe('0');
        });

        it("AgCasinoBalance.setExcelUrl() should be called with expected params.", function () {
            expect(AgCasinoBalance.setExcelUrl).toHaveBeenCalledWith(0, 0, 0, "", "");
        });

        it("AgCasinoBalance.ajaxComplete() should be called with expected params.", function () {
            expect(AgCasinoBalance.ajaxComplete).toHaveBeenCalledWith(0, jasmine.any(Object));
        });
    });

    describe("goBack(level)", function () {
        it("Correspondent functions should be called if level = 0.", function () {
            spyOn(AgCasinoBalance, 'renderBreadCrumb');
            spyOn(AgCasinoBalance, 'bindData');
            spyOn(AgCasinoBalance, 'setExcelUrl');
            spyOn(AgCasinoBalance, 'setReportTitle');

            AgCasinoBalance.goBack(0);

            expect(AgCasinoBalance.renderBreadCrumb).toHaveBeenCalled();
            expect(AgCasinoBalance.bindData).toHaveBeenCalled();
            expect(AgCasinoBalance.setExcelUrl).toHaveBeenCalled();
            expect(AgCasinoBalance.setReportTitle).toHaveBeenCalled();
        });

        it("#DrillDownLevel should have value 1 if level = 1.", function () {
            var $level = affix('#DrillDownLevel');
            spyOn(AgCasinoBalance, 'bindData');

            AgCasinoBalance.goBack(1);

            expect($level.val()).toBe('1');
        });
    });

    describe("bindTotal(level, totalData, summaryTotal)", function () {
        it("Some helper functions should be called.", function () {
            spyOn(jqGridAdditional, 'assignTotal');
            spyOn(Report, 'bindSummaryTotal');
            spyOn(jqGridAdditional, 'setGroupFooters');

            AgCasinoBalance.bindTotal();

            expect(jqGridAdditional.assignTotal).toHaveBeenCalled();
            expect(Report.bindSummaryTotal).toHaveBeenCalled();
            expect(jqGridAdditional.setGroupFooters).toHaveBeenCalled();
        });
    });

    describe("bindData(level, response)", function () {
        it(".paging should be hide if level = 0.", function () {
            var $pager = affix('.paging');
            AgCasinoBalance.bindData(0, { Data: [], Total: []});

            expect($pager.is(':visible')).toBe(false);
        });

        it("jqGridAdditional.resetLayout() should be called.", function () {
            spyOn(jqGridAdditional, 'resetLayout');
            AgCasinoBalance.bindData(0, { Data: [], Total: [] });

            expect(jqGridAdditional.resetLayout).toHaveBeenCalled();
        });
    });
});
