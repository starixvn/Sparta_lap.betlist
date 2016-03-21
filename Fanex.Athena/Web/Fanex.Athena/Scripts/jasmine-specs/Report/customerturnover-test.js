describe("CustTurnover", function () {
    describe("CustTurnover default properties.", function () {
        it("defaultUserName should be correct.", function () {
            expect(CustTurnover.defaultUserName).toBe('All');
        });

        it("sunGameLicensee should be correct.", function () {
            expect(CustTurnover.sunGameLicensee).toBe(8);
        });

        it("sportUrl should be correct.", function () {
            expect(CustTurnover.sportUrl).toBe('report/GetSportsAndProducts');
        });

        it("bettypeUrl should be correct.", function () {
            expect(CustTurnover.bettypeUrl).toBe('ReportAPI/GetBettypesBySport');
        });

        it("urlData should be correct.", function () {
            expect(CustTurnover.urlData).toBe('reportapi/CustomerTurnover');
        });

        it("urlExcelData should be correct.", function () {
            expect(CustTurnover.urlExcelData).toBe('report/CustomerTurnoverExcel');
        });

        it("excelParam should have correct properties.", function () {
            expect(CustTurnover.excelParam.reportby).toBe(1);
            expect(CustTurnover.excelParam.pageindex).toBe(1);
            expect(CustTurnover.excelParam.pagesize).toBe(50);
        });

        it("param.reportby should have correct properties.", function () {
            expect(CustTurnover.param.reportby).toBe(1);
            expect(CustTurnover.param.pageindex).toBe(1);
            expect(CustTurnover.param.pagesize).toBe(50);
        });

        it("param.pagingOption should have correct properties.", function () {
            expect(CustTurnover.pagingOption).toEqual({
                pageindex: 1,
                pagesize: 25,
                pagelist: [25, 50, 100, 500],
                totalrow: 1,
                elementcontainer: "#paging",
                method: "AJAX",
                ajaxfunction: "CustTurnover.ajaxPaging()",
                alwayshow: true
            });
        });
    });

    describe("initialize()", function () {
        it("CustTurnover.currentLicensee should be assigned value got from #LicenseeId.", function () {
            affix('input#LicenseeId[value=1]');

            CustTurnover.initialize();

            expect(CustTurnover.currentLicensee).toBe(1);
        });

        it("initEvent() should be call when initialize() is called.", function () {
            spyOn(CustTurnover, 'initEvent');

            CustTurnover.initialize();

            expect(CustTurnover.initEvent).toHaveBeenCalled();
        });

        it("initAutoComplete() should be call when initialize() is called.", function () {
            spyOn(CustTurnover, 'initAutoComplete');

            CustTurnover.initAutoComplete();

            expect(CustTurnover.initAutoComplete).toHaveBeenCalled();
        });

        it("Site.addUserGuide() should be call when initialize() is called with correct params.", function () {
            spyOn(Site, 'addUserGuide');

            CustTurnover.initialize();

            expect(Site.addUserGuide).toHaveBeenCalledWith({
                resourceName: "CustomerTurnover",
                title: Site.getSingleResource("CustomerTurnover")
            });
        });
    });

    describe("initEvent()", function () {

        it("initSportProduct() should be call when initEvent() is called.", function () {
            spyOn(CustTurnover, 'initSportProduct');

            CustTurnover.initEvent();

            expect(CustTurnover.initSportProduct).toHaveBeenCalled();
        });

        it("CustTurnoverOption.hasCustomerComm should be assigned value got from #IsAllowedCustomerCommission.", function () {
            affix('input#IsAllowedCustomerCommission[value="true"]');

            CustTurnover.initEvent();

            expect(CustTurnoverOption.hasCustomerComm).toBe(true);
        });

        describe("#SubmitButton, #ExportExcel", function () {
            beforeEach(function () {
                spyOn(CustTurnover, 'clearExcelExport');
                spyOn(CustTurnover, 'submitForm');
            });

            it("CustTurnover.clearExcelExport() should have been called when #SubmitButton is clicked.", function () {
                var $submit = affix('#SubmitButton');

                CustTurnover.initEvent();
                $submit.click();

                expect(CustTurnover.clearExcelExport).toHaveBeenCalled();
            });

            it("CustTurnover.param.pageindex should have value 1 when #SubmitButton is clicked.", function () {
                var $submit = affix('#SubmitButton');

                CustTurnover.initEvent();
                $submit.click();

                expect(CustTurnover.param.pageindex).toBe(1);
            });

            it("CustTurnover.submitForm() should have been called when #SubmitButton is clicked.", function () {
                var $submit = affix('#SubmitButton');

                CustTurnover.initEvent();
                $submit.click();

                expect(CustTurnover.submitForm).toHaveBeenCalled();
            });

            it("CustTurnover.clearExcelExport() should have been called when #ExportToExcel is clicked.", function () {
                var $excel = affix('#ExportToExcel');

                CustTurnover.initEvent();
                $excel.click();

                expect(CustTurnover.clearExcelExport).toHaveBeenCalled();
            });

            it("CustTurnover.isExportExcel should be true when #ExportToExcel is clicked.", function () {
                var $excel = affix('#ExportToExcel');

                CustTurnover.initEvent();
                $excel.click();

                expect(CustTurnover.isExportExcel).toBe(true);
            });

            it("CustTurnover.param.pageindex should have value -1 when #ExportToExcel is clicked.", function () {
                var $excel = affix('#ExportToExcel');

                CustTurnover.initEvent();
                $excel.click();

                expect(CustTurnover.param.pageindex).toBe(-1);
            });

            it("CustTurnover.submitForm() should have been called when #ExportToExcel is clicked.", function () {
                var $excel = affix('#ExportToExcel');

                CustTurnover.initEvent();
                $excel.click();

                expect(CustTurnover.submitForm).toHaveBeenCalled();
            });

        });

        it("setUsernameOnFocus() should be called if #Username is focused.", function () {
            var $input = affix('input#Username');
            spyOn(CustTurnover, 'setUsernameOnFocus');

            CustTurnover.initEvent();
            $input.focus();

            expect(CustTurnover.setUsernameOnFocus).toHaveBeenCalled();
        });

        it("$.fn.checkboxgroup should be called with correct selector.", function () {
            spyOn($.fn, 'checkboxgroup');

            CustTurnover.initEvent();

            expect($.fn.checkboxgroup.calls.mostRecent().object.selector).toBe('#TickerFilter input[type=checkbox]');
        });

        it("$.fn.keypress should be called with selector #Odds.", function () {
            spyOn($.fn, 'keypress');

            CustTurnover.initEvent();

            expect($.fn.keypress.calls.mostRecent().object.selector).toBe('#Odds');
        });
    });

    describe("initAutoComplete()", function () {
        it("$.fn.autocomplete() should be called with correct params.", function () {
            var $input = affix('input#Username[value=1]');
            
            spyOn($.fn, 'autocomplete');

            CustTurnover.initAutoComplete();

            expect($.fn.autocomplete).toHaveBeenCalledWith({
                serviceUrl: 'CustomerAPI/SearchCustomer?licenseeType=&currencyId=0',
                minChars: 2
            });
        });
    });


    describe("exportExcel()", function () {
        it("#excelparam should contains json string got from CustTurnover.excelParam.", function () {
            spyOn($.fn, 'submit');
            var $excel = affix('input#excelparam');

            CustTurnover.pagingOption.pageindex = 1;
            CustTurnover.pagingOption.pagesize = 10;
            CustTurnover.excelParam = {};

            CustTurnover.exportExcel();

            expect($excel.val()).toBe('{"pageindex":1,"pagesize":10}');
        });
    });

    describe("clearExcelExport()", function () {
        it("#excelparam should be cleared.", function () {
            spyOn($.fn, 'submit');
            var $excel = affix('input#excelparam[value="1234"]');

            CustTurnover.clearExcelExport();

            expect($excel.val()).toBe('');
        });
    });

    describe("setReportTitle()", function () {
        it("setReportTitle() should assign correct content to .report-title.", function () {
            var $title = affix('.report-title');
            CustTurnover.param = {
                username: 'abc',
                fromdate: new Date(),
                todate: new Date(),
                odds: 0
            };
            spyOn(Date.prototype, 'toString').and.callFake(function () {
                return '1/1/2016';
            });

            CustTurnover.setReportTitle('-1', new Date(), new Date());
            console.log($title.html());
            expect($title.html()).toBe('Customer Turnover, 1/1/2016 → 1/1/2016 - Currency:  - MinimumOdds: 0  - Username: abc');
        });
    });

    describe("setDefaultUsername()", function () {
        it("#Username should have class 'default-username' if it's value is empty.", function () {
            var $input = affix('input#Username');
            
            CustTurnover.setDefaultUsername();

            expect($input.hasClass('default-username')).toBe(true);
        });
    });
});
