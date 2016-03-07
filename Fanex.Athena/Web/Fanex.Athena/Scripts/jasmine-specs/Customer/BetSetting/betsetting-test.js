describe("BetSetting", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(BetSetting, "initMenu");
            spyOn(BetSetting, "initEvent");

            BetSetting.initialize();
        });

        it("BetSetting.initMenu() should be called", function () {
            expect(BetSetting.initMenu).toHaveBeenCalled();
        });
        it("BetSetting.initEvent() should be called", function () {
            expect(BetSetting.initEvent).toHaveBeenCalled();
        });
    });

    describe("initEvent()", function () {
        beforeEach(function () {
            affix("input#dataCount[value=5]");
            affix("#chkCopyAll");
            affix("#btnSubmitBetSetting");
            affix("#btnReset");
            affix("#btnUpdateStatus");
            affix("#btnDisableEsports");

            spyOn(BetSetting, "checkCopyAllValues");
            spyOn(BetSetting, "updateBetSetting");
            spyOn(BetSetting, "resetBetSetting");
            spyOn(BetSetting, "updateStatus");
            spyOn(BetSetting, "updateEsportStatus");
            spyOn(BetSetting, "formatInputText");

            BetSetting.initEvent();
        });

        it("BetSetting.checkCopyAllValues($checkbox) should be called with correct parameter when #chkCopyAll fired click event", function () {
            $("#chkCopyAll").click();
            expect(BetSetting.checkCopyAllValues).toHaveBeenCalledWith($("#chkCopyAll"));
        });
        it("BetSetting.updateBetSetting should be called when #btnSubmitBetSetting fired click event", function () {
            $("#btnSubmitBetSetting").click();
            expect(BetSetting.updateBetSetting).toHaveBeenCalled();
        });
        it("BetSetting.resetBetSetting should be called when #btnReset fired click event", function () {
            $("#btnReset").click();
            expect(BetSetting.resetBetSetting).toHaveBeenCalled();
        });
        it("BetSetting.updateStatus should be called when #btnUpdateStatus fired click event", function () {
            $("#btnUpdateStatus").click();
            expect(BetSetting.updateStatus).toHaveBeenCalled();
        });
        it("BetSetting.updateEsportStatus should be called when #btnDisableEsports fired click event", function () {
            $("#btnDisableEsports").click();
            expect(BetSetting.updateEsportStatus).toHaveBeenCalled();
        });

        it("BetSetting.formatInputText() should be called", function () {
            expect(BetSetting.formatInputText).toHaveBeenCalled();
        });
    });

    describe("initMenu()", function () {
        beforeEach(function () {
            affix("input#ProductId[value=1]");
            affix("input#editable[value=False]");
            affix("#product-dropdown a");

            spyOn($.fn, "toggleClass");
            spyOn(Site, "blockUI");
            spyOn(BetSetting, "checkEnableBetSetting");
            BetSetting.initMenu();          
        });

        it("toggleClass function of jquery should be called with correct parameter when #product-dropdown fired click event", function () {           
            $("#product-dropdown").click();
            expect($.fn.toggleClass).toHaveBeenCalledWith("active");
        });
        it("Site.blockUI() should be called when '#product-dropdown a' fired click event", function () {
            $("#product-dropdown a").click();
            expect(Site.blockUI).toHaveBeenCalled();
        });

        it("BetSetting.checkEnableBetSetting(productId, enable) should be called with correct parameter", function () {
            expect(BetSetting.checkEnableBetSetting).toHaveBeenCalledWith(1, false);
        });
    });

    describe("updateStatus()", function () {
        var request;
        beforeEach(function (done) {
            affix("input#ProductId[value=11]");
            spyOn(BetSetting, "showDialogMesage");

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("BetSettingAPI/UpdateGoldDeluxeStatus")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ IsSuccessful: true }'
                });

            BetSetting.updateStatus();

            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with url is 'BetSettingAPI/UpdateGoldDeluxeStatus'", function () {
            expect(request.url).toBe("BetSettingAPI/UpdateGoldDeluxeStatus");
        });
    });

    describe("updateEsportStatus()", function () {
        var request;
        beforeEach(function (done) {
            spyOn(BetSetting, "showDialogMesage");

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("BetSettingAPI/UpdateEsportStatus")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ }'
                });

            BetSetting.updateEsportStatus();

            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with url is 'BetSettingAPI/UpdateEsportStatus'", function () {
            expect(request.url).toBe("BetSettingAPI/UpdateEsportStatus");
        });
    });

    describe("checkEnableBetSetting(tabId, enable)", function () {
        it("BetSetting.checkEnableLiveCasino(enable) should be called with correct parameter when tabId is TAB_LIVECASINO", function () {
            spyOn(BetSetting, "checkEnableLiveCasino");
            BetSetting.checkEnableBetSetting(4, true);

            expect(BetSetting.checkEnableLiveCasino).toHaveBeenCalledWith(true);
        });
        it("BetSetting.checkEnable(sportIds, enable) should not be called when tabId equal TAB_LIVECASINO", function () {
            spyOn(BetSetting, "checkEnable");
            BetSetting.checkEnableBetSetting(4, true);

            expect(BetSetting.checkEnable).not.toHaveBeenCalled();
        });

        it("BetSetting.checkEnable() should be called when tabId not equal TAB_LIVECASINO", function () {
            spyOn(BetSetting, "checkEnable");
            BetSetting.checkEnableBetSetting(2, true);

            expect(BetSetting.checkEnable).toHaveBeenCalled();
        });
    });
});