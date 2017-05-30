define(['jquery', 'common/site', 'components/pluginhelper/daterangepicker', 'pages/report/winlossbyleague/winlossbyleague.option',
    'pages/report/winlossbyleague/winlossbyleague.filter', 'pages/report/winlossbyleague/winlossbyleague', 'pages/report/report', 'lib/jqgrid/jquery.jqgrid.additional', 'common/popup',
    'components/pluginhelper/multiselect', "components/paging"],
        function ($, Site, DatePicker, WinLossByLeagueOption, WinLossByLeagueFilter, WinLossByLeague, Report, jqGridAdditional, Popup, MultiSelect, PagingHelper) {
            describe("WinLossByLeague", function () {
                beforeAll(function () {
                    spyOn(Site, "getSingleResource").and.callFake(function (resource) { return resource; });
                    spyOn(Site, "initialize");
                    spyOn(Site, "resolveClientUrl").and.callFake(function (url) {
                        return url;
                    });
                });

                describe("ajaxPaging()", function () {
                    beforeAll(function () {
                        spyOn(PagingHelper, "getSetting");
                        spyOn(WinLossByLeague, "drillDown");

                        WinLossByLeague.ajaxPaging();
                    });

                    it("PagingHelper.getSetting() should be called", function () {
                        expect(PagingHelper.getSetting).toHaveBeenCalled();
                    });

                    it("WinLossByLeague.drillDown(leagueid, leaguename) should be called with correct parameters", function () {
                        expect(WinLossByLeague.drillDown).toHaveBeenCalledWith(-1, null);
                    });
                });

                describe("initialize()", function () {
                    it("WinLossByLeague.initEvent() should be called", function () {
                        spyOn(WinLossByLeague, "initEvent");
                        spyOn(WinLossByLeagueFilter, "initialize");

                        WinLossByLeague.initialize();

                        expect(WinLossByLeague.initEvent).toHaveBeenCalled();
                        expect(WinLossByLeagueFilter.initialize).toHaveBeenCalled();
                    });
                });

                describe("initEvent()", function () {
                    var $submitButton;

                    beforeEach(function () {
                        setFixtures("<input type='button' id='SubmitButton' />");
                        appendSetFixtures("<div id='leagueHeader' data-leagueid='1'>League Name Here</div>");
                        appendSetFixtures("<div id='level-link-container'><div class='level-link' data-level='0'></div></div>");
                        appendSetFixtures("<div class='drilldown' data-custid='1' data-externalid='2' data-systemid='3' />");
                        appendSetFixtures("<div id='ExportToExcel'></div>");
                        $submitButton = $('#SubmitButton');

                        WinLossByLeague.initEvent();
                    });

                    it("WinLossByLeague.submit() should be called when '#SubmitButton' fired click event", function () {
                        spyOn(WinLossByLeague, "submit");

                        $submitButton.click();

                        expect(WinLossByLeague.submit).toHaveBeenCalled();
                    });

                    it("WinLossByLeague.submit() should be called when '.level-link' fired click event", function () {
                        spyOn(WinLossByLeague, "submit");

                        $('.level-link').click();

                        expect(WinLossByLeague.submit).toHaveBeenCalled();
                    });

                    it("WinLossByLeague.drillDown(leagueid, leaguename) should be called when '#leagueHeader' is clicked", function () {
                        spyOn(WinLossByLeague, "drillDown");

                        $('#leagueHeader').click();

                        expect(WinLossByLeague.drillDown).toHaveBeenCalledWith(1, 'League Name Here');
                    });

                    it("WinLossByLeague.drillDownBetList(custid, leagueid, leaguename) should be called when '.drilldown' fired click event", function () {
                        spyOn(WinLossByLeague, "drillDownBetList");

                        $('.drilldown').click();

                        expect(WinLossByLeague.drillDownBetList).toHaveBeenCalledWith(1, 2, 3);
                    });

                    it("Site.exportExcel() should be called when '#ExportToExcel' is clicked", function () {
                        spyOn(Site, "exportExcel");

                        $('#ExportToExcel').click();

                        expect(Site.exportExcel).toHaveBeenCalledWith(jasmine.any(Object), "api/WinLossByLeagueReport/Excel");
                    });

                    it("WinLossByLeague.resizeGrid() function should be called when window fire resize event", function () {
                        spyOn(WinLossByLeague, "resizeGrid");

                        $(window).resize();

                        expect(WinLossByLeague.resizeGrid).toHaveBeenCalled();
                    });

                    it("$(window).off() should be called with correct parameter when window fire unload event", function () {
                        spyOn($.fn, "off");

                        $(window).unload();

                        expect($.fn.off).toHaveBeenCalledWith("resize");
                    });
                });

                describe("resizeGrid()", function () {
                    it("Corresponding methods should be called", function () {
                        setFixtures('<div id="ReportGrid"></div>');
                        spyOn(jqGridAdditional, "updateGridHeight");
                        spyOn(jqGridAdditional, "updateGridWidth");

                        WinLossByLeague.resizeGrid();

                        expect(jqGridAdditional.updateGridHeight).toHaveBeenCalled();
                        expect(jqGridAdditional.updateGridWidth).toHaveBeenCalledWith($("#ReportGrid"));
                    });
                });

                describe("setReportTitle(level, minOdds, fromdate, todate)", function () {
                    it("Report title should be correct text when level = 0", function () {
                        var titleExpected = 'WinLossByLeague, 2/01/2016 → 2/29/2016 - Currency:  undefined - MinimumOdds : 1';

                        setFixtures("<div class='report-title' id='caption' />");
                        var minOddStr = Site.getSingleResource("MinimumOdds") + " : 1";

                        WinLossByLeague.setReportTitle(0, 1, "2016/02/01", "2016/02/29");

                        expect($("#caption").html()).toBe(titleExpected);
                    });

                    it("Report title should be correct text  when level = 1", function () {
                        var titleExpected = 'null &gt; CustomerWinLoss, 2/01/2016 → 2/29/2016 - Currency:  undefined - MinimumOdds : 1';

                        setFixtures("<div class='report-title' id='caption' />");
                        var minOddStr = Site.getSingleResource("MinimumOdds") + " : 1";

                        WinLossByLeague.setReportTitle(1, 1, "2016/02/01", "2016/02/29");

                        expect($("#caption").html()).toBe(titleExpected);
                    });
                });

                describe("ajaxComplete", function () {
                    beforeEach(function () {
                        spyOn(Report, "renderBreadCrumb");
                        spyOn(WinLossByLeague, "bindData");
                        spyOn(WinLossByLeague, "setReportTitle");
                    });

                    it("Nothing happen if response param has null value", function () {
                        WinLossByLeague.ajaxComplete(0, null);

                        expect(WinLossByLeague.bindData).not.toHaveBeenCalled();
                        expect(Report.renderBreadCrumb).not.toHaveBeenCalled();
                    });

                    it("WinLossByLeague.setReportTitle() should be called if level is 0", function () {
                        WinLossByLeague.ajaxComplete(0, {});

                        expect(WinLossByLeague.setReportTitle).toHaveBeenCalled();
                    });

                    it("WinLossByLeague.setReportTitle() should NOT be called if level param greater than 0", function () {
                        WinLossByLeague.ajaxComplete(1, {});

                        expect(WinLossByLeague.setReportTitle).not.toHaveBeenCalled();
                    });

                    it("Report.renderBreadCrumb(level) should be called with correct parameter", function () {
                        var level = 1;
                        WinLossByLeague.ajaxComplete(level, {});

                        expect(Report.renderBreadCrumb).toHaveBeenCalledWith(level);
                    });

                    it("WinLossByLeague.bindData(level) should be called with correct parameter", function () {
                        var level = 1;
                        var response = { Data: "Hello" };

                        WinLossByLeague.ajaxComplete(level, response);

                        expect(WinLossByLeague.bindData).toHaveBeenCalledWith(level, response);
                    });
                });

                describe("submit()", function () {
                    var request;
                    beforeEach(function (done) {
                        jasmine.Ajax.install();

                        var submitData = {
                            BaseCurrency: "eur"
                        };

                        setFixtures('<form id="SearchForm"></form>');
                        appendSetFixtures('<input id="CurrencyName" value="ABC" />');
                        appendSetFixtures('<select id="League"><option value="100" selected>League Test</option></select>');
                        spyOn(WinLossByLeagueFilter, "isValid").and.returnValue(true);
                        spyOn($.fn, "serializeObject").and.returnValue(submitData);
                        spyOn(WinLossByLeague, "ajaxComplete");
                        spyOn(Site, "blockUI");
                        spyOn(Site, "unBlockUI");

                        jasmine.Ajax
                                .stubRequest("api/WinLossByLeagueReport/Index")
                                .andReturn({
                                    'status': 200,
                                    'contentType': 'application/json',
                                    'responseText': '{ "ProductOptions": "SB,RC" }'
                                });

                        WinLossByLeague.submit();
                        request = jasmine.Ajax.requests.mostRecent();

                        setTimeout(done, 10);
                    });
                    afterEach(function () {
                        jasmine.Ajax.uninstall();
                    });

                    it("request url should be correct", function () {
                        var expectedUrl = "api/WinLossByLeagueReport/Index";

                        expect(request.url).toBe(expectedUrl);
                    });
                    it("Site.blockUI() should be called", function () {
                        expect(Site.blockUI).toHaveBeenCalled();
                    });
                    it("Site.unBlockUI() should be called", function () {
                        expect(Site.unBlockUI).toHaveBeenCalled();
                    });
                    it("WinLossByLeague.ajaxComplete(level, response) should be called with correct parameters", function () {
                        expect(WinLossByLeague.ajaxComplete).toHaveBeenCalledWith(0, { ProductOptions: "SB,RC" });
                    });
                });

                describe("drillDownBetList(custId, externalId, systemId)", function () {
                    it("WinLossByLeagueFilter.buildProductsParams() should be called", function () {
                        spyOn(WinLossByLeagueFilter, "buildProductsParams");

                        WinLossByLeague.drillDownBetList(0, 0, 0);

                        expect(WinLossByLeagueFilter.buildProductsParams).toHaveBeenCalled();
                    });

                    it("window.open() should be called", function () {
                        spyOn(window, "open");

                        WinLossByLeague.drillDownBetList(0, 0, 0);

                        expect(window.open).toHaveBeenCalledWith(jasmine.stringMatching(/WinlossByLeagueBetList\/Index\?.*/), "", 'modal=no');
                    });
                });

                describe("bindTotal(level, totalData)", function () {
                    it("WinLossByLeagueOption.getColModel(level) should be called with correct parameter", function () {
                        spyOn(WinLossByLeagueOption, "getColModel");

                        WinLossByLeague.bindTotal(0, 100);

                        expect(WinLossByLeagueOption.getColModel).toHaveBeenCalledWith(0);
                    });

                    it("jqGridAdditional.assignTotal(totalData, columnModel) should be called", function () {
                        spyOn(jqGridAdditional, "assignTotal");

                        WinLossByLeague.bindTotal(0, 100);

                        expect(jqGridAdditional.assignTotal).toHaveBeenCalled();
                    });

                    it("jqGridAdditional.assignTotal(totalData, columnModel) should be called with correct parameter", function () {
                        spyOn(jqGridAdditional, "setGroupFooters");

                        WinLossByLeague.bindTotal(0, 100);

                        expect(jqGridAdditional.setGroupFooters).toHaveBeenCalledWith(2, 0, 0);
                    });
                });

                describe("bindData(level, response)", function () {
                    beforeEach(function () {
                        setFixtures("<div id='ReportGrid'></div>");

                        spyOn(jqGridAdditional, "clearGrid");
                        spyOn(WinLossByLeagueOption, "getGridOption").and.returnValue({});
                        spyOn($.fn, "jqGrid");
                        spyOn(jqGridAdditional, "gridComplete");
                        spyOn(WinLossByLeague, "resizeGrid");

                        WinLossByLeague.bindData(2, { Data: [1, 2, 3], Total: [4, 5, 6] });
                    });

                    it("jqGridAdditional.clearGrid(reportId) should be called with correct parameter", function () {
                        expect(jqGridAdditional.clearGrid).toHaveBeenCalledWith("ReportGrid");
                    });
                    it("WinLossByLeagueOption.getGridOption(level) should be called with correct parameter", function () {
                        expect(WinLossByLeagueOption.getGridOption).toHaveBeenCalledWith(2);
                    });
                    it("jqGrid function of jquery should be called", function () {
                        expect($.fn.jqGrid).toHaveBeenCalled();
                    });
                });
            });
        });