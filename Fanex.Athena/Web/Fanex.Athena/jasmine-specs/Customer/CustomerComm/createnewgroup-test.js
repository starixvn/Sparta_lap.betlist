describe("CreateGroup", function () {
    describe("initialize()", function () {
        it("CreateGroup.initEvent() should be called", function () {
            spyOn(CreateGroup, "initEvent");

            CreateGroup.initialize();

            expect(CreateGroup.initEvent).toHaveBeenCalled();
        });
    });

    describe("initEvent()", function () {
        beforeEach(function () {
            affix("input#GroupColor[value=#fff]");
            affix("#showInput");
            affix("#Add");
            affix("#Cancel");
            affix("#Validate");
            affix("input#DdlSBComm1[value=2]");
            affix("input#DdlRCComm1[value=10]");
            affix("input#DdlNGComm1[value=20]");
            affix("input#DdlLCComm1[value=30]");
            affix("input#DdlCSComm1[value=50]");

            spyOn($.fn, "spectrum").and.returnValue({});
            spyOn(GroupComm, "getcolorPalette");
            CreateGroup.initEvent();
        });

        it("spectrum function of jquery should be called", function () {                      
            expect($.fn.spectrum).toHaveBeenCalled();
        });
        it("GroupComm.getcolorPalette() should be called", function () {
            expect(GroupComm.getcolorPalette).toHaveBeenCalled();
        });

        it("GroupComm.initCommParam() should be called when '#Add' fired click event", function () {
            spyOn(GroupComm, "initCommParam");

            $("#Add").click();

            expect(GroupComm.initCommParam).toHaveBeenCalled();
        });

        it("Site.redirect() should be called with correct url when '#Cancel' fired click event", function () {
            spyOn(Site, "redirect");

            $("#Cancel").click();

            expect(Site.redirect).toHaveBeenCalledWith(Site.resolveClientUrl("CustomerComGroup/Index"));
        });

        it("GroupComm.initDefaultParam() should be called with correct parameter when '#Add' fired click event", function () {
            spyOn(GroupComm, "initDefaultParam");

            $("#Add").click();

            expect(GroupComm.initDefaultParam).toHaveBeenCalledWith("#create-group-popup");
        });

        describe("#Validate button click", function () {
            beforeAll(function () {
                spyOn(CreateGroup, "validateName");
                spyOn(CreateGroup, "displayError");
            });

            it("CreateGroup.displayError() should be called with correct param if groupname is empty", function () {
                affix('input#GroupName[type=text]');
                $("#Validate").click();
                expect(CreateGroup.displayError).toHaveBeenCalledWith('GroupNameNotEmptyMsg');
            });

            it("CreateGroup.displayError() should be called with correct param if groupname length >= 100", function () {
                var text = new Array(101).join("a");
                affix('input#GroupName[type=text][value=' + text + ']');
                $("#Validate").click();
                expect(CreateGroup.displayError).toHaveBeenCalledWith('GroupNameNotExceed100CharactersMsg');
            });

            it("CreateGroup.displayError() should be called with correct param if groupname = 'none'", function () {
                affix('input#GroupName[type=text][value=none]');
                $("#Validate").click();
                expect(CreateGroup.displayError).toHaveBeenCalledWith('ChooseGroupNameMsg');
            });

            it("CreateGroup.displayError() should be called with correct param if groupname contains special characters", function () {
                affix('input#GroupName[type=text][value=$%^&]');
                $("#Validate").click();
                expect(CreateGroup.displayError).toHaveBeenCalledWith('GroupNameOnlyContainCharactersAndNumberMsg');
            });

            it("CreateGroup.validateName(groupname) should be called if groupname is valid", function () {
                affix('input#GroupName[type=text][value=abc]');
                $("#Validate").click();
                expect(CreateGroup.validateName).toHaveBeenCalledWith("abc");
            });
        });

        it("'#Add' should be enabled when '#GroupName' fired keyup event", function () {
            $("#GroupName").keyup();

            expect($("#Add")).not.toBeDisabled();
        });

        it("GroupComm.commissionOnChange(value, id) should be called when '#DdlSBComm1' fired change event", function () {
            spyOn(GroupComm, "commissionOnChange");

            $("#DdlSBComm1").change();

            expect(GroupComm.commissionOnChange).toHaveBeenCalledWith("2", "#DdlSBComm");
        });

        it("GroupComm.commissionOnChange(value, id) should be called when '#DdlRCComm1' fired change event", function () {
            spyOn(GroupComm, "commissionOnChange");

            $("#DdlRCComm1").change();

            expect(GroupComm.commissionOnChange).toHaveBeenCalledWith("10", "#DdlRCComm");
        });

        it("GroupComm.commissionOnChange(value, id) should be called when '#DdlNGComm1' fired change event", function () {
            spyOn(GroupComm, "commissionOnChange");

            $("#DdlNGComm1").change();

            expect(GroupComm.commissionOnChange).toHaveBeenCalledWith("20", "#DdlNGComm");
        });

        it("GroupComm.commissionOnChange(value, id) should be called when '#DdlLCComm1' fired change event", function () {
            spyOn(GroupComm, "commissionOnChange");

            $("#DdlLCComm1").change();

            expect(GroupComm.commissionOnChange).toHaveBeenCalledWith("30", "#DdlLCComm");
        });

        it("GroupComm.commissionOnChange(value, id) should be called when '#DdlCSComm1' fired change event", function () {
            spyOn(GroupComm, "commissionOnChange");

            $("#DdlCSComm1").change();

            expect(GroupComm.commissionOnChange).toHaveBeenCalledWith("50", "#DdlCSComm");
        });
    });

    describe("CreateGroup.displayError(msg)", function () {
        beforeEach(function () {
            affix(".response-area");
            $(".response-area").addClass("field-validation-success");
            affix("#ValidateFail");
            affix("#ValidateOK");
            affix("input#Add");
            affix("input#GroupName");

            $(".response-area").hide();
            CreateGroup.displayError("this is an error");
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
            expect($("#ValidateFail")).toHaveCss({display: "inline"});
        });
        it("'#ValidateOK' should have property 'display: none'", function () {
            expect($("#ValidateOK")).toHaveCss({ display: "none" });
        });
        it("'#Add' should be disabled", function () {
            expect($("#Add")).toBeDisabled();
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
                    'contentType': 'application/json',
                    'responseText': 'OK'
                });

            CreateGroup.validateName("group01");

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