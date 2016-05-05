describe("RnGCasino", function () {
    describe("initialize()", function () {
        it("RnGCasino.initEvent() should be called", function () {
            spyOn(RnGCasino, "initEvent");
            RnGCasino.initialize();

            expect(RnGCasino.initEvent).toHaveBeenCalled();
        });

        it("'#btnRngCasino' should be disabled when 'isEnabled' value is false", function () {
            affix("input#isEnabled[value=False]");
            affix("button#btnRngCasino");
            RnGCasino.initialize();

            expect($("#btnRngCasino")).toBeDisabled();
        });
    });

    describe("initEvent()", function () {
        it("RnGCasino.initParams(custId, userName, nickName) should be called with correct parameters when '#btnRngCasino' fired click event", function () {
            affix("#btnRngCasino");
            $("#btnRngCasino").attr("data-cust-id", 1);
            $("#btnRngCasino").attr("data-user-name", "walter");
            $("#btnRngCasino").attr("data-nick-name", "walter");
            spyOn(RnGCasino, "initParams");
            RnGCasino.initEvent();

            $("#btnRngCasino").click();

            expect(RnGCasino.initParams).toHaveBeenCalledWith("1", "walter", "walter");
        });

        it("RnGCasino.resetBettingLimit() should be called when '#btnResetRngCasino' fired click event", function () {
            affix("#btnResetRngCasino");
            spyOn(RnGCasino, "resetBettingLimit");
            RnGCasino.initEvent();

            $("#btnResetRngCasino").click();

            expect(RnGCasino.resetBettingLimit).toHaveBeenCalled();
        });
    });

    describe("updateStakeLevel(userName, params, gameTypeChangedList)", function () {
        var request;
        beforeEach(function (done) {
            spyOn(Site, "getSingleResource").and.returnValue("Success");
            spyOn(RnGCasino, "showUpdateSettingResult");

            var gameTypeChangedList = ["A", "B"];

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("BetsettingAPI/UpdateRngCasinoBetSetting")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ "Code": 0 }'
                });
            RnGCasino.updateStakeLevel("walter", {}, gameTypeChangedList);
            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });
        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with url is 'BetsettingAPI/UpdateRngCasinoBetSetting'", function () {
            expect(request.url).toBe("BetsettingAPI/UpdateRngCasinoBetSetting");
        });

        it("Site.getSingleResource(enghlishName) should be called with correct parameter", function () {
            expect(Site.getSingleResource).toHaveBeenCalledWith("UpdateRNGCasinoBetSettingSuccessMsg");
        });

        it("RnGCasino.showUpdateSettingResult(result) should be called", function () {
            expect(RnGCasino.showUpdateSettingResult).toHaveBeenCalled();
        });
    });

    describe("compareStake(orignialStake, newStake)", function () {
        it("Value should be false when 'orignialStake' equal 'newStake' value", function () {
            var result = RnGCasino.compareStake("value1, value2, value3", "value1, value2, value3");
            expect(result).toBe(false);
        });
    });

    describe("showConfirmAction(message, custId, userName, nickName, params, gameTypeChangedList, callback)", function () {
        it("dialog function of jquery should be called", function () {
            spyOn($.fn, "dialog");

            RnGCasino.showConfirmAction("test", 1, "walter", "walter", {}, "A,B,C", function () { });

            expect($.fn.dialog).toHaveBeenCalled();
        });
    });

    describe("showDialogMesage(title, message, closePopup)", function () {
        it("dialog function of jquery should be called", function () {
            spyOn($.fn, "dialog");

            RnGCasino.showDialogMesage("title", "test", true);

            expect($.fn.dialog).toHaveBeenCalled();
        });
    });

    describe("showUpdateSettingResult", function () {
        it("RnGCasino.showDialogMesage(title, message, closePopup) should be called with correct parameters", function () {
            spyOn(RnGCasino, "showDialogMesage");

            var result = {
                Code: 0,
                Message: "Success Message"
            };

            RnGCasino.showUpdateSettingResult(result);

            expect(RnGCasino.showDialogMesage).toHaveBeenCalledWith("Success", "Success Message", false);
        });
    });
});