describe("BetList", function () {

    describe("getSortParams()", function () {
        it("should return sorting data stored in inputs", function () {
            affix('input#SortBy[value="Foo"]');
            affix('input#SortOrder[value="asc"]');
            BetList.initialize();
            var result = BetList.getSortParams();

            expect(result).toEqual({ SortBy: 'Foo', SortOrder: 'asc' });
        });
    });

    describe("loadData(params, url)", function () {
        var request;

        beforeEach(function (done) {
            affix('input#HiddenColumnList[value="1"]')
            jasmine.Ajax.install();

            jasmine.Ajax
                .stubRequest('localhost-test')
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ "Main": {}, "Total": {}, "IsShowTotal": true, "LeagueName": "Test league" }'
                });

            spyOn(BetList, 'bindData');
            spyOn(BetList, 'setLeagueTitle');

            BetList.loadData({}, 'localhost-test');
            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("should return sorting data stored in inputs", function () {
            expect(request.url).toBe('localhost-test');
        });

        it("BetList.bindData() should be called with expected param", function () {
            expect(BetList.bindData).toHaveBeenCalledWith({ Main: {}, Total: {}, IsShowTotal: true, LeagueName: "Test league" });
        });

        it("BetList.setLeagueTitle() should be called with expected param", function () {
            expect(BetList.setLeagueTitle).toHaveBeenCalledWith('Test league');
        });
    });

    describe("loadDataPaging(url)", function () {
        beforeEach(function () {
            affix('#HiddenColumnList');
            spyOn($.fn, 'jqGrid');
            BetList.loadDataPaging('');
        });

        it("should call .jqGrid('GridUnload')", function () {
            expect($.fn.jqGrid).toHaveBeenCalledWith('GridUnload');
        });

        it("should call .jqGrid() with a selector '#BetListGrid'", function () {
            expect($.fn.jqGrid.calls.mostRecent().object.selector).toBe('#BetListGrid');
        });
    });

    describe("bindData(url)", function () {
        
        it("gridOption.colNames should be a valid array", function () {
            var gridOption;
            affix('#HiddenColumnList');
            spyOn($.fn, 'jqGrid').and.callFake(function (param) { gridOption = param; })
            BetList.bindData({ Main: {}, Total: {}, IsShowTotal: true, LeagueName: "Test league" });
            expect(gridOption.colNames).toEqual(["#", "ExternalId", "SystemId", "TransTime", "Choice", "Odds", "Stake", "WinLoss", "EffectiveBettingValue", "LicenseeWL/Comm", "Status"]);
        });
    });

    describe("getMixParlayResultUrl(refno, winlostdate, bettype)", function () {
        it("should return an url with correct parameters", function () {
            var result = BetList.getMixParlayResultUrl(1, 2, 3);
            expect(result).toBe('Result/ResultPopup?refno=1&winlossdate=2&bettype=3&matchid=1');
        });
    });

    describe("getSportBookResultUrl(matchid, bettype, raceNo, sporttype, league, isOutright, betid)", function () {
        it("should return an url with correct parameters", function () {
            var result = BetList.getSportBookResultUrl(1, 2, 3, 4, 5, 6, 7);
            expect(result).toBe('Result/ResultPopup?matchid=1&bettype=2&raceno=3&sporttype=4&isoutright=6&betid=7');
        });
    });

    describe("getMicrogamingResultUrl(gameid, custid)", function () {
        it("should return an url with correct parameters", function () {
            var result = BetList.getMicrogamingResultUrl(1, 2);
            expect(result).toBe('Result/MicrogamingResultPopup?gameid=1&custid=2');
        });
    });

    describe("setLeagueTitle(leagueName)", function () {
        beforeEach(function () {
            affix('.league-name-title');
        });

        it("'.league-name-title' should contain resource 'BetList' as default", function () {
            BetList.setLeagueTitle();
            var result = BetList.getMicrogamingResultUrl(1, 2);
            expect($('.league-name-title').html()).toBe('BetList - ');
        });

        it("'.league-name-title' should contain the text passed as paramter", function () {
            BetList.setLeagueTitle('Hello');
            var result = BetList.getMicrogamingResultUrl(1, 2);
            expect($('.league-name-title').html()).toBe('Hello - ');
        });
    });

    describe("showMicrogamingResultPopup(title, gameid, custid)", function () {
        var $dialogContainer;
        beforeEach(function () {
            spyOn($.fn, 'dialog');
            BetList.showMicrogamingResultPopup('Hello', 1, 2);
            $dialogContainer = $.fn.dialog.calls.mostRecent().object;
        });

        it(".dialog() should be called with correct parameters", function () {
            expect($.fn.dialog).toHaveBeenCalledWith({ modal: true, width: 750, height: 400 });
        });

        it("the dialog container should has attribute 'title' = 'Hello'", function () {
            expect($dialogContainer.attr('title')).toBe('Hello');
        });

        it("the dialog container should contain an iframe with correct src attribute", function () {
            var $iframe = $dialogContainer.find('iframe');
            expect($iframe.attr('src')).toBe('Result/MicrogamingResultPopup?gameid=1&custid=2');
        });
    });

    describe("showResult(matchid, raceNo, bettype, sporttype, refno, username, winlostdate, refnoMixParlay, league, isOutright, betid, betcheck, custid)", function () {
        beforeEach(function () {
            spyOn($.fn, 'dialog');
        });

        it("BetList.showMicrogamingResultPopup() should be called if sporttype is Microgaming", function () {
            var SPORT_MICROGAMMING = 203;
            spyOn(BetList, 'showMicrogamingResultPopup');
            BetList.showResult(null, null, null, SPORT_MICROGAMMING);

            expect(BetList.showMicrogamingResultPopup).toHaveBeenCalled();
        });

        it("BetList.getMixParlayResultUrl() should be called if bettype is MixParlay", function () {
            var BETTYPE_MIXPARLAY = 9;
            spyOn(BetList, 'getMixParlayResultUrl');
            BetList.showResult(null, null, BETTYPE_MIXPARLAY);

            expect(BetList.getMixParlayResultUrl).toHaveBeenCalled();
        });

        it("BetList.getSportBookResultUrl() should be called if bettype is NOT MixParlay", function () {
            spyOn(BetList, 'getSportBookResultUrl');
            BetList.showResult(null, null);

            expect(BetList.getSportBookResultUrl).toHaveBeenCalled();
        });

        it("$.fn.dialog() should be called if sporttype is NOT Microgaming", function () {
            spyOn(BetList, 'getSportBookResultUrl');
            BetList.showResult(null, null);

            expect($.fn.dialog).toHaveBeenCalled();
        });
    });

    describe("showMixParlayDetail(transId, divEvent)", function () {

        beforeEach(function () {
            spyOn(BetList, 'resizeGrid');
        });

        describe("'divEvent' element is visible", function () {
            var $div;
            beforeEach(function () {
                $div = affix('div div');
                BetList.showMixParlayDetail(0, $div);
            });

            it("'divEvent' element should be hide", function () {                                
                expect($div.is(':visible')).toBe(false);
            });

            it("'divEvent' element shoult not contain any child.", function () {
                expect($div.html()).toBe('');
            });

            it("BetList.resizeGrid() should be called.", function () {
                expect(BetList.resizeGrid).toHaveBeenCalled();
            });
        });

        describe("'divEvent' element is hidden", function () {
            var $div, request, onSuccess;
            beforeEach(function (done) {
                $div = affix('div[style="display:none"]');
                jasmine.Ajax.install();
                onSuccess = jasmine.createSpy('onSuccess');

                BetList.showMixParlayDetail(0, $div);
                request = jasmine.Ajax.requests.mostRecent();
                request.respondWith({
                    status: 200,
                    responseText: '[{ "Choice": 1 }, { "Choice": 2 }, { "Choice": 3 }]'
                });

                setTimeout(function () {
                    done();
                }, 100);
            });

            afterEach(function () {
                jasmine.Ajax.uninstall();
            });

            it("request url should be correct", function () {
                expect(request.url).toMatch(/api\/betlist\/MixParlayDetail\?transId=0/);
            });

            it("'divEvent' element should be visible", function () {
                expect($div.is(':visible')).toBe(true);
            });

            it("'divEvent' element should contain correct html", function () {
                expect($div.html()).toBe('1<div class="line"></div>2<div class="line"></div>3');
            });

            it("BetList.resizeGrid() should be called.", function () {
                expect(BetList.resizeGrid).toHaveBeenCalled();
            });
        });
    });

    describe("showSystemParlayDetail(transId, refNo, betId, custId, betTeam, baseCurrency)", function () {
        var $dialogContainer;
        beforeEach(function () {
            spyOn($.fn, 'dialog');
            BetList.showSystemParlayDetail(1, 0, 0, 0, 'Test BetTeam', 0);
            $dialogContainer = $.fn.dialog.calls.mostRecent().object;
        });

        it(".dialog() should be called with correct parameters", function () {
            expect($.fn.dialog).toHaveBeenCalledWith({ modal: true, width: 775, height: 500 });
        });

        it("the dialog container should has attribute 'title' = 'Test BetTeam'", function () {
            expect($dialogContainer.attr('title')).toBe('Test BetTeam');
        });

        it("the dialog container should contain an iframe with correct src attribute", function () {
            var $iframe = $dialogContainer.find('iframe');
            expect($iframe.attr('src')).toBe('BetList/SystemParlayDetail?transid=1&refno=0&betid=0&custid=0&betteam=Test BetTeam&basecurrency=0');
        });
    });

    describe("showMicroGamingRnGBetsDetails(transId, winlostDate, custId, transType)", function () {
        var $dialogContainer;
        beforeEach(function () {
            spyOn($.fn, 'dialog');
            BetList.showMicroGamingRnGBetsDetails(0, new Date('2015/2/1'), 0, 0);
            $dialogContainer = $.fn.dialog.calls.mostRecent().object;
        });

        it(".dialog() should be called", function () {
            expect($.fn.dialog).toHaveBeenCalled();
        });

        it("the dialog container should has attribute 'title' = 'Microgaming - DetailedBetlist - 02/01/2015'", function () {
            expect($dialogContainer.attr('title')).toBe('Microgaming - DetailedBetlist - 02/01/2015');
        });

        it("the dialog container should contain an iframe with correct src attribute", function () {
            var $iframe = $dialogContainer.find('iframe');
            expect($iframe.attr('src')).toBe('BetList/MicroGamingRngDetail?transid=0&custid=0&transtype=0');
        });
    });

    describe("showAGCasinoDetails(transId, winlostDate, custId, transType)", function () {
        var $dialogContainer;
        beforeEach(function () {
            spyOn($.fn, 'dialog');
            BetList.showAGCasinoDetails(0, new Date('2015/2/1'), 0, 0);
            $dialogContainer = $.fn.dialog.calls.mostRecent().object;
        });

        it(".dialog() should be called", function () {
            expect($.fn.dialog).toHaveBeenCalled();
        });

        it("the dialog container should has attribute 'title' = 'AGCasino - DetailedBetlist - 02/01/2015'", function () {
            expect($dialogContainer.attr('title')).toBe('AGCasino - DetailedBetlist - 02/01/2015');
        });

        it("the dialog container should contain an iframe with correct src attribute", function () {
            var $iframe = $dialogContainer.find('iframe');
            expect($iframe.attr('src')).toBe('BetList/AGCasinoDetail?transid=0&custid=0&transtype=0');
        });
    });

    describe("ValidProducts()", function () {
        beforeEach(function () {
            affix('#ProductList');
            spyOn($.fn, 'dialog');
        });

        it("should return false if #ProductList does not contain checked input", function () {            
            expect(BetList.ValidProducts()).toBe(false)
        });

        it("$.fn.dialog() should be called if #ProductList does not contain checked input", function () {
            BetList.ValidProducts();
            expect($.fn.dialog).toHaveBeenCalled();
        });

        it("$.fn.dialog() should be called with correct html element if #ProductList does not contain checked input", function () {
            BetList.ValidProducts();
            var $dialogContainer = $.fn.dialog.calls.mostRecent().object;
            expect($dialogContainer.prop('outerHTML')).toBe('<div title="LrfNotification">AlertChooseProductMsg</div>');
        });

        it("should return true if #ProductList contain a checked input", function () {
            $('#ProductList').affix('input[type="checkbox"]').prop('checked', true);
            expect(BetList.ValidProducts()).toBe(true)
        });
    });
});