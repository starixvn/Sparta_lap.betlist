describe("GroupCommLog", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            affix("#groupcommlog");
            affix("input#Username");

            spyOn(GroupCommLog, "initMonthPicker");
            spyOn(CustomerLog, "initAutoComplete");
            spyOn(GroupCommLog, "initEvent");
            spyOn(Site, "addUserGuide");

            GroupCommLog.initialize();
        });

        it("GroupCommLog.initMonthPicker() should be called", function () {
            expect(GroupCommLog.initMonthPicker).toHaveBeenCalled();
        });
        it("CustomerLog.initAutoComplete() should be called", function () {
            expect(CustomerLog.initAutoComplete).toHaveBeenCalled();
        });
        it("GroupCommLog.initEvent() should be called", function () {
            expect(GroupCommLog.initEvent).toHaveBeenCalled();
        });

        it("'#groupcommlog' should has class named 'activeTab'", function () {
            expect($("#groupcommlog")).toHaveClass("activeTab");
        });

        it("'#Username' should has value 'ExternalIdOrSystemId'", function () {
            expect($("#Username").val()).toBe("ExternalIdOrSystemId");
        });

        it("Site.addUserGuide() should be called with correct parameters", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "CustomerLog", title: "CustomerLog" });
        });
    });

    describe("initMonthPicker()", function () {
        it("monPickr function of jquery should be called", function () {
            affix("input#DateControl");

            spyOn($.fn, "monPickr");

            GroupCommLog.initMonthPicker();

            expect($.fn.monPickr).toHaveBeenCalled();
        });
    });

    describe("initEvent()", function () {
        beforeEach(function () {
            affix("#Username");
            affix("#SubmitButton");
            affix("input#showall[type=checkbox]");

            GroupCommLog.initEvent();
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
            beforeEach(function () {
                affix("input#showall[type=checkbox]");
                spyOn(CustomerLog, "dateConstraint").and.returnValue(true);
            });

            it("CustomerLog.dateConstraint() should be called when '#SubmitButton' fired click event", function () {
                $("#SubmitButton").click();

                expect(CustomerLog.dateConstraint).toHaveBeenCalled();
            });

            it("Site.blockUI() should be called when '#SubmitButton' fired click event", function () {
                spyOn(Site, "blockUI");

                $("#SubmitButton").click();

                expect(Site.blockUI).toHaveBeenCalled();
            });

            it("GroupCommLog.loadGrid(isShowAll) should be called with correct parameter when '#SubmitButton' fired click event", function () {
                spyOn(GroupCommLog, "loadGrid");

                $("#SubmitButton").click();

                expect(GroupCommLog.loadGrid).toHaveBeenCalledWith(false);
            });
        });

        describe("'#showall' change event", function () {
            beforeEach(function () {
                affix("input#showall[type=checkbox]");
                affix(".old-value");
                $(".old-value").css({ width: "10px", height: "20px", background: "yellow" });
            });

            it("'.old-value' should be hidden when '#showall' is not checked", function () {
                $("#showall").change();

                expect($(".old-value")).toBeHidden();
            });

            it("'.old-value' should be showed when '#showall' is checked", function () {
                $("#showall").prop("checked", true);

                $("#showall").change();

                expect($(".old-value")).not.toBeHidden();
            });
        });
    });

    describe("loadGrid(isShowAll)", function () {
        beforeEach(function () {
            affix("#GroupCommLogGrid");

            spyOn($.fn, "jqGrid");

            GroupCommLog.loadGrid(true);
        });

        it("jqGrid function of jquery should be called with correct parameter", function () {
            expect($.fn.jqGrid).toHaveBeenCalledWith("GridUnload");
        });

        it("jqGrid function of jquery should be called two times", function () {
            expect($.fn.jqGrid.calls.count()).toBe(2);
        });
    });

    describe("initParam()", function () {
        beforeEach(function () {
            affix("input#FromDay[value=2]");
            affix("input#ToDay[value=30]");
            affix("input#Year[value=2016]");
            affix("input#Month[value=8]");
        });

        it("Value should be returned correctly", function () {
            var expectedResult = "api/Log/GroupCommission?Year=2016&Month=8&FromDay=2&ToDay=30";

            expect(GroupCommLog.initParam()).toBe(expectedResult);
        });
    });
});