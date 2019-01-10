describe('EditPermission', function () {
    describe('collapseExpand(sender)', function () {
        it('"sender" element should be toggled attribute "data-status" from "expand" to "collapse"', function () {
            var $element = affix('[data-status=expand]');
            EditPermission.collapseExpand($element.get(0));
            expect($element.attr('data-status')).toBe('collapse');
        });
    });

    describe('checkedChange(sender)', function () {
        it('"EditPermission.uncheckAll()" should be called if "sender" is not checked', function () {
            var $element = affix('input[type=checkbox]');
            spyOn(EditPermission, 'uncheckAll');
            EditPermission.checkedChange($element);

            expect(EditPermission.uncheckAll).toHaveBeenCalled();
        });
    });

    describe('checkAll(id)', function () {
        it('all checkboxes in the container should be checked', function () {
            var $container = affix('#group_img1');
            $container.affix('input[type=checkbox]');
            $container.affix('input[type=checkbox]');

            EditPermission.checkAll(1);

            expect($container.find('input:checked').length).toBe(2);
        });
    });

    describe('uncheckAll(id)', function () {
        it('all checkboxes in the container should be un-checked', function () {
            $("#group_img1").remove();
            var $container = affix('#group_img1');
            $container.affix('input[type=checkbox]');
            $container.affix('input[type=checkbox]');

            EditPermission.uncheckAll(1);

            expect($container.find('input:checked').length).toBe(0);
        });
    });

    describe('childCheckedChange(parentId)', function () {
        it('all checkboxes in the container should be un-checked', function () {
            $("#group_img1").remove();
            var $container = affix('#group_img1');
            $container.affix('input[type=checkbox]').prop('checked', true);
            $container.affix('input[type=checkbox]').prop('checked', true);

            EditPermission.uncheckAll(1);

            expect($container.find('input:checked').length).toBe(0);
        });
    });

    describe('getSelectedFunctions()', function () {
        it('returns the id list (as comma-separated string) of checked checkboxes in ".permissionBox"', function () {
            var $permissionBox = affix('.permissionBox');
            $permissionBox.affix('input#1[type=checkbox]').prop('checked', true);
            $permissionBox.affix('input#2[type=checkbox]');
            $permissionBox.affix('input#3[type=checkbox]').prop('checked', true);

            var result = EditPermission.getSelectedFunctions();

            expect(result).toBe('1,3');
        });
    });

    describe('updatePermission()', function () {
        beforeEach(function () {
            $("#currency-content").remove();
            affix('form#EditPermissionForm[action="localhost-test"]');

            jasmine.Ajax.install();

            jasmine.Ajax
                .stubRequest('localhost-test')
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ "IsSuccessful": true, "Message": "Hello" }'
                });
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it('ajax url should get from form[action]', function () {
            EditPermission.updatePermission();
            var request = jasmine.Ajax.requests.mostRecent();
            expect(request.url).toBe('localhost-test');
        });

        it('request data "UserId" should get from input#UserId', function () {
            affix('input#UserId[value=1]');
            EditPermission.updatePermission();
            var request = jasmine.Ajax.requests.mostRecent();

            expect(request.data().UserId).toEqual(['1']);
        });

        it('#resultMessage should contain message from response', function (done) {
            affix('#resultMessage');

            EditPermission.updatePermission();

            setTimeout(function () {
            expect($('#resultMessage').text()).toBe('Hello');           
                done();
            }, 100);
        });

    });

    describe('changeAllCurrency(sender)', function () {
        it('all checkboxes in #currency-content should be checked', function () {
            $('#currency-content').remove();
            var $sender = affix('input[type=checkbox]').prop('checked', true);
            var $container = affix('#currency-content');
            $container.affix('input[type=checkbox][name=123]');
            $container.affix('input[type=checkbox][name=123]');

            EditPermission.changeAllCurrency($sender.get(0));

            expect($container.find('input:checked').length).toBe(2);
        });
    });

    describe('changeEachCurrency()', function () {
        it('checkbox #currency-header should be checked if all checkboxes in #currency-content are checked', function () {
            $('#currency-content').remove();
            var $currencyHeader = affix('input#currency-header[type=checkbox]');
            var $container = affix('#currency-content');
            $container.affix('input[type=checkbox]').prop('checked', true);

            EditPermission.changeEachCurrency();

            expect($('#currency-header').is(':checked')).toBe(true);
        });
    });

    describe('getSelectedCurrencies()', function () {
        it('return values of checked checkboxes', function () {
            affix('.currencyBox');
            $('#currency-content').remove();
            var $container = affix('#currency-content');
            $container.affix('input[type=checkbox]').val(1).prop('checked', true);
            $container.affix('input[type=checkbox]').val(2);
            $container.affix('input[type=checkbox]').val(3).prop('checked', true);

            var currencies = EditPermission.getSelectedCurrencies();

            expect(currencies).toBe('1,3');
        });
    });

    describe('showResponseMessage(response)', function () {
        it('#resultMessage should has class "field-validation-success" if response.IsSuccessful is true', function () {
            $("#resultMessage").remove();
            var $resultMessage = affix('#resultMessage');

            var currencies = EditPermission.showResponseMessage({ IsSuccessful : true });

            expect($resultMessage.hasClass('field-validation-success')).toBe(true);
        });
    });

    describe('assignCurrency()', function () {

        beforeEach(function () {
            affix('form#AssignCurrencyForm[action="localhost-test"]');
            spyOn(Site, "getSingleResource").and.callFake(function (text) {
                return text;
            });

            jasmine.Ajax.install();

            jasmine.Ajax
                .stubRequest('localhost-test')
                .andReturn({
                    'status': 200,
                    'contentType': 'application/json',
                    'responseText': '{ "IsSuccessful": true, "Message": "Hello" }'
                });
        });

        afterEach(function () {
            jasmine.Ajax.uninstall();
        });

        it('ajax url should get from form#AssignCurrencyForm[action]', function () {
            spyOn(EditPermission, 'getSelectedCurrencies').and.callFake(function () {
                return '1,2';
            });

            EditPermission.assignCurrency();
            var request = jasmine.Ajax.requests.mostRecent();
            expect(request.url).toBe('localhost-test');
        });

        it('#resultMessage should contain text resource "SelectAtLeastOneCurrencyMsg" if there is no currencies', function () {
            $("#resultMessage").remove();
            var $resultMessage = affix('#resultMessage');
            EditPermission.assignCurrency();

            expect($resultMessage.text()).toBe('SelectAtLeastOneCurrencyMsg');
        });        

        it('#resultMessage should contain text retrived from the response', function (done) {
            spyOn(EditPermission, 'getSelectedCurrencies').and.callFake(function () {
                return '1,2';
            });
            $("#resultMessage").remove();
            var $resultMessage = affix('#resultMessage');
            EditPermission.assignCurrency();

            setTimeout(function () {
                expect($("#resultMessage").text()).toBe('Hello');
                done();
            }, 200);          
        });
    });
});
