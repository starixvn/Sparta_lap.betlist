describe("CorrectScoreBetList", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(CorrectScoreBetList, 'initEvent');
            spyOn(CorrectScoreBetList, 'submit');
            spyOn(Site, 'addUserGuide');
            CorrectScoreBetList.initialize();
        });

        it("CorrectScoreBetList.initEvent() should be called", function () {
            expect(CorrectScoreBetList.initEvent).toHaveBeenCalled();
        });

        it("CorrectScoreBetList.submit()  should be called", function () {
            expect(CorrectScoreBetList.submit).toHaveBeenCalled();
        });

        it("Site.addUserGuide() should be called with correct parameters", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: 'TotalBetsCorrectScore', title: 'TotalBets - CorrectScore' });
        });
    });

    describe("initEvent()", function () {
        it("CorrectScoreBetList.submit() should be called once #SubmitButton is clicked", function () {
            affix('button#SubmitButton');
            spyOn(CorrectScoreBetList, 'submit');

            CorrectScoreBetList.initEvent();
            $('#SubmitButton').click();

            expect(CorrectScoreBetList.submit).toHaveBeenCalled();
        });
    });
});