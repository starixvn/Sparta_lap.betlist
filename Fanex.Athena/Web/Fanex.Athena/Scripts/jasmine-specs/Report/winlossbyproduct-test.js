describe("WinLossByProduct", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(WinLossByProduct, "initEvent");
            spyOn(WinLossByProduct, "initSetting");
            spyOn($.fn, "autocomplete");
            spyOn(Site, "setConstraintMonthRange");
            spyOn(Site, "addFilterCollapseExpand");
            spyOn(Site, "addUserGuide");

            WinLossByProduct.initialize();
        });

        it("WinLossByProduct.initEvent() should be called", function () {
            expect(WinLossByProduct.initEvent).toHaveBeenCalled();
        });
        it("WinLossByProduct.initSetting() should be called", function () {
            expect(WinLossByProduct.initSetting).toHaveBeenCalled();
        });
        
        it("Site.setConstraintMonthRange(monthRange, url) should be called with correct parameter", function () {
            expect(Site.setConstraintMonthRange).toHaveBeenCalledWith(3, "api/report/WinLostByProduct");
        });
        it("Site.addFilterCollapseExpand() should be called", function () {
            expect(Site.addFilterCollapseExpand).toHaveBeenCalled();
        });
        it("Site.addUserGuide(options) should be called with correct parameter", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "WinLossByProduct", title: Site.getSingleResource("WinLossByProduct") });
        });
    });

    describe("initSetting()", function () {
        it("Properties of WinLossByProductOption should be correct", function () {
            affix("input#IsAllowedCustomerCommission[value=true]");
            affix("input#IsAllowedBACommission[value=false]");
            affix("input#IsAllowedRacing[value=False]");
            affix("input#drillDownLicenseeId[value=34]");

            WinLossByProduct.initSetting();

            expect(WinLossByProductOption.hasCustomerComm).toBe(true);
            expect(WinLossByProductOption.IsAllowBA).toBe(false);
            expect(WinLossByProductOption.IsAllowRB).toBe(false);
            expect(WinLossByProductOption.licenseeId).toBe(34);
        });
    });

    describe("initEvent()", function () {
        var $betlistLink, $submitButton, $levelLink;
        beforeEach(function () {
            $betlistLink = affix(".bet-list-link");
            $betlistLink.attr("data-drill-down", 2);

            $submitButton = affix("#SubmitButton");
            affix("input#IsExportExcel");

            affix("#level-link-container .level-link");
            $levelLink = $(".level-link");
            $levelLink.attr("data-level", 3);

            WinLossByProduct.initEvent();
        });

        it("WinLossByProduct.openBetlistPopUp(dataDrillDown) should be called with correct parameter when '.bet-list-link' fired click event", function () {
            spyOn(WinLossByProduct, "openBetlistPopUp");

            $betlistLink.click();

            expect(WinLossByProduct.openBetlistPopUp).toHaveBeenCalledWith("2");
        });

        it("WinLossByProduct.submit() should be called when '#SubmitButton' fired click event", function () {
            spyOn(WinLossByProduct, "submit");

            $submitButton.click();

            expect(WinLossByProduct.submit).toHaveBeenCalled();
        });

        it("'#IsExportExcel' value should be false", function () {
            spyOn(WinLossByProduct, "submit");

            $submitButton.click();

            expect($("#IsExportExcel").val()).toBe("false");
        });

        it("WinLossByProduct.goBack(level) should be called with correct parameter when '.level-link' fired click event", function () {
            spyOn(WinLossByProduct, "goBack");

            $levelLink.click();

            expect(WinLossByProduct.goBack).toHaveBeenCalledWith("3");
        });
    });

    describe("setExcelUrl(level, currencyId, custId, fdate, tdate, currencyName)", function () {
        it("WinLossByProductOption.urlExcelData should has correct data", function () {
            var expected = 'api/report/WinLostByProduct';

            WinLossByProduct.setExcelUrl(0, 2, 221, "Apr 15, 2016", "Apr 20, 2016", "UUS");

            expect(WinLossByProductOption.urlExcelData).toBe(expected);
        });       
    });

    describe("renderBreadCrumb(level)", function () {
        var $breadCumHolder;
        beforeEach(function () {
            $breadCumHolder = affix("input.breadCrumbHolder");
        });

        it("'.breadCrumbHolder' should be hidden when level equal 0", function () {
            WinLossByProduct.renderBreadCrumb(0);

            expect($breadCumHolder).not.toBeVisible();
        });

        it("'.breadCrumbHolder' should be hidden when level greater than 0", function () {
            WinLossByProduct.renderBreadCrumb(4);

            expect($breadCumHolder).toBeVisible();
        });
    });

    xdescribe("setReportTitle(currencyId, fromdate, todate, currencyName)", function () {
        it("'div.report-title' should be correct", function () {
            var $reportTitle = affix(".report-title");
            spyOn(jqGridAdditional, "getLicenseeWLNote").and.callFake(function (a,b) { return ""; });

            WinLossByProduct.setReportTitle(1, "Apr 10, 2015", "Apr 15, 2015", "USD");

            var expected = Site.getSingleResource('WinLossByProduct') + ", 4/10/2015 → 4/15/2015 - USD";

            expect($reportTitle).toHaveText(expected);
        });
    });

    describe("lastDateOfMonth(year, month)", function () {
        it("WinLossByProduct.lastDateOfMonth(year, month) should return correct value", function () {
            var dateExpected = new Date(2016, 9, 0);

            var result = WinLossByProduct.lastDateOfMonth(2016, 8);

            expect(result).toEqual(dateExpected);
        });
    });

    describe("ajaxComplete(level, response)", function () {
        beforeEach(function () {
            setFixtures("<input type='checkbox' name='ReportBy' checked='checked' value='test' />");

            spyOn(WinLossByProduct, "renderBreadCrumb");
            spyOn(WinLossByProduct, "bindData");

            WinLossByProduct.ajaxComplete(2, {});
        });

        it("WinLossByProduct.renderBreadCrumb(level) should be called with correct parameter", function () {
            expect(WinLossByProduct.renderBreadCrumb).toHaveBeenCalledWith(2);
        });

        it("WinLossByProduct.bindData(level, reportBy, response) should be called with correct parameter", function () {
            expect(WinLossByProduct.bindData).toHaveBeenCalledWith(2, "test", {});
        });
    });

    describe("submit()", function () {
        var request;
        beforeEach(function (done) {
            spyOn(Report, "setOriginalProduct");
            spyOn(Report, "ValidProducts").and.returnValue(true);
            spyOn(Site, "updateOriginalLicenseeOption");

            spyOn($.fn, "serializeObject").and.returnValue({ Username: "test" });
            spyOn($.fn, "dialog");

            spyOn(WinLossByProduct, "setReportTitle");
            spyOn(WinLossByProduct, "setExcelUrl");
            spyOn(WinLossByProduct, "ajaxComplete");

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("/api\/report\/WinLostByProduct/")
                .andReturn({
                    'status': 200,
                    'contentType': "application/x-www-form-urlencoded; charset=UTF-8",
                    'responseText': '{ProductOptions: ""}'
                });

            WinLossByProduct.submit();

            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        xit("ajax request should be called with correct url", function () {
            var expectedUrl = "api/report/WinLostByProduct";
            expect(request.url).toBe(expectedUrl);
        });

        it("Report.setOriginalProduct() should be called", function () {
            expect(Report.setOriginalProduct).toHaveBeenCalled();
        });
        it("Report.ValidProducts() should be called", function () {
            expect(Report.ValidProducts).toHaveBeenCalled();
        });
        it("Site.updateOriginalLicenseeOption() should be called", function () {
            expect(Site.updateOriginalLicenseeOption).toHaveBeenCalled();
        });
    });

    describe("bindData(level, reportBy, response)", function () {
        beforeEach(function () {
            spyOn(jqGridAdditional, "clearGrid");
            spyOn(WinLossByProductOption, "getGridOption").and.returnValue({});

            WinLossByProduct.bindData(0, "test", { Data: [{ Id: 0, Name: "foo" }], Total: [{ Id: 0, Name: "foo" }] });
        });

        it("jqGridAdditional.clearGrid(reportId) should be called", function () {
            expect(jqGridAdditional.clearGrid).toHaveBeenCalled();
        });
        it("WinLossByProductOption.getGridOption(level, reportBy) should be called", function () {
            expect(WinLossByProductOption.getGridOption).toHaveBeenCalled();
        });
    });
});