describe("RunningBetListMultipleBettype", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(RunningBetListMultipleBettype, "initEvent");
            spyOn(RunningBetListMultipleBettype, "submit");
            spyOn(Site, "addUserGuide");

            RunningBetListMultipleBettype.initialize();
        });

        it("RunningBetListMultipleBettype.initEvent() should be called", function () {
            expect(RunningBetListMultipleBettype.initEvent).toHaveBeenCalled();
        });

        it("RunningBetListMultipleBettype.submit() should be called", function () {
            expect(RunningBetListMultipleBettype.submit).toHaveBeenCalled();
        });

        it("Site.addUserGuide(options) should be called.", function () {
            expect(Site.addUserGuide).toHaveBeenCalled();
        });
    });

    describe("initEvent()", function () {
        beforeEach(function () {
            affix('button#SubmitButton');
            spyOn(RunningBetListMultipleBettype, "submit");

            RunningBetListMultipleBettype.initEvent();

            $('#SubmitButton').click();
        });

        it("RunningBetListMultipleBettype.submit() should be called when $('#SubmitButton') fired click event", function () {
            expect(RunningBetListMultipleBettype.submit).toHaveBeenCalled();
        });
    });

    describe("submit()", function () {
        beforeEach(function () {
            var params = {
                BaseCurrency: "eur",
                CurrencyId: 0,
                CurrencyName: "ALL",
                LicenseeType: "",
                multiplebettype: ""
            };
            spyOn(Site, "setLicenseeOptionParam").and.returnValue(params);
            spyOn(BetList, "initialize");
            spyOn(BetList, "loadData");

            RunningBetListMultipleBettype.submit();
        });

        it("BetList.initialize() should be called", function () {
            expect(BetList.initialize).toHaveBeenCalled();
        });

        it("BetList.loadData() should be called with correct parameters", function () {
            var params = {
                BaseCurrency: "eur",
                CurrencyId: 0,
                CurrencyName: "ALL",
                LicenseeType: "",
                multiplebettype: ""
            };
            expect(BetList.loadData).toHaveBeenCalledWith(params, jasmine.any(String));
        });
    });

    describe("showCombMPDetail(transId, refNo, betId, custId, betTeam, baseCurrency)", function () {
        describe("custId value is 0", function () {
            beforeEach(function () {
                spyOn(Site, "resolveClientUrl");

                RunningBetListMultipleBettype.showCombMPDetail(1, 1234, 1, 0, 4, 'eur');
            });

            it("Site.resolveClientUrl(url) should not be called", function () {
                expect(Site.resolveClientUrl).not.toHaveBeenCalled();
            });
        });

        describe("custId value not equal 0", function () {
            beforeEach(function () {
                affix("#popupSystemParlay");
                spyOn(Site, "blockUI");
                spyOn($.fn, "load").and.returnValue($("#popupSystemParlay"));
                spyOn($.fn, "dialog");

                RunningBetListMultipleBettype.showCombMPDetail(1, 1234, 1, 1, 4, 'eur');
            });

            it("Site.blockUI() should be called", function () {
                expect(Site.blockUI).toHaveBeenCalled();
            });

            it("load(url) function of jquery should be called with correct parameter", function () {
                var url = "_BetList/Popup/ViewSystemParlay.aspx?transid=1&refno=1234&betteam=4&betid=1&custid=1&basecurrency=eur";
                expect($.fn.load).toHaveBeenCalledWith(url);
            });

            it("dialog() function of jquery should be called", function () {
                expect($.fn.dialog).toHaveBeenCalled();
            });
        });
    });
});