describe("CommonBetList", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            affix('.content-filter');
            CommonBetList.initialize();
        });

        it("'.content-filter' should be removed", function () {
            expect($('.content-filter')).not.toExist();
        });
    });
});