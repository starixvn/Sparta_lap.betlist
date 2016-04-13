describe("result", function () {
    describe("init()", function () {
        var $detailLink, $submitOutright, $submitNormal, $submitButton, $submitToDate,
            $submitYesterday, $exportToExcel, $sport, $league;
        beforeEach(function () {
            var $resultPage = affix("#result-page");
            $detailLink = affix(".detail-link");
            $resultPage.append($detailLink);

            $submitOutright = affix("#submit-outright");
            $submitNormal = affix("#submit-normal");
            $submitButton = affix("#SubmitButton");
            $submitToDate = affix("#submit-todate");

            $submitYesterday = affix("#submit-yesterdate");
            $exportToExcel = affix("#ExportToExcel");
            $sport = affix("input#sport");
            $league = affix("input#league");

            result.init();
        });

        it("dialog function of jquery should be called when '.detail-link' fired click event", function () {
            spyOn($.fn, "dialog");

            $detailLink.click();

            expect($.fn.dialog).toHaveBeenCalled();
        });

        describe("'#submit-outright' fired click event", function () {
            it("'.for-out-right' should be show when 'data-isoutright' attribute equal 1", function () {
                var $forOutright = affix("input.for-out-right");
                $submitOutright.attr("data-isoutright", 1);

                $submitOutright.click();

                expect($forOutright).not.toBeVisible();
            });

            it("'#SubmitButton' should be trigged", function () {
                var spyEvent = spyOnEvent('#SubmitButton', 'click');

                $submitOutright.click();

                expect(spyEvent).toHaveBeenTriggered();
            });
        });

        describe("'#submit-normal' fired click event", function () {
            it("'.for-out-right' should be show when 'data-isoutright' attribute equal 1", function () {
                var $forOutright = affix("input.for-out-right");

                $submitNormal.click();

                expect($forOutright).not.toBeVisible();
            });

            it("'#SubmitButton' should be trigged", function () {
                var spyEvent = spyOnEvent('#SubmitButton', 'click');

                $submitNormal.click();

                expect(spyEvent).toHaveBeenTriggered();
            });
        });

        describe("'#SubmitButton' fired click event", function () {
            var request, getReportDataRequest;

            beforeEach(function (done) {
                jasmine.Ajax.install();
                jasmine.Ajax
                    .stubRequest('Report/GetSports')
                    .andReturn({
                        'status': 200,
                        'contentType': 'application/json',
                        'responseText': '{ "Data": [], "Sports": [], "Leagues": [] }'
                    });

                $submitButton.click();
                request = jasmine.Ajax.requests.mostRecent();

                setTimeout(done, 10);
            });

            afterEach(function () {
                jasmine.Ajax.uninstall();
            });

            it("Request url should be 'Report/GetSports'", function () {
                expect(request.url).toBe("Report/GetSports");
            });

            describe("getReportData() is called", function () {
                var getReportDataRequest;

                beforeEach(function (done) {
                    jasmine.Ajax
                        .stubRequest('Report/Result')
                        .andReturn({
                            'status': 200,
                            'contentType': 'application/json',
                            'responseText': '{ "Data": [] }'
                        });
                    getReportDataRequest = jasmine.Ajax.requests.mostRecent();

                    setTimeout(done, 10);
                });


                it("Request url should be 'Report/Result'", function () {
                    expect(getReportDataRequest.url).toBe("Report/Result");
                });
                
            });
        });

        it("datepicker function of jquery should be call two times when '#submit-todate' fired click event", function () {
            spyOn($.fn, "datepicker");

            $submitToDate.click();

            expect($.fn.datepicker.calls.count()).toBe(2);
        });

        it("datepicker function of jquery should be call two times when '#submit-yesterdate' fired click event", function () {
            spyOn($.fn, "datepicker");

            $submitYesterday.click();

            expect($.fn.datepicker.calls.count()).toBe(2);
        });

        describe("'#sport' fired change event", function () {
            var request;

            beforeEach(function (done) {
                jasmine.Ajax.install();
                jasmine.Ajax
                    .stubRequest('Report/GetSports')
                    .andReturn({
                        'status': 200,
                        'contentType': 'application/json',
                        'responseText': '{ "Data": [], "Sports": [], "Leagues": [] }'
                    });

                $sport.change();
                request = jasmine.Ajax.requests.mostRecent();

                setTimeout(done, 10);
            });

            afterEach(function () {
                jasmine.Ajax.uninstall();
            });

            it("Request url should be 'Report/GetSports'", function () {
                expect(request.url).toBe("Report/GetSports");
            });
        });

        describe("'#league' fired change event", function () {
            var request;

            beforeEach(function (done) {
                jasmine.Ajax.install();
                jasmine.Ajax
                    .stubRequest('Report/Result')
                    .andReturn({
                        'status': 200,
                        'contentType': 'application/json',
                        'responseText': '{ "Data": [] }'
                    });

                $league.change();
                request = jasmine.Ajax.requests.mostRecent();

                setTimeout(done, 10);
            });

            afterEach(function () {
                jasmine.Ajax.uninstall();
            });

            it("Request url should be 'Report/Result'", function () {
                expect(request.url).toBe("Report/Result");
            });
        });
    });
});