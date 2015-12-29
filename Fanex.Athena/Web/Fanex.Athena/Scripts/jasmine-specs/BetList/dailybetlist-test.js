describe("DailyBetList", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(DailyBetList, "initEvent");
            spyOn(Site, "addUserGuide");

            DailyBetList.initialize();
        });

        it("DailyBetList.initEvent() should be called", function () {
            expect(DailyBetList.initEvent).toHaveBeenCalled();
        });

        it("Site.addUserGuide() should be called with correct parameters", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "DailyBetList", title: "DailyBetList" });
        });
    });

    describe("initEvent", function () {
        it("DailyBetList.submit() should be called when fire click event on #SubmitButton", function () {
            affix("button#SubmitButton");
            spyOn(DailyBetList, 'submit');
            DailyBetList.initEvent();

            $("#SubmitButton").click();
            expect(DailyBetList.submit).toHaveBeenCalled();
        });

        it("DailyBetList.exportToExcel() should be called when fire click event on #ExportToExcel", function () {
            affix("button#ExportToExcel");
            spyOn(DailyBetList, 'exportToExcel');
            DailyBetList.initEvent();

            $("#ExportToExcel").click();
            expect(DailyBetList.exportToExcel).toHaveBeenCalled();
        });

        it("$('.export-excel-area') should be showed when fire change event on #CurrencyId with value is 1", function () {
            affix(".export-excel-area");
            affix("input#CurrencyId[value=1]");

            DailyBetList.initEvent();

            $("#CurrencyId").change();
            expect($(".export-excel-area")).toBeVisible();
        });

        it("$('.export-excel-area') should be hid when fire change event on #CurrencyId with value is 0", function () {
            affix(".export-excel-area");
            affix("input#CurrencyId[value=0]");

            DailyBetList.initEvent();

            $("#CurrencyId").change();
            expect($(".export-excel-area")).not.toBeVisible();
        });
    });
});