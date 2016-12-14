define(['jquery', 'common/site', 'common/querystring', 'components/refreshcountdown', 'pages/totalbet/totalbetsbase'], function ($, Site, QueryString, RefreshCountDown, TotalBet) {
    describe("TotalBet", function () {
        describe("resizeGrid()", function () {
            beforeEach(function () {
                setFixtures('<div class="body-content" style="width: 300px; height: 500px; top: 20px; position:relative;"></div>');
                setFixtures('<div class="footer-content" style="width: 300px; height: 100px;"></div>');
            });

            it(".body-content should have correct height vaue", function () {
                spyOn(window, "innerHeight").and.returnValue(300);

                TotalBet.resizeGrid();
                var bodyHeight = $(".body-content").css("max-height");

                expect(bodyHeight).toBe("190px");
            });
        })
    });
});