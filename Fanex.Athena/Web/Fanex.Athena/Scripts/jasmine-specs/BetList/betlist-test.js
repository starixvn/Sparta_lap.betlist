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

            expect(result).toEqual({ SortBy: 'Foo', SortOrder: 'asc'});
        });
    });

    describe("loadData(params, url)", function () {
        var request;

        beforeEach(function () {
            affix('input#HiddenColumnList[value="1"]')
            jasmine.Ajax.install();

            jasmine.Ajax
                .stubRequest('localhost-test')
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ "Main": {}, "Total": {}, "IsShowTotal": true, "LeagueName": "Test league" }'
                });
            spyOn(BetList, 'bindData').and.callFake(function (response) { });
            spyOn(BetList, 'setLeagueTitle').and.callFake(function () { });
            
            BetList.loadData({}, 'localhost-test');
            request = jasmine.Ajax.requests.mostRecent();
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("should return sorting data stored in inputs", function (done) {            
            setTimeout(function () {
                expect(request.url).toBe('localhost-test');
                done();
            }, 100);
        });

        it("BetList.bindData() should be called with expected param", function (done) {
            setTimeout(function () {
                expect(BetList.bindData).toHaveBeenCalledWith({ Main: {}, Total: {}, IsShowTotal: true, LeagueName: "Test league" });
                done();
            }, 100);
        });

        it("BetList.setLeagueTitle() should be called with expected param", function (done) {
            setTimeout(function () {
                expect(BetList.setLeagueTitle).toHaveBeenCalledWith('Test league');
                done();
            }, 100);
        });
    });

    describe("loadDataPaging(url)", function () {
        it("should return sorting data stored in inputs", function () {
            affix('#HiddenColumnList');
            spyOn($.fn, 'jqGrid');
            BetList.loadDataPaging('');
            expect($("#BetListGrid").jqGrid).toHaveBeenCalledWith('GridUnload');
        });
    });
});
