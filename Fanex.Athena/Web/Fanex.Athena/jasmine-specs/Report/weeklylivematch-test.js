describe("WeeklyLiveMatch", function () {
    describe("initialize()", function () {
        it("WeeklyLiveMatch.initEvent() should be called", function () {
            spyOn(WeeklyLiveMatch, "initEvent");

            WeeklyLiveMatch.initialize();

            expect(WeeklyLiveMatch.initEvent).toHaveBeenCalled();
        });

        it("Site.addUserGuide(option) should be called with correct parameter", function () {
            spyOn(Site, "addUserGuide");

            WeeklyLiveMatch.initialize();

            var option = {
                resourceName: "WeeklyLiveMatches",
                title: Site.getSingleResource("WeeklyLiveMatches"),
                height: 460
            };

            expect(Site.addUserGuide).toHaveBeenCalledWith(option);
        });
    });

    describe("initEvent()", function () {
        it("Site.setConstraintMonthRange(monthRange, url) should be called with correct parameter", function () {
            spyOn(Site, "setConstraintMonthRange");

            WeeklyLiveMatch.initEvent();

            expect(Site.setConstraintMonthRange).toHaveBeenCalledWith(2, "api/WeeklyLiveMatchReport/index");
        });

        describe("'#SubmitButton' fired click event", function () {
            var $isExportExcel;
            var $submitButton;
            beforeEach(function () {
                $isExportExcel = affix('input#IsExportExcel');
                $submitButton = affix('#SubmitButton');

                spyOn(WeeklyLiveMatch, "submit");

                WeeklyLiveMatch.initEvent();
                $submitButton.click();
            });

            it("'#IsExportExcel' should has value equal false", function () {                
                expect($isExportExcel.val()).toBe("false");
            });

            it("WeeklyLiveMatch.submit() should be called", function () {
                expect(WeeklyLiveMatch.submit).toHaveBeenCalled();
            });
        });

        it("Site.exportExcel() should be called with correct parameter when '#ExportToExcel' fired click event", function () {
            var $exportExcel = affix('#ExportToExcel');
            spyOn($.fn, "serializeObject").and.returnValue({Id: 1, Name: "WeeklyLiveMatch"});
            spyOn(Site, "exportExcel");

            WeeklyLiveMatch.initEvent();
            $exportExcel.click();

            var data = {
                Id: 1, Name: "WeeklyLiveMatch", IsExportExcel: true, SortColumn: null, SortDirection: null
            }

            expect(Site.exportExcel).toHaveBeenCalledWith(data, "api/WeeklyLiveMatchReport/index");
        });
    });

    describe("submit()", function () {
        var request;

        beforeEach(function (done) {
            WeeklyLiveMatchOption.urlData = "urlTest";
            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest(WeeklyLiveMatchOption.urlData)
                .andReturn({
                    'status': 200,
                    'contentType': 'application/x-www-form-urlencoded; charset=UTF-8',
                    'responseText': '{ "Data": [{Id: 1, Name: "test"}] }'
                });

            spyOn($.fn, "dialog");
            spyOn($.fn, "serializeObject").and.returnValue({ Id: 1, Name: "WeeklyLiveMatch" });
            spyOn(WeeklyLiveMatch, "bindData");

            WeeklyLiveMatch.submit();
            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("Request url should be 'urlTest'", function () {
            expect(request.url).toBe("urlTest");
        });
    });

    describe("bindData(fromDate, toDate, response)", function () {
        beforeEach(function () {
            spyOn(jqGridAdditional, "clearGrid");
            spyOn(WeeklyLiveMatchOption, "getGridOption").and.returnValue({ colModel: [] });
            spyOn($.fn, "jqGrid");
            spyOn(jqGridAdditional, "resetLayout");

            var response = {
                Data: [{ Id: 1, Name: "Test1" }, { Id: 2, Name: "Test2" }],
                TotalData: [78, 911],
                NumberOfWeeks: 2,
                WeekHeaders: ["26 Jun - 26 Jun 2016", "27 Jun - 28 Jun 2016"]
            }
            WeeklyLiveMatch.bindData("26 Jun 2016", "28 Jun 2016", response);
        });

        it("jqGridAdditional.clearGrid(reportId) should be called with correct parameter", function () {
            expect(jqGridAdditional.clearGrid).toHaveBeenCalledWith("ReportGrid");
        });

        it("jqGrid function of jquery should be called", function () {
            expect($.fn.jqGrid).toHaveBeenCalled();
        });

        it("jqGridAdditional.resetLayout(grid) should be called", function () {
            expect(jqGridAdditional.resetLayout).toHaveBeenCalled();
        });
    });
});