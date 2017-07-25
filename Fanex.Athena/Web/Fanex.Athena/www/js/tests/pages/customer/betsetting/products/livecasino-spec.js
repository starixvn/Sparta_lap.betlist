var basePath = "pages/customer/betsetting/";
var modulePath = ["jquery", "common/site", basePath + "common/betsettingbase", basePath + "common/betsetting.status", basePath + "products/livecasino"];

define(modulePath, function ($, Site, BetSettingBase, BetSettingStatus, LiveCasinoBetSetting) {
    describe("LiveCasinoBetSetting", function () {
        beforeAll(function () {
            spyOn(Site, "resolveClientUrl").and.callFake(function (url) { return url; });
        });

        describe("initialize", function () {
            beforeAll(function () {
                spyOn(Site, "initialize");
                spyOn(BetSettingBase, "initialize");
                spyOn(LiveCasinoBetSetting, "initEvent");
                spyOn(LiveCasinoBetSetting, "checkEnable");
            });

            beforeEach(function () {
                LiveCasinoBetSetting.initialize();
            });

            it("Site.initialize() should be called", function () {
                expect(Site.initialize).toHaveBeenCalled();
            });
            it("BetSettingBase.initialize() should be called", function () {
                expect(BetSettingBase.initialize).toHaveBeenCalled();
            });
            it("LiveCasinoBetSetting.checkEnable() should be called", function () {
                expect(LiveCasinoBetSetting.checkEnable).toHaveBeenCalled();
            });
            it("LiveCasinoBetSetting.initEvent() should be called", function () {
                expect(LiveCasinoBetSetting.initEvent).toHaveBeenCalled();
            });
        });

        describe("initEvent", function () {
            beforeEach(function () {
                var htmlGameType = '<input id="gameTypeCount" value="1" />';
                var htmlButton = '<button id="btnSubmitBetSetting">Submit</button><button id="btnReset">Reset</button><button id="btnUpdateStatus">Update Status</button>';
                var htmlInput = '<input id="oldCheck1" value="oldCheck" /> <input type="checkbox" name="gameTypeName1" data-stake-id="oldCheck" id="gameTypeName1" />';

                setFixtures(htmlButton + htmlGameType + htmlInput);

                LiveCasinoBetSetting.initEvent();
            });

            it("#btnReset fire click event - input with name=gameTypeName1 and data-stake-id=oldCheck should be checked", function () {
                $("#btnReset").click();

                var result = $("#gameTypeName1").prop("checked");

                expect(result).toBe(true);
            });

            it("#btnSubmitBetSetting fire click event - BetSettingBase.showComfirm should be called with correct parameter", function () {
                spyOn(BetSettingBase, "showComfirm");
                var $submit = $("#btnSubmitBetSetting");
                var resourceMessage = "UpdateLiveCasinoBetSettings";

                $submit.click();

                expect(BetSettingBase.showComfirm).toHaveBeenCalledWith($submit, resourceMessage, LiveCasinoBetSetting.updateBetSetting);
            });

            it("#btnUpdateStatus fire click event - LiveCasinoBetSetting.updateStatus should be called with correct parameter", function () {
                spyOn(LiveCasinoBetSetting, "updateStatus");
                var $btnUpdateStatus = $("#btnUpdateStatus");

                $btnUpdateStatus.click();

                expect(LiveCasinoBetSetting.updateStatus).toHaveBeenCalledWith($btnUpdateStatus);
            });
        });

        describe("checkEnable", function () {
            beforeEach(function () {
                var htmlCommon = '<input id="gameTypeCount" value="1" /> <input id="editable" value="False" />';
                var htmlButton = '<button id="btnSubmitBetSetting">Submit</button><button id="btnReset">Reset</button><button id="btnUpdateStatus">Update Status</button>';
                var htmlInput = '<input type="checkbox" name="gameTypeName1" id="gameTypeName1" />';

                setFixtures(htmlButton + htmlCommon + htmlInput);
            });

            it("Can not Edit - input with name=gameTypeName1 should be disabled", function () {
                LiveCasinoBetSetting.checkEnable();

                var result = $("#gameTypeName1").prop("disabled");

                expect(result).toBe(true);
            });

            it("Can not Edit - #btnSubmitBetSetting and #btnReset should be disabled and have class 'disabled'", function () {
                LiveCasinoBetSetting.checkEnable();

                var isDisabledSubmit = $("#btnSubmitBetSetting").prop("disabled");
                var isDisabledReset = $("#btnReset").prop("disabled");

                var hasDisabledClassInSubmit = $("#btnSubmitBetSetting").hasClass("disabled");
                var hasDisabledClassInReset = $("#btnReset").hasClass("disabled");

                expect(isDisabledSubmit).toBe(true);
                expect(isDisabledReset).toBe(true);
                expect(hasDisabledClassInSubmit).toBe(true);
                expect(hasDisabledClassInReset).toBe(true);
            });

            it("Can Edit - input with name=gameTypeName1 should be enabled", function () {
                $("#editable").val("True");
                LiveCasinoBetSetting.checkEnable();

                var result = $("#gameTypeName1").prop("disabled");

                expect(result).toBe(false);
            });
        });

        describe("updateBetSetting", function () {
            beforeEach(function () {
                var htmlCommon = '<input id="gameTypeCount" value="1" />';
                var htmlButton = '<button id="btnSubmitBetSetting">Submit</button><button id="btnReset">Reset</button><button id="btnUpdateStatus">Update Status</button>';
                var htmlInput = '<input id="gameTypeId1" value="1" /><input id="stakeLevelName1" value="2" /><input id="betTypeId1" value="3" />' +
                    '<input type="checkbox" checked name="gameTypeName1" data-stake-id="4" />';

                setFixtures(htmlButton + htmlCommon + htmlInput);
            });

            it("BetSettingBase.updateBetSetting should be called with correct parameter", function () {
                spyOn(BetSettingBase, "updateBetSetting");

                var custId = 1, userName = "username", nickName = "nickname";
                var expectedUrl = "LiveCasinoBetSetting/UpdateBetSetting";
                var expectedParams = {
                    custId: custId,
                    nickName: nickName,
                    gameTypeCount: 1,
                    stakeId1: 4,
                    gameTypeId1: "1",
                    stakeLevelName1: "2",
                    betTypeId1: "3"
                };

                LiveCasinoBetSetting.updateBetSetting(custId, userName, nickName);

                expect(BetSettingBase.updateBetSetting).toHaveBeenCalledWith(expectedUrl, expectedParams);
            });
        });

        describe("updateStatus", function () {
            var params = {};
            beforeEach(function () {
                var htmlButton = '<button id="btnSubmitBetSetting">Submit</button><button id="btnReset">Reset</button><button id="btnUpdateStatus">Update Status</button>';
                setFixtures(htmlButton);

                spyOn(BetSettingStatus, "getCommonPostData").and.returnValue(params);
                spyOn(BetSettingStatus, "update");
            });

            it("BetSettingStatus.update(url, params) should be called with correct parameter", function () {
                var expectedUrl = "LiveCasinoBetSetting/UpdateStatus";

                LiveCasinoBetSetting.updateStatus();

                expect(BetSettingStatus.update).toHaveBeenCalledWith(expectedUrl, params);
            });

            it("BetSettingStatus.getCommonPostData($context) should be called with correct parameter", function () {
                var expectedUrl = "LiveCasinoBetSetting/UpdateStatus";
                var $context = $("#btnUpdateStatus");

                LiveCasinoBetSetting.updateStatus($context);

                expect(BetSettingStatus.getCommonPostData).toHaveBeenCalledWith($context);
            });
        });
    });
});