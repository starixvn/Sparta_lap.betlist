define(
    ['jquery', 'common/site', 'common/querystring', 'pages/betlist/betlist', 'pages/betlist/betlistcancelledbets'],
    function ($, Site, QueryString, BetList, BetListCancelledBets) {
        describe("BetListCancelledBets", function () {
            beforeAll(function () {
                spyOn(Site, "getSingleResource").and.callFake(function (text) {
                    return text;
                });

                spyOn(Site, "resolveClientUrl").and.callFake(function (url) {
                    return url;
                });

                spyOn(Site, "setLicenseeOptionParam").and.callFake(function (param) {
                    param.CurrencyId = 2;
                    return param;
                });
            });

            describe("initialize()", function () {
                beforeEach(function () {
                    spyOn(BetListCancelledBets, "initEvent");
                    spyOn(BetListCancelledBets, "submit");
                    spyOn(Site, "addUserGuide");

                    BetListCancelledBets.initialize();
                });

                it("initEvent() should be called.", function () {
                    expect(BetListCancelledBets.initEvent).toHaveBeenCalled();
                });

                it("submit() should be called.", function () {
                    expect(BetListCancelledBets.submit).toHaveBeenCalled();
                });

                it("Site.addUserGuide() should be called with correct parameters.", function () {
                    expect(Site.addUserGuide).toHaveBeenCalledWith({
                        resourceName: "CancelledBets",
                        title: "CancelledBets"
                    });
                });
            });

            describe("initEvent()", function () {
                it("submit() should be called when #CustId fired change event", function () {
                    setFixtures('<input id="CustId" type="text" />');
                    spyOn(BetListCancelledBets, "submit");

                    BetListCancelledBets.initEvent();
                    $("#CustId").change();

                    expect(BetListCancelledBets.submit).toHaveBeenCalled();
                });

                it("submit() should be called when #SubmitButton is clicked.", function () {
                    setFixtures('<input id="SubmitButton" type="button" />');
                    spyOn(BetListCancelledBets, "submit");

                    BetListCancelledBets.initEvent();
                    $("#SubmitButton").click();

                    expect(BetListCancelledBets.submit).toHaveBeenCalled();
                });

                it("exportToExcel() should be called when #ExportToExcel is clicked.", function () {
                    setFixtures('<input id="ExportToExcel" type="button" />');
                    spyOn(BetListCancelledBets, "exportToExcel");
                    BetListCancelledBets.initEvent();

                    $("#ExportToExcel").click();

                    expect(BetListCancelledBets.exportToExcel).toHaveBeenCalled();
                });
            });

            describe("submit()", function () {
                beforeEach(function () {
                    setFixtures('<input id="CustId" type="text" value="1" />');

                    spyOn(BetList, "initialize");
                    spyOn(BetList, "loadData");
                    BetListCancelledBets.submit();
                });

                it("BetList.initialize() should be called", function () {
                    expect(BetList.initialize).toHaveBeenCalled();
                });

                it("Site.setLicenseeOptionParam(option) should be called with correct parameter", function () {
                    expect(Site.setLicenseeOptionParam).toHaveBeenCalled();
                });

                it("BetList.loadData(params, url) should be called with correct parameter", function () {
                    expect(BetList.loadData).toHaveBeenCalledWith({ custId: "1", CurrencyId: 2 }, 'api/CancelledBetsBetList/Index');
                });
            });

            describe("exportToExcel()", function () {
                var spyEvent;
                beforeEach(function () {
                    setFixtures(
                        '<input id="CustId" type="text" value="1" />' +
                        '<input id="IsExportExcel" type="text" value="false" />' +
                        '<form id="CancellerBetsBetListForm"></form>');
                    spyOn(Site, "getCurrentUrl").and.returnValue("localhost-test");
                    spyEvent = spyOnEvent($('#CancellerBetsBetListForm'), 'submit');
                    $('#CancellerBetsBetListForm').submit(function () { return false; });

                    BetListCancelledBets.exportToExcel();
                });

                afterEach(function () {
                    spyEvent = null;
                });

                it("Site.getCurrentUrl() should be called", function () {
                    expect(Site.getCurrentUrl).toHaveBeenCalled();
                });
                it("Site.setLicenseeOptionParam(url) should be called with correct paramter", function () {
                    expect(Site.setLicenseeOptionParam).toHaveBeenCalledWith("localhost-test");
                });
                it("#IsExportExcel value should be true", function () {
                    var result = $('#IsExportExcel').val() === "true";
                    expect(result).toBeTruthy();
                });
                it("form submit() function should be called", function () {
                    expect(spyEvent).toHaveBeenTriggered();
                });
            });
        });
    });