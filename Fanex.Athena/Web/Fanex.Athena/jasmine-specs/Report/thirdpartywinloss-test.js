describe("ThirdPartyWinLoss", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            affix("input#drillDownLicenseeId[value=1]");
            spyOn(ThirdPartyWinLoss, "initEvent");

            affix("input#Year[value=2016]");
            affix("input#Month[value=4]");
            affix("input#FromDay[value=1]");
            affix("input#ToDay[value=25]");

            spyOn($.fn, "monPickr");

            ThirdPartyWinLoss.initialize();
        });

        it("ThirdPartyWinLossOption.licenseeId should has correct value", function () {
            expect(ThirdPartyWinLossOption.licenseeId).toBe(1);
        });

        it("ThirdPartyWinLoss.initEvent() should be called", function () {
            expect(ThirdPartyWinLoss.initEvent).toHaveBeenCalled();
        });

        it("monPickr function of jquery should be called", function () {
            expect($.fn.monPickr).toHaveBeenCalled();
        });
    });

    describe("initEvent()", function () {
        var $submitButton, $exportToExcel, $levelLink, $isExportExcel;
        beforeEach(function () {
            $submitButton = affix("#SubmitButton");
            $exportToExcel = affix("#ExportToExcel");

            affix("#level-link-container .level-link");
            $levelLink = $(".level-link");

            $isExportExcel = affix("input#IsExportExcel");

            ThirdPartyWinLoss.initEvent();
        });

        describe("'#SubmitButton' fired click event", function () {
            it("'#IsExportExcel' should has value equal false", function () {
                spyOn(ThirdPartyWinLoss, 'submit');
                $submitButton.click();
                
                expect($isExportExcel.val()).toBe("false");
            });

            it("ThirdPartyWinLoss.submit() should be called", function () {
                spyOn(ThirdPartyWinLoss, "submit");

                $submitButton.click();

                expect(ThirdPartyWinLoss.submit).toHaveBeenCalled();
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

        describe("'.level-link' fired click event", function () {
            it("ThirdPartyWinLoss.goBack(level) should be called with correct parameter", function () {
                $levelLink.attr("data-level", 2);
                spyOn(ThirdPartyWinLoss, "goBack");

                $levelLink.click();

                expect(ThirdPartyWinLoss.goBack).toHaveBeenCalledWith(2);
            });
        });
    });

    describe("setExcelUrl(level, currencyId, custId, fdate, tdate)", function () {
        it("ThirdPartyWinLossOption.urlExcelData should be 'api/ThirdPartyWinLossReport/Index' when level equal 0", function () {
            ThirdPartyWinLoss.setExcelUrl(0);

            expect(ThirdPartyWinLossOption.urlExcelData).toBe("api/ThirdPartyWinLossReport/Index");
        });

        it("ThirdPartyWinLossOption.urlExcelData should be correct when level greater than 0", function () {
            var url = 'api/ThirdPartyWinLossReport/Index';
            var queryString = "?level=1&currencyId=1&custId=2&fdate=04/1/2016&tdate=04/12/2016";
            var expected = url + queryString;

            ThirdPartyWinLoss.setExcelUrl(1, 1, 2, "04/1/2016", "04/12/2016");

            expect(ThirdPartyWinLossOption.urlExcelData).toBe(expected);
        });
    });

    describe("renderBreadCrumb(level)", function () {
        var $breadcumHolder;
        beforeEach(function () {
            $breadcumHolder = affix("input.breadCrumbHolder");
        });

        it("'.breadCrumbHolder' should be hidden when level equal 0", function () {
            ThirdPartyWinLoss.renderBreadCrumb(0);

            expect($breadcumHolder).not.toBeVisible();
        });
        it("'.breadCrumbHolder' should be shown when level greater than 0", function () {
            ThirdPartyWinLoss.renderBreadCrumb(1);

            expect($breadcumHolder).toBeVisible();
        });

        it("'.level2' should be shown when level equal 2", function () {
            var $level2 = affix(".level2");
            ThirdPartyWinLoss.renderBreadCrumb(2);

            expect($level2).toBeVisible();
        });
    });

    describe("setReportTitle(currencyId, fromdate, todate)", function () {
        it("'div.report-title' should has correct test", function () {
            var $reportTitle = affix("div.report-title");
            affix('input#ReportTitle[value="Third Party Win loss"]');
            var viewin = Site.getSingleResource("Viewin").replace("{0}", "EUR");
            var expected = "Third Party Win loss, 04/1/2016 → 04/12/2016 - " + viewin;

            ThirdPartyWinLoss.setReportTitle(-1, "04/1/2016", "04/12/2016");
            expect($reportTitle.html()).toBe(expected);
        });
    });

    describe("ajaxComplete(level, response)", function () {
        beforeEach(function () {
            spyOn(ThirdPartyWinLoss, "setReportTitle");
            spyOn(ThirdPartyWinLoss, "renderBreadCrumb");
            spyOn(ThirdPartyWinLoss, "bindData");
        });

        it("ThirdPartyWinLoss.setReportTitle(currencyId, fromdate, todate) should be called with correct parameter", function () {
            var $currencyId = affix("input#CurrencyId[value=1]");
            var $fromDate = affix("input#FromDate");
            $fromDate.val("04/1/2016");
            var $toDate = affix("input#ToDate");
            $toDate.val("04/12/2016");

            ThirdPartyWinLoss.ajaxComplete(0, {});

            expect(ThirdPartyWinLoss.setReportTitle).toHaveBeenCalledWith("1", '04/1/2016', '04/12/2016');
        });

        it("ThirdPartyWinLoss.renderBreadCrumb(level) should be called with correct parameter", function () {
            ThirdPartyWinLoss.ajaxComplete(1, {});

            expect(ThirdPartyWinLoss.renderBreadCrumb).toHaveBeenCalledWith(1);
        });

        it("ThirdPartyWinLoss.bindData(level, response) should be called with correct parameter", function () {
            ThirdPartyWinLoss.ajaxComplete(1, { Data: [1, 2, 3] });

            expect(ThirdPartyWinLoss.bindData).toHaveBeenCalledWith(1, { Data: [1, 2, 3] });
        });
    });

    describe("submit()", function () {
        var request;

        beforeEach(function (done) {
            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest('api/ThirdPartyWinLossReport/Index')
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ "Data": [] }'
                });

            spyOn(ThirdPartyWinLoss, "setExcelUrl");
            spyOn(ThirdPartyWinLoss, "ajaxComplete");

            ThirdPartyWinLoss.submit();
            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("Request url should be 'api/ThirdPartyWinLossReport/Index'", function () {
            expect(request.url).toBe("api/ThirdPartyWinLossReport/Index");
        });
        it("ThirdPartyWinLoss.setExcelUrl(level, currencyId, custId, fdate, tdate) should be called with correct parameters", function () {
            expect(ThirdPartyWinLoss.setExcelUrl).toHaveBeenCalledWith(0, 0, 0, "", "");
        });
        it("ThirdPartyWinLoss.ajaxComplete(level, response) should be called with correct parameter", function () {
            expect(ThirdPartyWinLoss.ajaxComplete).toHaveBeenCalledWith(0, { Data: [] });
        });
    });

    describe("drillDown(level, currencyId, customerId, datetime)", function () {
        var request;

        beforeEach(function (done) {
            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest('api/ThirdPartyWinLossReport/Index?level=1&currencyId=1&custId=2&fdate=04/15/2016&tdate=04/15/2016')
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ "Data": [1, 2, 3] }'
                });
            spyOn(ThirdPartyWinLoss, "ajaxComplete");
            spyOn(ThirdPartyWinLoss, "setReportTitle");

            ThirdPartyWinLoss.drillDown(1, 1, 2, '04/15/2016');
            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("Request url should be correct", function () {
            expect(request.url).toBe("api/ThirdPartyWinLossReport/Index?level=1&currencyId=1&custId=2&fdate=04/15/2016&tdate=04/15/2016");
        });
        it("ThirdPartyWinLoss.ajaxComplete(level, response) should be called with correct parameter", function () {
            expect(ThirdPartyWinLoss.ajaxComplete).toHaveBeenCalledWith(1, { Data: [1, 2, 3] });
        });
        it("ThirdPartyWinLoss.setReportTitle(currencyId, fromdate, todate) should be called with correct parameter", function () {
            expect(ThirdPartyWinLoss.setReportTitle).toHaveBeenCalledWith("1", new Date(2016, 3, 15), new Date(2016, 3, 15));
        });
    });

    describe("goBack(level)", function () {
        beforeEach(function () {
            spyOn(ThirdPartyWinLoss, "renderBreadCrumb");
            spyOn(ThirdPartyWinLoss, "bindData");
            spyOn(ThirdPartyWinLoss, "setExcelUrl");
            spyOn(ThirdPartyWinLoss, "setReportTitle");
        });

        describe("level 0", function () {
            beforeEach(function () {
                ThirdPartyWinLoss.goBack(0);
            });

            it("ThirdPartyWinLoss.renderBreadCrumb(level) should be called with correct parameter", function () {
                expect(ThirdPartyWinLoss.renderBreadCrumb).toHaveBeenCalledWith(0);
            });
            it("ThirdPartyWinLoss.bindData(level, response) should be called with correct parameter", function () {
                expect(ThirdPartyWinLoss.bindData).toHaveBeenCalledWith(0, {});
            });
            it("ThirdPartyWinLoss.setExcelUrl(level, currencyId, custId, fdate, tdate) should be called with correct parameter", function () {
                expect(ThirdPartyWinLoss.setExcelUrl).toHaveBeenCalledWith(0, 0, 0, "", "");
            });
            it("ThirdPartyWinLoss.setReportTitle(currencyId, fromdate, todate) should be called with correct parameter", function () {
                var fromDate = Date.parse("04/1/2016");
                var toDate = Date.parse("04/12/2016");
                expect(ThirdPartyWinLoss.setReportTitle).toHaveBeenCalledWith("1", fromDate, toDate);
            });
        });

        describe("level 1", function () {
            beforeEach(function () {
                ThirdPartyWinLoss.goBack(1);
            });

            it("ThirdPartyWinLoss.renderBreadCrumb(level) should be called with correct parameter", function () {
                expect(ThirdPartyWinLoss.renderBreadCrumb).toHaveBeenCalledWith(1);
            });
            it("ThirdPartyWinLoss.bindData(level, response) should be called with correct parameter", function () {
                expect(ThirdPartyWinLoss.bindData).toHaveBeenCalledWith(1, { Data: [1, 2, 3] });
            });
        });
    });

    describe("bindTotal(level, totalData, summaryTotal)", function () {
        beforeEach(function () {
            spyOn(ThirdPartyWinLossOption, "getTotalColModel").and.returnValue([{ Name: "Column0", Index: 0 }]);
            spyOn(Report, "bindSummaryTotal").and.returnValue(300);
            spyOn(jqGridAdditional, "assignTotal");
            spyOn(jqGridAdditional, "setGroupFooters");           

            ThirdPartyWinLoss.bindTotal(1, { Total: 100 }, { Summary: 200 });
        });

        it("jqGridAdditional.assignTotal(totalData, columnModel) should be called with correct parameters", function () {
            expect(jqGridAdditional.assignTotal).toHaveBeenCalledWith({ Total: 100 }, [{ Name: "Column0", Index: 0 }]);
        });
        it("jqGridAdditional.setGroupFooters(colspan, summaryColspan, summaryRowCount, allowScrolling) should be called with correct parameters", function () {
            expect(jqGridAdditional.setGroupFooters).toHaveBeenCalledWith(2, 3, 300);
        });
    });

    describe("bindData(level, response)", function () {
        beforeEach(function () {
            affix("#ReportGrid");
            spyOn(jqGridAdditional, "clearGrid");
            spyOn($.fn, "jqGrid");
            spyOn(jqGridAdditional, "resetLayout");

            ThirdPartyWinLoss.bindData(1, { Data: [1, 2, 3], Total: [1, 2] });
        });

        it("jqGridAdditional.clearGrid(reportId) should be called with correct parameter", function () {
            expect(jqGridAdditional.clearGrid).toHaveBeenCalledWith("ReportGrid");
        });

        it("jqGrid function of jquery should be called two times", function () {
            expect($.fn.jqGrid.calls.count()).toBe(2);
        });

        it("jqGridAdditional.resetLayout(grid) should be called with correct parameter", function () {
            expect(jqGridAdditional.resetLayout).toHaveBeenCalledWith($("#ReportGrid"));
        });
    });
});