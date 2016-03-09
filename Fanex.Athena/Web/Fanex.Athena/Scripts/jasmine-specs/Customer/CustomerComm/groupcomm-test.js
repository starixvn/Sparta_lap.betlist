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

        it("'#IsExportExcel' should has value is true when '#ExportToExcel' fired click event", function () {
            $("#ExportToExcel").click();

            expect($("#IsExportExcel").val()).toBe("true");
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
});