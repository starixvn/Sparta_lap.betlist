describe("RacingCommission", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(RacingCommission, "initEvent");
            spyOn(Site, "createSortingIcon");
            spyOn(Site, "addUserGuide");

            RacingCommission.initialize();
        });

        it("RacingCommission.initEvent() should be called", function () {
            expect(RacingCommission.initEvent).toHaveBeenCalled();
        });
        it("Site.createSortingIcon(sortHandler) should be called", function () {
            expect(Site.createSortingIcon).toHaveBeenCalled();
        });
        it("Site.addUserGuide(option) should be called with correct parameter", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "RacingCommission", title: Site.getSingleResource("RacingCommission") });
        });
    });

    describe("initEvent()", function () {
        var $exportToExcel, $submitButton, $sortReport, $betListLink, $isExportExcel, $sortColumn, $sortDirection;
        beforeEach(function () {
            $exportToExcel = affix("#ExportToExcel");
            $submitButton = affix("#SubmitButton");
            $sortReport = affix("#sortReport");
            affix("#data-content .bet-list-link");
            $betListLink = $(".bet-list-link");
            $betListLink.attr("data-cust-id", 1);
            $betListLink.attr("data-system-id", 2);
            $betListLink.attr("data-external-id", 3);

            $isExportExcel = affix("input#IsExportExcel");
            $sortColumn = affix("input#SortColumn");
            $sortDirection = affix("input#SortDirection");

            RacingCommission.initEvent();
        });

        describe("'#ExportToExcel' fired click event", function () {
            it("'#IsExportExcel' should has value equal true", function () {
                $exportToExcel.click();

                expect($isExportExcel.val()).toBe("true");
            });
            it("submit function of jquery should be called", function () {
                spyOn($.fn, "submit");

                $exportToExcel.click();

                expect($.fn.submit).toHaveBeenCalled();
            });
        });

        describe("'#SubmitButton' fired click event", function () {
            it("'#SortColumn' should has value equal ExternalID", function () {
                $submitButton.click();

                expect($sortColumn.val()).toBe("ExternalID");
            });
            it("'#SortDirection' should has value equal ASC", function () {
                $submitButton.click();

                expect($sortDirection.val()).toBe("ASC");
            });
            it("RacingCommission.submitForm() should be called", function () {
                spyOn(RacingCommission, "submitForm");

                $submitButton.click();

                expect(RacingCommission.submitForm).toHaveBeenCalled();
            });
        });

        it("RacingCommission.sortReport() should be called when '#sortReport' fired click event", function () {
            spyOn(RacingCommission, "sortReport");

            $sortReport.click();

            expect(RacingCommission.sortReport).toHaveBeenCalled();
        });

        it("RacingCommission.viewRacingBetList(custId, systemId, externalId) should be called with correct parameters", function () {
            spyOn(RacingCommission, "viewRacingBetList");

            $betListLink.click();

            expect(RacingCommission.viewRacingBetList).toHaveBeenCalledWith("1", "2", "3");
        });
    });

    describe("submitForm()", function () {
        var $isExportExcel;
        beforeEach(function () {
            $isExportExcel = affix("input#IsExportExcel");
            spyOn(Site, "blockUI");
            spyOn($.fn, "submit");

            RacingCommission.submitForm();
        });

        it("'#IsExportExcel' should has value equal false", function () {
            expect($isExportExcel.val()).toBe("false");
        });
        it("Site.blockUI() should be called", function () {
            expect(Site.blockUI).toHaveBeenCalled();
        });
        it("submit function of jquery should be called", function () {
            expect($.fn.submit).toHaveBeenCalled();
        });
    });

    describe("sortReport()", function () {
        var $isExportExcel;
        beforeEach(function () {
            $isExportExcel = affix("input#IsExportExcel");
            spyOn(Site, "blockUI");
            spyOn($.fn, "submit");

            RacingCommission.sortReport();
        });

        it("'#IsExportExcel' should has value equal false", function () {
            expect($isExportExcel.val()).toBe("false");
        });
        it("Site.blockUI() should be called", function () {
            expect(Site.blockUI).toHaveBeenCalled();
        });
        it("submit function of jquery should be called", function () {
            expect($.fn.submit).toHaveBeenCalled();
        });
    });
});