describe("GoldDeluxe", function () {
    beforeEach(function () {
        var bettingLimitJsonData = '[{"GameType":"BAC","Name":"Baccarat","Groups":[{"GroupType":"A","Min":5,"Max":10000},{"GroupType":"B","Min":10,"Max":20000},{"GroupType":"C","Min":30,"Max":50000}]},{"GameType":"DT","Name":"Dragon Tiger","Groups":[{"GroupType":"A","Min":5,"Max":10000},{"GroupType":"B","Min":10,"Max":20000}]}]';
        affix("#BettingLimit");
        $("#BettingLimit").val(bettingLimitJsonData);

        affix("input#betSettingGroup[value=A]");
        affix("button#btnSubmitBetSetting");

        GoldDeluxe.initialize();
    });

    describe("initialize()", function () {
        it("GoldDeluxe.initEvent() should be called", function () {
            spyOn(GoldDeluxe, "initEvent");
            GoldDeluxe.initialize();

            expect(GoldDeluxe.initEvent).toHaveBeenCalled();
        });
    });

    describe("initEvent()", function () {
        it("'#btnSubmitBetSetting' should be enabled when '#betSettingGroup' fired change event", function () {
            $("#betSettingGroup").change();
            expect($("#btnSubmitBetSetting")).not.toBeDisabled();
        });
    });

    describe("checkApiContainGroup(groupType)", function () {
        it("Result should be 'true' with groupType is 'A'", function () {
            var result = GoldDeluxe.checkApiContainGroup("A");
            expect(result).toBe(true);
        });
        it("Result should be 'false' with groupType is 'D'", function () {
            var result = GoldDeluxe.checkApiContainGroup("D");
            expect(result).toBe(false);
        });
    });
});