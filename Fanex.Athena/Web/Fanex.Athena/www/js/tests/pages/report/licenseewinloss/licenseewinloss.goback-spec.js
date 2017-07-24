define(['jquery', 'common/site', 'components/pluginhelper/daterangepicker', 'pages/report/licenseewinloss/licenseewinloss.option', 'pages/report/licenseewinloss/licenseewinloss',
    'pages/report/report', 'components/pluginhelper/multiselect'],
    function ($, Site, DatePicker, LicenseeWinLossOption, LicenseeWinLoss, Report, MultiSelect) {
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
                spyOn(MultiSelect, "initProductList");
                spyOn(DatePicker, "initDateRangePicker");
            });

            describe("goBack(level)", function () {
                beforeEach(function () {
                    var submitData = {
                        ProductOptions: '["SB","RC"]',
                        DrawFilter: 0,
                        CurrencyId: 1,
                        ReportBy: 2,
                        FromDate: "2017-02-01T01:00:00",
                        ToDate: "2017-03-31T01:00:00",
                        CurrencyName: "EUR",
                        IsExcludedTestData: true
                    };

                    setFixtures('<form id="licenseewinloss"></form>');

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

                it("LicenseeWinLossOption.urlExcelData should be updated to correct value.", function () {
                    expect(LicenseeWinLossOption.urlExcelData).toBe('api/LicenseeWinLossReport/Index');
                });

                it("Report.renderBreadCrumb(level, licenseeId) should be called with correct parameter", function () {
                    LicenseeWinLoss.drillDown(0, 9, 0, '2017-02-04T01:00:00', 'EUR');
                    var ajaxOptionForDrillingDown = $.ajax.calls.mostRecent().args[0];
                    ajaxOptionForDrillingDown.success({ Data: [{ Name: "Test Level 0" }] });
                    LicenseeWinLoss.goBack(0);

                    expect(Report.renderBreadCrumb).toHaveBeenCalledWith(0, 0);
                });
                it("LicenseeWinLoss.bindData(level) should be called with correct parameter", function () {
                    LicenseeWinLoss.drillDown(0, 9, 0, '2017-02-04T01:00:00', 'EUR');
                    var ajaxOptionForDrillingDown = $.ajax.calls.mostRecent().args[0];
                    ajaxOptionForDrillingDown.success({ Data: [{ Name: "Test Level 0" }] });
                    LicenseeWinLoss.goBack(0);

                    expect(LicenseeWinLoss.bindData).toHaveBeenCalledWith(0, 2, { Data: [{ Name: "Test Level 0" }] });
                });

                describe("level equal 0", function () {
                    beforeEach(function () {
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
                        expect(args[1].toDateString()).toBe("Wed Feb 01 2017");
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
        });
    });