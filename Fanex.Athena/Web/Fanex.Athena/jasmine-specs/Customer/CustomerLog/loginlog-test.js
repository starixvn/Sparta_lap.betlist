describe("LogInLog", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            affix("#Username");
            affix("#loginlog");
            spyOn(CustomerLog, "initAutoComplete");
            spyOn(LogInLog, "initEvent");
            spyOn(Site, "addUserGuide");

            LogInLog.initialize();
        });

        it("CustomerLog.initAutoComplete() should be called", function () {
            expect(CustomerLog.initAutoComplete).toHaveBeenCalled();
        });
        it("LogInLog.initEvent() should be called", function () {
            expect(LogInLog.initEvent).toHaveBeenCalled();
        });
        it("Site.addUserGuide(options) should be called with correct parameters", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "CustomerLog", title: "CustomerLog" });
        });
        it("'#Username' should has correct value", function () {
            expect($("#Username").val()).toBe("ExternalIdOrSystemId");
        });
        it("'#loginlog' should has class named 'activeTab'", function () {
            expect($("#loginlog")).toHaveClass("activeTab");
        });
    });

    describe("initEvent()", function () {
        beforeEach(function () {
            affix("#Username");
            affix("#SubmitButton");

            LogInLog.initEvent();
        });

        it("CustomerLog.setUsernameOnFocus() should be called when '#Username' fired click event", function () {
            spyOn(CustomerLog, "setUsernameOnFocus");

            $("#Username").click();

            expect(CustomerLog.setUsernameOnFocus).toHaveBeenCalled();
        });

        it("CustomerLog.setDefaultUsername() should be called when '#Username' fired blur event", function () {
            spyOn(CustomerLog, "setDefaultUsername");

            $("#Username").blur();

            expect(CustomerLog.setDefaultUsername).toHaveBeenCalled();
        });

        describe("'#SubmitButton' click event", function () {
            it("Site.blockUI() should be called when '#SubmitButton' fired click event", function () {
                spyOn(Site, "blockUI");

                $("#SubmitButton").click();

                expect(Site.blockUI).toHaveBeenCalled();
            });

            it("LogInLog.loadGrid(isShowAll) should be called when '#SubmitButton' fired click event", function () {
                spyOn(LogInLog, "loadGrid");

                $("#SubmitButton").click();

                expect(LogInLog.loadGrid).toHaveBeenCalled();
            });
        });
    });

    describe("loadGrid()", function () {
        beforeEach(function () {
            affix("#LogInLogGrid");

            spyOn($.fn, "jqGrid");

            LogInLog.loadGrid();
        });

        it("jqGrid function of jquery should be called with correct parameter", function () {
            expect($.fn.jqGrid).toHaveBeenCalledWith("GridUnload");
        });

        it("jqGrid function of jquery should be called two times", function () {
            expect($.fn.jqGrid.calls.count()).toBe(2);
        });
    });

    describe("initParam()", function () {
        it("Value should be returned correctly", function () {
            affix("input#Username[value=user01]");

            var expectedResult = "api/Log/LoginLog?UserName=user01";

            expect(LogInLog.initParam()).toBe(expectedResult);
        });
    });
});