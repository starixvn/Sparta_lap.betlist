define(
    ['jquery', 'common/site', 'common/querystring', 'pages/betlist/betlist', 'components/pluginhelper/daterangepicker', 'pages/betlist/betlistdaily'],
    function ($, Site, QueryString, BetList, DatePicker, BetListDaily) {
        describe("BetListDaily", function () {
            function transformQueryParamNameToLowerCase(url) {
                var paramNameRegex = /[&|?][^=]*/g;

                return url.replace(paramNameRegex, function (match) {
                    return match.toLowerCase();
                });
            }

            beforeAll(function () {
                spyOn(Site, "getSingleResource").and.callFake(function (text) {
                    return text;
                });

                spyOn(Site, "resolveClientUrl").and.callFake(function (url) {
                    return url;
                });
            });

            describe("initialize()", function () {
                beforeEach(function () {
                    setFixtures('<input id="LicenseeId" value="1" />');

                    spyOn(Site, "initialize");
                    spyOn(BetListDaily, "initEvent");
                    spyOn(Site, "addUserGuide");
                    spyOn(DatePicker, "initSingleDatePicker");

                    BetListDaily.initialize();
                });

                it("Site.initialize() should be called", function () {
                    expect(Site.initialize).toHaveBeenCalled();
                });

                it("BetListDaily.initEvent() should be called", function () {
                    expect(BetListDaily.initEvent).toHaveBeenCalled();
                });

                it("Site.addUserGuide(option) should be called with correct parameter", function () {
                    expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "DailyBetList", title: "DailyBetList" });
                });

                it("DatePicker.initSingleDatePicker() should be called with correct parameter", function () {
                    expect(DatePicker.initSingleDatePicker).toHaveBeenCalledWith("Date");
                });
            });

            describe("initEvent()", function () {
                beforeEach(function () {
                    setFixtures('<div id="SubmitButton" style="height: 10px; width: 10px;"></div>' +
                        '<div id="ExportToExcel" style="height: 10px; width: 10px;"></div>' +
                        '<input id="CurrencyId" value="1" />'
                        );

                    BetListDaily.initEvent();
                });

                it("BetListDaily.submit() should be called when '#SubmitButton' fired click event", function () {
                    spyOn(BetListDaily, "submit");

                    $("#SubmitButton").click();

                    expect(BetListDaily.submit).toHaveBeenCalled();
                });

                it("BetListDaily.exportToExcel() should be called when '#ExportToExcel' fired click event", function () {
                    spyOn(BetListDaily, "exportToExcel");

                    $("#ExportToExcel").click();

                    expect(BetListDaily.exportToExcel).toHaveBeenCalled();
                });

                describe("'#CurrencyId' fire change event", function () {
                    beforeEach(function () {
                        appendSetFixtures('<input id="LicenseeId" value="2" />');

                        spyOn(Site, "initialize");
                        spyOn(Site, "addUserGuide");
                        spyOn(DatePicker, "initSingleDatePicker");
                    });

                    it("'#ExportToExcel' should be showed when licensee is KeenOcean (96)", function () {
                        $("#LicenseeId").val(96);
                        BetListDaily.initialize();

                        $("#CurrencyId").change();

                        expect($("#ExportToExcel")).toBeVisible();
                    });

                    it("'#ExportToExcel' should be showed when we select specific currencyId value and licensee is not KeenOcean (96)", function () {
                        BetListDaily.initialize();

                        $("#CurrencyId").change();

                        expect($("#ExportToExcel")).toBeVisible();
                    });

                    it("'#ExportToExcel' should be hidden when we select all currency and licensee is not KeenOcean (96)", function () {
                        BetListDaily.initialize();
                        $("#CurrencyId").val(0);

                        $("#CurrencyId").change();

                        expect($("#ExportToExcel")).not.toBeVisible();
                    });
                });
            });

            describe("submit()", function () {
                it("BetList.loadDataPaging(url) should be called with correct parameter", function () {
                    setFixtures('<input id="ProductCode" value="2" />' +
                        '<input id="DailyBetListStatus" value="won" />' +
                        '<input id="Date" value="03/28/2017" />'
                        );
                    spyOn(Site, "initialize");
                    spyOn(Site, "setLicenseeOptionParam").and.callFake(function (url) { return url + "?currencyid=1&basecurrency=EUR"; });
                    spyOn(BetList, "loadDataPaging");
                    var url = "api/DailyBetList/index?currencyid=1&basecurrency=EUR&productCode=2&status=won&date=03/28/2017";
                    var expectedUrl = transformQueryParamNameToLowerCase(url);

                    BetListDaily.submit();

                    expect(BetList.loadDataPaging).toHaveBeenCalledWith(expectedUrl);
                });
            });

            describe("exportToExcel()", function () {
                it("Site.redirect(url) should be called with correct parameter", function () {
                    setFixtures('<input id="ProductCode" value="2" />' +
                        '<input id="DailyBetListStatus" value="won" />' +
                        '<input id="Date" value="03/28/2017" />'
                        );
                    spyOn(Site, "redirect");
                    spyOn(Site, "getCurrentUrl").and.returnValue("localhost");
                    spyOn($.fn, "getGridParam").and.callFake(function (param) { if (param === "rowNum") { return 10; } return null; });
                    spyOn(Site, "setLicenseeOptionParam").and.callFake(function (url) { return url + "?currencyid=1&basecurrency=EUR"; });
                    spyOn(BetList, "getSortParams").and.returnValue({ SortBy: "test-column", SortOrder: "asc" });

                    var url = "localhost?currencyid=1&basecurrency=EUR&IsExportExcel=true&date=03/28/2017&productCode=2&DailyBetListStatus=won&PageIndex=1&PageSize=10&SortBy=test-column&SortOrder=asc";
                    var expectedUrl = transformQueryParamNameToLowerCase(url);

                    BetListDaily.exportToExcel();

                    expect(Site.redirect).toHaveBeenCalledWith(expectedUrl);
                });

                it("Do not have sortParams - Site.redirect(url) should be called with correct parameter", function () {
                    setFixtures('<input id="ProductCode" value="2" />' +
                        '<input id="DailyBetListStatus" value="won" />' +
                        '<input id="Date" value="03/28/2017" />'
                        );
                    spyOn(Site, "redirect");
                    spyOn(Site, "getCurrentUrl").and.returnValue("localhost");
                    spyOn($.fn, "getGridParam").and.callFake(function (param) { if (param === "rowNum") { return 10; } return null; });
                    spyOn(Site, "setLicenseeOptionParam").and.callFake(function (url) { return url + "?currencyid=1&basecurrency=EUR"; });
                    spyOn(BetList, "getSortParams").and.returnValue(null);

                    var url = "localhost?currencyid=1&basecurrency=EUR&IsExportExcel=true&date=03/28/2017&productCode=2&DailyBetListStatus=won&PageIndex=1&PageSize=10";
                    var expectedUrl = transformQueryParamNameToLowerCase(url);

                    BetListDaily.exportToExcel();

                    expect(Site.redirect).toHaveBeenCalledWith(expectedUrl);
                });
            });
        });
    });