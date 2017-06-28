define(["jquery", "common/site", "common/querystring", "components/pluginhelper/daterangepicker", "components/paging",
    "components/scrollablegrid", "helpers/sortableheader", "pages/report/racingcommission/racingcommission", "extensions/jquery.extension"],
    function ($, Site, QueryString, DatePicker, Paging, GridHelper, SortableHeader, RacingCommission) {
        describe("RacingCommission", function () {
            beforeAll(function () {
                spyOn(Site, "getSingleResource").and.callFake(function (text) {
                    return text;
                });
            });

            describe("initialize()", function () {
                beforeEach(function () {
                    spyOn(Site, "initialize");
                    spyOn(RacingCommission, "initEvent");
                    spyOn(SortableHeader, "createSortingIcon");
                    spyOn(Site, "addUserGuide");
                    spyOn(RacingCommission, "initDateRangePicker");
                    spyOn(GridHelper, "resizeGrid");
                    spyOn(Paging, "initialize");

                    RacingCommission.initialize();
                });

                it("Site.initialize() should be called", function () {
                    expect(Site.initialize).toHaveBeenCalled();
                });
                it("RacingCommission.initEvent() should be called", function () {
                    expect(RacingCommission.initEvent).toHaveBeenCalled();
                });
                it("SortableHeader.createSortingIcon() should be called with correct parameter", function () {
                    expect(SortableHeader.createSortingIcon).toHaveBeenCalledWith(RacingCommission.submitForm);
                });
                it("Site.addUserGuide() should be called with correct parameter", function () {
                    expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "RacingCommission", title: "RacingCommission" });
                });
                it("RacingCommission.initDateRangePicker() should be called", function () {
                    expect(RacingCommission.initDateRangePicker).toHaveBeenCalled();
                });
                it("GridHelper.resizeGrid() should be called", function () {
                    expect(GridHelper.resizeGrid).toHaveBeenCalled();
                });
                it("Paging.initialize() should be called", function () {
                    expect(Paging.initialize).toHaveBeenCalled();
                });
            });

            describe("initDateRangePicker()", function () {
                it("DatePicker.initDateRangePicker(firstDayOfWeekIsMonday, rangeOption) should be called with correct parameter", function () {
                    spyOn(DatePicker, "initDateRangePicker");
                    RacingCommission.initDateRangePicker();

                    expect(DatePicker.initDateRangePicker).toHaveBeenCalledWith(false, ["Today", "Yesterday", "Currentweek"]);
                });
            });

            describe("initEvent()", function () {
                beforeEach(function () {
                    setFixtures('<button id="ExportToExcel"></button>' +
                        '<button id="SubmitButton"></button>' +
                        '<div class="body-content"><a class="bet-list-link" data-cust-id="1" data-system-id="2" data-external-id="3"></a></div>' +
                        '<input id="IsExportExcel" value="false" />' +
                        '<input id="SortColumn" value="Column1" />' +
                        '<input id="SortDirection" value="DESC" />' +
                        '<form id="ReportForm"></form>'
                        );
                    $("body").off();

                    RacingCommission.initEvent();
                });

                afterEach(function () {
                    $(".body-content").remove();
                    $("body").off();
                });

                describe("'#ExportToExcel' fire click event", function () {
                    var spyEvent;
                    beforeEach(function () {
                        spyEvent = spyOnEvent($('#ReportForm'), 'submit');
                        $('#ReportForm').submit(function () { return false; });

                        $("#ExportToExcel").click();
                    });
                    it("'#IsExportExcel' should has value is true", function () {
                        expect($('#IsExportExcel').val()).toBe("true");
                    });

                    it("'#ReportForm' should trigger submit event", function () {
                        expect(spyEvent).toHaveBeenTriggered();
                    });
                });

                describe("'#SubmitButton' fire click event", function () {
                    beforeEach(function () {
                        spyOn(RacingCommission, "submitForm");
                    });

                    it("'#SortColumn' should has correct value", function () {
                        $("#SubmitButton").click();

                        expect($("#SortColumn").val()).toBe("ExternalID");
                    });

                    it("'#SortDirection' should has correct value", function () {
                        $("#SubmitButton").click();

                        expect($("#SortDirection").val()).toBe("ASC");
                    });

                    it("RacingCommission.submitForm() should be called", function () {
                        $("#SubmitButton").click();

                        expect(RacingCommission.submitForm).toHaveBeenCalled();
                    });
                });

                it("RacingCommission.viewRacingBetList(custid, systemId, externalId) should be called with correct parameter when '.bet-list-link' fire click event", function () {
                    spyOn(RacingCommission, "viewRacingBetList");

                    $(".body-content").find(".bet-list-link").click();

                    expect(RacingCommission.viewRacingBetList).toHaveBeenCalledWith("1", "2", "3");
                });

                it("GridHelper.resizeGrid() should be called when window trigger resize event", function () {
                    spyOn(GridHelper, "resizeGrid");

                    $(window).resize();

                    expect(GridHelper.resizeGrid).toHaveBeenCalled();
                });

                it(".off() function should be called with correct parameter when window trigger unload event", function () {
                    spyOn($.fn, "off");

                    $(window).unload();

                    expect($.fn.off).toHaveBeenCalledWith("resize");
                });
            });

            describe("viewRacingBetList(custid, systemId, externalId)", function () {
                beforeEach(function () {
                    setFixtures('<input id="FromDate" value="03/01/2017" /> <input id="ToDate" value="03/31/2017" />');
                    spyOn(Site, "resolveClientUrl").and.callFake(function (url) {
                        return url;
                    });

                    spyOn(Site, "setLicenseeOptionOriginalParam").and.callFake(function (url) { return url + "?basecurrency=1"; });
                    spyOn(Site, "blockUI");
                    spyOn(Site, "redirect");

                    RacingCommission.viewRacingBetList(1, 2, 3);
                });

                it("Site.setLicenseeOptionOriginalParam(url) should be called with correct parameter", function () {
                    expect(Site.setLicenseeOptionOriginalParam).toHaveBeenCalledWith("RacingCommissionBetList/Index");
                });

                it("Site.blockUI() should be called", function () {
                    expect(Site.blockUI).toHaveBeenCalled();
                });

                it("Site.redirect(url) should be called with correct parameter", function () {
                    var url = "RacingCommissionBetList/Index?basecurrency=1&fromdate=03/01/2017&todate=03/31/2017&custid=1&systemId=2&externalId=3";
                    var expectedUrl = url.replace(/[&|?][^=]*/g, function (match) {
                        return match.toLowerCase();
                    });

                    expect(Site.redirect).toHaveBeenCalledWith(expectedUrl);
                });
            });

            describe("submitForm()", function () {
                var spyEvent;
                beforeEach(function () {
                    setFixtures('<input id="IsExportExcel" />' +
                        '<form id="ReportForm"></form>'
                        );

                    spyEvent = spyOnEvent($('#ReportForm'), 'submit');
                    $('#ReportForm').submit(function () { return false; });
                    spyOn(Site, "blockUI");

                    RacingCommission.submitForm();
                });

                it("Site.blockUI() should be called", function () {
                    expect(Site.blockUI).toHaveBeenCalled();
                });
                it("'#IsExportExcel' should has correct value", function () {
                    expect($("#IsExportExcel").val()).toBe("false");
                });
                it("'#ReportForm' should trigger submit event", function () {
                    expect(spyEvent).toHaveBeenTriggered();
                });
            });
        });
    });