describe("CustomerListCustOption", function () {
    describe("viewCustOption", function () {
        it("#view-cust-option should be showed", function () {
            affix("#context");
            $("#context").attr("custId", 1);
            affix("#view-cust-option");
            $("#view-cust-option").attr("custId", 2);

            CustomerListCustOption.viewCustOption($("#context"));

            expect($("#view-cust-option").is(":visible")).toBe(true);
        });
    });

    describe("closeCustOption()", function () {
        it("#view-cust-option should be hidden", function () {
            affix("#view-cust-option");

            CustomerListCustOption.closeCustOption();

            expect($("#view-cust-option").is(":visible")).toBe(false);
        });
    });

    describe("Customer Option", function () {
        beforeEach(function () {
            affix("#view-cust-option");
            $("#view-cust-option").attr({
                custid: "1020055",
                systemid: "12BETAUD01001",
                externalid: "alvinaud"
            });
        });

        describe("viewBetSetting()", function () {
            it("dialog function of jquery should be called", function () {
                affix("#bet-setting-dialog");

                spyOn($.fn, "dialog");

                CustomerListCustOption.viewBetSetting();

                expect($.fn.dialog).toHaveBeenCalled();
            });
        });

        describe("viewLogInfo()", function () {
            beforeEach(function () {
                spyOn(window, "alert");
                spyOn($.fn, "dialog");
                spyOn($.fn, "load");
                CustomerListCustOption.viewLogInfo();
            });

            it("dialog function of jquery should be called", function () {
                expect($.fn.dialog).toHaveBeenCalled();
            });

            it("load function of jquery should be called", function () {
                expect($.fn.load).toHaveBeenCalled();
            });
        });

        describe("viewBetList()", function () {
            it("dialog function of jquery should be called", function () {
                spyOn($.fn, "dialog");

                CustomerListCustOption.viewBetList();

                expect($.fn.dialog).toHaveBeenCalled();
            });
        });

        describe("viewStatement()", function () {
            it("dialog function of jquery should be called", function () {
                spyOn($.fn, "dialog");

                CustomerListCustOption.viewStatement();

                expect($.fn.dialog).toHaveBeenCalled();
            });
        });

        describe("viewRunningBetlist()", function () {
            it("dialog function of jquery should be called", function () {
                spyOn($.fn, "dialog");
                CustomerListCustOption.viewRunningBetlist("1020055", "12BETAUD01001", "alvinaud");

                expect($.fn.dialog).toHaveBeenCalled();
            });
        });
    })
});