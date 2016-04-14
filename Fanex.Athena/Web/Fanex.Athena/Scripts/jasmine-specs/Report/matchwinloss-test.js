describe("MatchWinLoss", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(MatchWinLoss, "initProductList");
            spyOn(MatchWinLoss, "initEvent");
            spyOn(MatchWinLoss, "resizeGrid");
            spyOn(Site, "addFilterCollapseExpand");
            spyOn(Site, "addUserGuide");

            MatchWinLoss.initialize();
        });

        it("MatchWinLoss.initProductList() should be called", function () {
            expect(MatchWinLoss.initProductList).toHaveBeenCalled();
        });
        it("MatchWinLoss.initEvent() should be called", function () {
            expect(MatchWinLoss.initEvent).toHaveBeenCalled();
        });
        it("MatchWinLoss.resizeGrid() should be called", function () {
            expect(MatchWinLoss.resizeGrid).toHaveBeenCalled();
        });
        it("Site.addFilterCollapseExpand() should be called", function () {
            expect(Site.addFilterCollapseExpand).toHaveBeenCalled();
        });
        it("Site.addUserGuide(option) should be called with correct parameter", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "MatchWL", title: Site.getSingleResource("MatchWinLoss") });
        });     
    });

    describe("initEvent()", function () {
        var $exportToExcel, $submitButton, $matchHandler, $matchLink, $downlineLink, $isExportExcel;
        
        beforeEach(function () {
            $exportToExcel = affix("#ExportToExcel");
            $submitButton = affix("#SubmitButton");
            $matchHandler = affix(".match-handler");
            $matchLink = affix(".match-link");
            $downlineLink = affix(".downline-link");

            $matchHandler.append($matchLink, $downlineLink);
            affix("#data-content").append($matchHandler);

            $isExportExcel = affix('input#IsExportExcel');

            spyOn(MatchWinLoss, "viewMatchWinlossDetail");

            MatchWinLoss.initEvent();
        });

        describe("'#ExportToExcel' fired click event", function () {
            it("'#IsExportExcel' should has value euqla true", function () {
                $exportToExcel.click();

                expect($isExportExcel.val()).toBe("true");
            });
            it("submit function of jquery should be called", function () {
                spyOn($.fn, "submit");

                $exportToExcel.click();

                expect($.fn.submit).toHaveBeenCalled();
            });
        });

        it("MatchWinLoss.submit() should be called when '#SubmitButton' fired click event", function () {
            spyOn(MatchWinLoss, "submit");

            $submitButton.click();

            expect(MatchWinLoss.submit).toHaveBeenCalled();
        });

        it("MatchWinLoss.viewMatchWinlossDetail(element) should be called with correct parameter when '.match-handler' fired click event", function () {            
            $matchHandler.click();

            expect(MatchWinLoss.viewMatchWinlossDetail).toHaveBeenCalledWith($matchHandler[0]);
        });

        it("MatchWinLoss.viewMatchWinlossBetList(element) should be called with correct parameter when '.match-link' fired click event", function () {
            spyOn(MatchWinLoss, "viewMatchWinlossBetList");

            $matchLink.click();

            expect(MatchWinLoss.viewMatchWinlossBetList).toHaveBeenCalledWith($matchLink[0]);
        });

        it("MatchWinLoss.viewMatchWinlossBetList(element) should be called with correct parameter when '.downline-link' fired click event", function () {
            spyOn(MatchWinLoss, "viewMatchWinlossBetList");

            $downlineLink.click();

            expect(MatchWinLoss.viewMatchWinlossBetList).toHaveBeenCalledWith($downlineLink[0]);
        });
    });

    describe("initProductList()", function () {
        var $productList;
        beforeEach(function () {
            $productList = affix("#ProductList");
            affix(".product-lable");
        });

        it("'.product-lable' should exist when '#ProductList' has class 'all-products'", function () {
            $productList.addClass("all-products");

            MatchWinLoss.initProductList();

            expect($(".product-lable").length).toBe(1);
        });

        it("'.product-lable' should be removed when '#ProductList' has not class 'all-products'", function () {
            $productList.removeClass("all-products");

            MatchWinLoss.initProductList();

            expect($(".product-lable").length).toBe(0);
        });
    });

    describe("submit()", function () {
        it("Site.blockUI() should be called", function () {
            spyOn(Site, "blockUI");

            MatchWinLoss.submit();

            expect(Site.blockUI).toHaveBeenCalled();
        });

        it("'#IsExportExcel' should has value equal false", function () {
            var $isExportExcel = affix("#IsExportExcel");

            MatchWinLoss.submit();

            expect($isExportExcel.val()).toBe(false);
        });

        it("submit function of jquery should be called", function () {
            spyOn($.fn, "submit");

            MatchWinLoss.submit();

            expect($.fn.submit).toHaveBeenCalled();
        });
    });

    describe("getProductOption()", function () {
        it("MatchWinLoss.getProductOption() should be returned with correct value", function () {
            var $productList = affix("#ProductList");
            var $productSB = affix("input.childCheckbox[type=checkbox]");
            $productList.append($productSB);
            $productSB.attr("data-original", "True");
            $productSB.attr("data-product-code", "SB");

            var expected = [{ Code: "SB", Status: true }];

            var result = MatchWinLoss.getProductOption("SB");

            expect(result).toEqual(expected);
        });
    });

    describe("viewMatchWinlossDetail(element)", function () {
        var request, $parentRow, $firstColumn;

        beforeEach(function (done) {
            var $element = affix("#element");
            $parentRow = affix("#parentRow");
            $firstColumn = $("<td>test</td>");
            $parentRow.append($firstColumn);

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest('MatchWinLossDetail')
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '<tr class="expand"></tr><tr class="expand"></tr>'
                });

            spyOn($.fn, "closest").and.returnValue($parentRow);
            spyOn($.fn, "data").and.returnValue({ LeagueId: 1, MatchId: 2, ProductCode: "SB" });
            spyOn(MatchWinLoss, "getProductOption").and.returnValue([{ Code: "SB", Status: true }]);

            MatchWinLoss.viewMatchWinlossDetail($element);
            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("Request url should be 'MatchWinLossDetail'", function () {
            expect(request.url).toBe("MatchWinLossDetail");
        });

        it("'#parentRow' should has class named 'main'", function () {
            expect($parentRow).toHaveClass("main");
        });

        it("First column of '#parentRow' should has rowspan value equal 3", function () {
            expect($firstColumn.attr("rowspan")).toBe("3");
        });

        it("First column of '#parentRow' should has class named 'current-expanding-column'", function () {
            expect($firstColumn).toHaveClass("current-expanding-column");
        });
    });
});