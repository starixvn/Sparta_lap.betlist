describe("GroupComm", function () {
    describe("initialize()", function () {
        it("GroupComm.initEvent() should be called", function () {
            spyOn(GroupComm, "initEvent");

            GroupComm.initialize();

            expect(GroupComm.initEvent).toHaveBeenCalled();
        });

        it("Site.addUserGuide(options) should be called", function () {
            spyOn(Site, "addUserGuide");

            GroupComm.initialize();

            expect(Site.addUserGuide).toHaveBeenCalled();
        });
    });

    describe("initEvent()", function () {
        beforeEach(function () {
            affix("#ExportToExcel");
            affix("input#IsExportExcel");
            affix("#CommissionGroupForm");

            GroupComm.initEvent();
        });

        it("submit function of jquery should be called when '#ExportToExcel' fired click event", function () {
            spyOn($.fn, "submit");

            $("#ExportToExcel").click();

            expect($.fn.submit).toHaveBeenCalled();
        });
    });

    describe("commissionOnChange(value, id)", function () {
        beforeEach(function () {
            affix("input#comm2");
            affix("input#comm3");
        });

        it("'#comm2, #comm3' should be disabled when value equal 15", function () {
            GroupComm.commissionOnChange(15, "#comm");

            expect($("#comm2")).toBeDisabled();
            expect($("#comm3")).toBeDisabled();
        });

        it("'#comm2, #comm3' should have value equal 0 when value passed equal 15", function () {
            GroupComm.commissionOnChange(15, "#comm");

            expect($("#comm2").val()).toBe("0");
            expect($("#comm3").val()).toBe("0");
        });

        it("'#comm2, #comm3' should be enabled when value not equal 15", function () {
            GroupComm.commissionOnChange(12, "#comm");

            expect($("#comm2")).not.toBeDisabled();
            expect($("#comm3")).not.toBeDisabled();
        });
    });

    describe("initDeleteGroup(groupId)", function () {
        var request;
        beforeEach(function (done) {
            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("/CustomerComGroup\/GetDefaultGroup/")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '1'
                });

            GroupComm.initDeleteGroup(1);

            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with url is 'CustomerComGroup/GetDefaultGroup'", function () {
            expect(request.url).toContain("CustomerComGroup/GetDefaultGroup");
        });
    });

    describe("deleteGroup(groupId, isDefault)", function () {
        it("GroupComm.showConfirmAction(message, callback, id, groupId, isResetDefault) should be called when 'isDefault' value equal true", function () {
            spyOn(GroupComm, "showConfirmAction");

            GroupComm.deleteGroup(1, true);

            expect(GroupComm.showConfirmAction).toHaveBeenCalled();
        });

        it("dialog function of jquery should be called when 'isDefault' value equal false", function () {
            spyOn($.fn, "dialog");

            GroupComm.deleteGroup(1, false);

            expect($.fn.dialog).toHaveBeenCalled();
        });
    });

    describe("processDelete(groupId, isRemovePlayerComm, isResetDefault)", function () {
        var request;
        beforeEach(function (done) {
            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("CustomerComGroup/Delete")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ErrorCode : 1}'
                });

            GroupComm.processDelete(1, true, true);

            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with url is 'CustomerComGroup/Delete'", function () {
            expect(request.url).toBe("CustomerComGroup/Delete");
        });
    });

    describe("doDelete(groupId, isResetDefault)", function () {
        it("dialog function of jquery should be called", function () {
            spyOn($.fn, "dialog");

            GroupComm.doDelete(1, true);

            expect($.fn.dialog).toHaveBeenCalled();
        });
    });

    describe("initDefaultParam(id)", function () {
        beforeEach(function () {
            affix("input#IsDefaultGroup[type=checkbox]");
            $("#IsDefaultGroup").prop("checked", true);

            affix("input#DefaultGroupId[value=2]");
            affix("input#GroupCommId[value=4]");
        });

        it("GroupComm.showConfirmAction(message, callback, id, groupId, isResetDefault) should be called when first condition is satisfied", function () {
            spyOn(GroupComm, "showConfirmAction");

            GroupComm.initDefaultParam(1);

            expect(GroupComm.showConfirmAction).toHaveBeenCalled();
        });

        it("submit function of jquery should be called when another condition is satisfied", function () {
            $("#IsDefaultGroup").prop("checked", false);
            spyOn($.fn, "submit");

            GroupComm.initDefaultParam(1);

            expect($.fn.submit).toHaveBeenCalled();
        });
    });

    describe("showConfirmAction(message, callback, id, groupId, isResetDefault)", function () {
        it("dialog function of jquery should be called", function () {
            spyOn($.fn, "dialog");

            GroupComm.showConfirmAction("testing message", function () { }, "#Id", 12, true);

            expect($.fn.dialog).toHaveBeenCalled();
        });
    });

    describe("doSubmit()", function () {
        beforeEach(function () {
            affix("input#IsResetDefault[value=false]");
            affix("form");

            spyOn($.fn, "submit");
        });

        it("'#IsResetDefault' should has value equal true", function () {
            GroupComm.doSubmit();

            expect($("#IsResetDefault").val()).toBe("true");
        });

        it("submit function of jquery should be called", function () {
            GroupComm.doSubmit();

            expect($.fn.submit).toHaveBeenCalled();
        });
    });

    describe("initCommParam()", function () {
        beforeEach(function () {
            affix("input#DdlSBComm1[value=10]");
            affix("input#DdlSBComm2[value=15]");
            affix("input#DdlSBComm3[value=15]");
            affix("input#SBComm");
            affix("input#SBComm1");
            affix("input#SBComm2");
            affix("input#SBComm3");

            affix("input#DdlRCComm1[value=20]");
            affix("input#DdlRCComm2[value=10]");
            affix("input#DdlRCComm3[value=30]");
            affix("input#RCComm");
            affix("input#RCComm1");
            affix("input#RCComm2");
            affix("input#RCComm3");

            affix("input#DdlNGComm1[value=10]");
            affix("input#DdlNGComm2[value=10]");
            affix("input#DdlNGComm3[value=10]");
            affix("input#NGComm");
            affix("input#NGComm1");
            affix("input#NGComm2");
            affix("input#NGComm3");

            affix("input#DdlLCComm1[value=100]");
            affix("input#DdlLCComm2[value=101]");
            affix("input#DdlLCComm3[value=102]");
            affix("input#LCComm");
            affix("input#LCComm1");
            affix("input#LCComm2");
            affix("input#LCComm3");

            affix("input#DdlCSComm1[value=110]");
            affix("input#DdlCSComm2[value=120]");
            affix("input#DdlCSComm3[value=130]");
            affix("input#CSComm");
            affix("input#CSComm1");
            affix("input#CSComm2");
            affix("input#CSComm3");

            GroupComm.initCommParam();
        });

        it("'#SBComm1, #SBComm2, #SBComm3' should have correct value", function () {
            expect($("#SBComm1").val()).toBe("10");
            expect($("#SBComm2").val()).toBe("15");
            expect($("#SBComm3").val()).toBe("15");
        });

        it("'#RCComm1, #RCComm2, #RCComm3' should have correct value", function () {
            expect($("#RCComm1").val()).toBe("20");
            expect($("#RCComm2").val()).toBe("10");
            expect($("#RCComm3").val()).toBe("30");
        });

        it("'#NGComm1, #NGComm2, #NGComm3' should have correct value", function () {
            expect($("#NGComm1").val()).toBe("10");
            expect($("#NGComm2").val()).toBe("10");
            expect($("#NGComm3").val()).toBe("10");
        });

        it("'#LCComm1, #LCComm2, #LCComm3' should have correct value", function () {
            expect($("#LCComm1").val()).toBe("100");
            expect($("#LCComm2").val()).toBe("101");
            expect($("#LCComm3").val()).toBe("102");
        });

        it("'#CSComm1, #CSComm2, #CSComm3' should have correct value", function () {
            expect($("#CSComm1").val()).toBe("110");
            expect($("#CSComm2").val()).toBe("120");
            expect($("#CSComm3").val()).toBe("130");
        });

        it("'#SBComm' should has correct value", function () {
            expect($("#SBComm").val()).toBe("0.1165");
        });

        it("'#RCComm' should has correct value", function () {
            expect($("#RCComm").val()).toBe("0.2130");
        });

        it("'#NGComm' should has correct value", function () {           
            expect($("#NGComm").val()).toBe("0.1110");
        });

        it("'#LCComm' should has correct value", function () {
            expect($("#LCComm").val()).toBe("1.1112");
        });

        it("'#CSComm' should has correct value", function () {
            expect($("#CSComm").val()).toBe("1.2330");
        });
    });
});