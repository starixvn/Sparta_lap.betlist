describe("EditAccount", function () {
    describe("init(option)", function () {
        it("#resultMessage should contain 'Hello' when clicking #btnSave", function () {
            var $btnSave = affix('input#btnSave[type=button]');
            var $resultMessage = affix('#resultMessage');
            var ajaxOption;
            spyOn($, "ajax").and.callFake(function (options) { ajaxOption = options; });
            EditAccount.init();

            $btnSave.click();
            ajaxOption.success({ IsSuccessful: true, Message: "Hello" });

            expect($resultMessage.html()).toBe("Hello");
        });

        it("#resultMessage should contain 'Hello' when clicking #btnReset", function () {
            var $firstName = affix('input#FirstName');
            var $lastName = affix('input#LastName');
            affix('input#OldFirstName[value=Foo]');
            affix('input#OldLastName[value=Bar]');

            var $btnReset = affix('input#btnReset[type=button]');
            EditAccount.init();

            $btnReset.click();

            expect($firstName.val() === "Foo" && $lastName.val() === "Bar").toBe(true);
        });
    });
});