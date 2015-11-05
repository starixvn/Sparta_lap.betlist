describe("ManageAccount", function () {

    beforeEach(function () {
        spyOn(Site, "getSingleResource").and.callFake(function (text) {
            return text;
        });
    });

    describe("resetPassword(sender)", function () {
        it("showPopup() should be called with appropriate params", function () {
            var $sender = affix('[data-custid=1]');
            spyOn(ManageAccount, 'showPopup');
            ManageAccount.resetPassword($sender.get(0));

            expect(ManageAccount.showPopup).toHaveBeenCalledWith('ResetPassword', 400, 250, 'Account/ResetPassword?UserId=1&UserName=undefined&FirstName=undefined&LastName=undefined');
        });
    });

    describe("editAccount(sender)", function () {
        it("showPopup() should be called with correct url param", function () {
            var $sender = affix('[data-custid=1]');
            spyOn(ManageAccount, 'showPopup');
            ManageAccount.editAccount($sender.get(0));

            var params = ManageAccount.showPopup.calls.allArgs()[0];
            expect(params[3] === 'Account/EditAccount?UserId=1').toBe(true);
        });
    });
});
