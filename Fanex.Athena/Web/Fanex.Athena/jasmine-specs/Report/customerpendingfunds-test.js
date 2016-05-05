describe("CustPendingFunds", function () {
    
    describe("initialize()", function () {
        it("initEvent() should be call when initialize() is called", function () {
            spyOn(CustPendingFunds, 'initEvent');

            CustPendingFunds.initialize();

            expect(CustPendingFunds.initEvent).toHaveBeenCalled();
        });

        it("Site.addUserGuide() should be call when initialize() is called with correct params.", function () {
            spyOn(Site, 'addUserGuide');

            CustPendingFunds.initialize();

            expect(Site.addUserGuide).toHaveBeenCalledWith({
                resourceName: "CustomerPendingFunds",
                title: Site.getSingleResource("CustomerPedingFunds"),
                height: jasmine.any(Number)
            });
        });
    });

    describe("initEvent()", function () {
        beforeEach(function () {
            $("body").off("click", "**");
        });

        it("viewPendingFundsBetList() should be called with correct params if .bet-list-link is clicked.", function () {
            var $content = affix('#data-content');
            var $link = $content.affix('.bet-list-link');
            $link.attr('data-cust-id', 1);
            $link.attr('data-system-id', 2);
            $link.attr('data-external-id', 3);

            spyOn(CustPendingFunds, 'viewPendingFundsBetList');

            CustPendingFunds.initEvent();
            $link.click();

            expect(CustPendingFunds.viewPendingFundsBetList).toHaveBeenCalledWith('1', '2', '3');
        });

        it("$.fb.submit() should be called on #CustomerPendingFundsForm if #ExportToExcel is clicked.", function () {
            var $excel = affix('input#ExportToExcel');
            affix('#CustomerPendingFundsForm');

            spyOn($.fn, 'submit');

            CustPendingFunds.initEvent();
            $excel.click();

            expect($.fn.submit.calls.mostRecent().object.selector).toBe('#CustomerPendingFundsForm');
        });

        it("Inputs should be assigned correct values if #Submit is clicked.", function () {
            var $sortColumn = affix('input#SortColumn');
            var $sortDirection = affix('input#SortDirection');
            var $excel = affix('input#IsExportExcel');
            var $submit = affix('#SubmitButton');

            spyOn($.fn, 'submit');

            CustPendingFunds.initEvent();
            $submit.click();

            expect($sortColumn.val()).toBe('ExternalId');
            expect($sortDirection.val()).toBe('ASC');
            expect($excel.val()).toBe('false');
        });
    });
});
