define(['jquery', 'common/site', 'components/pluginhelper/daterangepicker', 'pages/report/winlossbyproduct/winlossbyproduct.option', 'pages/report/winlossbyproduct/winlossbyproduct',
    'pages/report/report', 'lib/jqgrid/jquery.jqgrid.additional', 'common/popup', 'components/pluginhelper/multiselect'],
        function ($, Site, DatePicker, WinLossByProductOption, WinLossByProduct, Report, jqGridAdditional, Popup, MultiSelect) {
            describe("WinLossByProduct", function () {
                beforeAll(function () {
                    spyOn(Site, "getSingleResource").and.callFake(function (resource) { return resource; });
                    spyOn(Site, "setConstraintMonthRange");
                    spyOn(Site, "addUserGuide");
                    spyOn(Site, "initialize");
                    spyOn(MultiSelect, "initProductList");
                    spyOn(DatePicker, "initDateRangePicker");
                    spyOn(Site, "resolveClientUrl").and.callFake(function (url) {
                        return url;
                    });
                });

                describe("initialize()", function () {
                    beforeAll(function () {
                        spyOn(WinLossByProduct, "initEvent");
                        spyOn(WinLossByProduct, "initSetting");
                    });

                    beforeEach(function () {
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
                    it("Site.addUserGuide(options) should be called with correct parameter", function () {
                        expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "WinLossByProduct", title: Site.getSingleResource("WinLossByProduct") });
                    });
                });

                describe("initSetting()", function () {
                    it("Properties of WinLossByProductOption should be correct", function () {
                        setFixtures("<input id='IsAllowedCustomerCommission' value='true' />");
                        appendSetFixtures("<input id='IsAllowedBACommission' value='false' />");
                        appendSetFixtures("<input id='IsAllowedRacing' value='False' />");
                        appendSetFixtures("<input id='drillDownLicenseeId' value='34' />");

                        WinLossByProduct.initSetting();

                        expect(WinLossByProductOption.hasCustomerComm).toBe(true);
                        expect(WinLossByProductOption.IsAllowBA).toBe(false);
                        expect(WinLossByProductOption.IsAllowRB).toBe(false);
                        expect(WinLossByProductOption.licenseeId).toBe(34);
                    });
                });

                describe("initEvent()", function () {
                    beforeEach(function () {
                        setFixtures("<div class='bet-list-link' data-drill-down='2' />");
                        

                        appendSetFixtures("<div id='SubmitButton' />");
                        appendSetFixtures("<input id='IsExportExcel' />");

                        appendSetFixtures("<div id='level-link-container'><div class='level-link' data-level='1' /></div>");

                        $("body").off("click", "**");
                        WinLossByProduct.initEvent();
                    });

                    it("WinLossByProduct.openBetlistPopUp(dataDrillDown) should be called with correct parameter when '.bet-list-link' fired click event", function () {
                        spyOn(WinLossByProduct, "openBetlistPopUp");

                        $(".bet-list-link").click();

                        expect(WinLossByProduct.openBetlistPopUp).toHaveBeenCalledWith("2");
                    });

                    it("WinLossByProduct.submit() should be called when '#SubmitButton' fired click event", function () {
                        spyOn(WinLossByProduct, "submit");

                        $("#SubmitButton").click();

                        expect(WinLossByProduct.submit).toHaveBeenCalled();
                    });

                    it("'#IsExportExcel' value should be false", function () {
                        spyOn(WinLossByProduct, "submit");

                        $("#SubmitButton").click();

                        expect($("#IsExportExcel").val()).toBe("false");
                    });

                    it("WinLossByProduct.goBack(level) should be called with correct parameter when '.level-link' fired click event", function () {
                        spyOn(WinLossByProduct, "goBack");

                        $(".level-link").click();

                        expect(WinLossByProduct.goBack).toHaveBeenCalledWith(1);
                    });

                    it("WinLossByProduct.goBack(level) should NOT be called if the value of attribute 'data-level' >= 2", function () {
                        spyOn(WinLossByProduct, "goBack");
                        $(".level-link").attr('data-level', 2);
                        $(".level-link").click();

                        expect(WinLossByProduct.goBack).not.toHaveBeenCalled();
                    });
                });

                describe("setExcelUrl(level, currencyId, custId, fdate, tdate, currencyName)", function () {
                    it("WinLossByProductOption.urlExcelData should has correct data", function () {
                        var expected = 'api/WinLossByProductReport/Index';

                        WinLossByProduct.setExcelUrl(0, 2, 221, "Apr 15, 2016", "Apr 20, 2016", "UUS");

                        expect(WinLossByProductOption.urlExcelData).toBe(expected);
                    });
                });

                describe("setReportTitle(currencyId, fromdate, todate, currencyName)", function () {
                    it("'div.report-title' should be correct", function () {
                        setFixtures("<div class='report-title' />");

                        WinLossByProduct.setReportTitle(1, new Date("Apr 10, 2015"), new Date("Apr 15, 2015"), "USD");

                        expect($('.report-title').html()).toBe('WinLossByProduct, 04/10/2015 → 04/15/2015 - Viewin');
                    });
                });

                describe("ajaxComplete(level, response)", function () {
                    beforeEach(function () {
                        setFixtures("<input type='checkbox' name='ReportBy' checked='checked' value='test' />");

                        spyOn(Report, "renderBreadCrumb");
                        spyOn(WinLossByProduct, "bindData");
                        WinLossByProductOption.licenseeId = 100;

                        WinLossByProduct.ajaxComplete(2, {});
                    });

                    it("Report.renderBreadCrumb(level) should be called with correct parameter", function () {
                        expect(Report.renderBreadCrumb).toHaveBeenCalledWith(2, 100);
                    });

                    it("WinLossByProduct.bindData(level, reportBy, response) should be called with correct parameter", function () {
                        expect(WinLossByProduct.bindData).toHaveBeenCalledWith(2, undefined, {});
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
        });