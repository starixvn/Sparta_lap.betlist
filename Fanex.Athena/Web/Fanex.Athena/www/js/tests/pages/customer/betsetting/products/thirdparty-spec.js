var basePath = "pages/customer/betsetting/";
var modulePath = ["jquery", "common/site", basePath + "common/betsettingbase", basePath + "common/betsetting.status", basePath + "products/thirdparty"];

define(modulePath, function ($, Site, BetSettingBase, BetSettingStatus, ThirdPartyBetSetting) {
    var MACAU_PRODUCT = 12;
    var MGCLIVECASINO_PRODUCT = 14;
    var COLOSSUSBETS_PRODUCT = 15;
    var MGCRNGCASINO_PRODUCT = 17;
    var BETTRADE_PRODUCT = 13;
    var CASHOUT_PRODUCT = 25;

    describe("ThirdPartyBetSetting", function () {
        describe("initialize", function () {
            beforeAll(function () {
                spyOn(Site, "initialize");
                spyOn(BetSettingBase, "initialize");
                spyOn(ThirdPartyBetSetting, "initEvent");
            });

            beforeEach(function () {
                ThirdPartyBetSetting.initialize();
            });

            it("Site.initialize() should be called", function () {
                expect(Site.initialize).toHaveBeenCalled();
            });
            it("BetSettingBase.initialize() should be called", function () {
                expect(BetSettingBase.initialize).toHaveBeenCalled();
            });
            it("ThirdPartyBetSetting.initEvent() should be called", function () {
                expect(ThirdPartyBetSetting.initEvent).toHaveBeenCalled();
            });
        });

        describe("initEvent", function () {
            beforeEach(function () {
                var htmlButton = '<button id="btnUpdateStatus">Submit</button><button id="btnReset">Reset</button>';
                var htmlInput = '<input type="checkbox" id="enable-product" data-original="1"/>';
                setFixtures(htmlButton + htmlInput);

                ThirdPartyBetSetting.initEvent();
            });

            it("#btnUpdateStatus fire click event - ThirdPartyBetSetting.updateStatus() should be called", function () {
                spyOn(ThirdPartyBetSetting, "updateStatus");
                var $btnUpdateStatus = $("#btnUpdateStatus");

                $btnUpdateStatus.click();

                expect(ThirdPartyBetSetting.updateStatus).toHaveBeenCalledWith($btnUpdateStatus);
            });

            it("#btnReset fire click event - #enable-product should be checked", function () {
                $("#btnReset").click();

                var result = $("#enable-product").prop("checked");

                expect(result).toBe(true);
            });
        });

        describe("updateStatus", function () {
            function initialize(productId, isCheckBoxChecking) {
                productId = productId || MACAU_PRODUCT;
                var checkedText = isCheckBoxChecking ? " checked" : "";

                var htmlProductId = '<input id="ProductId" value="' + productId + '"/>';
                var htmlCheckbox = '<input type="checkbox" id="enable-product" ' + checkedText + '/>';
                setFixtures(htmlProductId + htmlCheckbox);
            }

            function assertByProductAndUrl(productId, url) {
                initialize(productId);

                ThirdPartyBetSetting.updateStatus();

                expect(BetSettingStatus.update).toHaveBeenCalledWith(url, params);
            }

            var params = {};
            beforeAll(function () {
                spyOn(BetSettingStatus, "getCommonPostData").and.returnValue(params);
                spyOn(Site, "resolveClientUrl").and.callFake(function (url) { return url; });
            });

            beforeEach(function () {
                spyOn(BetSettingStatus, "update");
            });

            it("Product is Macau - BetSettingStatus.update() should be called with correct parameter", function () {
                var expectedUrl = "ThirdPartyBetSetting/UpdateMacauStatus";
                assertByProductAndUrl(MACAU_PRODUCT, expectedUrl);
            });

            it("Product is MgcLiveCasino - BetSettingStatus.update() should be called with correct parameter", function () {
                var expectedUrl = "ThirdPartyBetSetting/UpdateMicroGamingLiveCasinoStatus";
                assertByProductAndUrl(MGCLIVECASINO_PRODUCT, expectedUrl);
            });

            it("Product is Colossusbets - BetSettingStatus.update() should be called with correct parameter", function () {
                var expectedUrl = "ThirdPartyBetSetting/UpdateColossusBetsStatus";
                assertByProductAndUrl(COLOSSUSBETS_PRODUCT, expectedUrl);
            });

            it("Product is MgcRngCasino - BetSettingStatus.update() should be called with correct parameter", function () {
                var expectedUrl = "ThirdPartyBetSetting/UpdateMicroGamingRNGCasinoStatus";
                assertByProductAndUrl(MGCRNGCASINO_PRODUCT, expectedUrl);
            });

            it("Product is BetTrade - BetSettingStatus.update() should be called with correct parameter", function () {
                var expectedUrl = "ThirdPartyBetSetting/UpdateBetTradeStatus";
                assertByProductAndUrl(BETTRADE_PRODUCT, expectedUrl);
            });

            it("Product is Cashout - BetSettingStatus.update() should be called with correct parameter", function () {
                var expectedUrl = "ThirdPartyBetSetting/UpdateCashOutStatus";
                assertByProductAndUrl(CASHOUT_PRODUCT, expectedUrl);
            });

            it("url is null - BetSettingStatus.update() should not be called", function () {
                initialize(1);

                ThirdPartyBetSetting.updateStatus();

                expect(BetSettingStatus.update).not.toHaveBeenCalled();
            });

            it("Product is MgcLiveCasino - param has more status property - BetSettingStatus.update() should be called with correct parameter", function () {
                var expectedUrl = "ThirdPartyBetSetting/UpdateMicroGamingLiveCasinoStatus";
                var expectedParam = { status: 1 };
                initialize(MGCLIVECASINO_PRODUCT, true);

                ThirdPartyBetSetting.updateStatus();

                expect(BetSettingStatus.update).toHaveBeenCalledWith(expectedUrl, expectedParam);
            });
        });
    });
});