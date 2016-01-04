describe("MonitorBetList", function () {
    beforeEach(function () {
        monitor = new MonitorBetList();
    });

    describe("initialize()", function () {
        beforeEach(function () {
            spyOn(Site, "addUserGuide");
            spyOn(monitor, "updateParams");
            spyOn(Site, "clone");

            monitor.initialize();
        });

        it("Site.addUserGuide() should be called with correct parameters", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "LastBetsMonitoring", title: "LastBetsMonitoring" });
        });

        it("MonitorBetList.updateParams() should be called", function () {
            expect(monitor.updateParams).toHaveBeenCalled();
        });

        it("Site.clone() should be called", function () {
            expect(Site.clone).toHaveBeenCalled();
        });
    });

    describe("numberParam(name, value)", function () {
        beforeEach(function () {
            monitor.testNumberValue = null;
        });

        it("MonitorBetList.testNumberValue should be 0 when 'value' param is undefined", function () {
            monitor.numberParam('testNumberValue');
            expect(monitor.testNumberValue).toBe(0);
        });

        it("MonitorBetList.testNumberValue should be 0 when 'value' param is null", function () {
            monitor.numberParam('testNumberValue', null);
            expect(monitor.testNumberValue).toBe(0);
        });

        it("MonitorBetList.testNumberValue should be 0 when 'value' param is empty", function () {
            monitor.numberParam('testNumberValue', '');
            expect(monitor.testNumberValue).toBe(0);
        });

        it("MonitorBetList.testNumberValue should be 1 when 'value' param is 1", function () {
            monitor.numberParam('testNumberValue', 1);
            expect(monitor.testNumberValue).toBe(1);
        });
    });

    describe("stringParam(name, value)", function () {
        var emptyString = "";
        beforeEach(function () {
            monitor.testStringValue = null;
        });

        it("MonitorBetList.testStringValue should be empty when 'value' param is undefined", function () {
            monitor.stringParam('testStringValue');
            expect(monitor.testStringValue).toBe(emptyString);
        });

        it("MonitorBetList.testStringValue should be empty when 'value' param is null", function () {
            monitor.stringParam('testStringValue', null);
            expect(monitor.testStringValue).toBe(emptyString);
        });

        it("MonitorBetList.testStringValue should be empty when 'value' param is empty", function () {
            monitor.stringParam('testStringValue', '');
            expect(monitor.testStringValue).toBe(emptyString);
        });

        it("MonitorBetList.testStringValue should be 'abc' when 'value' param is 'abc'", function () {
            monitor.stringParam('testStringValue', 'abc');
            expect(monitor.testStringValue).toBe('abc');
        });

        it("MonitorBetList.testStringValue should be 'abc' when 'value' param is '  abc   '", function () {
            monitor.stringParam('testStringValue', '  abc   ');
            expect(monitor.testStringValue).toBe('abc');
        });
    });

    describe("tryUpdateStack()", function () {
        beforeEach(function () {
            monitor.maxStake = 9999;
        });

        describe("$('#stake-from').val() and $('#stake-to').val() is empty", function () {
            beforeEach(function () {
                affix("input#stake-from");
                affix("input#stake-to");
                monitor.tryUpdateStack();
            });

            it("$this.params.StakeFrom should be 0", function () {
                expect(monitor.params.StakeFrom).toBe(0);
            });

            it("$this.params.StakeFrom should be equal to monitor.maxStake", function () {
                expect(monitor.params.StakeTo).toBe(monitor.maxStake);
            });
        });

        describe("$('#stake-from').val() is 1 and $('#stake-to').val() is 2", function () {
            beforeEach(function () {
                affix("input#stake-from[value=1]");
                affix("input#stake-to[value=2]");
                monitor.tryUpdateStack();
            });

            it("$this.params.StakeFrom should be 1", function () {
                expect(monitor.params.StakeFrom).toBe(1);
            });

            it("$this.params.StakeFrom should be 2", function () {
                expect(monitor.params.StakeTo).toBe(2);
            });
        });
    });

    describe("Submit(freeStatus)", function () {
        beforeEach(function () {
            spyOn(monitor, "tryUpdateStack");
            spyOn(monitor, "startMonitor");
            spyOn(monitor, "stopMonitor");
            spyOn(Site, "clone");
        });
        it("MonitorBetList.startMonitor should be called with freeStatus is 1", function () {
            monitor.Submit(1);
            expect(monitor.startMonitor.calls.count()).toBe(1);
        });

        it("MonitorBetList.stopMonitor should be called once with freeStatus is 1", function () {
            monitor.Submit(1);
            expect(monitor.stopMonitor).toHaveBeenCalled();
        });

        it("MonitorBetList.tryUpdateStack should be called", function () {
            monitor.Submit(1);
            expect(monitor.tryUpdateStack).toHaveBeenCalled();
        });

        it("Site.clone should be called", function () {
            monitor.Submit(1);
            expect(Site.clone).toHaveBeenCalled();
        });

        it("MonitorBetList.stopMonitor should be called twice with freeStatus is 0", function () {
            monitor.Submit(0);
            expect(monitor.stopMonitor.calls.count()).toBe(2);
        });
    });

    describe("reloadData()", function () {
        beforeEach(function () {
            affix("#div-loading");
            affix(".no-info-row");
        });

        describe("ajax not be called yet", function () {
            it("MonitorBetList.doMonitor() should be called when 'monitor.isLoading' is true", function () {
                monitor.isLoading = true;
                spyOn(monitor, "doMonitor");
                monitor.reloadData();
                expect(monitor.doMonitor).toHaveBeenCalled();
            });

            it("$.ajax() should not be called when 'isValidParam()' function return false", function () {
                spyOn(monitor, "isValidParam").and.returnValue(false);
                spyOn($, "ajax");
                monitor.reloadData();
                expect($.ajax).not.toHaveBeenCalled();
            });
        });

        describe("ajax has been called", function () {
            var request;
            beforeEach(function (done) {
                jasmine.Ajax.install();
                jasmine.Ajax.stubRequest('api/betlist/LastMonitorBetListNew')
                        .andReturn({
                            status: 200,
                            contentType: 'application/json',
                            responseText: '{}'
                        });

                spyOn(monitor, "isValidParam").and.returnValue(true);
                spyOn(monitor, "bindNoInfoRow");

                spyOn(monitor, "bindData");

                monitor.reloadData();
                request = jasmine.Ajax.requests.mostRecent();
                setTimeout(done, 10);
            });
            afterEach(function () {
                jasmine.Ajax.uninstall();
            });

            it("url of ajax should be 'api/betlist/LastMonitorBetListNew'", function () {
                expect(request.url).toEqual("api/betlist/LastMonitorBetListNew");
            });

            it("MonitorBetList.bindData() should be called", function () {
                expect(monitor.bindData).toHaveBeenCalled();
            });
        });
    });

    describe("clearContent()", function () {
        beforeEach(function () {
            spyOn(monitor, "bindNoInfoRow");

            monitor.clearContent();
        });

        it("MonitorBetList.lastTransId should be 0", function () {
            expect(monitor.lastTransId).toBe(0);
        });

        it("MonitorBetList.bindNoInfoRow should be called", function () {
            expect(monitor.bindNoInfoRow).toHaveBeenCalled();
        });
    });

    describe("startMonitor()", function () {
        beforeEach(function () {
            spyOn(monitor, "stopMonitor");
            spyOn(monitor, "doMonitor");

            monitor.startMonitor();
        });

        it("MonitorBetList.lastTransId should be 0", function () {
            expect(monitor.lastTransId).toBe(0);
        });

        it("MonitorBetList.stopMonitor should be called", function () {
            expect(monitor.stopMonitor).toHaveBeenCalled();
        });

        it("MonitorBetList.doMonitor should be called", function () {
            expect(monitor.doMonitor).toHaveBeenCalled();
        });
    });
});