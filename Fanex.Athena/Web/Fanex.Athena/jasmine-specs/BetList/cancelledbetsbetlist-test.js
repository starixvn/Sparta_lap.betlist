describe("CancelledBetsBetList", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(CancelledBetsBetList, 'initEvent');
            spyOn(CancelledBetsBetList, 'submit');
            spyOn(Site, 'addUserGuide');
            CancelledBetsBetList.initialize();
        });

        it("CancelledBetsBetList.initEvent() should be called", function () {
            expect(CancelledBetsBetList.initEvent).toHaveBeenCalled();
        });

        it("CancelledBetsBetList.submit()  should be called", function () {
            expect(CancelledBetsBetList.submit).toHaveBeenCalled();
        });

        it("Site.addUserGuide() should be called with correct parameters", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: 'CancelledBets', title: 'CancelledBets' });
        });
    });

    describe("initEvent()", function () {
        it("CancelledBetsBetList.submit() should be called once #CustId fire 'change' event", function () {
            affix('input#CustId');
            spyOn(CancelledBetsBetList, 'submit');

            CancelledBetsBetList.initEvent();
            $('#CustId').change();

            expect(CancelledBetsBetList.submit).toHaveBeenCalled();
        });

        it("#IsExportExcel should have value 'false' once #SubmitButton is clicked", function () {
            affix('button#SubmitButton');
            affix('input#IsExportExcel');
            
            CancelledBetsBetList.initEvent();
            $('#SubmitButton').click();
            expect($('#IsExportExcel').val()).toBe('false');
        });

        it("CancelledBetsBetList.exportToExcel() should be called once #ExportToExcel is clicked", function () {
            affix('#ExportToExcel');
            spyOn(CancelledBetsBetList, 'exportToExcel');

            CancelledBetsBetList.initEvent();
            $('#ExportToExcel').click();

            expect(CancelledBetsBetList.exportToExcel).toHaveBeenCalled();
        });
    });
});