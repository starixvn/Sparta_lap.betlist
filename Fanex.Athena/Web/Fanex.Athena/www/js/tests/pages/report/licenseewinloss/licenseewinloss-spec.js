define(['jquery', 'common/site', 'components/pluginhelper/daterangepicker', 'pages/report/licenseewinloss/licenseewinloss.option', 'pages/report/licenseewinloss/licenseewinloss',
    'pages/report/report', 'lib/jqgrid/jquery.jqgrid.additional', 'common/popup', 'components/pluginhelper/multiselect'],
    function ($, Site, DatePicker, LicenseeWinLossOption, LicenseeWinLoss, Report, jqGridAdditional, Popup, MultiSelect) {
        describe("LicenseeWinLoss", function () {
            beforeAll(function () {
                spyOn(Site, "getSingleResource").and.callFake(function (text) {
                    if (text === "Viewin") {
                        return "Viewin {0}";
                    }

                    return text;
                });
                spyOn(Site, "resolveClientUrl").and.callFake(function (url) {
                    return url;
                });
                spyOn(Site, "initialize");
                spyOn(MultiSelect, "initialize");
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
                        spyOn(Report, "renderBreadCrumb");

                        spyOn(Site, "blockUI");
                        spyOn(LicenseeWinLoss, "sortData").and.callFake(function (data) { return data; });
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
                    spyOn(Report, "renderBreadCrumb");

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
                    setFixtures('<div id="level-link-container"><div class="level-link" data-level="1"></div></div>');

                    spyOn(LicenseeWinLoss, 'goBack');

                    LicenseeWinLoss.initEvent();
                    $('.level-link').click();

                    expect(LicenseeWinLoss.goBack).toHaveBeenCalledWith(1);
                });

                it("#LicenseeWinLoss.goBack() is not called when data-level in .level-link is greater than 1.", function () {
                    setFixtures('<div id="level-link-container"><div class="level-link" data-level="2"></div></div>');

                    spyOn(LicenseeWinLoss, 'goBack');

                    LicenseeWinLoss.initEvent();
                    $('.level-link').click();

                    expect(LicenseeWinLoss.goBack).not.toHaveBeenCalled();
                });

                it("LicenseeWinLoss.openBetlistPopUp(datadrilldown) should be called if .bet-list-link is clicked.", function () {
                    setFixtures('<div class="bet-list-link"></div>');

                    spyOn(LicenseeWinLoss, "openBetlistPopUp");
                    LicenseeWinLoss.initEvent();
                    $('.bet-list-link').click();
                    expect(LicenseeWinLoss.openBetlistPopUp).toHaveBeenCalled();
                });

                it("resizeGrid function should be called when window fire resize event", function () {
                    spyOn(LicenseeWinLoss, "resizeGrid");
                    LicenseeWinLoss.initEvent();

                    $(window).resize();

                    expect(LicenseeWinLoss.resizeGrid).toHaveBeenCalled();
                });

                it("off() jquery function should be called with correct parameter when window fire unload event", function () {
                    spyOn($.fn, "off");
                    LicenseeWinLoss.initEvent();

                    $(window).unload();

                    expect($.fn.off).toHaveBeenCalledWith("resize");
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

            describe("setReportTitle(currencyId, fromdate, todate)", function () {
                it(".report-title should has correct html content", function () {
                    setFixtures('<div class="report-title"></div>');
                    spyOn(Report, "getCurrencyNameForReportTitle").and.returnValue("Viewin EUR");
                    var date = new Date(2001, 1, 1);

                    LicenseeWinLoss.setReportTitle('-1', date, date);

                    expect($('.report-title').html()).toBe('LicenseeWinLoss, 02/01/2001 → 02/01/2001 - Viewin EUR');
                });

                it("class 'hide' should be removed in .panel-heading", function () {
                    setFixtures('<div class="panel-heading hide"></div>');
                    spyOn(Report, "getCurrencyNameForReportTitle").and.returnValue("Viewin EUR");
                    var date = new Date(2001, 1, 1);

                    LicenseeWinLoss.setReportTitle('-1', date, date);
                    var hasClass = $(".panel-heading").hasClass("hide");

                    expect(hasClass).toBe(false);
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
                    spyOn(Report, "renderBreadCrumb");
                    spyOn(LicenseeWinLoss, "bindData");
                    spyOn(LicenseeWinLoss, "setReportTitle");
                    LicenseeWinLossOption.licenseeId = 0;
                });

                it("Nothing happen if response param has null value", function () {
                    LicenseeWinLoss.ajaxComplete(0, null);

                    expect(Report.renderBreadCrumb).not.toHaveBeenCalled();
                });

                it("LicenseeWinLoss.setReportTitle(currencyId, fromDate, toDate) should be called with correct parameter if level param is 0", function () {
                    LicenseeWinLoss.ajaxComplete(0, { Data: "Hello" });

                    expect(LicenseeWinLoss.setReportTitle).toHaveBeenCalledWith("1", "03/01/2017", "03/31/2017");
                });

                it("LicenseeWinLoss.setReportTitle(currencyId, fromDate, toDate) should not be called with correct parameter if level param greater than 0", function () {
                    LicenseeWinLoss.ajaxComplete(1, { Data: "Hello" });

                    expect(LicenseeWinLoss.setReportTitle).not.toHaveBeenCalled();
                });

                it("Report.renderBreadCrumb(level, licenseeId) should be called with correct parameter", function () {
                    LicenseeWinLoss.ajaxComplete(0, { Data: "Hello" });

                    expect(Report.renderBreadCrumb).toHaveBeenCalledWith(0, 0);
                });

                it("LicenseeWinLoss.bindData(level) should be called with correct parameter", function () {
                    LicenseeWinLoss.ajaxComplete(0, { Data: "Hello" });

                    expect(LicenseeWinLoss.bindData).toHaveBeenCalledWith(0, "1", { Data: "Hello" });
                });
            });

            describe("submit()", function () {
                describe("Products is valid", function () {
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

                describe("Product is invalid", function () {
                    beforeEach(function () {
                        setFixtures('<form id="licenseewinloss"></form>');
                        spyOn(Report, "setOriginalProduct");
                        spyOn(Site, "updateOriginalLicenseeOption");
                        spyOn(Report, "ValidProducts").and.returnValue(false);
                        spyOn($.fn, "serializeObject");
                    });

                    it("Function return false", function () {
                        var result = LicenseeWinLoss.submit();

                        expect(result).toBe(false);
                    });

                    it("serializeObject() function is not called", function () {
                        LicenseeWinLoss.submit();

                        expect($.fn.serializeObject).not.toHaveBeenCalled();
                    });
                });
                
            });

            describe("lastDateOfMonth(year, month)", function () {
                it("Value should return correctly", function () {
                    var expected = "Fri Mar 31 2017";

                    var result = LicenseeWinLoss.lastDateOfMonth(2017, 2).toDateString();

                    expect(result).toBe(expected);
                });
            });

            describe("drillDown(level, currencyId, customerId, datetime, currencyName, keepSorting)", function () {
                var submitData;
                beforeEach(function () {
                    submitData = {
                        ProductOptions: '["SB","RC"]',
                        DrawFilter: 0,
                        CurrencyId: 1,
                        ReportBy: 2,
                        FromDate: "2017-03-01T00:00:00",
                        ToDate: "2017-03-31T00:00:00",
                        CurrencyName: "EUR",
                        IsExcludedTestData: true
                    };

                    setFixtures('<form id="licenseewinloss"></form> <input id="BaseCurrency" data-original="EUR" />');
                    spyOn(Report, "setOriginalProduct");
                    spyOn(Site, "updateOriginalLicenseeOption");
                    spyOn(Report, "ValidProducts").and.returnValue(true);
                    spyOn($.fn, "serializeObject").and.returnValue(submitData);
                    spyOn(LicenseeWinLoss, "setExcelUrl");
                    spyOn(LicenseeWinLoss, "ajaxComplete");
                    spyOn($, "ajax").and.returnValue({ abort: function () { } });
                    spyOn(Report, "GetDrilldownDateRange").and.returnValues({ FromDate: new Date(2017, 2, 1), ToDate: new Date(2017, 2, 31) });
                });
                afterEach(function () {
                    submitData = {
                        ProductOptions: '["SB","RC"]',
                        DrawFilter: 0,
                        CurrencyId: 1,
                        ReportBy: 2,
                        FromDate: "2017-03-01T00:00:00",
                        ToDate: "2017-03-31T00:00:00",
                        CurrencyName: "EUR",
                        IsExcludedTestData: true
                    };
                });

                it("property data of ajax option should be correct", function () {
                    LicenseeWinLoss.submit();
                    var ajaxOptionForCreateSubmitData = $.ajax.calls.mostRecent().args[0];
                    ajaxOptionForCreateSubmitData.success(submitData);

                    LicenseeWinLoss.drillDown(1, 9, 0, '2017-02-04T00:00:00', 'EUR');
                    var ajaxOptionForDrillingDown = $.ajax.calls.mostRecent().args[0];
                    var dataOptionResult = $.parseJSON(ajaxOptionForDrillingDown.data);

                    expect(dataOptionResult.ProductOptions).toEqual(["SB", "RC"]);
                    expect(dataOptionResult.DrawFilter).toBe(submitData.DrawFilter);
                    expect(dataOptionResult.CurrencyId).toBe(submitData.CurrencyId);
                    expect(dataOptionResult.ReportBy).toBe(submitData.ReportBy);
                    expect(new Date(dataOptionResult.FromDate)).toEqual(new Date(submitData.FromDate));
                    expect(new Date(dataOptionResult.ToDate)).toEqual(new Date(submitData.ToDate));
                    expect(dataOptionResult.IsExcludedTestData).toBe(submitData.IsExcludedTestData);
                });

                it("url of ajax option should be correct when month of date param equal with month in FromDate and ToDate", function () {
                    LicenseeWinLoss.submit();
                    var ajaxOptionForCreateSubmitData = $.ajax.calls.mostRecent().args[0];
                    ajaxOptionForCreateSubmitData.success(submitData);

                    LicenseeWinLoss.drillDown(2, 9, 0, '2017-03-04T00:00:00', 'EUR', true);
                    var ajaxOptionForDrillingDown = $.ajax.calls.mostRecent().args[0];

                    var resultUrl = ajaxOptionForDrillingDown.url;
                    var expectedUrl = "api/LicenseeWinLossReport/Index?level=2&currencyId=9&custId=0&fdate=03/01/2017&tdate=03/31/2017";

                    expect(resultUrl).toBe(expectedUrl);
                });

                it("Site.blockUI() should be called with beforeSend function of ajax option", function () {
                    spyOn(Site, "blockUI");
                    LicenseeWinLoss.submit();
                    var ajaxOptionForCreateSubmitData = $.ajax.calls.mostRecent().args[0];
                    ajaxOptionForCreateSubmitData.success(submitData);

                    LicenseeWinLoss.drillDown(2, 9, 0, '2017-03-04T00:00:00', 'EUR', true);
                    var ajaxOptionForDrillingDown = $.ajax.calls.mostRecent().args[0];

                    ajaxOptionForDrillingDown.beforeSend();

                    expect(Site.blockUI).toHaveBeenCalled();
                });
                it("Site.unBlockUI() should be called with complete function of ajax option", function () {
                    spyOn(Site, "unBlockUI");
                    LicenseeWinLoss.submit();
                    var ajaxOptionForCreateSubmitData = $.ajax.calls.mostRecent().args[0];
                    ajaxOptionForCreateSubmitData.success(submitData);

                    LicenseeWinLoss.drillDown(2, 9, 0, '2017-03-04T00:00:00', 'EUR', true);
                    var ajaxOptionForDrillingDown = $.ajax.calls.mostRecent().args[0];

                    ajaxOptionForDrillingDown.complete();

                    expect(Site.unBlockUI).toHaveBeenCalled();
                });

                describe("success function of ajax option", function () {
                    it("LicenseeWinLoss.ajaxComplete(level, response) should be called with correct parameter", function () {
                        LicenseeWinLoss.submit();
                        var ajaxOptionForCreateSubmitData = $.ajax.calls.mostRecent().args[0];
                        ajaxOptionForCreateSubmitData.success(submitData);

                        LicenseeWinLoss.drillDown(2, 9, 0, '2017-03-04T00:00:00', 'EUR', true);
                        var ajaxOptionForDrillingDown = $.ajax.calls.mostRecent().args[0];
                        ajaxOptionForDrillingDown.success({ Data: [] });

                        expect(LicenseeWinLoss.ajaxComplete.calls.mostRecent().args).toEqual([2, { Data: [] }]);
                    });

                    it("LicenseeWinLoss.setExcelUrl(level, currencyId, customerId, fdate, tdate, currencyName) should be called with correct parameter", function () {
                        LicenseeWinLoss.submit();
                        var ajaxOptionForCreateSubmitData = $.ajax.calls.mostRecent().args[0];
                        ajaxOptionForCreateSubmitData.success(submitData);

                        LicenseeWinLoss.drillDown(0, 9, 0, '2017-03-04T00:00:00', 'EUR', true);
                        var ajaxOptionForDrillingDown = $.ajax.calls.mostRecent().args[0];
                        ajaxOptionForDrillingDown.success({ Data: [{ Name: "Test" }] });

                        var args = LicenseeWinLoss.setExcelUrl.calls.mostRecent().args;
                        expect(args[0]).toBe(0);
                        expect(args[1]).toBe(9);
                        expect(args[2]).toBe(0);
                        expect(args[3]).toEqual(new Date(2017, 2, 1));
                        expect(args[4]).toEqual(new Date(2017, 2, 31));
                        expect(args[5]).toBe("EUR");
                    });

                    it("LicenseeWinLoss.setReportTitle(currencyId, fdate, tdate, currencyName) should be called with correct parameter", function () {
                        LicenseeWinLoss.submit();
                        var ajaxOptionForCreateSubmitData = $.ajax.calls.mostRecent().args[0];
                        ajaxOptionForCreateSubmitData.success(submitData);

                        spyOn(LicenseeWinLoss, "setReportTitle");
                        LicenseeWinLoss.drillDown(1, 9, 0, '2017-03-04T00:00:00', 'EUR', true);
                        var ajaxOptionForDrillingDown = $.ajax.calls.mostRecent().args[0];
                        ajaxOptionForDrillingDown.success({ Data: [{ Name: "Test" }] });

                        var args = LicenseeWinLoss.setReportTitle.calls.mostRecent().args;
                        expect(args[0]).toBe(1);
                        expect(args[1]).toEqual(new Date(2017, 2, 1));
                        expect(args[2]).toEqual(new Date(2017, 2, 31));
                        expect(args[3]).toBe("EUR");
                    });

                    it("LicenseeWinLoss.setReportTitle(currencyId, fdate, tdate, currencyName) should be called with correct parameter when level > 0 & currencyId === 0", function () {
                        submitData.CurrencyId = 0;
                        LicenseeWinLoss.submit();
                        var ajaxOptionForCreateSubmitData = $.ajax.calls.mostRecent().args[0];
                        ajaxOptionForCreateSubmitData.success(submitData);

                        spyOn(LicenseeWinLoss, "setReportTitle");
                        LicenseeWinLoss.drillDown(1, 9, 0, '2017-03-04T00:00:00', 'EUR', true);
                        var ajaxOptionForDrillingDown = $.ajax.calls.mostRecent().args[0];
                        ajaxOptionForDrillingDown.success({ Data: [{ Name: "Test" }] });

                        var args = LicenseeWinLoss.setReportTitle.calls.mostRecent().args;
                        expect(args[0]).toBe(9);
                        expect(args[1]).toEqual(new Date(2017, 2, 1));
                        expect(args[2]).toEqual(new Date(2017, 2, 31));
                        expect(args[3]).toBe("EUR");
                    });
                });
            });

            describe("goBack(level)", function () {
                var submitData;
                beforeEach(function () {
                    submitData = {
                        ProductOptions: '["SB","RC"]',
                        DrawFilter: 0,
                        CurrencyId: 1,
                        ReportBy: 2,
                        FromDate: "2017-03-01T00:00:00",
                        ToDate: "2017-03-31T00:00:00",
                        CurrencyName: "EUR",
                        IsExcludedTestData: true
                    };

                    setFixtures('<form id="licenseewinloss"></form> <input id="BaseCurrency" data-original="EUR" />');
                    appendSetFixtures('<input name="ReportBy" type="checkbox" checked value="1" />' +
                        '<input id="CurrencyId" value="1" />' +
                        '<input id="FromDate" value="03/01/2017" /> <input id="ToDate" value="03/31/2017" />');

                    spyOn(Report, "setOriginalProduct");
                    spyOn(Site, "updateOriginalLicenseeOption");
                    spyOn(Report, "ValidProducts").and.returnValue(true);
                    spyOn($.fn, "serializeObject").and.returnValue(submitData);
                    spyOn(LicenseeWinLoss, "setExcelUrl");
                    spyOn(LicenseeWinLoss, "setReportTitle");
                    spyOn(Report, "renderBreadCrumb");
                    spyOn(LicenseeWinLoss, "bindData");
                    spyOn($, "ajax").and.returnValue({ abort: function () { } });

                    LicenseeWinLoss.submit();
                    var ajaxOption = $.ajax.calls.mostRecent().args[0];
                    ajaxOption.success(submitData);
                });
                afterEach(function () {
                    submitData = {
                        ProductOptions: '["SB","RC"]',
                        DrawFilter: 0,
                        CurrencyId: 1,
                        ReportBy: 2,
                        FromDate: "2017-03-01T00:00:00",
                        ToDate: "2017-03-31T00:00:00",
                        CurrencyName: "EUR",
                        IsExcludedTestData: true
                    };
                });

                it("Report.renderBreadCrumb(level, licenseeId) should be called with correct parameter", function () {
                    LicenseeWinLoss.drillDown(0, 9, 0, '2017-02-04T00:00:00', 'EUR');
                    var ajaxOptionForDrillingDown = $.ajax.calls.mostRecent().args[0];
                    ajaxOptionForDrillingDown.success({ Data: [{ Name: "Test Level 0" }] });
                    LicenseeWinLoss.goBack(0);

                    expect(Report.renderBreadCrumb).toHaveBeenCalledWith(0, 0);
                });
                it("LicenseeWinLoss.bindData(level) should be called with correct parameter", function () {
                    LicenseeWinLoss.drillDown(0, 9, 0, '2017-02-04T00:00:00', 'EUR');
                    var ajaxOptionForDrillingDown = $.ajax.calls.mostRecent().args[0];
                    ajaxOptionForDrillingDown.success({ Data: [{ Name: "Test Level 0" }] });
                    LicenseeWinLoss.goBack(0);

                    expect(LicenseeWinLoss.bindData).toHaveBeenCalledWith(0, 2, { Data: [{ Name: "Test Level 0" }] });
                });

                describe("level equal 0", function () {
                    beforeEach(function () {
                        LicenseeWinLoss.drillDown(0, 9, 0, '2017-02-04T00:00:00', 'EUR');
                        var ajaxOptionForDrillingDown = $.ajax.calls.mostRecent().args[0];
                        ajaxOptionForDrillingDown.success({ Data: [{ Name: "Test Level 0" }] });
                        LicenseeWinLoss.goBack(0);
                    });

                    it("LicenseeWinLoss.setExcelUrl(level, currencyId, customerId, fdate, tdate, currencyName) should be called with correct parameter", function () {
                        expect(LicenseeWinLoss.setExcelUrl).toHaveBeenCalledWith(0, 0, 0, "", "", "");
                    });
                    it("LicenseeWinLoss.setReportTitle(level) should be called with correct parameter", function () {
                        var args = LicenseeWinLoss.setReportTitle.calls.mostRecent().args;
                        
                        expect(args[0]).toBe(1);
                        expect(args[1].toDateString()).toBe("Wed Mar 01 2017");
                        expect(args[2].toDateString()).toBe("Fri Mar 31 2017");
                    });
                });

                describe("level equal 1", function () {
                    var urlExcelData = LicenseeWinLossOption.urlExcelData;

                    beforeEach(function () {
                        LicenseeWinLossOption.urlExcelData = "localhost/Test?level=2";
                        LicenseeWinLoss.drillDown(1, 9, 0, '2017-02-04T00:00:00', 'EUR');
                        var ajaxOptionForDrillingDown = $.ajax.calls.mostRecent().args[0];
                        ajaxOptionForDrillingDown.success({ Data: [{ Name: "Test Level 1" }] });
                        LicenseeWinLoss.goBack(1);
                    });

                    afterEach(function () {
                        LicenseeWinLossOption.urlExcelData = urlExcelData;
                    });

                    it("LicenseeWinLossOption.urlExcelData should be correct", function () {
                        expect(LicenseeWinLossOption.urlExcelData).toBe("localhost/Test?level=1");
                    });
                });

                it("Nothing happen with level param is equal 2", function () {
                    LicenseeWinLoss.drillDown(2, 9, 0, '2017-02-04T00:00:00', 'EUR');
                    LicenseeWinLoss.goBack(2);

                    expect(LicenseeWinLoss.setReportTitle.calls.count()).toBe(1);
                });

                it("urlExcelData is not be changed", function () {
                    var urlExcel = LicenseeWinLossOption.urlExcelData;
                    LicenseeWinLoss.drillDown(2, 9, 0, '2017-02-04T00:00:00', 'EUR');
                    LicenseeWinLoss.goBack(2);

                    expect(urlExcel).toBe(LicenseeWinLossOption.urlExcelData);
                });
            });

            describe("bindTotal(level, totalData, summaryTotal)", function () {
                var submitData;
                beforeEach(function () {
                    submitData = {
                        ProductOptions: '["SB","RC"]',
                        DrawFilter: 0,
                        CurrencyId: 1,
                        ReportBy: 2,
                        FromDate: "2017-03-01T00:00:00",
                        ToDate: "2017-03-31T00:00:00",
                        CurrencyName: "EUR",
                        IsExcludedTestData: true
                    };

                    setFixtures('<form id="licenseewinloss"></form> <input id="BaseCurrency" data-original="EUR" />');

                    spyOn(Report, "setOriginalProduct");
                    spyOn(Site, "updateOriginalLicenseeOption");
                    spyOn(Report, "ValidProducts").and.returnValue(true);
                    spyOn($.fn, "serializeObject").and.returnValue(submitData);
                    spyOn(LicenseeWinLoss, "setExcelUrl");
                    spyOn(LicenseeWinLoss, "ajaxComplete");
                    spyOn($, "ajax").and.returnValue({ abort: function () { } });

                    LicenseeWinLoss.submit();
                    var ajaxOption = $.ajax.calls.mostRecent().args[0];
                    ajaxOption.success(submitData);

                    spyOn(LicenseeWinLossOption, "getTotalColModel").and.returnValue([{ Name: "Col1" }]);
                    spyOn(jqGridAdditional, "assignTotal");
                    spyOn(jqGridAdditional, "setGroupFooters");
                    spyOn(Report, "bindSummaryTotal").and.returnValue(3);
                    LicenseeWinLoss.bindTotal(1, { Total: [1, 2] }, 3);
                });
                afterEach(function () {
                    submitData = {
                        ProductOptions: '["SB","RC"]',
                        DrawFilter: 0,
                        CurrencyId: 1,
                        ReportBy: 2,
                        FromDate: "2017-03-01T00:00:00",
                        ToDate: "2017-03-31T00:00:00",
                        CurrencyName: "EUR",
                        IsExcludedTestData: true
                    };
                });

                it("LicenseeWinLossOption.getTotalColModel(level, productOptions) should be called with correct parameter", function () {
                    expect(LicenseeWinLossOption.getTotalColModel).toHaveBeenCalledWith(1, ["RC", "SB"]);
                });
                it("jqGridAdditional.assignTotal(totalData, columnModel) should be called with correct parameter", function () {
                    expect(jqGridAdditional.assignTotal).toHaveBeenCalledWith({ Total: [1, 2] }, [{ Name: "Col1" }]);
                });
                it("Report.bindSummaryTotal(columnModel, summaryTotal) should be called with correct parameter", function () {
                    expect(Report.bindSummaryTotal).toHaveBeenCalledWith([{ Name: "Col1" }], 3);
                });
                it("jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount) should be called with correct parameter", function () {
                    expect(jqGridAdditional.setGroupFooters).toHaveBeenCalledWith(2, 3, 3);
                });
            });

            describe("openBetlistPopUp(dataDrillDown)", function () {
                var submitData, dataDrilldown;
                beforeEach(function () {
                    submitData = {
                        ProductOptions: '["SB","RC"]',
                        DrawFilter: 0,
                        CurrencyId: 1,
                        ReportBy: 2,
                        FromDate: "2017-03-01T00:00:00",
                        ToDate: "2017-03-31T00:00:00",
                        CurrencyName: "EUR",
                        IsExcludedTestData: true
                    };

                    setFixtures('<form id="licenseewinloss"></form> <input id="BaseCurrency" data-original="EUR" />');

                    spyOn(Report, "setOriginalProduct");
                    spyOn(Site, "updateOriginalLicenseeOption");
                    spyOn($.fn, "serializeObject").and.returnValue(submitData);
                    spyOn(Report, "ValidProducts").and.returnValue(true);
                    spyOn(LicenseeWinLoss, "setExcelUrl");
                    spyOn(LicenseeWinLoss, "ajaxComplete");
                    spyOn($, "ajax").and.returnValue({ abort: function () { } });
                    spyOn(Site, "setProductOptionOriginalParamList").and.callFake(function (url) { return url; });
                    spyOn(Popup, "openPopupWithIframe");
                    $(window).height(300);

                    dataDrilldown = '{"DrillDownFromDate":"2017-01-01T00:00:00","DrillDownToDate":"2017-03-20T00:00:00","DrillDownCurrencyId":9,"DrillDownCurrencyName":"AUD",' +
                        '"CustomerId":6507355,"ExternalId":"DEWSNIP","SystemId":"12BETAUD01159"}';
                });
                afterEach(function () {
                    submitData = {
                        ProductOptions: '["SB","RC"]',
                        DrawFilter: 0,
                        CurrencyId: 1,
                        ReportBy: 2,
                        FromDate: "2017-03-01T00:00:00",
                        ToDate: "2017-03-31T00:00:00",
                        CurrencyName: "EUR",
                        IsExcludedTestData: true
                    };
                });

                describe("Report by monthly", function () {
                    it("Popup.openPopupWithIframe(url, option) should be called with correct parameter", function () {
                        LicenseeWinLoss.submit();
                        var ajaxOption = $.ajax.calls.mostRecent().args[0];
                        ajaxOption.success(submitData);

                        LicenseeWinLoss.openBetlistPopUp(dataDrilldown);

                        var url = 'LicenseeWinLossBetList/Index?fromdate=01/01/2017&todate=01/31/2017&excludetest=true&currencyid=9&currencyname=AUD&' +
                            'drawfilter=0&custid=6507355&systemid=12BETAUD01159&externalid=DEWSNIP&basecurrency=EUR&originalcurrenceid=1&originalcurrencyname=EUR';
                        var popupOption = { id: "betlist-popup", width: 970, height: 250, title: "Customer Bet List" };

                        var args = Popup.openPopupWithIframe.calls.mostRecent().args;
                        var optionResult = args[1];

                        expect(args[0]).toBe(url);
                        expect(optionResult.id).toBe(popupOption.id);
                        expect(optionResult.width).toBe(popupOption.width);
                        expect(optionResult.title).toBe(popupOption.title);
                    });

                    it("Popup.openPopupWithIframe(url, option) should be called with correct parameter when DrillDownDate equal to date in submit data", function () {
                        submitData.FromDate = "2017-01-10T00:00:00";
                        submitData.ToDate = "2017-01-15T00:00:00";

                        LicenseeWinLoss.submit();
                        var ajaxOption = $.ajax.calls.mostRecent().args[0];
                        ajaxOption.success(submitData);

                        LicenseeWinLoss.openBetlistPopUp(dataDrilldown);

                        var url = 'LicenseeWinLossBetList/Index?fromdate=01/10/2017&todate=01/15/2017&excludetest=true&currencyid=9&currencyname=AUD&' +
                            'drawfilter=0&custid=6507355&systemid=12BETAUD01159&externalid=DEWSNIP&basecurrency=EUR&originalcurrenceid=1&originalcurrencyname=EUR';
                        var popupOption = { id: "betlist-popup", width: 970, height: 250, title: "Customer Bet List" };

                        var args = Popup.openPopupWithIframe.calls.mostRecent().args;
                        var optionResult = args[1];

                        expect(args[0]).toBe(url);
                        expect(optionResult.id).toBe(popupOption.id);
                        expect(optionResult.width).toBe(popupOption.width);
                        expect(optionResult.title).toBe(popupOption.title);
                    });
                });

                describe("Report not by monthly", function () {
                    afterEach(function () {
                        submitData = {
                            ProductOptions: '["SB","RC"]',
                            DrawFilter: 0,
                            CurrencyId: 1,
                            ReportBy: 2,
                            FromDate: "2017-03-01T00:00:00",
                            ToDate: "2017-03-31T00:00:00",
                            CurrencyName: "EUR",
                            IsExcludedTestData: true
                        };
                    });
                    it("Popup.openPopupWithIframe(url, option) should be called with correct parameter", function () {
                        submitData.ReportBy = 1;
                        LicenseeWinLoss.submit();
                        var ajaxOption = $.ajax.calls.mostRecent().args[0];
                        ajaxOption.success(submitData);

                        LicenseeWinLoss.openBetlistPopUp(dataDrilldown);

                        var url = 'LicenseeWinLossBetList/Index?fromdate=01/01/2017&todate=03/20/2017&excludetest=true&currencyid=9&currencyname=AUD&' +
                            'drawfilter=0&custid=6507355&systemid=12BETAUD01159&externalid=DEWSNIP&basecurrency=EUR&originalcurrenceid=1&originalcurrencyname=EUR';
                        var popupOption = { id: "betlist-popup", width: 970, height: 250, title: "Customer Bet List" };

                        var args = Popup.openPopupWithIframe.calls.mostRecent().args;
                        var optionResult = args[1];

                        expect(args[0]).toBe(url);
                        expect(optionResult.id).toBe(popupOption.id);
                        expect(optionResult.width).toBe(popupOption.width);
                        expect(optionResult.title).toBe(popupOption.title);
                    });
                });
            });

            describe("bindData(level, reportBy, response)", function () {
                var submitData, gridOption;
                beforeEach(function () {
                    submitData = {
                        ProductOptions: '["SB","RC"]',
                        DrawFilter: 0,
                        CurrencyId: 1,
                        ReportBy: 2,
                        FromDate: "2017-03-01T00:00:00",
                        ToDate: "2017-03-31T00:00:00",
                        CurrencyName: "EUR",
                        IsExcludedTestData: true
                    };

                    setFixtures('<form id="licenseewinloss"></form> <div id="ReportGrid"></div>');

                    spyOn(Report, "setOriginalProduct");
                    spyOn(Site, "updateOriginalLicenseeOption");
                    spyOn($.fn, "serializeObject").and.returnValue(submitData);
                    spyOn(Report, "ValidProducts").and.returnValue(true);
                    spyOn(LicenseeWinLoss, "setExcelUrl");
                    spyOn(LicenseeWinLoss, "ajaxComplete");
                    spyOn($, "ajax").and.returnValue({ abort: function () { } });

                    spyOn(jqGridAdditional, "clearGrid");
                    spyOn(jqGridAdditional, "setNoInfo");
                    spyOn(LicenseeWinLossOption, "getGridOption").and.returnValue({});
                    spyOn($.fn, "jqGrid").and.callFake(function (option) { gridOption = option; });
                    spyOn(LicenseeWinLossOption, "setGroupHeaderColumns");
                    spyOn(LicenseeWinLossOption, "restoreExpanse");

                    LicenseeWinLoss.submit();
                    var ajaxOption = $.ajax.calls.mostRecent().args[0];
                    ajaxOption.success(submitData);
                });

                it("jqGridAdditional.clearGrid(grid) should be called with correct parameter", function () {
                    LicenseeWinLoss.bindData(0, 2, { Data: [] });

                    expect(jqGridAdditional.clearGrid).toHaveBeenCalledWith("ReportGrid");
                });
                it("jqGridAdditional.setNoInfo() should be called", function () {
                    LicenseeWinLoss.bindData(0, 2, { Data: [] });

                    expect(jqGridAdditional.setNoInfo).toHaveBeenCalled();
                });
                it("jqGrid function should be called with correct grid option value", function () {
                    LicenseeWinLoss.bindData(0, 2, { Data: [1, 2, 3], Total: [1] });

                    expect($.fn.jqGrid).toHaveBeenCalled();
                    expect(gridOption.rownumbers).toBe(true);
                    expect(gridOption.data).toEqual([1, 2, 3]);
                    expect(gridOption.userData).toEqual({});
                    expect(gridOption.rowNum).toBe(2000);
                    expect(gridOption.rowList).toEqual([50, 100, 200, 300, 400, 500, 1000, 2000]);
                    expect(gridOption.rowTotal).toBe(3);
                    expect(gridOption.pager).toBe("#ppdata");
                });

                describe("gridOption.onPaging(pgButton)", function () {
                    it("Should return 'stop' when level not equal 2", function () {
                        LicenseeWinLoss.bindData(0, 2, { Data: [1, 2, 3], Total: [1] });

                        var result = gridOption.onPaging("Test");

                        expect(result).toBe("stop");
                    });

                    it("Should return 'stop' when numOfRow not greater than LicenseeWinLossOption.pageSize", function () {
                        LicenseeWinLoss.bindData(2, 2, { Data: [1, 2, 3], Total: [1] });

                        var result = gridOption.onPaging("Test");

                        expect(result).toBe("stop");
                    });

                    it("Site.blockUI() should be called when pgButton not has class 'ui-state-disabled'", function () {
                        var defaultPagesize = LicenseeWinLossOption.pageSize;
                        appendSetFixtures('<button id="pgButton"></button>');
                        LicenseeWinLossOption.pageSize = 2;
                        spyOn(Site, "blockUI");

                        gridOption.onPaging("pgButton");
                        LicenseeWinLossOption.pageSize = defaultPagesize;

                        expect(Site.blockUI).toHaveBeenCalled();
                    });

                    it("Site.blockUI() should not be called when pgButton has class 'ui-state-disabled'", function () {
                        var defaultPagesize = LicenseeWinLossOption.pageSize;
                        appendSetFixtures('<button id="pgButton" class="ui-state-disabled"></button>');
                        LicenseeWinLossOption.pageSize = 2;
                        spyOn(Site, "blockUI");

                        gridOption.onPaging("pgButton");
                        LicenseeWinLossOption.pageSize = defaultPagesize;

                        expect(Site.blockUI).not.toHaveBeenCalled();
                    });
                });

                describe("gridOption.gridComplete()", function () {
                    var originalPageSize;
                    beforeEach(function () {
                        spyOn(jqGridAdditional, "gridComplete");
                        spyOn(LicenseeWinLoss, "resizeGrid");
                        spyOn(LicenseeWinLossOption, "addProductButton");
                        spyOn(Site, "unBlockUI");
                        spyOn(LicenseeWinLoss, "bindTotal");

                        originalPageSize = LicenseeWinLossOption.pageSize;
                    });

                    afterEach(function () {
                        LicenseeWinLossOption.pageSize = originalPageSize;
                    });

                    it("jqGridAdditional.gridComplete() should be called", function () {
                        LicenseeWinLoss.bindData(2, 2, { Data: [1, 2, 3], Total: [1] });

                        gridOption.gridComplete();

                        expect(jqGridAdditional.gridComplete).toHaveBeenCalled();
                    });

                    it("LicenseeWinLoss.resizeGrid() should be called", function () {
                        LicenseeWinLoss.bindData(2, 2, { Data: [1, 2, 3], Total: [1] });

                        gridOption.gridComplete();

                        expect(LicenseeWinLoss.resizeGrid).toHaveBeenCalled();
                    });

                    it("LicenseeWinLoss.bindTotal() should be called with correct parameter", function () {
                        LicenseeWinLoss.bindData(2, 2, { Data: [1, 2, 3], Total: [1], SummaryTotal: [2] });

                        gridOption.gridComplete();

                        expect(LicenseeWinLoss.bindTotal).toHaveBeenCalledWith(2, [1], [2]);
                    });

                    it("#footer should has correct html content when onPaging is false or level is not 2", function () {
                        appendSetFixtures('<div class="ui-jqgrid-ftable">Footer Content</div> <div id="footer"></div>');
                        LicenseeWinLoss.bindData(1, 2, { Data: [1, 2, 3], Total: [1], SummaryTotal: [2] });

                        gridOption.gridComplete();
                        var result = $("#footer").html();

                        expect(result).toBe("Footer Content");
                    });

                    it("LicenseeWinLossOption.addProductButton should be called with correct parameter", function () {
                        LicenseeWinLoss.bindData(2, 2, { Data: [1, 2, 3], Total: [1] });

                        gridOption.gridComplete();

                        expect(LicenseeWinLossOption.addProductButton).toHaveBeenCalledWith($("#ReportGrid"), "jqgh_ReportGrid", "ReportGrid", ["RC", "SB"]);
                    });

                    it("LicenseeWinLossOption.addProductButton should not be called when Total data is empty", function () {
                        LicenseeWinLoss.bindData(2, 2, { Data: [1, 2, 3], Total: [] });

                        gridOption.gridComplete();

                        expect(LicenseeWinLossOption.addProductButton).not.toHaveBeenCalled();
                    });

                    it("#pg_ppdata should be showed when level is 2 and rowNum greater than pagesize", function () {
                        appendSetFixtures('<button id="pg_ppdata"></button>');
                        LicenseeWinLoss.bindData(2, 2, { Data: [1, 2, 3], Total: [1] });
                        LicenseeWinLossOption.pageSize = 1;

                        gridOption.gridComplete();

                        expect($("#pg_ppdata")).toBeVisible();
                    });

                    it("#pg_ppdata should be hidden when level is not 2", function () {
                        appendSetFixtures('<button id="pg_ppdata"></button>');
                        LicenseeWinLoss.bindData(1, 2, { Data: [1, 2, 3], Total: [1] });

                        gridOption.gridComplete();

                        expect($("#pg_ppdata")).not.toBeVisible();
                    });

                    it(".ui-jqgrid-ftable should has correct html content when onPaging is true and level is 2", function () {
                        LicenseeWinLoss.bindData(2, 2, { Data: [1, 2, 3], Total: [] });
                        appendSetFixtures('<button id="pgButton"></button>' +
                            '<div class="ui-jqgrid-ftable"></div>' +
                            '<div id="footer">Footer Content</div>');
                        LicenseeWinLossOption.pageSize = 1;

                        gridOption.onPaging("pgButton");
                        gridOption.gridComplete();
                        var result = $(".ui-jqgrid-ftable").html();

                        expect(result).toBe("Footer Content");
                    });
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