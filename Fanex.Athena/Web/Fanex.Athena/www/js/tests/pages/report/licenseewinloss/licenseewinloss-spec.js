define(['jquery', 'common/site', 'components/pluginhelper/daterangepicker', "pages/report/licenseewinloss/licenseewinloss.option", 'pages/report/licenseewinloss/licenseewinloss',
    'pages/report/report', 'lib/jqgrid/jquery.jqgrid.additional'],
    function ($, Site, DatePicker, LicenseeWinLossOption, LicenseeWinLoss, Report, jqGridAdditional) {
        describe("LicenseeWinLoss", function () {
            beforeAll(function () {
                spyOn(Site, "getSingleResource").and.callFake(function (text) {
                    return text;
                });
                spyOn(Site, "resolveClientUrl").and.callFake(function (url) {
                    return url;
                });
                spyOn(Site, "initialize");
                spyOn(Site, "setMultiSelectForDropDownList");
                spyOn(DatePicker, "initDateRangePicker");
            });

            describe("initialize()", function () {
                it("Site.initialize() should be called", function () {
                    LicenseeWinLoss.initialize();

                    expect(Site.initialize).toHaveBeenCalled();
                });

                it("LicenseeWinLossOption.licenseeId should be assigned data from #drillDownLicenseeId.", function () {
                    var $input = setFixtures('<input id="drillDownLicenseeId" value="1">');

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

                describe("LicenseeWinLossOption.gridOption.onSortCol(colName)", function () {
                    var _sortColumn = "Column1";
                    beforeEach(function () {
                        jasmine.clock().install();

                        setFixtures('<input name="ReportBy" type="checkbox" checked value="1" />');
                        spyOn(LicenseeWinLoss, "renderBreadCrumb");

                        spyOn(Site, "blockUI");
                        spyOn(LicenseeWinLoss, "sortData").and.callFake(function (data) { return data });
                        spyOn(LicenseeWinLoss, "bindData");
                        spyOn(LicenseeWinLoss, "setReportTitle");
                        spyOn(Site, "unBlockUI");

                        LicenseeWinLoss.ajaxComplete(0, { Data: "Hello" }); //set value for cache data
                        LicenseeWinLossOption.gridOption.onSortCol(_sortColumn);
                        jasmine.clock().tick(101);
                    });
                    afterEach(function () {
                        jasmine.clock().uninstall();
                    });

                    it("Site.blockUI() should be called", function () {
                        expect(Site.blockUI).toHaveBeenCalled();
                    });
                    it("LicenseeWinLoss.sortData() should be called with correct parameter", function () {
                        expect(LicenseeWinLoss.sortData).toHaveBeenCalledWith("Hello");
                    });
                    it("LicenseeWinLoss.bindData() should be called with correct parameter", function () {
                        expect(LicenseeWinLoss.bindData.calls.argsFor(1)).toEqual([0, "1", { Data: "Hello" }]);
                    });
                    it("Site.unBlockUI() should be called", function () {
                        expect(Site.unBlockUI).toHaveBeenCalled();
                    });
                });
            });

            describe("sortData(arrayData)", function () {
                var arrayData;
                beforeEach(function () {
                    arrayData = [{ Index: 1, Column2: "A" }, { Index: 2, Column2: "B" }];
                    jasmine.clock().install();

                    setFixtures('<input name="ReportBy" type="checkbox" checked value="1" />');
                    spyOn(LicenseeWinLoss, "renderBreadCrumb");

                    spyOn(Site, "blockUI");
                    spyOn(LicenseeWinLoss, "bindData");
                    spyOn(LicenseeWinLoss, "setReportTitle");
                    spyOn(Site, "unBlockUI");

                    LicenseeWinLoss.ajaxComplete(0, { Data: arrayData }); //set value for cache data                    
                    LicenseeWinLossOption.gridOption.onSortCol("Column2");
                    jasmine.clock().tick(101);
                });
                afterEach(function () {
                    jasmine.clock().uninstall();
                });

                it("Array data should be returned with ascending order", function () {
                    var result = LicenseeWinLoss.sortData(arrayData);

                    expect(result).toEqual(arrayData);
                });

                it("Array data should be returned with descending order", function () {
                    jasmine.clock().tick(101);
                    var result = LicenseeWinLoss.sortData(arrayData);

                    expect(result).toEqual([{ Index: 1, Column2: "B" }, { Index: 2, Column2: "A" }]);
                });
            });

            describe("initEvent()", function () {
                beforeEach(function () {
                    $("body").off();
                });

                afterEach(function () {
                    $("body").off();
                });

                it("#IsExportExcel should have value 'false' when #SubmitButton is clicked.", function () {
                    setFixtures('<div id="SubmitButton"></div>');
                    appendSetFixtures('<div id="IsExportExcel"></div>');
                    spyOn(LicenseeWinLoss, "submit");

                    LicenseeWinLoss.initEvent();

                    $('#SubmitButton').click();
                    expect($('#IsExportExcel').val()).toBe(false);
                });

                it("#LicenseeWinLossForm.submit() should be called if #ExportToExcel is clicked.", function () {
                    setFixtures('<div id="ExportToExcel"></div>');
                    appendSetFixtures('<div id="licenseewinloss"></div>');

                    var spyEvent = spyOnEvent('#licenseewinloss', 'submit');

                    LicenseeWinLoss.initEvent();
                    $('#ExportToExcel').click();
                    expect(spyEvent).toHaveBeenTriggered();
                });

                it("#LicenseeWinLoss.goBack() should be called with correct param when .level-link is clicked.", function () {
                    setFixtures('<div id="level-link-container"><div class="level-link" data-level="3"></div></div>');

                    spyOn(LicenseeWinLoss, 'goBack');

                    LicenseeWinLoss.initEvent();
                    $('.level-link').click();

                    expect(LicenseeWinLoss.goBack).toHaveBeenCalledWith(3);
                });

                it("LicenseeWinLoss.openBetlistPopUp(datadrilldown) should be called if .bet-list-link is clicked.", function () {
                    setFixtures('<div class="bet-list-link"></div>');

                    spyOn(LicenseeWinLoss, "openBetlistPopUp");
                    LicenseeWinLoss.initEvent();
                    $('.bet-list-link').click();
                    expect(LicenseeWinLoss.openBetlistPopUp).toHaveBeenCalled();
                });
            });

            describe("setExcelUrl(level, currencyId, custId, fdate, tdate, currencyName)", function () {
                var originalExcelUrl = LicenseeWinLossOption.urlExcelData;
                afterEach(function () {
                    LicenseeWinLossOption.urlExcelData = originalExcelUrl;
                });

                it("All input fields should have correct value when level grater than 0", function () {
                    LicenseeWinLoss.setExcelUrl(1, 2, 3, new Date(2001, 1, 1), new Date(2001, 1, 1), 'UUS');

                    expect(LicenseeWinLossOption.urlExcelData).toBe('api/LicenseeWinLossReport/Index?level=1&currencyId=2&custId=3&fdate=02/01/2001&tdate=02/01/2001&currencyName=UUS');
                });

                it("All input fields should have correct value when level equal 0", function () {
                    LicenseeWinLoss.setExcelUrl(0, 2, 3, new Date(2001, 1, 1), new Date(2001, 1, 1), 'UUS');

                    expect(LicenseeWinLossOption.urlExcelData).toBe('api/LicenseeWinLossReport/Index');
                });
            });

            describe("renderBreadCrumb(level)", function () {
                it(".breadCrumbHolder should be hide if param level is 0.", function () {
                    setFixtures('<div class="breadCrumbHolder"></div>');

                    LicenseeWinLoss.renderBreadCrumb(0);

                    expect($('.breadCrumbHolder').is(':visible')).toBe(false);
                });

                it(".level0 should show if param level > 0.", function () {
                    setFixtures('<div id="level-link-container"><div class="level0"></div></div>');

                    LicenseeWinLoss.renderBreadCrumb(1);

                    expect($('.level0').is(':visible')).toBe(true);
                });

                it(".level2 should be hide if param level = 1.", function () {
                    setFixtures('<div id="level-link-container"><div class="level2"></div></div>');

                    LicenseeWinLoss.renderBreadCrumb(1);

                    expect($('.level2').is(':visible')).toBe(false);
                });

                it(".level2 should show if param level = 2.", function () {
                    setFixtures('<div id="level-link-container"><div class="level2"></div></div>');

                    LicenseeWinLoss.renderBreadCrumb(2);

                    expect($('.level2').is(':visible')).toBe(true);
                });
            });

            describe("setReportTitle(currencyId, fromdate, todate)", function () {
                it("setReportTitle() should assign correct content to .report-title.", function () {
                    setFixtures('<div class="report-title"></div>');
                    var date = new Date(2001, 1, 1);

                    LicenseeWinLoss.setReportTitle('-1', date, date);

                    expect($('.report-title').html()).toBe('LicenseeWinLoss, 02/01/2001 → 02/01/2001 - Viewin');
                });
            });

            describe("goBack(level)", function () {
                it("LicenseeWinLossOption.urlExcelData should be updated to correct value.", function () {
                    expect(LicenseeWinLossOption.urlExcelData).toBe('api/LicenseeWinLossReport/Index');
                });
            });

            describe("ajaxComplete", function () {
                beforeEach(function () {
                    setFixtures('<input name="ReportBy" type="checkbox" checked value="1" />' +
                        '<input id="CurrencyId" value="1" />' +
                        '<input id="FromDate" value="03/01/2017" /> <input id="ToDate" value="03/31/2017" />');
                    spyOn(LicenseeWinLoss, "renderBreadCrumb");
                    spyOn(LicenseeWinLoss, "bindData");
                    spyOn(LicenseeWinLoss, "setReportTitle");
                });

                it("Nothing happen if response param has null value", function () {
                    LicenseeWinLoss.ajaxComplete(0, null);

                    expect(LicenseeWinLoss.renderBreadCrumb).not.toHaveBeenCalled();
                });

                it("LicenseeWinLoss.setReportTitle(currencyId, fromDate, toDate) should be called with correct parameter if level param is 0", function () {
                    LicenseeWinLoss.ajaxComplete(0, { Data: "Hello" });

                    expect(LicenseeWinLoss.setReportTitle).toHaveBeenCalledWith("1", "03/01/2017", "03/31/2017");
                });

                it("LicenseeWinLoss.setReportTitle(currencyId, fromDate, toDate) should not be called with correct parameter if level param greater than 0", function () {
                    LicenseeWinLoss.ajaxComplete(1, { Data: "Hello" });

                    expect(LicenseeWinLoss.setReportTitle).not.toHaveBeenCalled();
                });

                it("LicenseeWinLoss.renderBreadCrumb(level) should be called with correct parameter", function () {
                    LicenseeWinLoss.ajaxComplete(0, { Data: "Hello" });

                    expect(LicenseeWinLoss.renderBreadCrumb).toHaveBeenCalledWith(0);
                });

                it("LicenseeWinLoss.bindData(level) should be called with correct parameter", function () {
                    LicenseeWinLoss.ajaxComplete(0, { Data: "Hello" });

                    expect(LicenseeWinLoss.bindData).toHaveBeenCalledWith(0, "1", { Data: "Hello" });
                });
            });

            describe("submit()", function () {
                var request;
                beforeEach(function (done) {
                    jasmine.Ajax.install();

                    var submitData = {
                        ProductOptions: "SB, RC",
                        DrawFilter: 0,
                        CurrencyId: 1,
                        ReportBy: 1,
                        FromDate: "03/01/2017",
                        ToDate: "03/31/2017",
                        CurrencyName: "EUR",
                        IsExcludedTestData: true
                    };

                    setFixtures('<form id="licenseewinloss"></form>');
                    spyOn(Report, "setOriginalProduct");
                    spyOn(Site, "updateOriginalLicenseeOption");
                    spyOn(Report, "ValidProducts").and.returnValue(true);
                    spyOn($.fn, "serializeObject").and.returnValue(submitData);
                    spyOn(LicenseeWinLoss, "setExcelUrl");
                    spyOn(LicenseeWinLoss, "ajaxComplete");
                    spyOn(Site, "blockUI");
                    spyOn(Site, "unBlockUI");

                    jasmine.Ajax
                            .stubRequest("api/LicenseeWinLossReport/Index")
                            .andReturn({
                                'status': 200,
                                'contentType': 'application/json',
                                'responseText': '{ "ProductOptions": "SB,RC" }'
                            });

                    LicenseeWinLoss.submit();
                    request = jasmine.Ajax.requests.mostRecent();

                    setTimeout(done, 10);
                });
                afterEach(function () {
                    jasmine.Ajax.uninstall();
                });

                it("request url should be correct", function () {
                    var expectedUrl = "api/LicenseeWinLossReport/Index";

                    expect(request.url).toBe(expectedUrl);
                });
                it("Report.setOriginalProduct() should be called", function () {
                    expect(Report.setOriginalProduct).toHaveBeenCalled();
                });
                it("Site.updateOriginalLicenseeOption() should be called", function () {
                    expect(Site.updateOriginalLicenseeOption).toHaveBeenCalled();
                });
                it("Site.blockUI() should be called", function () {
                    expect(Site.blockUI).toHaveBeenCalled();
                });
                it("Site.unBlockUI() should be called", function () {
                    expect(Site.unBlockUI).toHaveBeenCalled();
                });
                it("LicenseeWinLoss.setExcelUrl(level, currencyId, custId, fdate, tdate, currencyName) should be called with correct parameters", function () {
                    expect(LicenseeWinLoss.setExcelUrl).toHaveBeenCalledWith(0, 0, 0, "", "", "");
                });
                it("LicenseeWinLoss.ajaxComplete(level, response) should be called with correct parameters", function () {
                    expect(LicenseeWinLoss.ajaxComplete).toHaveBeenCalledWith(0, { ProductOptions: "SB,RC" });
                });
            });

            describe("lastDateOfMonth(year, month)", function () {
                it("Value should return correctly", function () {
                    var expected = "Fri Mar 31 2017";

                    var result = LicenseeWinLoss.lastDateOfMonth(2017, 2).toDateString();

                    expect(result).toBe(expected);
                });
            });

            describe("resizeGrid()", function () {
                beforeEach(function () {
                    setFixtures('<div id="ReportGrid"></div>');
                    spyOn(jqGridAdditional, "updateGridHeight");
                    spyOn(jqGridAdditional, "updateGridWidth");

                    LicenseeWinLoss.resizeGrid();
                });

                it("jqGridAdditional.updateGridHeight() should be called", function () {
                    expect(jqGridAdditional.updateGridHeight).toHaveBeenCalled();
                });
                it("jqGridAdditional.updateGridWidth($grid) should be called", function () {
                    expect(jqGridAdditional.updateGridWidth).toHaveBeenCalledWith($("#ReportGrid"));
                });
            });
        });
    });