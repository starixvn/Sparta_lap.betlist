/// <reference path="../../jasmine/jasmine-jquery.js" />
/// <reference path="../../jasmine/jasmine-fixture.js" />

describe("EditAccount", function () {
    describe("init(option)", function () {        
        it("#resultMessage should contain 'Hello' when clicking #btnSave", function () {
            var $btnSave = affix('input#btnSave[type=button]');
            var $resultMessage = affix('#resultMessage');
            spyOn($, "ajax").and.callFake(function (options) {
                options.success({ IsSuccessful: true, Message : "Hello"});                
            });
            EditAccount.init();

            $btnSave.click();
            expect($resultMessage.html()).toBe("Hello");
        });

        it("#resultMessage should contain 'Hello' when clicking #btnSave", function () {
            var $firstName = affix('input#FirstName');
            var $lastName = affix('input#LastName');
            affix('input#OldFirstName[value=Foo]');
            affix('input#OldLastName[value=Bar]');            
            
            var $btnReset = affix('input#btnReset[type=button]');
            EditAccount.init();

            $btnReset.click();
            expect($firstName.val()).toBe("Fooa");
        });
    });
});
