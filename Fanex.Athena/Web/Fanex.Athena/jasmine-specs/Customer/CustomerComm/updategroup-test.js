describe("UpdateGroup", function () {
    beforeEach(function () {
        affix("input#OriginalGroupName[value=]");
    });

    describe("initialize()", function () {
        it("UpdateGroup.initEvent() should be called", function () {
            spyOn(UpdateGroup, "initEvent");

            UpdateGroup.initialize();

            expect(UpdateGroup.initEvent).toHaveBeenCalled();
        });

        it("UpdateGroup.checkCommFirstLoad() should be called", function () {
            spyOn(UpdateGroup, "checkCommFirstLoad");

            UpdateGroup.initialize();

            expect(UpdateGroup.checkCommFirstLoad).toHaveBeenCalled();
        });
    });

    describe("checkCommFirstLoad()", function () {
        it("GroupComm.commissionOnChange(value, id) should be called with 5 times", function () {
            spyOn(GroupComm, "commissionOnChange");

            UpdateGroup.checkCommFirstLoad();

            expect(GroupComm.commissionOnChange.calls.count()).toBe(5);
        });
    });

    describe("initEvent()", function () {
        beforeEach(function () {
            affix("input#GroupName[value=group01]");

            affix("#showInput");
            affix("#GroupColor");
            spyOn($.fn, "spectrum");
            spyOn(GroupComm, "getcolorPalette");

            affix("#Validate");
            spyOn(UpdateGroup, "validateName");
            spyOn(UpdateGroup, "displayError");

            affix("#Save");
            spyOn(GroupComm, "initCommParam");
            spyOn(GroupComm, "initDefaultParam");

            affix("input#DdlSBComm1[value=10]");
            affix("input#DdlRCComm1[value=11]");
            affix("input#DdlNGComm1[value=12]");
            affix("input#DdlLCComm1[value=13]");
            affix("input#DdlCSComm1[value=14]");
            spyOn(GroupComm, "commissionOnChange");

            UpdateGroup.initEvent();
        });

        it("'#GroupName' should be focused", function () {
            expect($("#GroupName")).toBeFocused();
        });

        it("spectrum function of jquery should be called", function () {
            expect($.fn.spectrum).toHaveBeenCalled();
        })

        it("GroupComm.getcolorPalette() should be called", function () {
            expect(GroupComm.getcolorPalette).toHaveBeenCalled();
        })

        it("UpdateGroup.validateName(name) should be called when '#Validate' fired click event with correct name", function () {
            $("#Validate").click();

            expect(UpdateGroup.validateName).toHaveBeenCalledWith("group01");
        });

        it("UpdateGroup.displayError(msg) should be called when '#Validate' fired click event with incorrect name", function () {
            $("#GroupName").val("!@#");

            $("#Validate").click();

            expect(UpdateGroup.displayError).toHaveBeenCalled();
        });

        it("GroupComm.initCommParam() should be called when '#Save' fired click event", function () {
            $("#Save").click();

            expect(GroupComm.initCommParam).toHaveBeenCalled();
        });

        it("GroupComm.initDefaultParam(id) should be called with correct parameter when '#Save' fired click event", function () {
            $("#Save").click();

            expect(GroupComm.initDefaultParam).toHaveBeenCalledWith("#update-group-popup");
        });

        it("GroupComm.commissionOnChange(value, id) should be called with correct parameters when '#DdlSBComm1' fired change event", function () {
            $("#DdlSBComm1").change();

            expect(GroupComm.commissionOnChange).toHaveBeenCalledWith("10", "#DdlSBComm");
        });

        it("GroupComm.commissionOnChange(value, id) should be called with correct parameters when '#DdlRCComm1' fired change event", function () {
            $("#DdlRCComm1").change();

            expect(GroupComm.commissionOnChange).toHaveBeenCalledWith("11", "#DdlRCComm");
        });

        it("GroupComm.commissionOnChange(value, id) should be called with correct parameters when '#DdlNGComm1' fired change event", function () {
            $("#DdlNGComm1").change();

            expect(GroupComm.commissionOnChange).toHaveBeenCalledWith("12", "#DdlNGComm");
        });

        it("GroupComm.commissionOnChange(value, id) should be called with correct parameters when '#DdlLCComm1' fired change event", function () {
            $("#DdlLCComm1").change();

            expect(GroupComm.commissionOnChange).toHaveBeenCalledWith("13", "#DdlLCComm");
        });

        it("GroupComm.commissionOnChange(value, id) should be called with correct parameters when '#DdlCSComm1' fired change event", function () {
            $("#DdlCSComm1").change();

            expect(GroupComm.commissionOnChange).toHaveBeenCalledWith("14", "#DdlCSComm");
        });
    });

    describe("showConfirmAction(message, callback)", function () {
        it("dialog function of jquery should be called", function () {
            spyOn($.fn, "dialog");

            UpdateGroup.showConfirmAction("Testing message", function () { });

            expect($.fn.dialog).toHaveBeenCalled();
        });
    });

    describe("displayError(msg)", function () {
        beforeEach(function () {
            affix(".response-area");
            $(".response-area").addClass("field-validation-success");
            affix("#ValidateFail");
            affix("#ValidateOK");
            affix("input#Save");
            affix("input#GroupName");

            $(".response-area").hide();
            UpdateGroup.displayError("this is an error");
        });

        it("'.response-area' should not has class named 'field-validation-success'", function () {
            expect($(".response-area")).not.toHaveClass("field-validation-success");
        });
        it("'.response-area' should has class named 'field-validation-error'", function () {
            expect($(".response-area")).toHaveClass("field-validation-error");
        });
        it("'.response-area' should has text with content 'this is an error'", function () {
            expect($(".response-area")).toHaveText("this is an error");
        });
        it("'.response-area' should be visible", function () {
            expect($(".response-area")).toBeVisible();
        });

        it("'#ValidateFail' should have property 'display: inline'", function () {
            expect($("#ValidateFail")).toHaveCss({ display: "inline" });
        });
        it("'#ValidateOK' should have property 'display: none'", function () {
            expect($("#ValidateOK")).toHaveCss({ display: "none" });
        });
        it("'#Save' should be disabled", function () {
            expect($("#Save")).toBeDisabled();
        });

        it("'#GroupName' should be focused", function () {
            expect($("#GroupName")).toBeFocused();
        });
    });

    describe("validateName(groupName)", function () {
        var request;
        beforeEach(function (done) {
            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("/CustomerComGroup\/Validate\?groupName=group01/")
                .andReturn({
                    'status': 200,
                    'responseText': 'OK'
                });

            UpdateGroup.validateName("group01");

            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with url is 'CustomerComGroup/Validate?groupName=group01'", function () {
            expect(request.url).toContain("CustomerComGroup/Validate?groupName=group01");
        });
    });
});