describe("CustomerWinLoss", function () {
    
    describe("initialize()", function () {
        it("Expected functions should be call when initialize() is called", function () {
            spyOn(ProductListHelper, 'initProductList');
            spyOn(CustomerWinLoss, 'initEvent');
            spyOn(CustomerWinLoss, 'initSetting');
            spyOn(CustomerWinLoss, 'initMonthPicker');
            spyOn(Site, 'addFilterCollapseExpand');
            spyOn(Site, 'addUserGuide');

            CustomerWinLoss.initialize();

            expect(CustomerWinLoss.initEvent).toHaveBeenCalled();
            expect(CustomerWinLoss.initMonthPicker).toHaveBeenCalled();
            expect(Site.addFilterCollapseExpand).toHaveBeenCalled();
            expect(Site.addUserGuide).toHaveBeenCalled();
        });
    });

    describe("initMonthPicker()", function () {
        it("$.fn.monPickr() should be called on a correct selector.", function () {
            $.fn.monPickr = function () { };
            affix('input#IsHistory[value="True"]');
            affix('#DateControl');
            spyOn($.fn, 'monPickr');

            CustomerWinLoss.initMonthPicker();

            expect($.fn.monPickr.calls.mostRecent().object.selector).toBe('#DateControl');
        });
    });

    describe("initEvent()", function () {
        it("CustomerWinLoss.submit() should be called if #SubmitButton is clicked.", function () {
            var $button = affix('#SubmitButton');
            var $mainform = affix('#CustomerWinlossForm');

            spyOn(Report, "ValidProducts").and.returnValues(true);
            spyOn(CustomerWinLoss, 'submit');

            CustomerWinLoss.initEvent();
            $button.click();

            expect(CustomerWinLoss.submit).toHaveBeenCalled();
        });

        it("Site.exportExcel should be called if #ExportToExcel is clicked.", function () {
            var $button = affix('#ExportToExcel');

            spyOn(Report, "ValidProducts").and.returnValues(true);
            spyOn(Site, 'exportExcel');

            CustomerWinLoss.initEvent();
            $button.click();

            expect(Site.exportExcel).toHaveBeenCalled();
        });

        it("viewHistoryReport() should be called if #history-report-link is clicked.", function () {
            var $link = affix('#history-report-link');
            spyOn(CustomerWinLoss, 'viewHistoryReport');

            CustomerWinLoss.initEvent();
            $link.click();

            expect(CustomerWinLoss.viewHistoryReport).toHaveBeenCalled();
        });

        it("viewCurrentReport() should be called if #current-report-link is clicked.", function () {
            var $link = affix('#current-report-link');
            spyOn(CustomerWinLoss, 'viewCurrentReport');

            CustomerWinLoss.initEvent();
            $link.click();

            expect(CustomerWinLoss.viewCurrentReport).toHaveBeenCalled();
        });

        it("viewWinlossBetList() should be called with correct param when .bet-list-link is clicked.", function () {
            var $container = affix('.report-content');
            var $link = $container.affix('.bet-list-link');
            $link.attr('data-cust-id', 1);
            $link.attr('data-system-id', 2);
            $link.attr('data-external-id', 3);
            spyOn(CustomerWinLoss, 'viewWinlossBetList');

            CustomerWinLoss.initEvent();
            $link.click();

            expect(CustomerWinLoss.viewWinlossBetList).toHaveBeenCalledWith('1', '2', '3');
        });
    });
});
