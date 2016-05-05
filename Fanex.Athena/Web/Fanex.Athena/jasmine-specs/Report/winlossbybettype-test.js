describe("WinLossByBetType", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(WinLossByBetType, "initEvent");
            spyOn(Site, "createSortingIcon");
            spyOn(WinLossByBetType, "resizeGrid");
            spyOn(Site, "addUserGuide");

            WinLossByBetType.initialize();
        });

        it("WinLossByBetType.initEvent() should be called", function () {
            expect(WinLossByBetType.initEvent).toHaveBeenCalled();
        });

        it("Site.createSortingIcon(sortHandler) should be called", function () {
            expect(Site.createSortingIcon).toHaveBeenCalled();
        });

        it("WinLossByBetType.resizeGrid() should be called", function () {
            expect(WinLossByBetType.resizeGrid).toHaveBeenCalled();
        });

        it("Site.addUserGuide(options) should be called with correct parameter", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "WinLossByBetType", title: Site.getSingleResource("WinLossByBetType") });
        });
    });

    describe("initEvent()", function () {
        var $exportToExcel, $submitButton, $betTypeLink;

        beforeEach(function () {
            $exportToExcel = affix("input#ExportToExcel");
            $submitButton = affix("#SubmitButton");

            affix("#data-content .bettype-link");
            $betTypeLink = $(".bettype-link");

            spyOn($.fn, "submit");

            WinLossByBetType.initEvent();
        });

        it("Value of '#IsExportExcel' should be true when '#ExportToExcel' fired click event", function () {
            var $isExportExcel = affix("input#IsExportExcel");

            $exportToExcel.click();

            expect($isExportExcel.val()).toBe("true");
        });

        it("submit function of jquery should be called '#ExportToExcel' fired click event", function () {
            $exportToExcel.click();

            expect($.fn.submit).toHaveBeenCalled();
        });

        it("Site.blockUI() should be called when '#SubmitButton' fired click event", function () {
            spyOn(Site, "blockUI");

            $submitButton.click();

            expect(Site.blockUI).toHaveBeenCalled();
        });

        it("Value of '#IsExportExcel' should be false when '#SubmitButton' fired click event", function () {
            var $isExportExcel = affix("input#IsExportExcel");

            $submitButton.click();

            expect($isExportExcel.val()).toBe("false");
        });

        it("submit function of jquery should be called '#SubmitButton' fired click event", function () {
            $submitButton.click();

            expect($.fn.submit).toHaveBeenCalled();
        });

        it("WinLossByBetType.viewBetList(bettype) should be called with correct parameter when '.bettype-link' fired click event", function () {
            $betTypeLink.attr("data-bettype", 1);
            spyOn(WinLossByBetType, "viewBetList");

            $betTypeLink.click();

            expect(WinLossByBetType.viewBetList).toHaveBeenCalledWith(1);
        });
    });

    describe("sortReport()", function () {
        it("Site.blockUI() should be called", function () {
            spyOn(Site, "blockUI");

            WinLossByBetType.sortReport();

            expect(Site.blockUI).toHaveBeenCalled();
        });

        it("Value of '#IsExportExcel' should be false", function () {
            var $isExportExcel = affix("input#IsExportExcel");
            WinLossByBetType.sortReport();

            expect($isExportExcel.val()).toBe("false");
        });

        it("submit function of jquery should be called", function () {
            spyOn($.fn, "submit");

            WinLossByBetType.sortReport();

            expect($.fn.submit).toHaveBeenCalled();
        });
    });  
});