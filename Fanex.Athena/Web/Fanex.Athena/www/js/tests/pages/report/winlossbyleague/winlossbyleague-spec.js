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

                        appendSetFixtures("<div id='level-link-container'><div class='level-link' data-level='0'></div></div>");
                        appendSetFixtures("<div class='drilldown' data-custid='1' data-externalid='2' data-systemid='3' />");
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

                    it("WinLossByLeague.drillDownBetList(custid, leagueid, leaguename) should be called when '.drilldown' fired click event", function () {
                        spyOn(WinLossByLeague, "drillDownBetList");

                        $('.drilldown').click();

                        expect(WinLossByLeague.drillDownBetList).toHaveBeenCalledWith(1, 2, 3);
                    });
                });

                describe("setReportTitle(level, minOdds, fromdate, todate)", function () {
                    it("Report title should be correct text", function () {
                        var titleExpected = 'WinLossByLeague, 2/01/2016 → 2/29/2016 - Currency:  undefined - MinimumOdds : 1';

                        setFixtures("<div class='report-title' id='caption' />");
                        var minOddStr = Site.getSingleResource("MinimumOdds") + " : 1";

                        WinLossByLeague.setReportTitle(0, 1, "2016/02/01", "2016/02/29");

                        expect($("#caption").html()).toBe(titleExpected);
                    });
                });
            });
        });