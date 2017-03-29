define(['jquery', 'common/popup'], function ($, PopUp) {
    describe("PopUp", function () {
        describe("openPopup(element, option)", function () {
            it("flatpopup jquery plugin should be called with correct parameters", function () {
                spyOn($.fn, "flatPopup");

                var $element = setFixtures('<div id="popup"></div>');
                var option = {
                    width: 200, height: 300, title: "title"
                };

                PopUp.openPopup($element, option);

                expect($.fn.flatPopup).toHaveBeenCalledWith(option);
            });
        });

        describe("openPopupWithIframe(url, option)", function () {
            it("Bet List popup should be called with correct url", function () {
                spyOn($.fn, "flatPopup");

                var url = "/Test/Index?transid=1234&custId=5678";
                var option = { width: 500, height: 600, title: "Bet List", id: "BetListPopUp" };

                PopUp.openPopupWithIframe(url, option);

                expect($.fn.flatPopup).toHaveBeenCalledWith(option);
            });
        });

        describe("updateCurrentPopup(url, option)", function () {
            it("flatPopup should be called with correct parameter", function () {
                parent.$ = $;
                spyOn($.fn, "flatPopup");
                setFixtures('<div id="current" class="popup-frame"><div id="popupFrame"></div></div>');
                var $frame = parent.$("#popupFrame");
                parent.$frame = $frame;

                var url = "localhost/Test";
                var option = { id: "current", title: "title" };

                PopUp.updateCurrentPopup(url, option);

                expect($.fn.flatPopup).toHaveBeenCalledWith(option);
            });

            it("#popupFrame should has correct src attribute", function () {
                parent.$ = $;
                spyOn($.fn, "flatPopup");
                setFixtures('<div id="current" class="popup-frame"><div id="popupFrame"></div></div>');
                var $frame = parent.$("#popupFrame");
                parent.$frame = $frame;

                var url = "localhost/Test";
                var option = { id: "current", title: "title" };

                PopUp.updateCurrentPopup(url, option);

                expect($frame.attr("src")).toBe("localhost/Test");
            });

            it(".txt-title should has correct text", function () {
                parent.$ = $;
                spyOn($.fn, "flatPopup");
                setFixtures('<div id="current" class="popup-frame"><div class="txt-title"></div>"<div id="popupFrame"></div></div>');
                var $frame = parent.$("#popupFrame");
                parent.$frame = $frame;

                var url = "localhost/Test";
                var option = { id: "current", title: "title" };

                PopUp.updateCurrentPopup(url, option);
                var result = $(".txt-title").html();

                expect(result).toBe(option.title);
            });
        });
    });
});