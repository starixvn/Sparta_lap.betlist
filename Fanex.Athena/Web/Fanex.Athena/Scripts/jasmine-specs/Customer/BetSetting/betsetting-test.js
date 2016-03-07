describe("BetSetting", function () {
    beforeEach(function () {
        affix("input#IDList[value=1,2,3]");
    });

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

        it("'#max-per-ball-161' should be disabled when tabId equal TAB_NUMBERGAME", function () {
            affix("input#max-per-ball-161");
            spyOn(BetSetting, "checkEnable");
            BetSetting.checkEnableBetSetting(2, false);

            expect($("#max-per-ball-161")).toBeDisabled();
        });

        it("'#copy-settting' should be hide when tabId equal TAB_SPORTSBOOK", function () {
            affix("#copy-settting");
            spyOn(BetSetting, "checkEnable");
            BetSetting.checkEnableBetSetting(0, false);

            expect($("#copy-settting")).not.toBeVisible();
        });
    });

    describe("checkEnableLiveCasino(enable)", function () {
        it("'input[name=gameTypeName1]' should be disabled when param value is false", function () {
            affix("input#gameTypeCount[value=2]");
            affix("input[name=gameTypeName1]");

            BetSetting.checkEnableLiveCasino(false);

            expect($("input[name=gameTypeName1]")).toBeDisabled();
        });

        it("'#btnSubmitBetSetting, #btnReset' should be disabled when param value is false", function () {
            affix("button#btnSubmitBetSetting");
            affix("button#btnReset");

            BetSetting.checkEnableLiveCasino(false);

            expect($("#btnSubmitBetSetting")).toBeDisabled();
            expect($("#btnReset")).toBeDisabled();
        });
    });

    describe("updateBetSetting()", function () {
        beforeEach(function () {
            affix("#ProductId");
            spyOn(BetSetting, "checkValidationNumberGame").and.returnValue(true);
            spyOn(BetSetting, "checkValidation").and.returnValue(true);
            spyOn(BetSetting, "showConfirmAction");
        });

        it("BetSetting.showConfirmAction(message, custId, userName, nickName, callback) should be called", function () {
            $("#ProductId").val(1);

            BetSetting.updateBetSetting();

            expect(BetSetting.showConfirmAction).toHaveBeenCalled();
        });

        it("AgCasino.checkApiContainGroup(group) should be called with correct parameter", function () {
            affix("input#betSettingGroup[value=1]");
            $("#ProductId").val(10);

            spyOn(AgCasino, "checkApiContainGroup");

            BetSetting.updateBetSetting();

            expect(AgCasino.checkApiContainGroup).toHaveBeenCalledWith("1");
        });
    });

    describe("checkValidation()", function () {
        beforeEach(function () {
            affix("input#chkCopyAll[type=checkbox]");
            spyOn(BetSetting, "checkCopyAllValues");
            spyOn(BetSetting, "checkLimitMinMaxBets");

            BetSetting.checkValidation();
        });

        it("BetSetting.checkCopyAllValues($checkbox) should be called", function () {
            expect(BetSetting.checkCopyAllValues).toHaveBeenCalled();
        });
        it("BetSetting.checkLimitMinMaxBets(sportIds) should be called", function () {
            expect(BetSetting.checkLimitMinMaxBets).toHaveBeenCalled();
        });
    });

    describe("checkValidationNumberGame()", function () {
        it("BetSetting.checkLimitMinMaxBet(sportId) should be called", function () {
            spyOn(BetSetting, "checkLimitMinMaxBet").and.returnValue(false);

            BetSetting.checkValidationNumberGame();

            expect(BetSetting.checkLimitMinMaxBet).toHaveBeenCalled();
        });
    });

    describe("updateGoldDeluxeBetSetting(custId, userName, nickName)", function () {
        var request;
        beforeEach(function (done) {
            spyOn(BetSetting, "showUpdateSettingResult");

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("BetSettingAPI/UpdateGoldDeluxeBetSetting")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ }'
                });

            BetSetting.updateGoldDeluxeBetSetting(1, "user01", "walter");

            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with url is 'BetSettingAPI/UpdateGoldDeluxeBetSetting'", function () {
            expect(request.url).toBe("BetSettingAPI/UpdateGoldDeluxeBetSetting");
        });

        it("BetSetting.showUpdateSettingResult(result) should be called", function () {
            expect(BetSetting.showUpdateSettingResult).toHaveBeenCalled();
        });
    });

    describe("updateAgCasinoBetSetting(custId, userName, nickName)", function () {
        var request;
        beforeEach(function (done) {
            spyOn(BetSetting, "showUpdateSettingResult");

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("BetSettingAPI/UpdateAgCasinoBetSetting")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ }'
                });

            BetSetting.updateAgCasinoBetSetting(1, "user01", "walter");

            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with url is 'BetSettingAPI/UpdateAgCasinoBetSetting'", function () {
            expect(request.url).toBe("BetSettingAPI/UpdateAgCasinoBetSetting");
        });

        it("BetSetting.showUpdateSettingResult(result) should be called", function () {
            expect(BetSetting.showUpdateSettingResult).toHaveBeenCalled();
        });
    });

    describe("updateKenoBetSetting(custId, userName, nickName)", function () {
        var request;
        beforeEach(function (done) {
            spyOn(BetSetting, "getBetSettingParamsForSports").and.returnValue({});
            spyOn(BetSetting, "showUpdateSettingResult");

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("BetSettingAPI/UpdateKenoBetSetting?")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ }'
                });

            BetSetting.updateKenoBetSetting(1, "user01", "walter");

            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with url is 'BetSettingAPI/UpdateKenoBetSetting?'", function () {
            expect(request.url).toBe("BetSettingAPI/UpdateKenoBetSetting?");
        });

        it("BetSetting.getBetSettingParamsForSports(sportIds) should be called", function () {
            expect(BetSetting.getBetSettingParamsForSports).toHaveBeenCalled();
        });
    });

    describe("updateSportsbookBetSetting(custId, userName, nickName)", function () {
        var request;
        beforeEach(function (done) {
            spyOn(BetSetting, "getBetSettingParamsForSports").and.returnValue({});
            spyOn(BetSetting, "showUpdateSettingResult");

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("BetSettingAPI/UpdateSportBookBetSetting")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ }'
                });

            BetSetting.updateSportsbookBetSetting(1, "user01", "walter");

            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with url is 'BetSettingAPI/UpdateSportBookBetSetting'", function () {
            expect(request.url).toBe("BetSettingAPI/UpdateSportBookBetSetting");
        });

        it("BetSetting.getBetSettingParamsForSports(sportIds) should be called", function () {
            expect(BetSetting.getBetSettingParamsForSports).toHaveBeenCalled();
        });
    });

    describe("updateRacingBetSetting(custId, userName, nickName)", function () {
        var request;
        beforeEach(function (done) {
            spyOn(BetSetting, "getBetSettingParamsForSports").and.returnValue({});
            spyOn(BetSetting, "showUpdateSettingResult");

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("BetSettingAPI/updateRacingBetSetting")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ }'
                });

            BetSetting.updateRacingBetSetting(1, "user01", "walter");

            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with url is 'BetSettingAPI/updateRacingBetSetting?'", function () {
            expect(request.url).toBe("BetSettingAPI/updateRacingBetSetting?");
        });

        it("BetSetting.getBetSettingParamsForSports(sportIds) should be called", function () {
            expect(BetSetting.getBetSettingParamsForSports).toHaveBeenCalled();
        });
    });

    describe("updateNumberGameBetSetting(custId, userName, nickName)", function () {
        var request;
        beforeEach(function (done) {
            affix("input#max-per-ball-161[value=3]");
            spyOn(BetSetting, "getBetSettingParams").and.returnValue({});
            spyOn(BetSetting, "showUpdateSettingResult");

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("BetSettingAPI/updateNumberGameBetSetting")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ }'
                });

            BetSetting.updateNumberGameBetSetting(1, "user01", "walter");

            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with url is 'BetSettingAPI/updateNumberGameBetSetting'", function () {
            expect(request.url).toBe("BetSettingAPI/updateNumberGameBetSetting");
        });

        it("BetSetting.getBetSettingParams(sportId) should be called", function () {
            expect(BetSetting.getBetSettingParams).toHaveBeenCalled();
        });
    });

    describe("updateVirtualSportsBetSetting(custId, userName, nickName)", function () {
        var request;
        beforeEach(function (done) {
            spyOn(BetSetting, "getBetSettingParamsForSports").and.returnValue({});
            spyOn(BetSetting, "showUpdateSettingResult");

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("BetSettingAPI/updateVirtualSportsBetSetting?")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ }'
                });

            BetSetting.updateVirtualSportsBetSetting(1, "user01", "walter");

            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with url is 'BetSettingAPI/updateVirtualSportsBetSetting?'", function () {
            expect(request.url).toBe("BetSettingAPI/updateVirtualSportsBetSetting?");
        });

        it("BetSetting.getBetSettingParamsForSports(sportIds) should be called", function () {
            expect(BetSetting.getBetSettingParamsForSports).toHaveBeenCalled();
        });
    });

    describe("updateLiveCasinoBetSetting(custId, userName, nickName)", function () {
        var request;
        beforeEach(function (done) {
            affix("input#gameTypeCount[value=2]")
            spyOn(BetSetting, "showDialogMesage");

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("BetSettingAPI/updateLiveCasinoBetSetting?")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ Code: "0" }'
                });

            BetSetting.updateLiveCasinoBetSetting(1, "user01", "walter");

            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with url is 'BetSettingAPI/updateLiveCasinoBetSetting?'", function () {
            expect(request.url).toBe("BetSettingAPI/updateLiveCasinoBetSetting?");
        });

        it("BetSetting.showDialogMesage should be called", function () {
            expect(BetSetting.showDialogMesage).toHaveBeenCalled();
        });
    });

    describe("getBetSettingParams(sportId)", function () {
        beforeEach(function () {
            affix("input#min-bet-1[value=10]");
            affix("input#max-bet-1[value=10]");
            affix("input#max-per-match-1[value=10]");
            $("#min-bet-1").attr("data-original", 20);
            $("#max-bet-1").attr("data-original", 20);
            $("#max-per-match-1").attr("data-original", 20);
        });

        it("Value returned should be equal with expected object when value not equal data-original", function () {
            var expectedObj = { minbet1: 10, maxbet1: 10, maxpermatch1: 10, update1: true };

            var result = BetSetting.getBetSettingParams(1);

            expect(result).toEqual(expectedObj);
        });

        it("Value returned should be equal with expected object when value equal data-original", function () {
            $("#min-bet-1").val(20);
            $("#max-bet-1").val(20);
            $("#max-per-match-1").val(20);
            var expectedObj = { minbet1: 20, maxbet1: 20, maxpermatch1: 20, update1: false };

            var result = BetSetting.getBetSettingParams(1);

            expect(result).toEqual(expectedObj);
        });
    });
});