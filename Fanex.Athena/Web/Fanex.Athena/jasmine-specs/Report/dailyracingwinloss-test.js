describe("DailyRacingWinLoss", function () {
    
    describe("initialize()", function () {
        it("Expected functions should be call when initialize() is called", function () {
            spyOn(DailyRacingWinLoss, 'initEvent');
            spyOn(DailyRacingWinLoss, 'resizeGrid');
            spyOn(Site, 'addUserGuide');

            DailyRacingWinLoss.initialize();

            expect(DailyRacingWinLoss.initEvent).toHaveBeenCalled();
            expect(DailyRacingWinLoss.resizeGrid).toHaveBeenCalled();
            expect(Site.addUserGuide).toHaveBeenCalledWith({
                resourceName: "DailyRacingWL",
                title: Site.getSingleResource("DailyRacingWinLoss")
            });
        });
    });

    describe("initEvent()", function () {
        it("submitForm() should be called if #SubmitButton is clicked.", function () {
            var $button = affix('#SubmitButton');
            spyOn(DailyRacingWinLoss, 'submitForm');

            DailyRacingWinLoss.initEvent();
            $button.click();

            expect(DailyRacingWinLoss.submitForm).toHaveBeenCalled();
        });

        it("#IsExportExcel should have value 'true' if #ExportToExcel is clicked.", function () {
            var $button = affix('#ExportToExcel');
            var $input = affix('input#IsExportExcel');

            DailyRacingWinLoss.initEvent();
            $button.click();

            expect($input.val()).toBe('true');
        });

        it("viewRacingWinlossBetList() should be called with correct param when .bet-list-link is clicked.", function () {
            var $container = affix('#data-content');
            var $link = $container.affix('.bet-list-link');
            $link.attr('data-league', 1);
            $link.attr('data-race-no', 2);
            $link.attr('data-currency', 3);
            spyOn(DailyRacingWinLoss, 'viewRacingWinlossBetList');

            DailyRacingWinLoss.initEvent();
            $link.click();

            expect(DailyRacingWinLoss.viewRacingWinlossBetList).toHaveBeenCalledWith('1', '2', '3');
        });
    });

    describe("submitForm()", function () {
        it("#ReportForm.submit() should be called when calling submitForm().", function () {
            var $mainform = affix('#ReportForm');
            var spyEvent = spyOnEvent($mainform, 'submit');

            DailyRacingWinLoss.submitForm();

            expect(spyEvent).toHaveBeenTriggered();
        });
    });
});
