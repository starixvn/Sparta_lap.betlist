var basePath = "pages/customer/betsetting/";
var modulePath = ["jquery", "common/site", basePath + "common/betsettingbase", basePath + "common/betsetting.status", basePath + "products/dcsbetsetting"];

define(modulePath, function ($, Site, BetSettingBase, BetSettingStatus, DcsBetSetting) {
    describe("DcsBetSetting", function () {
        describe("initialize", function () {
            beforeAll(function () {
                spyOn(Site, "initialize");
                spyOn(BetSettingBase, "initialize");
                spyOn(DcsBetSetting, "initEvent");
            });

            beforeEach(function () {
                DcsBetSetting.initialize();
            });

            it("Site.initialize() should be called", function () {
                expect(Site.initialize).toHaveBeenCalled();
            });
            it("BetSettingBase.initialize() should be called", function () {
                expect(BetSettingBase.initialize).toHaveBeenCalled();
            });
            it("DcsBetSetting.initEvent() should be called", function () {
                expect(DcsBetSetting.initEvent).toHaveBeenCalled();
            });
        });

        describe("initEvent()", function () {
            it("#btnUpdateStatus fire click event - DcsBetSetting.updateStatus() should be called with correct parameter", function () {
                setFixtures('<button id="btnUpdateStatus"></button>');
                spyOn(DcsBetSetting, "updateStatus");
                DcsBetSetting.initEvent();

                var $button = $("#btnUpdateStatus");

                $button.click();

                expect(DcsBetSetting.updateStatus).toHaveBeenCalledWith($button);
            });


            it("#btnSubmitBetSetting fire click event with product Virtual Sports - BetSettingBase.showComfirm() should be called with correct parameter", function () {
                setFixtures('<input id="ProductId" value="5" /><button id="btnSubmitBetSetting"></button>');
                spyOn(BetSettingBase, "showComfirm");
                DcsBetSetting.initEvent();
                var $submit = $("#btnSubmitBetSetting");

                $submit.click();

                expect(BetSettingBase.showComfirm).toHaveBeenCalledWith($submit, "UpdateProductBetSettings", DcsBetSetting.updateBetSetting);
            });

        });

        describe("updateStatus($context)", function () {
            var params = {};
            beforeAll(function () {
                spyOn(Site, "resolveClientUrl").and.callFake(function (url) { return url; });
                spyOn(BetSettingStatus, "getCommonPostData").and.returnValue(params);
                spyOn(BetSettingStatus, "update");
            });

            it("BetSettingStatus.update(url, params) should be called with crrect parameter", function () {
                var expectedUrl = "DcsBetSetting/UpdateStatus";

                DcsBetSetting.updateStatus();

                expect(BetSettingStatus.update).toHaveBeenCalledWith(expectedUrl, params);
            });

            it("BetSettingStatus.getCommonPostData() should be called with correct parameter", function () {
                setFixtures('<button id="btnUpdateStatus">UPdate Status</button>');
                var $context = $("#btnUpdateStatus");

                DcsBetSetting.updateStatus($context);

                expect(BetSettingStatus.getCommonPostData).toHaveBeenCalledWith($context);
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

            it("When Product is Virtual Sports - BetSettingBase.updateBetSetting should be called with correct parameter", function () {
                appendSetFixtures('<input id="ProductId" value="5" />');
                var expectedUrl = "DcsBetSetting/UpdateBetSetting";
                expectedParam.productId = 5;

                DcsBetSetting.updateBetSetting(custId, userName, nickName);

                expect(BetSettingBase.updateBetSetting).toHaveBeenCalledWith(expectedUrl, expectedParam);
            });
        });
    });
});