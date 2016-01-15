describe("CustomerList", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            var productAccessData = {
                ProductList: [
                                {
                                    ProductID: 0,
                                    ProductName: 'Sportsbook',
                                    ProductCode: 'SB',
                                    AllowCommission: true,
                                    AllowWinloss: true
                                }]
            };
            var productAccessValue = JSON.stringify(productAccessData);
            affix("input#ProductAccess");
            $("#ProductAccess").val(productAccessValue);
            spyOn(CustomerList, "initEvent");
            spyOn(CustomerList, "initPopup");
            spyOn(CustomerList, "initAutoComplete");
            spyOn(CustomerList, "initGridColumnOption");
            spyOn(CustomerList, "initOddGroupOption");
            spyOn(Site, "addUserGuide");

            CustomerList.initialize();
        });

        it("CustomerList.initEvent() should be called", function () {
            expect(CustomerList.initEvent).toHaveBeenCalled();
        });

        it("CustomerList.initPopup() should be called", function () {
            expect(CustomerList.initPopup).toHaveBeenCalled();
        });

        it("CustomerList.initAutoComplete() should be called", function () {
            expect(CustomerList.initAutoComplete).toHaveBeenCalled();
        });

        it("CustomerList.initGridColumnOption() should be called", function () {
            expect(CustomerList.initGridColumnOption).toHaveBeenCalled();
        });

        it("CustomerList.initOddGroupOption() should be called", function () {
            expect(CustomerList.initOddGroupOption).toHaveBeenCalled();
        });

        it("Site.addUserGuide() should be called with correct parameters", function () {
            expect(Site.addUserGuide).toHaveBeenCalledWith({ resourceName: "CustomerList", title: "CustomerList" });
        });
    });

    describe("initEvent()", function () {
        beforeEach(function () {
            spyOn(CustomerList, "loadPostbackParam");

            affix("button#SubmitButtonComm");
            spyOn(CustomerList, "submit");

            affix("input#txtUsername");
            spyOn(CustomerList, "setUsernameOnFocus");
            spyOn(CustomerList, "setDefaultUsername");

            affix("#report-content .ba-off");
            affix(".ba-on");
            $("#report-content").append($(".ba-on")[0]);
            spyOn(CustomerList, "updateBASetting");

            affix(".cust-status-option");
            $("#report-content").append($(".cust-status-option")[0]);
            spyOn(CustomerList, "viewUpdatePopUp");

            affix(".custoptionlink");
            $("#report-content").append($(".custoptionlink")[0]);
            spyOn(CustomerList, "viewCustOption");

            affix("#popup-update");
            spyOn(CustomerList, "updateCustStatus");

            affix("#popup-close");
            spyOn(CustomerList, "closeUpdatePopUp");

            affix("input#popup-suspended");

            affix("input#popup-closed");
            spyOn(CustomerList, "custStatusCheckedChanged");

            affix("#bet-setting-link");
            spyOn(CustomerList, "viewBetSetting");

            affix("#bet-list-link");
            spyOn(CustomerList, "viewBetList");

            affix("#statement-link");
            spyOn(CustomerList, "viewStatement");

            affix("#login-info-link");
            spyOn(CustomerList, "viewLogInfo");

            affix("#close-cust-option-link");
            spyOn(CustomerList, "closeCustOption");

            CustomerList.initEvent();
        });

        it("CustomerList.loadPostbackParam() should be called", function () {
            expect(CustomerList.loadPostbackParam).toHaveBeenCalled();
        });

        it("CustomerList.submit() should be called when $('#SubmitButtonComm') fired click event", function () {
            $('#SubmitButtonComm').click();
            expect(CustomerList.submit).toHaveBeenCalled();
        });

        it("CustomerList.setUsernameOnFocus() should be called when $('#txtUsername') fired click event", function () {
            $('#txtUsername').click();
            expect(CustomerList.setUsernameOnFocus).toHaveBeenCalled();
        });

        it("CustomerList.setDefaultUsername() should be called when $('#txtUsername') fired blur event", function () {
            $('#txtUsername').blur();
            expect(CustomerList.setDefaultUsername).toHaveBeenCalled();
        });

        it("CustomerList.updateBASetting() should be called when $('.ba-off') fired click event", function () {
            $('#report-content .ba-off').click();
            expect(CustomerList.updateBASetting).toHaveBeenCalled();
        });

        it("CustomerList.updateBASetting() should be called when $('.ba-on') fired click event", function () {
            $('#report-content .ba-on').click();
            expect(CustomerList.updateBASetting).toHaveBeenCalled();
        });

        it("CustomerList.viewUpdatePopUp() should be called when $('.cust-status-option') fired click event", function () {
            $('#report-content .cust-status-option').click();
            expect(CustomerList.viewUpdatePopUp).toHaveBeenCalled();
        });

        it("CustomerList.viewCustOption() should be called when $('.custoptionlink') fired click event", function () {
            $('#report-content .custoptionlink').click();
            expect(CustomerList.viewCustOption).toHaveBeenCalled();
        });



        it("CustomerList.updateCustStatus() should be called when $('#popup-update') fired click event", function () {
            $('#popup-update').click();
            expect(CustomerList.updateCustStatus).toHaveBeenCalled();
        });

        it("CustomerList.closeUpdatePopUp() should be called when $('#popup-close') fired click event", function () {
            $('#popup-close').click();
            expect(CustomerList.closeUpdatePopUp).toHaveBeenCalled();
        });

        it("CustomerList.custStatusCheckedChanged() should be called when $('#popup-suspended') fired change event", function () {
            $('#popup-suspended').change();
            expect(CustomerList.custStatusCheckedChanged).toHaveBeenCalled();
        });

        it("CustomerList.custStatusCheckedChanged() should be called when $('#popup-closed') fired change event", function () {
            $('#popup-closed').change();
            expect(CustomerList.custStatusCheckedChanged).toHaveBeenCalled();
        });

        it("CustomerList.viewBetSetting() should be called when $('#bet-setting-link') fired click event", function () {
            $('#bet-setting-link').click();
            expect(CustomerList.viewBetSetting).toHaveBeenCalled();
        });

        it("CustomerList.viewBetList() should be called when $('#bet-list-link') fired click event", function () {
            $('#bet-list-link').click();
            expect(CustomerList.viewBetList).toHaveBeenCalled();
        });

        it("CustomerList.viewStatement() should be called when $('#statement-link') fired click event", function () {
            $('#statement-link').click();
            expect(CustomerList.viewStatement).toHaveBeenCalled();
        });

        it("CustomerList.viewLogInfo() should be called when $('#login-info-link') fired click event", function () {
            $('#login-info-link').click();
            expect(CustomerList.viewLogInfo).toHaveBeenCalled();
        });

        it("CustomerList.closeCustOption() should be called when $('#close-cust-option-link') fired click event", function () {
            $('#close-cust-option-link').click();
            expect(CustomerList.closeCustOption).toHaveBeenCalled();
        });
    });
});