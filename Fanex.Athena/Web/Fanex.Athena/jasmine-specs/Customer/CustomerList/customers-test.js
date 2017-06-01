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
            spyOn(CustomerListCustOption, "viewCustOption");

            affix("#popup-update");
            spyOn(CustomerList, "updateCustStatus");

            affix("#popup-close");
            spyOn(CustomerList, "closeUpdatePopUp");

            affix("input#popup-suspended");

            affix("input#popup-closed");
            spyOn(CustomerList, "custStatusCheckedChanged");

            affix("#bet-setting-link");
            spyOn(CustomerListCustOption, "viewBetSetting");

            affix("#bet-list-link");
            spyOn(CustomerListCustOption, "viewBetList");

            affix("#statement-link");
            spyOn(CustomerListCustOption, "viewStatement");

            affix("#login-info-link");
            spyOn(CustomerListCustOption, "viewLogInfo");

            affix("#close-cust-option-link");
            spyOn(CustomerListCustOption, "closeCustOption");

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

        it("CustomerListCustOption.viewCustOption() should be called when $('.custoptionlink') fired click event", function () {
            $('#report-content .custoptionlink').click();
            expect(CustomerListCustOption.viewCustOption).toHaveBeenCalled();
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

        it("CustomerListCustOption.viewBetSetting() should be called when $('#bet-setting-link') fired click event", function () {
            $('#bet-setting-link').click();
            expect(CustomerListCustOption.viewBetSetting).toHaveBeenCalled();
        });

        it("CustomerListCustOption.viewBetList() should be called when $('#bet-list-link') fired click event", function () {
            $('#bet-list-link').click();
            expect(CustomerListCustOption.viewBetList).toHaveBeenCalled();
        });

        it("CustomerListCustOption.viewStatement() should be called when $('#statement-link') fired click event", function () {
            $('#statement-link').click();
            expect(CustomerListCustOption.viewStatement).toHaveBeenCalled();
        });

        it("CustomerListCustOption.viewLogInfo() should be called when $('#login-info-link') fired click event", function () {
            $('#login-info-link').click();
            expect(CustomerListCustOption.viewLogInfo).toHaveBeenCalled();
        });

        it("CustomerListCustOption.closeCustOption() should be called when $('#close-cust-option-link') fired click event", function () {
            $('#close-cust-option-link').click();
            expect(CustomerListCustOption.closeCustOption).toHaveBeenCalled();
        });
    });

    describe("initGridColumnOption()", function () {
        beforeEach(function () {
            var allowedGridColumns = [{ Name: "Group", ID: 0, Checked: true }, { Name: "External ID", ID: 1, Checked: true }];
            var allowedGridColumnsJsonString = JSON.stringify(allowedGridColumns);
            affix("input#AllowedGridColumns");
            $("#AllowedGridColumns").val(allowedGridColumnsJsonString);

            affix("#grid-columns-multiselect");
            spyOn($.fn, "multiSelect");

            CustomerList.initGridColumnOption();
        });
        it("multiSelect function of jquery should be called", function () {
            expect($.fn.multiSelect).toHaveBeenCalled();
        });
    });

    describe("initAutoComplete()", function () {
        beforeEach(function () {
            affix("input#txtUsername");
            spyOn($.fn, "autocomplete");
            spyOn(CustomerList, "setDefaultUsername");

            CustomerList.initAutoComplete();
        });

        it("autocomplete function of jquery should be called with correct param", function () {
            expect($.fn.autocomplete).toHaveBeenCalledWith({ serviceUrl: "CustomerAPI/SearchCustomer", minChars: 2 });
        });

        it("CustomerList.setDefaultUsername() should be called", function () {
            expect(CustomerList.setDefaultUsername).toHaveBeenCalled();
        });
    });

    describe("submit()", function () {
        var request;
        beforeEach(function (done) {
            spyOn(CustomerList, "hasSelectedMultiselectData").and.returnValue(true);

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("Customer/CustomerList")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ }'
                });

            affix('#ReportGrid');
            spyOn(CustomersOption, 'setGroupHeaderColumns');
            CustomerList.submit();

            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with url is 'Customer/CustomerList'", function () {
            expect(request.url).toBe("Customer/CustomerList");
        });

    });

    describe("hasSelectedMultiselectData(dataList)", function () {
        it("Result should be true when all items checked", function () {
            var dataList = [{ Name: "Group", ID: 0, Checked: true }, { Name: "External ID", ID: 1, Checked: true }];

            var result = CustomerList.hasSelectedMultiselectData(dataList);

            expect(result).toBe(true);
        });

        it("Result should be false when all items unchecked", function () {
            var dataList = [{ Name: "Group", ID: 0, Checked: false }, { Name: "External ID", ID: 1, Checked: false }];

            var result = CustomerList.hasSelectedMultiselectData(dataList);

            expect(result).toBe(false);
        });

        it("Result should be true when at least one item checked", function () {
            var dataList = [{ Name: "Group", ID: 0, Checked: true }, { Name: "External ID", ID: 1, Checked: false }];

            var result = CustomerList.hasSelectedMultiselectData(dataList);

            expect(result).toBe(true);
        });
    });

    describe("loadPostbackParam", function () {
        it("Site.currencyConstraint() should be called", function () {
            spyOn(Site, "currencyConstraint");
            CustomerList.loadPostbackParam();
            expect(Site.currencyConstraint).toHaveBeenCalled();
        });
    });

    describe("custStatusCheckedChanged()", function () {
        it("#popup-update should be enable", function () {
            affix("#update-cust-status-popup");
            var $container = affix("div");
            $container.append("<input type='checkbox' id='popup-suspended' />");
            $("#popup-suspended").prop("checked", true);
            affix("#popup-update");

            CustomerList.custStatusCheckedChanged();

            expect($("#popup-update").prop("disabled")).toBe(false);
        });
    });

    describe("updateCustStatus()", function () {
        var request;
        beforeEach(function (done) {
            spyOn(window, "alert");
            spyOn(window, "confirm").and.returnValue(true);

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("CustomerAPI/UpdateCustStatus")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ }'
                });

            CustomerList.updateCustStatus();

            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with url is 'CustomerAPI/UpdateCustStatus'", function () {
            expect(request.url).toBe("CustomerAPI/UpdateCustStatus");
        });
    });

    describe("updateBASetting(context)", function () {
        var request;
        beforeEach(function (done) {
            spyOn(window, "alert");
            spyOn(window, "confirm").and.returnValue(true);
            affix("input#CanUpdateBA[value=true]");
            affix("div[class=ba-on]");
            $(".ba-on").attr("data-val", "#1032392#false#zhangfan87");

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("CustomerAPI/UpdateBAStatus")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ }'
                });

            CustomerList.updateBASetting(".ba-on");

            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with url is 'CustomerAPI/UpdateBAStatus'", function () {
            expect(request.url).toBe("CustomerAPI/UpdateBAStatus");
        });
    });
});