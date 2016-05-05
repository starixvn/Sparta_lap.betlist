describe("Report", function () {
    describe("ValidProducts()", function () {
        var $checkbox;
        beforeEach(function () {
            $checkbox = affix("input.childCheckbox[type=checkbox]");

            spyOn($.fn, "dialog");
        });

        describe("No checkboxed were checked", function () {
            it("Report.ValidProducts() should return value equal false", function () {
                var expected = false;

                var result = Report.ValidProducts();

                expect(result).toBe(expected);
            });

            it("dialog function of jquery should be called", function () {
                Report.ValidProducts();

                expect($.fn.dialog).toHaveBeenCalled();
            });
        });

        it("Report.ValidProducts() should return value equal true when there are at least checkbox was checked", function () {
            $checkbox.prop("checked", true);
            var expected = true;

            var result = Report.ValidProducts();

            expect(result).toBe(expected);
        });
    });

    describe("setOriginalProduct()", function () {
        it("data-original attribute of checkbox should have correct value", function () {
            var $productList = affix("#ProductList");           
            var $checkbox1 = affix("input.childCheckbox[type=checkbox]");
            $checkbox1.prop("checked", true);
            var $checkbox2 = affix("input.childCheckbox[type=checkbox]");
            $productList.append($checkbox1);
            $productList.append($checkbox2);

            Report.setOriginalProduct();

            expect($checkbox1.attr("data-original")).toBe("true");
            expect($checkbox2.attr("data-original")).toBe("false");
        });
    });
});