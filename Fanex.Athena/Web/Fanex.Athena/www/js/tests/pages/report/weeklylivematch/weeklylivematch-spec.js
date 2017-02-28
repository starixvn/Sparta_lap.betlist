define(["jquery", "common/site", "pages/report/weeklylivematch/weeklylivematch.option", "lib/jqgrid/jquery.jqgrid.additional", "components/pluginhelper/daterangepicker",
        "pages/report/weeklylivematch/weeklylivematch", "lib/jqgrid/jquery.jqGrid.en", "lib/jqgrid/jquery.jqGrid.min", "lib/linq", "extensions/jquery.extension"],
        function ($, Site, WeeklyLiveMatchOption, jqGridAdditional, DatePicker, WeeklyLiveMatch) {
            describe("WeeklyLiveMatch", function () {
                beforeAll(function () {
                    spyOn(Site, "getSingleResource").and.callFake(function (text) {
                        return text;
                    });
                });

                describe("initialize()", function () {
                    beforeEach(function () {
                        spyOn(Site, "initialize");
                        spyOn(WeeklyLiveMatch, "initEvent");
                        spyOn(Site, "addUserGuide");
                        spyOn(DatePicker, "initDateRangePicker");

                        WeeklyLiveMatch.initialize();
                    });

                    it("Site.initialize() should be called", function () {
                        expect(Site.initialize).toHaveBeenCalled();
                    });
                    it("WeeklyLiveMatch.initEvent() should be called", function () {
                        expect(WeeklyLiveMatch.initEvent).toHaveBeenCalled();
                    });
                    it("Site.addUserGuide(option) should be called with correct parameter", function () {
                        expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "WeeklyLiveMatches", title: "WeeklyLiveMatches", height: 380 });
                    });
                    it("DatePicker.initDateRangePicker(firstDayOfWeekIsMonday) should be called with correct parameter", function () {
                        expect(DatePicker.initDateRangePicker).toHaveBeenCalledWith(true);
                    });
                });
            });
        });