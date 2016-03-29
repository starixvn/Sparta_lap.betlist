describe("WinLossByProduct", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(WinLossByProduct, "initEvent");
            spyOn(WinLossByProduct, "initSetting");
            spyOn($.fn, "autocomplete");
            spyOn(Site, "setConstraintMonthRange");
            spyOn(Site, "addFilterCollapseExpand");
            spyOn(Site, "addUserGuide");

            WinLossByProduct.initialize();
        });

        it("WinLossByProduct.initEvent() should be called", function () {
            expect(WinLossByProduct.initEvent).toHaveBeenCalled();
        });
        it("WinLossByProduct.initSetting() should be called", function () {
            expect(WinLossByProduct.initSetting).toHaveBeenCalled();
        });
        
        it("Site.setConstraintMonthRange(monthRange, url) should be called with correct parameter", function () {
            expect(Site.setConstraintMonthRange).toHaveBeenCalledWith(3, "api/report/WinLostByProduct");
        });
        it("Site.addFilterCollapseExpand() should be called", function () {
            expect(Site.addFilterCollapseExpand).toHaveBeenCalled();
        });
        it("Site.addUserGuide(options) should be called with correct parameter", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "WinLossByProduct", title: Site.getSingleResource("WinLossByProduct") });
        });
    });

    describe("initSetting()", function () {
        it("Properties of WinLossByProductOption should be correct", function () {
            affix("input#IsAllowedCustomerCommission[value=true]");
            affix("input#IsAllowedBACommission[value=false]");
            affix("input#IsAllowedRacing[value=False]");
            affix("input#drillDownLicenseeId[value=34]");

            WinLossByProduct.initSetting();

            expect(WinLossByProductOption.hasCustomerComm).toBe(true);
            expect(WinLossByProductOption.IsAllowBA).toBe(false);
            expect(WinLossByProductOption.IsAllowRB).toBe(false);
            expect(WinLossByProductOption.licenseeId).toBe(34);
        });
    });

    describe("initEvent()", function () {
        var $betlistLink, $submitButton, $levelLink;
        beforeEach(function () {
            $betlistLink = affix(".bet-list-link");
            $betlistLink.attr("data-drill-down", 2);

            $submitButton = affix("#SubmitButton");
            affix("input#IsExportExcel");

            affix("#level-link-container .level-link");
            $levelLink = $(".level-link");
            $levelLink.attr("data-level", 3);

            WinLossByProduct.initEvent();
        });

        it("WinLossByProduct.openBetlistPopUp(dataDrillDown) should be called with correct parameter when '.bet-list-link' fired click event", function () {
            spyOn(WinLossByProduct, "openBetlistPopUp");

            $betlistLink.click();

            expect(WinLossByProduct.openBetlistPopUp).toHaveBeenCalledWith("2");
        });

        it("WinLossByProduct.submit() should be called when '#SubmitButton' fired click event", function () {
            spyOn(WinLossByProduct, "submit");

            $submitButton.click();

            expect(WinLossByProduct.submit).toHaveBeenCalled();
        });

        it("'#IsExportExcel' value should be false", function () {
            spyOn(WinLossByProduct, "submit");

            $submitButton.click();

            expect($("#IsExportExcel").val()).toBe("false");
        });

        it("WinLossByProduct.goBack(level) should be called with correct parameter when '.level-link' fired click event", function () {
            spyOn(WinLossByProduct, "goBack");

            $levelLink.click();

            expect(WinLossByProduct.goBack).toHaveBeenCalledWith("3");
        });
    });

    describe("setExcelUrl(level, currencyId, custId, fdate, tdate, currencyName)", function () {
    });
});