var basePath = "pages/customer/betsetting/";
var modulePath = ["jquery", "common/site", basePath + "common/betsettingbase", basePath + "common/betsetting.copy", basePath + "products/betsettingmain"];

define(modulePath, function ($, Site, BetSettingBase, BetSettingCopy, BetSettingMain) {
    var PRODUCTID_SPORTSBOOK = 0;
    var PRODUCTID_RACING = 1;

    describe("BetSettingMain", function () {
        describe("initialize()", function () {
            beforeAll(function () {
                spyOn(Site, "initialize");
                spyOn(BetSettingBase, "initialize");
                spyOn(BetSettingBase, "setEnable");
                spyOn(BetSettingMain, "initEvent");
            });

            beforeEach(function () {
                BetSettingMain.initialize();
            });

            it("Site.initialize() should be called", function () {
                expect(Site.initialize).toHaveBeenCalled();
            });
            it("BetSettingBase.initialize() should be called", function () {
                expect(BetSettingBase.initialize).toHaveBeenCalled();
            });
            it("BetSettingBase.setEnable() should be called", function () {
                expect(BetSettingBase.setEnable).toHaveBeenCalled();
            });
            it("BetSettingMain.initEvent() should be called", function () {
                expect(BetSettingMain.initEvent).toHaveBeenCalled();
            });
        });

        describe("initEvent()", function () {
            beforeEach(function () {
                var htmlButton = '<button id="btnSubmitBetSetting">Submit</button><button id="btnReset">Reset</button>';
                setFixtures('<input type="checkbox" id="chkCopyAll"/>' + htmlButton);

                spyOn(BetSettingBase, "formatInputText");

                BetSettingMain.initEvent();
            });

            it("#chkCopyAll fire click event - BetSettingCopy.checkCopyAllValues() should be called with correct parameter", function () {
                spyOn(BetSettingCopy, "checkCopyAllValues");
                var $checkbox = $("#chkCopyAll");

                $checkbox.click();

                expect(BetSettingCopy.checkCopyAllValues).toHaveBeenCalledWith($checkbox);
            });

            it("#btnReset fire click event - BetSettingBase.resetInput() should be called", function () {
                spyOn(BetSettingBase, "resetInput");

                $("#btnReset").click();

                expect(BetSettingBase.resetInput).toHaveBeenCalled();
            });

            it("#btnSubmitBetSetting fire click event with product Sportsbook - BetSettingBase.showComfirm() should be called with correct parameter", function () {
                appendSetFixtures('<input id="ProductId" value="0" />');
                spyOn(BetSettingBase, "showComfirm");
                var $submit = $("#btnSubmitBetSetting");

                $submit.click();

                expect(BetSettingBase.showComfirm).toHaveBeenCalledWith($submit, "UpdateSportsbookBetSettings", BetSettingMain.updateBetSetting);
            });

            it("#btnSubmitBetSetting fire click event with product Racing - BetSettingBase.showComfirm() should be called with correct parameter", function () {
                appendSetFixtures('<input id="ProductId" value="1" />');
                spyOn(BetSettingBase, "showComfirm");
                var $submit = $("#btnSubmitBetSetting");

                $submit.click();

                expect(BetSettingBase.showComfirm).toHaveBeenCalledWith($submit, "UpdateRacingBetSettings", BetSettingMain.updateBetSetting);
            });

            it("#btnSubmitBetSetting fire click event with product Virtual Sports - BetSettingBase.showComfirm() should be called with correct parameter", function () {
                appendSetFixtures('<input id="ProductId" value="5" />');
                spyOn(BetSettingBase, "showComfirm");
                var $submit = $("#btnSubmitBetSetting");

                $submit.click();

                expect(BetSettingBase.showComfirm).toHaveBeenCalledWith($submit, "UpdateVirtualSportsBetSettings", BetSettingMain.updateBetSetting);
            });

            it("#btnSubmitBetSetting fire click event with product which is not supported  - BetSettingBase.showComfirm() should be called with correct parameter", function () {
                appendSetFixtures('<input id="ProductId" value="10" />');
                spyOn(BetSettingBase, "showComfirm");
                var $submit = $("#btnSubmitBetSetting");

                $submit.click();

                expect(BetSettingBase.showComfirm).toHaveBeenCalledWith($submit, "", BetSettingMain.updateBetSetting);
            });

            it("BetSettingBase.formatInputText() should be called", function () {
                expect(BetSettingBase.formatInputText).toHaveBeenCalled();
            });
        });

        describe("updateBetSetting(custId, userName, nickName)", function () {
            var custId = 1, userName = "username", nickName = "nickname";
            var expectedParam = {
                custid: custId,
                userName: userName,
                nickName: nickName
            };

            beforeAll(function () {
                spyOn(Site, "resolveClientUrl").and.callFake(function (url) { return url; });
            });

            beforeEach(function () {
                spyOn(BetSettingBase, "updateBetSetting");
            });

            it("When Product is Sportsbook - BetSettingBase.updateBetSetting should be called with correct parameter", function () {
                appendSetFixtures('<input id="ProductId" value="0" />');
                var expectedUrl = "SportsbookBetSetting/UpdateBetSetting";
                expectedParam.productId = 0;

                BetSettingMain.updateBetSetting(custId, userName, nickName);

                expect(BetSettingBase.updateBetSetting).toHaveBeenCalledWith(expectedUrl, expectedParam);
            });

            it("When Product is Racing - BetSettingBase.updateBetSetting should be called with correct parameter", function () {
                appendSetFixtures('<input id="ProductId" value="1" />');
                var expectedUrl = "RacingBetSetting/UpdateBetSetting";
                expectedParam.productId = 1;

                BetSettingMain.updateBetSetting(custId, userName, nickName);

                expect(BetSettingBase.updateBetSetting).toHaveBeenCalledWith(expectedUrl, expectedParam);
            });

            it("When Product is Virtual Sports - BetSettingBase.updateBetSetting should be called with correct parameter", function () {
                appendSetFixtures('<input id="ProductId" value="5" />');
                var expectedUrl = "DcsBetSetting/UpdateBetSetting";
                expectedParam.productId = 5;

                BetSettingMain.updateBetSetting(custId, userName, nickName);

                expect(BetSettingBase.updateBetSetting).toHaveBeenCalledWith(expectedUrl, expectedParam);
            });

            it("When Product which is not supported should not be called", function () {
                appendSetFixtures('<input id="ProductId" value="10" />');
                expectedParam.productId = 10;

                BetSettingMain.updateBetSetting(custId, userName, nickName);

                expect(BetSettingBase.updateBetSetting).not.toHaveBeenCalled();
            });
        });
    });
});