describe("PlaytechCasino", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(PlaytechCasino, "initEvent");
            spyOn(PlaytechCasino, "resizeGrid");
            spyOn(Site, "addUserGuide");

            PlaytechCasino.initialize();
        });

        it("PlaytechCasino.initEvent() should be called", function () {
            expect(PlaytechCasino.initEvent).toHaveBeenCalled();
        });
        it("PlaytechCasino.resizeGrid() should be called", function () {
            expect(PlaytechCasino.resizeGrid).toHaveBeenCalled();
        });
        it("Site.addUserGuide(option) should be called with correct parameter", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "PlaytechCasinoWL", title: Site.getSingleResource("PlaytechCasino") });
        });
    });

    describe("initEvent()", function () {
        var $exportToExcel, $drillDownLink, $levelLink, $submitButton;
        beforeEach(function () {
            $exportToExcel = affix("#ExportToExcel");
            $drillDownLink = affix(".drill-down-link");
            $levelLink = affix(".level-link");
            $submitButton = affix("#SubmitButton");

            PlaytechCasino.initEvent();
        });

        it("submit function of jquery should be called when '#ExportToExcel' fired click event", function () {
            spyOn($.fn, "submit");

            $exportToExcel.click();

            expect($.fn.submit).toHaveBeenCalled();
        });

        it("PlaytechCasino.drillDown(context) should be called with correct parameter when '.drill-down-link' fired click event", function () {
            spyOn(PlaytechCasino, "drillDown");

            $drillDownLink.click();

            expect(PlaytechCasino.drillDown).toHaveBeenCalledWith($drillDownLink[0]);
        });

        it("PlaytechCasino.goBack() should be called with correct parameter when '.level-link' fired click event", function () {
            spyOn(PlaytechCasino, "goBack");

            $levelLink.click();

            expect(PlaytechCasino.goBack).toHaveBeenCalledWith($levelLink[0]);
        });

        it("PlaytechCasino.submit() should be called when '#SubmitButton' fired click event", function () {
            spyOn(PlaytechCasino, "submit");

            $submitButton.click();

            expect(PlaytechCasino.submit).toHaveBeenCalled();
        });
    });

    describe("submit()", function () {
        var $level, $isExportExcel;

        beforeEach(function () {
            $level = affix("input#Level");
            $isExportExcel = affix("input#IsExportExcel");
            spyOn(Site, "blockUI");
            spyOn($.fn, "submit");

            PlaytechCasino.submit();
        });
        it("Site.blockUI() should be called", function () {
            expect(Site.blockUI).toHaveBeenCalled();
        });

        it("'#Level' should has value equal 0", function () {
            expect($level.val()).toBe("0");
        });
        it("'#IsExportExcel' should has value equal false", function () {
            expect($isExportExcel.val()).toBe("false");
        });

        it("submit function of jquery should be called", function () {
            expect($.fn.submit).toHaveBeenCalled();
        });
    });

    describe("drillDown(context)", function () {
        var $context, $level, $isExportExcel, $currency, $custId, $fromDate, $toDate;
        beforeEach(function () {
            $context = $("<div data-level=0 data-currency=1 data-date=04/12/2016 data-custid=3></div>");
            $level = affix("input#Level");
            $isExportExcel = affix("input#IsExportExcel");
            $currency = affix("input#DrillDownCurrency");
            $custId = affix("input#CustId");
            $fromDate = affix("input#DrillDownFromDate");
            $toDate = affix("input#DrillDownToDate");

            spyOn($.fn, "submit");

            PlaytechCasino.drillDown($context);
        });

        it("DOM elements should be assigned with correct value", function () {
            expect($level.val()).toBe("0");
            expect($isExportExcel.val()).toBe("false");
            expect($currency.val()).toBe("1");
            expect($custId.val()).toBe("3");
            expect($fromDate.val()).toBe("04/12/2016");
            expect($toDate.val()).toBe("04/12/2016");
        });

        it("submit function of jquery should be called", function () {
            expect($.fn.submit).toHaveBeenCalled();
        });
    });

    describe("goBack()", function () {
        it("PlaytechCasino.submit() should be called", function () {
            spyOn(PlaytechCasino, "submit");

            PlaytechCasino.goBack();

            expect(PlaytechCasino.submit).toHaveBeenCalled();
        });
    });
});