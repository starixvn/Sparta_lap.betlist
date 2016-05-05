describe("CustomerLog", function () {
    describe("initAutoComplete()", function () {
        it("autocomplete function of jquery should be called", function () {
            affix("input#Username[value=user01]");

            spyOn($.fn, "autocomplete");

            CustomerLog.initAutoComplete();

            expect($.fn.autocomplete).toHaveBeenCalled();
        });
    });

    describe("dateConstraint()", function () {
        beforeEach(function () {
            affix("#MonthPickerFromDay .selectedDay");
            affix("#MonthPickerToDay .selectedDay");
        });

        it("value should be returned 'false' when fromDate greater than toDay", function () {
            $("#MonthPickerFromDay .selectedDay").text("12");
            $("#MonthPickerToDay .selectedDay").text("8");

            var result = CustomerLog.dateConstraint();

            expect(result).toBe(false);
        });

        it("value should be returned 'true' when fromDate less than toDay", function () {
            $("#MonthPickerFromDay .selectedDay").text("12");
            $("#MonthPickerToDay .selectedDay").text("20");

            var result = CustomerLog.dateConstraint();

            expect(result).toBe(true);
        });
    });

    describe("setDefaultUsername()", function () {
        beforeEach(function () {
            affix("input#Username");
        });

        it("'#Username' should has class named 'default-username' when value of user name is empty", function () {
            CustomerLog.setDefaultUsername();

            expect($("#Username")).toHaveClass("default-username");
        });

        it("'#Username' should not has class named 'default-username' when value of user name is not empty", function () {
            $("#Username").val("user01");

            CustomerLog.setDefaultUsername();

            expect($("#Username")).not.toHaveClass("default-username");
        });
    });

    describe("setUsernameOnFocus()", function () {
        beforeEach(function () {
            affix("input#Username[value=ExternalIdOrSystemId]");
            CustomerLog.setUsernameOnFocus();
        });

        it("'#Username' should has not class named 'class'", function () {
            expect($("#Username").val()).toBe("");
        });
        it("'#Username' should has empty value", function () {
            expect($("#Username")).not.toHaveClass("class");
        });
    });

    describe("checkShowAll()", function () {
        beforeEach(function () {
            affix("input#showall[type=checkbox]");
            affix(".old-value");
            $(".old-value").css({width: "10px", height: "20px", background: "yellow"});           
        });

        it("'.old-value' should be hidden when '#showall' is not checked", function () {
            CustomerLog.checkShowAll();

            expect($(".old-value")).toBeHidden();
        });

        it("'.old-value' should be showed when '#showall' is checked", function () {
            $("#showall").prop("checked", true);

            CustomerLog.checkShowAll();

            expect($(".old-value")).not.toBeHidden();
        });
    });
});