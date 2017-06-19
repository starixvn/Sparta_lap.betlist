describe("CustomerWinLoss", function () {
    
    describe("initialize()", function () {
        it("Expected functions should be call when initialize() is called", function () {
            spyOn(ProductListHelper, 'initProductList');
            spyOn(CustomerWinLoss, 'initEvent');
            spyOn(CustomerWinLoss, 'initMonthPicker');
            spyOn(Site, 'createSortingIcon');
            spyOn(Site, 'addFilterCollapseExpand');
            spyOn(Site, 'addUserGuide');

            CustomerWinLoss.initialize();

            expect(CustomerWinLoss.initEvent).toHaveBeenCalled();
            expect(CustomerWinLoss.initMonthPicker).toHaveBeenCalled();
            expect(Site.createSortingIcon).toHaveBeenCalled();
            expect(Site.addFilterCollapseExpand).toHaveBeenCalled();
            expect(Site.addUserGuide).toHaveBeenCalled();
        });
    });

    describe("initMonthPicker()", function () {
        it("$.fn.monPickr() should be called on a correct selector.", function () {
            $.fn.monPickr = function () { };
            affix('input#IsHistoryReport[value="True"]');
            affix('#DateControl');
            spyOn($.fn, 'monPickr');

            CustomerWinLoss.initMonthPicker();

            expect($.fn.monPickr.calls.mostRecent().object.selector).toBe('#DateControl');
        });
    });

    describe("initEvent()", function () {
        it("#CustomerWinlossForm.submit() should be called if #SubmitHistoryButton is clicked.", function () {
            var $button = affix('#SubmitHistoryButton');
            var $mainform = affix('#CustomerWinlossForm');

            var spyEvent = spyOnEvent($mainform, 'submit');

            CustomerWinLoss.initEvent();
            $button.click();

            expect(spyEvent).toHaveBeenTriggered();
        });

        it("#CustomerWinlossForm.submit() should be called if #SubmitButton is clicked.", function () {
            var $button = affix('#SubmitButton');
            var $mainform = affix('#CustomerWinlossForm');

            var spyEvent = spyOnEvent($mainform, 'submit');

            CustomerWinLoss.initEvent();
            $button.click();

            expect(spyEvent).toHaveBeenTriggered();
        });

        it("#CustomerWinlossForm.submit() should be called if #ExportToExcel is clicked.", function () {
            var $button = affix('#ExportToExcel');
            var $mainform = affix('#CustomerWinlossForm');

            var spyEvent = spyOnEvent($mainform, 'submit');

            CustomerWinLoss.initEvent();
            $button.click();

            expect(spyEvent).toHaveBeenTriggered();
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
            var $container = affix('#data-content');
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
