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

            it("Popup was existed - append function of jquery should be called", function () {
                setFixtures('<div id="ExistedBetListPopUp"></div>');
                spyOn($.fn, "flatPopup");
                spyOn($.fn, "append");

                var url = "about:blank";
                var option = { width: 500, height: 600, title: "Bet List", id: "ExistedBetListPopUp" };

                PopUp.openPopupWithIframe(url, option);
                var expectedParam = '<iframe id="popupFrame" src="about:blank" ></iframe>';

                expect($.fn.append).toHaveBeenCalledWith(expectedParam);
            });
        });

        describe("updateCurrentPopup(url, option)", function () {
            beforeEach(function () {
                parent.$ = $;
                spyOn($.fn, "flatPopup");
                setFixtures('<div id="current" class="popup-frame"><div id="popupFrame"></div></div>');
            });

            it("flatPopup should be called with correct parameter", function () {
                var url = "localhost/Test";
                var option = { id: "current", title: "title" };

                PopUp.updateCurrentPopup(url, option);

                expect($.fn.flatPopup).toHaveBeenCalledWith(option);
            });

            it("#popupFrame should has correct src attribute", function () {
                var url = "localhost/Test";
                var option = { id: "current", title: "title" };

                PopUp.updateCurrentPopup(url, option);
                var result = $("#popupFrame").attr("src");

                expect(result).toBe("localhost/Test");
            });

            it(".txt-title should has correct text", function () {
                var url = "localhost/Test";
                var option = { id: "current", title: "title" };
                $("#current").prepend('<div class="txt-title"></div>');

                PopUp.updateCurrentPopup(url, option);
                var result = $(".txt-title").html();

                expect(result).toBe(option.title);
            });

            it("id in option is not match - Get popup current id", function () {
                var option = { id: "newPopup" };

                PopUp.updateCurrentPopup(null, option);
                var result = $(".popup-frame").attr("id");

                expect(result).toBe("current");
            });

            it("id in option is undefine - flatPopup should be called with id of option is popup current id", function () {
                var option = {};

                PopUp.updateCurrentPopup(null, option);
                var result = $(".popup-frame").attr("id");

                expect(option.id).toBe("current");
                expect($.fn.flatPopup).toHaveBeenCalledWith(option);
            });

            it("flatPopup function is not called when popup element is not exist", function () {
                $("#current").remove();
                var option = {};

                PopUp.updateCurrentPopup(null, option);

                expect($.fn.flatPopup).not.toHaveBeenCalled();
            });

            it(".txt-title should keep current text when title in option is empty", function () {
                var url = "localhost/Test";
                var option = { id: "current", title: "" };
                $("#current").prepend('<div class="txt-title">Current Title</div>');

                PopUp.updateCurrentPopup(url, option);
                var result = $(".txt-title").html();

                expect(result).toBe("Current Title");
            });
        });
    });
});