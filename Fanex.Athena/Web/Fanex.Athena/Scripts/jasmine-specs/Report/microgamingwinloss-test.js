describe("MicroGamingWinLoss", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(MicroGamingWinLoss, "initEvent");
            spyOn(Site, "setConstraintMonthRange");

            MicroGamingWinLoss.initialize();
        });

        it("MicroGamingWinLoss.initEvent() should be called", function () {
            expect(MicroGamingWinLoss.initEvent).toHaveBeenCalled();
        });
        it("Site.setConstraintMonthRange(monthRange, url) should be called with correct parameter", function () {
            expect(Site.setConstraintMonthRange).toHaveBeenCalledWith(3, "api/report/MicrogamingWinloss");
        });

        xit("MicroGamingWinLoss.resizeGrid() should be called when '.tree-wrap-ltr' fired click event", function () {
            var $reportContent = affix(".report-content");
            var $treeWrap = affix(".tree-wrap-ltr");
            $reportContent.append($treeWrap);

            spyOn(MicroGamingWinLoss, "resizeGrid");

            $treeWrap.click();

            expect(MicroGamingWinLoss.resizeGrid).toHaveBeenCalled();
        });
    });

    describe("initEvent()", function () {
        var $submitButton, $exportToExcel, $isExportExcel;

        beforeEach(function () {
            $submitButton = affix("#SubmitButton");
            $exportToExcel = affix("#ExportToExcel");
            $isExportExcel = affix("input#IsExportExcel");

            MicroGamingWinLoss.initEvent();
        });

        describe("'#SubmitButton' fired click event", function () {
            it("'#IsExportExcel' should has value equal false", function () {
                $submitButton.click();

                expect($isExportExcel.val()).toBe("false");
            });

            it("MicroGamingWinLoss.submit() should be called", function () {
                spyOn(MicroGamingWinLoss, "submit");

                $submitButton.click();

                expect(MicroGamingWinLoss.submit).toHaveBeenCalled();
            });
        });

        describe("'#ExportToExcel' fired click event", function () {
            it("'#IsExportExcel' should has value equal false", function () {
                $exportToExcel.click();

                expect($isExportExcel.val()).toBe("false");
            });

            it("submit function of jquery should be called", function () {
                spyOn($.fn, "submit");

                $exportToExcel.click();

                expect($.fn.submit).toHaveBeenCalled();
            });
        });
    });

    describe("setReportTitle(currencyId, fromdate, todate)", function () {
        it("Title of report should has correct text", function () {
            affix('input#ReportTitle[value="Microgaming Win Loss"]');

            var $reportTitle = affix("div.report-title");

            MicroGamingWinLoss.setReportTitle(1, "04/1/2016", "04/15/2016");

            expect($reportTitle.html()).toBe("Microgaming Win Loss, 04/1/2016 → 04/15/2016 - Viewin");
        });
    });

    describe("ajaxComplete(response)", function () {
        beforeEach(function () {
            spyOn(MicroGamingWinLoss, "setReportTitle");
            spyOn(MicroGamingWinLoss, "bindData");

            affix("input#CurrencyId[value=1]");
            affix("input#FromDate").val("04/1/2016");
            affix("input#ToDate").val("04/15/2016");

            MicroGamingWinLoss.ajaxComplete({});
        });

        it("MicroGamingWinLoss.setReportTitle(currencyId, fromdate, todate) should be called with correct parameters", function () {
            expect(MicroGamingWinLoss.setReportTitle).toHaveBeenCalledWith("1", "04/1/2016", "04/15/2016");
        });
        it("MicroGamingWinLoss.bindData(response) should be called with correct parameters", function () {
            expect(MicroGamingWinLoss.bindData).toHaveBeenCalledWith({});
        });
    });

    describe("submit()", function () {
        var request;

        beforeEach(function (done) {
            affix("input#FromDate").val("04/1/2016");
            affix("input#ToDate").val("04/15/2016");

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest('api/report/MicrogamingWinloss')
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ "Data": [] }'
                });

            spyOn($.fn, "serializeObject").and.returnValue({ FromDate: "01/1/2016" });
            spyOn(MicroGamingWinLoss, "ajaxComplete");

            MicroGamingWinLoss.submit();
            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("Request url should be 'api/report/MicrogamingWinloss'", function () {
            expect(request.url).toBe("api/report/MicrogamingWinloss");
        });
        it("MicroGamingWinLoss.ajaxComplete(response) should be called with correct parameters", function () {
            expect(MicroGamingWinLoss.ajaxComplete).toHaveBeenCalled();
        });
    });

    describe("bindTotal(totalData, summaryTotal)", function () {
        beforeEach(function () {
            spyOn(MicroGamingWinLossOption, "getTotalColModel").and.returnValue([{ Index: 0, Name: '#' }]);
            spyOn(Report, "bindSummaryTotal").and.returnValue(5);
            spyOn(jqGridAdditional, "assignTotal");
            spyOn(jqGridAdditional, "setGroupFooters");
            
            MicroGamingWinLoss.bindTotal({ Total: 100 }, { Summary: 200 });
        });

        it("jqGridAdditional.assignTotal(totalData, columnModel) should be called with correct parameters", function () {
            expect(jqGridAdditional.assignTotal).toHaveBeenCalledWith({ Total: 100 }, [{ Index: 0, Name: '#' }]);
        });
        it("jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount, allowScrolling) should be called with correct parameters", function () {
            expect(jqGridAdditional.setGroupFooters).toHaveBeenCalledWith(2, 3, 5);
        });
    });

    describe("bindData(response)", function () {
        beforeEach(function () {
            var response = { Data: [1, 2, 3], Total: [1, 2] };
            affix("#ReportGrid");
            spyOn(jqGridAdditional, "clearGrid");
            spyOn($.fn, "jqGrid");
            spyOn(jqGridAdditional, "resetLayout");

            MicroGamingWinLoss.bindData(response);
        });

        it("jqGridAdditional.clearGrid(reportId) should be called with correct parameter", function () {
            expect(jqGridAdditional.clearGrid).toHaveBeenCalledWith("ReportGrid");
        });

        it("jqGrid function of jquery should be called", function () {
            expect($.fn.jqGrid).toHaveBeenCalled();
        });

        it("jqGridAdditional.resetLayout(grid) should be called with correct parameter", function () {
            expect(jqGridAdditional.resetLayout).toHaveBeenCalledWith($("#ReportGrid"));
        });
    });
});