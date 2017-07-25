var basePath = "pages/customer/betsetting/";
var modulePath = ["jquery", "common/site", basePath + "common/betsettingbase", basePath + "products/betradarvirtualsports"];

define(modulePath, function ($, Site, BetSettingBase, BetradarVsBetSetting) {
    describe("BetradarVsBetSetting", function () {
        describe("initialize", function () {
            beforeAll(function () {
                spyOn(Site, "initialize");
                spyOn(BetSettingBase, "initialize");
            });

            beforeEach(function () {
                BetradarVsBetSetting.initialize();
            });

            it("Site.initialize() should be called", function () {
                expect(Site.initialize).toHaveBeenCalled();
            });
            it("BetSettingBase.initialize() should be called", function () {
                expect(BetSettingBase.initialize).toHaveBeenCalled();
            });
        });
    });
});