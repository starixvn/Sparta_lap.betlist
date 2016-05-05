describe("CustomerComm", function () {
    describe("initialize()", function () {
        beforeEach(function () {
            affix("#MoveToGroup");
            affix("#UpdateComm[data-group-id=12]");
            affix("#RemoveFromGroup");
            affix("input#GroupListSelect");

            affix("input#CustSBComm1[value=5]");
            affix("input#CustRBComm1[value=6]");
            affix("input#CustNGComm1[value=7]");
            affix("input#CustLCComm1[value=8]");
            affix("input#CustCSComm1[value=9]");

            CustomerComm.initialize();
        });

        it("CustomerComm.showMoveCustomerToGroup() should be called when '#MoveToGroup' fired click event", function () {            
            spyOn(CustomerComm, "showMoveCustomerToGroup");
            $('#MoveToGroup').click();

            expect(CustomerComm.showMoveCustomerToGroup).toHaveBeenCalled();
        });

        it("CustomerComm.showUpdateCustComm(groupCommId) should be called with correct parameter when '#UpdateComm' fired click event", function () {
            spyOn(CustomerComm, "showUpdateCustComm");
            $('#UpdateComm').click();

            expect(CustomerComm.showUpdateCustComm).toHaveBeenCalledWith("12");
        });

        it("CustomerComm.showRemoveFromGroup() should be called when '#RemoveFromGroup' fired click event", function () {
            spyOn(CustomerComm, "showRemoveFromGroup");
            $('#RemoveFromGroup').click();

            expect(CustomerComm.showRemoveFromGroup).toHaveBeenCalled();
        });

        it("CustomerComm.newGroupChanged(obj) should be called when '#GroupListSelect' fired change event", function () {
            spyOn(CustomerComm, "newGroupChanged");
            $('#GroupListSelect').change();

            expect(CustomerComm.newGroupChanged).toHaveBeenCalled();
        });

        it("CustomerComm.commissionOnChange(value, id) should be called with correct parameters when '#CustSBComm1' fired change event", function () {
            spyOn(CustomerComm, "commissionOnChange");
            $('#CustSBComm1').change();

            expect(CustomerComm.commissionOnChange).toHaveBeenCalledWith("5", "#CustSBComm");
        });
        it("CustomerComm.commissionOnChange(value, id) should be called with correct parameters when '#CustRBComm1' fired change event", function () {
            spyOn(CustomerComm, "commissionOnChange");
            $('#CustRBComm1').change();

            expect(CustomerComm.commissionOnChange).toHaveBeenCalledWith("6", "#CustRBComm");
        });
        it("CustomerComm.commissionOnChange(value, id) should be called with correct parameters when '#CustNGComm1' fired change event", function () {
            spyOn(CustomerComm, "commissionOnChange");
            $('#CustNGComm1').change();

            expect(CustomerComm.commissionOnChange).toHaveBeenCalledWith("7", "#CustNGComm");
        });
        it("CustomerComm.commissionOnChange(value, id) should be called with correct parameters when '#CustLCComm1' fired change event", function () {
            spyOn(CustomerComm, "commissionOnChange");
            $('#CustLCComm1').change();

            expect(CustomerComm.commissionOnChange).toHaveBeenCalledWith("8", "#CustLCComm");
        });
        it("CustomerComm.commissionOnChange(value, id) should be called with correct parameters when '#CustCSComm1' fired change event", function () {
            spyOn(CustomerComm, "commissionOnChange");
            $('#CustCSComm1').change();

            expect(CustomerComm.commissionOnChange).toHaveBeenCalledWith("9", "#CustCSComm");
        });
    });

    describe("commissionOnChange(value, id)", function () {
        beforeEach(function () {
            affix("input#CustSBComm2");
            affix("input#CustSBComm3");
        });

        it("value of #CustSBComm2, #CustSBComm3 should be 0 when 'value' param equal 15", function () {
            CustomerComm.commissionOnChange(15, "#CustSBComm");

            expect($("#CustSBComm2").val()).toBe("0");
            expect($("#CustSBComm3").val()).toBe("0");
        });

        it("#CustSBComm2, #CustSBComm3 should be disabled when 'value' param equal 15", function () {
            CustomerComm.commissionOnChange(15, "#CustSBComm");

            expect($("#CustSBComm2")).toBeDisabled();
            expect($("#CustSBComm3")).toBeDisabled();
        });

        it("#CustSBComm2, #CustSBComm3 should be enabled when 'value' param not equal 15", function () {
            CustomerComm.commissionOnChange(5, "#CustSBComm");

            expect($("#CustSBComm2")).not.toBeDisabled();
            expect($("#CustSBComm3")).not.toBeDisabled();
        });
    });

    describe("showMoveCustomerToGroup()", function () {
        beforeEach(function () {
            affix("#MoveToGroupPopUp");

            spyOn(CustomerComm, "clearNotification");
            spyOn($.fn, "dialog");

            CustomerComm.showMoveCustomerToGroup();
        });

        it("CustomerComm.clearNotification() should be called", function () {
            expect(CustomerComm.clearNotification).toHaveBeenCalled();
        });
        it("dialog function of jquery should be called", function () {
            expect($.fn.dialog).toHaveBeenCalled();
        });
    });

    describe("moveCustomerToGroup()", function () {
        var request;
        beforeEach(function (done) {
            affix("input#GroupListSelect[value=4]");
            spyOn(CustomerList, "getCheckedCustomers").and.returnValue([{ Id: 1, Group: 1 }, { Id: 2, Group: 1 }, { Id: 3, Group: 2 }]);

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("CustomerAPI/MoveToGroupCom")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ }'
                });
            CustomerComm.moveCustomerToGroup();
            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });
        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("CustomerList.getCheckedCustomers() should be called", function () {
            expect(CustomerList.getCheckedCustomers).toHaveBeenCalled();
        });
        it("ajax request should be called with url is 'CustomerAPI/MoveToGroupCom'", function () {
            expect(request.url).toBe("CustomerAPI/MoveToGroupCom");
        });
    });

    describe("showUpdateCustComm(groupCommId)", function () {
        beforeEach(function () {
            affix("#UpdateCustCommPopUp");
            spyOn(CustomerList, "getCheckedCustomers").and.returnValue([{ Id: 1, Group: 1 }, { Id: 2, Group: 1 }, { Id: 3, Group: 2 }]);
            spyOn(CustomerComm, "clearNotification");
            spyOn($.fn, "dialog");

            CustomerComm.showUpdateCustComm(1);
        });

        it("CustomerList.getCheckedCustomers() should be called", function () {
            expect(CustomerList.getCheckedCustomers).toHaveBeenCalled();
        });
        it("CustomerComm.clearNotification() should be called", function () {
            expect(CustomerComm.clearNotification).toHaveBeenCalled();
        });
        it("dialog function of jquery should be called", function () {
            expect($.fn.dialog).toHaveBeenCalled();
        });
    });

    describe("initCommissionParam(id)", function () {
        beforeEach(function () {
            affix("input#CustSBComm1[value=10]");
            affix("input#CustSBComm2[value=10]");
            affix("input#CustSBComm3[value=10]");
        });

        it("CustomerComm.initCommissionParam(id) should be returned with correct value", function () {
            var commissionValue = CustomerComm.initCommissionParam("#CustSBComm");
            var expectedValue = 0.111.toFixed(4);
            expect(commissionValue).toBe(expectedValue);
        });
    });

    describe("updateCustomerComm(ctx, groupCommId)", function () {
        beforeEach(function (done) {
            spyOn(CustomerList, "getCheckedCustomers").and.returnValue([{ Id: 1, Group: 1 }, { Id: 2, Group: 1 }, { Id: 3, Group: 2 }]);
            spyOn(CustomerComm, "initCommissionParam").and.returnValue(0.1234);

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("CustomerAPI/MoveToGroupCom")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ }'
                });
            CustomerComm.updateCustomerComm(null, 1);
            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });
        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("CustomerList.getCheckedCustomers() should be called", function () {
            expect(CustomerList.getCheckedCustomers).toHaveBeenCalled();
        });

        it("CustomerComm.initCommissionParam(id) should be called four times", function () {
            expect(CustomerComm.initCommissionParam.calls.count()).toBe(5);
        });

        it("ajax request should be called with url is 'CustomerAPI/UpdateCustomerCom'", function () {
            expect(request.url).toBe("CustomerAPI/UpdateCustomerCom");
        });
    });

    describe("showRemoveFromGroup()", function () {
        beforeEach(function () {
            affix("#ConfirmRemoveFromGroupPopUp");
            spyOn($.fn, "dialog");

            CustomerComm.showRemoveFromGroup();
        });

        it("dialog function of jquery should be called", function () {
            expect($.fn.dialog).toHaveBeenCalled();
        });
    });

    describe("removeFromGroupComm()", function () {
        beforeEach(function (done) {
            affix("input#KeepComm[type=checkbox][checked=checked]");
            spyOn(CustomerList, "getCheckedCustomers").and.returnValue([{ Id: 1, Group: 1 }, { Id: 2, Group: 1 }, { Id: 3, Group: 2 }]);

            jasmine.Ajax.install();
            jasmine.Ajax
                .stubRequest("CustomerAPI/RemoveFromGroupCom")
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ }'
                });
            CustomerComm.removeFromGroupComm();
            request = jasmine.Ajax.requests.mostRecent();

            setTimeout(done, 10);
        });
        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it("ajax request should be called with url is 'CustomerAPI/RemoveFromGroupCom'", function () {
            expect(request.url).toBe("CustomerAPI/RemoveFromGroupCom");
        });
    });

    describe("showMsg(msg, isSuccess)", function () {
        beforeEach(function () {
            affix(".success-message");
            affix(".error-message");
        });

        it("'.success-message' should be got success message when success", function () {
            CustomerComm.showMsg("success", true);
            expect($(".success-message").html()).toBe("success");
        });

        it("'.error-message' should be hidden when success", function () {
            CustomerComm.showMsg("success", true);
            expect($(".error-message")).toBeHidden();
        });

        it("'.error-message' should be showed when fail", function () {
            CustomerComm.showMsg("fail", false);
            expect($(".error-message")).not.toBeHidden();
        });

        it("'.error-message' should be got error message when fail", function () {
            CustomerComm.showMsg("fail", false);
            expect($(".error-message").html()).toBe("fail");
        });
    });
});