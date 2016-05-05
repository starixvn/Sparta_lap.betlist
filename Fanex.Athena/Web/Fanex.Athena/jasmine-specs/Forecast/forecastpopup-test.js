describe("ForecastPopup", function () {
    describe("showPopup(width, height, url)", function () {
        it("dialog function of jquery should be called", function () {
            spyOn($.fn, "dialog");

            ForecastPopup.showPopup(300, 100, "localhost-test");

            expect($.fn.dialog).toHaveBeenCalled();
        })
    });
});