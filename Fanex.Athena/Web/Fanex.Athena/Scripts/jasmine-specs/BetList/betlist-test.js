describe("BetList", function () {

    beforeEach(function () {
        //spyOn($.fn, "on").and.callFake(function () {

        //});

        //affix('#btnSave');
        //ResetPassword.init();
    });

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

            setTimeout(function () {
                done();
            }, 100);
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
        var gridOption;
        beforeEach(function () {
            affix('#HiddenColumnList');
            spyOn($.fn, 'jqGrid').and.callFake(function (param) { gridOption = param; })
            BetList.bindData({ Main: {}, Total: {}, IsShowTotal: true, LeagueName: "Test league" });
        });

        it("gridOption.rowNum should be a 50", function () {
            expect(gridOption.rowNum).toBe(50);
        });

        it("gridOption.rowList should be a valid array", function () {
            expect(JSON.stringify(gridOption.rowList)).toEqual(JSON.stringify([50, 100, 200, 300, 400, 500, 1000, 2000]));
        });

        it("gridOption.colNames should be a valid array", function () {
            expect(gridOption.colNames).toEqual(["#", "ExternalId", "SystemId", "TransTime", "Choice", "Odds", "Stake", "WinLoss", "EffectiveBettingValue", "LicenseeWL/Comm", "Status"]);
        });
    });

    describe("getMixParlayResultUrl(refno, winlostdate, bettype)", function () {
        it("should return an url with correct parameters", function () {
            var result = BetList.getMixParlayResultUrl(1, 2, 3);
            expect(result).toBe('Report/ResultPopup?refno=1&winlossdate=2&bettype=3&matchid=1');
        });
    });

    describe("getSportBookResultUrl(matchid, bettype, raceNo, sporttype, league, isOutright, betid)", function () {
        it("should return an url with correct parameters", function () {
            var result = BetList.getSportBookResultUrl(1, 2, 3, 4, 5, 6, 7);
            expect(result).toBe('Report/ResultPopup?matchid=1&bettype=2&raceno=3&sporttype=4&isoutright=6&betid=7');
        });
    });

    describe("getMicrogamingResultUrl(gameid, custid)", function () {
        it("should return an url with correct parameters", function () {
            var result = BetList.getMicrogamingResultUrl(1, 2);
            expect(result).toBe('Report/MicrogamingResultPopup?gameid=1&custid=2');
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
            expect($.fn.dialog).toHaveBeenCalledWith({ modal: true, width: 750, height: 500 });
        });

        it("the dialog container should has attribute 'title' = 'Hello'", function () {
            expect($dialogContainer.attr('title')).toBe('Hello');
        });

        it("the dialog container should contain an iframe with correct src attribute", function () {
            var $iframe = $dialogContainer.find('iframe');
            expect($iframe.attr('src')).toBe('Report/MicrogamingResultPopup?gameid=1&custid=2');
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

    describe("showMixParlayDetail(params, url)", function () {
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

            setTimeout(function () {
                done();
            }, 100);
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
});