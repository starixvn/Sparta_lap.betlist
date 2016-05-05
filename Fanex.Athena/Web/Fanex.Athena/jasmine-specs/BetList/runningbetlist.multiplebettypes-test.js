describe("RunningBetListMultipleBettype", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            userGuideName = "UserGuideName-test";
            popupTitle = "PopupTitle-test";
            spyOn(RunningBetListMultipleBettype, "initEvent");
            spyOn(RunningBetListMultipleBettype, "switchItemSelectedMixParlay");
            spyOn(RunningBetListMultipleBettype, "submit");
            spyOn(Site, "addUserGuide");

            RunningBetListMultipleBettype.initialize();
        });

        it("RunningBetListMultipleBettype.initEvent() should be called", function () {
            expect(RunningBetListMultipleBettype.initEvent).toHaveBeenCalled();
        });

        it("RunningBetListMultipleBettype.switchItemSelectedMixParlay() should be called", function () {
            expect(RunningBetListMultipleBettype.switchItemSelectedMixParlay).toHaveBeenCalled();
        });

        it("RunningBetListMultipleBettype.submit() should be called", function () {
            expect(RunningBetListMultipleBettype.submit).toHaveBeenCalled();
        });

        it("Site.addUserGuide(options) should be called with correct parameters", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "UserGuideName-test", title: "TotalBets - PopupTitle-test" });
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
            var url = 'api/betlist/RunningBetListMultipleBettypes';
            expect(BetList.loadData).toHaveBeenCalledWith(params, url);
        });
    });

    describe("switchMixParlayTotalBet(bettype)", function () {
        beforeEach(function () {
            spyOn(QueryString, "setParam");
            spyOn(Site, "redirect");
        });
        it("QueryString.setParam(url, name, value) should not be called when length of bettype is 0", function () {
            RunningBetListMultipleBettype.switchMixParlayTotalBet("");
            expect(QueryString.setParam).not.toHaveBeenCalled();
        })

        it("QueryString.setParam(url, name, value) should be called when length of bettype greater than 0", function () {
            RunningBetListMultipleBettype.switchMixParlayTotalBet("abc");
            expect(QueryString.setParam).toHaveBeenCalled();
        });

        it("Site.redirect(url) should be called when length of bettype greater than 0", function () {
            RunningBetListMultipleBettype.switchMixParlayTotalBet("abc");
            expect(Site.redirect).toHaveBeenCalled();
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

        describe("switchItemSelectedMixParlay()", function () {
            describe("betType value is 9", function () {
                beforeEach(function () {
                    affix("#mixparlay_9");
                    affix("#mixparlay_29");

                    spyOn(RunningBetListMultipleBettype, "getUrlParams").and.returnValue("9");

                    RunningBetListMultipleBettype.switchItemSelectedMixParlay();
                });

                it("$('#mixparlay_9') should not have '.active-switch-link' class", function () {
                    expect($('#mixparlay_9')).not.toHaveClass("active-switch-link");
                });

                it("$('#mixparlay_9') should have '.active-switch-link' class", function () {
                    expect($('#mixparlay_9')).toHaveClass("inactive-switch-link");
                });

                it("$('#mixparlay_29') should not have '.inactive-switch-link' class", function () {
                    expect($('#mixparlay_29')).not.toHaveClass("inactive-switch-link");
                });

                it("$('#mixparlay_29') should have '.inactive-switch-link' class", function () {
                    expect($('#mixparlay_29')).toHaveClass("active-switch-link");
                });
            });

            describe("betType value not equal 9", function () {
                beforeEach(function () {
                    affix("#mixparlay_9");
                    affix("#mixparlay_29");

                    spyOn(RunningBetListMultipleBettype, "getUrlParams").and.returnValue("5");

                    RunningBetListMultipleBettype.switchItemSelectedMixParlay();
                });

                it("$('#mixparlay_9') should have '.active-switch-link' class", function () {
                    expect($('#mixparlay_9')).toHaveClass("active-switch-link");
                });

                it("$('#mixparlay_9') should not have '.active-switch-link' class", function () {
                    expect($('#mixparlay_9')).not.toHaveClass("inactive-switch-link");
                });

                it("$('#mixparlay_29') should have '.inactive-switch-link' class", function () {
                    expect($('#mixparlay_29')).toHaveClass("inactive-switch-link");
                });

                it("$('#mixparlay_29') should not have '.inactive-switch-link' class", function () {
                    expect($('#mixparlay_29')).not.toHaveClass("active-switch-link");
                });
            });
        });
    });
});