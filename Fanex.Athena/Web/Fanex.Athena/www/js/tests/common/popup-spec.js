define(['jquery', 'common/popup', 'lib/jquery.flatpopup'], function ($, PopUp) {
    describe("PopUp", function () {
        describe("openPopup(element, option)", function () {
            xit("flatpopup jquery plugin should be called with correct parameters", function () {
                spyOn($.fn, "flatPopup");
                var $element = setFixtures('<div id="popup"></div>');
                var option = { width: 200, height: 300, title: "title" };

                PopUp.openPopup($element, option);

                expect($.fn.flatPopup).toHaveBeenCalled();
            });
        });
    });
});