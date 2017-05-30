describe("LastBetGrid", function () {
    beforeEach(function () {
        LastBetGrid.initialize();
    });
    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(Site, "addUserGuide");
            spyOn(Site, "clone");

            LastBetGrid.initialize();
        });

        it("Site.addUserGuide() should be called with correct parameters", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "LastBetsMonitoring", title: "LastBetsMonitoring" });
        });
    });

    describe("Submit(freezeStatus)", function () {
        beforeEach(function () {
            spyOn(LastBetGrid, "tryUpdateStake");
            spyOn(LastBetGrid, "startMonitor");
            spyOn(LastBetGrid, "stopMonitor");
        });
        it("LastBetGrid.startMonitor() should be called with freeStatus is 1", function () {
            LastBetGrid.Submit(1);
            expect(LastBetGrid.startMonitor.calls.count()).toBe(1);
        });

        it("LastBetGrid.stopMonitor() should be called once with freeStatus is 1", function () {
            LastBetGrid.Submit(1);
            expect(LastBetGrid.stopMonitor).toHaveBeenCalled();
        });

        it("LastBetGrid.tryUpdateStake() should be called", function () {
            LastBetGrid.Submit(1);
            expect(LastBetGrid.tryUpdateStake).toHaveBeenCalled();
        });

        it("LastBetGrid.stopMonitor() should be called once with freeStatus is 0", function () {
            LastBetGrid.Submit(0);
            expect(LastBetGrid.stopMonitor.calls.count()).toBe(1);
        });
    });

    describe("reloadData()", function () {
        beforeEach(function () {
            affix("#div-loading");
            affix(".no-info-row");
        });

        describe("ajax not be called yet", function () {
            it("$.ajax() should not be called when 'isValidParam()' function return false", function () {
                spyOn(LastBetGrid, "isValidParam").and.returnValue(false);
                spyOn($, "ajax");
                LastBetGrid.reloadData();
                expect($.ajax).not.toHaveBeenCalled();
            });
        });

        describe("ajax has been called", function () {
            var request;
            beforeEach(function (done) {
                jasmine.Ajax.install();
                jasmine.Ajax.stubRequest('api/LastBetMonitoringBetList/Index')
                        .andReturn({
                            status: 200,
                            contentType: 'application/json',
                            responseText: '{}'
                        });

                spyOn(LastBetGrid, "isValidParam").and.returnValue(true);
                spyOn(LastBetGrid, "bindNoInfoRow");

                spyOn(LastBetGrid, "bindData");

                LastBetGrid.reloadData();
                request = jasmine.Ajax.requests.mostRecent();
                setTimeout(done, 10);
            });
            afterEach(function () {
                jasmine.Ajax.uninstall();
            });

            it("url of ajax should be 'api/LastBetMonitoringBetList/Index'", function () {
                expect(request.url).toEqual("api/LastBetMonitoringBetList/Index");
            });

            it("LastBetGrid.bindData() should be called", function () {
                expect(LastBetGrid.bindData).toHaveBeenCalled();
            });
        });
    });

    describe("clearContent()", function () {
        beforeEach(function () {
            spyOn(LastBetGrid, "bindNoInfoRow");

            LastBetGrid.clearContent();
        });

        it("LastBetGrid.bindNoInfoRow() should be called", function () {
            expect(LastBetGrid.bindNoInfoRow).toHaveBeenCalled();
        });
    });

    describe("startMonitor()", function () {
        beforeEach(function () {
            spyOn(LastBetGrid, "stopMonitor");
            spyOn(LastBetGrid, "doMonitor");

            LastBetGrid.startMonitor();
        });

        it("LastBetGrid.stopMonitor() should be called", function () {
            expect(LastBetGrid.stopMonitor).toHaveBeenCalled();
        });

        it("LastBetGrid.doMonitor() should be called", function () {
            expect(LastBetGrid.doMonitor).toHaveBeenCalled();
        });
    });
});
