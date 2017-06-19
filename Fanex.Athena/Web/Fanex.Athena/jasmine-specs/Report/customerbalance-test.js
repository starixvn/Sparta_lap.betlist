describe("CustBalance", function () {
    
    describe("initialize()", function () {
        it("initEvent() should be call when initialize() is called", function () {
            spyOn(ProductListHelper, 'initProductList');
            spyOn(CustBalance, 'initEvent');

            CustBalance.initialize();

            expect(CustBalance.initEvent).toHaveBeenCalled();
        });

        it("Site.addUserGuide() should be call when initialize() is called with correct params.", function () {
            spyOn(Site, 'addUserGuide');

            CustBalance.initialize();

            expect(Site.addUserGuide).toHaveBeenCalledWith({
                resourceName: "CustomerBalance",
                title: Site.getSingleResource("CustomerBalance")
            });
        });
    });

    describe("initEvent()", function () {
        it("#data-content should be register scroll event.", function () {
            affix('#data-content');
            spyOn($.fn, 'scroll');

            CustBalance.initEvent();

            expect($.fn.scroll.calls.mostRecent().object.selector).toBe('#data-content');
        });
    });

    describe("checkIsValidTimeRequest()", function () {
        it("$.fn.dialog() will not be called if #IsValidTimeRequest does not have value = 'False'.", function () {
            spyOn($.fn, 'dialog');

            CustBalance.checkIsValidTimeRequest();

            expect($.fn.dialog).not.toHaveBeenCalled();
        });

        it("$.fn.dialog will be called on a correct html element.", function () {
            var $input = affix('input#IsValidTimeRequest[value="False"]');
            spyOn($.fn, 'dialog');

            CustBalance.checkIsValidTimeRequest();

            expect($.fn.dialog.calls.mostRecent().object[0].outerHTML).toBe('<div title="LrfNotification">CustomerBalanceDateRangeNotification</div>');
        });
    });
});
