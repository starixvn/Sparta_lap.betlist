describe("ProductListHelper", function () {
    beforeAll(function () {
        affix("form");
        affix('#ProductList')
        spyOn(Site, 'setMultiSelectForDropDownList');
        spyOn($.fn, 'dialog');
    });

    describe("initProductList()", function () {
        it('dialog should be showed if no checkbox is checked', function () {
            affix("#product-container input[type=checkbox]");
            ProductListHelper.initProductList();
            $("form").submit();
            var $dialogContainer = $.fn.dialog.calls.mostRecent().object;

            expect($dialogContainer.attr('title')).toBe('LrfNotification');
        })
    });
});