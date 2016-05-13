describe("LiveReport", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(LiveReport, "initEvent");
            spyOn(LiveReport, "initMonthPicker");
            spyOn(Site, "addUserGuide");

            LiveReport.initialize();
        });

        it("LiveReport.initEvent() should be called", function () {
            expect(LiveReport.initEvent).toHaveBeenCalled();
        });

        it("LiveReport.initMonthPicker() should be called", function () {
            expect(LiveReport.initMonthPicker).toHaveBeenCalled();
        });

        it("Site.addUserGuide(option) should be called with correct parameter", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "LiveReport", title: Site.getSingleResource("LiveReport") });
        });
    });

    describe("initEvent()", function () {
        var $productDetailLink;
        beforeEach(function () {
            $productDetailLink = affix(".product-detail-link");

            LiveReport.initEvent();
        });

        it("LiveReport.viewProductDetail(target) should be called with correct parameter when '.product-detail-link' fired click event", function () {
            spyOn(LiveReport, "viewProductDetail");

            $productDetailLink.click();

            expect(LiveReport.viewProductDetail).toHaveBeenCalledWith($productDetailLink[0]);
        });
    });

    describe("initMonthPicker()", function () {
        it("monPickr function of jquery should be called", function () {
            spyOn($.fn, "monPickr");

            LiveReport.initMonthPicker();

            expect($.fn.monPickr).toHaveBeenCalled();
        });
    });

    describe("viewProductDetail(target)", function () {
        var request, $parentRow, $firstColumn, $productCode, $detailContent, $element;

        function initDOMElement() {
            $element = affix("#element");
            $element.attr("data-product-code", "SB");
            affix("#FromDate");
            $("#FromDate").attr('data-original', "03/01/2016");
            affix("#ToDate");
            $("#ToDate").attr('data-original', "03/20/2016");
            affix("#BaseCurrency");
            $("#BaseCurrency").attr('data-original', "EUR");
            affix("#LiveIndicator");
            $("#LiveIndicator").attr('data-original', "1");

            $productCode = affix("input#ProductCode");
            $detailContent = affix("#detail-content");
        }

        beforeEach(function (done) {
            initDOMElement();

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest('LiveReport/Detail')
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': 'success'
                });

            spyOn(LiveReport, "ajaxStart");
            spyOn(LiveReport, "ajaxComplete");
            
            LiveReport.viewProductDetail($element);
            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("Request url should be 'LiveReport/Detail'", function () {
            expect(request.url).toBe("LiveReport/Detail");
        });

        it("LiveReport.ajaxStart() should be called", function () {
            expect(LiveReport.ajaxStart).toHaveBeenCalled();
        });

        it("LiveReport.ajaxComplete() should be called", function () {
            expect(LiveReport.ajaxComplete).toHaveBeenCalled();
        });

        it("'#ProductCode' should has value equal SB", function () {
            expect($productCode.val()).toBe("SB");
        });

        it("'#detail-content' should has html value equal success", function () {
            expect($detailContent.html()).toBe("success");
        });
    });

    describe("setSumParams(url, totalValue)", function () {
        it("LiveReport.setSumParams(url, totalValue) should return correct value", function () {
            var totalValue = {
                BetCount: 100,
                CustomerWinLoss: 200,
                CustomerComm: 300,
                LicenseeWinLoss: 400,
                LicenseeComm: 500,
                TurnOver: 600,
                AmountDue: 700
            };

            var expected = "localhost-test?totalrows=100&sumwinlost=200&sumplayercomm=300&sumlicenseewl=400&sumcomm=500&sumturnover=600&sumamtdueto3rd=700";

            var result = LiveReport.setSumParams("localhost-test", totalValue);

            expect(result).toBe(expected);
        });
    });

    describe("getMatchListProductOption(sportId, productCode)", function () {
        describe("sportId greater than 0", function () {
            it("Result should return with ['VS'] when sportType equal 180 or 186", function () {
                var expected = ["VS"];

                var result = LiveReport.getMatchListProductOption(180, "ALL");
                expect(result).toEqual(expected);

                result = LiveReport.getMatchListProductOption(180, "ALL");
                expect(result).toEqual(expected);
            });

            it("Result should return with ['LC'] when sportType equal 162", function () {
                var expected = ["LC"];

                var result = LiveReport.getMatchListProductOption(162, "ALL");
                expect(result).toEqual(expected);
            });

            it("Result should return with ['SB', 'BA'] when sportType not in 180,186,162", function () {
                var expected = ["SB", "BA"];

                var result = LiveReport.getMatchListProductOption(100, "ALL");
                expect(result).toEqual(expected);
            });
        });

        describe("sportId less than or equal 0", function () {
            it("Result should return with ['SB', 'BA', 'NG', 'RB', 'LC', 'VS', 'CS', 'BG'] when productCode equal ALL", function () {
                var expected = ['SB', 'BA', 'NG', 'RB', 'LC', 'VS', 'CS', 'BG'];

                var result = LiveReport.getMatchListProductOption(0, "ALL");
                expect(result).toEqual(expected);
            });

            it("Result should return with ['RB'] when productCode not equal ALL", function () {
                var expected = ['RB'];

                var result = LiveReport.getMatchListProductOption(0, "RB");
                expect(result).toEqual(expected);
            });
        });
    });

    describe("ajaxStart()", function () {
        it("Site.blockUI() should be called", function () {
            spyOn(Site, "blockUI");

            LiveReport.ajaxStart();

            expect(Site.blockUI).toHaveBeenCalled();
        });
    });

    describe("ajaxComplete()", function () {
        it("Site.unBlockUI() should be called", function () {
            spyOn(Site, "unBlockUI");

            LiveReport.ajaxComplete();

            expect(Site.unBlockUI).toHaveBeenCalled();
        });
    });
});