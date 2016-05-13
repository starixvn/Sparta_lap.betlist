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
                .stubRequest("GoldDeluxeBetSetting/UpdateStatus")
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

        it("ajax request should be called with url is 'GoldDeluxeBetSetting/UpdateStatus'", function () {
            expect(request.url).toBe("GoldDeluxeBetSetting/UpdateStatus");
        });
    });

    describe("updateEsportStatus()", function () {
        var request;
        beforeEach(function (done) {
            spyOn(BetSetting, "showDialogMesage");

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("SportbooksBetSetting/UpdateEsportStatus")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ Code: 0 }'
                });

            BetSetting.updateEsportStatus();

            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with url is 'SportbooksBetSetting/UpdateEsportStatus'", function () {
            expect(request.url).toBe("SportbooksBetSetting/UpdateEsportStatus");
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
                .stubRequest("GoldDeluxeBetSetting/UpdateBetSetting")
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

        it("ajax request should be called with url is 'GoldDeluxeBetSetting/UpdateBetSetting'", function () {
            expect(request.url).toBe("GoldDeluxeBetSetting/UpdateBetSetting");
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
                .stubRequest("AgCasinoBetSetting/UpdateBetSetting")
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

        it("ajax request should be called with url is 'AgCasinoBetSetting/UpdateBetSetting'", function () {
            expect(request.url).toBe("AgCasinoBetSetting/UpdateBetSetting");
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
                .stubRequest("KenoBetSetting/UpdateBetSetting?")
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

        it("ajax request should be called with url is 'KenoBetSetting/UpdateBetSetting?'", function () {
            expect(request.url).toBe("KenoBetSetting/UpdateBetSetting?");
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
                .stubRequest("SportsbookBetSetting/UpdateBetSetting")
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

        it("ajax request should be called with url is 'SportsbookBetSetting/UpdateBetSetting'", function () {
            expect(request.url).toBe("SportsbookBetSetting/UpdateBetSetting");
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
                .stubRequest("RacingBetSetting/updateBetSetting")
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

        it("ajax request should be called with url is 'RacingBetSetting/updateBetSetting?'", function () {
            expect(request.url).toBe("RacingBetSetting/updateBetSetting");
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
                .stubRequest("NumberGameBetSetting/UpdateBetSetting")
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

        it("ajax request should be called with url is 'NumberGameBetSetting/UpdateBetSetting'", function () {
            expect(request.url).toBe("NumberGameBetSetting/UpdateBetSetting");
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
                .stubRequest("VirtualSportsBetSetting/updateBetSetting?")
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

        it("ajax request should be called with url is 'VirtualSportsBetSetting/updateBetSetting?'", function () {
            expect(request.url).toBe("VirtualSportsBetSetting/updateBetSetting?");
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
                .stubRequest("LiveCasinoBetSetting/updateBetSetting")
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

        it("ajax request should be called with url is 'LiveCasinoBetSetting/updateBetSetting?'", function () {
            expect(request.url).toBe("LiveCasinoBetSetting/updateBetSetting");
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

    describe("resetBetSetting()", function () {
        beforeEach(function () {
            affix("input#ProductId[value=4]");

            spyOn(BetSetting, "resetLiveCasinoBetSetting");
            spyOn(BetSetting, "resetValues");
        });

        it("BetSetting.resetLiveCasinoBetSetting() should be called when productId equal TAB_LIVECASINO", function () {
            BetSetting.resetBetSetting();

            expect(BetSetting.resetLiveCasinoBetSetting).toHaveBeenCalled();
        });

        it("BetSetting.resetValues() should be called when productId equal default", function () {
            $("#ProductId").val(0);

            BetSetting.resetBetSetting();

            expect(BetSetting.resetValues).toHaveBeenCalled();
        });
    });

    describe("resetValues()", function () {
        beforeEach(function () {
            affix("input#min-bet[data-original=10]");
            affix("input#max-bet[data-original=12]");
            affix("input#max-per-match[data-original=15]");

            BetSetting.resetValues();
        });

        it("Values of inputs should be equal data-original", function () {
            expect($("#min-bet").val()).toBe("10");
            expect($("#max-bet").val()).toBe("12");
            expect($("#max-per-match").val()).toBe("15");
        });

        it("Inputs should not be disabled", function () {
            expect($("#min-bet")).not.toBeDisabled();
            expect($("#max-bet")).not.toBeDisabled();
            expect($("#max-per-match")).not.toBeDisabled();
        });
    });

    describe("resetBetTradeSetting()", function () {
        it("'#enable-product' should be checked when original data equal 1", function () {
            affix("input#enable-product[type=checkbox]");
            var $checkbox = $("#enable-product");
            $checkbox.data("original", 1);

            BetSetting.resetBetTradeSetting();

            expect($checkbox).toBeChecked();
        });
    });

    describe("resetLiveCasinoBetSetting()", function () {
        it("'input[name=gameTypeName1]' should be checked", function () {
            affix("input#gameTypeCount[value=1]");
            affix("input#oldCheck1[value=1]");
            affix("input#checkbox[type=checkbox]");

            var $checkbox = $("#checkbox");
            $checkbox.attr("name", "gameTypeName1");
            $checkbox.attr("data-stake-id", 1);

            BetSetting.resetLiveCasinoBetSetting();

            expect(("input[name=gameTypeName1]")).toBeChecked();
        });
    });

    describe("checkCopyAllValues($checkbox)", function () {
        it("BetSetting.setCopyValueBetSetting($element, enabled) should be called", function () {
            affix("input#checkbox[type=checkbox]");
            spyOn(BetSetting, "setCopyValueBetSetting");

            BetSetting.checkCopyAllValues($("#checkbox"));

            expect(BetSetting.setCopyValueBetSetting).toHaveBeenCalled();
        });
    });

    describe("setMinMaxBet(sportId, minBet, maxBet, maxPerMatch)", function () {
        it("Inputs should have correct value", function () {
            affix("input#min-bet-1");
            affix("input#max-bet-1");
            affix("input#max-per-match-1");

            BetSetting.setMinMaxBet(1, 10, 20, 15);

            expect($("#min-bet-1").val()).toBe("10");
            expect($("#max-bet-1").val()).toBe("20");
            expect($("#max-per-match-1").val()).toBe("15");
        });
    });

    describe("checkLimitMinMaxBets(sportIds)", function () {
        it("Result should be false when BetSetting.checkLimitMinMaxBet(sportId) return false", function () {
            spyOn(BetSetting, "checkLimitMinMaxBet").and.returnValue(false);

            var result = BetSetting.checkLimitMinMaxBets([1, 2, 3]);

            expect(result).toBe(false);
        })
    });

    describe("checkLimitMinMaxBet(sportId)", function () {
        beforeEach(function () {
            affix("input#min-bet-1[data-min-value=10]");
            affix("input#max-bet-1[data-max-value=20]");
            affix("input#max-per-match-1[data-min-value=15]");
            $("#max-per-match-1").attr("data-max-value", 18);

            $("#min-bet-1").val(12);
            $("#max-bet-1").val(16);
            $("#max-per-match-1").val(17);

            spyOn(BetSetting, "showBalloonMsg");
        });

        it("Result should be true with initial values", function () {
            var result = BetSetting.checkLimitMinMaxBet(1);

            expect(result).toBe(true);
        });

        it("Result should be true when all values equal 0", function () {
            $("#min-bet-1").val(0);
            $("#max-bet-1").val(0);
            $("#max-per-match-1").val(0);

            var result = BetSetting.checkLimitMinMaxBet(1);

            expect(result).toBe(true);
        });

        it("Result should be false with '#min-bet-1' value greater than max value", function () {
            $("#min-bet-1").val(22);
            var result = BetSetting.checkLimitMinMaxBet(1);

            expect(result).toBe(false);
        });

        it("Result should be false with maxbet value greater than max per match", function () {
            $("#max-bet-1").val(18);
            $("#max-per-match-1").val(17);
            var result = BetSetting.checkLimitMinMaxBet(1);

            expect(result).toBe(false);
        });
    });

    describe("convertToFloat(str)", function () {
        it("Result should be float number when pass valid number with string", function () {
            var result = BetSetting.convertToFloat("2");

            expect(result).toBe(2.0);
        });

        it("Result should be float number when pass valid float number with string", function () {
            var result = BetSetting.convertToFloat("2.5");

            expect(result).toBe(2.5);
        });

        it("Result should be NaN when pass invalid string", function () {
            var result = BetSetting.convertToFloat("abc");

            expect(result).toEqual(NaN);
        });
    });

    describe("isInteger(s)", function () {
        it("Result should be true number when pass valid number with string", function () {
            var result = BetSetting.isInteger("2");

            expect(result).toBe(true);
        });

        it("Result should be false when pass empty string", function () {
            var result = BetSetting.isInteger("");

            expect(result).toBe(false);
        });

        it("Result should be false when pass string value", function () {
            var result = BetSetting.isInteger("abc");

            expect(result).toBe(false);
        });
    });

    describe("isDigit", function () {
        it("Result should be true when pass valid digit", function () {
            var result = BetSetting.isDigit("2");

            expect(result).toBe(true);
        });

        it("Result should be false when pass invalid digit", function () {
            var result = BetSetting.isDigit("g");

            expect(result).toBe(false);
        });
    });

    describe("isEmpty", function () {
        it("Result should be true when pass empty string", function () {
            var result = BetSetting.isEmpty("");

            expect(result).toBe(true);
        });

        it("Result should be true when pass white space", function () {
            var result = BetSetting.isEmpty(" ");

            expect(result).toBe(true);
        });

        it("Result should be false when pass character", function () {
            var result = BetSetting.isEmpty("h");

            expect(result).toBe(false);
        });
    });

    describe("showDialogMesage(title, message, closePopup)", function () {
        it("dialog function of jquery should be called", function () {
            spyOn($.fn, "dialog");

            BetSetting.showDialogMesage("title", "This is message", function () { });

            expect($.fn.dialog).toHaveBeenCalled();
        });
    });

    describe("showConfirmAction(message, custId, userName, nickName, callback)", function () {
        it("dialog function of jquery should be called", function () {
            spyOn($.fn, "dialog");

            BetSetting.showConfirmAction("This is message", 1, "user01", "walter", function () { });

            expect($.fn.dialog).toHaveBeenCalled();
        });
    });
});