describe("LiveReportMatchList", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(LiveReportMatchList, "initEvent");
            spyOn(LiveReportMatchList, "resizeContent");

            LiveReportMatchList.initialize();
        });

        it("LiveReportMatchList.initEvent() should be called", function () {
            expect(LiveReportMatchList.initEvent).toHaveBeenCalled();
        });
        it("LiveReportMatchList.resizeContent() should be called", function () {
            expect(LiveReportMatchList.resizeContent).toHaveBeenCalled();
        });
    });

    describe("initEvent()", function () {
        var $matchDetailLink;

        beforeEach(function () {
            $matchDetailLink = affix(".match-detail-link");

            LiveReportMatchList.initEvent();
        });

        it("LiveReportMatchList.viewMatchBetList(target) should be called with correct parameter", function () {
            spyOn(LiveReportMatchList, "viewMatchBetList");

            $matchDetailLink.click();

            expect(LiveReportMatchList.viewMatchBetList).toHaveBeenCalledWith($matchDetailLink[0]);
        });
    });

    describe("resizeContent()", function () {
        it("'.content-main' should has width same with '.data-content'", function () {
            var $contentMain = affix(".content-main");
            var $dataContent = affix(".data-content");
            $dataContent.width(500);

            LiveReportMatchList.resizeContent();

            expect($contentMain.width()).toBe(500);
        });
    });
});