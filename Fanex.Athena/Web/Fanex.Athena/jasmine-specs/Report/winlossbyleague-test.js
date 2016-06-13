describe("WinLossByLeague", function () {
    describe("ajaxPaging()", function () {
        beforeEach(function () {
            spyOn(pagingHeper, "getSetting");
            spyOn(WinLossByLeague, "drillDown");

            WinLossByLeague.ajaxPaging();
        });

        it("pagingHeper.getSetting() should be called", function () {
            expect(pagingHeper.getSetting).toHaveBeenCalled();
        });

        it("WinLossByLeague.drillDown(leagueid, leaguename) should be called with correct parameters", function () {
            expect(WinLossByLeague.drillDown).toHaveBeenCalledWith(-1, null);
        });
    });

    describe("initialize()", function () {
        var $searchForm, $league, $currencyId, $sport, $submitButton, $drilldown, $backlink;
        beforeEach(function () {
            $searchForm = affix("#searchForm");
            $league = affix("input#League[value=1]");
            $currencyId = affix("input#CurrencyId[value=0]");
            $sport = affix("input#Sport[value=2]");
            $submitButton = affix("#SubmitButton");
            $drilldown = affix(".drilldown");
            $backlink = affix("#BackLink");

            spyOn(WinLossByLeague, "initEvent");
            spyOn(WinLossByLeague, "initSetting");
            spyOn(Site, "addFilterCollapseExpand");

            spyOn($.fn, "monPickr").and.returnValue({});
            spyOn($.fn, "checkboxgroup").and.returnValue({});

            WinLossByLeague.initialize();
        });

        it("WinLossByLeague.initEvent() should be called", function () {
            expect(WinLossByLeague.initEvent).toHaveBeenCalled();
        });
        it("WinLossByLeague.initSetting() should be called", function () {
            expect(WinLossByLeague.initSetting).toHaveBeenCalled();
        });
        it("Site.addFilterCollapseExpand() should be called", function () {
            expect(Site.addFilterCollapseExpand).toHaveBeenCalled();
        });

        it("WinLossByLeague.submit() should be called when '#searchForm' fired submit event", function () {
            spyOn(WinLossByLeague, "submit");

            $searchForm.submit();

            expect(WinLossByLeague.submit).toHaveBeenCalled();
        });

        describe("'#League' change event", function () {
            var request;
            beforeEach(function (done) {
                affix("#SubmitButton");
                affix("#Match");
                
                affix("#DateControl");                

                jasmine.Ajax.install();
                jasmine.Ajax
                    .stubRequest("WinLossByLeagueReport/GetMatchesByLeague")
                    .andReturn({
                        'status': 200,
                        'contentType': 'application/json',
                        'responseText': '[{Key: 1, Value: "test"}]'
                    });

                $league.change();
                request = jasmine.Ajax.requests.mostRecent();

                setTimeout(done, 10);
            });

            afterEach(function () {
                jasmine.Ajax.uninstall();
            });

            it("ajax request should be called with correct url", function () {
                expect(request.url).toBe("WinLossByLeagueReport/GetMatchesByLeague");
            });
        });

        describe("'#Sport' or '#CurrencyId' change event", function () {
            var request;
            beforeEach(function (done) {              
                jasmine.Ajax.install();
                jasmine.Ajax
                    .stubRequest("WinLossByLeagueReport/GetLeaguesBySport")
                    .andReturn({
                        'status': 200,
                        'contentType': 'application/json',
                        'responseText': '[{Key: 1, Value: "test"}]'
                    });

                $sport.change();
                request = jasmine.Ajax.requests.mostRecent();

                setTimeout(done, 10);
            });

            afterEach(function () {
                jasmine.Ajax.uninstall();
            });

            it("ajax request should be called with correct url", function () {
                expect(request.url).toBe("WinLossByLeagueReport/GetLeaguesBySport");
            });
        });

        it("checkboxgroup() function of jquery should be called", function () {
            expect($.fn.checkboxgroup).toHaveBeenCalled();
        });

        it("WinLossByLeague.drillDownBetList(leagueid, leaguename) should be called when '.drilldown' fired click event", function () {
            spyOn(WinLossByLeague, "drillDownBetList");

            $drilldown.click();

            expect(WinLossByLeague.drillDownBetList).toHaveBeenCalled();
        });
    });

    describe("initSetting()", function () {
        it("WinLossByLeagueOption.licenseeId should have correct value", function () {
            affix("input#drillDownLicenseeId[value=1]");

            WinLossByLeague.initSetting();

            expect(WinLossByLeagueOption.licenseeId).toBe(1);
        });
    });

    describe("initEvent()", function () {
        var $submitButton, $levelLink;
        beforeEach(function () {
            $submitButton = affix("#SubmitButton");

            affix("#level-link-container .level-link");
            $levelLink = $(".level-link");
            $levelLink.attr("data-level", 1);

            WinLossByLeague.initEvent();
        });

        it("WinLossByLeague.submit() should be called when '#SubmitButton' fired click event", function () {
            spyOn(WinLossByLeague, "submit");

            $submitButton.click();

            expect(WinLossByLeague.submit).toHaveBeenCalled();
        });

        it("WinLossByLeague.goBack(level) should be called with correct parameter when '.level-link' fired click event", function () {
            spyOn(WinLossByLeague, "goBack");

            $levelLink.click();

            expect(WinLossByLeague.goBack).toHaveBeenCalledWith(1);
        });
    });

    describe("setReportTitle(level, minOdds, fromdate, todate)", function () {
        it("Report title should be correct text", function () {
            var titleExpected = 'WinLossByLeague, 2016/02/01 → 2016/02/29 - Currency:  undefined - MinimumOdds : 1';

            affix("div.report-title #caption");
            var minOddStr = Site.getSingleResource("MinimumOdds") + " : 1";

            WinLossByLeague.setReportTitle(0, 1, "2016/02/01", "2016/02/29");            

            expect($("#caption").html()).toBe(titleExpected);
        });
    });

    describe("lastDateOfMonth(year, month)", function () {
        it("WinLossByLeague.lastDateOfMonth(year, month) should be return value correctly", function () {
            var dateExpected = new Date("Thu Mar 31 2016");

            var result = WinLossByLeague.lastDateOfMonth(2016, 2);

            expect(result).toEqual(dateExpected);
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
        var $reportGrid;
        beforeEach(function () {
            $reportGrid = affix("#ReportGrid");

            spyOn(jqGridAdditional, "clearGrid");
            spyOn(WinLossByLeagueOption, "getGridOption").and.returnValue({});
            spyOn($.fn, "jqGrid");
            spyOn(jqGridAdditional, "showRecommendedBrowser");
            spyOn(jqGridAdditional, "resetLayout");
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
        it("jqGridAdditional.showRecommendedBrowser(pagingContainer) should be called with correct parameter", function () {
            expect(jqGridAdditional.showRecommendedBrowser).toHaveBeenCalledWith("");
        });
        it("jqGridAdditional.resetLayout(reportGrid) should be called with correct parameter", function () {
            expect(jqGridAdditional.resetLayout).toHaveBeenCalledWith($reportGrid);
        });
        it("WinLossByLeague.resizeGrid() should be called", function () {
            expect(WinLossByLeague.resizeGrid).toHaveBeenCalled();
        });
    });
});