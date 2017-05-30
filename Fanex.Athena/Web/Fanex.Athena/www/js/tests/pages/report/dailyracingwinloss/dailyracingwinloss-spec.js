define(["jquery", "common/site", "common/querystring", "components/pluginhelper/daterangepicker",
    "components/scrollablegrid", "pages/report/dailyracingwinloss/dailyracingwinloss", "extensions/jquery.extension"],
    function ($, Site, QueryString, DatePicker, GridHelper, DailyRacingWinLoss) {
        describe("DailyRacingWinLoss", function () {
            beforeAll(function () {
                spyOn(Site, "getSingleResource").and.callFake(function (resource) { return resource; });
                spyOn(Site, "resolveClientUrl").and.callFake(function (url) { return url; });
            });

            describe("initialize()", function () {
                beforeEach(function () {
                    spyOn(Site, "initialize");
                    spyOn(DatePicker, "initSingleDatePicker");
                    spyOn(Site, "addUserGuide");
                    spyOn(DailyRacingWinLoss, "initEvent");
                    spyOn(GridHelper, "resizeGrid");

                    DailyRacingWinLoss.initialize();
                });

                it("Site.initialize() should be called", function () {
                    expect(Site.initialize).toHaveBeenCalled();
                });
                it("DatePicker.initSingleDatePicker() should be called with correct parameter", function () {
                    expect(DatePicker.initSingleDatePicker).toHaveBeenCalledWith("Date");
                });
                it("Site.addUserGuide() should be called with correct parameter", function () {
                    expect(Site.addUserGuide).toHaveBeenCalledWith({
                        resourceName: "DailyRacingWL",
                        title: "DailyRacingWinLoss"
                    });
                });
                it("DailyRacingWinLoss.initEvent() should be called", function () {
                    expect(DailyRacingWinLoss.initEvent).toHaveBeenCalled();
                });
                it("GridHelper.resizeGrid() should be called", function () {
                    expect(GridHelper.resizeGrid).toHaveBeenCalled();
                });
            });

            describe("initEvent", function () {
                beforeEach(function () {
                    $("body").off();
                    setFixtures('<button id="SubmitButton"></button>' +
                        '<button id="ExportToExcel"></button><input id="IsExportExcel" /><div id="ReportForm"></div>' +
                        '<div class="body-content"><a class="bet-list-link" data-league="1" data-race-no="2" data-currency="3"></a></div>');

                    DailyRacingWinLoss.initEvent();
                });

                afterEach(function () {
                    $("body").off();
                });

                it("'#SubmitButton' fired click event - DailyRacingWinLoss.submitForm should be called", function () {
                    spyOn(DailyRacingWinLoss, "submitForm");

                    $("#SubmitButton").click();

                    expect(DailyRacingWinLoss.submitForm).toHaveBeenCalled();
                });

                it("'#ExportToExcel' fired click event - '#IsExportExcel' should has correct value", function () {
                    $("#ExportToExcel").click();

                    var result = $("#IsExportExcel").val();

                    expect(result).toBe("true");
                });

                it("'#ExportToExcel' fired click event - '#ReportForm' fire submit event", function () {
                    spyOn($.fn, "submit");

                    $("#ExportToExcel").click();

                    expect($.fn.submit).toHaveBeenCalled();
                });

                it("'.bet-list-link' fired click event - DailyRacingWinLoss.viewRacingWinlossBetList() should be called with correct parameter", function () {
                    spyOn(DailyRacingWinLoss, "viewRacingWinlossBetList");

                    $(".bet-list-link").click();

                    expect(DailyRacingWinLoss.viewRacingWinlossBetList).toHaveBeenCalledWith("1", "2", "3");
                });

                it("GridHelper.resizeGrid() should be called when window fire resize event", function () {
                    spyOn(GridHelper, "resizeGrid");

                    $(window).resize();

                    expect(GridHelper.resizeGrid).toHaveBeenCalled();
                });

                it("off() jquery function should be called with correct parameter when window fire unload event", function () {
                    spyOn($.fn, "off");

                    $(window).unload();

                    expect($.fn.off).toHaveBeenCalledWith("resize");
                });
            });

            describe("viewRacingWinlossBetList(leagueid, raceNo, currency)", function () {
                beforeEach(function () {
                    setFixtures('<input id="Date" value="05/01/2017" />');
                    spyOn(Site, "setLicenseeOptionOriginalParam").and.callFake(function (url) { return url; });
                    spyOn(Site, "blockUI");
                    spyOn(Site, "redirect");

                    DailyRacingWinLoss.viewRacingWinlossBetList(1, 2, 3);
                });

                it("Site.blockUI() should be called", function () {
                    expect(Site.blockUI).toHaveBeenCalled();
                });

                it("Site.redirect(url) should be called with correct parameter", function () {
                    var expectedParam = "DailyRacingWinLossBetList/Index?date=05/01/2017&leagueid=1&raceno=2&type=RacingWinloss&currencyid=3";

                    expect(Site.redirect).toHaveBeenCalledWith(expectedParam);
                });
            });

            describe("submitForm()", function () {
                beforeEach(function () {
                    setFixtures('<input id="IsExportExcel" /><div id="ReportForm"></div>');
                    spyOn($.fn, "submit");

                    DailyRacingWinLoss.submitForm();
                });

                it("'#IsExportExcel' should has correct value", function () {
                    var result = $("#IsExportExcel").val();

                    expect(result).toBe("false");
                });

                it("'#ReportForm' fire submit event", function () {
                    expect($.fn.submit).toHaveBeenCalled();
                });
            });
        });
    });