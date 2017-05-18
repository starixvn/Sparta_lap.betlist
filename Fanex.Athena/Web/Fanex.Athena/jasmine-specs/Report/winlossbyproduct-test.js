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
            expect(Site.setConstraintMonthRange).toHaveBeenCalledWith(3, "api/WinLossByProductReport/Index");
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

            $("body").off("click", "**");
            WinLossByProduct.initEvent();
        });

        it("WinLossByProduct.openBetlistPopUp(dataDrillDown) should be called with correct parameter when '.bet-list-link' fired click event", function () {
            //spyOn(LicenseeWinLoss, "openBetlistPopUp");
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
            var expected = 'api/WinLossByProductReport/Index';

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

    describe("setReportTitle(currencyId, fromdate, todate, currencyName)", function () {
        it("'div.report-title' should be correct", function () {
            var $reportTitle = affix(".report-title");

            WinLossByProduct.setReportTitle(1, new Date("Apr 10, 2015"), new Date("Apr 15, 2015"), "USD");

            expect($reportTitle.html()).toBe('WinLossByProduct, 4/10/2015 → 4/15/2015 - Viewin');
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
        var TESTURL = "testurl";
        var originalUrl = WinLossByProductOption.urlData;
        var request;

        beforeEach(function (done) {
            WinLossByProductOption.urlData = TESTURL;
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
                .stubRequest(Site.resolveClientUrl(TESTURL))
                .andReturn({
                    'status': 200,
                    'contentType': 'application/x-www-form-urlencoded; charset=UTF-8',
                    'responseText': '{"ProductOptions": []}'
                });
            WinLossByProduct.submit();

            request = jasmine.Ajax.requests.mostRecent();
            setTimeout(done, 10);
        });

        afterEach(function () {
            WinLossByProductOption.urlData = originalUrl;
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with correct url", function () {
            expect(request.url).toBe(TESTURL);
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
        it("WinLossByProduct.setReportTitle(currencyId, fromdate, todate, currencyName) should be called", function () {
            expect(WinLossByProduct.setReportTitle).toHaveBeenCalled();
        });
        it("WinLossByProduct.setExcelUrl(level, currencyId, custId, fdate, tdate, currencyName) should be called", function () {
            expect(WinLossByProduct.setExcelUrl).toHaveBeenCalled();
        });
        it("WinLossByProduct.ajaxComplete(level, response) should be called", function () {
            expect(WinLossByProduct.ajaxComplete).toHaveBeenCalled();
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