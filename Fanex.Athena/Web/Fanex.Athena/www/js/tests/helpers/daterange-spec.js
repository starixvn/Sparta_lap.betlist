define(["jquery", "common/site", "common/popup", "helpers/daterange"], function ($, Site, Popup, DateRangeHelper) {
    describe("Helper - DateRange", function () {
        describe("showMessageBox(message)", function () {
            it("Popup.openPopup function should be called with correct parameter", function () {
                setFixtures('<input id="PageLanguage" value="en-US" />');
                window["_language_en_US"] = { "LrfNotification": "LrfNotification" };
                Site.initialize();

                spyOn(Popup, "openPopup");
                var message = "Test";
                var expectedParam1 = '<div title="LrfNotification"><p class="notification">' + message + '!</p></div>';
                var expectedParam2 = { width: 350, height: 120, title: "LrfNotification", className: "popup-notification" };

                DateRangeHelper.showMessageBox(message);

                expect(Popup.openPopup).toHaveBeenCalledWith(expectedParam1, expectedParam2);
            });
        });

        describe("setConstraintMonthRange(monthRange, url)", function () {
            beforeEach(function () {
                $("form").off();
                spyOn(Popup, "openPopup");
                setFixtures('<input id="PageLanguage" value="en-US" /> <input id="FromDate" value="4/1/2017" /> <input id="ToDate" value="7/1/2017" />' +
                    '<form id="form"></form>');
                window["_language_en_US"] = { "DateRangeExceedMsg": "Exceed {0}" };
                Site.initialize();
            });

            afterEach(function () {
                $("form").off();
            })

            it("Popup.openPopup function should be called with correct parameter when exceed range", function () {
                DateRangeHelper.setConstraintMonthRange(2, "about:blank");

                $("form").submit();

                var expectedParam1 = '<div title="LrfNotification"><p class="notification">Exceed 2!</p></div>';
                var expectedParam2 = { width: 350, height: 120, title: "LrfNotification", className: "popup-notification" };

                expect(Popup.openPopup).toHaveBeenCalledWith(expectedParam1, expectedParam2);
            });

            xit("Popup.openPopup function should not be called when range is valid", function () {
                DateRangeHelper.setConstraintMonthRange(4, "about:blank");

                $("form").submit();

                expect(Popup.openPopup).not.toHaveBeenCalled();
            });
        });
    });
});