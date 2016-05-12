describe("PlatformWinLoss", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(PlatformWinLoss, "initEvent");
            spyOn(PlatformWinLoss, "initSetting");
            spyOn($.fn, "autocomplete");
            spyOn(Site, "addFilterCollapseExpand");
            spyOn(Site, "addUserGuide");

            PlatformWinLoss.initialize();
        });

        it("PlatformWinLoss.initEvent() should be called", function () {
            expect(PlatformWinLoss.initEvent).toHaveBeenCalled();
        });
        it("PlatformWinLoss.initSetting() should be called", function () {
            expect(PlatformWinLoss.initSetting).toHaveBeenCalled();
        });
        
        it("Site.addFilterCollapseExpand() should be called", function () {
            expect(Site.addFilterCollapseExpand).toHaveBeenCalled();
        });
        it("Site.addUserGuide(options) should be called with correct parameter", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "PlatformWinLoss", title: Site.getSingleResource("PlatformWinLoss") });
        });
    });

    describe("initSetting()", function () {
        it("PlatformWinLossOption.hasCustomerComm should be correct", function () {
            affix("input#IsAllowedCustomerCommission[value=true]");
            
            PlatformWinLoss.initSetting();

            expect(PlatformWinLossOption.hasCustomerComm).toBe(true);
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
            PlatformWinLoss.initEvent();
        });

        it("PlatformWinLoss.openBetlistPopUp(dataDrillDown) should be called with correct parameter when '.bet-list-link' fired click event", function () {
            spyOn(PlatformWinLoss, "openBetlistPopUp");

            $betlistLink.click();

            expect(PlatformWinLoss.openBetlistPopUp).toHaveBeenCalledWith(2);
        });

        it("PlatformWinLoss.submit() should be called when '#SubmitButton' fired click event", function () {
            spyOn(PlatformWinLoss, "submit");

            $submitButton.click();

            expect(PlatformWinLoss.submit).toHaveBeenCalled();
        });

        it("PlatformWinLoss.goBack(level) should be called with correct parameter when '.level-link' fired click event", function () {
            spyOn(PlatformWinLoss, "goBack");

            $levelLink.click();

            expect(PlatformWinLoss.goBack).toHaveBeenCalledWith(3);
        });
    });

    describe("renderBreadCrumb(level)", function () {
        var $breadCumHolder;
        beforeEach(function () {
            $breadCumHolder = affix("input.breadCrumbHolder");
        });

        it("'.breadCrumbHolder' should be hidden when level equal 0", function () {
            PlatformWinLoss.renderBreadCrumb(0);

            expect($breadCumHolder).not.toBeVisible();
        });

        it("'.breadCrumbHolder' should be hidden when level greater than 0", function () {
            PlatformWinLoss.renderBreadCrumb(4);

            expect($breadCumHolder).toBeVisible();
        });
    });

    describe("lastDateOfMonth(year, month)", function () {
        it("PlatformWinLoss.lastDateOfMonth(year, month) should return correct value", function () {
            var dateExpected = new Date(2016, 9, 0);

            var result = PlatformWinLoss.lastDateOfMonth(2016, 8);

            expect(result).toEqual(dateExpected);
        });
    });

    describe("submit()", function () {
        var TESTURL = "testurl";
        var originalUrl = PlatformWinLossOption.urlData;
        var request;

        beforeEach(function (done) {
            PlatformWinLossOption.urlData = TESTURL;
            spyOn(Report, "setOriginalProduct");
            spyOn(Report, "ValidProducts").and.returnValue(true);
            spyOn(Site, "updateOriginalLicenseeOption");

            spyOn($.fn, "serializeObject").and.returnValue({ Username: "test" });
            spyOn($.fn, "dialog");

            spyOn(PlatformWinLoss, "setReportTitle");

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest(Site.resolveClientUrl(TESTURL))
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{"ProductOptions": [ { Code: 1} ]}'
                });
            PlatformWinLoss.submit();

            request = jasmine.Ajax.requests.mostRecent();
            setTimeout(done, 10);
        });

        afterEach(function () {
            PlatformWinLossOption.urlData = originalUrl;
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
    });

    describe("bindData(level, response, productOptions)", function () {
        beforeEach(function () {
            spyOn(jqGridAdditional, "clearGrid");
            spyOn(PlatformWinLossOption, "getGridOption").and.returnValue({});

            PlatformWinLoss.bindData(0, { Data: [{ Id: 0, Name: "foo" }], Total: [{ Id: 0, Name: "foo" }] }, []);
        });

        it("jqGridAdditional.clearGrid(reportId) should be called", function () {
            expect(jqGridAdditional.clearGrid).toHaveBeenCalled();
        });
        it("PlatformWinLossOption.getGridOption(level, reportBy) should be called", function () {
            expect(PlatformWinLossOption.getGridOption).toHaveBeenCalled();
        });
    });
});